using Ferro_Velho.Repositorio.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferro_Velho.Entidades;
using System.Data.SqlClient;
using System.Data;

namespace Ferro_Velho.Repositorio
{
    public class ClienteRepositorio : ICliente
    {
        private Contexto contexto;

        public void DisExcluir(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE Clientes SET Ativo = 1 Where ID_Cliente = @IdCliente ");
            using (contexto = new Contexto())
            {
                List<SqlParameter> param = new List<SqlParameter>()
                {
                    new SqlParameter { ParameterName = "@IdCliente", SqlDbType = SqlDbType.Int, Value = id }
                };
                contexto.ExecutaComando(sql.ToString(), param);
            }
        }

        public void Excluir(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE Clientes SET Removido = 0 Where ID_Cliente = @IdCliente ");
            using (contexto = new Contexto())
            {
                List<SqlParameter> param = new List<SqlParameter>()
                {
                    new SqlParameter { ParameterName = "@IdCliente", SqlDbType = SqlDbType.Int, Value = id }
                };
                contexto.ExecutaComando(sql.ToString(), param);
            }
        }

        public ClienteVo ListarPorId(int id)
        {
            using (contexto = new Contexto())
            {
                //var strQuery = "SELECT cl.*, tel.Tipo_Telefone, tel.Telefone FROM Clientes cl ";
                //strQuery += " INNER JOIN Telefones tel ON cl.ID_Cliente = tel.ID_Cliente ";
                var strQuery = "SELECT * FROM Clientes ";
                strQuery += " Where ID_Cliente = @IdCliente ";
                strQuery += " order by Nome asc";

                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter() { ParameterName = "@IdCliente", SqlDbType = SqlDbType.Int, Value = id });

                var retorno = contexto.ExecutaComRetorno(strQuery, param);
                return ReaderObjeto(retorno).FirstOrDefault();
            }
        }

        //IEnumerable<ClienteVo> IRepositorio<ClienteVo>.ListarPorId(int id)
        //{
        //    using (contexto = new Contexto())
        //    {
        //        //var strQuery = "SELECT cl.*, tel.Tipo_Telefone, tel.Telefone FROM Clientes cl ";
        //        //strQuery += " INNER JOIN Telefones tel ON cl.ID_Cliente = tel.ID_Cliente ";
        //        //strQuery += " Where cl.ID_Cliente = @IdCliente ";
        //        var strQuery = "SELECT * FROM Clientes ";
        //        strQuery += " Where ID_Cliente = @IdCliente ";
        //        strQuery += " order by Nome asc";

        //        List<SqlParameter> param = new List<SqlParameter>();
        //        param.Add(new SqlParameter() { ParameterName = "@IdCliente", SqlDbType = SqlDbType.Int, Value = id });

        //        var retorno = contexto.ExecutaComRetorno(strQuery, param);
        //        return ReaderObjeto(retorno);
        //    }
        //}

        public IEnumerable<ClienteVo> ListarTodos()
        {
            using (contexto = new Contexto())
            {
                //var strQuery = "SELECT cl.*, tel.Tipo_Telefone, tel.Telefone FROM Clientes cl ";
                //strQuery += " LEFT JOIN Telefones tel ON cl.ID_Cliente = tel.ID_Cliente ";
                var strQuery = "SELECT * FROM Clientes ";
                strQuery += " order by Nome asc";

                //var strQuery = "SELECT * FROM Clientes ";

                var retorno = contexto.ExecutaComRetorno(strQuery, new List<SqlParameter>());
                return ReaderObjeto(retorno);
            }
        }

        public IEnumerable<ClienteVo> ListarTodos(string nomeCliente)
        {
            using (contexto = new Contexto())
            {
                List<SqlParameter> param = new List<SqlParameter>();
                //var strQuery = "SELECT cl.*, tel.Tipo_Telefone, tel.Telefone FROM Clientes cl ";
                //strQuery += " LEFT JOIN Telefones tel ON cl.ID_Cliente = tel.ID_Cliente ";
                var strQuery = "SELECT * FROM Clientes ";
                if (!string.IsNullOrEmpty(nomeCliente))
                {
                    strQuery += " And Nome like '%' + @NomeCliente + '%' ";
                    param.Add(new SqlParameter() { ParameterName = "@NomeCliente", SqlDbType = SqlDbType.VarChar, Value = nomeCliente });
                }
                strQuery += " where 1 = 1 order by Nome asc ";
                var retorno = contexto.ExecutaComRetorno(strQuery, param);
                return ReaderObjeto(retorno);
            }
        }

