using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferro_Velho.Entidades
{
    public class TelefoneVo : BaseVo
    {
        public string Tipo_Telefone { get; set; }
        public string Telefone { get; set; }
        public virtual ClienteVo Cliente { get; set; }
        public TelefoneVo()
        {
            this.Cliente = new ClienteVo();
        }
    }
}
