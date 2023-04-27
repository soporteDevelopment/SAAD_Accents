using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class UnifiedQuotations
    {
        public UnifiedQuotationViewModel GenerateUnifiedQuotation(List<int> quotations)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var unifiedQuotation = context.tCotizacions.Where(p => p.idCotizacion == quotations.Select(q => q).FirstOrDefault()).Select(p => new UnifiedQuotationViewModel()
                    {
                        idCotizacion =  p.idCotizacion,
                        idUsuario1 = p.idUsuario1,
                        Usuario1 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
                        idUsuario2 = p.idUsuario2,
                        Usuario2 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        idClienteFisico = p.idClienteFisico,
                        idClienteMoral = p.idClienteMoral,
                        ClienteFisico = (p.idClienteFisico != null) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : p.tClientesMorale.Nombre,
                        idDespacho = p.idDespacho,
                        Proyecto = p.Proyecto,
                        idDespachoReferencia = p.idDespachoReferencia,
                        Despacho = p.tDespacho.Nombre,
                        DespachoReferencia = p.tDespacho1.Nombre,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        Subtotal = p.Subtotal,
                        Descuento = p.Descuento,
                        IVA = p.IVA,
                        Total = p.Total,
                        TipoCliente = p.TipoCliente,
                        Dolar = p.Dolar,
                        oAddress = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? new AddressViewModel()
                        {

                            Correo = p.tClientesFisico.Correo,
                            TelCasa = p.tClientesFisico.Telefono,
                            TelCelular = p.tClientesFisico.TelefonoCelular,
                            Direccion = p.tClientesFisico.Calle + " " + p.tClientesFisico.NumExt + " " + p.tClientesFisico.NumInt + " " + p.tClientesFisico.Colonia + " " + ((p.tClientesFisico.tMunicipio != null) ? p.tClientesFisico.tMunicipio.nombre_municipio : "") + " " + p.tClientesFisico.CP

                        } : (p.TipoCliente == TypesCustomers.MoralCustomer) ? new AddressViewModel()
                        {

                            Correo = p.tClientesMorale.Correo,
                            TelCasa = p.tClientesMorale.Telefono,
                            TelCelular = p.tClientesMorale.TelefonoCelular,
                            Direccion = p.tClientesMorale.Calle + " " + p.tClientesMorale.NumExt + " " + p.tClientesMorale.NumInt + " " + p.tClientesMorale.Colonia + " " + ((p.tClientesMorale.tMunicipio != null) ? p.tClientesMorale.tMunicipio.nombre_municipio : "") + " " + p.tClientesMorale.CP

                        } : new AddressViewModel()
                        {

                            Correo = p.tDespacho.Correo,
                            TelCasa = p.tDespacho.Telefono,
                            TelCelular = "",
                            Direccion = p.tDespacho.Calle + " " + p.tDespacho.NumExt + " " + p.tDespacho.NumInt + " " + p.tDespacho.Colonia + " " + ((p.tDespacho.tMunicipio != null) ? p.tDespacho.tMunicipio.nombre_municipio : "") + " " + p.tDespacho.CP

                        },
                        lCotizaciones = context.tCotizacions.Where(q => quotations.Contains(q.idCotizacion)).Select(co => new QuotationViewModel()
                        {
                            idCotizacion = co.idCotizacion,
                            Numero = co.Numero,
                            Proyecto = co.Proyecto,
                            idSucursal = co.idSucursal,
                            oDetail = co.tCotizacionDetalles.Select(x => new DetailQuotationViewModel()
                            {
                                idDetalleCotizacion = x.idCotizacionDetalle,
                                idCotizacion = x.idCotizacion,
                                idProducto = x.idProducto,
                                Codigo = x.tProducto.Codigo,
                                idServicio = x.idServicio,
                                Descripcion = x.Descripcion,
                                Precio = x.Precio,
                                Descuento = x.Descuento,
                                Cantidad = x.Cantidad,
                                TipoImagen = x.tProducto.TipoImagen,
                                Imagen = x.Imagen,
                                Comentarios = x.Comentarios,
                                idSucursal = co.idSucursal,
                                Sucursal = (x.idSucursal == TypesBranch.Amazonas) ? "Amazonas" : (x.idSucursal == TypesBranch.Guadalquivir ? "Guadalquivir" : "Texturas"),
                                idVista = x.idVista,
                                Remision = context.tVistas.Where(o => o.idVista == x.idVista).Select(y => y.Remision).FirstOrDefault(),
                                oProducto = context.tProductos.Where(o => o.idProducto == x.idProducto).Select(o => new ProductViewModel()
                                {
                                    idProducto = o.idProducto,
                                    urlImagen = o.TipoImagen == 1 ? "/Content/Products/" + o.NombreImagen + o.Extension : o.urlImagen,
                                    idProveedor = o.idProveedor
                                }).FirstOrDefault(),
                            }).ToList()
                        }).ToList()
                    }).FirstOrDefault();

                    return unifiedQuotation;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }
    }
}