        public string Salvar(ClienteVo entidade)
        {
            StringBuilder sql = new StringBuilder();

            contexto = new Contexto();

            var mensagem = "";
                if (entidade.ID_Cliente == 0)
                {
                    if (!string.IsNullOrEmpty(entidade.Nome) ||
                        !string.IsNullOrEmpty(entidade.Numero) ||
                        !string.IsNullOrEmpty(entidade.Tipo_Cliente) ||
                        !string.IsNullOrEmpty(entidade.Estado) ||
                        !string.IsNullOrEmpty(entidade.Endereco) ||
                        !string.IsNullOrEmpty(entidade.Email) ||
                        !string.IsNullOrEmpty(entidade.Complemento) ||
                        !string.IsNullOrEmpty(entidade.Cidade) ||
                        !string.IsNullOrEmpty(entidade.Bairro))
                    {
                        sql.Append("INSERT INTO Clientes (Nome, Email, Data_Cadastro, Endereco, Numero, Complemento, Bairro, Cidade, Estado, Tipo_Cliente, Ativo) ");
                        sql.Append(" VALUES (@Nome, @Email, @Data_Cadastro, @Endereco, @Numero, @Complemento, @Bairro, @Cidade, @Estado, @Tipo_Cliente, @Ativo) ");
                        mensagem = "Cadastro inserido com Sucesso!!!";
                    }
                    else
                    {
                        mensagem = "Existe campos em Branco, preencha-os por favor!";
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(entidade.Nome) ||
                        !string.IsNullOrEmpty(entidade.Numero) ||
                        !string.IsNullOrEmpty(entidade.Tipo_Cliente) ||
                        !string.IsNullOrEmpty(entidade.Estado) ||
                        !string.IsNullOrEmpty(entidade.Endereco) ||
                        !string.IsNullOrEmpty(entidade.Email) ||
                        !string.IsNullOrEmpty(entidade.Complemento) ||
                        !string.IsNullOrEmpty(entidade.Cidade) ||
                        !string.IsNullOrEmpty(entidade.Bairro))
                    {
                        sql.Append("UPDATE Clientes SET Nome = @Nome, Email = @Email, Data_Cadastro = @Data_Cadastro, Endereco = @Endereco,  ");
                        sql.Append(" Numero = @Numero, Complemento = @Complemento, Bairro = @Bairro, Cidade = @Cidade, Estado = @Estado, Tipo_Cliente = @Tipo_Cliente, Ativo = @Ativo");
                        sql.Append(" Where ID_Cliente = @IdCliente");
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
                    new SqlParameter { ParameterName = "@Nome", SqlDbType = SqlDbType.VarChar, Value = entidade.Nome },
                    new SqlParameter { ParameterName = "@Email", SqlDbType = SqlDbType.VarChar, Value = entidade.Email },
                    new SqlParameter { ParameterName = "@Data_Cadastro", SqlDbType = SqlDbType.DateTime, Value = entidade.Data_Cadastro },
                    new SqlParameter { ParameterName = "@Endereco", SqlDbType = SqlDbType.VarChar, Value = entidade.Endereco },
                    new SqlParameter { ParameterName = "@Ativo", SqlDbType = SqlDbType.Bit, Value = entidade.Ativo },
                    new SqlParameter { ParameterName = "@Numero", SqlDbType = SqlDbType.VarChar, Value = entidade.Numero },
                    new SqlParameter { ParameterName = "@Complemento", SqlDbType = SqlDbType.VarChar, Value = entidade.Complemento },
                    new SqlParameter { ParameterName = "@Bairro", SqlDbType = SqlDbType.VarChar, Value = entidade.Bairro },
                    new SqlParameter { ParameterName = "@Cidade", SqlDbType = SqlDbType.VarChar, Value = entidade.Cidade },
                    new SqlParameter { ParameterName = "@Estado", SqlDbType = SqlDbType.VarChar, Value = entidade.Estado },
                    new SqlParameter { ParameterName = "@Tipo_Cliente", SqlDbType = SqlDbType.VarChar, Value = entidade.Tipo_Cliente }
                };

                    if (entidade.ID_Cliente > 0)
                    {
                        param.Add(new SqlParameter { ParameterName = "@IdCliente", SqlDbType = SqlDbType.Int, Value = entidade.ID_Cliente });
                    }
                    contexto.ExecutaComando(sql.ToString(), param);
                }
            }
            return mensagem.ToString();
        }

        public IEnumerable<ClienteVo> SemRemovidos()
        {
            using (contexto = new Contexto())
            {
                var strQuery = "SELECT * FROM Clientes Where Ativo = 1 order by Nome ";
                var retorno = contexto.ExecutaComRetorno(strQuery, new List<SqlParameter>());
                return ReaderObjeto(retorno);
            }
        }

        private List<ClienteVo> ReaderObjeto(SqlDataReader reader)
        {
            var cliente = new List<ClienteVo>();
            while (reader.Read())
            {
                //var telefone = new TelefoneVo()
                //{
                //    ID_Telefone = int.Parse(reader["ID_Telefone"].ToString()),
                //    Tipo_Telefone = reader["Tipo_Telefone"].ToString(),
                //    Telefone = reader["Telefone"].ToString()
                //};

                var temObjeto = new ClienteVo()
                {
                    ID_Cliente = reader["ID_Cliente"] != DBNull.Value ? Convert.ToInt32(reader["ID_Cliente"]) : 0,
                    //ID_Telefone = reader["ID_Telefone"] != DBNull.Value ? Convert.ToInt32(reader["ID_Telefone"]) : 0,
                    Nome = reader["Nome"] != DBNull.Value ? reader["Nome"].ToString() : null,
                    Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
                    Ativo = reader["Ativo"] != DBNull.Value ? Convert.ToBoolean(reader["Ativo"]) : false,
                    Data_Cadastro = Convert.ToDateTime(reader["Data_Cadastro"]),
                    Endereco = reader["Endereco"] != DBNull.Value ? reader["Endereco"].ToString() : null,
                    Complemento = reader["Complemento"] != DBNull.Value ? reader["Complemento"].ToString() : null,
                    Numero = reader["Numero"] != DBNull.Value ? reader["Numero"].ToString() : null,
                    Bairro = reader["Bairro"] != DBNull.Value ? reader["Bairro"].ToString() : null,
                    Cidade = reader["Cidade"] != DBNull.Value ? reader["Cidade"].ToString() : null,
                    Tipo_Cliente = reader["Tipo_Cliente"] != DBNull.Value ? reader["Tipo_Cliente"].ToString() : null,
                    Estado = reader["Estado"] != DBNull.Value ? reader["Estado"].ToString() : null
                    //Telefone = telefone
                };
                cliente.Add(temObjeto);
            }
            reader.Close();
            return cliente;
        }
    }
}
