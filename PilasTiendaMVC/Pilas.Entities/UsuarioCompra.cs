using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PilasTiendaMVC.Pilas.Entities
{
    public class UsuarioCompra
    {
        public string nomUser { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
    }
}