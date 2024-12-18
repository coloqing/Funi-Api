﻿namespace Util.DTO
{
    /// <summary>
    /// Ajax请求结果
    /// </summary>
    public class AjaxResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// 错误代码
        /// </summary>
        public int Code { get; set; } = 200;

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; } = "查询成功";
    }
}