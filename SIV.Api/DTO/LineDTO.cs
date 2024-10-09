using SqlSugar;

namespace SIV.Api.DTO
{
    [SugarTable("Lines")]
    public class LineDTO
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)] 
        public int Id { get; set; }

        public string Name { get; set; }
        public string LineId { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
