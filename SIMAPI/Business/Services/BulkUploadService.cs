using AutoMapper;
using ClosedXML.Excel;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Repository.Interfaces;
using System.Data;
using System.Net;

namespace SIMAPI.Business.Services
{
    public class BulkUploadService : IBulkUploadService
    {
        private readonly IBulkUploadRepository _bulkRepository;
        private readonly INetworkRepository _networkRepository;
        private readonly ILookUpRepository _lookUpRepository;
        private readonly IMapper _mapper;
        private readonly IFileUtility _fileUtility;
        public BulkUploadService(IBulkUploadRepository bulkRepository, INetworkRepository networkRepository, IMapper mapper, IFileUtility fileUtility,
            ILookUpRepository lookUpRepository)
        {
            _bulkRepository = bulkRepository;
            _networkRepository = networkRepository;
            _mapper = mapper;
            _fileUtility = fileUtility;
            _lookUpRepository = lookUpRepository;
        }
        public async Task<CommonResponse> UploadFile(BulkUploadDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                DataTable dt = new DataTable();
                var fileLocation = _fileUtility.UploadFile(request.ImportFile, request.ImportType);
                var statusMessage = "";
                if (request.ImportType == "Stock")
                {
                    statusMessage = await ValidateStockFile(fileLocation);
                }
                else
                {
                    statusMessage = await ValidateBulkFile(request.ImportType, fileLocation, request.SelectedDate);
                }
                if (statusMessage == "Success")
                {
                    BulkUploadFile obj = new BulkUploadFile();
                    obj.FilePath = fileLocation;
                    obj.FileStatus = "Pending";
                    obj.CreatedDate = DateTime.Now;
                    obj.FileType = request.ImportType;
                    obj.FileName = request.ImportFile.FileName;
                    obj.ExclusiveDate = request.SelectedDate;
                    _bulkRepository.Add(obj);
                    await _bulkRepository.SaveChangesAsync();
                    response = Utility.CreateResponse("Uploaded successfully, It is being processed soon.", HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse(statusMessage, HttpStatusCode.OK);
                }

            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _bulkRepository);
            }
            return response;

        }

        private DataTable LoadExcel(string fileLocation, string type)
        {
            XLWorkbook workbook = new XLWorkbook(fileLocation);
            bool firstRow = true;
            DataTable dt = new DataTable();
            foreach (IXLRow row in workbook.Worksheet(1).Rows())
            {
                //Use the first row to add columns to DataTable.
                if (firstRow)
                {
                    foreach (IXLCell cell in row.Cells())
                    {
                        dt.Columns.Add(cell.Value.ToString().Trim());
                    }
                    firstRow = false;
                }
                else if (type == "StockDataLoad")
                {
                    //Add rows to DataTable.
                    dt.Rows.Add();
                    if (row.CellsUsed().Count() > 0)
                    {
                        dt.Rows[dt.Rows.Count - 1][0] = row.Cells().ToList()[0].Value.ToString();
                        dt.Rows[dt.Rows.Count - 1][1] = row.Cells().ToList()[1].Value.ToString();
                        dt.Rows[dt.Rows.Count - 1][2] = row.Cells().ToList()[2].Value.ToString();
                        dt.Rows[dt.Rows.Count - 1][3] = row.Cells().ToList()[3].Value.ToString();
                        dt.Rows[dt.Rows.Count - 1][4] = row.Cells().ToList()[4].Value.ToString();
                        dt.Rows[dt.Rows.Count - 1][5] = row.Cells().ToList()[5].Value.ToString();
                    }
                }
                else if(type == "AccessoriesStockDataLoad")
                {
                    dt.Rows.Add();
                    if (row.CellsUsed().Count() > 0)
                    {
                        dt.Rows[dt.Rows.Count - 1][0] = row.Cells().ToList()[0].Value.ToString();
                        dt.Rows[dt.Rows.Count - 1][1] = row.Cells().ToList()[1].Value.ToString();
                        dt.Rows[dt.Rows.Count - 1][2] = row.Cells().ToList()[2].Value.ToString();
                    }
                }
            }

            return dt;
        }

