using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class Textiles : Base
    {
        public int CountRegisters()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tTipoMedida.Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<TextilesViewModel> GetTextiles()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tTextiles.Where(p => p.Habilitado == 1).Select(p => new TextilesViewModel()
                    {
                        idTextiles = p.idTextiles,
                        Marca = p.Marca,
                        NombreTela = p.NombreTela,
                        Coleccion = p.Coleccion,
                        AnchoTela = p.AnchoTela,
                        Precio = p.Precio,
                        Moneda = p.Moneda

                    }).OrderBy(p => p.NombreTela).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int AddTextiles(List<TextilesViewModel> textiles)
        {

            int iResult;

            using (var context = new admDB_SAADDBEntities())
                try
                {

                    var tTextilesU = context.tTextiles.ToList();
                    foreach (var textile in tTextilesU)
                    {
                        textile.Habilitado = 0;
                    }

                    foreach (var textile in textiles)
                    {
                        tTextile oTextiles = new tTextile();
                        oTextiles.Marca = textile.Marca;
                        oTextiles.NombreTela = textile.NombreTela;
                        oTextiles.Coleccion = textile.Coleccion;
                        oTextiles.AnchoTela = textile.AnchoTela;
                        oTextiles.Precio = textile.Precio;
                        oTextiles.Moneda = textile.Moneda;
                        oTextiles.Habilitado = 1;
                        context.tTextiles.Add(oTextiles);
                    }
                    context.SaveChanges();

                    iResult = 1;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }

            return iResult;
        }

        class TextilesException : Exception
        {
            public string messageEx { get; set; }

            public TextilesException(string message)
            {
                messageEx = message;
            }
        }
    }
}