using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferro_Velho.Entidades
{
    public class ClienteVo : BaseVo
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime Data_Cadastro { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Tipo_Cliente { get; set; }
        public bool Ativo { get; set; }
        //public virtual TelefoneVo Telefone { get; set; }
        public ClienteVo()
        {
            //this.Telefone = new TelefoneVo();
        }
    }
}
