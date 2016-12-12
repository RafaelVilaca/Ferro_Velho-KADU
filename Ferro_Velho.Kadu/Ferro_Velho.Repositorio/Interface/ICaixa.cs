using Ferro_Velho.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferro_Velho.Repositorio.Interface
{
    public interface ICaixa : IRepositorio<CaixaVo>
    {
        IEnumerable<CaixaVo> ListarTodos(DateTime? dataInicial, DateTime? dataFinal, string nomeCliente);
        IEnumerable<CaixaVo> ListarPorId(int idCliente);
        IEnumerable<CaixaVo> SemBaixados();
        void Baixar(int id);
    }
}