        public async Task<string> ValidateStockFile(string fileLocation)
        {
            DataTable dt = new DataTable();
            dt = LoadExcel(fileLocation, "StockTemplate");

            if (dt != null)
            {
                if (!(dt.Columns[0].ToString().Trim().ToUpper() == "IMEI" && dt.Columns[1].ToString().Trim().ToUpper() == "PCNNO" &&
                    dt.Columns[2].ToString().Trim().ToUpper() == "NETWORK" && dt.Columns[3].ToString().Trim().ToUpper() == "SUPPLIER" &&
                    dt.Columns[4].ToString().Trim().ToUpper() == "SIMCOST" &&
                    dt.Columns[5].ToString().Trim().ToUpper() == "LOTNO"))
                {
                    return "Please upload the correct stock file, with column names IMEI,PCNNO,NETWORK,SUPPLIER,SIMCOST,LOTNO";
                }
                else
                {
                    try
                    {
                        dt = LoadExcel(fileLocation, "StockDataLoad");
                    }
                    catch
                    {
                        return "Uploaded data is invalid, cross check with all the fields data.";
                    }
                    var networkSkuCodeList = await _networkRepository.GetAllNetworksAsync();
                    var supplierAccountNameList = await _lookUpRepository.GetAllSupplierAccountsAsync();

                    var res = dt.AsEnumerable().Select(s => s.Field<string>("NETWORK")).ToArray();
                    string[] uniqueNetworks = dt.DefaultView.ToTable(true, "NETWORK").AsEnumerable().Select(r => r.Field<string>("NETWORK")).ToArray();
                    string[] uniqueSuppliers = dt.DefaultView.ToTable(true, "SUPPLIER").AsEnumerable().Select(r => r.Field<string>("SUPPLIER")).ToArray();

                    bool isValidNetworkNames = true;
                    string invalidNetworkNames = "";
                    foreach (string name in uniqueNetworks)
                    {
                        if (!string.IsNullOrEmpty(name))
                        {
                            var network = networkSkuCodeList.FirstOrDefault(f => f.SkuCode.ToLower().Trim() == name.ToLower().Trim());

                            if (network == null)
                            {
                                isValidNetworkNames = false;
                                invalidNetworkNames += name + "\n";
                            }
                        }
                    }

                    bool isValidSupplierNames = true;
                    string invalidSupplierNames = "";
                    foreach (string name in uniqueSuppliers)
                    {
                        if (!string.IsNullOrEmpty(name))
                        {
                            var accountName = supplierAccountNameList.FirstOrDefault(f => f.Name.ToLower().Trim() == name.ToLower().Trim());

                            if (accountName == null)
                            {
                                isValidSupplierNames = false;
                                invalidSupplierNames += name + "\n";
                            }
                        }
                    }
                    if (isValidNetworkNames && isValidSupplierNames)
                    {
                        return "Success";
                    }
                    else if (!isValidSupplierNames)
                    {
                        return "File has invalid supplier account names " + invalidSupplierNames;
                    }
                    else if (!isValidNetworkNames)
                    {
                        return "File has invalid networks " + invalidNetworkNames;
                    }
                    return "Success";
                }
            }
            else
            {
                return "Somthing went wrong, while uploading";
            }
        }

