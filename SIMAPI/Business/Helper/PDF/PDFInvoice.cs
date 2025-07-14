//using PdfSharpCore.Drawing;
//using PdfSharpCore.Fonts;
//using PdfSharpCore.Pdf;
//using SIMAPI.Data.Models.OrderListModels;
//using SIMAPI.Repository.Interfaces;


//namespace SIMAPI.Business.Helper.PDF
//{
//    public class PDFInvoice
//    {
//        public async Task<byte[]> GenerateInvoice(IOrderRepository orderRepository, int orderId, bool IsVATInvoice)
//        {
//            InvoiceDetailModel invoiceDetails = await orderRepository.GetOrderDetailsForInvoiceByIdAsync(orderId);




//            // Register Fonts (Important for PDFsharpCore in .NET Core)

//            // Create a new PDF document
//            PdfDocument document = new PdfDocument();
//            PdfPage page = document.AddPage();
//            XGraphics gfx = XGraphics.FromPdfPage(page);

//            XFont titleFont = new XFont("Arial", 16, XFontStyle.Bold);
//            XFont headerFont = new XFont("Arial", 12, XFontStyle.Bold);
//            XFont normalFont = new XFont("Arial", 10, XFontStyle.Regular);

//            double y = 50;
//            double pageHeight = page.Height.Point - 50;

//            // **Invoice Header**
//            gfx.DrawString("INVOICE : INV116592", titleFont, XBrushes.Black, new XPoint(50, y));
//            y += 25;
//            gfx.DrawString("OrderId : 100116592", normalFont, XBrushes.Black, new XPoint(50, y));
//            y += 15;
//            gfx.DrawString("Type : Bonus", normalFont, XBrushes.Black, new XPoint(50, y));
//            y += 25;
//            gfx.DrawString("March 11 2025", normalFont, XBrushes.Black, new XPoint(50, y));
//            y += 15;
//            gfx.DrawString("Shariful/Beeston", normalFont, XBrushes.Black, new XPoint(50, y));
//            y += 30;

//            gfx.DrawString("DELIVERY NOTE", titleFont, XBrushes.Black, new XPoint(50, y));
//            y += 25;

//            // **Customer Details**
//            gfx.DrawString("Customer: 120858", headerFont, XBrushes.Black, new XPoint(50, y));
//            y += 15;
//            gfx.DrawString("Ak off-licence", normalFont, XBrushes.Black, new XPoint(50, y));
//            y += 15;
//            gfx.DrawString("267 Beeston Rd, Beeston.", normalFont, XBrushes.Black, new XPoint(50, y));
//            y += 15;
//            gfx.DrawString("Leeds, LS11 7LR", normalFont, XBrushes.Black, new XPoint(50, y));
//            y += 15;
//            gfx.DrawString("United Kingdom", normalFont, XBrushes.Black, new XPoint(50, y));
//            y += 15;
//            gfx.DrawString("01132286464994", normalFont, XBrushes.Black, new XPoint(50, y));
//            y += 25;

//            // **Table Header with Background Color**
//            double x = 50;
//            double columnWidth = 90;
//            double rowHeight = 20;

//            XBrush headerBackground = XBrushes.LightGray;
//            gfx.DrawRectangle(headerBackground, new XRect(x, y, columnWidth * 5, rowHeight));
//            gfx.DrawString("Product Code", headerFont, XBrushes.Black, new XPoint(x + 5, y + 15));
//            gfx.DrawString("Product Name", headerFont, XBrushes.Black, new XPoint(x + columnWidth, y + 15));
//            gfx.DrawString("Quantity", headerFont, XBrushes.Black, new XPoint(x + columnWidth * 2, y + 15));
//            gfx.DrawString("Price", headerFont, XBrushes.Black, new XPoint(x + columnWidth * 3, y + 15));
//            gfx.DrawString("Total", headerFont, XBrushes.Black, new XPoint(x + columnWidth * 4, y + 15));
//            y += rowHeight;

//            // **Sample Products**
//            string[,] products = new string[,] {
//            { "LT-IP-01", "1M - 3.1A CHARGING CABLE", "5", "£1.05", "£5.25" },
//            { "LT-TYPE C-01", "1M 3.1A, USB -C", "5", "£1.05", "£5.25" },
//            { "LT-PD-15", "30W-1.2M, C-IP BRAIDED DISPLAY CABLE", "2", "£2.29", "£4.58" }
//        };

