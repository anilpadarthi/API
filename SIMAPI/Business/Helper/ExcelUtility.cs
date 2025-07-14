using ClosedXML.Excel;
using OfficeOpenXml;
using SIMAPI.Data.Models.OrderListModels;
using System.ComponentModel;
using System.Data;

namespace SIMAPI.Business.Helper
{
    public class ExcelUtility
    {
        public static MemoryStream ConvertyListToMemoryStream<T>(List<T> list, string type)
        {          
            string[] OrderListColumns = new string[] { "OrderId", "UserId", "UserName"};

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
    }
}
