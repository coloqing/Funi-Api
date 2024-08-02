using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Util
{
    public static class ListExtensions
    {
        public static DataTable ToDataTable<T>(this List<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);

            // 获取对象的所有属性  
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {
                // 将属性的名称设置为列名，属性的类型设置为列的类型  
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    // 获取属性的值并添加到数组  
                    values[i] = Props[i].GetValue(item, null);
                }
                // 将数组转换为DataRow并添加到DataTable  
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
}
