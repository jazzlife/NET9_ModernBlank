# PosterAdmin - .NET 9.0 WPF Material Design 보일러플레이트

**MaterialDesignThemes 5.2.1** 기반의 .NET 9.0 WPF 애플리케이션 보일러플레이트입니다.

## 🎨 Material Design 3.0 스타일

이 프로젝트는 **MaterialDesignThemes 5.2.1**의 최신 공식 문서에 따라 구현되었습니다:

- **BundledTheme**: 최신 테마 설정 방식 사용
- **MaterialDesign3.Defaults.xaml**: Material Design 3 스타일 적용
- **MaterialDesignWindow**: 올바른 Window 스타일 적용
- **Material Design 컨트롤**: Card, Chip, PackIcon 등 최신 컨트롤 사용

## ✨ 주요 기능

- ✅ **.NET 9.0** 최신 프레임워크
- ✅ **Material Design 3.0** 테마 적용
- ✅ **MVVM 아키텍처** (CommunityToolkit.Mvvm)
- ✅ **LiteDB** 로컬 데이터베이스
- ✅ **완전한 CRUD** 기능
- ✅ **Single-file 배포** 지원 (131MB)
- ✅ **DPI 인식** 및 고해상도 모니터 지원

## 🏗️ 프로젝트 구조

```
PosterAdmin/
├── Models/              # 데이터 모델
│   └── Item.cs         # LiteDB 엔티티
├── ViewModels/         # MVVM ViewModel
│   └── MainViewModel.cs
├── Views/              # 사용자 컨트롤
│   └── AboutView.xaml
├── Services/           # 비즈니스 로직
│   └── LiteDbController.cs
└── Resources/          # 리소스 파일
```

## 🎯 Material Design 컨트롤 사용 예시

### Card 컴포넌트
```xml
<materialDesign:Card Padding="20" materialDesign:ElevationAssist.Elevation="Dp4">
    <!-- 카드 내용 -->
</materialDesign:Card>
```

### Material Design TextBox
```xml
<TextBox materialDesign:HintAssist.Hint="힌트 텍스트"
         Style="{StaticResource MaterialDesignFilledTextBox}" />
```

### Material Design Button
```xml
<Button Content="버튼 텍스트"
        Style="{StaticResource MaterialDesignRaisedButton}" />
```

### PackIcon 사용
```xml
<materialDesign:PackIcon Kind="Loading" 
                         Width="24" Height="24" />
```

## 🚀 빠른 시작

### 필수 조건
- .NET 9.0 SDK
- Visual Studio 2022 또는 JetBrains Rider

### 프로젝트 실행
```bash
git clone <repository-url>
cd PosterAdmin
dotnet restore
dotnet run --project PosterAdmin
```

### Single-file 배포
```bash
dotnet publish -c Release --self-contained true -r win-x64 -p:PublishSingleFile=true
```

## 📦 NuGet 패키지

| 패키지 | 버전 | 용도 |
|--------|------|------|
| MaterialDesignThemes | 5.2.1 | Material Design UI 컨트롤 |
| MaterialDesignColors | 5.2.1 | Material Design 색상 팔레트 |
| LiteDB | 5.0.21 | 로컬 NoSQL 데이터베이스 |
| CommunityToolkit.Mvvm | 8.2.2 | MVVM 패턴 구현 |

## 💡 핵심 설정

### App.xaml 설정
```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <!-- Material Design 5.2.1 테마 설정 -->
            <materialDesign:BundledTheme 
                BaseTheme="Light" 
                PrimaryColor="DeepPurple" 
                SecondaryColor="Lime" />
            
            <!-- Material Design 3 기본 스타일 -->
            <ResourceDictionary Source="pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesign3.Defaults.xaml" />
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

### MainWindow 설정
```xml
<Window Style="{StaticResource MaterialDesignWindow}"
        xmlns:materialDesign="http://materialdesigninxaml.net/winfx/xaml/themes">
```

## 🎨 테마 커스터마이징

### 색상 변경
```xml
<materialDesign:BundledTheme 
    BaseTheme="Dark"           <!-- Light/Dark -->
    PrimaryColor="Blue"        <!-- 주 색상 -->
    SecondaryColor="Orange" /> <!-- 보조 색상 -->
```

### 사용 가능한 색상
- Primary: Red, Pink, Purple, DeepPurple, Indigo, Blue, LightBlue, Cyan, Teal, Green, LightGreen, Lime, Yellow, Amber, Orange, DeepOrange, Brown, Grey, BlueGrey
- Secondary: Red, Pink, Purple, DeepPurple, Indigo, Blue, LightBlue, Cyan, Teal, Green, LightGreen, Lime, Yellow, Amber, Orange, DeepOrange

## 🔧 해결된 문제들

### 1. Material Design 호환성 문제
- **문제**: MahApps 호환성 리소스로 인한 Windows 환경 실행 오류
- **해결**: BundledTheme + MaterialDesign3.Defaults.xaml로 단순화

### 2. PackIcon Spin 속성 오류
- **문제**: MaterialDesignThemes 5.2.1에서 Spin 속성 지원 중단
- **해결**: Spin 속성 제거, 기본 PackIcon 사용

### 3. 패키지 버전 충돌
- **문제**: MaterialDesignThemes 5.2.1과 MaterialDesignColors 3.1.0 충돌
- **해결**: 모든 Material Design 패키지를 5.2.1로 통일

## 📊 성능 정보

- **빌드 시간**: ~2초
- **배포 파일 크기**: 131MB (single-file)
- **메모리 사용량**: ~50MB (기본 상태)
- **시작 시간**: ~1초

## 🔗 참고 자료

- [MaterialDesignThemes 공식 문서](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit/wiki)
- [Material Design 3 가이드라인](https://m3.material.io/)
- [.NET 9.0 문서](https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-9)
- [LiteDB 문서](https://www.litedb.org/)

## 📝 버전 히스토리

### v1.2 (2025-01-28)
- ✅ **MaterialDesignThemes 5.2.1** 업데이트
- ✅ **Material Design 3** 스타일 적용
- ✅ **BundledTheme** 방식으로 테마 설정 개선
- ✅ **MaterialDesignWindow** 스타일 적용
- ✅ 모든 UI 컨트롤을 Material Design 컨트롤로 교체
- ✅ PackIcon, Card, Chip 등 최신 Material Design 컨트롤 사용
- ✅ 패키지 버전 충돌 해결
- ✅ 안정적인 빌드 및 배포 확인

### v1.1 (2025-01-28)
- ✅ Windows 환경 실행 문제 해결
- ✅ Material Design 호환성 개선
- ✅ 향상된 예외 처리 및 로깅
- ✅ nullable 경고 수정

### v1.0 (2025-01-28)
- ✅ 초기 .NET 9.0 WPF 프로젝트 생성
- ✅ Material Design 테마 적용
- ✅ MVVM 아키텍처 구현
- ✅ LiteDB 통합
- ✅ CRUD 기능 구현
- ✅ Single-file 배포 설정

## 📄 라이선스

이 프로젝트는 MIT 라이선스 하에 배포됩니다.

## 🤝 기여

1. Fork 프로젝트
2. Feature 브랜치 생성
3. 변경사항 커밋
4. 브랜치에 Push
5. Pull Request 생성

---

**✅ 완전히 안정화된 .NET 9.0 WPF 보일러플레이트입니다!** 🎉

> **주의**: 이 버전은 Material Design 호환성 문제를 해결하여 Windows 환경에서 즉시 실행 가능하도록 최적화되었습니다. 