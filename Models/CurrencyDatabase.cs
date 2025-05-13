using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace CurrencyExchangeApp.Models
{
    public class CurrencyDatabase
    {
        private readonly string _dataFolderPath;
        private readonly string _currenciesFilePath;
        private readonly string _lastUpdateFilePath;

        public CurrencyDatabase(string dataFolderPath)
        {
            _dataFolderPath = dataFolderPath;
            _currenciesFilePath = Path.Combine(_dataFolderPath, "currencies.json");
            _lastUpdateFilePath = Path.Combine(_dataFolderPath, "last_update.txt");
            // 샘플 데이터 생성 (API가 완전히 실패할 경우 사용)
        }
public List<Currency> GetSampleCurrencyData()
        {
            var now = DateTime.Now;
            return new List<Currency>
    {
        new Currency { Code = "KRW", Name = "한국 원화", Rate = 1358.42, PreviousRate = 1355.20, LastUpdate = now },
        new Currency { Code = "USD", Name = "미국 달러", Rate = 1.0, PreviousRate = 1.0, LastUpdate = now },
        new Currency { Code = "EUR", Name = "유로", Rate = 0.92, PreviousRate = 0.91, LastUpdate = now },
        new Currency { Code = "JPY", Name = "일본 엔", Rate = 151.42, PreviousRate = 150.5, LastUpdate = now },
        new Currency { Code = "GBP", Name = "영국 파운드", Rate = 0.79, PreviousRate = 0.78, LastUpdate = now },
        new Currency { Code = "AUD", Name = "호주 달러", Rate = 1.52, PreviousRate = 1.53, LastUpdate = now },
        new Currency { Code = "CAD", Name = "캐나다 달러", Rate = 1.37, PreviousRate = 1.36, LastUpdate = now },
        new Currency { Code = "CHF", Name = "스위스 프랑", Rate = 0.90, PreviousRate = 0.89, LastUpdate = now },
        new Currency { Code = "CNY", Name = "중국 위안", Rate = 7.24, PreviousRate = 7.23, LastUpdate = now },
        new Currency { Code = "HKD", Name = "홍콩 달러", Rate = 7.82, PreviousRate = 7.81, LastUpdate = now },
        new Currency { Code = "NZD", Name = "뉴질랜드 달러", Rate = 1.63, PreviousRate = 1.62, LastUpdate = now },
        new Currency { Code = "SGD", Name = "싱가포르 달러", Rate = 1.34, PreviousRate = 1.33, LastUpdate = now },
        new Currency { Code = "THB", Name = "태국 바트", Rate = 36.12, PreviousRate = 35.98, LastUpdate = now }
        
    };
        }

        // 환율 데이터 저장
        public async Task SaveCurrenciesAsync(List<Currency> currencies)
        {
            try
            {
                var json = JsonSerializer.Serialize(currencies, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                await File.WriteAllTextAsync(_currenciesFilePath, json);

                // 업데이트 시간 저장
                await File.WriteAllTextAsync(_lastUpdateFilePath, DateTime.Now.ToString("o"));
            }
            catch (Exception ex)
            {
                // 실제 앱에서는 로깅 처리
                Console.WriteLine($"Error saving currencies: {ex.Message}");
            }
        }

        // 캐시된 환율 데이터 로드
        public async Task<List<Currency>> LoadCachedCurrenciesAsync()
        {
            try
            {
                if (File.Exists(_currenciesFilePath))
                {
                    var json = await File.ReadAllTextAsync(_currenciesFilePath);
                    return JsonSerializer.Deserialize<List<Currency>>(json);
                }
            }
            catch (Exception ex)
            {
                // 실제 앱에서는 로깅 처리
                Console.WriteLine($"Error loading cached currencies: {ex.Message}");
            }

            return new List<Currency>();
        }

        // 마지막 업데이트 시간 로드
        public async Task<DateTime?> GetLastUpdateTimeAsync()
        {
            try
            {
                if (File.Exists(_lastUpdateFilePath))
                {
                    var timeStr = await File.ReadAllTextAsync(_lastUpdateFilePath);
                    if (DateTime.TryParse(timeStr, out var updateTime))
                    {
                        return updateTime;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting last update time: {ex.Message}");
            }

            return null;
        }

        // 오늘 업데이트 여부 확인
        public async Task<bool> IsUpdatedTodayAsync()
        {
            var lastUpdate = await GetLastUpdateTimeAsync();
            if (lastUpdate.HasValue)
            {
                return lastUpdate.Value.Date == DateTime.Today;
            }
            return false;
        }
    }
}