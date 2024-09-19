using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SIV.Entity
{
    public abstract class BaseEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key, Column(Order = 1)]
        [MaxLength(50)]
        public String? Id { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [JsonIgnore]
        [MaxLength(50)]
        public String? CreateUser { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [JsonIgnore]
        [MaxLength(50)]
        public String? UpdateUser { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonIgnore]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 修改时间
        /// </summary>
        [JsonIgnore]
        public DateTime UpdateTime { get; set; } = DateTime.Now;
    }
}