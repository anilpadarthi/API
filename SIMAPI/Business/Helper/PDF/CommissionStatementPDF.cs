using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Models.CommissionStatement;
using SIMAPI.Repository.Interfaces;
using System.Globalization;

namespace SIMAPI.Business.Helper.PDF
{
    public class CommissionStatementPDF
    {
        private IEnumerable<CommissionShopListModel> commissionShopList;

        public async Task<byte[]> GenerateVATPDFStatement(ICommissionStatementRepository commissionStatementRepository, GetReportRequest request)
        {
            commissionShopList = await commissionStatementRepository.GetCommissionShopList(request);

            var imageURL = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Images", "signature.jpg");
            string totalAmount = "", amountInWords = "", monthName = "";
            DateTime commissionGivenDate;
            if (commissionShopList.Count() > 0)
            {
                QuestPDF.Settings.License = LicenseType.Community;
                try
                {
                    return Document.Create(container =>
                    {
                        foreach (var customer in commissionShopList)
                        {
                            var vatAmount = Convert.ToDecimal(customer.CommissionAmount) * 20 / 100;
                            var netAmount = customer.CommissionAmount - vatAmount;
                            commissionGivenDate = customer.CommissionDate.Value.AddMonths(1);
                            monthName = commissionGivenDate.ToString("MMMM, yyyy");
                            commissionGivenDate = GetMonthEndDate(commissionGivenDate.Year, commissionGivenDate.Month);
                            container.Page(async page =>
                            {
                                page.Margin(1, Unit.Centimetre);
                                page.Background().Element(ComposeWatermark);
                                page.Content().Column(column =>
                                    {
                                        column.Item().Text("1A Victoria Road").AlignRight().FontSize(10).FontFamily("Calibri");
                                        column.Item().Text("London, E18 1LJ").AlignRight().FontSize(10).FontFamily("Calibri");
                                        column.Item().PaddingBottom(10).Text("Commission Statement for the month of " + monthName).AlignCenter().FontSize(14).FontFamily("Calibri").Bold();

                                        column.Item().Table(table =>
                                        {
                                            table.ColumnsDefinition(columns =>
                                            {
                                                columns.RelativeColumn(2);
                                                columns.RelativeColumn(3);
                                                columns.RelativeColumn(1);
                                            });

                                            table.Cell().Element(CellNoBorderStyle).Text(customer.Address1).FontFamily("Calibri").FontSize(10).Bold();
                                            table.Cell().Element(CellNoBorderStyle).Text("Shop Id :").FontFamily("Calibri").FontSize(10).Bold().AlignRight();
                                            table.Cell().Element(CellNoBorderStyle).Text(customer.ShopId.ToString()).FontFamily("Calibri").FontSize(10).Bold().AlignRight();

                                            table.Cell().Element(CellNoBorderStyle).Text(customer.Address2).FontFamily("Calibri").FontSize(10).Bold();
                                            table.Cell().Element(CellNoBorderStyle).Text("Area Code :").FontFamily("Calibri").FontSize(10).Bold().AlignRight();
                                            table.Cell().Element(CellNoBorderStyle).Text(customer.AreaCode).FontFamily("Calibri").FontSize(10).Bold().AlignRight();

                                            table.Cell().Element(CellNoBorderStyle).Text(customer.AreaName).FontFamily("Calibri").FontSize(10).Bold();
                                            table.Cell().Element(CellNoBorderStyle).Text("Agent :").FontFamily("Calibri").FontSize(10).Bold().AlignRight();
                                            table.Cell().Element(CellNoBorderStyle).Text(customer.UserName).FontFamily("Calibri").FontSize(10).Bold().AlignRight();

                                            table.Cell().Element(CellNoBorderStyle).Text(customer.PostCode).FontFamily("Calibri").FontSize(10).Bold();
                                            table.Cell().Element(CellNoBorderStyle).Text("Date :").FontFamily("Calibri").FontSize(10).Bold().AlignRight();
                                            table.Cell().Element(CellNoBorderStyle).Text(commissionGivenDate.ToString("dd/MM/yyyy", new CultureInfo("en-GB"))).FontFamily("Calibri").FontSize(10).Bold().AlignRight();
                                        });

                                        column.Item().Table(table =>
                                        {
                                            table.ColumnsDefinition(columns =>
                                            {
                                                columns.RelativeColumn(3);
                                                columns.RelativeColumn(1);
                                                columns.RelativeColumn(1);
                                                columns.RelativeColumn(1);
                                            });

                                            table.Header(header =>
                                            {

                                                header.Cell().Element(CellStyle).Text("Description").FontFamily("Calibri").FontSize(10).Bold();
                                                header.Cell().Element(CellStyle).Text("Net Commission").FontFamily("Calibri").FontSize(10).Bold().AlignCenter();
                                                header.Cell().Element(CellStyle).Text("VAT").FontFamily("Calibri").FontSize(10).Bold().AlignCenter();
                                                header.Cell().Element(CellStyle).Text("Total Commission").FontFamily("Calibri").FontSize(10).Bold().AlignCenter();
                                            });

                                            table.Cell().Element(CellStyle).Text("The Attached Commission Statement is a VAT Invoice wherein the retailer is liable to pay VAT on the commission earned.").FontFamily("Calibri").FontSize(10);
                                            table.Cell().Element(CellStyle).Text(Convert.ToString(netAmount)).FontFamily("Calibri").FontSize(10).AlignCenter();
                                            table.Cell().Element(CellStyle).Text(Convert.ToString(vatAmount)).FontFamily("Calibri").FontSize(10).Bold().AlignCenter();
                                            table.Cell().Element(CellStyle).Text(Convert.ToString(customer.CommissionAmount)).FontFamily("Calibri").FontSize(10).AlignCenter();

                                        });

                                        // Add a page break between customers
                                        //if (commissionShopList.Count() != pageCount)
                                        //{
                                        //    column.Item().PageBreak();
                                        //}
                                    });

                                //page.Footer().Padding(10).Width(100).AlignRight().Image(imageURL);

                            });
                        }

                    }).GeneratePdf();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }

        public async Task<byte[]> GeneratePDFStatement(ICommissionStatementRepository commissionStatementRepository, GetReportRequest request)
        {
            commissionShopList = await commissionStatementRepository.GetCommissionShopList(request);

            foreach (var item in commissionShopList)
            {
                GetReportRequest getReportRequest = new GetReportRequest();
                getReportRequest.shopId = item.ShopId;
                getReportRequest.fromDate = request.fromDate;
                item.commissionStatementDetails = await commissionStatementRepository.GetCommissionStatementAsync(getReportRequest);
            }
            var imageURL = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Images", "signature.jpg");
            string totalAmount = "", amountInWords = "", monthName = "";
            DateTime commissionGivenDate;
            QuestPDF.Settings.License = LicenseType.Community;
            return Document.Create(container =>
            {
                foreach (var customer in commissionShopList)
                {
                    totalAmount = Convert.ToString(customer.commissionStatementDetails.Sum(s => s.Comm1 + s.Comm2));
                    amountInWords = NumberToText(Convert.ToInt32(totalAmount.Split('.')[0]), false) + " Pounds and " + NumberToText(Convert.ToInt32(totalAmount.Split('.')[1]), false) + " Pence Only/-";
                    commissionGivenDate = customer.CommissionDate.Value.AddMonths(1);
                    monthName = commissionGivenDate.ToString("MMMM, yyyy");
                    commissionGivenDate = GetMonthEndDate(commissionGivenDate.Year, commissionGivenDate.Month);
                    container.Page(async page =>
                    {
                        page.Margin(1, Unit.Centimetre);
                        page.Background().Element(ComposeWatermark);
                        page.Content().Column(column =>
                        {
                            column.Item().Text("1A Victoria Road").AlignRight().FontSize(10).FontFamily("Calibri");
                            column.Item().Text("London, E18 1LJ").AlignRight().FontSize(10).FontFamily("Calibri");
                            column.Item().PaddingBottom(10).Text("Commission Statement for the month of " + monthName).AlignCenter().FontSize(14).FontFamily("Calibri").Bold();

                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(1);
                                });

                                table.Cell().Element(CellNoBorderStyle).Text(customer.Address1).FontFamily("Calibri").FontSize(10).Bold();
                                table.Cell().Element(CellNoBorderStyle).Text("Shop Id :").FontFamily("Calibri").FontSize(10).Bold().AlignRight();
                                table.Cell().Element(CellNoBorderStyle).Text(customer.ShopId.ToString()).FontFamily("Calibri").FontSize(10).Bold().AlignRight();

                                table.Cell().Element(CellNoBorderStyle).Text(customer.Address2).FontFamily("Calibri").FontSize(10).Bold();
                                table.Cell().Element(CellNoBorderStyle).Text("Area Code :").FontFamily("Calibri").FontSize(10).Bold().AlignRight();
                                table.Cell().Element(CellNoBorderStyle).Text(customer.AreaCode).FontFamily("Calibri").FontSize(10).Bold().AlignRight();

                                table.Cell().Element(CellNoBorderStyle).Text(customer.AreaName).FontFamily("Calibri").FontSize(10).Bold();
                                table.Cell().Element(CellNoBorderStyle).Text("Agent :").FontFamily("Calibri").FontSize(10).Bold().AlignRight();
                                table.Cell().Element(CellNoBorderStyle).Text(customer.UserName).FontFamily("Calibri").FontSize(10).Bold().AlignRight();

                                table.Cell().Element(CellNoBorderStyle).Text(customer.PostCode).FontFamily("Calibri").FontSize(10).Bold();
                                table.Cell().Element(CellNoBorderStyle).Text("Date :").FontFamily("Calibri").FontSize(10).Bold().AlignRight();
                                table.Cell().Element(CellNoBorderStyle).Text(commissionGivenDate.ToString("dd/MM/yyyy", new CultureInfo("en-GB"))).FontFamily("Calibri").FontSize(10).Bold().AlignRight();
                            });

                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(2);
                                });

                                table.Header(header =>
                                {

                                    header.Cell().Element(CellStyle).Text(" ").FontFamily("Calibri").FontSize(10).Bold();
                                    header.Cell().ColumnSpan(3).Element(CellStyle).AlignCenter().Text("1st Topup").FontFamily("Calibri").FontSize(10).Bold();
                                    header.Cell().ColumnSpan(3).Element(CellStyle).AlignCenter().Text("Following Topups").FontFamily("Calibri").FontSize(10).Bold();
                                    header.Cell().ColumnSpan(1).Element(CellStyle).Text("").FontFamily("Calibri").FontSize(10).Bold();

                                    header.Cell().Element(CellStyle).Text("Network").FontFamily("Calibri").FontSize(10).Bold();
                                    header.Cell().Element(CellStyle).Text("1st").FontFamily("Calibri").FontSize(10).Bold().AlignCenter();
                                    header.Cell().Element(CellStyle).Text("Rate (£)").FontFamily("Calibri").FontSize(10).Bold().AlignCenter();
                                    header.Cell().Element(CellStyle).Text("Total (£)").FontFamily("Calibri").FontSize(10).Bold().AlignCenter();
                                    header.Cell().Element(CellStyle).Text("Topups").FontFamily("Calibri").FontSize(10).Bold().AlignCenter();
                                    header.Cell().Element(CellStyle).Text("Rate (£)").FontFamily("Calibri").FontSize(10).Bold().AlignCenter();
                                    header.Cell().Element(CellStyle).Text("Total (£)").FontFamily("Calibri").FontSize(10).Bold().AlignCenter();
                                    header.Cell().Element(CellStyle).Text("Grand Total (£)").FontFamily("Calibri").FontSize(10).Bold().AlignCenter();
                                });

                                foreach (var detail in customer.commissionStatementDetails)
                                {
                                    table.Cell().Element(CellStyle).Text(detail.Network).FontFamily("Calibri").FontSize(10).Bold();
                                    table.Cell().Element(CellStyle).Text(detail.Conn1.ToString()).FontFamily("Calibri").FontSize(10).AlignCenter();
                                    table.Cell().Element(CellStyle).Text(detail.Rate1.ToString("F2")).FontFamily("Calibri").FontSize(10).Bold().AlignCenter();
                                    table.Cell().Element(CellStyle).Text(detail.Comm1.ToString("F2")).FontFamily("Calibri").FontSize(10).AlignCenter();
                                    table.Cell().Element(CellStyle).Text(detail.Conn2.ToString()).FontFamily("Calibri").FontSize(10).AlignCenter();
                                    table.Cell().Element(CellStyle).Text(detail.Rate2.ToString("F2")).FontFamily("Calibri").FontSize(10).Bold().AlignCenter();
                                    table.Cell().Element(CellStyle).Text(detail.Comm2.ToString("F2")).FontFamily("Calibri").FontSize(10).AlignCenter();
                                    table.Cell().Element(CellStyle).Text((detail.Comm1 + detail.Comm2).ToString("F2")).FontSize(10).FontFamily("Calibri").AlignCenter();
                                }

                                table.Cell().Element(CellStyle).Text("Total").FontFamily("Calibri").FontSize(10).Bold();
                                table.Cell().Element(CellStyle).Text(customer.commissionStatementDetails.Sum(s => s.Conn1).ToString()).FontFamily("Calibri").FontSize(10).AlignCenter();
                                table.Cell().Element(CellStyle).Text("").FontFamily("Calibri").FontSize(10).Bold().AlignCenter();
                                table.Cell().Element(CellStyle).Text(customer.commissionStatementDetails.Sum(s => s.Comm1).ToString()).FontFamily("Calibri").FontSize(10).AlignCenter();
                                table.Cell().Element(CellStyle).Text(customer.commissionStatementDetails.Sum(s => s.Conn2).ToString()).FontFamily("Calibri").FontSize(10).AlignCenter();
                                table.Cell().Element(CellStyle).Text("").FontFamily("Calibri").FontSize(10).Bold().AlignCenter();
                                table.Cell().Element(CellStyle).Text(customer.commissionStatementDetails.Sum(s => s.Comm2).ToString()).FontFamily("Calibri").FontSize(10).AlignCenter();
                                table.Cell().Element(CellStyle).Text(totalAmount).FontSize(10).FontFamily("Calibri").AlignCenter();

                            });

                            column.Item().PaddingTop(5).Text("To re-stock the sims please call: 0333-0119-880").AlignCenter().FontSize(10).FontFamily("Calibri").Bold();
                            column.Item().PaddingTop(5).Text("This is a Commission statement and is not a VAT document. If you are VAT registered VAT should be charged on your invoice at the appropriate rate.").AlignCenter().FontSize(10).FontFamily("Calibri");
                            column.Item().PaddingTop(10).Text(customer.AreaCode + "/" + customer.ShopId + "/" + customer.ShopCommissionHistoryId).AlignLeft().FontSize(10).FontFamily("Calibri").Bold();
                            if (request.isDisplayChequeInfo.HasValue && request.isDisplayChequeInfo.Value)
                            {
                                column.Item().PaddingTop(50).PaddingRight(10).Text(commissionGivenDate.ToString("dd/MM/yyyy", new CultureInfo("en-GB"))).AlignRight().FontSize(10).FontFamily("Calibri").Bold();
                                column.Item().PaddingTop(10).PaddingLeft(10).Text($"{customer.PayableName}").FontSize(10).FontFamily("Calibri").Bold();
                                column.Item().PaddingTop(10).PaddingRight(30).Text(totalAmount).AlignRight().FontSize(10).FontFamily("Calibri").Bold();
                                column.Item().PaddingTop(10).PaddingLeft(10).Text(amountInWords).FontSize(10).FontFamily("Calibri").Bold();

                                column.Item().PaddingTop(10).PaddingLeft(400).Width(100).AlignRight().Image(imageURL);
                            }
                            // Add a page break between customers
                            //if (commissionShopList.Count() != pageCount)
                            //{
                            //    column.Item().PageBreak();
                            //}
                        });

