using ADEntities.Enums;
using ADEntities.Models;
using ADEntities.Queries.IRepository;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using ADSystem.Manager.IManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADSystem.Manager
{
    /// <summary>
    /// CatalogViewManager
    /// </summary>
    /// <seealso cref="ADSystem.Manager.BaseManager{Category, CategoryViewModel}" />
    /// <seealso cref="ICategoryManager" />
    public class CatalogViewManager : BaseManager<tCatalogoVista, CatalogViewViewModel>, ICatalogViewManager
    {
        private ICatalogViewRepository repository;
        private ICatalogDetailViewManager detailViewManager;
        private ICatalogDetailDevolutionManager catalogDetailDevolutionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryManager"/> class.
        /// </summary>
        public CatalogViewManager(ICatalogViewRepository _repository, ICatalogDetailViewManager _detailViewManager, ICatalogDetailDevolutionManager _catalogDetailDevolutionManager) : base(_repository)
        {
            repository = _repository;
            detailViewManager = _detailViewManager;
            catalogDetailDevolutionManager = _catalogDetailDevolutionManager;
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public int GetCount(CatalogViewFilterViewModel filter)
        {
            return repository.GetCount(filter);
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public CatalogViewPrintViewModel GetForPrint(int id)
        {
            var entity = repository.GetById(id);

            return new CatalogViewPrintViewModel()
            {
                idVista = entity.idVista,
                Numero = entity.Numero,
                idUsuario = entity.idUsuario,
                Usuario = entity.tUsuario.Nombre + " " + entity.tUsuario.Apellidos,
                Cliente = (entity.TipoCliente == TypesCustomers.PhysicalCustomer) ? entity.tClientesFisico.Nombre + " " + entity.tClientesFisico.Apellidos :
                          (entity.TipoCliente == TypesCustomers.MoralCustomer) ? entity.tClientesMorale.Nombre :
                          (entity.TipoCliente == TypesCustomers.OfficeCustomer) ? entity.tDespacho.Nombre : "",
                Direccion = (entity.TipoCliente == TypesCustomers.PhysicalCustomer) ? new AddressViewModel()
                {
                    Correo = entity.tClientesFisico.Correo,
                    TelCasa = entity.tClientesFisico.Telefono,
                    TelCelular = entity.tClientesFisico.TelefonoCelular,
                    Direccion = entity.tClientesFisico.Calle + " " + entity.tClientesFisico.NumExt + " " + entity.tClientesFisico.NumInt + " " + entity.tClientesFisico.Colonia + " " + ((entity.tClientesFisico.tMunicipio != null) ? entity.tClientesFisico.tMunicipio.nombre_municipio : "") + " " + entity.tClientesFisico.CP
                } : (entity.TipoCliente == TypesCustomers.MoralCustomer) ? new AddressViewModel()
                {
                    Correo = entity.tClientesMorale.Correo,
                    TelCasa = entity.tClientesMorale.Telefono,
                    TelCelular = entity.tClientesMorale.TelefonoCelular,
                    Direccion = entity.tClientesMorale.Calle + " " + entity.tClientesMorale.NumExt + " " + entity.tClientesMorale.NumInt + " " + entity.tClientesMorale.Colonia + " " + ((entity.tClientesMorale.tMunicipio != null) ? entity.tClientesMorale.tMunicipio.nombre_municipio : "") + " " + entity.tClientesMorale.CP
                } : new AddressViewModel()
                {
                    Correo = entity.tDespacho.Correo,
                    TelCasa = entity.tDespacho.Telefono,
                    TelCelular = "",
                    Direccion = entity.tDespacho.Calle + " " + entity.tDespacho.NumExt + " " + entity.tDespacho.NumInt + " " + entity.tDespacho.Colonia + " " + ((entity.tDespacho.tMunicipio != null) ? entity.tDespacho.tMunicipio.nombre_municipio : "") + " " + entity.tDespacho.CP
                },
                idSucursal = entity.idSucursal,
                Sucursal = entity.tSucursale.Nombre,
                Fecha = entity.Fecha,
                CantidadProductos = entity.CantidadProductos,
                Subtotal = entity.Subtotal,
                Total = entity.Total,
                Estatus = entity.Estatus,
                TipoCliente = entity.TipoCliente,
                Detalle = entity.tCatalogoDetalleVistas.Select(o => new CatalogDetailViewViewModel()
                {
                    idDetalleVista = o.idDetalleVista,
                    idVista = o.idVista,
                    idCatalogo = o.idCatalogo,
                    Catalogo = new CatalogViewModel()
                    {
                        idCatalogo = o.tCatalogo.idCatalogo,
                        Codigo = o.tCatalogo.Codigo,
                        Modelo = o.tCatalogo.Modelo,
                        Volumen = o.tCatalogo.Volumen,
                        Imagen = "/Content/Catalogs/" + o.tCatalogo.Imagen
                    },
                    Precio = o.Precio,
                    Cantidad = o.Cantidad,
                    idSucursal = o.idSucursal,
                    Devolucion = o.Devolucion,
                    Comentarios = o.Comentarios,
                    Estatus = o.Estatus,
                    Detalle = o.tCatalogoDetalleDevolucions.Select(q => new CatalogDetailDevolutionViewModel()
                    {
                        idDetalleDevoluciones = q.idDetalleDevoluciones,
                        idDetalleVista = q.idDetalleDevoluciones,
                        Devolucion = q.Devolucion,
                        Fecha = q.Fecha,
                        idVerificador = q.idVerificador,
                        Comentarios = q.Comentarios
                    }).ToList()
                }).ToList()
            };
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public List<CatalogViewViewModel> GetAll(CatalogViewFilterViewModel filter)
        {
            var entities = repository.GetAll(filter);

            return this.PrepareMultipleReturn(entities);
        }

        public override CatalogViewViewModel Post(CatalogViewViewModel model)
        {
            var entity = PrepareAddData(model);
            var result = repository.Add(entity);
            detailViewManager.PostRange(result.idVista, model.Detalle);

            return new CatalogViewViewModel()
            {
                idVista = result.idVista,
                idUsuario = result.idUsuario,
                idClienteFisico = result.idClienteFisico,
                idClienteMoral = result.idClienteMoral,
                idDespacho = result.idDespacho,
                idDespachoReferencia = result.idDespachoReferencia,
                idSucursal = result.idSucursal,
                Fecha = DateTime.Now,
                CantidadProductos = result.CantidadProductos,
                Subtotal = result.Subtotal,
                Total = result.Total,
                Estatus = (short)StatusCatalogView.Pending,
                TipoCliente = result.TipoCliente,
                CreadoPor = result.CreadoPor,
                Creado = result.Creado
            };
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="category">The category.</param>
        public void Update(int id, CatalogViewViewModel catalog)
        {
            var entity = repository.GetById(id);

            repository.Update(this.PrepareUpdateData(entity, catalog));
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <param name="idUser">The identifier user.</param>
        public void UpdateStatus(int id, short status, int idUser)
        {
            var entity = repository.GetById(id);

            entity.Estatus = status;
            entity.ModificadoPor = idUser;
            entity.Modificado = DateTime.Now;

            repository.Update(entity);
        }

        /// <summary>
        /// Prepares the add data.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override tCatalogoVista PrepareAddData(CatalogViewViewModel viewModel)
        {
            return new tCatalogoVista()
            {
                Numero = this.GeneratePreviousNumber(),
                idUsuario = viewModel.idUsuario,
                idClienteFisico = viewModel.idClienteFisico,
                idClienteMoral = viewModel.idClienteMoral,
                idDespacho = viewModel.idDespacho,
                idDespachoReferencia = viewModel.idDespachoReferencia,
                idSucursal = viewModel.idSucursal,
                Fecha = DateTime.Now,
                CantidadProductos = viewModel.CantidadProductos,
                Subtotal = viewModel.Subtotal,
                Total = viewModel.Total,
                Estatus = (short)StatusCatalogView.Pending,
                TipoCliente = viewModel.TipoCliente,
                CreadoPor = viewModel.CreadoPor,
                Creado = viewModel.Creado
            };
        }

        /// <summary>
        /// Prepares the single return.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override CatalogViewViewModel PrepareSingleReturn(tCatalogoVista entity)
        {
            return new CatalogViewViewModel()
            {
                idVista = entity.idVista,
                Numero = entity.Numero,
                idUsuario = entity.idUsuario,
                Usuario = entity.tUsuario.Nombre + " " + entity.tUsuario.Apellidos,
                idClienteFisico = entity.idClienteFisico,
                idClienteMoral = entity.idClienteMoral,
                idDespacho = entity.idDespacho,
                idDespachoReferencia = entity.idDespachoReferencia,
                NombreCliente = (entity.TipoCliente == TypesCustomers.PhysicalCustomer) ? entity.tClientesFisico.Nombre + " " + entity.tClientesFisico.Apellidos :
                          (entity.TipoCliente == TypesCustomers.MoralCustomer) ? entity.tClientesMorale.Nombre :
                          (entity.TipoCliente == TypesCustomers.OfficeCustomer) ? entity.tDespacho.Nombre : "",
                idSucursal = entity.idSucursal,
                Sucursal = entity.tSucursale.Nombre,
                Fecha = entity.Creado,
                CantidadProductos = entity.CantidadProductos,
                Subtotal = entity.Subtotal,
                Total = entity.Total,
                Estatus = entity.Estatus,
                TipoCliente = entity.TipoCliente,
                CreadoPor = entity.CreadoPor,
                Creado = entity.Creado,
                Detalle = entity.tCatalogoDetalleVistas.Select(o => new CatalogDetailViewViewModel()
                {
                    idDetalleVista = o.idDetalleVista,
                    idVista = o.idVista,
                    idCatalogo = o.idCatalogo,
                    Catalogo = new CatalogViewModel()
                    {
                        idCatalogo = o.tCatalogo.idCatalogo,
                        Codigo = o.tCatalogo.Codigo,
                        Modelo = o.tCatalogo.Modelo,
                        Volumen = o.tCatalogo.Volumen,
                        Imagen = "/Content/Catalogs/" + o.tCatalogo.Imagen
                    },
                    Precio = o.Precio,
                    Cantidad = o.Cantidad,
                    idSucursal = o.idSucursal,
                    Pendiente = o.Cantidad - (o.Devolucion ?? 0),
                    Devolucion = o.Devolucion,
                    Comentarios = o.Comentarios,
                    Estatus = o.Estatus,
                    Detalle = o.tCatalogoDetalleDevolucions.Select(q => new CatalogDetailDevolutionViewModel()
                    {
                        idDetalleDevoluciones = q.idDetalleDevoluciones,
                        idDetalleVista = q.idDetalleDevoluciones,
                        Devolucion = q.Devolucion,
                        Fecha = q.Fecha,
                        idVerificador = q.idVerificador,
                        Comentarios = q.Comentarios
                    }).ToList()
                }).ToList()
            };
        }

        /// <summary>
        /// Prepares the multiple return.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override List<CatalogViewViewModel> PrepareMultipleReturn(List<tCatalogoVista> entities)
        {
            return entities.Select(p => new CatalogViewViewModel()
            {
                idVista = p.idVista,
                Numero = p.Numero,
                idUsuario = p.idUsuario,
                Usuario = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                idClienteFisico = p.idClienteFisico,
                idClienteMoral = p.idClienteMoral,
                idDespacho = p.idDespacho,
                idDespachoReferencia = p.idDespachoReferencia,
                NombreCliente = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos :
                          (p.TipoCliente == TypesCustomers.MoralCustomer) ? p.tClientesMorale.Nombre :
                          (p.TipoCliente == TypesCustomers.OfficeCustomer) ? p.tDespacho.Nombre : "",
                idSucursal = p.idSucursal,
                Fecha = p.Creado,
                CantidadProductos = p.CantidadProductos,
                CantidadPendiente = (p.CantidadProductos - (p.tCatalogoDetalleVistas.Sum(q => q.Devolucion) ?? 0)),
                Subtotal = p.Subtotal,
                Total = p.Total,
                Estatus = p.Estatus,
                ColorEstatus = (p.Estatus == (short)StatusCatalogView.Pending) ? "yellow" : (p.Estatus == (short)StatusCatalogView.Completed) ? "grey" : "red",
                TipoCliente = p.TipoCliente,
                CreadoPor = p.CreadoPor,
                Creado = p.Creado,
                Detalle = p.tCatalogoDetalleVistas.Select(o => new CatalogDetailViewViewModel()
                {
                    idDetalleVista = o.idDetalleVista,
                    idVista = o.idVista,
                    idCatalogo = o.idCatalogo,
                    Imagen = "/Content/Catalogs/" + o.tCatalogo.Imagen,
                    Precio = o.Precio,
                    Cantidad = o.Cantidad,
                    idSucursal = o.idSucursal,
                    Devolucion = o.Devolucion,
                    Comentarios = o.Comentarios,
                    Estatus = o.Estatus,
                    Detalle = o.tCatalogoDetalleDevolucions.Select(q => new CatalogDetailDevolutionViewModel()
                    {
                        idDetalleDevoluciones = q.idDetalleDevoluciones,
                        idDetalleVista = q.idDetalleDevoluciones,
                        Devolucion = q.Devolucion,
                        Fecha = q.Fecha,
                        idVerificador = q.idVerificador,
                        Verificador = q.tUsuario?.Nombre + " " + q.tUsuario?.Apellidos,
                        Comentarios = q.Comentarios
                    }).ToList()
                }).ToList()
            }).ToList();
        }

        /// <summary>
        /// Generates the previous number.
        /// </summary>
        /// <returns></returns>
        public string GeneratePreviousNumber()
        {
            string month = DateTime.Now.ToString("MM");
            string year = DateTime.Now.ToString("yy");

            string ID = (repository.GetLastID() + 1).ToString();
            char pad = '0';

            return string.Concat(month, string.Concat(string.Concat(year + "-", ID.PadLeft(4, pad)), "PC"));
        }

        /// <summary>
        /// Posts the specified entity.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="catalogs">The catalogs.</param>
        /// <param name="idUser">The identifier user.</param>
        /// <returns></returns>
        public CatalogViewViewModel Return(int id, int[][] catalogs, int idUser)
        {
            var catalogView = repository.GetById(id);

            if (catalogView.Estatus == (int)StatusCatalogView.Pending)
            {
                foreach (var prod in catalogs)
                {
                    int catalogDetailId = prod[0];
                    int amount = prod[1];

                    var catalogDetailView = detailViewManager.GetById(catalogDetailId);

                    catalogDetailView.Devolucion = (catalogDetailView.Devolucion ?? 0) + amount;

                    //Se cambia el Estatus a Entregado
                    if (catalogDetailView.Cantidad == catalogDetailView.Devolucion)
                    {
                        catalogDetailView.Estatus = (int)StatusCatalogView.Completed;
                    }

                    detailViewManager.Patch(catalogDetailView.idDetalleVista, catalogDetailView);

                    //Se inserta el historico de devoluciones
                    catalogDetailDevolutionManager.Post(new CatalogDetailDevolutionViewModel()
                    {
                        idDetalleVista = catalogDetailView.idDetalleVista,
                        Devolucion = amount,
                        idVerificador = idUser,
                        Fecha = DateTime.Now
                    });
                }

                //Si ya no existen productos pendientes se cambia el estatus general de la Salida a Vista por entregado
                if (detailViewManager.GetByCatalogViewId(id).Where(p => p.Estatus == (int)StatusCatalogView.Pending).Count() == 0)
                {
                    catalogView.Estatus = (int)StatusCatalogView.Completed;
                    catalogView.ModificadoPor = idUser;
                    catalogView.Modificado = DateTime.Now;

                    repository.Update(catalogView);
                }
            }

            return PrepareSingleReturn(catalogView);
        }
    }
}