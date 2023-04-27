using System;

namespace ADEntities.ViewModels
{
    public class ProviderViewModel
    {

        public int idProveedor { get; set; }

        public string Nombre { get; set; }

        public string Correo { get; set; }

        public string Telefono { get; set; }

        public int? Credito { get; set; }

        public int? DiasCredito { get; set; }

        public MXNBankAccountViewModel oCuentaBancariaMXN { get; set; }

        public INTBankAccountViewModel oCuentaBancariaINT { get; set; }

        public DateTime? FechaCaptura { get; set; }

        public decimal? Flete { get; set; }

        public decimal? Importacion { get; set; }

        public string Empresa { get; set; }

        public string RazonSocial { get; set; }

        public string NombreComercial { get; set; }

        public string DomicilioFiscal { get; set; }

        public string DomicilioEntrega { get; set; }

        public string Nacionalidad { get; set; }

        public string RFC { get; set; }

        public string SitioWeb { get; set; }

        public string SitioWebCatalogo { get; set; }

        public string Usuario { get; set; }

        public string Contrasena { get; set; }

        public int? idMarca { get; set; }

        public short Estatus { get; set; }

        public string RegimenFiscal { get; set; }

        public int? idTipoServicio { get; set; }

        public Boolean? ProveedorPedidosMedida { get; set; }

    }
}