using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferro_Velho.Repositorio.Interface
{
    public interface IRepositorio<T> where T : class
    {
        string Salvar(T entidade);
        void Excluir(int id);
        IEnumerable<T> ListarTodos();
        //IEnumerable<T> ListarPorId(int id);
        T ListarPorId(int id);
        IEnumerable<T> SemRemovidos();
    }
}
