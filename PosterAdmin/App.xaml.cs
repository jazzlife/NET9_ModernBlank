using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace PosterAdmin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 애플리케이션 시작 시 호출
        /// </summary>
        /// <param name="e">시작 이벤트 인수</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                // 디버깅 정보 로그
                LogDebugInfo();

                // 전역 예외 처리기 등록
                this.DispatcherUnhandledException += OnDispatcherUnhandledException;
                AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                // 시작 중 예외 처리
                var errorMessage = $"애플리케이션 시작 중 오류가 발생했습니다:\n\n{ex.Message}\n\n상세 정보:\n{ex}";
                MessageBox.Show(errorMessage, "시작 오류", MessageBoxButton.OK, MessageBoxImage.Error);
                
                // 로그 파일에 기록
                LogError("Startup Error", ex);
                
                // 애플리케이션 종료
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// UI 스레드에서 처리되지 않은 예외 처리
        /// </summary>
        /// <param name="sender">이벤트 발신자</param>
        /// <param name="e">예외 이벤트 인수</param>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var errorMessage = $"예상치 못한 오류가 발생했습니다:\n\n{e.Exception.Message}";
            
            MessageBox.Show(errorMessage, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            
            // 로그 파일에 기록
            LogError("Dispatcher Exception", e.Exception);
            
            // 애플리케이션이 계속 실행되도록 설정
            e.Handled = true;
        }

        /// <summary>
        /// 백그라운드 스레드에서 처리되지 않은 예외 처리
        /// </summary>
        /// <param name="sender">이벤트 발신자</param>
        /// <param name="e">예외 이벤트 인수</param>
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            var errorMessage = $"심각한 오류가 발생했습니다:\n\n{exception?.Message ?? "알 수 없는 오류"}";
            
            MessageBox.Show(errorMessage, "심각한 오류", MessageBoxButton.OK, MessageBoxImage.Error);
            
            // 로그 파일에 기록
            LogError("Unhandled Exception", exception);
        }

        /// <summary>
        /// 디버깅 정보 로그
        /// </summary>
        private void LogDebugInfo()
        {
            try
            {
                var debugInfo = $@"
PosterAdmin 시작 - {DateTime.Now:yyyy-MM-dd HH:mm:ss}
.NET Version: {Environment.Version}
OS Version: {Environment.OSVersion}
Machine Name: {Environment.MachineName}
User Name: {Environment.UserName}
Working Directory: {Environment.CurrentDirectory}
Application Data: {Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}
";
                
                LogToFile("Debug Info", debugInfo);
            }
            catch
            {
                // 로깅 실패 시 무시
            }
        }

        /// <summary>
        /// 오류를 로그 파일에 기록
        /// </summary>
        /// <param name="title">오류 제목</param>
        /// <param name="exception">예외 객체</param>
        private void LogError(string title, Exception? exception)
        {
            try
            {
                var errorInfo = $@"
{title} - {DateTime.Now:yyyy-MM-dd HH:mm:ss}
Message: {exception?.Message ?? "Unknown"}
Stack Trace: {exception?.StackTrace ?? "No stack trace"}
Inner Exception: {exception?.InnerException?.Message ?? "None"}
";
                
                LogToFile(title, errorInfo);
            }
            catch
            {
                // 로깅 실패 시 무시
            }
        }

        /// <summary>
        /// 로그 파일에 정보 기록
        /// </summary>
        /// <param name="title">로그 제목</param>
        /// <param name="content">로그 내용</param>
        private void LogToFile(string title, string content)
        {
            try
            {
                var logDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PosterAdmin");
                Directory.CreateDirectory(logDir);
                
                var logFile = Path.Combine(logDir, "app.log");
                var logEntry = $"\n========== {title} ==========\n{content}\n";
                
                File.AppendAllText(logFile, logEntry);
            }
            catch
            {
                // 로깅 실패 시 무시
            }
        }
    }
}
