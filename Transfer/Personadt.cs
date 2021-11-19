using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Transfer
{
    public class Personadt
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Direccion { get; set; }
        public string Ubicacion { get; set; }
        public string Telefono { get; set; }
        public string CodigoPostal { get; set; }
        public DateTime? FechaNacimiento { get; set; }
    }
}
