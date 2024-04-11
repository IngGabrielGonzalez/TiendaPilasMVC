using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PilasTiendaMVC.Pilas.Entities
{
    public class Usuarios
    {
        public int idUsuario { get; set; }
        public string nomUser { get; set; }
        public string rol { get; set; }
        public string email { get; set; }
        public string passwd { get; set; }
    }
}