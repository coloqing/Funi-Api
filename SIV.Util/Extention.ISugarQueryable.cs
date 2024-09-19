using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Util.DTO;

namespace SIV.Util
{
    /// <summary>
    /// ISugarQueryable"T"的拓展操作
    /// </summary>
    public static partial class Extention
    {
  
        /// <summary>
        /// 获取分页数据(包括总数量)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="sortFile">排序字段</param>
        /// <param name="sortType">排序类型</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageRow">每页条数(0：查询全部)</param>
        /// <returns></returns>
        public static async Task<PageResult<T>> GetPageResultAsync<T>(this ISugarQueryable<T> source, string sortFile, string sortType, int pageIndex, int pageRow)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (pageIndex <= 0) throw new ArgumentOutOfRangeException(nameof(pageIndex), "Page index must be greater than 0.");

            int count = await source.CountAsync();

            // 增加不分页判断
            if (pageRow <= 0)
            {
                pageRow = count;
            }

            List<T> list;
            if (string.IsNullOrEmpty(sortFile) || string.IsNullOrEmpty(sortType))
            {
                // 如果没有提供排序信息，则直接分页
                list = await source.Skip((pageIndex - 1) * pageRow)
                    .Take(pageRow)
                    .ToListAsync();
            }
            else
            {
                // 验证 sortType 是否为有效值（这里简单检查 asc 或 desc）
                if (!new[] { "asc", "desc" }.Contains(sortType, StringComparer.OrdinalIgnoreCase))
                {
                    throw new ArgumentException("Sort type must be 'asc' or 'desc'.", nameof(sortType));
                }

                // 使用提供的排序字段和排序方式进行排序并分页
                list = await source.OrderBy($"{sortFile} {sortType}")
                    .Skip((pageIndex - 1) * pageRow)
                    .Take(pageRow)
                    .ToListAsync();
            }

            // 返回分页结果
            return new PageResult<T>
            {
                Data = list,
                Total = count,
                Success = true,
                Code = 200,
                Message = "查询成功"
            };         
        }       
    }
}