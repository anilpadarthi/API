using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using SIMAPI.Data.Dto;
using SIMAPI.Repository.Interfaces;

namespace SIMAPI.Business.Helper.PDF
{
    public class CommissionStatementPDF
    {
        public async Task<byte[]> GenerateVATPDFStatement(ICommissionStatementRepository commissionStatementRepository, GetReportRequest request)
        {
            var result = await commissionStatementRepository.GetShopListForCommission(request);
            if (result != null && result.Count() > 0)
            {
                //Define your memory stream which will temporarily hold the PDF
                using (MemoryStream stream = new MemoryStream())
                {
                    //Initialize PDF writer
                    PdfWriter writer = new PdfWriter(stream);
                    //Initialize PDF document
                    PdfDocument pdf = new PdfDocument(writer);
                    // Initialize document
                    Document document = new Document(pdf);

                    foreach (var item in result)
                    {
                        GetReportRequest request1 = new GetReportRequest();
                        request1.areaId = item.AreaId;
                        request1.shopId = item.ShopId;
                        request1.fromDate = item.TopupDate.ToString();
                        var commissionStatement = await commissionStatementRepository.GetCommissionStatementAsync(request1);

                        decimal limitAmount = 0;
                        decimal totalCommission = commissionStatement.Sum(s => s.Comm1 + s.Comm2);
                        int month = item.TopupDate.Month;
                        int year = item.TopupDate.Year;
                        if (((month >= 2 && year >= 2023) || year > 2023))
                        {
                            limitAmount = 10;
                        }
                        else if (((month >= 3 && year >= 2021) || year > 2021))
                        {
                            limitAmount = 5;
                        }

                        if (totalCommission >= limitAmount)
                        {
                            Table table = new Table(new float[] { 1, 1 });
                            table.SetWidth(UnitValue.CreatePercentValue(100));
                            table.AddCell("1A Victoria Road");
                            table.AddCell("Tel: 0333-0119-880");
                            table.AddCell("Ilford, London");
                            table.AddCell("Email: info@mcommsolutions.co.uk");
                            table.AddCell("United Kingdom, E18 1LJ");
                            table.AddCell("VAT Reg: 182 581 101");
                            document.Add(table);
                            document.Add(new Paragraph("Commission Statement for the month of February 2024")
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFontSize(20));

                            table = new Table(new float[] { 1, 1 });
                            table.SetWidth(UnitValue.CreatePercentValue(100));
                            table.AddCell(item.ShopName);
                            table.AddCell("ShopId: LT-" + item.ShopId);
                            table.AddCell(item.Address);
                            table.AddCell("Area: " + item.AreaId);
                            table.AddCell(item.PostCode);
                            table.AddCell("Agent: " + item.UserName);
                            table.AddCell("");
                            table.AddCell("Date: " + item.TopupDate.ToString());
                            document.Add(table);

                           

                            table = new Table(new float[] { 1, 1, 1, 1 });
                            table.SetWidth(UnitValue.CreatePercentValue(100));
                            table.AddHeaderCell("Description");
                            table.AddHeaderCell("Net Commission");
                            table.AddHeaderCell("VAT");
                            table.AddHeaderCell("Total Commission");

                            var vatAmount = Convert.ToDecimal(totalCommission) * 20 / 100;
                            var netAmount = totalCommission - vatAmount;
                            table.AddCell("The Attached Commission Statement is a VAT Invoice wherein the retailer is liable to pay VAT on the commission earned");
                            table.AddCell(netAmount.ToString());
                            table.AddCell(vatAmount.ToString());
                            table.AddCell(totalCommission.ToString());

                            document.Add(table);

                        }
                    }

                    document.Close();
                    return stream.ToArray();
                }

            }

            return null;


        }

