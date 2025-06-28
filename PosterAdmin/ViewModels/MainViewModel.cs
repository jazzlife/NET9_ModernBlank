using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PosterAdmin.Models;
using PosterAdmin.Services;
using System.Collections.ObjectModel;
using System.Windows;

namespace PosterAdmin.ViewModels;

/// <summary>
/// 메인 윈도우의 ViewModel
/// UI 로직과 데이터 바인딩을 담당합니다.
/// </summary>
public partial class MainViewModel : ObservableObject, IDisposable
{
    private readonly LiteDbController _dbController = null!;
    private bool _disposed = false;

    /// <summary>
    /// 아이템 컬렉션 (UI에 바인딩됨)
    /// </summary>
    public ObservableCollection<Item> Items { get; } = new();

    /// <summary>
    /// 새 아이템의 이름
    /// </summary>
    [ObservableProperty]
    private string _newItemName = string.Empty;

    /// <summary>
    /// 새 아이템의 설명
    /// </summary>
    [ObservableProperty]
    private string _newItemDescription = string.Empty;

    /// <summary>
    /// 선택된 아이템
    /// </summary>
    [ObservableProperty]
    private Item? _selectedItem;

    /// <summary>
    /// 검색 텍스트
    /// </summary>
    [ObservableProperty]
    private string _searchText = string.Empty;

    /// <summary>
    /// 로딩 상태
    /// </summary>
    [ObservableProperty]
    private bool _isLoading = false;

    /// <summary>
    /// 상태 메시지
    /// </summary>
    [ObservableProperty]
    private string _statusMessage = "준비됨";

    /// <summary>
    /// 총 아이템 개수
    /// </summary>
    [ObservableProperty]
    private int _totalItemCount = 0;

    public MainViewModel()
    {
        try
        {
            // LiteDB 컨트롤러 초기화
            _dbController = new LiteDbController();
            
            // 초기 데이터 로드
            LoadItems();
            
            // 샘플 데이터 추가 (최초 실행 시)
            if (Items.Count == 0)
            {
                AddSampleData();
            }

            StatusMessage = "준비됨 - 데이터베이스가 성공적으로 초기화되었습니다.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"초기화 실패: {ex.Message}";
            
            // 기본 컨트롤러라도 생성
            try
            {
                _dbController = new LiteDbController();
            }
            catch
            {
                // 완전 실패 시 더미 처리
                StatusMessage = "심각한 오류: 데이터베이스를 초기화할 수 없습니다.";
            }
        }
    }

