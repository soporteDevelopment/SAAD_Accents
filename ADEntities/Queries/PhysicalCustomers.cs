using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;

namespace ADEntities.Queries
{
    public class PhysicalCustomers : Base
    {

        public int CountRegisters()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tClientesFisicos.Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<PhysicalCustomerViewModel> GetPhysicalCustomers()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tClientesFisicos.Select(p => new PhysicalCustomerViewModel()
                    {

                        Nombre = p.Nombre + " " + p.Apellidos,
                        Calle = p.Calle,
                        NumExt = p.NumExt,
                        NumInt = p.NumInt,
                        Colonia = p.Colonia,
                        idMunicipio = p.idMunicipio,
                        idEstado = context.tEstados.Join(context.tMunicipios, x => x.id_estado, m => m.estado, (x, m) => new { tEstados = x, tMunicipios = m }).Where(m => m.tMunicipios.id_municipio == p.idMunicipio).Select(y => y.tEstados.id_estado).FirstOrDefault(),
                        CP = p.CP,
                        FechaNacimiento = p.FechaNacimiento,
                        RFC = p.RFC,
                        TelefonoCelular = p.TelefonoCelular,
                        Sexo = p.Sexo,
                        NoIFE = p.NumeroIFE,
                        Telefono = p.Telefono,
                        Correo = p.Correo,
                        Credito = p.Credito,
                        Plazo = p.Plazo,
                        LimiteCredito = p.LimiteCredito,
                        FechaAlta = p.FechaAlta,
                        NombreIntermediario = p.NombreIntermediario,
                        TelefonoIntermediario = p.TelefonoIntermediario,
                        idCliente = p.idCliente

                    }).OrderBy(p => p.Nombre).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<PhysicalCustomerViewModel> GetPhysicalCustomers(string costumer, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var Customers = context.tClientesFisicos.Where(p => (p.Nombre + " " + p.Apellidos).Trim().ToUpper().Contains(costumer.Trim().ToUpper()) || String.IsNullOrEmpty(costumer)).Select(p => new PhysicalCustomerViewModel()
                    {

                        Nombre = p.Nombre,
                        Apellidos = (p.Apellidos == null) ? "" : p.Apellidos,
                        NombreCompleto = p.Nombre + " " + p.Apellidos,
                        Calle = p.Calle,
                        NumExt = p.NumExt,
                        NumInt = p.NumInt,
                        Colonia = p.Colonia,
                        idMunicipio = p.idMunicipio,
                        CP = p.CP,
                        FechaNacimiento = p.FechaNacimiento,
                        RFC = p.RFC,
                        TelefonoCelular = p.TelefonoCelular,
                        Sexo = p.Sexo,
                        NoIFE = p.NumeroIFE,
                        Telefono = p.Telefono,
                        Correo = p.Correo,
                        Credito = p.Credito,
                        Plazo = p.Plazo,
                        LimiteCredito = p.LimiteCredito,
                        FechaAlta = p.FechaAlta,
                        NombreIntermediario = p.NombreIntermediario,
                        TelefonoIntermediario = p.TelefonoIntermediario,
                        idCliente = p.idCliente

                    }).OrderBy(p => p.Nombre);

                    return Customers.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public PhysicalCustomerViewModel GetPhysicalCustomer(int idCliente)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tClientesFisicos.Where(p => p.idCliente == idCliente).Select(p => new PhysicalCustomerViewModel
                    {

                        Nombre = p.Nombre,
                        Apellidos = p.Apellidos,
                        Calle = p.Calle,
                        NumExt = p.NumExt,
                        NumInt = p.NumInt,
                        Colonia = p.Colonia,
                        idEstado = context.tEstados.Join(context.tMunicipios, x => x.id_estado, m => m.estado, (x, m) => new { tEstados = x, tMunicipios = m }).Where(m => m.tMunicipios.id_municipio == p.idMunicipio).Select(y => y.tEstados.id_estado).FirstOrDefault(),
                        idMunicipio = p.idMunicipio,
                        CP = p.CP,
                        FechaNacimiento = p.FechaNacimiento,
                        RFC = p.RFC,
                        TelefonoCelular = p.TelefonoCelular,
                        Sexo = p.Sexo,
                        NoIFE = p.NumeroIFE,
                        Telefono = p.Telefono,
                        Correo = p.Correo,
                        Credito = p.Credito ?? 0,
                        Plazo = p.Plazo,
                        LimiteCredito = p.LimiteCredito,
                        FechaAlta = p.FechaAlta,
                        NombreIntermediario = p.NombreIntermediario,
                        TelefonoIntermediario = p.TelefonoIntermediario,
                        idCliente = p.idCliente,
                        idOrigen = p.idOrigen

                    }).First();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public int AddPhysicalCustomer(tClientesFisico oCustomer)
        {
            int iResult = 0;

            using (var context = new admDB_SAADDBEntities())
                try
                {

                    if (context.tClientesFisicos.Where(p => ((p.Nombre + " " + p.Apellidos).Trim().ToUpper() == (oCustomer.Nombre + " " + oCustomer.Apellidos).Trim().ToUpper()) && (p.Correo.Trim().ToUpper() == oCustomer.Correo.Trim().ToUpper())).Count() == 0)
                    {
                        context.tClientesFisicos.Add(oCustomer);
                        context.SaveChanges();
                        iResult = oCustomer.idCliente;
                    }

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }

            return iResult;
        }

        public void UpdateCustomerBill(int idCustomer, string name, string phone, string mail, string rfc, string street, string outNum, string inNum, string suburb, int town, int cp)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tClientesFisico tCustomer = context.tClientesFisicos.FirstOrDefault(p => p.idCliente == idCustomer);

                    tCustomer.Nombre = name;
                    tCustomer.Calle = street;
                    tCustomer.NumExt = outNum;
                    tCustomer.NumInt = inNum;
                    tCustomer.Colonia = suburb;
                    tCustomer.idMunicipio = town;
                    tCustomer.CP = cp;
                    tCustomer.RFC = rfc;
                    tCustomer.Telefono = phone;
                    tCustomer.Correo = mail;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public bool UpdateCustomer(PhysicalCustomerViewModel oCustomer)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    if (context.tClientesFisicos.Where(p => ((p.Nombre + " " + p.Apellidos).Trim().ToUpper() == (oCustomer.Nombre + " " + oCustomer.Apellidos).Trim().ToUpper()) && (p.Correo.Trim().ToUpper() == oCustomer.Correo.Trim().ToUpper()) && (p.idCliente != oCustomer.idCliente)).Count() == 0)
                    {

                        tClientesFisico tCustomer = context.tClientesFisicos.FirstOrDefault(p => p.idCliente == oCustomer.idCliente);

                        tCustomer.Nombre = oCustomer.Nombre;
                        tCustomer.Apellidos = oCustomer.Apellidos;
                        tCustomer.Calle = oCustomer.Calle;
                        tCustomer.NumExt = oCustomer.NumExt;
                        tCustomer.NumInt = oCustomer.NumInt;
                        tCustomer.Colonia = oCustomer.Colonia;
                        tCustomer.idMunicipio = oCustomer.idMunicipio;
                        tCustomer.CP = oCustomer.CP;
                        tCustomer.FechaNacimiento = oCustomer.FechaNacimiento;
                        tCustomer.RFC = oCustomer.RFC;
                        tCustomer.TelefonoCelular = oCustomer.TelefonoCelular;
                        tCustomer.Telefono = oCustomer.Telefono;
                        tCustomer.Correo = oCustomer.Correo;
                        tCustomer.Sexo = oCustomer.Sexo;
                        tCustomer.NumeroIFE = oCustomer.NoIFE;
                        tCustomer.Credito = oCustomer.Credito;
                        tCustomer.Plazo = oCustomer.Plazo;
                        tCustomer.LimiteCredito = oCustomer.LimiteCredito;
                        tCustomer.FechaAlta = oCustomer.FechaAlta;
                        tCustomer.NombreIntermediario = oCustomer.NombreIntermediario;
                        tCustomer.TelefonoIntermediario = oCustomer.TelefonoIntermediario;
                        tCustomer.idCliente = oCustomer.idCliente;
                        tCustomer.idOrigen = oCustomer.idOrigen;

                        context.SaveChanges();

                        return true;

                    }
                    else
                    {

                        return false;

                    }

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public void UpdateIFE(int idCustomer, string noIFE)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tClientesFisico oCustomer = context.tClientesFisicos.FirstOrDefault(c => c.idCliente == idCustomer);

                    oCustomer.NumeroIFE = noIFE;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public bool DeletePhysicalCustomer(int idCustomer)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var customerEraser = context.tBorradorVentas.AsEnumerable().Any(p => p.idDespacho == idCustomer);
                    var quotation = context.tCotizacions.AsEnumerable().Any(p => p.idClienteFisico == idCustomer);
                    var creditnote = context.tNotasCreditoes.AsEnumerable().Any(p => p.idCustomerP == idCustomer);
                    //var pending = context.tPedidos.AsEnumerable().Any(p => p.idClienteFisico == idCustomer);
                    var sales = context.tVentas.AsEnumerable().Any(p => p.idClienteFisico == idCustomer);
                    var views = context.tVistas.AsEnumerable().Any(p => p.idClienteFisico == idCustomer);


                    if ((customerEraser == false) && (quotation == false) && (creditnote == false) && (sales == false) && (views == false))
                    {

                        tClientesFisico oCustomer = context.tClientesFisicos.FirstOrDefault(p => p.idCliente == idCustomer);

                        context.tClientesFisicos.Remove(oCustomer);

                        context.SaveChanges();

                        return true;

                    }
                    else
                    {

                        return false;

                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public object GetPhysicalCustomerButNot(int idCustomer)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tClientesFisicos.Where(p => p.idCliente != idCustomer).Select(p => new PhysicalCustomerViewModel()
                    {

                        Nombre = p.Nombre + " " + p.Apellidos,
                        Calle = p.Calle,
                        NumExt = p.NumExt,
                        NumInt = p.NumInt,
                        Colonia = p.Colonia,
                        idMunicipio = p.idMunicipio,
                        idEstado = context.tEstados.Join(context.tMunicipios, x => x.id_estado, m => m.estado, (x, m) => new { tEstados = x, tMunicipios = m }).Where(m => m.tMunicipios.id_municipio == p.idMunicipio).Select(y => y.tEstados.id_estado).FirstOrDefault(),
                        CP = p.CP,
                        FechaNacimiento = p.FechaNacimiento,
                        RFC = p.RFC,
                        TelefonoCelular = p.TelefonoCelular,
                        Sexo = p.Sexo,
                        NoIFE = p.NumeroIFE,
                        Telefono = p.Telefono,
                        Correo = p.Correo,
                        Credito = p.Credito,
                        Plazo = p.Plazo,
                        LimiteCredito = p.LimiteCredito,
                        FechaAlta = p.FechaAlta,
                        NombreIntermediario = p.NombreIntermediario,
                        TelefonoIntermediario = p.TelefonoIntermediario,
                        idCliente = p.idCliente

                    }).OrderBy(p => p.Nombre);
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }
    }
}