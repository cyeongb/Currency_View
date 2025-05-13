using System;
using System.Configuration;
using System.IO;
using System.Windows;

namespace CurrencyExchangeApp.Helpers
{
    public static class SettingsHelper
    {
        private const string SettingsFileName = "settings.json";
        private static readonly string SettingsFilePath;

        // 정적 생성자
        static SettingsHelper()
        {
            // 설정 파일 경로 초기화
            SettingsFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CurrencyExchangeApp",
                SettingsFileName
            );
        }

        // 설정값 저장
        public static void SaveSetting(string key, string value)
        {
            try
            {
                var settings = ConfigurationManager.AppSettings;
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                if (settings[key] != null)
                {
                    config.AppSettings.Settings.Remove(key);
                }

                config.AppSettings.Settings.Add(key, value);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"설정 저장 오류: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 설정값 로드
        public static string GetSetting(string key, string defaultValue = "")
        {
            try
            {
                var value = ConfigurationManager.AppSettings[key];
                return string.IsNullOrEmpty(value) ? defaultValue : value;
            }
            catch
            {
                return defaultValue;
            }
        }

        // 정수 설정값 로드
        public static int GetSettingInt(string key, int defaultValue = 0)
        {
            if (int.TryParse(GetSetting(key), out int result))
            {
                return result;
            }

            return defaultValue;
        }

        // 불리언 설정값 로드
        public static bool GetSettingBool(string key, bool defaultValue = false)
        {
            if (bool.TryParse(GetSetting(key), out bool result))
            {
                return result;
            }

            return defaultValue;
        }
    }
}