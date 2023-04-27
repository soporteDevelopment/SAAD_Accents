using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.IO;
using ADSystem.Helpers;
using ADEntities.Queries;
using ADEntities.ViewModels;

namespace ADSystem.Helpers
{
    public class PDFGenerator
    {
        const string LabelFormat = "$#,##0.00";
        const float ValueHundred = 100;
        const string ISNULL = "NULL";

        public MemoryStream GenerateSale(int id)
        {
            Sales _sales = new Sales();
            var sale = _sales.GetSaleForIdSale(id);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                var hp = new HelperTextSharp();

                Document pdfDoc = new Document(PageSize.LETTER, 50f, 50f, 10f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                pdfDoc.Open();

                string imageURL = System.Web.HttpContext.Current.Server.MapPath("~") + "content\\images\\logo_des.png";

                iTextSharp.text.Image logo_accents = iTextSharp.text.Image.GetInstance(imageURL);
                logo_accents.ScaleToFit(100f, 50f);

                PdfPTable tableLogo = new PdfPTable(3) { WidthPercentage = ValueHundred, SpacingAfter = 20 };

                //LOGO START
                tableLogo.AddCell(new PdfPCell(new Phrase("", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableLogo.AddCell(new PdfPCell(logo_accents)
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableLogo.AddCell(new PdfPCell(new Phrase("Remision:" + sale.Remision, hp.fontRedCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                pdfDoc.Add(tableLogo);

                PdfPTable tableHeader = new PdfPTable(3) { WidthPercentage = ValueHundred };

                //SELLER
                tableHeader.AddCell(new PdfPCell(new Phrase("Sucursal:" + sale.Sucursal, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableHeader.AddCell(new PdfPCell(new Phrase("Vendedor:" + sale.Usuario1, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableHeader.AddCell(new PdfPCell(new Phrase("Fecha:" + sale.Fecha.Value.ToString("dd/MM/yyyy"), hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                pdfDoc.Add(tableHeader);

                //CUSTOMER
                PdfPTable tableCustomer = new PdfPTable(2) { WidthPercentage = ValueHundred, SpacingAfter = 10 };

                //CUSTOMER NAME
                string customerName = String.Empty;

                switch (sale.TipoCliente)
                {
                    case (0):
                        customerName = sale.oClienteFisico.Nombre + " " + sale.oClienteFisico.Apellidos;
                        break;
                    case (1):
                        customerName = sale.oClienteMoral.Nombre;
                        break;
                    case (2):
                        customerName = sale.Despacho;
                        break;
                }

                tableCustomer.AddCell(new PdfPCell(new Phrase("Cliente:" + customerName, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Móvil:" + sale.oAddress.TelCelular, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Teléfono:" + sale.oAddress.TelCasa, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Dirección:" + sale.oAddress.Direccion, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Correo:" + sale.oAddress.Correo, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Proyecto:" + sale.Proyecto, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                pdfDoc.Add(tableCustomer);

                float[] columnPayment = { 33, 33, 33 };
                PdfPTable tablePayment = new PdfPTable(columnPayment)
                {
                    WidthPercentage = ValueHundred,
                    SpacingAfter = 10
                };

                tablePayment.AddCell(new PdfPCell(new Phrase("FORMA DE PAGO", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Colspan = 4
                });

                tablePayment.AddCell(new PdfPCell(new Phrase("Forma/Pago", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tablePayment.AddCell(new PdfPCell(new Phrase("Monto", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tablePayment.AddCell(new PdfPCell(new Phrase("Fecha", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                var payments = _sales.GetTypePaymentForPrint(id);

                foreach (var payment in payments)
                {
                    if (payment.amount > 0)
                    {
                        tablePayment.AddCell(new PdfPCell(new Phrase(payment.sTypePayment, hp.fontCell))
                        {
                            HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                            BackgroundColor = new BaseColor(System.Drawing.Color.White)
                        });

                        tablePayment.AddCell(new PdfPCell(new Phrase(new Phrase(Math.Round(payment.amount + (payment.amountIVA ?? 0), 2).ToString(LabelFormat), hp.fontCell)))
                        {
                            HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                            BackgroundColor = new BaseColor(System.Drawing.Color.White)
                        });

                        if (payment.DatePayment != null)
                        {
                            tablePayment.AddCell(new PdfPCell(new Phrase(payment.DatePayment.Value.ToString("dd/MM/yyyy"), hp.fontCell))
                            {
                                HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                                BackgroundColor = new BaseColor(System.Drawing.Color.White)
                            });
                        }
                        else
                        {
                            tablePayment.AddCell(new PdfPCell(new Phrase("", hp.fontCell))
                            {
                                HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                                BackgroundColor = new BaseColor(System.Drawing.Color.White)
                            });
                        }
                    }
                }

                pdfDoc.Add(tablePayment);

                float[] columnDetail = { 25, 15, 20, 10, 10, 10, 10 };

                PdfPTable tableSaleDetail = new PdfPTable(columnDetail)
                {
                    WidthPercentage = ValueHundred
                };

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("DETALLE DE LA VENTA", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Colspan = 7
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Imagen", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Código", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Descripción", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Precio", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Cantidad", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Descuento", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Total", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                foreach (var item in sale.oDetail)
                {
                    string image_URL = System.Web.HttpContext.Current.Server.MapPath("~") + "content\\images\\not_found.png";

                    if (item.oProducto != null && item.TipoImagen == 1)
                    {
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("~") + "content\\products\\" + item.oProducto.NombreImagen))
                        {
                            image_URL = System.Web.HttpContext.Current.Server.MapPath("~") + "content\\products\\" + item.oProducto.NombreImagen;
                        }
                    }
                    else if (!String.IsNullOrEmpty(item.Imagen))
                    {
                        if (item.Imagen.ToUpper() != ISNULL)
                        {
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("~") + "content\\services\\" + item.Imagen))
                            {
                                image_URL = System.Web.HttpContext.Current.Server.MapPath("~") + "content\\services\\" + item.Imagen;
                            }
                        }
                    }

                    iTextSharp.text.Image imagen_item = iTextSharp.text.Image.GetInstance(image_URL);
                    imagen_item.ScaleToFit(70f, 70f);
                    imagen_item.SpacingBefore = 2f;
                    imagen_item.SpacingAfter = 2f;
                    imagen_item.Alignment = Element.ALIGN_CENTER;

                    tableSaleDetail.AddCell(new PdfPCell(imagen_item)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    tableSaleDetail.AddCell(new PdfPCell(new Phrase(item.Codigo, hp.fontCell))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    tableSaleDetail.AddCell(new PdfPCell(new Phrase(item.Descripcion, hp.fontCell))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    tableSaleDetail.AddCell(new PdfPCell(new Phrase(new Phrase(Math.Round(item.Precio ?? 0, 2).ToString(LabelFormat), hp.fontCell)))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    tableSaleDetail.AddCell(new PdfPCell(new Phrase((item.Cantidad ?? 0).ToString(), hp.fontCell))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    if ((item.Descuento ?? 0) > 0)
                    {
                        tableSaleDetail.AddCell(new PdfPCell(new Phrase((item.Descuento ?? 0).ToString() + "%", hp.fontCell))
                        {
                            HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                            BackgroundColor = new BaseColor(System.Drawing.Color.White)
                        });
                    }
                    else
                    {
                        tableSaleDetail.AddCell(new PdfPCell(new Phrase("", hp.fontCell))
                        {
                            HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                            BackgroundColor = new BaseColor(System.Drawing.Color.White)
                        });
                    }

                    var total = (item.Precio ?? 0) * (item.Cantidad ?? 0) - (((item.Precio ?? 0) * (item.Cantidad ?? 0)) * ((item.Descuento ?? 0) / 100));

                    tableSaleDetail.AddCell(new PdfPCell(new Phrase(new Phrase(Math.Round(total, 2).ToString(LabelFormat), hp.fontCell)))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    if (!String.IsNullOrEmpty(item.Comentarios))
                    {
                        tableSaleDetail.AddCell(new PdfPCell(new Phrase(new Phrase("COMENTARIOS:" + item.Comentarios, hp.fontCell)))
                        {
                            HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                            BackgroundColor = new BaseColor(System.Drawing.Color.White),
                            Colspan = 7
                        });
                    }
                }

                pdfDoc.Add(tableSaleDetail);

                PdfPTable tableTotal = new PdfPTable(6)
                {
                    WidthPercentage = ValueHundred
                };

                tableTotal.AddCell(new PdfPCell(new Phrase("Cantidad de Productos", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                    Colspan = 2
                });

                tableTotal.AddCell(new PdfPCell(new Phrase((sale.CantidadProductos ?? 0).ToString(), hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableTotal.AddCell(new PdfPCell(new Phrase("Subtotal", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                    Colspan = 2
                });

                tableTotal.AddCell(new PdfPCell(new Phrase(new Phrase(Math.Round(sale.Subtotal ?? 0, 2).ToString(LabelFormat), hp.fontCell)))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                });

                //DISCOUNT
                if (sale.Descuento > 0)
                {
                    tableTotal.AddCell(new PdfPCell(new Phrase("Descuento", hp.fontHeader))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White),
                        Border = Rectangle.NO_BORDER,
                        Colspan = 5
                    });

                    tableTotal.AddCell(new PdfPCell(new Phrase(new Phrase((sale.Descuento ?? 0).ToString() + "%", hp.fontCell)))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White),
                        Border = Rectangle.NO_BORDER,
                    });
                }

                tableTotal.AddCell(new PdfPCell(new Phrase("Total", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                    Colspan = 5
                });

                tableTotal.AddCell(new PdfPCell(new Phrase(new Phrase(Math.Round(sale.Total ?? 0, 2).ToString(LabelFormat), hp.fontCell)))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                pdfDoc.Add(tableTotal);

                pdfDoc.Add(new Phrase(Environment.NewLine));

                PdfPTable tableTerms = new PdfPTable(1)
                {
                    WidthPercentage = ValueHundred
                };

                tableTerms.AddCell(new PdfPCell(new Phrase("Cuenta con 5 días para solicitar su factura.", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableTerms.AddCell(new PdfPCell(new Phrase("Nuestra Política de Devolución  se encuentra impresa en la RECEPCIÓN de cada una de nuestras sucursales.", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableTerms.AddCell(new PdfPCell(new Phrase("Precios sujetos a cambio sin previo aviso, en productos importados o sobre pedido.", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableTerms.AddCell(new PdfPCell(new Phrase("En pedidos de diseños especiales proporcionados por el cliente no nos hacemos responsables por fallas de cálculo o de ergonomía, los diseños que los clientes nos proveen son totalmente bajo su riesgo y responsabilidad y deberán ser pagados al 100% por el cliente antes de la entrega, cualquier cambio o modificación al diseño ya aceptado se cobrará como extra. Las garantías de fabricación varían según el proveedor y serán estipuladas en una carta cuando el cliente lo solicite a la hora de la compra.", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableTerms.AddCell(new PdfPCell(new Phrase("Al pagar este pedido (producto) usted conoce y acepta las especificaciones del producto/servicio así como también los términos y condiciones.", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });
                pdfDoc.Add(tableTerms);

                pdfDoc.Add(new Phrase(Environment.NewLine));

                PdfPTable tableFooter = new PdfPTable(1)
                {
                    WidthPercentage = ValueHundred
                };

                tableFooter.AddCell(new PdfPCell(new Phrase("AMAZONAS #305 OTE. COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL.66220 MÉXICO. T: (81) 8356-9558 / (81) 8335-0321", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableFooter.AddCell(new PdfPCell(new Phrase("RIO GUADALQUIVIR #13. ESQ. CALZADA SAN PEDRO COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL. 66220 MÉXICO. T: (81) 8335-5591", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableFooter.AddCell(new PdfPCell(new Phrase("AMAZONAS #320 PLAZA AMAZONAS COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL. 66220 MÉXICO. T: (81) 8335-0042", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableFooter.AddCell(new PdfPCell(new Phrase("WWW.ACCENTSDECORATION.COM", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                pdfDoc.Add(tableFooter);

                pdfDoc.Close();
                writer.Flush();

                return memoryStream;
            }
        }

        public MemoryStream GenerateSaleWithPayment(int id, string paymentType, string cardType, string check, int? idCreditNote, decimal amount, decimal left)
        {
            Sales _sales = new Sales();
            var sale = _sales.GetSaleForIdSale(id);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                var hp = new HelperTextSharp();

                Document pdfDoc = new Document(PageSize.LETTER, 50f, 50f, 10f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                pdfDoc.Open();

                string imageURL = System.Web.HttpContext.Current.Server.MapPath("~") + "content\\images\\logo_des.png";

                iTextSharp.text.Image logo_accents = iTextSharp.text.Image.GetInstance(imageURL);
                logo_accents.ScaleToFit(100f, 50f);

                PdfPTable tableLogo = new PdfPTable(3) { WidthPercentage = ValueHundred, SpacingAfter = 20 };

                //LOGO START
                tableLogo.AddCell(new PdfPCell(new Phrase("", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableLogo.AddCell(new PdfPCell(logo_accents)
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableLogo.AddCell(new PdfPCell(new Phrase("Remision:" + sale.Remision, hp.fontRedCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                pdfDoc.Add(tableLogo);

                PdfPTable tableHeader = new PdfPTable(3) { WidthPercentage = ValueHundred };

                //SELLER
                tableHeader.AddCell(new PdfPCell(new Phrase("Sucursal:" + sale.Sucursal, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableHeader.AddCell(new PdfPCell(new Phrase("Vendedor:" + sale.Usuario1, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableHeader.AddCell(new PdfPCell(new Phrase("Fecha:" + sale.Fecha.Value.ToString("dd/MM/yyyy"), hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                pdfDoc.Add(tableHeader);

                //CUSTOMER
                PdfPTable tableCustomer = new PdfPTable(2) { WidthPercentage = ValueHundred, SpacingAfter = 10 };

                //CUSTOMER NAME
                string customerName = String.Empty;

                switch (sale.TipoCliente)
                {
                    case (0):
                        customerName = sale.oClienteFisico.Nombre + " " + sale.oClienteFisico.Apellidos;
                        break;
                    case (1):
                        customerName = sale.oClienteMoral.Nombre;
                        break;
                    case (2):
                        customerName = sale.Despacho;
                        break;
                }

                tableCustomer.AddCell(new PdfPCell(new Phrase("Cliente:" + customerName, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Móvil:" + sale.oAddress.TelCelular, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Teléfono:" + sale.oAddress.TelCasa, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Dirección:" + sale.oAddress.Direccion, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Correo:" + sale.oAddress.Correo, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Proyecto:" + sale.Proyecto, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                pdfDoc.Add(tableCustomer);

                float[] columnPayment = { 25, 25, 25, 25 };
                PdfPTable tablePayment = new PdfPTable(columnPayment)
                {
                    WidthPercentage = ValueHundred,
                    SpacingAfter = 10
                };

                tablePayment.AddCell(new PdfPCell(new Phrase("ABONO", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Colspan = 4
                });

                tablePayment.AddCell(new PdfPCell(new Phrase("Forma/Pago", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tablePayment.AddCell(new PdfPCell(new Phrase("Detalle", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tablePayment.AddCell(new PdfPCell(new Phrase("Monto", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tablePayment.AddCell(new PdfPCell(new Phrase("Restante", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tablePayment.AddCell(new PdfPCell(new Phrase(paymentType, hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                if (!String.IsNullOrEmpty(check))
                {
                    tablePayment.AddCell(new PdfPCell(new Phrase(check, hp.fontCell))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });
                }
                else if (idCreditNote != null)
                {
                    var credits = new Credits();

                    var creditNote = credits.GetSingleCreditForRemision(idCreditNote ?? 0);
                    tablePayment.AddCell(new PdfPCell(new Phrase(creditNote.RemisionCredito, hp.fontCell))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });
                }
                else
                {
                    tablePayment.AddCell(new PdfPCell(new Phrase("", hp.fontCell))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });
                }

                tablePayment.AddCell(new PdfPCell(new Phrase(new Phrase(Math.Round(amount, 2).ToString(LabelFormat), hp.fontCell)))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tablePayment.AddCell(new PdfPCell(new Phrase(new Phrase(Math.Round(left, 2).ToString(LabelFormat), hp.fontCell)))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                pdfDoc.Add(tablePayment);

                float[] columnDetail = { 25, 15, 20, 10, 10, 10, 10 };

                PdfPTable tableSaleDetail = new PdfPTable(columnDetail)
                {
                    WidthPercentage = ValueHundred
                };

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("DETALLE DE LA VENTA", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Colspan = 7
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Imagen", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Código", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Descripción", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Precio", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Cantidad", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Descuento", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Total", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                foreach (var item in sale.oDetail)
                {
                    string image_URL = System.Web.HttpContext.Current.Server.MapPath("~") + "content\\images\\not_found.png";

                    if (item.oProducto != null && item.TipoImagen == 1)
                    {
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("~") + "content\\products\\" + item.oProducto.NombreImagen))
                        {
                            image_URL = System.Web.HttpContext.Current.Server.MapPath("~") + "content\\products\\" + item.oProducto.NombreImagen;
                        }
                    }
                    else if (!String.IsNullOrEmpty(item.Imagen))
                    {
                        if (item.Imagen.ToUpper() != ISNULL)
                        {
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("~") + "content\\services\\" + item.Imagen))
                            {
                                image_URL = System.Web.HttpContext.Current.Server.MapPath("~") + "content\\services\\" + item.Imagen;
                            }
                        }
                    }

                    iTextSharp.text.Image imagen_item = iTextSharp.text.Image.GetInstance(image_URL);
                    imagen_item.ScaleToFit(70f, 70f);
                    imagen_item.SpacingBefore = 2f;
                    imagen_item.SpacingAfter = 2f;
                    imagen_item.Alignment = Element.ALIGN_CENTER;

                    tableSaleDetail.AddCell(new PdfPCell(imagen_item)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    tableSaleDetail.AddCell(new PdfPCell(new Phrase(item.Codigo, hp.fontCell))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    tableSaleDetail.AddCell(new PdfPCell(new Phrase(item.Descripcion, hp.fontCell))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    tableSaleDetail.AddCell(new PdfPCell(new Phrase(new Phrase(Math.Round(item.Precio ?? 0, 2).ToString(LabelFormat), hp.fontCell)))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    tableSaleDetail.AddCell(new PdfPCell(new Phrase((item.Cantidad ?? 0).ToString(), hp.fontCell))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    if ((item.Descuento ?? 0) > 0)
                    {
                        tableSaleDetail.AddCell(new PdfPCell(new Phrase((item.Descuento ?? 0).ToString() + "%", hp.fontCell))
                        {
                            HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                            BackgroundColor = new BaseColor(System.Drawing.Color.White)
                        });
                    }
                    else
                    {
                        tableSaleDetail.AddCell(new PdfPCell(new Phrase("", hp.fontCell))
                        {
                            HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                            BackgroundColor = new BaseColor(System.Drawing.Color.White)
                        });
                    }

                    var total = (item.Precio ?? 0) * (item.Cantidad ?? 0) - (((item.Precio ?? 0) * (item.Cantidad ?? 0)) * ((item.Descuento ?? 0) / 100));

                    tableSaleDetail.AddCell(new PdfPCell(new Phrase(new Phrase(Math.Round(total, 2).ToString(LabelFormat), hp.fontCell)))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    if (!String.IsNullOrEmpty(item.Comentarios))
                    {
                        tableSaleDetail.AddCell(new PdfPCell(new Phrase(new Phrase("COMENTARIOS:" + item.Comentarios, hp.fontCell)))
                        {
                            HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                            BackgroundColor = new BaseColor(System.Drawing.Color.White),
                            Colspan = 7
                        });
                    }
                }

                pdfDoc.Add(tableSaleDetail);

                PdfPTable tableTotal = new PdfPTable(6)
                {
                    WidthPercentage = ValueHundred
                };

                tableTotal.AddCell(new PdfPCell(new Phrase("Cantidad de Productos", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                    Colspan = 2
                });

                tableTotal.AddCell(new PdfPCell(new Phrase((sale.CantidadProductos ?? 0).ToString(), hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableTotal.AddCell(new PdfPCell(new Phrase("Subtotal", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                    Colspan = 2
                });

                tableTotal.AddCell(new PdfPCell(new Phrase(new Phrase(Math.Round(sale.Subtotal ?? 0, 2).ToString(LabelFormat), hp.fontCell)))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                });

                //DISCOUNT
                if (sale.Descuento > 0)
                {
                    tableTotal.AddCell(new PdfPCell(new Phrase("Descuento", hp.fontHeader))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White),
                        Border = Rectangle.NO_BORDER,
                        Colspan = 5
                    });

                    tableTotal.AddCell(new PdfPCell(new Phrase(new Phrase((sale.Descuento ?? 0).ToString() + "%", hp.fontCell)))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White),
                        Border = Rectangle.NO_BORDER,
                    });
                }

                tableTotal.AddCell(new PdfPCell(new Phrase("Total", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                    Colspan = 5
                });

                tableTotal.AddCell(new PdfPCell(new Phrase(new Phrase(Math.Round(sale.Total ?? 0, 2).ToString(LabelFormat), hp.fontCell)))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                pdfDoc.Add(tableTotal);

                pdfDoc.Add(new Phrase(Environment.NewLine));

                PdfPTable tableTerms = new PdfPTable(1)
                {
                    WidthPercentage = ValueHundred
                };

                tableTerms.AddCell(new PdfPCell(new Phrase("Cuenta con 5 días para solicitar su factura.", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableTerms.AddCell(new PdfPCell(new Phrase("Nuestra Política de Devolución  se encuentra impresa en la RECEPCIÓN de cada una de nuestras sucursales.", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableTerms.AddCell(new PdfPCell(new Phrase("Precios sujetos a cambio sin previo aviso, en productos importados o sobre pedido.", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableTerms.AddCell(new PdfPCell(new Phrase("En pedidos de diseños especiales proporcionados por el cliente no nos hacemos responsables por fallas de cálculo o de ergonomía, los diseños que los clientes nos proveen son totalmente bajo su riesgo y responsabilidad y deberán ser pagados al 100% por el cliente antes de la entrega, cualquier cambio o modificación al diseño ya aceptado se cobrará como extra. Las garantías de fabricación varían según el proveedor y serán estipuladas en una carta cuando el cliente lo solicite a la hora de la compra.", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableTerms.AddCell(new PdfPCell(new Phrase("Al pagar este pedido (producto) usted conoce y acepta las especificaciones del producto/servicio así como también los términos y condiciones.", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                pdfDoc.Add(tableTerms);

                pdfDoc.Add(new Phrase(Environment.NewLine));

                PdfPTable tableFooter = new PdfPTable(1)
                {
                    WidthPercentage = ValueHundred
                };

                tableFooter.AddCell(new PdfPCell(new Phrase("AMAZONAS #305 OTE. COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL.66220 MÉXICO. T: (81) 8356-9558 / (81) 8335-0321", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableFooter.AddCell(new PdfPCell(new Phrase("RIO GUADALQUIVIR #13. ESQ. CALZADA SAN PEDRO COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL. 66220 MÉXICO. T: (81) 8335-5591", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableFooter.AddCell(new PdfPCell(new Phrase("AMAZONAS #320 PLAZA AMAZONAS COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL. 66220 MÉXICO. T: (81) 8335-0042", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableFooter.AddCell(new PdfPCell(new Phrase("WWW.ACCENTSDECORATION.COM", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                pdfDoc.Add(tableFooter);

                pdfDoc.Close();
                writer.Flush();

                return memoryStream;
            }
        }

        public MemoryStream GenerateQuotation(int id)
        {
            Quotations _quotation = new Quotations();
            var quotation = _quotation.GetQuotationId(id);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                var hp = new HelperTextSharp();

                Document pdfDoc = new Document(PageSize.LETTER, 50f, 50f, 10f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                pdfDoc.Open();

                string imageURL = System.Web.HttpContext.Current.Server.MapPath("~") + "content\\images\\logo_des.png";

                iTextSharp.text.Image logo_accents = iTextSharp.text.Image.GetInstance(imageURL);
                logo_accents.ScaleToFit(100f, 50f);

                PdfPTable tableLogo = new PdfPTable(3) { WidthPercentage = ValueHundred, SpacingAfter = 20 };

                //LOGO START
                tableLogo.AddCell(new PdfPCell(new Phrase("", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableLogo.AddCell(new PdfPCell(logo_accents)
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableLogo.AddCell(new PdfPCell(new Phrase("Cotización:" + quotation.Numero, hp.fontRedCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                pdfDoc.Add(tableLogo);

                PdfPTable tableHeader = new PdfPTable(3) { WidthPercentage = ValueHundred };

                //SELLER
                tableHeader.AddCell(new PdfPCell(new Phrase("Sucursal:" + quotation.Sucursal, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableHeader.AddCell(new PdfPCell(new Phrase("Vendedor:" + quotation.Usuario1, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableHeader.AddCell(new PdfPCell(new Phrase("Fecha:" + quotation.Fecha.Value.ToString("dd/MM/yyyy"), hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                pdfDoc.Add(tableHeader);

                //CUSTOMER
                PdfPTable tableCustomer = new PdfPTable(2) { WidthPercentage = ValueHundred, SpacingAfter = 10 };

                //CUSTOMER NAME
                string customerName;

                if (!String.IsNullOrEmpty(quotation.ClienteFisico))
                {
                    customerName = quotation.ClienteFisico;
                }
                else if (!String.IsNullOrEmpty(quotation.ClienteMoral))
                {
                    customerName = quotation.ClienteMoral;
                }
                else
                {
                    customerName = quotation.Despacho;
                }

                tableCustomer.AddCell(new PdfPCell(new Phrase("Cliente:" + customerName, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Móvil:" + quotation.oAddress.TelCelular, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Teléfono:" + quotation.oAddress.TelCasa, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Dirección:" + quotation.oAddress.Direccion, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Correo:" + quotation.oAddress.Correo, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Proyecto:" + quotation.Proyecto, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                pdfDoc.Add(tableCustomer);

                float[] columnDetail = { 25, 15, 20, 10, 10, 10, 10 };
                PdfPTable tableSaleDetail = new PdfPTable(columnDetail)
                {
                    WidthPercentage = ValueHundred
                };

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("DETALLE DE LA COTIZACIÓN", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Colspan = 7
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Imagen", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Código", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Descripción", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Precio", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Cantidad", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Descuento", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Total", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                foreach (var item in quotation.oDetail)
                {
                    string image_URL = System.Web.HttpContext.Current.Server.MapPath("~") + "content\\images\\not_found.png";

                    if (item.oProducto != null && item.TipoImagen == 1)
                    {
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("~") + "content\\products\\" + item.oProducto.NombreImagen))
                        {
                            image_URL = System.Web.HttpContext.Current.Server.MapPath("~") + "content\\products\\" + item.oProducto.NombreImagen;
                        }
                    }
                    else if (!String.IsNullOrEmpty(item.Imagen))
                    {
                        if (item.Imagen.ToUpper() != ISNULL)
                        {
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("~") + "content\\services\\" + item.Imagen))
                            {
                                image_URL = System.Web.HttpContext.Current.Server.MapPath("~") + "content\\services\\" + item.Imagen;
                            }
                        }
                    }

                    iTextSharp.text.Image imagen_item = iTextSharp.text.Image.GetInstance(image_URL);
                    imagen_item.ScaleToFit(70f, 70f);
                    imagen_item.SpacingBefore = 2f;
                    imagen_item.SpacingAfter = 2f;
                    imagen_item.Alignment = Element.ALIGN_CENTER;

                    tableSaleDetail.AddCell(new PdfPCell(imagen_item)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    tableSaleDetail.AddCell(new PdfPCell(new Phrase(item.Codigo, hp.fontCell))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    tableSaleDetail.AddCell(new PdfPCell(new Phrase(item.Descripcion, hp.fontCell))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    tableSaleDetail.AddCell(new PdfPCell(new Phrase(new Phrase(Math.Round(item.Precio ?? 0, 2).ToString(LabelFormat), hp.fontCell)))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    tableSaleDetail.AddCell(new PdfPCell(new Phrase((item.Cantidad ?? 0).ToString(), hp.fontCell))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    if ((item.Descuento ?? 0) > 0)
                    {
                        tableSaleDetail.AddCell(new PdfPCell(new Phrase((item.Descuento ?? 0).ToString() + "%", hp.fontCell))
                        {
                            HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                            BackgroundColor = new BaseColor(System.Drawing.Color.White)
                        });
                    }
                    else
                    {
                        tableSaleDetail.AddCell(new PdfPCell(new Phrase("", hp.fontCell))
                        {
                            HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                            BackgroundColor = new BaseColor(System.Drawing.Color.White)
                        });
                    }

                    var total = (item.Precio ?? 0) * (item.Cantidad ?? 0) - (((item.Precio ?? 0) * (item.Cantidad ?? 0)) * ((item.Descuento ?? 0) / 100));

                    tableSaleDetail.AddCell(new PdfPCell(new Phrase(new Phrase(Math.Round(total, 2).ToString(LabelFormat), hp.fontCell)))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    if (!String.IsNullOrEmpty(item.Comentarios))
                    {
                        tableSaleDetail.AddCell(new PdfPCell(new Phrase(new Phrase("COMENTARIOS:" + item.Comentarios, hp.fontCell)))
                        {
                            HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                            BackgroundColor = new BaseColor(System.Drawing.Color.White),
                            Colspan = 7
                        });
                    }
                }

                pdfDoc.Add(tableSaleDetail);

                PdfPTable tableTotal = new PdfPTable(6)
                {
                    WidthPercentage = ValueHundred
                };

                tableTotal.AddCell(new PdfPCell(new Phrase("Cantidad de Productos", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                    Colspan = 2
                });

                tableTotal.AddCell(new PdfPCell(new Phrase((quotation.CantidadProductos ?? 0).ToString(), hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableTotal.AddCell(new PdfPCell(new Phrase("Subtotal", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                    Colspan = 2
                });

                tableTotal.AddCell(new PdfPCell(new Phrase(new Phrase(Math.Round(quotation.Subtotal ?? 0, 2).ToString(LabelFormat), hp.fontCell)))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                });

                //Descuento
                if (quotation.Descuento > 0)
                {
                    tableTotal.AddCell(new PdfPCell(new Phrase("Descuento", hp.fontHeader))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White),
                        Border = Rectangle.NO_BORDER,
                        Colspan = 5
                    });

                    tableTotal.AddCell(new PdfPCell(new Phrase(new Phrase((quotation.Descuento ?? 0).ToString() + "%", hp.fontCell)))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White),
                        Border = Rectangle.NO_BORDER,
                    });
                }

                tableTotal.AddCell(new PdfPCell(new Phrase("Total", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                    Colspan = 5
                });

                tableTotal.AddCell(new PdfPCell(new Phrase(new Phrase(Math.Round(quotation.Total ?? 0, 2).ToString(LabelFormat), hp.fontCell)))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                pdfDoc.Add(tableTotal);

                pdfDoc.Add(new Phrase(Environment.NewLine));

                PdfPTable tableTerms = new PdfPTable(1)
                {
                    WidthPercentage = ValueHundred
                };

                tableTerms.AddCell(new PdfPCell(new Phrase("Precios sujetos a cambio sin previo aviso, en productos importados o sobre pedido.", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableTerms.AddCell(new PdfPCell(new Phrase("En pedidos de diseños especiales proporcionados por el cliente no nos hacemos responsables por fallas de cálculo o de ergonomía, los diseños que los clientes nos proveen son totalmente bajo su riesgo y responsabilidad y deberán ser pagados al 100% por el cliente antes de la entrega, cualquier cambio o modificación al diseño ya aceptado se cobrará como extra. Las garantías de fabricación varían según el proveedor y serán estipuladas en una carta cuando el cliente lo solicite a la hora de la compra.", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                pdfDoc.Add(tableTerms);

                pdfDoc.Add(new Phrase(Environment.NewLine));

                PdfPTable tableFooter = new PdfPTable(1)
                {
                    WidthPercentage = ValueHundred
                };

                tableFooter.AddCell(new PdfPCell(new Phrase("AMAZONAS #305 OTE. COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL.66220 MÉXICO. T: (81) 8356-9558 / (81) 8335-0321", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableFooter.AddCell(new PdfPCell(new Phrase("RIO GUADALQUIVIR #13. ESQ. CALZADA SAN PEDRO COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL. 66220 MÉXICO. T: (81) 8335-5591", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableFooter.AddCell(new PdfPCell(new Phrase("AMAZONAS #320 PLAZA AMAZONAS COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL. 66220 MÉXICO. T: (81) 8335-0042", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableFooter.AddCell(new PdfPCell(new Phrase("WWW.ACCENTSDECORATION.COM", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                pdfDoc.Add(tableFooter);

                pdfDoc.Close();
                writer.Flush();

                return memoryStream;
            }
        }

        public MemoryStream GenerateView(int id)
        {
            OutProducts _view = new OutProducts();
            var view = _view.GetViewForIdView(id);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                var hp = new HelperTextSharp();

                Document pdfDoc = new Document(PageSize.LETTER, 50f, 50f, 10f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                pdfDoc.Open();

                string imageURL = System.Web.HttpContext.Current.Server.MapPath("~") + "content\\images\\logo_des.png";

                iTextSharp.text.Image logo_accents = iTextSharp.text.Image.GetInstance(imageURL);
                logo_accents.ScaleToFit(100f, 50f);

                PdfPTable tableLogo = new PdfPTable(3) { WidthPercentage = ValueHundred, SpacingAfter = 20 };

                //LOGO START
                tableLogo.AddCell(new PdfPCell(new Phrase("", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableLogo.AddCell(new PdfPCell(logo_accents)
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableLogo.AddCell(new PdfPCell(new Phrase("Salida Vista:" + view.remision, hp.fontRedCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                pdfDoc.Add(tableLogo);

                PdfPTable tableHeader = new PdfPTable(3) { WidthPercentage = ValueHundred };

                //SELLER
                tableHeader.AddCell(new PdfPCell(new Phrase("Sucursal:" + view.Sucursal, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableHeader.AddCell(new PdfPCell(new Phrase("Vendedor:" + view.Vendedor, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableHeader.AddCell(new PdfPCell(new Phrase("Fecha:" + view.Fecha.Value.ToString("dd/MM/yyyy"), hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                pdfDoc.Add(tableHeader);

                //CUSTOMER
                PdfPTable tableCustomer = new PdfPTable(2) { WidthPercentage = ValueHundred, SpacingAfter = 10 };

                //CUSTOMER NAME
                string customerName;

                if (!String.IsNullOrEmpty(view.ClienteFisico))
                {
                    customerName = view.ClienteFisico;
                }
                else if (!String.IsNullOrEmpty(view.ClienteMoral))
                {
                    customerName = view.ClienteMoral;
                }
                else
                {
                    customerName = view.Despacho;
                }

                tableCustomer.AddCell(new PdfPCell(new Phrase("Cliente:" + customerName, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Móvil:" + view.oAddress.TelCelular, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Teléfono:" + view.oAddress.TelCasa, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Dirección:" + view.oAddress.Direccion, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Correo:" + view.oAddress.Correo, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Proyecto:" + view.Proyecto, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                pdfDoc.Add(tableCustomer);

                float[] columnDetail = { 25, 15, 30, 10, 10, 10 };
                PdfPTable tableViewDetail = new PdfPTable(columnDetail)
                {
                    WidthPercentage = ValueHundred
                };

                tableViewDetail.AddCell(new PdfPCell(new Phrase("DETALLE DE LA SALIDA A VISTA", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Colspan = 6
                });

                tableViewDetail.AddCell(new PdfPCell(new Phrase("Imagen", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableViewDetail.AddCell(new PdfPCell(new Phrase("Código", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableViewDetail.AddCell(new PdfPCell(new Phrase("Descripción", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableViewDetail.AddCell(new PdfPCell(new Phrase("Precio", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableViewDetail.AddCell(new PdfPCell(new Phrase("Cantidad", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableViewDetail.AddCell(new PdfPCell(new Phrase("Total", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                foreach (var item in view.oDetail)
                {
                    string image_URL = System.Web.HttpContext.Current.Server.MapPath("~") + "content\\images\\not_found.png";

                    if (item.TipoImagen == 1)
                    {
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("~") + "content\\products\\" + item.Imagen))
                        {
                            image_URL = System.Web.HttpContext.Current.Server.MapPath("~") + "content\\products\\" + item.Imagen;
                        }
                    }

                    iTextSharp.text.Image imagen_item = iTextSharp.text.Image.GetInstance(image_URL);
                    imagen_item.ScaleToFit(70f, 70f);
                    imagen_item.SpacingBefore = 2f;
                    imagen_item.SpacingAfter = 2f;
                    imagen_item.Alignment = Element.ALIGN_CENTER;

                    tableViewDetail.AddCell(new PdfPCell(imagen_item)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    tableViewDetail.AddCell(new PdfPCell(new Phrase(item.Codigo, hp.fontCell))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    tableViewDetail.AddCell(new PdfPCell(new Phrase(item.Descripcion, hp.fontCell))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    tableViewDetail.AddCell(new PdfPCell(new Phrase(new Phrase(Math.Round(item.Precio ?? 0, 2).ToString(LabelFormat), hp.fontCell)))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    tableViewDetail.AddCell(new PdfPCell(new Phrase((item.Cantidad ?? 0).ToString(), hp.fontCell))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    var total = (item.Precio ?? 0) * (item.Cantidad ?? 0);

                    tableViewDetail.AddCell(new PdfPCell(new Phrase(new Phrase(Math.Round(total, 2).ToString(LabelFormat), hp.fontCell)))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });
                }

                pdfDoc.Add(tableViewDetail);

                PdfPTable tableTotal = new PdfPTable(6)
                {
                    WidthPercentage = ValueHundred
                };

                tableTotal.AddCell(new PdfPCell(new Phrase("Cantidad de Productos", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                    Colspan = 2
                });

                tableTotal.AddCell(new PdfPCell(new Phrase((view.CantidadProductos ?? 0).ToString(), hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableTotal.AddCell(new PdfPCell(new Phrase("Subtotal", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                    Colspan = 2
                });

                tableTotal.AddCell(new PdfPCell(new Phrase(new Phrase(Math.Round(view.Subtotal ?? 0, 2).ToString(LabelFormat), hp.fontCell)))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                });

                tableTotal.AddCell(new PdfPCell(new Phrase("Total", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                    Colspan = 5
                });

                tableTotal.AddCell(new PdfPCell(new Phrase(new Phrase(Math.Round(view.Total ?? 0, 2).ToString(LabelFormat), hp.fontCell)))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                pdfDoc.Add(tableTotal);

                pdfDoc.Add(new Phrase(Environment.NewLine));

                PdfPTable tableTerms = new PdfPTable(1)
                {
                    WidthPercentage = ValueHundred
                };

                tableTerms.AddCell(new PdfPCell(new Phrase("Debo y pagaré incondicionalmente por este pagaré a la orden de Accents Decoration S.de R.L.de C.V.el valor de la mercancía antes descrita, entregada a mi entera satisfacción.Al no pagarse a su vencimiento, desde ese momento hasta el día de la liquidación, causará intereses moratorios al tipo de 10 % mensual, pagadero en esta ciudad justamente con el principal.", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                pdfDoc.Add(tableTerms);

                pdfDoc.Add(new Phrase(Environment.NewLine));

                PdfPTable tableFooter = new PdfPTable(1)
                {
                    WidthPercentage = ValueHundred
                };

                tableFooter.AddCell(new PdfPCell(new Phrase("AMAZONAS #305 OTE. COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL.66220 MÉXICO. T: (81) 8356-9558 / (81) 8335-0321", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableFooter.AddCell(new PdfPCell(new Phrase("RIO GUADALQUIVIR #13. ESQ. CALZADA SAN PEDRO COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL. 66220 MÉXICO. T: (81) 8335-5591", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableFooter.AddCell(new PdfPCell(new Phrase("AMAZONAS #320 PLAZA AMAZONAS COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL. 66220 MÉXICO. T: (81) 8335-0042", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableFooter.AddCell(new PdfPCell(new Phrase("WWW.ACCENTSDECORATION.COM", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                pdfDoc.Add(tableFooter);

                pdfDoc.Close();
                writer.Flush();

                return memoryStream;
            }
        }

        public MemoryStream GenerateCatalogView(CatalogViewPrintViewModel view)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                var hp = new HelperTextSharp();

                Document pdfDoc = new Document(PageSize.LETTER, 50f, 50f, 10f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                pdfDoc.Open();

                string imageURL = System.Web.HttpContext.Current.Server.MapPath("~") + "content\\images\\logo_des.png";

                iTextSharp.text.Image logo_accents = iTextSharp.text.Image.GetInstance(imageURL);
                logo_accents.ScaleToFit(100f, 50f);

                PdfPTable tableLogo = new PdfPTable(3) { WidthPercentage = ValueHundred, SpacingAfter = 20 };

                //LOGO START
                tableLogo.AddCell(new PdfPCell(new Phrase("", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableLogo.AddCell(new PdfPCell(logo_accents)
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableLogo.AddCell(new PdfPCell(new Phrase("Préstamo:" + view.Numero, hp.fontRedCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                pdfDoc.Add(tableLogo);

                PdfPTable tableHeader = new PdfPTable(3) { WidthPercentage = ValueHundred };

                //SELLER
                tableHeader.AddCell(new PdfPCell(new Phrase("Sucursal:" + view.Sucursal, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableHeader.AddCell(new PdfPCell(new Phrase("Vendedor:" + view.Usuario, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableHeader.AddCell(new PdfPCell(new Phrase("Fecha:" + view.Fecha.ToString("dd/MM/yyyy"), hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                pdfDoc.Add(tableHeader);

                //CUSTOMER
                PdfPTable tableCustomer = new PdfPTable(2) { WidthPercentage = ValueHundred, SpacingAfter = 10 };

                //CUSTOMER NAME

                tableCustomer.AddCell(new PdfPCell(new Phrase("Cliente:" + view.Cliente, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Móvil:" + view.Direccion.TelCelular, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Dirección:" + view.Direccion.Direccion, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Correo:" + view.Direccion.Correo, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                pdfDoc.Add(tableCustomer);

                float[] columnDetail = { 35, 25, 15, 10, 15 };

                PdfPTable tableViewDetail = new PdfPTable(columnDetail)
                {
                    WidthPercentage = ValueHundred
                };

                tableViewDetail.AddCell(new PdfPCell(new Phrase("DETALLE DE LOS CATÁLOGOS", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Colspan = 6
                });

                tableViewDetail.AddCell(new PdfPCell(new Phrase("Imagen", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableViewDetail.AddCell(new PdfPCell(new Phrase("Código", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableViewDetail.AddCell(new PdfPCell(new Phrase("Precio", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableViewDetail.AddCell(new PdfPCell(new Phrase("Cantidad", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableViewDetail.AddCell(new PdfPCell(new Phrase("Total", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                foreach (var item in view.Detalle)
                {
                    string image_URL = System.Web.HttpContext.Current.Server.MapPath("~") + "content\\images\\not_found.png";

                    if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("~") + "content\\catalogs\\" + item.Imagen))
                    {
                        image_URL = System.Web.HttpContext.Current.Server.MapPath("~") + "content\\catalogs\\" + item.Imagen;
                    }

                    iTextSharp.text.Image imagen_item = iTextSharp.text.Image.GetInstance(image_URL);
                    imagen_item.ScaleToFit(70f, 70f);
                    imagen_item.SpacingBefore = 2f;
                    imagen_item.SpacingAfter = 2f;
                    imagen_item.Alignment = Element.ALIGN_CENTER;

                    tableViewDetail.AddCell(new PdfPCell(imagen_item)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    tableViewDetail.AddCell(new PdfPCell(new Phrase(item.Catalogo.Codigo, hp.fontCell))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    tableViewDetail.AddCell(new PdfPCell(new Phrase(new Phrase(Math.Round(item.Precio, 2).ToString(LabelFormat), hp.fontCell)))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    tableViewDetail.AddCell(new PdfPCell(new Phrase((item.Cantidad).ToString(), hp.fontCell))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    var total = (item.Precio) * (item.Cantidad);

                    tableViewDetail.AddCell(new PdfPCell(new Phrase(new Phrase(Math.Round(total, 2).ToString(LabelFormat), hp.fontCell)))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });
                }

                pdfDoc.Add(tableViewDetail);

                PdfPTable tableTotal = new PdfPTable(6)
                {
                    WidthPercentage = ValueHundred
                };

                tableTotal.AddCell(new PdfPCell(new Phrase("Cantidad de Productos", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                    Colspan = 2
                });

                tableTotal.AddCell(new PdfPCell(new Phrase((view.CantidadProductos).ToString(), hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableTotal.AddCell(new PdfPCell(new Phrase("Subtotal", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                    Colspan = 2
                });

                tableTotal.AddCell(new PdfPCell(new Phrase(new Phrase(Math.Round(view.Subtotal, 2).ToString(LabelFormat), hp.fontCell)))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                });

                tableTotal.AddCell(new PdfPCell(new Phrase("Total", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                    Colspan = 5
                });

                tableTotal.AddCell(new PdfPCell(new Phrase(new Phrase(Math.Round(view.Total, 2).ToString(LabelFormat), hp.fontCell)))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                pdfDoc.Add(tableTotal);

                pdfDoc.Add(new Phrase(Environment.NewLine));

                PdfPTable tableTerms = new PdfPTable(1)
                {
                    WidthPercentage = ValueHundred
                };

                tableTerms.AddCell(new PdfPCell(new Phrase("Debo y pagaré incondicionalmente por este pagaré a la orden de Accents Decoration S.de R.L.de C.V. el valor de la mercancía antes descrita, entregada a mi entera satisfacción.Al no pagarse a su vencimiento, desde ese momento hasta el día de la liquidación, causará intereses moratorios al tipo de 10 % mensual, pagadero en esta ciudad justamente con el principal.", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                pdfDoc.Add(tableTerms);

                pdfDoc.Add(new Phrase(Environment.NewLine));

                PdfPTable tableFooter = new PdfPTable(1)
                {
                    WidthPercentage = ValueHundred
                };

                tableFooter.AddCell(new PdfPCell(new Phrase("AMAZONAS #305 OTE. COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL.66220 MÉXICO. T: (81) 8356-9558 / (81) 8335-0321", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableFooter.AddCell(new PdfPCell(new Phrase("RIO GUADALQUIVIR #13. ESQ. CALZADA SAN PEDRO COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL. 66220 MÉXICO. T: (81) 8335-5591", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableFooter.AddCell(new PdfPCell(new Phrase("AMAZONAS #320 PLAZA AMAZONAS COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL. 66220 MÉXICO. T: (81) 8335-0042", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableFooter.AddCell(new PdfPCell(new Phrase("WWW.ACCENTSDECORATION.COM", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                pdfDoc.Add(tableFooter);

                pdfDoc.Close();
                writer.Flush();

                return memoryStream;
            }
        }

        public MemoryStream GeneratePreQuotation(int id)
        {
            PreQuotations _preQuotation = new PreQuotations();
            var quotation = _preQuotation.GetPreQuotationsId(id);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                var hp = new HelperTextSharp();

                Document pdfDoc = new Document(PageSize.LETTER, 50f, 50f, 10f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                pdfDoc.Open();

                string imageURL = System.Web.HttpContext.Current.Server.MapPath("~") + "content\\images\\logo_des.png";

                iTextSharp.text.Image logo_accents = iTextSharp.text.Image.GetInstance(imageURL);
                logo_accents.ScaleToFit(100f, 50f);

                PdfPTable tableLogo = new PdfPTable(3) { WidthPercentage = ValueHundred, SpacingAfter = 20 };

                //LOGO START
                tableLogo.AddCell(new PdfPCell(new Phrase("", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableLogo.AddCell(new PdfPCell(logo_accents)
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableLogo.AddCell(new PdfPCell(new Phrase("Precotización:" + quotation.Numero, hp.fontRedCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                pdfDoc.Add(tableLogo);

                PdfPTable tableHeader = new PdfPTable(3) { WidthPercentage = ValueHundred };

                //SELLER
                tableHeader.AddCell(new PdfPCell(new Phrase("Sucursal:" + quotation.Sucursal, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableHeader.AddCell(new PdfPCell(new Phrase("Vendedor:" + quotation.Usuario1, hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                tableHeader.AddCell(new PdfPCell(new Phrase("Fecha:" + quotation.Fecha.Value.ToString("dd/MM/yyyy"), hp.fontNormalHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                pdfDoc.Add(tableHeader);

                float[] columnDetail = { 25, 8, 15, 8, 22, 22 };
                PdfPTable tableSaleDetail = new PdfPTable(columnDetail)
                {
                    WidthPercentage = ValueHundred
                };

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("DETALLE DE LA PRECOTIZACIÓN", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Colspan = 7
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Imagen", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("ID", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Servicio", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Cantidad", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Medidas", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                tableSaleDetail.AddCell(new PdfPCell(new Phrase("Telas", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White)
                });

                foreach (var item in quotation.oDetail)
                {
                    string image_URL = System.Web.HttpContext.Current.Server.MapPath("~") + "content\\images\\not_found.png";

                    string fullPath = item.Imagen;
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullPath);
                    string fileExtension = Path.GetExtension(fullPath);
                    string fileNameWithExtension = fileNameWithoutExtension + fileExtension;


                    if (!String.IsNullOrEmpty(item.Imagen))
                    {
                        if (item.Imagen.ToUpper() != ISNULL)
                        {
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("~") + "content\\services\\" + fileNameWithExtension))
                            {
                                image_URL = System.Web.HttpContext.Current.Server.MapPath("~") + "content\\services\\" + fileNameWithExtension;
                            }
                        }
                    }

                    iTextSharp.text.Image imagen_item = iTextSharp.text.Image.GetInstance(image_URL);
                    imagen_item.ScaleToFit(70f, 70f);
                    imagen_item.SpacingBefore = 2f;
                    imagen_item.SpacingAfter = 2f;
                    imagen_item.Alignment = Element.ALIGN_CENTER;

                    tableSaleDetail.AddCell(new PdfPCell(imagen_item)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    tableSaleDetail.AddCell(new PdfPCell(new Phrase((item.idServicio).ToString(), hp.fontCell))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    tableSaleDetail.AddCell(new PdfPCell(new Phrase(item.Descripcion, hp.fontCell))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    tableSaleDetail.AddCell(new PdfPCell(new Phrase((item.Cantidad ?? 0).ToString(), hp.fontCell))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    var textMedidas = "";
                    foreach (var m in item.measures)
                    {
                        textMedidas += m.NombreTipoMedida + " " + m.Valor + " ";
                    }

                    tableSaleDetail.AddCell(new PdfPCell(new Phrase(textMedidas, hp.fontCell))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    var textTelas = "";
                    foreach (var t in item.fabrics)
                    {
                        textTelas += t.NombreTextiles + " CostoXMts: " + t.CostoPorMts + " /";
                    }

                    tableSaleDetail.AddCell(new PdfPCell(new Phrase(textTelas, hp.fontCell))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(System.Drawing.Color.White)
                    });

                    if (!String.IsNullOrEmpty(item.Comentarios))
                    {
                        tableSaleDetail.AddCell(new PdfPCell(new Phrase(new Phrase("COMENTARIOS:" + item.Comentarios, hp.fontCell)))
                        {
                            HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                            BackgroundColor = new BaseColor(System.Drawing.Color.White),
                            Colspan = 7
                        });
                    }
                }

                pdfDoc.Add(tableSaleDetail);

                PdfPTable tableTotal = new PdfPTable(6)
                {
                    WidthPercentage = ValueHundred
                };

                tableTotal.AddCell(new PdfPCell(new Phrase("Cantidad de Servicios", hp.fontHeader))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                    Colspan = 2
                });

                tableTotal.AddCell(new PdfPCell(new Phrase((quotation.oDetail.Count).ToString(), hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER,
                    Colspan = 1
                });

                pdfDoc.Add(tableTotal);

                pdfDoc.Add(new Phrase(Environment.NewLine));

                PdfPTable tableTerms = new PdfPTable(1)
                {
                    WidthPercentage = ValueHundred
                };

                tableTerms.AddCell(new PdfPCell(new Phrase("Precios sujetos a cambio sin previo aviso, en productos importados o sobre pedido.", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableTerms.AddCell(new PdfPCell(new Phrase("En pedidos de diseños especiales proporcionados por el cliente no nos hacemos responsables por fallas de cálculo o de ergonomía, los diseños que los clientes nos proveen son totalmente bajo su riesgo y responsabilidad y deberán ser pagados al 100% por el cliente antes de la entrega, cualquier cambio o modificación al diseño ya aceptado se cobrará como extra. Las garantías de fabricación varían según el proveedor y serán estipuladas en una carta cuando el cliente lo solicite a la hora de la compra.", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                pdfDoc.Add(tableTerms);

                pdfDoc.Add(new Phrase(Environment.NewLine));

                PdfPTable tableFooter = new PdfPTable(1)
                {
                    WidthPercentage = ValueHundred
                };

                tableFooter.AddCell(new PdfPCell(new Phrase("AMAZONAS #305 OTE. COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL.66220 MÉXICO. T: (81) 8356-9558 / (81) 8335-0321", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableFooter.AddCell(new PdfPCell(new Phrase("RIO GUADALQUIVIR #13. ESQ. CALZADA SAN PEDRO COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL. 66220 MÉXICO. T: (81) 8335-5591", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableFooter.AddCell(new PdfPCell(new Phrase("AMAZONAS #320 PLAZA AMAZONAS COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL. 66220 MÉXICO. T: (81) 8335-0042", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                tableFooter.AddCell(new PdfPCell(new Phrase("WWW.ACCENTSDECORATION.COM", hp.fontCell))
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(System.Drawing.Color.White),
                    Border = Rectangle.NO_BORDER
                });

                pdfDoc.Add(tableFooter);

                pdfDoc.Close();
                writer.Flush();

                return memoryStream;
            }
        }
    }
}