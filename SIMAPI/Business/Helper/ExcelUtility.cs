using ClosedXML.Excel;
using Microsoft.Net.Http.Headers;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SIMAPI.Data.Models.OrderListModels;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace SIMAPI.Business.Helper
{
    public class ExcelUtility
    {

        public static MemoryStream ConvertyListToMemoryStream<T>(List<T> list, string type)
        {
            string[] OrderListColumns = new string[] { "OrderId", "UserId", "UserName" };

            try
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
                DataTable table = new DataTable();
                if (type == "OrderList")
                {
                    foreach (var item in OrderListColumns)
                    {
                        table.Columns.Add(new DataColumn(item));
                    }
                }
                else
                {
                    foreach (PropertyDescriptor prop in properties)
                    {
                        if (type == "OrderList1")
                        {
                            if (Array.IndexOf(OrderListColumns, prop.Name) >= 0)
                            {
                                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                            }
                        }
                    }
                }
                if (type == "OrderList")
                {
                    foreach (var item in list as List<OrderListViewModel>)
                    {
                        DataRow row = table.NewRow();
                        row["OrderId"] = item.OrderId;
                        row["UserId"] = item.UserId;
                        row["UserName"] = item.UserName;
                        table.Rows.Add(row);
                    }
                }
                else
                {
                    foreach (T item in list)
                    {
                        DataRow row = table.NewRow();
                        foreach (PropertyDescriptor prop in properties)
                        {
                            if (type == "OrderList1")
                            {
                                if (Array.IndexOf(OrderListColumns, prop.Name) >= 0)
                                {
                                    var value = prop.GetValue(item) ?? DBNull.Value;
                                    row[prop.Name] = Convert.ToString(value).Replace("-0001", "-1900");
                                }
                            }
                        }
                        table.Rows.Add(row);
                    }

                }
                table.TableName = type; // "data";
                table.AcceptChanges();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(table);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return stream;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static MemoryStream ConvertDataToExcelFormat<T>(List<T> data)
        {
            // Generate Excel file
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells.LoadFromCollection(data, true);

                // Identify DateTime columns and format them
                var properties = typeof(T).GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    var prop = properties[i];
                    if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                    {
                        var column = i + 1; // EPPlus is 1-based index
                        worksheet.Column(column).Style.Numberformat.Format = "yyyy-mm-dd hh:mm";
                    }
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                return stream;
            }
        }

        public static MemoryStream ConvertDynamicDataToExcelFormat<T>(IList<T> data)
        {
            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Report");

            if (data == null || data.Count == 0)
                return new MemoryStream(package.GetAsByteArray());

            // Determine whether the first item is dynamic (ExpandoObject) or a regular class
            bool isDynamic = data[0] is IDictionary<string, object>;

            List<string> headers = new();

            if (isDynamic)
            {
                var dict = (IDictionary<string, object>)data[0];
                headers.AddRange(dict.Keys);
            }
            else
            {
                var props = typeof(T).GetProperties();
                headers.AddRange(props.Select(p => p.Name));
            }

            // Write header
            for (int i = 0; i < headers.Count; i++)
                ws.Cells[1, i + 1].Value = headers[i];

            // Write body
            int row = 2;
            foreach (var item in data)
            {
                if (isDynamic)
                {
                    var dict = (IDictionary<string, object>)item;
                    int col = 1;

                    foreach (var value in dict.Values)
                    {
                        ws.Cells[row, col].Value = value;
                        col++;
                    }
                }
                else
                {
                    var props = typeof(T).GetProperties();
                    int col = 1;

                    foreach (var prop in props)
                    {
                        ws.Cells[row, col].Value = prop.GetValue(item);
                        col++;
                    }
                }

                row++;
            }

            var stream = new MemoryStream(package.GetAsByteArray());
            stream.Position = 0;
            return stream;
        }


        public static MemoryStream ConvertDynamicDataToExcelFormatWithColours<T>(IList<T> data)
        {
            var excludedColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "ShopId",
                "ShopName",
                 "AreaId",
                "AreaName",
                 "AgentId",
                "AgentName",
                 "ManagerId",
                "ManagerName",
            };
            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Report");

            if (data == null || data.Count == 0)
                return new MemoryStream(package.GetAsByteArray());

            bool isDynamic = data[0] is IDictionary<string, object>;
            List<string> headers = new();

            // Get dynamic headers
            if (isDynamic)
            {
                var dict = (IDictionary<string, object>)data[0];
                headers.AddRange(dict.Keys);
            }
            else
            {
                var props = typeof(T).GetProperties();
                headers.AddRange(props.Select(p => p.Name));
            }

            // Write header
            for (int i = 0; i < headers.Count; i++)
                ws.Cells[1, i + 1].Value = headers[i];

            // Write body
            int row = 2;
            foreach (var item in data)
            {
                int col = 1;

                if (isDynamic)
                {
                    var dict = (IDictionary<string, object>)item;

                    foreach (var value in dict.Values)
                    {
                        var cell = ws.Cells[row, col];
                        cell.Value = value;
                        string headerName = headers[col - 1];
                        ApplyColor(cell, value, headerName, excludedColumns);     // 👈 Apply colour here

                        col++;
                    }
                }
                else
                {
                    var props = typeof(T).GetProperties();

                    foreach (var prop in props)
                    {
                        var value = prop.GetValue(item);
                        var cell = ws.Cells[row, col];
                        cell.Value = value;
                        string headerName = headers[col - 1];

                        ApplyColor(cell, value, headerName, excludedColumns);   // 👈 Apply colour here

                        col++;
                    }
                }
                row++;
            }

            // Autofit columns after writing
            ws.Cells[ws.Dimension.Address].AutoFitColumns();

            var stream = new MemoryStream(package.GetAsByteArray());
            stream.Position = 0;
            return stream;
        }


        // ========================================
        //  🔥 Colour Logic Function
        // ========================================
        private static void ApplyColor(ExcelRange cell, object value, string headerName, HashSet<string> excludedColumns)
        {

            if (excludedColumns.Contains(headerName))
                return;

            if (value == null)
                return;

            if (!double.TryParse(value.ToString(), out double numericValue))
                return; // only color numeric values

            var fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;   // 👈 REQUIRED

            if (numericValue >= 10)
                fill.BackgroundColor.SetColor(Color.Green);
            else if (numericValue > 5)
                fill.BackgroundColor.SetColor(Color.Orange);
            else
                fill.BackgroundColor.SetColor(Color.Red);
        }

    }
}
