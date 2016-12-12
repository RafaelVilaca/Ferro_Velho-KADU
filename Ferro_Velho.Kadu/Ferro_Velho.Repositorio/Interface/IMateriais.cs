using Ferro_Velho.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferro_Velho.Repositorio.Interface
{
    public interface IMateriais : IRepositorio<MateriaisVo>
    {
        IEnumerable<MateriaisVo> ListarTodos(string descricao);
        void DisExcluir(int id);
    }
}
