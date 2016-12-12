using Ferro_Velho.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferro_Velho.Repositorio.Interface
{
    public interface ICliente : IRepositorio<ClienteVo>
    {
        IEnumerable<ClienteVo> ListarTodos(string nomeCliente);
        void DisExcluir(int id);
    }
}