        public async Task<byte[]> GeneratePDFStatement(ICommissionStatementRepository commissionStatementRepository, GetReportRequest request)
        {
            var result = await commissionStatementRepository.GetShopListForCommission(request);
            if (result != null && result.Count() > 0)
            {
                //Define your memory stream which will temporarily hold the PDF
                using (MemoryStream stream = new MemoryStream())
                {
                    //Initialize PDF writer
                    PdfWriter writer = new PdfWriter(stream);
                    //Initialize PDF document
                    PdfDocument pdf = new PdfDocument(writer);
                    // Initialize document
                    Document document = new Document(pdf);

                    foreach (var item in result)
                    {
                        GetReportRequest request1 = new GetReportRequest();
                        request1.areaId = item.AreaId;
                        request1.shopId = item.ShopId;
                        request1.fromDate = item.TopupDate.ToString();
                        var commissionStatementData = await commissionStatementRepository.GetCommissionStatementAsync(request1);

                        decimal limitAmount = 0;
                        decimal totalCommission = commissionStatementData.Sum(s => s.Comm1 + s.Comm2);
                        int month = item.TopupDate.Month;
                        int year = item.TopupDate.Year;
                        if (((month >= 2 && year >= 2023) || year > 2023))
                        {
                            limitAmount = 10;
                        }
                        else if (((month >= 3 && year >= 2021) || year > 2021))
                        {
                            limitAmount = 5;
                        }

                        if (totalCommission >= limitAmount)
                        {
                            Table table = new Table(new float[] { 1, 1 });
                            table.SetWidth(UnitValue.CreatePercentValue(100));
                            table.AddCell("1A Victoria Road");
                            table.AddCell("London, E18 1LJ");
                            document.Add(table);
                            document.Add(new Paragraph("Commission Statement for the month of February 2024")
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFontSize(20));

                            table = new Table(new float[] { 1, 1 });
                            table.SetWidth(UnitValue.CreatePercentValue(100));
                            table.AddCell(item.ShopName);
                            table.AddCell("ShopId: LT-" + item.ShopId);
                            table.AddCell(item.Address);
                            table.AddCell("Area: " + item.AreaId);
                            table.AddCell(item.PostCode);
                            table.AddCell("Agent: " + item.UserName);
                            table.AddCell("");
                            table.AddCell("Date: " + item.TopupDate.ToString());
                            document.Add(table);



                            table = new Table(new float[] { 1, 1, 1, 1, 1, 1, 1, 1 });
                            table.SetWidth(UnitValue.CreatePercentValue(100));

                            //Cell cell11 = new Cell(1, 1).SetBackgroundColor(ColorConstants.GRAY).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("State"));

                            //main header
                            Cell cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(" "));
                            table.AddCell(cell);
                            cell = new Cell(1, 3).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("1st Topup"));
                            table.AddCell(cell);
                            cell = new Cell(1, 3).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Following Topups"));
                            table.AddCell(cell);
                            cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(" "));
                            table.AddCell(cell);

                            //sub header
                            cell = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("NETWORK"));
                            table.AddCell(cell);
                            cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("1st"));
                            table.AddCell(cell);
                            cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("RATE (£)"));
                            table.AddCell(cell);
                            cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("TOTAL (£)"));
                            table.AddCell(cell);
                            cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("TOPUPS"));
                            table.AddCell(cell);
                            cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("RATE (£)"));
                            table.AddCell(cell);
                            cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("TOTAL (£)"));
                            table.AddCell(cell);
                            cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("GRAND TOTAL (£)"));
                            table.AddCell(cell);

                            //data
                            foreach (var data in commissionStatementData)
                            {
                                cell = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(data.Network));
                                table.AddCell(cell);
                                cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(Convert.ToString(data.Conn1)));
                                table.AddCell(cell);
                                cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(Convert.ToString(data.Rate1)));
                                table.AddCell(cell);
                                cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(Convert.ToString(data.Comm1)));
                                table.AddCell(cell);
                                cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(Convert.ToString(data.Conn2)));
                                table.AddCell(cell);
                                cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(Convert.ToString(data.Rate2)));
                                table.AddCell(cell);
                                cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(Convert.ToString(data.Comm2)));
                                table.AddCell(cell);
                                cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(Convert.ToString(data.Comm1 + data.Comm2)));
                                table.AddCell(cell);
                            }

                            //last row
                            cell = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph("Total"));
                            table.AddCell(cell);
                            cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(Convert.ToString(commissionStatementData.Sum(s => s.Conn1))));
                            table.AddCell(cell);
                            cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(" "));
                            table.AddCell(cell);
                            cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(Convert.ToString(commissionStatementData.Sum(s => s.Comm1))));
                            table.AddCell(cell);
                            cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(Convert.ToString(commissionStatementData.Sum(s => s.Conn2))));
                            table.AddCell(cell);
                            cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(" "));
                            table.AddCell(cell);
                            cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(Convert.ToString(commissionStatementData.Sum(s => s.Comm2))));
                            table.AddCell(cell);
                            cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(Convert.ToString(commissionStatementData.Sum(s => s.Comm1 + s.Comm2))));
                            table.AddCell(cell);

                            document.Add(table);

                            Paragraph subheader = new Paragraph("To re-stock the sims please call : 0333-0119-880").SetTextAlignment(TextAlignment.CENTER);//.SetBold();
                            document.Add(subheader);
                            //subheader = new Paragraph("This is a Commission statement and is not a VAT document. If you are VAT registered VAT should be charged on your invoice at the appropriate rate.")
                            //    .SetTextAlignment(TextAlignment.CENTER).SetBold();
                            document.Add(subheader);

                            table = new Table(new float[] { 1, 1, 1, 1, 1, 1, 1, 1 });
                            table.SetWidth(UnitValue.CreatePercentValue(100));
                            cell = new Cell(1, 8).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(item.AreaCode + "/" + item.ShopId));
                            cell.SetPaddings(5f, 25f, 80f, 5f);
                            table.AddCell(cell);
                            cell = new Cell(1, 8).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(item.DisplayDate.ToString()));
                            cell.SetPaddings(5f, 25f, 0f, 5f);
                            table.AddCell(cell);
                            cell = new Cell(1, 8).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(item.PayableName));
                            cell.SetPaddings(15f, 0f, 0f, 30f);
                            table.AddCell(cell);

                            var firstline = "";
                            var secondLine = "";
                            var integerValue = Convert.ToInt32(totalCommission.ToString().Split('.')[0]);
                            var fractionValue = Convert.ToInt32(totalCommission.ToString().Split('.')[1]);
                            var checkText = Utility.NumberToText(integerValue, false) + " Pounds and " + Utility.NumberToText(fractionValue, false) + " Pence Only/-";
                            var textArray = checkText.Split(' ');
                            if (textArray.Count() > 9)
                            {
                                for (int i = 0; i < 9; i++)
                                {
                                    firstline += textArray[i] + " ";
                                }
                                for (int i = 9; i < textArray.Count(); i++)
                                {
                                    secondLine += textArray[i] + " ";
                                }
                            }
                            else
                            {
                                firstline = checkText;
                                secondLine = " ";
                            }

                            cell = new Cell(1, 7).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(firstline));
                            cell.SetPaddings(25f, 0f, 0f, 10f);
                            table.AddCell(cell);
                            cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(totalCommission.ToString()));
                            cell.SetPaddings(7f, 55f, 0f, 10f);
                            table.AddCell(cell);
                            cell = new Cell(1, 7).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(secondLine));
                            cell.SetPaddings(20f, 0f, 30f, 10f);
                            table.AddCell(cell);

                            

                            Image img = new Image(ImageDataFactory.Create(@"..\..\image.jpg")).SetTextAlignment(TextAlignment.CENTER);
                            cell = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(img);
                            cell.SetPaddings(20f, 10f, 10f, 0f);
                            table.AddCell(cell);
                            document.Add(table);

                        }
                    }

                    document.Close();
                    return stream.ToArray();
                }

            }

            return null;

        }
    }
}
