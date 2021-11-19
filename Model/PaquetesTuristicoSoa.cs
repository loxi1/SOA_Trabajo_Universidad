using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Transfer;

namespace WebApi.Model
{
    public partial class PaquetesTuristico
    {
        public static DisponiblePaquetedt DisponibilidadPaqueteTuristico(int idpaquete, DateTime fechareserva, int cantidadpersonas)
        {
            SOIContext db = new SOIContext();
            string unidad = "";
            decimal? valor = 0;
            DateTime? fechafin;
            DateTime? fechainicioReserva;
            int? idrese = 0;
            string textoHoraInicio = "";
            string fechacancelacion = "";

            DateTime thisDay = DateTime.UtcNow.Date;

            if (idpaquete > 0 && cantidadpersonas > 0)
            {
                var lista2 = db.PaquetesTuristicos.Where(t => t.Id == idpaquete && t.MaxNumeroPersonas >= cantidadpersonas && fechareserva > thisDay && fechareserva >= t.FechaInicio && fechareserva <= t.FechaFin)
                            .OrderBy(t => t.PaqueteTuristico).ToList();

                foreach (var lis in lista2)
                {
                    unidad = lis.UnidadDuracion.ToUpper();
                    valor = lis.TiempoDuracion;
                    fechainicioReserva = lis.FechaInicio;
                    DateTime temp = Convert.ToDateTime(fechainicioReserva);
                    textoHoraInicio = temp.ToLongTimeString();
                }

                if (unidad.Length > 0 && valor > 0)
                {
                    DateTime fechainicior = Convert.ToDateTime(fechareserva.ToString("yyyy-MM-dd") + " " + textoHoraInicio);
                    DateTime startTime = new DateTime(fechainicior.Year, fechainicior.Month, fechainicior.Day, fechainicior.Hour, 0, 0);

                    DateTime answer = (unidad == "DIAS") ? startTime.AddDays(Convert.ToInt32(valor)) : startTime.AddHours((double)valor);

                    fechafin = answer;
                    
                    DateTime fechafinr = Convert.ToDateTime(fechafin);

                    var lista3 = db.Reservas
                        .Where(t => t.PaquetesTuristicoId == idpaquete && t.FechaInicio >= thisDay && t.FechaInicio >= fechainicior && t.FechaInicio <= fechafinr)
                        .ToList();

                    foreach (var li in lista3)
                    {
                        idrese = li.Id;
                    }

                    fechafin = (unidad == "DIAS" && valor > 1) ? fechafinr.AddDays(-1) : fechafin;

                    DateTime temp4 = Convert.ToDateTime(fechafin);
                    CultureInfo culture = new CultureInfo("es-ES");
                    fechacancelacion = temp4.ToString("U", culture);
                }
            }

            var obj = db.PaquetesTuristicos.Where(t => t.Id == idpaquete)
                      .Select(b => new DisponiblePaquetedt()
                      {
                          Id = b.Id,
                          PaqueteTuristico = b.PaqueteTuristico,
                          PrecioUnitario = b.PrecioUnitario,
                          Moneda = b.Moneda,
                          Simbolo = b.Simbolo,
                          Cantidad = cantidadpersonas,
                          MontoTotal = b.PrecioUnitario * cantidadpersonas,
                          FechaCancelacion = fechacancelacion,
                          HoraInicio = textoHoraInicio
                      }).SingleOrDefault();

            if (obj == null || idrese>0) obj = new DisponiblePaquetedt() { Id = 0 };

            return obj;
        }

        public static IEnumerable<PaquetesTuristicodt> ListarPaquetesSugeridos(int idpais, int idcat)
        {
            SOIContext db = new SOIContext();
            var lista = from b in db.PaquetesTuristicos
                        .Where(t => t.IdPais == idpais && t.IdCategoria == idcat).OrderBy(t => t.PaqueteTuristico)
                        select new PaquetesTuristicodt()
                        {
                            Id = b.Id,
                            PaqueteTuristico = b.PaqueteTuristico,
                            Descripcion = b.Descripcion,
                            Moneda = b.Moneda,
                            Simbolo = b.Simbolo,
                            PrecioUnitario = b.PrecioUnitario,
                            TiempoDuracion = b.TiempoDuracion,
                            UnidadDuracion = b.UnidadDuracion
                        };
            return lista;
        }

        public static PaquetesTuristicodt VerPaqueteTuristico(int id)
        {
            SOIContext db = new SOIContext();

            var obj = db.PaquetesTuristicos
                .Select(b => new PaquetesTuristicodt()
                {
                    Id = b.Id,
                    PaqueteTuristico = b.PaqueteTuristico,
                    Descripcion = b.Descripcion,
                    Moneda = b.Moneda,
                    Simbolo = b.Simbolo,
                    PrecioUnitario = b.PrecioUnitario,
                    TiempoDuracion = b.TiempoDuracion,
                    UnidadDuracion = b.UnidadDuracion
                }).SingleOrDefault(b => b.Id == id);
            if (obj == null) obj = new PaquetesTuristicodt() { Id = 0, PaqueteTuristico = "" };
            return obj;
        }
    }
}
