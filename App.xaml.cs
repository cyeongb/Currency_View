using System;
using System.IO;
using System.Windows;
using CurrencyExchangeApp.Models;
using CurrencyExchangeApp.Services;
using CurrencyExchangeApp.ViewModels;

namespace CurrencyExchangeApp
{
    public partial class App : Application
    {
        private UpdateService _updateService;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 앱 데이터 폴더 생성
            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CurrencyExchangeApp");

            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }

            // 서비스 및 모델 초기화
            var apiService = new CurrencyApiService();
            var database = new CurrencyDatabase(appDataPath);

            // 메인 뷰모델 생성 및 윈도우에 설정
            var mainViewModel = new MainViewModel(apiService, database);

            // 메인 윈도우 생성 및 뷰모델 설정
            var mainWindow = new Views.MainWindow
            {
                DataContext = mainViewModel
            };

            // 자동 업데이트 서비스 설정
            _updateService = new UpdateService(apiService, database);
            _updateService.StartDailyUpdates();

            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // 앱 종료 시 업데이트 서비스 정리
            _updateService?.Dispose();

            base.OnExit(e);
        }
    }
}