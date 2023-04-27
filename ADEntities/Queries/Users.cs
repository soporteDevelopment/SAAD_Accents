using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using ADEntities.Common;
using System.Data.Entity.Validation;

namespace ADEntities.Queries
{
    public class Users : Base
    {

        public int CountRegisters()
        {
            using (var context = new admDB_SAADDBEntities())
                return context.tUsuarios.Count();
        }

        public UserViewModel ValidateUser(string user, string password)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tUsuarios.Where(p => p.Correo.ToUpper().ToUpper().Equals(user) && p.Contrasena.Equals(password) && p.Estatus == TypesUser.EstatusActivo).Select(x =>
                      new UserViewModel()
                      {

                          idUsuario = x.idUsuario,
                          Nombre = x.Nombre,
                          Apellidos = x.Apellidos,
                          NombreCompleto = x.Nombre + " " + x.Apellidos,
                          Calle = x.Calle,
                          NumExt = x.NumExt,
                          NumInt = x.NumInt,
                          Colonia = x.Colonia,
                          idMunicipio = (int)x.idMunicipio,
                          CP = (int)x.CP,
                          TelefonoCelular = x.TelefonoCelular,
                          Telefono = x.Telefono,
                          FechaIngreso = (DateTime)x.FechaIngreso,
                          FechaNacimiento = (DateTime)x.FechaNacimiento,
                          Sexo = x.Sexo,
                          Correo = x.Correo,
                          idNivelAcademico = (int)x.idNivelAcademico,
                          idPerfil = x.idPerfil,
                          Contrasena = x.Contrasena,
                          Estatus = (short)x.Estatus,
                          idSucursal = (int)x.idSucursal,
                          Ventas = x.Ventas ?? 0,
                          Restringido = x.Restringido ?? 0,
                          Facturar = x.Facturar ?? false
                      }).SingleOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<UserViewModel> GetUsers()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tUsuarios.Where(p => p.Estatus == TypesUser.EstatusActivo).Select(p => new UserViewModel()
                    {
                        idUsuario = p.idUsuario,
                        Nombre = p.Nombre,
                        Apellidos = p.Apellidos,
                        Calle = p.Calle,
                        NumExt = p.NumExt,
                        NumInt = p.NumInt,
                        Colonia = p.Colonia,
                        idMunicipio = (int)p.idMunicipio,
                        CP = (int)p.CP,
                        TelefonoCelular = p.TelefonoCelular,
                        Telefono = p.Telefono,
                        FechaIngreso = (DateTime)p.FechaIngreso,
                        FechaNacimiento = (DateTime)p.FechaNacimiento,
                        Sexo = p.Sexo,
                        Correo = p.Correo,
                        idNivelAcademico = (int)p.idNivelAcademico,
                        idPerfil = p.idPerfil,
                        Estatus = (short)p.Estatus,
                        NombreCompleto = p.Nombre + " " + p.Apellidos,
                        Ventas = p.Ventas ?? 0,
                        Restringido = p.Restringido ?? 0,
                        Facturar = p.Facturar ?? false
                    }).OrderBy(p => p.NombreCompleto).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<UserViewModel> GetAllUsers()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tUsuarios.Select(p => new UserViewModel()
                    {
                        idUsuario = p.idUsuario,
                        Nombre = p.Nombre,
                        Apellidos = p.Apellidos,
                        NombreCompleto = p.Nombre + " " + p.Apellidos,
                        Calle = p.Calle,
                        NumExt = p.NumExt,
                        NumInt = p.NumInt,
                        Colonia = p.Colonia,
                        idMunicipio = (int)p.idMunicipio,
                        CP = (int)p.CP,
                        TelefonoCelular = p.TelefonoCelular,
                        Telefono = p.Telefono,
                        FechaIngreso = (DateTime)p.FechaIngreso,
                        FechaNacimiento = (DateTime)p.FechaNacimiento,
                        Sexo = p.Sexo,
                        Correo = p.Correo,
                        idNivelAcademico = (int)p.idNivelAcademico,
                        idPerfil = p.idPerfil,
                        Estatus = (short)p.Estatus,
                        Ventas = p.Ventas ?? 0,
                        Restringido = p.Restringido ?? 0,
                        Facturar = p.Facturar ?? false
                    }).OrderBy(p => p.NombreCompleto).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }
       