//            for (int i = 0; i < products.GetLength(0); i++)
//            {
//                gfx.DrawRectangle(XPens.Black, new XRect(x, y, columnWidth * 5, rowHeight));
//                gfx.DrawString(products[i, 0], normalFont, XBrushes.Black, new XPoint(x + 5, y + 15));
//                gfx.DrawString(products[i, 1], normalFont, XBrushes.Black, new XPoint(x + columnWidth, y + 15));
//                gfx.DrawString(products[i, 2], normalFont, XBrushes.Black, new XPoint(x + columnWidth * 2, y + 15));
//                gfx.DrawString(products[i, 3], normalFont, XBrushes.Black, new XPoint(x + columnWidth * 3, y + 15));
//                gfx.DrawString(products[i, 4], normalFont, XBrushes.Black, new XPoint(x + columnWidth * 4, y + 15));
//                y += rowHeight;
//            }

//            // **Generate Byte Stream**
//            using (MemoryStream stream = new MemoryStream())
//            {
//                document.Save(stream, false);
//                return stream.ToArray();
//            }


//            //try
//            //{

//            //    // Create a new PDF document
//            //    PdfDocument document = new PdfDocument();
//            //    PdfPage page = document.AddPage();
//            //    XGraphics gfx = XGraphics.FromPdfPage(page);

//            //    // Define Fonts
//            //    XFont titleFont = new XFont("Arial", 16, XFontStyle.Bold);
//            //    XFont headerFont = new XFont("Arial", 12, XFontStyle.Bold);
//            //    XFont normalFont = new XFont("Arial", 10, XFontStyle.Regular);

//            //    double y = 50; // Starting Y position

//            //    // **Invoice Header**
//            //    gfx.DrawString($"INVOICE : INV{orderId}", titleFont, XBrushes.Black, new XPoint(50, y));
//            //    y += 25;
//            //    gfx.DrawString($"Order ID : {orderId}", normalFont, XBrushes.Black, new XPoint(50, y));
//            //    y += 15;
//            //    gfx.DrawString($"Date: {invoiceDetails.CreatedDate:dd/MM/yyyy}", normalFont, XBrushes.Black, new XPoint(50, y));
//            //    y += 15;
//            //    gfx.DrawString($"Agent: {invoiceDetails.UserName}", normalFont, XBrushes.Black, new XPoint(50, y));
//            //    y += 15;
//            //    gfx.DrawString($"Customer: {invoiceDetails.ShopName}", headerFont, XBrushes.Black, new XPoint(50, y));
//            //    y += 15;
//            //    gfx.DrawString($"Address: {invoiceDetails.ShippingAddress}", normalFont, XBrushes.Black, new XPoint(50, y));
//            //    y += 15;
//            //    gfx.DrawString($"Phone: {invoiceDetails.PhoneNumber}", normalFont, XBrushes.Black, new XPoint(50, y));
//            //    y += 25;

//            //    gfx.DrawString("DELIVERY NOTE", titleFont, XBrushes.Black, new XPoint(50, y));
//            //    y += 20;

//            //    // **Table Header**
//            //    double x = 50;
//            //    gfx.DrawString("Product Code", headerFont, XBrushes.Black, new XPoint(x, y));
//            //    gfx.DrawString("Product Name", headerFont, XBrushes.Black, new XPoint(x + 100, y));
//            //    gfx.DrawString("Quantity", headerFont, XBrushes.Black, new XPoint(x + 250, y));
//            //    gfx.DrawString("Price", headerFont, XBrushes.Black, new XPoint(x + 320, y));
//            //    gfx.DrawString("Total", headerFont, XBrushes.Black, new XPoint(x + 400, y));
//            //    y += 15;

//            //    // **Table Data**
//            //    var itemList = invoiceDetails.Items.ToList();
//            //    foreach (var item in itemList)
//            //    {
//            //        gfx.DrawString(item.ProductCode, normalFont, XBrushes.Black, new XPoint(x, y));
//            //        gfx.DrawString(item.ProductName, normalFont, XBrushes.Black, new XPoint(x + 100, y));
//            //        gfx.DrawString(item.Qty.ToString(), normalFont, XBrushes.Black, new XPoint(x + 250, y));
//            //        gfx.DrawString($"£ {item.SalePrice:F2}", normalFont, XBrushes.Black, new XPoint(x + 320, y));
//            //        gfx.DrawString($"£ {(item.Qty * item.SalePrice):F2}", normalFont, XBrushes.Black, new XPoint(x + 400, y));
//            //        y += 15;
//            //    }

