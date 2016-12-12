﻿using Ferro_Velho.Entidades;
using Ferro_Velho.Repositorio.Interface;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferro_Velho.Business
{
    public class CaixaBo
    {
        private readonly ICaixa repositorio;

        public CaixaBo(ICaixa repo)
        {
            repositorio = repo;
        }

        public string Salvar(CaixaVo caixa)
        {
            return repositorio.Salvar(caixa);
        }

        public void Excluir(int id)
        {
            repositorio.Excluir(id);
        }

        public IEnumerable<CaixaVo> ListarTodos(int? pagina)
        {
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;
            return repositorio.ListarTodos().ToPagedList(numeroPagina, tamanhoPagina);
        }

        public IEnumerable<CaixaVo> ListarTodos(DateTime? dataInicial, DateTime? dataFinal, string nomeCliente, int? pagina)
        {
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;
            return repositorio.ListarTodos(dataInicial, dataFinal, nomeCliente).ToPagedList(numeroPagina, tamanhoPagina);
        }

        public IEnumerable<CaixaVo> SemRemovidos()
        {
            return repositorio.SemRemovidos();
        }

        public void Baixar(int id)
        {
            repositorio.Baixar(id);
        }

        public IEnumerable<CaixaVo> SemBaixados()
        {
            return repositorio.SemBaixados();
        }

        public IEnumerable<CaixaVo> ListarPorId(int idCliente, int? pagina)
        {
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;
            return repositorio.ListarPorId(idCliente).ToPagedList(numeroPagina, tamanhoPagina);
        }
    }
}
