using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Converts
{
    public class MetropolitanAreas
    {
        public static tAreaMetropolitana PrepareAdd(MetropolitanAreaViewModel entity)
        {
            tAreaMetropolitana result = new tAreaMetropolitana();
            result.idMunicipio = entity.idMunicipio;

            return result;
        }

        public static tAreaMetropolitana PrepareUpdate(MetropolitanAreaViewModel entity)
        {
            tAreaMetropolitana result = new tAreaMetropolitana();

            result.idAreaMetropolitana = entity.idAreaMetropolitana;
            result.idMunicipio = entity.idMunicipio;

            return result;
        }

        public static MetropolitanAreaViewModel TableToModel(tAreaMetropolitana entity)
        {
            MetropolitanAreaViewModel result = new MetropolitanAreaViewModel();

            result.idAreaMetropolitana = entity.idAreaMetropolitana;
            result.idMunicipio = entity.idMunicipio;

            return result;
        }

        public static List<MetropolitanAreaViewModel> TableToListModel(List<tAreaMetropolitana> entities)
        {
            return entities.Select(p=> new MetropolitanAreaViewModel()
            {
                idAreaMetropolitana = p.idAreaMetropolitana,
                idMunicipio = p.idMunicipio,
                Municipio = new TownViewModel()
                {
                    idMunicipio = p.tMunicipio.id_municipio,
                    Municipio = p.tMunicipio.nombre_municipio,
                    Estado = new StateViewModel()
                    {
                        idEstado = p.tMunicipio.tEstado.id_estado,
                        Estado = p.tMunicipio.tEstado.estado
                    }
                }
            }).ToList();
        }

        public static tAreaMetropolitana ModelToTable(MetropolitanAreaViewModel model)
        {
            tAreaMetropolitana result = new tAreaMetropolitana();

            result.idAreaMetropolitana = model.idAreaMetropolitana;
            result.idMunicipio = model.idMunicipio;

            return result;
        }
    }
}