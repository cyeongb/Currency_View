using System;
using System.Windows;
using System.Windows.Media;

namespace CurrencyExchangeApp.Helpers
{
    public static class ThemeHelper
    {
        // Ultra Violet 테마 색상 (더 진한 보라색)
        public static readonly Color PrimaryColor = Color.FromRgb(95, 75, 139);     // 진한 울트라 바이올렛
        public static readonly Color SecondaryColor = Color.FromRgb(125, 90, 180);  // 중간 톤의 울트라 바이올렛
        public static readonly Color AccentColor = Color.FromRgb(155, 105, 225);    // 밝은 울트라 바이올렛
        public static readonly Color TextColor = Color.FromRgb(255, 255, 255);      // 흰색 텍스트
        public static readonly Color SubTextColor = Color.FromRgb(230, 230, 250);   // 연한 보라색 부텍스트 (Lavender)
        public static readonly Color DarkBackgroundColor = Color.FromRgb(5, 15, 45); // 다크모드 배경색 (진한 남색)
        public static readonly Color KoreanCurrencyColor = Color.FromRgb(180, 125, 255); // 한국 통화용 특별 색상

        // 테마 브러시
        public static SolidColorBrush PrimaryBrush => new SolidColorBrush(PrimaryColor);
        public static SolidColorBrush SecondaryBrush => new SolidColorBrush(SecondaryColor);
        public static SolidColorBrush AccentBrush => new SolidColorBrush(AccentColor);
        public static SolidColorBrush TextBrush => new SolidColorBrush(TextColor);
        public static SolidColorBrush SubTextBrush => new SolidColorBrush(SubTextColor);
        public static SolidColorBrush DarkBackgroundBrush => new SolidColorBrush(DarkBackgroundColor);
        public static SolidColorBrush KoreanCurrencyBrush => new SolidColorBrush(KoreanCurrencyColor);

        // 변동률에 따른 색상 브러시
        public static SolidColorBrush GetChangeRateBrush(double changePercentage)
        {
            if (Math.Abs(changePercentage) < 0.01)
            {
                // 변화 없음 (회색)
                return new SolidColorBrush(Color.FromRgb(180, 180, 180));
            }
            else if (changePercentage > 0)
            {
                // 상승 (녹색)
                return new SolidColorBrush(Color.FromRgb(46, 204, 113));
            }
            else
            {
                // 하락 (빨간색)
                return new SolidColorBrush(Color.FromRgb(231, 76, 60));
            }
        }

        // 테마 리소스 설정
        public static void ApplyTheme(ResourceDictionary resources)
        {
            // 주요 브러시 설정
            resources["PrimaryBackgroundBrush"] = PrimaryBrush;
            resources["SecondaryBackgroundBrush"] = SecondaryBrush;
            resources["AccentBrush"] = AccentBrush;
            resources["TextBrush"] = TextBrush;
            resources["SubTextBrush"] = SubTextBrush;
            resources["DarkBackgroundBrush"] = DarkBackgroundBrush;
            resources["KoreanCurrencyBrush"] = KoreanCurrencyBrush;

            // 기타 테마 설정들...
        }
    }
}