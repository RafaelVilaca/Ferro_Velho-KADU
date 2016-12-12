using Ferro_Velho.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferro_Velho.Repositorio.Interface
{
    public interface ITelefone : IRepositorio<TelefoneVo>
    {
        IEnumerable<TelefoneVo> ListarTodos(string nome);
        IEnumerable<TelefoneVo> ListarPorId(int id);
    }
}