//            //    // **Totals**
//            //    y += 10;
//            //    gfx.DrawString("Total Quantity:", headerFont, XBrushes.Black, new XPoint(x + 250, y));
//            //    gfx.DrawString($"{invoiceDetails.Items.Sum(s => s.Qty)}", normalFont, XBrushes.Black, new XPoint(x + 400, y));
//            //    y += 15;

//            //    gfx.DrawString("Net Amount:", headerFont, XBrushes.Black, new XPoint(x + 250, y));
//            //    gfx.DrawString($"£ {invoiceDetails.NetAmount:F2}", normalFont, XBrushes.Black, new XPoint(x + 400, y));
//            //    y += 15;

//            //    gfx.DrawString("Delivery Charges:", headerFont, XBrushes.Black, new XPoint(x + 250, y));
//            //    gfx.DrawString($"£ {invoiceDetails.DeliveryCharges:F2}", normalFont, XBrushes.Black, new XPoint(x + 400, y));
//            //    y += 15;

//            //    gfx.DrawString("Total Amount:", titleFont, XBrushes.Black, new XPoint(x + 250, y));
//            //    decimal totalAmount = invoiceDetails.IsVAT.Value == 1 ? invoiceDetails.TotalWithVATAmount.Value : invoiceDetails.TotalWithOutVATAmount.Value;
//            //    gfx.DrawString($"£ {totalAmount:F2}", titleFont, XBrushes.Black, new XPoint(x + 400, y));

//            //    // **Generate Byte Stream Instead of Saving to Disk**
//            //    using (MemoryStream stream = new MemoryStream())
//            //    {
//            //        document.Save(stream, false);
//            //        return stream.ToArray();
//            //    }
//            //}
//            //catch (Exception ex)
//            //{
//            //    return null;
//            //}
//        }


//        //public static byte[] GenerateInvoice()
//        //{
//        //    // Create a new MigraDoc document
//        //    Document document = new Document();
//        //    Section section = document.AddSection();

//        //    // Add a title
//        //    Paragraph title = section.AddParagraph("Invoice");
//        //    title.Format.Font.Size = 18;
//        //    title.Format.Font.Bold = true;
//        //    title.Format.SpaceAfter = "10pt";

//        //    // Create a table
//        //    Table table = section.AddTable();
//        //    table.Borders.Width = 0.75;

//        //    // Define columns
//        //    Column col1 = table.AddColumn(Unit.FromCentimeter(6));
//        //    Column col2 = table.AddColumn(Unit.FromCentimeter(3));
//        //    Column col3 = table.AddColumn(Unit.FromCentimeter(3));

//        //    // Add Header Row
//        //    Row headerRow = table.AddRow();
//        //    headerRow.Cells[0].AddParagraph("Item");
//        //    headerRow.Cells[1].AddParagraph("Quantity");
//        //    headerRow.Cells[2].AddParagraph("Price");

//        //    // Add Data Rows
//        //    Row row1 = table.AddRow();
//        //    row1.Cells[0].AddParagraph("Product A");
//        //    row1.Cells[1].AddParagraph("2");
//        //    row1.Cells[2].AddParagraph("$50");

//        //    Row row2 = table.AddRow();
//        //    row2.Cells[0].AddParagraph("Product B");
//        //    row2.Cells[1].AddParagraph("1");
//        //    row2.Cells[2].AddParagraph("$30");

//        //    // Generate PDF
//        //    using (MemoryStream stream = new MemoryStream())
//        //    {
//        //        PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
//        //        renderer.Document = document;
//        //        renderer.RenderDocument();
//        //        renderer.PdfDocument.Save(stream, false);
//        //        return stream.ToArray();
//        //    }
//        //}
//    }
//}




using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SIMAPI.Data.Models.OrderListModels;

namespace SIMAPI.Business.Helper.PDF
{
    public class PDFInvoice
    {
        public byte[] GenerateInvoice(InvoiceDetailModel invoiceDetailModel, bool IsVATInvoice)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    // **Header Section**
                    //page.Header().Column(col =>
                    //{
                    //    col.Item().Table(table =>
                    //    {
                    //        table.ColumnsDefinition(columns =>
                    //        {
                    //            columns.RelativeColumn(300);
                    //            columns.RelativeColumn(300);
                    //        });

