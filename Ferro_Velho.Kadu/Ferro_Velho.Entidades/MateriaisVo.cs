using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferro_Velho.Entidades
{
    public class MateriaisVo : BaseVo
    {
        public string Descricao { get; set; }
        public string Qtde_Peso { get; set; }
        public string Valor { get; set; }
        public bool Ativo { get; set; }
    }
}
