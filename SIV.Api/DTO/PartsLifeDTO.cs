﻿using SIV.Entity;
using SIV.Util;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace SIV.Api.DTO
{
    /// <summary>
    /// 部件寿命
    /// </summary>
    [Map(typeof(PartsLife))]
    public class PartsLifeDTO
    {       
        

        /// <summary>
        /// 线路
        /// </summary>           
        
        public string? XL { get; set; }

        /// <summary>
        /// 车号
        /// </summary>           
        
        public string? CH { get; set; }

        /// <summary>
        /// 车厢
        /// </summary>           
        
        public string? CX { get; set; }

        /// <summary>
        /// 部件位置名称
        /// </summary>           
        public string? WZName { get; set; }

        /// <summary>
        /// 部件名称
        /// </summary>

        public string? Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>           
        public string? TypeName { get; set; }

        /// <summary>
        /// 已耗寿命/次数
        /// </summary>           

        public decimal? RunLife { get; set; }

        /// <summary>
        /// 额定寿命/次数
        /// </summary>           
        
        public decimal? RatedLife { get; set; }

        /// <summary>
        /// 剩余寿命/次数
        /// </summary>           
        
        public decimal? SurplusLife { get; set; }

        /// <summary>
        /// 百分比
        /// </summary>           

        public decimal? Percent { get; set; }

        /// <summary>
        /// 百分比
        /// </summary>           

        public string? Suggest { get; set; }

        public long Id { get; set; }

        /// <summary>
        /// 部件位置
        /// </summary>           
        public int? WZ { get; set; }

        /// <summary>
        /// 类型
        /// </summary>           

        public string? Type { get; set; }
        public string FaultCode { get; set; }
        public string Code { get; set; }
        public string FarecastCode { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