                    //        if (IsVATInvoice)
                    //        {
                    //            table.Cell().Element(cell =>
                    //            {
                    //                cell.Border(0).Padding(0).Text("Leap").AlignLeft().FontSize(12).FontColor(Colors.Red.Medium);
                    //            });
                    //        }
                    //        else
                    //        {
                    //            table.Cell().Element(cell =>
                    //            {
                    //                cell.Border(0).Padding(0).Text(invoiceDetailModel.OrderPaymentType).AlignLeft().FontSize(12).FontColor(Colors.Red.Medium);
                    //            });
                    //        }
                    //        table.Cell().Element(cell =>
                    //        {
                    //            cell.Border(0).Padding(0).Text("INVOICE: INV" + invoiceDetailModel.OrderId).FontSize(12).AlignRight().FontColor(Colors.Red.Medium);
                    //        });
                    //    });

                    //    col.Item().AlignRight().Text("Order ID: 100" + invoiceDetailModel.OrderId);
                    //    col.Item().AlignRight().Text("Date: " + invoiceDetailModel.CreatedDate.ToString("MMMM dd yyyy"));
                    //    col.Item().AlignRight().Text(invoiceDetailModel.OrderPaymentType);
                    //    col.Item().AlignRight().Text(invoiceDetailModel.UserName + "/" + invoiceDetailModel.AreaName);

                    //    //row.ConstantItem(100).Image("logo.png", ImageScaling.FitWidth); // Add logo
                    //});

                    page.Content().Column(col =>
                    {
                        // Header
                        col.Item().ShowOnce().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(300);
                                columns.RelativeColumn(300);
                            });


                            table.Cell().Element(cell =>
                            {
                                cell.Border(0).Padding(0).Text(invoiceDetailModel.OrderPaymentType).AlignLeft().FontSize(12).Bold().FontColor(Colors.Red.Medium);
                            });