                        //page.Footer().Padding(10).Width(100).AlignRight().Image(imageURL);

                    });
                }

            }).GeneratePdf();

        }





        private static IContainer CellStyle(IContainer container) =>
            container.Border(0.5f).Padding(3).AlignMiddle();

        private static IContainer CellNoBorderStyle(IContainer container) =>
            container.Border(0).Padding(3).AlignMiddle();

        private static IContainer CellStyleWithBackground(IContainer container)
        {
            return container
                .Border(1)
                .Background(Colors.Grey.Lighten2) // Light Gray Background
                .Padding(5)
                .AlignMiddle();
        }
        void ComposeWatermark(IContainer container)
        {
            container
                .AlignCenter()
                .AlignMiddle()
                .Rotate(-45) // Rotate to appear diagonally
                .Text("CONFIDENTIAL")
                .FontSize(60)
                .FontColor(Colors.Grey.Lighten3)
                .Bold();
        }

        private static string NumberToText(int number, bool isUK)
        {
            if (number == 0) return "Zero";
            string and = isUK ? "and " : ""; // deals with UK or US numbering
            if (number == -2147483648) return "Minus Two Billion One Hundred " + and +
            "Forty Seven Million Four Hundred " + and + "Eighty Three Thousand " +
            "Six Hundred " + and + "Forty Eight";
            int[] num = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (number < 0)
            {
                sb.Append("Minus ");
                number = -number;
            }
            string[] words0 = { "", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine " };
            string[] words1 = { "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen " };
            string[] words2 = { "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
            string[] words3 = { "Thousand ", "Million ", "Billion " };
            num[0] = number % 1000;           // units
            num[1] = number / 1000;
            num[2] = number / 1000000;
            num[1] = num[1] - 1000 * num[2];  // thousands
            num[3] = number / 1000000000;     // billions
            num[2] = num[2] - 1000 * num[3];  // millions
            for (int i = 3; i > 0; i--)
            {
                if (num[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (num[i] == 0) continue;
                u = num[i] % 10;              // ones
                t = num[i] / 10;
                h = num[i] / 100;             // hundreds
                t = t - 10 * h;               // tens
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0 || i < first) sb.Append(and);
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd();
        }

        private DateTime GetMonthEndDate(int year, int month)
        {
            int lastDay = DateTime.DaysInMonth(year, month);
            return new DateTime(year, month, lastDay);
        }


    }
}
