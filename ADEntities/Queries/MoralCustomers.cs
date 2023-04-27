using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class MoralCustomers : Base
    {

        public int CountRegisters()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tClientesMorales.Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<MoralCustomerViewModel> GetMoralCustomers()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tClientesMorales.Select(p => new MoralCustomerViewModel()
                    {

                        Nombre = p.Nombre,
                        Calle = p.Calle,
                        NumExt = p.NumExt,
                        NumInt = p.NumInt,
                        Colonia = p.Colonia,
                        idMunicipio = (int)p.idMunicipio,
                        idEstado = context.tEstados.Join(context.tMunicipios, x => x.id_estado, m => m.estado, (x, m) => new { tEstados = x, tMunicipios = m }).Where(m => m.tMunicipios.id_municipio == p.idMunicipio).Select(y => y.tEstados.id_estado).FirstOrDefault(),
                        CP = (int)p.CP,
                        Nacionalidad = (short)p.Nacionalidad,
                        RFC = p.RFC,
                        TelefonoCelular = p.TelefonoCelular,
                        Telefono = p.Telefono,
                        Correo = p.Correo,
                        SitioWeb = p.SitioWeb,
                        NombreContacto = p.NombreContacto,
                        TelefonoContacto = p.TelefonoContacto,
                        Credito = (short)p.Credito,
                        Plazo = (int)p.Plazo,
                        LimiteCredito = (decimal)p.LimiteCredito,
                        FechaAlta = (DateTime)p.FechaAlta,
                        idCliente = p.idCliente

                    }).OrderBy(p => p.Nombre).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<MoralCustomerViewModel> GetMoralCustomers(string costumer, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var Customers = context.tClientesMorales.Where(p => p.Nombre.Trim().ToUpper().Contains(costumer.Trim().ToUpper()) || String.IsNullOrEmpty(costumer)).Select(p => new MoralCustomerViewModel()
                    {

                        Nombre = p.Nombre,
                        Calle = p.Calle,
                        NumExt = p.NumExt,
                        NumInt = p.NumInt,
                        Colonia = p.Colonia,
                        idMunicipio = p.idMunicipio,
                        CP = p.CP,
                        Nacionalidad = p.Nacionalidad,
                        RFC = p.RFC,
                        TelefonoCelular = p.TelefonoCelular,
                        Telefono = p.Telefono,
                        Correo = p.Correo,
                        SitioWeb = p.SitioWeb,
                        NombreContacto = p.NombreContacto,
                        TelefonoContacto = p.TelefonoContacto,
                        Credito = p.Credito,
                        Plazo = (int)p.Plazo,
                        LimiteCredito = (decimal)p.LimiteCredito,
                        FechaAlta = (DateTime)p.FechaAlta,
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

        public MoralCustomerViewModel GetMoralCustomer(int idCliente)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tClientesMorales.Where(p => p.idCliente == idCliente).Select(p => new MoralCustomerViewModel
                    {

                        Nombre = p.Nombre,
                        Calle = p.Calle,
                        NumExt = p.NumExt,
                        NumInt = p.NumInt,
                        Colonia = p.Colonia,
                        idMunicipio = p.idMunicipio,
                        idEstado = context.tEstados.Join(context.tMunicipios, x => x.id_estado, m => m.estado, (x, m) => new { tEstados = x, tMunicipios = m }).Where(m => m.tMunicipios.id_municipio == p.idMunicipio).Select(y => y.tEstados.id_estado).FirstOrDefault(),
                        CP = p.CP,
                        Nacionalidad = p.Nacionalidad,
                        RFC = p.RFC,
                        TelefonoCelular = p.TelefonoCelular,
                        Telefono = p.Telefono,
                        Correo = p.Correo,
                        SitioWeb = p.SitioWeb,
                        NombreContacto = p.NombreContacto,
                        TelefonoContacto = p.TelefonoContacto,
                        CorreoContacto = p.CorreoContacto,
                        Credito = p.Credito ?? 0,
                        Plazo = (int)p.Plazo,
                        LimiteCredito = (decimal)p.LimiteCredito,
                        FechaAlta = (DateTime)p.FechaAlta,
                        idCliente = p.idCliente,
                        idOrigen = (int)p.idOrigen

                    }).First();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public int AddMoralCustomer(tClientesMorale oCustomer)
        {
            int iResult = 0;

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    if (context.tClientesMorales.Where(p => (p.Nombre) == (oCustomer.Nombre)).Count() == 0)
                    { 
                        context.tClientesMorales.Add(oCustomer);
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

        public void UpdateCustomerBill(int idCustomer, string name, string phone, string mail, string rfc, string street, string outNum, string inNum, string suburb, int town, int cp, int idOrigin)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    tClientesMorale tCustomer = context.tClientesMorales.FirstOrDefault(p => p.idCliente == idCustomer);

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
                    tCustomer.idOrigen = idOrigin;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public bool UpdateCustomer(MoralCustomerViewModel oCustomer)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {

                    if (context.tClientesMorales.Where(p => (p.Nombre == oCustomer.Nombre) && (p.idCliente != oCustomer.idCliente)).Count() == 0)
                    {

                        tClientesMorale tCustomer = context.tClientesMorales.FirstOrDefault(p => p.idCliente == oCustomer.idCliente);

                        tCustomer.Nombre = oCustomer.Nombre;
                        tCustomer.Calle = oCustomer.Calle;
                        tCustomer.NumExt = oCustomer.NumExt;
                        tCustomer.NumInt = oCustomer.NumInt;
                        tCustomer.Colonia = oCustomer.Colonia;
                        tCustomer.idMunicipio = oCustomer.idMunicipio;
                        tCustomer.CP = oCustomer.CP;
                        tCustomer.Nacionalidad = oCustomer.Nacionalidad;
                        tCustomer.RFC = oCustomer.RFC;
                        tCustomer.TelefonoCelular = oCustomer.TelefonoCelular;
                        tCustomer.Telefono = oCustomer.Telefono;
                        tCustomer.Correo = oCustomer.Correo;
                        tCustomer.SitioWeb = oCustomer.SitioWeb;
                        tCustomer.NombreContacto = oCustomer.NombreContacto;
                        tCustomer.TelefonoContacto = oCustomer.TelefonoContacto;
                        tCustomer.CorreoContacto = oCustomer.CorreoContacto;
                        tCustomer.Credito = oCustomer.Credito;
                        tCustomer.Plazo = oCustomer.Plazo;
                        tCustomer.LimiteCredito = oCustomer.LimiteCredito;
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

        public List<CustomerViewModel> GetCustomers(string name)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tClientesMorales.Where(p => p.Nombre.Contains(name)).Select(p => new CustomerViewModel
                    {
                        Nombre = p.Nombre,
                        idCliente = p.idCliente

                    }).Union(context.tClientesFisicos.Where(p => p.Nombre.Contains(name)).Select(p => new CustomerViewModel
                    {
                        Nombre = p.Nombre + " " + p.Apellidos,
                        idCliente = p.idCliente

                    })).OrderBy(p => p.Nombre).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public bool DeleteMoralCustomer(int idCustomer)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var customerEraser = context.tBorradorVentas.AsEnumerable().Any(p => p.idClienteMoral == idCustomer);
                    var quotation = context.tCotizacions.AsEnumerable().Any(p => p.idClienteMoral == idCustomer);
                    var creditnote = context.tNotasCreditoes.AsEnumerable().Any(p => p.idCustomerM == idCustomer);
                    //var pending = context.tPedidos.AsEnumerable().Any(p => p.idClienteMoral == idCustomer);
                    var sales = context.tVentas.AsEnumerable().Any(p => p.idClienteMoral == idCustomer);
                    var views = context.tVistas.AsEnumerable().Any(p => p.idClienteMoral == idCustomer);

                    if ((customerEraser == false) && (quotation == false) && (creditnote == false) && (sales == false) && (views == false))
                    {

                        tClientesMorale oCustomer = context.tClientesMorales.FirstOrDefault(p => p.idCliente == idCustomer);

                        context.tClientesMorales.Remove(oCustomer);

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

    }
}