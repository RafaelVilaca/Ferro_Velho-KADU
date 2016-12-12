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
    public class TelefoneRepositorio : ITelefone
    {
        private Contexto contexto;

        public void Excluir(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("DELETE FROM Telefones Where ID_Telefone = @ID ");
            using (contexto = new Contexto())
            {
                List<SqlParameter> param = new List<SqlParameter>()
                {
                    new SqlParameter { ParameterName = "@ID", SqlDbType = SqlDbType.Int, Value = id }
                };
                contexto.ExecutaComando(sql.ToString(), param);
            }
        }

        public TelefoneVo ListarPorId(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = "Select tel.*, cl.Nome From Telefones tel ";
                strQuery += " INNER JOIN Clientes cl ON cl.ID_Cliente = tel.ID_Cliente ";
                strQuery += " WHERE ID_Telefone = @ID order by Tipo_Telefone asc ";

                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter() { ParameterName = "@ID", SqlDbType = SqlDbType.Int, Value = id });

                var retorno = contexto.ExecutaComRetorno(strQuery, param);
                return ReaderObjeto(retorno).FirstOrDefault();
            }
        }

        IEnumerable<TelefoneVo> ITelefone.ListarPorId(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = "Select tel.*, cl.Nome From Telefones tel ";
                strQuery += " INNER JOIN Clientes cl ON cl.ID_Cliente = tel.ID_Cliente ";
                strQuery += " WHERE cl.ID_Cliente = @ID order by Tipo_Telefone asc ";

                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter() { ParameterName = "@ID", SqlDbType = SqlDbType.Int, Value = id });

                var retorno = contexto.ExecutaComRetorno(strQuery, param);
                return ReaderObjeto(retorno);
            }
        }

        public IEnumerable<TelefoneVo> ListarTodos()
        {
            var strQuery = "Select tel.*, cl.Nome From Telefones tel ";
            strQuery += " INNER JOIN Clientes cl ON cl.ID_Cliente = tel.ID_Cliente ";
            strQuery += " order by Tipo_Telefone asc ";
            var retorno = contexto.ExecutaComRetorno(strQuery, new List<SqlParameter>());
            return ReaderObjeto(retorno);
        }

        public IEnumerable<TelefoneVo> ListarTodos(string nome)
        {
            using (contexto = new Contexto())
            {
                List<SqlParameter> param = new List<SqlParameter>();
                var strQuery = "Select tel.*, cl.Nome From Telefones tel ";
                strQuery += " INNER JOIN Clientes cl ON cl.ID_Cliente = tel.ID_Cliente ";
                if (!string.IsNullOrEmpty(nome))
                {
                    strQuery += " And cl.Nome like '%' + @nome + '%' ";
                    param.Add(new SqlParameter() { ParameterName = "@nome", SqlDbType = SqlDbType.VarChar, Value = nome });
                }
                strQuery += " where 1 = 1 ";
                strQuery += " order by Tipo_Telefone asc ";
                var retorno = contexto.ExecutaComRetorno(strQuery, param);
                return ReaderObjeto(retorno);
            }
        }

        public string Salvar(TelefoneVo entidade)
        {
            StringBuilder sql = new StringBuilder();

            StringBuilder verifica = new StringBuilder();
            verifica.Append("Select Telefone From Telefones Where Telefone = @Tel");
            new SqlParameter { ParameterName = "@Tel", SqlDbType = SqlDbType.VarChar, Value = entidade.Telefone };

            contexto = new Contexto();
            var command = string.Format("Select Telefone From Telefones Where Telefone = '{0}'", entidade.Telefone);
            var retorno = contexto.ExecutaComRetorno(command, new List<SqlParameter>());
            string msg = null;
            while (retorno.Read())
            {
                msg = retorno["Telefone"] != DBNull.Value ? retorno["Telefone"].ToString() : null;
            }
            retorno.Close();

            var mensagem = "";
            if (entidade.ID_Telefone == 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(entidade.ID_Cliente))
                   || !string.IsNullOrEmpty(entidade.Tipo_Telefone)
                   || !string.IsNullOrEmpty(entidade.Telefone))
                {
                    if (string.IsNullOrEmpty(msg))
                    {
                        sql.Append("INSERT INTO Telefones (Tipo_Telefone, Telefone, ID_Cliente) ");
                        sql.Append(" VALUES (@Tipo_Telefone, @Telefone, @ID_Cliente)");
                        mensagem = "Cadastro inserido com Sucesso!!!";
                    }
                    else
                    {
                        mensagem = "Telefone Existente, corrija o Telefone ou Atualize!";
                    }
                }
                else
                {
                    mensagem = "Existe campos em Branco, preencha-os por favor!";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Convert.ToString(entidade.ID_Cliente))
                   || !string.IsNullOrEmpty(entidade.Tipo_Telefone)
                   || !string.IsNullOrEmpty(entidade.Telefone))
                {
                    sql.Append("UPDATE Telefones SET Tipo_Telefone = @Tipo_Telefone, Telefone = @Telefone, ID_Cliente = @ID_Cliente ");
                    sql.Append(" Where ID_Telefone = @ID");
                    mensagem = "Cadastro atualizado com Sucesso!!!";
                }

                else
                {
                    mensagem = "Existe campos em Branco, preencha-os por favor!";
                }
            }

            if (!string.IsNullOrEmpty(sql.ToString()))
            {
                using (contexto = new Contexto())
                {
                    List<SqlParameter> param = new List<SqlParameter>()
                {
                    new SqlParameter { ParameterName = "@Tipo_Telefone", SqlDbType = SqlDbType.VarChar, Value = entidade.Tipo_Telefone },
                    new SqlParameter { ParameterName = "@Telefone", SqlDbType = SqlDbType.VarChar, Value = entidade.Telefone },
                    new SqlParameter { ParameterName = "@ID_Cliente", SqlDbType = SqlDbType.Int, Value = entidade.ID_Cliente }
                };

                    if (entidade.ID_Telefone > 0)
                    {
                        param.Add(new SqlParameter { ParameterName = "@ID", SqlDbType = SqlDbType.Int, Value = entidade.ID_Telefone });
                    }
                    contexto.ExecutaComando(sql.ToString(), param);
                }
            }
            return mensagem.ToString();
        }

        public IEnumerable<TelefoneVo> SemRemovidos()
        {
            throw new NotImplementedException();
        }

        private List<TelefoneVo> ReaderObjeto(SqlDataReader reader)
        {
            var telefone = new List<TelefoneVo>();
            while (reader.Read())
            {
                var cliente = new ClienteVo()
                {
                    ID_Cliente = int.Parse(reader["ID_Cliente"].ToString()),
                    Nome = reader["Nome"].ToString()
                };

                var temObjeto = new TelefoneVo()
                {
                    ID_Telefone = int.Parse(reader["ID_Telefone"].ToString()),
                    Tipo_Telefone = reader["Tipo_Telefone"].ToString(),
                    Telefone = reader["Telefone"].ToString(),
                    Cliente = cliente
                };
                telefone.Add(temObjeto);
            }
            reader.Close();
            return telefone;
        }
    }
}