        public async Task<string> ValidateBulkFile(string uploadFileType, string fileLocation, string exclusiveDate)
        {
            bool isValidFile = false;
            string message = "";
            DataTable dt = new DataTable();
            dt = LoadExcel(fileLocation, uploadFileType);

            if (dt != null)
            {
                if (uploadFileType == "DailyActivation" || uploadFileType == "Spam")
                {
                    if (dt.Columns[0].ToString().Trim().ToUpper() == "IMEI"
                        && dt.Columns[1].ToString().Trim().ToUpper() == "PCNNO"
                        && dt.Columns[2].ToString().Trim().ToUpper() == "DATE")
                    {
                        isValidFile = true;
                    }
                    else
                    {
                        isValidFile = false;
                        message = "Please upload the correct Daily Activation file, with column names IMEI, PCNNO, DATE";
                    }
                }
                else if (uploadFileType == "TrackNumber")
                {
                    if (dt.Columns[1].ToString().Trim().ToUpper() == "ORDERID"
                        && dt.Columns[0].ToString().Trim().ToUpper() == "TRACKINGNUMBER"
                        && dt.Columns[2].ToString().Trim().ToUpper() == "COURIER")
                    {
                        isValidFile = true;
                    }
                    else
                    {
                        isValidFile = false;
                        message = "Please upload the correct bulk Track number file, It should contain the column names 'Reference 1','Consignment','Voided'";
                    }
                }
                else if (uploadFileType == "BankChequeWithdraw")
                {
                    if (dt.Columns.Contains("Date")
                    && dt.Columns.Contains("Type")
                    && dt.Columns.Contains("Description")
                    && dt.Columns.Contains("Amount"))
                    {
                        isValidFile = true;
                    }
                    else
                    {
                        isValidFile = false;
                        message = "Please upload the correct bank cheque withdraw file, It should contain the column names 'Date','Type','Description','Amount'";
                    }
                }
                else if (uploadFileType == "OrderStatus")
                {
                    if (dt.Columns.Contains("OrderId")
                    && dt.Columns.Contains("OrderStatus"))
                    {
                        isValidFile = true;
                    }
                    else
                    {
                        isValidFile = false;
                        message = "Please upload the correct bulk order change status file, It should contain the column names 'OrderId','OrderStatus'";
                    }
                }
                else if (uploadFileType == "Target")
                {
                    if (dt.Columns.Contains("ID")
                    && dt.Columns.Contains("KPI1")
                    && dt.Columns.Contains("KPI1Visits")
                    && dt.Columns.Contains("KPI1Accessories")
                    )
                    {
                        isValidFile = true;
                    }
                    else
                    {
                        isValidFile = false;
                        message = "Please upload the correct bulk Tareget file, It should contain the column names 'ID','KPI1','KPI1Visits','KPI1Accessories'";
                    }
                }
                else if (uploadFileType == "ShopCommissionCheque")
                {
                    if (dt.Columns.Contains("ChequeNo")
                    && dt.Columns.Contains("TotalAmount")
                    && dt.Columns.Contains("ShopId"))
                    {
                        isValidFile = true;
                    }
                    else
                    {
                        isValidFile = false;
                        message = "Please upload the correct shop commission cheque file, File should contain ChequeNo, TotalAmount, ShopId column names.";
                    }
                }
                else if (uploadFileType == "AccessoriesStock")
                {
                    if (
                    dt.Columns.Contains("PRODUCTCODE")
                    && dt.Columns.Contains("PRODUCTCOST")
                    && dt.Columns.Contains("QUANTITY"))
                    {
                        isValidFile = true;
                    }
                    else
                    {
                        isValidFile = false;
                        message = "Please upload the correct accessories stock file, File should contain  PRODUCTCODE, PRODUCTCOST, QUANTITY column names.";
                        return message;
                    }

                    try
                    {
                        dt = LoadExcel(fileLocation, "AccessoriesStockDataLoad");
                    }
                    catch
                    {
                        return "Uploaded data is invalid, cross check with all the fields data.";
                    }


                    var productList = await _lookUpRepository.GetAllProductsWithCodes();

                    var res = dt.AsEnumerable().Select(s => s.Field<string>("PRODUCTCODE")).ToArray();
                    string[] uniqueProductCodes = dt.DefaultView.ToTable(true, "PRODUCTCODE").AsEnumerable().Select(r => r.Field<string>("PRODUCTCODE")).ToArray();

                    bool isValidProducts = true;
                    string invalidProducts = "";
                    foreach (string name in uniqueProductCodes)
                    {
                        if (!string.IsNullOrEmpty(name))
                        {
                            var network = productList.FirstOrDefault(f => f.Name.ToLower().Trim() == name.ToLower().Trim());

                            if (network == null)
                            {
                                isValidProducts = false;
                                invalidProducts += name + "\n";
                            }
                        }
                    }

                    if (isValidProducts)
                    {
                        return "Success";
                    }
                    else
                    {
                        return "File has invalid product codes " + invalidProducts;
                    }
                }
                else if (uploadFileType == "ShopDataChanges")
                {
                    if (dt.Columns.Contains("ShopId")
                    && dt.Columns.Contains("ShopName")
                    && dt.Columns.Contains("Address1")
                    && dt.Columns.Contains("Address2")
                    && dt.Columns.Contains("City")
                    && dt.Columns.Contains("PostCode")
                    && dt.Columns.Contains("AreaId")
                    && dt.Columns.Contains("Vat_No")
                    )
                    {
                        isValidFile = true;
                    }
                    else
                    {
                        isValidFile = false;
                        message = "Please upload the correct bulk shop changes file";
                    }
                }
            }
            if (isValidFile)
            {
                message = "Success";
            }
            return message;
        }


        public async Task<Stream?> DownloadTargetDataAsync(GetReportRequest request)
        {
            var result = await _bulkRepository.DownloadTargetDataAsync(request);
            var stream = ExcelUtility.ConvertDynamicDataToExcelFormatWithColours<dynamic>(result.ToList());

            return stream;
        }
    }
}
