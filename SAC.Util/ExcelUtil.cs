using Aspose.Cells;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Web;

namespace SAC.Util
{
    /// <summary>
    /// Excel操作类
    /// </summary>
    public class ExcelUtil
    {
        /// <summary>
        /// License文件名
        /// </summary>
        private const string LICENSE_FILENAME = "License.lic";

        /// <summary>
        /// 模板文件所在的目录
        /// </summary>
        private string _templateFilePath = "ExcelTemplate";

        /// <summary>
        /// 文件目录
        /// </summary>
        private string _filePath = "";

        /// <summary>
        /// 导出的文件名称
        /// </summary>
        private string _fileName = "report.xlsx";

        /// <summary>
        /// 文件类型
        /// </summary>
        private FileFormatType _fileFormatType = FileFormatType.Xlsx;

        /// <summary>
        ///
        /// </summary>
        private Dictionary<string, string> _aliasDataSource = new Dictionary<string, string>();

        /// <summary>
        ///
        /// </summary>
        private static ExcelUtil _instance = null;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static ExcelUtil Instance()
        {
            if (_instance == null)
            {
                _instance = new ExcelUtil();
            }

            return _instance;
        }

        /// <summary>
        ///
        /// </summary>
        public ExcelUtil()
        {
            //_filePath = AppContext.BaseDirectory;
            _filePath = Directory.GetCurrentDirectory();
            _templateFilePath = Path.Combine(_filePath, "ExcelTemplate");
            //LoadLicense(_filePath);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName">导出的文件名</param>
        public ExcelUtil(string fileName)
            : this()
        {
            FileName = fileName;
        }

        /// <summary>
        /// 导出的文件名
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _fileName = HttpUtility.UrlEncode(value, System.Text.Encoding.UTF8);
                    if (value.ToLower().EndsWith(".xls"))
                    {
                        _fileFormatType = FileFormatType.Excel97To2003;
                    }
                    else if (value.ToLower().EndsWith(".xlsx"))
                    {
                        _fileFormatType = FileFormatType.Xlsx;
                    }
                    else
                    {
                        _fileFormatType = FileFormatType.Xlsx;
                        _fileName = _fileName + ".xlsx";
                    }
                }
            }
        }

        /// <summary>
        /// 别名数据源
        /// </summary>
        public Dictionary<string, string> AliasDataSource
        {
            get { return _aliasDataSource; }
            set { _aliasDataSource = value; }
        }

        /// <summary>
        /// 加载授权
        /// </summary>
        /// <param name="licensePath"></param>
        /// <returns></returns>
        private bool LoadLicense(string licensePath)
        {
            string path = Path.Combine(licensePath, LICENSE_FILENAME);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                path = "/" + path;

            Console.WriteLine(path);

            if (!File.Exists(Path.Combine(licensePath, LICENSE_FILENAME)))
            {
                throw new Exception("License文件不存在");
            }

            License li = new License();
            li.SetLicense(Path.Combine(licensePath, LICENSE_FILENAME));

            return true;
        }

        /// <summary>
        /// 检查模板是否存在
        /// </summary>
        /// <param name="templateName">模板名称</param>
        /// <returns>存在指定的文件，则为 true；否则为 false</returns>
        private bool ExistsTemplate(ref string templateName)
        {
            if (_fileFormatType == FileFormatType.Xlsx)
            {
                if (!templateName.ToLower().EndsWith(".xlsx"))
                    templateName += ".xlsx";
            }
            else //if (_fileFormatType == FileFormatType.Excel2003 || _fileFormatType == FileFormatType.Default)
            {
                if (!templateName.ToLower().EndsWith(".xls"))
                    templateName += ".xls";
            }

            if (File.Exists(Path.Combine(_templateFilePath, templateName)))
            {
                return true;
            }

            return false;
        }

        public void Save(DataTable data, string templateName, HttpResponseMessage resp)
        {

            if (!ExistsTemplate(ref templateName))
            {
                // 设置错误响应或进行其他处理  
                return;
            }

            // 加载 Workbook  
            string fullPath = Path.Combine(_templateFilePath, templateName);
            Workbook workbook = new Workbook(fullPath);

            // 创建 WorkbookDesigner 并设置 Workbook  
            WorkbookDesigner designer = new WorkbookDesigner(workbook);

            // 设置数据源（假设你有一些数据源和别名）  
            foreach (var item in _aliasDataSource)
            {
                designer.SetDataSource(item.Key, item.Value);
            }

            // 清除数据源（如果你不再需要它）  
            _aliasDataSource.Clear();

            // 处理模板（这通常涉及应用数据源并更新模板）  
            designer.Process();

            // 保存 Workbook 到 MemoryStream  
            MemoryStream memoryStream;
            switch (_fileFormatType)
            {
                case FileFormatType.Xlsx:
                    memoryStream = new MemoryStream();
                    workbook.Save(memoryStream, SaveFormat.Xlsx);
                    break;
                case FileFormatType.Excel97To2003: 
                    memoryStream = new MemoryStream();
                    workbook.Save(memoryStream, SaveFormat.Excel97To2003);
                    break;
                default:
                    // 默认情况，假设是 XLSX  
                    memoryStream = new MemoryStream();
                    workbook.Save(memoryStream, SaveFormat.Xlsx);
                    break;
            }

            // 重置 MemoryStream 的位置  
            memoryStream.Position = 0;

            // 设置 HTTP 响应的内容  
            resp.Content = new StreamContent(memoryStream);
            SetHeaders(resp, _fileName, _fileFormatType, memoryStream.Length);


        }

        private void SetHeaders(HttpResponseMessage resp, string fileName, FileFormatType fileFormatType, long contentLength)
        {
            // 设置适当的Content-Type和Content-Disposition等HTTP响应头  
            // 示例：  
            var contentType = _fileFormatType == FileFormatType.Xlsx ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/vnd.ms-excel";
            var contentDisposition = $"attachment; filename=\"{fileName}.{GetFileExtension(fileFormatType)}\"";

            resp.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
            resp.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = contentDisposition.Split(';')[1].Trim().Split('=')[1].Trim('"')
            };
            resp.Content.Headers.ContentLength = contentLength;
        }

        private string GetFileExtension(FileFormatType fileFormatType)
        {
            switch (fileFormatType)
            {
                case FileFormatType.Xlsx:
                    return "xlsx";
                case FileFormatType.Excel97To2003:
                    return "xls";
                // 添加其他情况...  
                default:
                    return "xls"; // 或者其他默认格式  
            }
        }

        //new Aspose.Cells.License().SetLicense(new MemoryStream(Convert.FromBase64String("PExpY2Vuc2U+CiAgPERhdGE+CiAgICA8TGljZW5zZWRUbz5TdXpob3UgQXVuYm94IFNvZnR3YXJlIENvLiwgTHRkLjwvTGljZW5zZWRUbz4KICAgIDxFbWFpbFRvPnNhbGVzQGF1bnRlYy5jb208L0VtYWlsVG8+CiAgICA8TGljZW5zZVR5cGU+RGV2ZWxvcGVyIE9FTTwvTGljZW5zZVR5cGU+CiAgICA8TGljZW5zZU5vdGU+TGltaXRlZCB0byAxIGRldmVsb3BlciwgdW5saW1pdGVkIHBoeXNpY2FsIGxvY2F0aW9uczwvTGljZW5zZU5vdGU+CiAgICA8T3JkZXJJRD4xOTA4MjYwODA3NTM8L09yZGVySUQ+CiAgICA8VXNlcklEPjEzNDk3NjAwNjwvVXNlcklEPgogICAgPE9FTT5UaGlzIGlzIGEgcmVkaXN0cmlidXRhYmxlIGxpY2Vuc2U8L09FTT4KICAgIDxQcm9kdWN0cz4KICAgICAgPFByb2R1Y3Q+QXNwb3NlLlRvdGFsIGZvciAuTkVUPC9Qcm9kdWN0PgogICAgPC9Qcm9kdWN0cz4KICAgIDxFZGl0aW9uVHlwZT5FbnRlcnByaXNlPC9FZGl0aW9uVHlwZT4KICAgIDxTZXJpYWxOdW1iZXI+M2U0NGRlMzAtZmNkMi00MTA2LWIzNWQtNDZjNmEzNzE1ZmMyPC9TZXJpYWxOdW1iZXI+CiAgICA8U3Vic2NyaXB0aW9uRXhwaXJ5PjIwMjAwODI3PC9TdWJzY3JpcHRpb25FeHBpcnk+CiAgICA8TGljZW5zZVZlcnNpb24+My4wPC9MaWNlbnNlVmVyc2lvbj4KICAgIDxMaWNlbnNlSW5zdHJ1Y3Rpb25zPmh0dHBzOi8vcHVyY2hhc2UuYXNwb3NlLmNvbS9wb2xpY2llcy91c2UtbGljZW5zZTwvTGljZW5zZUluc3RydWN0aW9ucz4KICA8L0RhdGE+CiAgPFNpZ25hdHVyZT53UGJtNUt3ZTYvRFZXWFNIY1o4d2FiVEFQQXlSR0pEOGI3L00zVkV4YWZpQnd5U2h3YWtrNGI5N2c2eGtnTjhtbUFGY3J0c0cwd1ZDcnp6MytVYk9iQjRYUndTZWxsTFdXeXNDL0haTDNpN01SMC9jZUFxaVZFOU0rWndOQkR4RnlRbE9uYTFQajhQMzhzR1grQ3ZsemJLZFZPZXk1S3A2dDN5c0dqYWtaL1E9PC9TaWduYXR1cmU+CjwvTGljZW5zZT4=")));

        public MemoryStream SaveWithEPPlus(DataTable data, string templateName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if (!ExistsTemplate(ref templateName))
            {
                return null;
            }

            string fullPath = Path.Combine(_templateFilePath, templateName);
            // 加载模板  
            var fileInfo = new FileInfo(fullPath);
            using (var package = new ExcelPackage(fileInfo))
            {
                // 假设模板中的第一个工作表是我们要操作的工作表  
                var worksheet = package.Workbook.Worksheets[0];

                // 在这里，我们假设DataTable的列名和Excel模板中的列标题是匹配的  
                // 遍历DataTable的每一行  
                foreach (DataRow row in data.Rows)
                {
                    // 找到模板中对应的行（这里假设模板已经预留了足够的行，或者需要动态添加行）  
                    // EPPlus不直接支持基于DataTable的填充，需要手动处理  
                    // 假设我们从第二行开始填充数据（第一行是标题）  
                    int rowNumber = 2; // 根据实际情况调整  
                    for (int col = 0; col < row.ItemArray.Length; col++)
                    {
                        // 假设Excel中的列也是从A开始  
                        var cell = worksheet.Cells[rowNumber, col + 1]; // Excel是1-based index  
                        if (row[col] == DBNull.Value)
                        {
                            cell.Value = ""; // 填充空字符串或其他默认值  
                        }
                        else
                        {
                            cell.Value = row[col].ToString();
                        }
                    }
                    rowNumber++; // 移动到下一行  
                }

                // 保存为MemoryStream  
                MemoryStream memoryStream = new MemoryStream();
                package.SaveAs(memoryStream);

                // 重置MemoryStream的位置  
                memoryStream.Position = 0;

                return memoryStream;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data">DataTable数据源</param>
        /// <param name="templateName">模板名称</param>
        public MemoryStream Save(DataTable data, string templateName)
        {

            if (!ExistsTemplate(ref templateName))
            {
                return null;
            }

            // 加载 Workbook  
            string fullPath = Path.Combine(_templateFilePath, templateName);
            Workbook workbook = new Workbook(fullPath);

            // 创建 WorkbookDesigner 并设置 Workbook  
            WorkbookDesigner designer = new WorkbookDesigner(workbook);

            foreach (DataRow row in data.Rows)
            {
                foreach (DataColumn column in data.Columns)
                {
                    if (row[column] == DBNull.Value) // 注意检查 DBNull.Value  
                    {
                        if (column.DataType == typeof(int) || column.DataType == typeof(long) || column.DataType == typeof(Int64) || column.DataType == typeof(double))
                        {
                            row[column] = 0; // 对于整数类型的列，设置为0  
                        }
                        else if (column.DataType == typeof(DateTime))
                        {
                            row[column] = DBNull.Value; // 或者设置为 DateTime.MinValue，取决于你的需求  
                                                        // row[column] = DateTime.MinValue; // 如果你确实需要一个默认值而不是null  
                        }
                        else
                        {
                            row[column] = ""; // 对于其他类型，设置为空字符串（但请注意，这可能不适用于所有情况）  
                        }   
                    }
                }
            }

            // 设置数据源  
            designer.SetDataSource(data); // 假设您使用 "MyDataTable" 作为数据源别名  

            // 如果还有其他数据源别名和数据，也在这里设置  
            foreach (var item in _aliasDataSource)
            {
                designer.SetDataSource(item.Key, item.Value);
            }

            // 清除数据源（如果您不再需要它）  
            _aliasDataSource.Clear();

            // 处理模板  
            designer.Process();

            // 保存 Workbook 到 MemoryStream  
            MemoryStream memoryStream = new MemoryStream();
            switch (_fileFormatType)
            {
                case FileFormatType.Xlsx:
                    workbook.Save(memoryStream, SaveFormat.Xlsx);
                    break;
                case FileFormatType.Excel97To2003: // 假设您有一个枚举值表示 XLS  
                    workbook.Save(memoryStream, SaveFormat.Excel97To2003);
                    break;
                default:
                    // 默认情况，假设是 XLSX  
                    workbook.Save(memoryStream, SaveFormat.Xlsx);
                    break;
            }

            // 重置 MemoryStream 的位置  
            memoryStream.Position = 0;

            return memoryStream;
        }

        ///// <summary>
        ///// 保存
        ///// </summary>
        ///// <param name="data">DataSet数据源</param>
        ///// <param name="templateName">报表模板名称</param>
        //public MemoryStream Save(DataSet data, string templateName)
        //{
        //    if (!ExistsTemplate(ref templateName))
        //    {
        //        return null;
        //    }

        //    WorkbookDesigner designer = new WorkbookDesigner();
        //    designer.Open(Path.Combine(_templateFilePath, templateName));
        //    //数据源
        //    designer.SetDataSource(data);

        //    foreach (var item in _aliasDataSource)
        //    {
        //        designer.SetDataSource(item.Key, item.Value);
        //    }

        //    _aliasDataSource.Clear();

        //    designer.Process();

        //    MemoryStream memoryStream = new MemoryStream();
        //    if (_fileFormatType == FileFormatType.Xlsx)
        //    {
        //        designer.Workbook.Save(memoryStream, _fileFormatType);
        //    }
        //    else //if (_fileFormatType == FileFormatType.Excel2003 || _fileFormatType == FileFormatType.Default)
        //    {
        //        memoryStream = designer.Workbook.SaveToStream();
        //    }
        //    memoryStream.Position = 0L;
        //    return memoryStream;
        //}

       

    


        ///// <summary>
        ///// 将Excel文件读取到表格中
        ///// </summary>
        ///// <param name="pstrFileName"></param>
        ///// <returns></returns>
        //public DataTable ReadExcelToDataTable(string pstrFileName)
        //{
        //    try
        //    {
        //        // 从文件中读取Excel
        //        Workbook book = new Workbook();
        //        book.Open(pstrFileName);
        //        Worksheet sheet = book.Worksheets[0];
        //        // 转化成DataTable
        //        DataTable dtData = new DataTable();
        //        if (sheet != null && sheet.Cells.Rows.Count > 1)
        //        {
        //            for (int i = 0; i < sheet.Cells.Rows.Count; i++)
        //            {
        //                DataRow dr = null;
        //                for (int j = 0; j < sheet.Cells.Columns.Count; j++)
        //                {
        //                    string strCellValue = Convert.ToString(sheet.Cells[i, j].Value);
        //                    if (i == 0)
        //                    {
        //                        // 第1行为列头
        //                        DataColumn dc = new DataColumn();
        //                        dc.ColumnName = strCellValue;
        //                        dtData.Columns.Add(dc);
        //                    }
        //                    else
        //                    {
        //                        // 从第2行开始为数据行
        //                        if (dr == null)
        //                        {
        //                            dr = dtData.NewRow();
        //                        }

        //                        dr[j] = strCellValue;
        //                    }
        //                }

        //                if (dr != null)
        //                {
        //                    // 将创建的数据行添加到表格中
        //                    dtData.Rows.Add(dr);
        //                }
        //            }
        //        }

        //        return dtData;
        //    }
        //    finally
        //    {
        //        GC.Collect();
        //    }
        //}

        ///// <summary>
        ///// 将Excel中数据读取到表格中
        ///// </summary>
        ///// <param name="pstrFileName">文件名</param>
        ///// <param name="pstrSheetName">表名</param>
        ///// <param name="mergeColumn">合并的列名</param>
        ///// <returns></returns>
        //public DataTable ReadExcelToDataTable(string pstrFileName, string pstrSheetName, List<string> mergeColumn)
        //{
        //    try
        //    {
        //        // 从文件中读取Excel
        //        Workbook book = new Workbook();
        //        book.Open(pstrFileName);
        //        Worksheet sheet = book.Worksheets[0];
        //        if (!string.IsNullOrEmpty(pstrSheetName))
        //        {
        //            sheet = book.Worksheets[pstrSheetName];
        //        }

        //        // 转化成DataTable
        //        DataTable dtData = new DataTable();
        //        int valueIndex = 0;
        //        if (sheet != null && sheet.Cells.Rows.Count > 1)
        //        {
        //            for (int i = 0; i < sheet.Cells.Rows.Count; i++)
        //            {
        //                DataRow dr = null;
        //                for (int j = 0; j < sheet.Cells.Columns.Count; j++)
        //                {
        //                    string strCellValue = Convert.ToString(sheet.Cells[i, j].Value).Replace("\r\n", "").Trim();
        //                    if (i == 0)
        //                    {
        //                        // 第1行为列头
        //                        DataColumn dc = new DataColumn();
        //                        dc.ColumnName = strCellValue;
        //                        dtData.Columns.Add(dc);
        //                    }
        //                    else
        //                    {
        //                        // 从第2行开始为数据行
        //                        if (dr == null)
        //                        {
        //                            dr = dtData.NewRow();
        //                        }

        //                        if (mergeColumn.Contains(dtData.Columns[j].ColumnName))
        //                        {
        //                            if (string.IsNullOrEmpty(strCellValue))
        //                            {
        //                                dr[j] = Convert.ToString(sheet.Cells[valueIndex, j].Value);
        //                            }
        //                            else
        //                            {
        //                                valueIndex = i;
        //                                dr[j] = strCellValue;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            dr[j] = strCellValue;
        //                        }
        //                    }
        //                }

        //                if (dr != null)
        //                {
        //                    // 将创建的数据行添加到表格中
        //                    dtData.Rows.Add(dr);
        //                }
        //            }
        //        }

        //        return dtData;
        //    }
        //    finally
        //    {
        //        GC.Collect();
        //    }
        //}
    }
}