        public List<UserViewModel> GetUsersButNot(int idUser)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tUsuarios.Where(p => p.Estatus == TypesUser.EstatusActivo && p.idUsuario != idUser).Select(p => new UserViewModel()
                    {
                        idUsuario = p.idUsuario,
                        Nombre = p.Nombre,
                        Apellidos = p.Apellidos,
                        Calle = p.Calle,
                        NumExt = p.NumExt,
                        NumInt = p.NumInt,
                        Colonia = p.Colonia,
                        idMunicipio = (int)p.idMunicipio,
                        CP = (int)p.CP,
                        TelefonoCelular = p.TelefonoCelular,
                        Telefono = p.Telefono,
                        FechaIngreso = (DateTime)p.FechaIngreso,
                        FechaNacimiento = (DateTime)p.FechaNacimiento,
                        Sexo = p.Sexo,
                        Correo = p.Correo,
                        idNivelAcademico = (int)p.idNivelAcademico,
                        idPerfil = p.idPerfil,
                        Estatus = (short)p.Estatus,
                        NombreCompleto = p.Nombre + " " + p.Apellidos,
                        Ventas = p.Ventas ?? 0,
                        Restringido = p.Restringido ?? 0,
                        Facturar = p.Facturar ?? false
                    }).OrderBy(p => p.NombreCompleto).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<UserViewModel> GetUsers(string name)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tUsuarios.Where(p => (p.Nombre + " " + p.Apellidos).Contains(name) || name == String.Empty).Select(p => new UserViewModel()
                    {
                        idUsuario = p.idUsuario,
                        Nombre = p.Nombre,
                        Apellidos = p.Apellidos,
                        Calle = p.Calle,
                        NumExt = p.NumExt,
                        NumInt = p.NumInt,
                        Colonia = p.Colonia,
                        idMunicipio = (int)p.idMunicipio,
                        CP = (int)p.CP,
                        TelefonoCelular = p.TelefonoCelular,
                        Telefono = p.Telefono,
                        FechaIngreso = (DateTime)p.FechaIngreso,
                        FechaNacimiento = (DateTime)p.FechaNacimiento,
                        Sexo = p.Sexo,
                        Correo = p.Correo,
                        idNivelAcademico = (int)p.idNivelAcademico,
                        idPerfil = p.idPerfil,
                        Estatus = (short)p.Estatus,
                        NombreCompleto = p.Nombre + " " + p.Apellidos,
                        Ventas = p.Ventas ?? 0,
                        Restringido = p.Restringido ?? 0,
                        Facturar = p.Facturar ?? false
                    }).OrderBy(p => p.NombreCompleto).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }
         public List<UserViewModel> GetAttentionTicket()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tUsuarios.Where(p => p.AtencionTicket == true).Select(p => new UserViewModel()
                    {
                        idUsuario = p.idUsuario,
                        Nombre = p.Nombre,
                        Apellidos = p.Apellidos,
                        Calle = p.Calle,
                        NumExt = p.NumExt,
                        NumInt = p.NumInt,
                        Colonia = p.Colonia,
                        idMunicipio = (int)p.idMunicipio,
                        CP = (int)p.CP,
                        TelefonoCelular = p.TelefonoCelular,
                        Telefono = p.Telefono,
                        FechaIngreso = (DateTime)p.FechaIngreso,
                        FechaNacimiento = (DateTime)p.FechaNacimiento,
                        Sexo = p.Sexo,
                        Correo = p.Correo,
                        idNivelAcademico = (int)p.idNivelAcademico,
                        idPerfil = p.idPerfil,
                        Estatus = (short)p.Estatus,
                        NombreCompleto = p.Nombre + " " + p.Apellidos,
                        Ventas = p.Ventas ?? 0,
                        Restringido = p.Restringido ?? 0,
                        Facturar = p.Facturar ?? false
                    }).OrderBy(p => p.NombreCompleto).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<UserViewModel> GetUsers(int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var users = context.tUsuarios.Select(p => new UserViewModel()
                    {
                        idUsuario = p.idUsuario,
                        Nombre = p.Nombre,
                        Apellidos = p.Apellidos,
                        Contrasena = p.Contrasena,
                        Calle = p.Calle,
                        NumExt = p.NumExt,
                        NumInt = p.NumInt,
                        Colonia = p.Colonia,
                        idMunicipio = p.idMunicipio,
                        CP = p.CP,
                        TelefonoCelular = p.TelefonoCelular,
                        Telefono = p.Telefono,
                        FechaIngreso = (DateTime)p.FechaIngreso,
                        FechaNacimiento = (DateTime)p.FechaNacimiento,
                        Sexo = p.Sexo,
                        Correo = p.Correo,
                        idNivelAcademico = p.idNivelAcademico,
                        idPerfil = p.idPerfil,
                        Estatus = p.Estatus,
                        Ventas = p.Ventas ?? 0,
                        Restringido = p.Restringido ?? 0,
                        Facturar = p.Facturar ?? false
                    }).OrderBy(p => p.Nombre).ThenByDescending(p => p.Apellidos);

                    return users.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public UserViewModel GetUser(int idUser)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tUsuarios.Where(p => p.idUsuario == idUser).Select(p => new UserViewModel()
                    {

                        idUsuario = p.idUsuario,
                        Nombre = p.Nombre,
                        Apellidos = p.Apellidos,
                        Calle = p.Calle,
                        NumExt = p.NumExt,
                        NumInt = p.NumInt,
                        Colonia = p.Colonia,
                        idMunicipio = p.idMunicipio,
                        idEstado = context.tEstados.Join(context.tMunicipios, x => x.id_estado, m => m.estado, (x, m) => new { tEstados = x, tMunicipios = m }).Where(m => m.tMunicipios.id_municipio == p.idMunicipio).Select(y => y.tEstados.id_estado).FirstOrDefault(),
                        CP = p.CP,
                        TelefonoCelular = p.TelefonoCelular,
                        Telefono = p.Telefono,
                        FechaIngreso = p.FechaIngreso,
                        FechaNacimiento = p.FechaNacimiento,
                        Sexo = p.Sexo,
                        Correo = p.Correo,
                        idNivelAcademico = p.idNivelAcademico,
                        titulo = p.Titulo,
                        idPerfil = p.idPerfil,
                        Sucursal = new BranchStoreViewModel()
                        {
                            idSucursal = p.tSucursale.idSucursal,
                            Nombre = p.tSucursale.Nombre,
                            Descripcion = p.tSucursale.Descripcion,
                            Horarios = p.tSucursale.Horarios,
                            Telefono = p.tSucursale.Telefono,
                            DatosFiscales = p.tSucursale.DatosFiscales
                        },
                        Estatus = p.Estatus,
                        Ventas = p.Ventas ?? 0,
                        Restringido = p.Restringido ?? 0,
                        Facturar = p.Facturar ?? false,
                        AtencionTicket = p.AtencionTicket ?? false                        
                    }).First();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<AcademicDegreeViewModel> GetAcademicDegree()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tNivelAcademicoes.Select(p => new AcademicDegreeViewModel
                    {

                        idNivelAcademico = p.idNivelAcademico,
                        NivelAcademico = p.NivelAcademico

                    }).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int AddUser(tUsuario oUser)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    int iResult = 0;

                    context.tUsuarios.Add(oUser);

                    context.SaveChanges();

                    iResult = oUser.idUsuario;

                    return iResult;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public bool UpdateUser(tUsuario oUser)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    tUsuario tUser = context.tUsuarios.FirstOrDefault(p => p.idUsuario == oUser.idUsuario);

                    tUser.Nombre = oUser.Nombre;
                    tUser.Apellidos = oUser.Apellidos;
                    tUser.Calle = oUser.Calle;
                    tUser.NumExt = oUser.NumExt;
                    tUser.NumInt = oUser.NumInt;
                    tUser.Colonia = oUser.Colonia;
                    tUser.idMunicipio = oUser.idMunicipio;
                    tUser.CP = oUser.CP;
                    tUser.Telefono = oUser.Telefono;
                    tUser.TelefonoCelular = oUser.TelefonoCelular;
                    tUser.FechaNacimiento = oUser.FechaNacimiento;
                    tUser.Sexo = oUser.Sexo;
                    tUser.Correo = oUser.Correo;
                    tUser.idNivelAcademico = oUser.idNivelAcademico;
                    tUser.Titulo = oUser.Titulo;
                    tUser.idPerfil = oUser.idPerfil;
                    tUser.idSucursal = oUser.idSucursal;
                    tUser.FechaIngreso = oUser.FechaIngreso;
                    tUser.Ventas = oUser.Ventas;
                    tUser.Restringido = oUser.Restringido;
                    tUser.Facturar = oUser.Facturar;
                    tUser.AtencionTicket = oUser.AtencionTicket;

                    context.SaveChanges();

                    return true;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public short UpdateStatusUser(int idUser, short status)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tUsuario tUser = context.tUsuarios.First(p => p.idUsuario == idUser);
                    tUser.Intentos = 0;
                    tUser.Estatus = status;
                    context.SaveChanges();

                    return (short)tUser.Estatus;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public bool ValidatePassword(int idUser, string password)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    if (context.tUsuarios.Where(p => p.idUsuario == idUser && p.Contrasena == password).Count() > 0)
                    {
                        return true;
                    }

                    return false;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public bool ChangePassword(int idUser, string password)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tUsuario tUser = context.tUsuarios.First(p => p.idUsuario == idUser);
                    tUser.Contrasena = password;
                    context.SaveChanges();

                    return true;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public bool ChangePassword(string email, string password)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    bool bResult = false;

                    if (this.ValidateEmail(email))
                    {

                        tUsuario user = context.tUsuarios.SingleOrDefault(p => p.Correo == email);
                        user.Contrasena = password;
                        context.SaveChanges();

                        bResult = true;
                    }

                    return bResult;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public bool ValidateEmail(string email)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    bool bResult = false;

                    int count = context.tUsuarios.Where(p => p.Correo == email && p.Estatus == TypesUser.EstatusActivo).Count();

                    if (count > 0)
                    {
                        bResult = true;
                    }

                    return bResult;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public short? VerifyStateUser(int idUser)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tUsuarios.Where(p => p.idUsuario == idUser).Select(p => p.Estatus).First();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int AddTryLogin(string user, string password)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    int result = 0;

                    tUsuario oUser = context.tUsuarios.FirstOrDefault(p => p.Correo.ToUpper() == user.ToUpper());

                    if (oUser != null)
                    {

                        if (oUser.Contrasena != password)
                        {
                            int intentos = oUser.Intentos ?? 0;

                            oUser.Intentos = intentos + 1;

                            if (oUser.Intentos >= 5)
                            {
                                oUser.Estatus = 0;
                            }

                            context.SaveChanges();

                            result = oUser.Intentos ?? 0;

                        }

                    }

                    return result;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void CleanTryLogin(int idUser)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tUsuario oUser = context.tUsuarios.FirstOrDefault(p => p.idUsuario == idUser);

                    oUser.Intentos = 0;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<UserViewModel> GetUsersBill()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tUsuarios.Where(p => p.Estatus == TypesUser.EstatusActivo && p.Facturar == true).Select(p => new UserViewModel()
                    {
                        idUsuario = p.idUsuario,
                        Nombre = p.Nombre,
                        Apellidos = p.Apellidos,
                        Calle = p.Calle,
                        NumExt = p.NumExt,
                        NumInt = p.NumInt,
                        Colonia = p.Colonia,
                        idMunicipio = (int)p.idMunicipio,
                        CP = (int)p.CP,
                        TelefonoCelular = p.TelefonoCelular,
                        Telefono = p.Telefono,
                        FechaIngreso = (DateTime)p.FechaIngreso,
                        FechaNacimiento = (DateTime)p.FechaNacimiento,
                        Sexo = p.Sexo,
                        Correo = p.Correo,
                        idNivelAcademico = (int)p.idNivelAcademico,
                        idPerfil = p.idPerfil,
                        Estatus = (short)p.Estatus,
                        NombreCompleto = p.Nombre + " " + p.Apellidos,
                        Ventas = p.Ventas ?? 0,
                        Restringido = p.Restringido ?? 0,
                        Facturar = p.Facturar ?? false
                    }).OrderBy(p => p.NombreCompleto).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }
    }
}