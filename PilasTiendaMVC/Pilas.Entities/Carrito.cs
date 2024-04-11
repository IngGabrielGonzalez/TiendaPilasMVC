using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PilasTiendaMVC.Pilas.Entities
{
    public class Carrito
    {
        public int idCarrito { get; set; }
        public int cantidad { get; set; }
        public double precio { get; set; }
        public double costo { get; set; }

    }
}