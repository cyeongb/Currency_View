using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CurrencyExchangeApp.Helpers;
using CurrencyExchangeApp.Models;
using CurrencyExchangeApp.Services;

namespace CurrencyExchangeApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly CurrencyApiService _apiService;
        private readonly CurrencyDatabase _database;
        private ObservableCollection<Currency> _currencies;
        private bool _isLoading;
        private string _statusMessage;
        private DateTime _lastUpdated;

        // 속성
        public ObservableCollection<Currency> Currencies
        {
            get => _currencies;
            set => SetProperty(ref _currencies, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public DateTime LastUpdated
        {
            get => _lastUpdated;
            set => SetProperty(ref _lastUpdated, value);
        }

        // 명령
        public ICommand RefreshCommand { get; }

        // 생성자
        public MainViewModel(CurrencyApiService apiService, CurrencyDatabase database)
        {
            _apiService = apiService;
            _database = database;
            _currencies = new ObservableCollection<Currency>();

            // RelayCommand 구현
            RefreshCommand = new RelayCommand(async () => await LoadCurrenciesAsync(), () => !IsLoading);

            // 초기 로드
            _ = InitializeAsync();
        }

        // 초기화
        private async Task InitializeAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "초기화 중...";

                // 캐시된 데이터 먼저 로드
                var cachedCurrencies = await _database.LoadCachedCurrenciesAsync();
                if (cachedCurrencies != null && cachedCurrencies.Any())
                {
                    UpdateCurrenciesList(cachedCurrencies);
                    var lastUpdateTime = await _database.GetLastUpdateTimeAsync();
                    if (lastUpdateTime.HasValue)
                    {
                        LastUpdated = lastUpdateTime.Value;
                    }
                }

                // 오늘 이미 업데이트된 경우 API 호출 스킵
                if (await _database.IsUpdatedTodayAsync())
                {
                    StatusMessage = "마지막 업데이트: " + LastUpdated.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    // 최신 데이터 로드
                    await LoadCurrenciesAsync();
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"초기화 오류: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        // 통화 데이터 로드
        public async Task LoadCurrenciesAsync()
        {
            if (IsLoading) return;

            try
            {
                IsLoading = true;
                StatusMessage = "최신 환율 데이터 로딩 중...";

                // 기존 데이터 저장 (변동률 계산용)
                var previousCurrencies = _currencies.ToList();

                // API에서 최신 데이터 가져오기
                var latestCurrencies = await _apiService.GetTopCurrenciesAsync(12);

                // 이전 환율 데이터 업데이트 및 변동률 계산
                foreach (var currency in latestCurrencies)
                {
                    var previous = previousCurrencies.FirstOrDefault(c => c.Code == currency.Code);
                    if (previous != null)
                    {
                        currency.PreviousRate = previous.Rate;
                    }
                    else
                    {
                        currency.PreviousRate = currency.Rate;
                    }

                    currency.CalculateChangePercentage();
                }

                // UI 업데이트
                UpdateCurrenciesList(latestCurrencies);

                // 캐싱
                await _database.SaveCurrenciesAsync(latestCurrencies);

                LastUpdated = DateTime.Now;
                StatusMessage = "마지막 업데이트: " + LastUpdated.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex)
            {
                StatusMessage = $"데이터 로드 오류: {ex.Message}";

                // 에러 시 캐시된 데이터 확인
                if (!_currencies.Any())
                {
                    var cached = await _database.LoadCachedCurrenciesAsync();
                    if (cached != null && cached.Any())
                    {
                        UpdateCurrenciesList(cached);
                        StatusMessage += " (캐시된 데이터 표시 중)";
                    }
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        // 통화 목록 업데이트
        private void UpdateCurrenciesList(System.Collections.Generic.List<Currency> currencies)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Currencies.Clear();
                foreach (var currency in currencies)
                {
                    Currencies.Add(currency);
                }
            });
        }
    }
}