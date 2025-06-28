using System.Windows;
using PosterAdmin.ViewModels;
using PosterAdmin.Models;
using System.Windows.Controls;

namespace PosterAdmin;

/// <summary>
/// MainWindow.xaml에 대한 상호 작용 로직
/// MVVM 패턴을 따라 최소한의 코드비하인드만 포함합니다.
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        // 윈도우 종료 시 ViewModel 리소스 해제
        this.Closing += OnWindowClosing;
    }

    /// <summary>
    /// 윈도우 종료 시 ViewModel 리소스 해제
    /// </summary>
    /// <param name="sender">이벤트 발신자</param>
    /// <param name="e">종료 이벤트 인수</param>
    private void OnWindowClosing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        if (DataContext is MainViewModel viewModel)
        {
            viewModel.Dispose();
        }
    }

    private void CheckBox_CheckChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainViewModel viewModel && sender is CheckBox checkBox && checkBox.DataContext is Item item)
        {
            viewModel.UpdateItemCommand.Execute(item);
        }
    }
}
