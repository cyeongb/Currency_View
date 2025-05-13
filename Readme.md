# Currency Exchange App

**�ǽð� ȯ�� ������ ǥ���ϴ� WPF ���ø����̼�**

## ������Ʈ �Ұ�

- �� ������Ʈ�� ���� 12���� + �ѱ� ��ȭ�� �ǽð� ȯ�� ������ ǥ���ϴ� Windows ����ũ�� ���ø����̼�. 
- ��ũ �׸� UI�� ����ϸ�, ȯ�� �������� �ð������� Ȯ�� ������. 
- �������� ������ ���� ������ ĳ�̰� �Ϸ翡 �� �� �ڵ� ������Ʈ ����� ������.

## �ֿ� ���

- 13����(�ѱ� ����) ��ȭ�� �ǽð� ȯ�� ���� ǥ��
- ��ũ �׸� UI
- ��ȭ�� ���� ���� ǥ�� (�ڵ�, �̸�, ȯ��)
- ȯ�� ������ ǥ�� (���/�϶� �ð��� ǥ��)
- �������� ������ ���� ������ ĳ��
- �ִ� ��û(�� 1,500ȸ)�� �ʰ����� �ʰ� ����.

## ��� ����

- **���/�����ӿ�ũ**: C#, .NET Framework 4.7.2 (�Ǵ� .NET 6.0+)
- **UI �÷���**: Windows Presentation Foundation (WPF)
- **������ ����**: MVVM (Model-View-ViewModel)
- **������ �ҽ�**: Exchange Rate API (https://www.exchangerate-api.com)

## ������Ʈ ����

CurrencyExchangeApp/
��
������ App.xaml                  # ���ø����̼� ������ �� ���� ���ҽ�
������ App.xaml.cs               # ���ø����̼� �ڵ� �����ε�
��
������ Models/
��   ������ Currency.cs           # ��ȭ �� (�̸�, �ڵ�, ȯ��, ������ ��)
��   ������ CurrencyDatabase.cs   # ���� ������ ����/ĳ�� ó��
��
������ ViewModels/
��   ������ MainViewModel.cs      # ���� ȭ�� ���
��   ������ ViewModelBase.cs      # MVVM �⺻ Ŭ����
��
������ Views/
��   ������ MainWindow.xaml       # ���� UI ������
��   ������ MainWindow.xaml.cs    # ���� ������ �ڵ� �����ε�
��
������ Services/
��   ������ CurrencyApiService.cs # ȯ�� API ��� ����
��   ������ UpdateService.cs      # �ڵ� ������Ʈ ����
��
������ Helpers/
��   ������ ThemeHelper.cs        # ��ũ �׸� ���� ����
��   ������ SettingsHelper.cs     # �� ���� ����
��   ������ RelayCommand.cs       # MVVM ��� ����
��   ������ Converters.cs         # �� ��ȯ�� ����
��
������ Resources/
    ������ Styles.xaml           # ���� ��Ÿ�� (��ũ �׸�)
    ������ Converters.xaml       # �� ��ȯ�� 



## API ����

- ExchangeRate-API
- API �ҽ�: https://www.exchangerate-api.com/
- ���� �÷�: ���� ��û �ѵ� 1,500ȸ
- API Ű ���� �� ���� ��û ���� ��� ����
- 13�� �ֿ� ��ȭ ����(�̱� �޷� ����)
