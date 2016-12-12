using Ferro_Velho.Entidades;
using Ferro_Velho.Repositorio.Interface;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferro_Velho.Business
{
    public class MateriaisBo
    {
        private readonly IMateriais repositorio;

        public MateriaisBo(IMateriais repo)
        {
            repositorio = repo;
        }

        public string Salvar(MateriaisVo material)
        {
            return repositorio.Salvar(material);
        }

        public void Excluir(int id)
        {
            repositorio.Excluir(id);
        }

        public void DisExcluir(int id)
        {
            repositorio.DisExcluir(id);
        }

        public IEnumerable<MateriaisVo> ListarTodos(int? pagina)
        {
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;
            return repositorio.ListarTodos().ToPagedList(numeroPagina, tamanhoPagina);
        }

        public IEnumerable<MateriaisVo> ListarTodos(string descricao, int? pagina)
        {
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;
            return repositorio.ListarTodos(descricao).ToPagedList(numeroPagina, tamanhoPagina);
        }

        public IEnumerable<MateriaisVo> SemRemovidos()
        {
            return repositorio.SemRemovidos();
        }

        public /*IEnumerable<MateriaisVo>*/ MateriaisVo ListarPorId(int id, int? pagina)
        {
            //int tamanhoPagina = 10;
            //int numeroPagina = pagina ?? 1;
            return repositorio.ListarPorId(id)/*.ToPagedList(numeroPagina, tamanhoPagina)*/;
        }
    }
}