                            table.Cell().Element(cell =>
                            {
                                cell.Border(0).Padding(0).Text("INVOICE: INV" + invoiceDetailModel.OrderId).FontSize(12).AlignRight().FontColor(Colors.Red.Medium);
                            });
                        });

                        //col.Item().ShowOnce().AlignRight().Text("Order ID: 100" + invoiceDetailModel.OrderId);
                        col.Item().ShowOnce().PaddingTop(5).AlignRight().Text("Date: " + invoiceDetailModel.CreatedDate.ToString("MMMM dd yyyy"));
                        //col.Item().ShowOnce().PaddingTop(5).AlignRight().Text(invoiceDetailModel.OrderPaymentType);
                        col.Item().ShowOnce().PaddingTop(5).AlignRight().Text(invoiceDetailModel.UserName + "/" + invoiceDetailModel.AreaName);

                        // Line seperator
                        col.Item().ShowOnce().PaddingTop(10).PaddingBottom(10).LineHorizontal(1).LineColor(Colors.Black);

                        if (!IsVATInvoice)
                        {
                            col.Item().PaddingBottom(10).AlignCenter().Text("Delivery Note").Bold().FontSize(12);
                        }

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(300); // Customer Details
                                if (IsVATInvoice)
                                {
                                    columns.RelativeColumn(300);   // Seller Details
                                }
                            });


                            table.Header(header =>
                            {
                                header.Cell().Element(CellNoBorderStyle).Border(0).Text("Customer: "+ invoiceDetailModel.ShopId).Bold();
                                if (IsVATInvoice)
                                {
                                    header.Cell().Element(CellNoBorderStyle).Border(0).Text("Seller ").Bold();
                                }
                            });

                            table.Cell().PaddingBottom(10).Element(cell =>
                            {
                                cell.Table(innerTable =>
                                {
                                    innerTable.ColumnsDefinition(innerColumns =>
                                    {
                                        innerColumns.RelativeColumn();  // Define a single column for customer details
                                    });
                                    innerTable.Cell().Element(CellNoBorderStyle).Border(0).Text(invoiceDetailModel.ShopName).FontColor(Colors.Red.Lighten1);
                                    innerTable.Cell().Element(CellNoBorderStyle).Border(0).Text("Address:");
                                    innerTable.Cell().Element(CellNoBorderStyle).Border(0).Text(invoiceDetailModel.ShippingAddress);
                                    innerTable.Cell().Element(CellNoBorderStyle).Border(0).Text(invoiceDetailModel.ShopEmail);
                                    innerTable.Cell().Element(CellNoBorderStyle).Border(0).Text(invoiceDetailModel.PhoneNumber);
                                });
                            });
                            if (IsVATInvoice)
                            {
                                table.Cell().PaddingBottom(10).Element(cell =>
                                {
                                    cell.Table(innerTable =>
                                    {
                                        innerTable.ColumnsDefinition(innerColumns =>
                                        {
                                            innerColumns.RelativeColumn();  // Define a single column for customer details
                                        });
                                        innerTable.Cell().Element(CellNoBorderStyle).Border(0).Text("M COMM Solutions Ltd").FontColor(Colors.Red.Lighten1);
                                        innerTable.Cell().Element(CellNoBorderStyle).Border(0).Text("1A Victoria Road,");
                                        innerTable.Cell().Element(CellNoBorderStyle).Border(0).Text("United Kingdom");
                                        innerTable.Cell().Element(CellNoBorderStyle).Border(0).Text("03330119880");
                                        innerTable.Cell().Element(CellNoBorderStyle).Border(0).Text("orders@mcommsolutions.co.uk");
                                        innerTable.Cell().Element(CellNoBorderStyle).Border(0).Text("Vat No:182581101");
                                        innerTable.Cell().Element(CellNoBorderStyle).Border(0).Text("Reg No:8060121");
                                    });
                                });
                            }

                        });



                        // **Table Section**
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(100);  // Product Code
                                columns.RelativeColumn(2);   // Product Name
                                columns.ConstantColumn(80);  // Quantity
                                columns.ConstantColumn(80);  // Price
                                columns.ConstantColumn(100); // Total
                            });

                            // **Table Header**
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyleWithBackground).Text("Product Code").Bold();
                                header.Cell().Element(CellStyleWithBackground).Text("Product Name").Bold();
                                header.Cell().Element(CellStyleWithBackground).AlignCenter().Text("Quantity").Bold();
                                header.Cell().Element(CellStyleWithBackground).AlignRight().Text("Price").Bold();
                                header.Cell().Element(CellStyleWithBackground).AlignRight().Text("Total").Bold();
                            });

                            foreach (var item in invoiceDetailModel.Items)
                            {
                                table.Cell().Element(CellStyle).Text(item.ProductCode);
                                table.Cell().Element(CellStyle).Text(item.ProductName);
                                table.Cell().Element(CellStyle).AlignCenter().Text(item.Qty.ToString());
                                table.Cell().Element(CellStyle).AlignRight().Text("£ " + item.SalePrice.ToString());
                                table.Cell().Element(CellStyle).AlignRight().Text("£ " + (item.Qty * item.SalePrice).ToString());
                            }
                        });

                        col.Item().PaddingVertical(5).AlignRight().Text("Item Total: £ " + invoiceDetailModel.ItemTotal).FontSize(10).Bold();
                        col.Item().PaddingVertical(5).AlignRight().Text("Delivery Charges: £ " + invoiceDetailModel.DeliveryCharges).FontSize(10).Bold();
                        if (IsVATInvoice)
                        {
                            col.Item().PaddingVertical(5).AlignRight().Text("VAT 20%: £ " + invoiceDetailModel.VatAmount).FontSize(10).Bold();
                        }
                        if (invoiceDetailModel.DiscountAmount > 0)
                        {
                            col.Item().PaddingVertical(5).AlignRight().Text("Discount " + invoiceDetailModel.DiscountPercentage + "% : £ " + invoiceDetailModel.DiscountAmount).FontSize(10).Bold().FontColor(Colors.Orange.Medium);
                        }
                        col.Item().PaddingVertical(5).AlignRight().Text("Total Amount: £ " + (IsVATInvoice ? invoiceDetailModel.TotalWithVATAmount : invoiceDetailModel.TotalWithOutVATAmount)).FontSize(10).Bold();
                    });

                    // Footer Section
                    if (IsVATInvoice)
                    {
                        page.Footer().AlignCenter().Text("Any shortages must be notified immediately, Title of goods remain the property of M Comm Solutions Limited until payment is received in full.");
                    }
                    else
                    {
                        page.Footer().AlignCenter().Text("Any shortages must be notified immediately.");
                    }
                });
            }).GeneratePdf();
        }

        private static IContainer CellStyle(IContainer container) =>
            container.Border(1, Unit.Point).Padding(5).AlignMiddle();

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
    }
}

