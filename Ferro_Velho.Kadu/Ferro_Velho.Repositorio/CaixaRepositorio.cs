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
    public class CaixaRepositorio : ICaixa
    {
        private Contexto contexto;

        public void Excluir(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE Caixa SET Baixado = 1 Where ID = @ID ");
            using (contexto = new Contexto())
            {
                List<SqlParameter> param = new List<SqlParameter>()
                {
                    new SqlParameter { ParameterName = "@ID", SqlDbType = SqlDbType.Int, Value = id }
                };
                contexto.ExecutaComando(sql.ToString(), param);
            }
        }

        public CaixaVo ListarPorId(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = "Select cx.*, cl.Nome, mat.Descricao from Caixa cx ";
                strQuery += " INNER JOIN Clientes cl ON cl.ID_Cliente = cx.ID_Cliente ";
                strQuery += " INNER JOIN Material mat ON mat.ID_Material = cx.ID_Material ";
                strQuery += " WHERE cx.ID_Caixa = @ID order by Data_Transacao desc ";

                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter() { ParameterName = "@ID", SqlDbType = SqlDbType.Int, Value = id });

                var retorno = contexto.ExecutaComRetorno(strQuery, param);
                return ReaderObjeto(retorno).FirstOrDefault();
            }
        }

        IEnumerable<CaixaVo> ICaixa.ListarPorId(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = "Select cx.*, cl.Nome, mat.Descricao from Caixa cx ";
                strQuery += " INNER JOIN Clientes cl ON cl.ID_Cliente = cx.ID_Cliente ";
                strQuery += " And cl.ID_Cliente = @ID_Cliente ";
                strQuery += " INNER JOIN Material mat ON mat.ID_Material = cx.ID_Material ";
                strQuery += " order by Data_Transacao desc ";

                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter() { ParameterName = "@ID_Cliente", SqlDbType = SqlDbType.Int, Value = id });

                var retorno = contexto.ExecutaComRetorno(strQuery, param);
                return ReaderObjeto(retorno);
            }
        }

        public IEnumerable<CaixaVo> ListarTodos()
        {
            using (contexto = new Contexto())
            {
                var strQuery = "Select cx.*, cl.Nome, mat.Descricao from Caixa cx ";
                strQuery += " INNER JOIN Clientes cl ON cl.ID_Cliente = cx.ID_Cliente ";
                strQuery += " INNER JOIN Material mat ON mat.ID_Material = cx.ID_Material ";
                strQuery += " where 1 = 1 order by Data_Transacao desc";
                var retorno = contexto.ExecutaComRetorno(strQuery, new List<SqlParameter>());
                return ReaderObjeto(retorno);
            }
        }

        public IEnumerable<CaixaVo> ListarTodos(DateTime? dataInicial, DateTime? dataFinal, string nomeCliente, string transacao)
        {
            using (contexto = new Contexto())
            {
                List<SqlParameter> param = new List<SqlParameter>();
                var strQuery = "Select cx.*, cl.Nome, mat.Descricao from Caixa cx ";
                strQuery += " INNER JOIN Clientes cl ON cl.ID_Cliente = cx.ID_Cliente ";
                if (!string.IsNullOrEmpty(transacao))
                {
                    strQuery += " And cx.Transacao like '%' + @Transacao + '%' ";
                    param.Add(new SqlParameter() { ParameterName = "@Transacao", SqlDbType = SqlDbType.VarChar, Value = transacao });
                }
                if (!string.IsNullOrEmpty(nomeCliente))
                {
                    strQuery += " And cl.Nome like '%' + @NomeCliente + '%' ";
                    param.Add(new SqlParameter() { ParameterName = "@NomeCliente", SqlDbType = SqlDbType.VarChar, Value = nomeCliente });
                }
                strQuery += " INNER JOIN Material mat ON mat.ID_Material = cx.ID_Material ";
                if (dataInicial.HasValue)
                {
                    if (dataFinal.HasValue)
                    {
                        strQuery += " AND cx.Data_Transacao Between @DataInicial and @DataFinal ";
                        param.Add(new SqlParameter() { ParameterName = "@DataInicial", SqlDbType = SqlDbType.DateTime, Value = dataInicial.Value.ToString("yyyy/MM/dd HH:mm:ss") });
                        param.Add(new SqlParameter() { ParameterName = "@DataFinal", SqlDbType = SqlDbType.DateTime, Value = dataFinal.Value.ToString("yyyy/MM/dd HH:mm:ss") });
                    }
                }
                strQuery += " where 1 = 1 order by Data_Transacao desc";

                var retorno = contexto.ExecutaComRetorno(strQuery, param);
                return ReaderObjeto(retorno);
            }
        }

        public string Salvar(CaixaVo entidade)
        {
            StringBuilder sql = new StringBuilder();

            StringBuilder verifica = new StringBuilder();
            verifica.Append("Select Qtde_Peso From Material Where Qtde_Peso = @Pesagem");
            new SqlParameter { ParameterName = "@Pesagem", SqlDbType = SqlDbType.VarChar, Value = entidade.Pesagem };

            contexto = new Contexto();
            var command = string.Format("Select Qtde_Peso From Material Where Qtde_Peso = '{0}'", entidade.Pesagem);
            var retorno = contexto.ExecutaComRetorno(command, new List<SqlParameter>());
            string msg = null;
            while (retorno.Read())
            {
                msg = retorno["Qtde_Peso"] != DBNull.Value ? retorno["Qtde_Peso"].ToString() : null;
            }
            retorno.Close();

            var mensagem = "";
            //     new { Valor = "Devolução", Texto = "Devolução" }, new { Valor = "Venda", Texto = "Venda" },
            //new { Valor = "Entrada", Texto = "Entrada" }, new { Valor = "Saida", Texto = "Saida" }

            if (entidade.ID_Caixa == 0)
            {
                if (!string.IsNullOrEmpty(entidade.Valor_Total)
                   || !string.IsNullOrEmpty(entidade.Transacao)
                   || !string.IsNullOrEmpty(Convert.ToString(entidade.ID_Material))
                   || !string.IsNullOrEmpty(Convert.ToString(entidade.ID_Cliente))
                   || !string.IsNullOrEmpty(entidade.Transacao)
                   || !string.IsNullOrEmpty(entidade.Pesagem))
                {
                    //if (!string.IsNullOrEmpty(msg) && Convert.ToInt32(msg) <= Convert.ToInt32(entidade.Pesagem))
                    //{
                        if (entidade.Transacao == "Devolução")
                        {
                            if (!string.IsNullOrEmpty(entidade.Motivo_Exclusao))
                            {
                                sql.Append("INSERT INTO Caixa (ID_Material, Transacao, Pesagem, Valor_Total, Data_Transacao, ID_Cliente, Motivo_Exclusao, Baixado) ");
                                sql.Append(" VALUES (@ID_Material, @Transacao, @Pesagem, @Valor_Total, @Data_Transacao, @ID_Cliente, @Motivo, 0); ");
                                sql.Append(" UPDATE Material Set Qtde_Peso = convert(decimal(15,2), Qtde_Peso) + convert(decimal(15,2), @Pesagem) WHERE ID_Material = @ID_Material ");
                                mensagem = "Transacao efetuada com Sucesso!!!";
                            }
                            else
                            {
                                mensagem = "Motivo da Exclusao em Branco, preencha-os por favor!";
                            }

                        }
                        else if (entidade.Transacao == "Venda")
                        {
                            sql.Append("INSERT INTO Caixa (ID_Material, Transacao, Pesagem, Valor_Total, Data_Transacao, ID_Cliente, Motivo_Exclusao, Baixado) ");
                            sql.Append(" VALUES (@ID_Material, @Transacao, @Pesagem, @Valor_Total, @Data_Transacao, @ID_Cliente, @Motivo, 0); ");
                            sql.Append(" UPDATE Material Set Qtde_Peso = convert(decimal(15,2), Qtde_Peso) - convert(decimal(15,2), @Pesagem) WHERE ID_Material = @ID_Material ");
                            mensagem = "Transacao efetuada com Sucesso!!!";
                        }
                        else if (entidade.Transacao == "Entrada")
                        {
                            sql.Append("INSERT INTO Caixa (ID_Material, Transacao, Pesagem, Valor_Total, Data_Transacao, ID_Cliente, Motivo_Exclusao, Baixado) ");
                            sql.Append(" VALUES (@ID_Material, @Transacao, @Pesagem, @Valor_Total, @Data_Transacao, @ID_Cliente, @Motivo, 0); ");
                            sql.Append(" UPDATE Material Set Qtde_Peso = convert(decimal(15,2), Qtde_Peso) + convert(decimal(15,2), @Pesagem) WHERE ID_Material = @ID_Material ");
                            mensagem = "Transacao efetuada com Sucesso!!!";
                        }
                        else if (entidade.Transacao == "Saida")
                        {
                            sql.Append("INSERT INTO Caixa (ID_Material, Transacao, Pesagem, Valor_Total, Data_Transacao, ID_Cliente, Motivo_Exclusao, Baixado) ");
                            sql.Append(" VALUES (@ID_Material, @Transacao, @Pesagem, @Valor_Total, @Data_Transacao, @ID_Cliente, @Motivo, 0); ");
                            sql.Append(" UPDATE Material Set Qtde_Peso = convert(decimal(15,2), Qtde_Peso) - convert(decimal(15,2), @Pesagem) WHERE ID_Material = @ID_Material ");
                            mensagem = "Transacao efetuada com Sucesso!!!";
                        }
                        else
                        {
                            mensagem = "Transacao invalida, preencha por favor";
                        }
                    //}
                    //else
                    //{
                    //    mensagem = "Pesagem invalida!!!";
                    //}
                }
                else
                {
                    mensagem = "Existe campos em Branco ou invalidos, preencha-os por favor!";
                }
            }
            //else
            //{
            //    sql.Append("UPDATE Caixa SET ID_Material = @ID_Material, Transacao = @Transacao, Pesagem = @Pesagem, Valor_Total = @Valor_Total, Data_Transacao = @Data_Transacao, ID_Cliente = @ID_Cliente, Motivo_Exclusao = @Motivo ");
            //    sql.Append(" Where ID = @ID");
            //    mensagem = "Venda atualizada com Sucesso!!!";
            //}

            if (!string.IsNullOrEmpty(sql.ToString()))
            {
                using (contexto = new Contexto())
                {
                    List<SqlParameter> param = new List<SqlParameter>()
                {
                    new SqlParameter { ParameterName = "@ID_Material", SqlDbType = SqlDbType.VarChar, Value = entidade.ID_Material },
                    new SqlParameter { ParameterName = "@ID_Cliente", SqlDbType = SqlDbType.VarChar, Value = entidade.ID_Cliente },
                    new SqlParameter { ParameterName = "@Transacao", SqlDbType = SqlDbType.VarChar, Value = entidade.Transacao },
                    new SqlParameter { ParameterName = "@Valor_Total", SqlDbType = SqlDbType.VarChar, Value = entidade.Valor_Total },
                    new SqlParameter { ParameterName = "@Pesagem", SqlDbType = SqlDbType.VarChar, Value = entidade.Pesagem },
                    new SqlParameter { ParameterName = "@Data_Transacao", SqlDbType = SqlDbType.DateTime, Value = entidade.Data_Transacao.ToString("yyyy/MM/dd HH:mm:ss") }
                };

                    if (string.IsNullOrEmpty(entidade.Motivo_Exclusao))
                    {
                        param.Add(new SqlParameter { ParameterName = "@Motivo", SqlDbType = SqlDbType.VarChar, Value = "" });
                    }
                    else
                    {
                        param.Add(new SqlParameter { ParameterName = "@Motivo", SqlDbType = SqlDbType.VarChar, Value = entidade.Motivo_Exclusao });
                    }

                    if (entidade.ID_Caixa > 0)
                    {
                        param.Add(new SqlParameter { ParameterName = "@ID", SqlDbType = SqlDbType.Int, Value = entidade.ID_Caixa });
                    }
                    contexto.ExecutaComando(sql.ToString(), param);
                }
            }
            return mensagem.ToString();
        }

        public IEnumerable<CaixaVo> SemRemovidos()
        {
            using (contexto = new Contexto())
            {
                var strQuery = "SELECT * FROM Clientes Where Ativo = 1 order by Nome ";
                var retorno = contexto.ExecutaComRetorno(strQuery, new List<SqlParameter>());
                return ReaderObjeto(retorno);
            }
        }

        public IEnumerable<CaixaVo> SemBaixados()
        {
            using (contexto = new Contexto())
            {
                List<SqlParameter> param = new List<SqlParameter>();
                var strQuery = "Select cx.*, cl.Nome, mat.Descricao from Caixa cx ";
                strQuery += " INNER JOIN Clientes cl ON cl.ID_Cliente = cx.ID_Cliente ";
                strQuery += " INNER JOIN Material mat ON mat.ID_Material = cx.ID_Material ";
                strQuery += " where cx.Baixado = 0 order by cx.Data_Transacao desc";
                var retorno = contexto.ExecutaComRetorno(strQuery, new List<SqlParameter>());
                return ReaderObjeto(retorno);
            }
        }

        private List<CaixaVo> ReaderObjeto(SqlDataReader reader)
        {
            var caixa = new List<CaixaVo>();
            while (reader.Read())
            {
                var cliente = new ClienteVo()
                {
                    ID_Cliente = int.Parse(reader["ID_Cliente"].ToString()),
                    Nome = reader["Nome"].ToString()
                };
                var material = new MateriaisVo()
                {
                    ID_Material = int.Parse(reader["ID_Material"].ToString()),
                    Descricao = reader["Descricao"].ToString()
                };
                var temObjeto = new CaixaVo()
                {
                    ID_Caixa = int.Parse(reader["ID"].ToString()),
                    ID_Cliente = int.Parse(reader["ID_Cliente"].ToString()),
                    ID_Material = int.Parse(reader["ID_Material"].ToString()),
                    Cliente = cliente,
                    Material = material,
                    Data_Transacao = Convert.ToDateTime(reader["Data_Transacao"]),
                    Transacao = reader["Transacao"].ToString(),
                    Valor_Total = reader["ID_Material"].ToString(),
                    Pesagem = reader["ID_Material"].ToString(),
                    Motivo_Exclusao = reader["Motivo_Exclusao"].ToString(),
                    Baixado = bool.Parse(reader["Baixado"].ToString())
                };
                caixa.Add(temObjeto);
            }
            reader.Close();
            return caixa;
        }
    }
}
