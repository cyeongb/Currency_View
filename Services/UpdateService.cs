using System;
using System.Threading;
using System.Threading.Tasks;
using CurrencyExchangeApp.Models;

namespace CurrencyExchangeApp.Services
{
    public class UpdateService : IDisposable
    {
        private readonly CurrencyApiService _apiService;
        private readonly CurrencyDatabase _database;
        private Timer _dailyUpdateTimer;
        private bool _disposed = false;

        public UpdateService(CurrencyApiService apiService, CurrencyDatabase database)
        {
            _apiService = apiService;
            _database = database;
        }

        // 하루에 한 번 자동 업데이트 시작
        public void StartDailyUpdates()
        {
            // 타이머 생성 (다음 날 오전 9시에 처음 실행)
            var now = DateTime.Now;
            var nextRun = new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(1);

            if (now.Hour < 9)
            {
                // 현재 시간이 오전 9시 이전이면 오늘 오전 9시에 실행
                nextRun = new DateTime(now.Year, now.Month, now.Day, 9, 0, 0);
            }

            var firstInterval = nextRun - now;
            var dailyInterval = TimeSpan.FromHours(24);

            // 타이머 시작
            _dailyUpdateTimer = new Timer(
                async _ => await UpdateCurrenciesAsync(),
                null,
                firstInterval,
                dailyInterval
            );
        }

        // 통화 데이터 업데이트
        private async Task UpdateCurrenciesAsync()
        {
            try
            {
                // 이미 오늘 업데이트되었는지 확인
                if (await _database.IsUpdatedTodayAsync())
                {
                    return;
                }

                // 최신 환율 데이터 조회
                var currencies = await _apiService.GetTopCurrenciesAsync(12);

                // 저장
                await _database.SaveCurrenciesAsync(currencies);
            }
            catch (Exception ex)
            {
                // 실제 앱에서는 로그 기록
                Console.WriteLine($"자동 업데이트 오류: {ex.Message}");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dailyUpdateTimer?.Dispose();
                }

                _disposed = true;
            }
        }
    }
}