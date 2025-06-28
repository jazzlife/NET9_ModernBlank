using LiteDB;
using PosterAdmin.Models;
using System.IO;

namespace PosterAdmin.Services;

/// <summary>
/// LiteDB를 사용한 데이터 접근 컨트롤러
/// 모든 CRUD 작업을 담당합니다.
/// </summary>
public class LiteDbController : IDisposable
{
    private readonly LiteDatabase _database;
    private readonly string _databasePath;
    private bool _disposed = false;

    /// <summary>
    /// LiteDbController 초기화
    /// </summary>
    /// <param name="databasePath">데이터베이스 파일 경로 (기본값: AppData/Local/PosterAdmin/data.db)</param>
    public LiteDbController(string? databasePath = null)
    {
        // 기본 데이터베이스 경로 설정
        if (string.IsNullOrEmpty(databasePath))
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appFolder = Path.Combine(appDataPath, "PosterAdmin");
            Directory.CreateDirectory(appFolder);
            _databasePath = Path.Combine(appFolder, "data.db");
        }
        else
        {
            _databasePath = databasePath;
        }

        // LiteDB 연결 문자열 설정
        var connectionString = new ConnectionString(_databasePath)
        {
            Connection = ConnectionType.Shared
        };

        _database = new LiteDatabase(connectionString);
        
        // Items 컬렉션 생성 및 인덱스 설정
        var items = _database.GetCollection<Item>("items");
        items.EnsureIndex(x => x.Name);
        items.EnsureIndex(x => x.CreatedAt);
    }

    /// <summary>
    /// 새 아이템 추가
    /// </summary>
    /// <param name="item">추가할 아이템</param>
    /// <returns>추가된 아이템의 ID</returns>
    public ObjectId AddItem(Item item)
    {
        var items = _database.GetCollection<Item>("items");
        return items.Insert(item);
    }

    /// <summary>
    /// 모든 아이템 조회
    /// </summary>
    /// <returns>아이템 리스트</returns>
    public List<Item> GetAllItems()
    {
        var items = _database.GetCollection<Item>("items");
        return items.FindAll().OrderByDescending(x => x.CreatedAt).ToList();
    }

    /// <summary>
    /// 활성화된 아이템만 조회
    /// </summary>
    /// <returns>활성화된 아이템 리스트</returns>
    public List<Item> GetActiveItems()
    {
        var items = _database.GetCollection<Item>("items");
        return items.Find(x => x.IsActive).OrderByDescending(x => x.CreatedAt).ToList();
    }

    /// <summary>
    /// ID로 아이템 조회
    /// </summary>
    /// <param name="id">아이템 ID</param>
    /// <returns>찾은 아이템 또는 null</returns>
    public Item? GetItemById(ObjectId id)
    {
        var items = _database.GetCollection<Item>("items");
        return items.FindById(id);
    }

    /// <summary>
    /// 이름으로 아이템 검색
    /// </summary>
    /// <param name="name">검색할 이름</param>
    /// <returns>검색된 아이템 리스트</returns>
    public List<Item> SearchItemsByName(string name)
    {
        var items = _database.GetCollection<Item>("items");
        return items.Find(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                   .OrderByDescending(x => x.CreatedAt)
                   .ToList();
    }

    /// <summary>
    /// 아이템 업데이트
    /// </summary>
    /// <param name="item">업데이트할 아이템</param>
    /// <returns>업데이트 성공 여부</returns>
    public bool UpdateItem(Item item)
    {
        var items = _database.GetCollection<Item>("items");
        return items.Update(item);
    }

    /// <summary>
    /// 아이템 삭제
    /// </summary>
    /// <param name="id">삭제할 아이템 ID</param>
    /// <returns>삭제 성공 여부</returns>
    public bool DeleteItem(ObjectId id)
    {
        var items = _database.GetCollection<Item>("items");
        return items.Delete(id);
    }

    /// <summary>
    /// 아이템 개수 조회
    /// </summary>
    /// <returns>전체 아이템 개수</returns>
    public int GetItemCount()
    {
        var items = _database.GetCollection<Item>("items");
        return items.Count();
    }

    /// <summary>
    /// 모든 데이터 삭제 (주의: 복구 불가능)
    /// </summary>
    public void ClearAllData()
    {
        var items = _database.GetCollection<Item>("items");
        items.DeleteAll();
    }

    /// <summary>
    /// 데이터베이스 연결 해제
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
            _database?.Dispose();
            _disposed = true;
        }
    }
} 