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
    public class TelefoneBo
    {
        private readonly ITelefone repositorio;

        public TelefoneBo(ITelefone repo)
        {
            repositorio = repo;
        }

        public string Salvar(TelefoneVo telefone)
        {
            return repositorio.Salvar(telefone);
        }

        public void Excluir(int id)
        {
            repositorio.Excluir(id);
        }

        public IEnumerable<TelefoneVo> ListarTodos(int? pagina)
        {
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;
            return repositorio.ListarTodos().ToPagedList(numeroPagina, tamanhoPagina);
        }

        public IEnumerable<TelefoneVo> ListarTodos(string nome, int? pagina)
        {
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;
            return repositorio.ListarTodos(nome).ToPagedList(numeroPagina, tamanhoPagina);
        }

        public IEnumerable<TelefoneVo> SemRemovidos()
        {
            return repositorio.SemRemovidos();
        }

        //public TelefoneVo ListarPorId(int id, int? pagina)
        //{
        //    //int tamanhoPagina = 10;
        //    //int numeroPagina = pagina ?? 1;
        //    return repositorio.ListarPorId(id);
        //}

        public IEnumerable<TelefoneVo> ListarPorId(int id, int? pagina)
        {
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;
            return repositorio.ListarPorId(id).ToPagedList(numeroPagina, tamanhoPagina);
        }
    }
}
