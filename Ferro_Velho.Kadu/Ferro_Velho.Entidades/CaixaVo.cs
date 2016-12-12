using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferro_Velho.Entidades
{
    public class CaixaVo : BaseVo
    {
        public virtual MateriaisVo Material { get; set; }
        public string Transacao { get; set; }
        public string Pesagem { get; set; }
        public string Valor_Total { get; set; }
        public DateTime Data_Transacao { get; set; }
        public virtual ClienteVo Cliente { get; set; }
        public string Motivo_Exclusao { get; set; }
        public bool Baixado { get; set; }
        public CaixaVo()
        {
            this.Cliente = new ClienteVo();
            this.Material = new MateriaisVo();
        }
    }
}