    /// <summary>
    /// 아이템 추가 명령
    /// </summary>
    [RelayCommand]
    private async Task AddItem()
    {
        if (string.IsNullOrWhiteSpace(NewItemName))
        {
            StatusMessage = "아이템 이름을 입력해주세요.";
            return;
        }

        try
        {
            IsLoading = true;
            StatusMessage = "아이템 추가 중...";

            var newItem = new Item
            {
                Name = NewItemName.Trim(),
                Description = NewItemDescription.Trim(),
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            await Task.Run(() => _dbController.AddItem(newItem));

            // UI 업데이트
            Items.Insert(0, newItem);
            TotalItemCount = Items.Count;

            // 입력 필드 초기화
            NewItemName = string.Empty;
            NewItemDescription = string.Empty;

            StatusMessage = $"'{newItem.Name}' 아이템이 추가되었습니다.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"아이템 추가 실패: {ex.Message}";
            MessageBox.Show($"아이템 추가 중 오류가 발생했습니다.\n{ex.Message}", "오류", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// 아이템 삭제 명령
    /// </summary>
    [RelayCommand]
    private async Task DeleteItem(Item? item)
    {
        if (item == null) return;

        var result = MessageBox.Show($"'{item.Name}' 아이템을 삭제하시겠습니까?", 
            "삭제 확인", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes) return;

        try
        {
            IsLoading = true;
            StatusMessage = "아이템 삭제 중...";

            await Task.Run(() => _dbController.DeleteItem(item.Id));

            // UI 업데이트
            Items.Remove(item);
            TotalItemCount = Items.Count;

            StatusMessage = $"'{item.Name}' 아이템이 삭제되었습니다.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"아이템 삭제 실패: {ex.Message}";
            MessageBox.Show($"아이템 삭제 중 오류가 발생했습니다.\n{ex.Message}", "오류", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// 아이템 새로고침 명령
    /// </summary>
    [RelayCommand]
    private async Task RefreshItems()
    {
        try
        {
            IsLoading = true;
            StatusMessage = "데이터 새로고침 중...";

            await Task.Run(() => LoadItems());

            StatusMessage = $"{Items.Count}개의 아이템을 로드했습니다.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"새로고침 실패: {ex.Message}";
            MessageBox.Show($"데이터 새로고침 중 오류가 발생했습니다.\n{ex.Message}", "오류", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// 검색 명령
    /// </summary>
    [RelayCommand]
    private async Task Search()
    {
        try
        {
            IsLoading = true;

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                StatusMessage = "전체 아이템 조회 중...";
                await Task.Run(() => LoadItems());
            }
            else
            {
                StatusMessage = $"'{SearchText}' 검색 중...";
                
                var searchResults = await Task.Run(() => 
                    _dbController.SearchItemsByName(SearchText.Trim()));

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Items.Clear();
                    foreach (var item in searchResults)
                    {
                        Items.Add(item);
                    }
                    TotalItemCount = Items.Count;
                });
            }

            StatusMessage = $"{Items.Count}개의 아이템을 찾았습니다.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"검색 실패: {ex.Message}";
            MessageBox.Show($"검색 중 오류가 발생했습니다.\n{ex.Message}", "오류", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// 모든 데이터 삭제 명령
    /// </summary>
    [RelayCommand]
    private async Task ClearAllData()
    {
        var result = MessageBox.Show("모든 데이터를 삭제하시겠습니까?\n이 작업은 되돌릴 수 없습니다!", 
            "전체 삭제 확인", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes) return;

        try
        {
            IsLoading = true;
            StatusMessage = "모든 데이터 삭제 중...";

            await Task.Run(() => _dbController.ClearAllData());

            // UI 업데이트
            Items.Clear();
            TotalItemCount = 0;

            StatusMessage = "모든 데이터가 삭제되었습니다.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"데이터 삭제 실패: {ex.Message}";
            MessageBox.Show($"데이터 삭제 중 오류가 발생했습니다.\n{ex.Message}", "오류", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// 아이템 로드 (비동기)
    /// </summary>
    private void LoadItems()
    {
        var items = _dbController.GetAllItems();

        Application.Current.Dispatcher.Invoke(() =>
        {
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }
            TotalItemCount = Items.Count;
        });
    }

    /// <summary>
    /// 샘플 데이터 추가
    /// </summary>
    private void AddSampleData()
    {
        try
        {
            var sampleItems = new[]
            {
                new Item { Name = "첫 번째 아이템", Description = "이것은 샘플 아이템입니다.", IsActive = true },
                new Item { Name = "두 번째 아이템", Description = "Material Design이 적용된 아이템입니다.", IsActive = true },
                new Item { Name = "세 번째 아이템", Description = "LiteDB에 저장되는 아이템입니다.", IsActive = false }
            };

            foreach (var item in sampleItems)
            {
                _dbController.AddItem(item);
                Items.Add(item);
            }

            TotalItemCount = Items.Count;
            StatusMessage = $"{sampleItems.Length}개의 샘플 아이템이 추가되었습니다.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"샘플 데이터 추가 실패: {ex.Message}";
        }
    }

    /// <summary>
    /// 아이템 업데이트 명령
    /// </summary>
    [RelayCommand]
    private async Task UpdateItem(Item item)
    {
        if (item == null) return;

        try
        {
            IsLoading = true;
            StatusMessage = "아이템 업데이트 중...";

            await Task.Run(() => _dbController.UpdateItem(item));

            StatusMessage = $"'{item.Name}' 아이템이 업데이트되었습니다.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"아이템 업데이트 실패: {ex.Message}";
            MessageBox.Show($"아이템 업데이트 중 오류가 발생했습니다.\n{ex.Message}", "오류", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// 리소스 해제
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _dbController?.Dispose();
            _disposed = true;
        }
    }
} 