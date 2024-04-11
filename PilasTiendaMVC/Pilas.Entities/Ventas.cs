using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PilasTiendaMVC.Pilas.Entities
{
    public class Ventas
    {
        public int idVenta { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public double Precio { get; set; }
        public double Total { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioId { get; set; }
        public int idProducto { get; set; }  
    }
}