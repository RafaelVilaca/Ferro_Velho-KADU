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
    public class ClientesBo
    {
        private readonly ICliente repositorio;

        public ClientesBo(ICliente repo)
        {
            repositorio = repo;
        }

        public string Salvar(ClienteVo clientes)
        {
            return repositorio.Salvar(clientes);
        }

        public void Excluir(int id)
        {
            repositorio.Excluir(id);
        }

        public void DisExcluir(int id)
        {
            repositorio.DisExcluir(id);
        }

        public IEnumerable<ClienteVo> ListarTodos(int? pagina)
        {
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;
            return repositorio.ListarTodos().ToPagedList(numeroPagina, tamanhoPagina);
        }

        public IEnumerable<ClienteVo> ListarTodos(string nomeCliente, int? pagina)
        {
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;
            return repositorio.ListarTodos(nomeCliente).ToPagedList(numeroPagina, tamanhoPagina);
        }

        public IEnumerable<ClienteVo> SemRemovidos()
        {
            return repositorio.SemRemovidos();
        }

        public ClienteVo ListarPorId(int id, int? pagina)
        {
            //int tamanhoPagina = 10;
            //int numeroPagina = pagina ?? 1;
            return repositorio.ListarPorId(id)/*.ToPagedList(numeroPagina, tamanhoPagina)*/;
        }
    }
}
