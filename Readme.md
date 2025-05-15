# Currency Exchange App

**실시간 환율 정보를 표시하는 WPF 애플리케이션**
![image](https://github.com/user-attachments/assets/49bf4f82-db21-4d36-8329-38a465e6e3fb)

## 프로젝트 소개

- 이 프로젝트는 상위 12개국 + 한국 통화의 실시간 환율 정보를 표시하는 Windows 데스크톱 애플리케이션. 
- 다크 테마 UI를 사용하며, 환율 변동률을 시각적으로 확인 가능함. 
- 오프라인 지원을 위한 데이터 캐싱과 하루에 한 번 자동 업데이트 기능을 제공함.

## 주요 기능

- 13개국(한국 포함) 통화의 실시간 환율 정보 표시
- 다크 테마 UI
- 통화별 정보 간단 표시 (코드, 이름, 환율)
- 환율 변동률 표시 (상승/하락 시각적 표현)
- 오프라인 지원을 위한 데이터 캐싱
- 최대 요청(일 1,500회)를 초과하지 않게 제한.

## 기술 스택

- 언어/프레임워크: C#, .NET Framework 8.0
- UI 플랫폼: Windows Presentation Foundation (WPF)
- 디자인 패턴: MVVM (Model-View-ViewModel)
- 데이터 소스: Exchange Rate API (https://www.exchangerate-api.com)

## 프로젝트 구조
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
    ├── Styles.xaml           # 공통 스타일 (디즈니 플러스 스타일 다크 테마)
    └── Converters.xaml       # 값 변환기 (변동률 → 색상 등)
```


## API 정보

- ExchangeRate-API
- API 소스: https://www.exchangerate-api.com/
- 무료 플랜: 일일 요청 한도 1,500회
- API 키 관리 및 일일 요청 제한 기능 포함
- 13개 주요 통화 지원(미국 달러 기준)
