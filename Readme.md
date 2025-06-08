# 💸💱Currency Exchange App

**WPF application that displays real-time exchange rate information**

(실시간 환율 정보를 표시하는 WPF 애플리케이션)

https://www.notion.so/cyeongb/6-C-NET-Project-1f4aa4605a0b802583f8cb858cc974a5?pvs=4

----


![image](https://github.com/user-attachments/assets/49bf4f82-db21-4d36-8329-38a465e6e3fb)



## 🔥 Introduce

- This project is a Windows desktop application that displays real-time exchange rate information for the top 12 countries     plus South Korean currency. 
-It uses a dark theme UI, enabling visual tracking of exchange rate fluctuations.
- The application also provides data caching for offline support and an automatic daily update function.

## 🧩 Main Function

- Real-time exchange rate display for 13 currencies (including South Korea)
- Dark theme UI
- Simple display of currency details (code, name, exchange rate)
- Display of exchange rate fluctuation (visual representation of rise/fall)
- Data caching for offline support
- Restrict requests to a maximum of 1,500 per day.

## ✨ Skill stack

- Language/Framework : C#, .NET Framework 8.0
- UI platform: Windows Presentation Foundation (WPF)
- Pattern: MVVM (Model-View-ViewModel)
- Data source: Exchange Rate API (https://www.exchangerate-api.com)

## 📻 API info
- ExchangeRate-API
- API 소스: https://www.exchangerate-api.com/
- 무료 플랜: 일일 요청 한도 1,500회
- API 키 관리 및 일일 요청 제한 기능 포함
- 200개국 161가지 주요 통화 지원(미국 달러를 기준)


## 🖼️ Project struct
```
CurrencyExchangeApp/
│
├── App.xaml                  # 애플리케이션 진입점 및 전역 리소스
├── App.xaml.cs               # 애플리케이션 코드 비하인드
│
├── Models/
│   ├── Currency.cs           # 통화 모델 (이름, 코드, 환율, 변동률 등)
│   └── CurrencyDatabase.cs   # 로컬 데이터 저장/캐싱 처리
│
├── ViewModels/
│   ├── MainViewModel.cs      # 메인 화면 뷰모델
│   └── ViewModelBase.cs      # MVVM 기본 클래스 (INotifyPropertyChanged 구현)
│
├── Views/
│   ├── MainWindow.xaml       # 메인 UI 윈도우
│   └── MainWindow.xaml.cs    # 메인 윈도우 코드 비하인드
│
├── Services/
│   ├── CurrencyApiService.cs # 환율 API 통신 서비스
│   └── UpdateService.cs      # 자동 업데이트 서비스
│
├── Helpers/
│   ├── ThemeHelper.cs        # 다크 테마 관련 헬퍼
│   ├── SettingsHelper.cs     # 앱 설정 관리
│   ├── RelayCommand.cs       # MVVM 명령 구현
│   └── Converters.cs         # 값 변환기 구현
│
└── Resources/
    ├── Styles.xaml           # 공통 스타일 (다크 테마)
    └── Converters.xaml       # 값 변환기 (변동률 → 색상 등)
```

