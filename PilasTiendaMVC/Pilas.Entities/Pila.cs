using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PilasTiendaMVC.Pilas.Entities
{
    public class Pila
    {
        public int PilaId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public double Precio { get; set; }
        public string Imagen { get; set; }
        public int Stock { get; set; }
    }
}