using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Transfer
{
    public class PaqueteAnidado
    {
        public IEnumerable<Galeriadt> Galeria { get; set; }
        public IEnumerable<InformacionesGeneraledt> Info { get; set; }
        public IEnumerable<PaquetesTuristicodt> Paquete { get; set; }
    }
}
