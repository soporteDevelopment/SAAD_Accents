using ADEntities.Models;
using ADEntities.ViewModels;
using ADEntities.ViewModels.VirtualStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries
{
    public class CustomersVirtualStore
    {
        //Get
        public Tuple<int, List<CustomerVirtualStoreViewModel>> Get(string name, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var customers = context.tClientesTiendaVirtuals
                    .Where(p => (p.Nombre + p.Apellidos).Contains(name) || String.IsNullOrEmpty(name))
                    .Select(p => new CustomerVirtualStoreViewModel()
                    {
                        IdCliente = p.idCliente,
                        Nombre = p.Nombre,
                        Apellidos = p.Apellidos,
                        Contrasena = p.Contrasena,
                        TelefonoCelular = p.TelefonoCelular,
                        Telefono = p.Telefono,
                        Correo = p.Correo,
                        FechaNacimiento = p.FechaNacimiento.Value.ToString("yyyy/MM/dd"),
                        Sexo = p.Sexo,
                        Calle = p.Calle,
                        NumInt = p.NumInt,
                        NumExt = p.NumExt,
                        Colonia = p.Colonia,
                        IdMunicipio = p.idMunicipio,
                        IdEstado = p.tMunicipio.tEstado.id_estado,
                        Cp = p.CP,
                        Activo = p.Activo
                    }).OrderByDescending(p => p.Nombre).Skip(page * pageSize).Take(pageSize).ToList();

                var total = context.tClientesTiendaVirtuals.Count();

                return Tuple.Create(total, customers);
            }
        }       

        public CustomerVirtualStoreViewModel GetById(int id)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                return context.tClientesTiendaVirtuals.Where(p => p.idCliente == id)
                    .Select(p => new CustomerVirtualStoreViewModel()
                    {
                        IdCliente = p.idCliente,
                        Nombre = p.Nombre,
                        Apellidos = p.Apellidos,
                        Contrasena = p.Contrasena,
                        TelefonoCelular = p.TelefonoCelular,
                        Telefono = p.Telefono,
                        Correo = p.Correo,
                        FechaNacimiento = p.FechaNacimiento.Value.ToString("yyyy/MM/dd"),
                        Sexo = p.Sexo,
                        Calle = p.Calle,
                        NumInt = p.NumInt,
                        NumExt = p.NumExt,
                        Colonia = p.Colonia,
                        IdMunicipio = p.idMunicipio,
                        IdEstado = p.tMunicipio.tEstado.id_estado,
                        Cp = p.CP,
                        Activo = p.Activo
                    }).FirstOrDefault();
            }
        }        

        //Delete
        public int Delete(int id)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var entity = context.tClientesTiendaVirtuals.Find(id);

                entity.Activo = !entity.Activo;

                return context.SaveChanges();
            }
        }
    }
}