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
                                cell.Border(0).Padding(0).Text(invoiceDetailModel.OrderPaymentType).AlignLeft().FontSize(14).Bold().FontColor(Colors.Red.Medium);
                            });

                            table.Cell().Element(cell =>
                            {
                                cell.Border(0).Padding(0).Text("INV" + invoiceDetailModel.OrderId).FontSize(14).Bold().AlignRight().FontColor(Colors.Red.Medium);
                            });
                        });

                        //col.Item().ShowOnce().AlignRight().Text("Order ID: 100" + invoiceDetailModel.OrderId);
                        col.Item().ShowOnce().PaddingTop(5).AlignRight().Text(invoiceDetailModel.CreatedDate.ToString("MMMM dd yyyy"));
                        //col.Item().ShowOnce().PaddingTop(5).AlignRight().Text(invoiceDetailModel.OrderPaymentType);
                        col.Item().ShowOnce().PaddingTop(5).AlignRight().Text(invoiceDetailModel.UserName + "/" + invoiceDetailModel.AreaName);

                        // Line seperator
                        col.Item().ShowOnce().PaddingTop(10).PaddingBottom(10).LineHorizontal(1).LineColor(Colors.Black);

                        if (!IsVATInvoice)
                        {
                            col.Item().PaddingBottom(10).AlignCenter().Text("DELIVERY NOTE").Bold().FontSize(14);
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
                                header.Cell().Element(CellNoBorderStyle).Border(0).Text("Customer: " + (invoiceDetailModel.OldShopId ?? invoiceDetailModel.ShopId)).Bold();
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
                                    innerTable.Cell().Element(CellNoBorderStyle).Border(0).Text(invoiceDetailModel.ShopName).Bold().FontColor(Colors.Red.Lighten1);
                                    innerTable.Cell().Element(CellNoBorderStyle).Border(0).Text(invoiceDetailModel.ContactName);
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
                                columns.RelativeColumn(3);   // Product Name
                                columns.ConstantColumn(80);  // Quantity
                                columns.ConstantColumn(80);  // Price
                                columns.ConstantColumn(80); // Total
                            });

                            // **Table Header**
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyleWithBackground).Text("Product Code").Bold();
                                header.Cell().Element(CellStyleWithBackground).Text("Product Name").Bold();
                                header.Cell().Element(CellStyleWithBackground).AlignCenter().Text("Quantity").Bold();
                                header.Cell().Element(CellStyleWithBackground).AlignCenter().Text("Price").Bold();
                                header.Cell().Element(CellStyleWithBackground).AlignCenter().Text("Total").Bold();
                            });

                            foreach (var item in invoiceDetailModel.Items)
                            {
                                if (item.IsBundle == 1)
                                {
                                    table.Cell().Element(CellStyle).Text(item.ProductCode).Bold();
                                    table.Cell().Element(CellStyle).Text(item.ProductName).Bold();
                                    table.Cell().Element(CellStyle).AlignCenter().Text(item.Qty.ToString()).Bold();
                                    table.Cell().Element(CellStyle).AlignRight().Text("£ " + item.SalePrice.ToString()).Bold();
                                    table.Cell().Element(CellStyle).AlignRight().Text("£ " + (item.Qty * item.SalePrice).ToString()).Bold();
                                }
                                else
                                {
                                    table.Cell().Element(CellStyle).Text(item.ProductCode);
                                    table.Cell().Element(CellStyle).Text(item.ProductName);
                                    table.Cell().Element(CellStyle).AlignCenter().Text(item.Qty.ToString());
                                    table.Cell().Element(CellStyle).AlignRight().Text("£ " + item.SalePrice.ToString());
                                    table.Cell().Element(CellStyle).AlignRight().Text("£ " + (item.Qty * item.SalePrice).ToString());
                                }
                            }
                        });

                        //Total table
                        col.Item().PaddingTop(20).AlignRight().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(120);
                                columns.ConstantColumn(120);
                            });


                            table.Cell().Element(CellStyle).AlignRight().Text("Item Total").Bold();
                            table.Cell().Element(CellStyle).AlignRight().Text("£ " + invoiceDetailModel.ItemTotal).Bold();

                            table.Cell().Element(CellStyle).AlignRight().Text("Delivery Charges").Bold();
                            table.Cell().Element(CellStyle).AlignRight().Text("£ " + invoiceDetailModel.DeliveryCharges).Bold();

                            if (IsVATInvoice)
                            {
                                table.Cell().Element(CellStyle).AlignRight().Text("VAT 20%").Bold();
                                table.Cell().Element(CellStyle).AlignRight().Text("£ " + invoiceDetailModel.VatAmount).Bold();
                            }
                            if (invoiceDetailModel.DiscountAmount > 0)
                            {
                                table.Cell().Element(CellStyle).AlignRight().Text("Discount").Bold();
                                table.Cell().Element(CellStyle).AlignRight().Text("£ " + invoiceDetailModel.DiscountPercentage + "% : £ " + invoiceDetailModel.DiscountAmount).Bold();
                            }
                            table.Cell().Element(CellStyle).AlignRight().Text("Total Amount").Bold();
                            table.Cell().Element(CellStyle).AlignRight().Text("£ " + (IsVATInvoice ? invoiceDetailModel.TotalWithVATAmount : invoiceDetailModel.TotalWithOutVATAmount)).Bold();
                        });
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

        public byte[] GenerateReceipt(PaymentReceiptModel model)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(35);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    // HEADER SECTION
                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("LEAP-TEL")
                                .FontSize(20)
                                .Bold()
                                .FontColor(Colors.Blue.Medium);

                            col.Item().Text("Telecom Solutions");
                            col.Item().Text("Customer Support: 03330119880");
                            col.Item().Text("info@leap-tel.com");
                        });

                        row.RelativeItem().AlignRight().Column(col =>
                        {
                            col.Item().Text("PAYMENT RECEIPT")
                                .FontSize(18)
                                .Bold();

                            col.Item().PaddingTop(5)
                                .Text($"Receipt No : {model.ReceiptNo}");

                            col.Item()
                                .Text($"Date : {model.PaymentDate:dd MMM yyyy}");
                        });
                    });

                    page.Content().Column(col =>
                    {
                        col.Item().PaddingVertical(10).LineHorizontal(1);

                        // CUSTOMER INFO BLOCK
                        col.Item().PaddingVertical(5).Text("Customer Details")
                            .Bold()
                            .FontSize(13);

                        col.Item().Text($"Customer Name : {model.CustomerName}");
                        col.Item().Text($"Order ID : {model.OrderId}");
                        col.Item().Text($"Payment Method : {model.PaymentMethod}");

                        col.Item().PaddingVertical(10).LineHorizontal(1);

                        // PAYMENT SUMMARY TABLE STYLE
                        col.Item().PaddingVertical(5).Text("Payment Summary")
                            .Bold()
                            .FontSize(13);

                        col.Item().Border(1).Padding(10).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.ConstantColumn(120);
                            });

                            table.Cell().Text("Description").Bold();
                            table.Cell().AlignRight().Text("Amount").Bold();

                            table.Cell().Text("Payment Received");
                            table.Cell().AlignRight().Text($"£ {model.AmountPaid:F2}");
                        });


                        // SIGNATURE AREA
                        col.Item().AlignRight().Column(sig =>
                        {
                            sig.Item().Text("Authorized Signature");
                            sig.Item().PaddingTop(10)
                                .Text("____________________________");
                        });
                    });

                    // FOOTER SECTION
                    page.Footer().AlignCenter().Column(col =>
                    {
                        col.Item().Text("This is a system generated receipt and does not require a physical signature.")
                            .FontSize(9)
                            .FontColor(Colors.Grey.Darken2);
                    });
                });
            })
            .GeneratePdf();

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

