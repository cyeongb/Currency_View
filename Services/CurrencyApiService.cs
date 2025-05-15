using CurrencyExchangeApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CurrencyExchangeApp.Services
{
    public class CurrencyApiService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "e7534ed6b58f5c30eadc375c";
        private const string BaseUrl = "https://v6.exchangerate-api.com/v6";

        // 기준 통화 (미국 달러)
        private const string BaseCurrency = "USD";

        // 상위 통화 코드 (주요 12개국 + 한국)
        private readonly string[] _topCurrencyCodes = new[]
        {
            "EUR", "JPY", "GBP", "AUD", "CAD",
            "CHF", "CNY", "HKD", "NZD", "SGD", "THB", "KRW"
        };

        // 통화 이름 매핑
        private readonly Dictionary<string, string> _currencyNames = new Dictionary<string, string>
        {
            {"KRW", "한국 원화"},
            {"USD", "미국 달러"},
            {"EUR", "유로"},
            {"JPY", "일본 엔"},
            {"GBP", "영국 파운드"},
            {"AUD", "호주 달러"},
            {"CAD", "캐나다 달러"},
            {"CHF", "스위스 프랑"},
            {"CNY", "중국 위안"},
            {"HKD", "홍콩 달러"},
            {"NZD", "뉴질랜드 달러"},
            {"SGD", "싱가포르 달러"},
            {"THB", "태국 바트"}
        };

        // API 요청 제한 관련 속성
        private static int _dailyRequestCount = 0;
        private static DateTime _lastRequestReset = DateTime.MinValue;
        private const int MaxDailyRequests = 1400; // 원래 1500 인데 안전빵으로..

        public CurrencyApiService()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(15)
            };

            // API는 하루에 한 번만 업데이트됨을 명시
            LastApiUpdate = DateTime.MinValue;

            // 요청 카운터 초기화
            ResetRequestCounterIfNeeded();
        }

        // API 마지막 업데이트 시간 (모든 인스턴스에서 공유)
        public static DateTime LastApiUpdate { get; private set; }

        // 요청 카운터 관리
        private void ResetRequestCounterIfNeeded()
        {
            var today = DateTime.Today;
            if (_lastRequestReset.Date != today)
            {
                _lastRequestReset = today;
                _dailyRequestCount = 0;
                Console.WriteLine("API 요청 카운터 리셋됨");
            }
        }

        // API 요청 가능 여부 확인
        private bool CanMakeRequest()
        {
            ResetRequestCounterIfNeeded();
            return _dailyRequestCount < MaxDailyRequests;
        }

        // 요청 카운터 증가
        private void IncrementRequestCounter()
        {
            _dailyRequestCount++;
            Console.WriteLine($"API 요청 카운트: {_dailyRequestCount}/{MaxDailyRequests}");
        }

        // 상위 통화 정보 조회
        public async Task<List<Currency>> GetTopCurrenciesAsync(int count)
        {
            try
            {
                // API는 하루에 한 번만 업데이트되므로, 이미 오늘 데이터를 가져왔다면 다시 호출하지 않음
                if (LastApiUpdate.Date == DateTime.Today)
                {
                    throw new Exception("API는 하루에 한 번만 업데이트됩니다. 오늘은 이미 최신 데이터를 가져왔습니다.");
                }

                // 요청 제한 확인
                if (!CanMakeRequest())
                {
                    throw new Exception($"API 일일 요청 한도({MaxDailyRequests}회)에 도달했습니다. 내일 다시 시도해 주세요.");
                }

                // API URL 구성 - ExchangeRate API 형식으로 수정
                var url = $"{BaseUrl}/{ApiKey}/latest/{BaseCurrency}";

                Console.WriteLine($"API URL: {url}"); // 디버깅용 URL 출력

                // 요청 카운터 증가
                IncrementRequestCounter();

                // API 호출
                var response = await _httpClient.GetStringAsync(url);

                Console.WriteLine($"API 응답 길이: {response.Length}"); // 디버깅용 응답 길이 출력

                // JSON 파싱
                using var jsonDoc = JsonDocument.Parse(response);
                var rootElement = jsonDoc.RootElement;

                // API 응답 형식 확인
                var result = rootElement.GetProperty("result").GetString();
                if (result != "success")
                {
                    var errorType = rootElement.TryGetProperty("error-type", out var errorElement)
                        ? errorElement.GetString()
                        : "unknown error";
                    throw new Exception($"API 응답 오류: {errorType}");
                }

                // 환율 데이터 가져오기
                var conversionRates = rootElement.GetProperty("conversion_rates");

                var currencies = new List<Currency>();

               // 기준 통화(USD) 추가
                currencies.Add(new Currency
                {
                    Code = "USD",
                    Name = _currencyNames["USD"],
                    Rate = 1.0, // 기준 통화는 항상 1
                    PreviousRate = 1.0,
                    LastUpdate = DateTime.Now
                });

                //currencies.Add(new Currency
                //{
                //    Code = "KRW",
                //    Name = _currencyNames["KRW"],
                //    Rate = 1416.30, // 기준 통화는 항상 1
                //    PreviousRate = 1416.30,
                //    LastUpdate = DateTime.Now
                //});

                // 통화 추출
                foreach (var code in _topCurrencyCodes.Take(Math.Min(count, _topCurrencyCodes.Length)))
                {
                    if (conversionRates.TryGetProperty(code, out var rateElement))
                    {
                        var rate = rateElement.GetDouble();

                        currencies.Add(new Currency
                        {
                            Code = code,
                            Name = _currencyNames.ContainsKey(code) ? _currencyNames[code] : code,
                            Rate = rate,
                            PreviousRate = rate, // 초기값은 동일하게 설정
                            LastUpdate = DateTime.Now
                        });
                        Debug.WriteLine("이 메시지는 출력 창에 표시됩니다!", currencies);
                    }
                    else
                    {
                        Console.WriteLine($"통화 코드 {code}를 API 응답에서 찾을 수 없습니다.");
                    }
                }

                if (currencies.Count <= 1) // USD만 있으면 실패로 간주
                {
                    throw new Exception("API 응답에서 통화 정보를 찾을 수 없습니다.");
                }

                // 마지막 API 업데이트 시간 갱신
                LastApiUpdate = DateTime.Now;

                return currencies;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP 요청 오류: {ex.Message}");
                throw new Exception($"환율 데이터 조회 실패 - 네트워크 오류: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON 파싱 오류: {ex.Message}");
                throw new Exception($"환율 데이터 조회 실패 - 데이터 형식 오류: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"기타 오류: {ex.Message}");
                throw new Exception($"환율 데이터 조회 실패: {ex.Message}", ex);
            }
        }

        // 특정 통화 간 환율 조회
        public async Task<double> GetExchangeRateAsync(string from, string to)
        {
            try
            {
                // 요청 제한 확인
                if (!CanMakeRequest())
                {
                    throw new Exception($"API 일일 요청 한도({MaxDailyRequests}회)에 도달했습니다. 내일 다시 시도해 주세요.");
                }

                // API URL 구성 (쌍 전환 API 사용)
                var url = $"{BaseUrl}/{ApiKey}/pair/{from}/{to}";

                // 요청 카운터 증가
                IncrementRequestCounter();

                var response = await _httpClient.GetStringAsync(url);

                using var jsonDoc = JsonDocument.Parse(response);
                var rootElement = jsonDoc.RootElement;

                // API 응답 형식 확인
                var result = rootElement.GetProperty("result").GetString();
                if (result != "success")
                {
                    var errorType = rootElement.TryGetProperty("error-type", out var errorElement)
                        ? errorElement.GetString()
                        : "unknown error";
                    throw new Exception($"API 응답 오류: {errorType}");
                }

                // 환율 가져오기
                return rootElement.GetProperty("conversion_rate").GetDouble();
            }
            catch (Exception ex)
            {
                throw new Exception($"환율 조회 실패: {from} -> {to}: {ex.Message}", ex);
            }
        }
    }
}