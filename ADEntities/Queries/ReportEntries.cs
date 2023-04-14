using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries
{
	public class ReportEntries
	{
		//Get
		public List<ReportEntriesViewModel> Get(DateTime start, DateTime end, string remission)
		{
			using (var context = new admDB_SAADDBEntities())
			{
				var dtStart = start.Date + new TimeSpan(0, 0, 0);
				var dtEnd = end.Date + new TimeSpan(23, 59, 59);

				return context.tReporteCajas.Where(p => (p.Fecha >= dtStart && p.Fecha <= dtEnd) &&
					p.tEntradas.Any(o => o.tVenta.Remision.Contains(remission) || String.IsNullOrEmpty(remission)))
					.Select(p => new ReportEntriesViewModel()
					{
						idReporteCaja = p.idReporteCaja,
						Fecha = p.Fecha,
						Comentarios = p.Comentarios,
						CantidadEgreso = p.CantidadEgreso,
						CantidadIngreso = p.CantidadIngreso,
						Revisado = p.Revisado,
						Ingresos = p.tEntradas.Select(q => new EntryViewModel()
						{
							idEntrada = q.idEntrada,
							Tipo = q.Tipo,
							idVenta = q.idVenta,
							Fecha = q.Fecha,
							EntregadaPor = q.EntregadaPor,
							Entregada = q.tUsuario1.Nombre + " " + q.tUsuario1.Apellidos,
							EntregadaOtro = q.EntregadaOtro,
							Cantidad = q.Cantidad,
							Comentarios = q.Comentarios,
							Remision = q.tVenta.Remision,
							Estatus = q.Estatus,
							CreadoPor = p.CreadoPor,
							Creado = p.Creado,
							ModificadoPor = p.ModificadoPor,
							Modificado = p.Modificado
						}).OrderByDescending(q => q.Creado).ToList(),
						Egresos = p.tSalidas.Select(q => new EgressViewModel()
						{
							idSalida = q.idSalida,
							Fecha = q.Fecha,
							RecibidaPor = q.RecibidaPor,
							Recibida = q.tUsuario2.Nombre + " " + q.tUsuario2.Apellidos,
							RecibidaOtro = q.RecibidaOtro,
							Tipo = q.Tipo,
							idVenta = q.idVenta,
							Cantidad = q.Cantidad,
							Comentarios = q.Comentarios,
							Estatus = q.Estatus,
							CreadoPor = q.CreadoPor,
							Creado = q.Creado,
							ModificadoPor = q.ModificadoPor,
							Modificado = q.Modificado
						}).OrderByDescending(q => q.Creado).ToList(),
						CreadoPor = p.CreadoPor,
						Creado = p.Creado,
						ModificadoPor = p.ModificadoPor,
						Modificado = p.Modificado
					}).OrderByDescending(p => p.Creado).ToList();
			}
		}

		public ReportEntriesViewModel GetById(int id)
		{
			using (var context = new admDB_SAADDBEntities())
			{
				return context.tReporteCajas.Where(p => p.idReporteCaja == id)
					.Select(p => new ReportEntriesViewModel()
					{
						idReporteCaja = p.idReporteCaja,
						Fecha = p.Fecha,
						Comentarios = p.Comentarios,
						CantidadEgreso = p.CantidadEgreso,
						CantidadIngreso = p.CantidadIngreso,
						Revisado = p.Revisado,
						Ingresos = context.tEntradas.Where(q => q.idReporteCaja == p.idReporteCaja).Select(q => new EntryViewModel()
						{
							idEntrada = q.idEntrada,
							Tipo = q.Tipo,
							idVenta = q.idVenta,
							Fecha = q.Fecha,
							EntregadaPor = q.EntregadaPor,
							Entregada = q.tUsuario1.Nombre + " " + q.tUsuario1.Apellidos,
							EntregadaOtro = q.EntregadaOtro,
							Cantidad = q.Cantidad,
							Comentarios = q.Comentarios,
							Remision = q.tVenta.Remision,
							Estatus = q.Estatus,
							CreadoPor = p.CreadoPor,
							Creado = p.Creado,
							ModificadoPor = p.ModificadoPor,
							Modificado = p.Modificado
						}).OrderByDescending(q => q.Creado).ToList(),
						Egresos = p.tSalidas.Where(q => q.idReporteCaja == p.idReporteCaja).Select(q => new EgressViewModel()
						{
							idSalida = q.idSalida,
							Fecha = q.Fecha,
							RecibidaPor = q.RecibidaPor,
							Recibida = q.tUsuario2.Nombre + " " + q.tUsuario2.Apellidos,
							RecibidaOtro = q.RecibidaOtro,
							Tipo = q.Tipo,
							idVenta = q.idVenta,
							Cantidad = q.Cantidad,
							Comentarios = q.Comentarios,
							Estatus = q.Estatus,
							CreadoPor = q.CreadoPor,
							Creado = q.Creado,
							ModificadoPor = q.ModificadoPor,
							Modificado = q.Modificado
						}).OrderByDescending(q => q.Creado).ToList(),
						CreadoPor = p.CreadoPor,
						Creado = p.Creado,
						ModificadoPor = p.ModificadoPor,
						Modificado = p.Modificado
					}).OrderByDescending(p => p.Creado).FirstOrDefault();
			}
		}

		//Post
		public int Add(tReporteCaja report)
		{
			using (var context = new admDB_SAADDBEntities())
			{
				report.Fecha = DateTime.Now;
				context.tReporteCajas.Add(report);
				context.SaveChanges();
				return report.idReporteCaja;
			}
		}

		//Patch
		public bool UpdateStatus(int id)
		{
			using (var context = new admDB_SAADDBEntities())
			{
				var report = context.tReporteCajas.Where(p => p.idReporteCaja == id).FirstOrDefault();
				report.Revisado = !(report.Revisado ?? false);
				context.SaveChanges();

				return report.Revisado ?? false;
			}
		}
	}
}