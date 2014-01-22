using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebScraping.Repositorio
{
    public class EnlacesDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Link { get; set; }
        public int NumeroPuntuaciones { get; set; }
        public decimal Puntuacion { get; set; }
        public int IdCategoria { get; set; }
    }
}
