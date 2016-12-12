using Ferro_Velho.Repositorio.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Ferro_Velho.Entidades;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

namespace Ferro_Velho.Repositorio
{
    public class MaterialRepositorio : IMateriais
    {
        private Contexto contexto;

        public void DisExcluir(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE Material SET Ativo = 1 Where ID_Material = @ID ");
            using (contexto = new Contexto())
            {
                List<SqlParameter> param = new List<SqlParameter>()
                {
                    new SqlParameter { ParameterName = "@ID", SqlDbType = SqlDbType.Int, Value = id }
                };
                contexto.ExecutaComando(sql.ToString(), param);
            }
        }

        public void Excluir(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE Material SET Ativo = 0 Where ID_Material = @ID ");
            using (contexto = new Contexto())
            {
                List<SqlParameter> param = new List<SqlParameter>()
                {
                    new SqlParameter { ParameterName = "@ID", SqlDbType = SqlDbType.Int, Value = id }
                };
                contexto.ExecutaComando(sql.ToString(), param);
            }
        }

        public MateriaisVo ListarPorId(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = "Select * From Material ";
                strQuery += " WHERE ID_Material = @ID order by Descricao asc ";

                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter() { ParameterName = "@ID", SqlDbType = SqlDbType.Int, Value = id });

                var retorno = contexto.ExecutaComRetorno(strQuery, param);
                return ReaderObjeto(retorno).FirstOrDefault();
            }
        }

        //IEnumerable<MateriaisVo> IRepositorio<MateriaisVo>.ListarPorId(int id)
        //{
        //    var strQuery = "Select * From Material ";
        //    strQuery += " WHERE ID_Material = @ID order by Descricao asc ";

        //    List<SqlParameter> param = new List<SqlParameter>();
        //    param.Add(new SqlParameter() { ParameterName = "@ID", SqlDbType = SqlDbType.Int, Value = id });

        //    var retorno = contexto.ExecutaComRetorno(strQuery, param);
        //    return ReaderObjeto(retorno);
        //}

        public IEnumerable<MateriaisVo> ListarTodos()
        {
            using (contexto = new Contexto())
            {
                var strQuery = "Select * From Material ";
                strQuery += " where 1 = 1 order by Descricao asc ";
                var retorno = contexto.ExecutaComRetorno(strQuery, new List<SqlParameter>());
                return ReaderObjeto(retorno);
            }
        }

        public IEnumerable<MateriaisVo> ListarTodos(string descricao)
        {
            using (contexto = new Contexto())
            {
                List<SqlParameter> param = new List<SqlParameter>();
                var strQuery = "Select * from Material ";
                if (!string.IsNullOrEmpty(descricao))
                {
                    strQuery += " And Descricao like '%' + @Descricao + '%' ";
                    param.Add(new SqlParameter() { ParameterName = "@Descricao", SqlDbType = SqlDbType.VarChar, Value = descricao });
                }
                strQuery += " where 1 = 1 order by Descricao asc ";
                var retorno = contexto.ExecutaComRetorno(strQuery, param);
                return ReaderObjeto(retorno);
            }
        }

        public string Salvar(MateriaisVo entidade)
        {
            StringBuilder sql = new StringBuilder();

            var mensagem = "";

            if (entidade.ID_Material == 0)
            {
                if (!string.IsNullOrEmpty(entidade.Descricao)
                   || !string.IsNullOrEmpty(entidade.Qtde_Peso)
                   || !string.IsNullOrEmpty(entidade.Valor))
                {
                    sql.Append("INSERT INTO Material (Descricao, Qtde_Peso, Valor, Ativo) ");
                    sql.Append(" VALUES (@Descricao, @Qtde_Peso, @Valor, @Ativo)");
                    mensagem = "Cadastro inserido com Sucesso!!!";
                }
                else
                {
                    mensagem = "Existe campos em Branco, preencha-os por favor!";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(entidade.Descricao)
                   || !string.IsNullOrEmpty(entidade.Qtde_Peso)
                   || !string.IsNullOrEmpty(entidade.Valor))
                {
                    sql.Append("UPDATE Material SET Descricao = @Descricao, Qtde_Peso = @Qtde_Peso, Valor = @Valor, Ativo = @Ativo ");
                    sql.Append(" Where ID_Material = @ID");
                    mensagem = "Cadastro atualizado com Sucesso!!!";
                }
                else
                {
                    mensagem = "Motivo da Devolucao Invalido, por gentileza, preencha o campo!!!";
                }
            }

            if (!string.IsNullOrEmpty(sql.ToString()))
            {
                using (contexto = new Contexto())
                {
                    List<SqlParameter> param = new List<SqlParameter>()
                {
                    new SqlParameter { ParameterName = "@Descricao", SqlDbType = SqlDbType.VarChar, Value = entidade.Descricao },
                    new SqlParameter { ParameterName = "@Qtde_Peso", SqlDbType = SqlDbType.VarChar, Value = entidade.Qtde_Peso },
                    new SqlParameter { ParameterName = "@Ativo", SqlDbType = SqlDbType.Bit, Value = entidade.Ativo },
                    new SqlParameter { ParameterName = "@Valor", SqlDbType = SqlDbType.VarChar, Value = entidade.Valor }
                };

                    if (entidade.ID_Material > 0)
                    {
                        param.Add(new SqlParameter { ParameterName = "@ID", SqlDbType = SqlDbType.Int, Value = entidade.ID_Material });
                    }
                    contexto.ExecutaComando(sql.ToString(), param);
                }
            }
            return mensagem.ToString();
        }

        public IEnumerable<MateriaisVo> SemRemovidos()
        {
            using (contexto = new Contexto())
            {
                var strQuery = "SELECT * FROM Material Where Ativo = 1 order by Descricao ";
                var retorno = contexto.ExecutaComRetorno(strQuery, new List<SqlParameter>());
                return ReaderObjeto(retorno);
            }
        }

        private List<MateriaisVo> ReaderObjeto(SqlDataReader reader)
        {
            var material = new List<MateriaisVo>();
            while (reader.Read())
            {
                var temObjeto = new MateriaisVo()
                {
                    ID_Material = reader["ID_Material"] != DBNull.Value ? Convert.ToInt32(reader["ID_Material"]) : 0,
                    Descricao = reader["Descricao"] != DBNull.Value ? reader["Descricao"].ToString() : null,
                    Qtde_Peso = reader["Qtde_Peso"] != DBNull.Value ? reader["Qtde_Peso"].ToString() : null,
                    Ativo = reader["Ativo"] != DBNull.Value ? Convert.ToBoolean(reader["Ativo"]) : false,
                    Valor = reader["Valor"] != DBNull.Value ? reader["Valor"].ToString() : null
                };
                material.Add(temObjeto);
            }
            reader.Close();
            return material;
        }
    }
}
