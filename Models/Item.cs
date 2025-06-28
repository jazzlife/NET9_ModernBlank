using LiteDB;

namespace PosterAdmin.Models;

/// <summary>
/// 기본 아이템 모델 클래스
/// LiteDB에서 CRUD 작업에 사용됩니다.
/// </summary>
public class Item
{
    /// <summary>
    /// 고유 식별자 (LiteDB에서 자동 생성)
    /// </summary>
    [BsonId]
    public ObjectId Id { get; set; } = ObjectId.NewObjectId();
    
    /// <summary>
    /// 아이템 이름
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// 아이템 설명
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// 생성일시
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    /// <summary>
    /// 활성화 상태
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// 디스플레이용 ID 문자열
    /// </summary>
    [BsonIgnore]
    public string DisplayId => Id.ToString()[^8..]; // 마지막 8자리만 표시
} 