using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class Offices : Base
    {

        public int CountRegisters()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tDespachoes.Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<OfficeViewModel> GetOffices(string office, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var offices = context.tDespachoes.Where(p => p.Nombre.Contains(office) || (String.IsNullOrEmpty(office))).Select(p => new OfficeViewModel()
                    {

                        idDespacho = p.idDespacho,
                        Nombre = p.Nombre,
                        Telefono = p.Telefono,
                        Correo = p.Correo,
                        Comision = p.Comision

                    }).OrderBy(p => p.Nombre);

                    return offices.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<OfficeViewModel> GetOffices()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tDespachoes.Select(p => new OfficeViewModel()
                    {

                        idDespacho = p.idDespacho,
                        Nombre = p.Nombre

                    }).OrderBy(p => p.Nombre).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<OfficeViewModel> GetOffices(string office)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tDespachoes.Where(p => p.Nombre.Contains(office)).Select(p => new OfficeViewModel()
                    {

                        idDespacho = p.idDespacho,
                        Nombre = p.Nombre,
                        Telefono = p.Telefono,
                        Calle = p.Calle,
                        NumExt = p.NumExt,
                        NumInt = p.NumInt,
                        Colonia = p.Colonia,
                        idMunicipio = p.idMunicipio,
                        CP = p.CP,
                        Correo = p.Correo,
                        Comision = p.Comision

                    }).OrderBy(p => p.Nombre).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public int AddOffice(string name, string phone, string street, string extNum, string intNum, string neighborhood, int idTown, int cp, string email, decimal percentage, int idUser, int idOrigin)
        {

            int iResult;

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tDespacho entity = new tDespacho();

                    entity.Nombre = name;
                    entity.Telefono = phone;
                    entity.Calle = street;
                    entity.NumExt = extNum;
                    entity.NumInt = intNum;
                    entity.Colonia = neighborhood;
                    entity.idMunicipio = idTown;
                    entity.CP = cp;
                    entity.Correo = email;
                    entity.Comision = percentage;
                    entity.CreadoPor = idUser;
                    entity.Creado = DateTime.Now;
                    entity.idOrigen = idOrigin;      

                    context.tDespachoes.Add(entity);

                    context.SaveChanges();

                    iResult = entity.idDespacho;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }

            return iResult;

        }

        public bool OfficeExist(string office)
        {
            using (var context = new admDB_SAADDBEntities())
                return context.tDespachoes.Where(p => p.Nombre.ToUpper() == office.ToUpper()).Any();

        }

        public OfficeViewModel GetOffice(int idOffice)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tDespachoes.Where(p => p.idDespacho == idOffice).Select(p => new OfficeViewModel()
                    {

                        idDespacho = p.idDespacho,
                        Nombre = p.Nombre,
                        Telefono = p.Telefono,
                        Calle = p.Calle,
                        NumExt = p.NumExt,
                        NumInt = p.NumInt,
                        Colonia = p.Colonia,
                        idMunicipio = p.idMunicipio,
                        CP = p.CP,
                        Correo = p.Correo,
                        Comision = p.Comision,
                        idOrigen = p.idOrigen
                        
                    }).FirstOrDefault();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public void UpdateOffice(int idOffice, string name, string phone, string street, string extNum, string intNum, string neighborhood, int idTown, int cp, string email, decimal percentage,int idOrigin)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tDespacho oDespacho = context.tDespachoes.FirstOrDefault(p => p.idDespacho == idOffice);

                    oDespacho.Nombre = name;
                    oDespacho.Telefono = phone;
                    oDespacho.Calle = street;
                    oDespacho.NumExt = extNum;
                    oDespacho.NumInt = intNum;
                    oDespacho.Colonia = neighborhood;
                    oDespacho.idMunicipio = idTown;
                    oDespacho.CP = cp;
                    oDespacho.Correo = email;
                    oDespacho.Comision = percentage;
                    oDespacho.idOrigen = idOrigin;

                    context.SaveChanges();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public bool DeleteOffice(int idDespacho)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var customerEraser = context.tBorradorVentas.AsEnumerable().Any(p => p.idDespacho == idDespacho);
                    var quotation = context.tCotizacions.AsEnumerable().Any(p => p.idDespacho == idDespacho);
                    var sales = context.tVentas.AsEnumerable().Any(p => p.idDespacho == idDespacho);
                    var views = context.tVistas.AsEnumerable().Any(p => p.idDespacho == idDespacho);


                    if ((customerEraser == false) && (quotation == false) && (sales == false) && (views == false))
                    {

                        tDespacho oOffice = context.tDespachoes.FirstOrDefault(p => p.idDespacho == idDespacho);

                        context.tDespachoes.Remove(oOffice);

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