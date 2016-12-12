using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ferro_Velho.Web.Models
{
    public class TelefoneModel
    {
        public string Tipo_Telefone { get; set; }
        public string Telefone { get; set; }
        public int ID_Cliente { get; set; }
        public int ID_Telefone { get; set; }
    }
}