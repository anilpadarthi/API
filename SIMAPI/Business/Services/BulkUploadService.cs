using AutoMapper;
using ClosedXML.Excel;
using Microsoft.Data.SqlClient;
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
        private readonly IMapper _mapper;
        public BulkUploadService(IBulkUploadRepository bulkRepository, INetworkRepository networkRepository, IMapper mapper)
        {
            _bulkRepository = bulkRepository;
            _networkRepository = networkRepository;
            _mapper = mapper;
        }
        public async Task<CommonResponse> UploadFile(BulkUploadDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                DataTable dt = new DataTable();
                var fileLocation = FileUtility.uploadFile(request.ImportFile, request.ImportType);
                var statusMessage = "";
                if (request.ImportType == "Stock")
                {
                    statusMessage = await ValidateStockFile(fileLocation);
                }
                else
                {
                    statusMessage = ValidateBulkFile(request.ImportType, fileLocation, request.SelectedDate);
                }
                if (statusMessage == "Success")
                {
                    BulkUploadFile obj = new BulkUploadFile();
                    obj.FilePath = fileLocation;
                    obj.FileStatus = "Pending";
                    obj.CreatedDate = DateTime.Now;
                    obj.FileType = request.ImportType;
                    _bulkRepository.Add(obj);
                    await _bulkRepository.SaveChangesAsync();
                }

                response = Utility.CreateResponse(statusMessage, HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _bulkRepository);
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
                        dt.Columns.Add(cell.Value.ToString());
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
                    var res = dt.AsEnumerable().Select(s => s.Field<string>("NETWORK")).ToArray();
                    string[] uniqueCols = dt.DefaultView.ToTable(true, "NETWORK").AsEnumerable().Select(r => r.Field<string>("NETWORK")).ToArray();

                    bool isValidNetworkNames = true;
                    string invalidNetworkNames = "";
                    foreach (string name in uniqueCols)
                    {
                        if (!string.IsNullOrEmpty(name))
                        {
                            int? networkId = networkSkuCodeList.FirstOrDefault(f => f.SkuCode == name).NetworkId;

                            if (networkId == null || networkId == 0)
                            {
                                isValidNetworkNames = false;
                                invalidNetworkNames += name + "\n";
                            }
                        }
                    }
                    if (isValidNetworkNames)
                    {
                        return "Success";
                    }
                    else
                    {
                        return "File has invalid networks " + invalidNetworkNames;
                    }
                }
            }
            else
            {
                return "Somthing went wrong, while uploading";
            }
        }

        public string ValidateBulkFile(string uploadFileType, string fileLocation, string exclusiveDate)
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
                else if (uploadFileType == "BankCheque")
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
                    && dt.Columns.Contains("KPI-1")
                    //&& dt.Columns.Contains("KPI1Visits")
                    //&& dt.Columns.Contains("KPI1Accessories")
                    )
                    {
                        isValidFile = true;
                    }
                    else
                    {
                        isValidFile = false;
                        message = "Please upload the correct bulk Tareget file, It should contain the column names 'ID','KPI1Activations','KPI1Visits','KPI1Accessories'";
                    }
                }
                else if (uploadFileType == "ShopCommissionCheque")
                {
                    if (dt.Columns.Contains("Cheqno")
                    && dt.Columns.Contains("TotalAmount")
                    && dt.Columns.Contains("CommissionDate")
                    && dt.Columns.Contains("ShopId"))
                    {
                        isValidFile = true;
                    }
                    else
                    {
                        isValidFile = false;
                        message = "Please upload the correct shop commission cheque file, File should contain ChequeNo, TotalAmount, ShopId, CommissionDate column names.";
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
    }
}
