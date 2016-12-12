using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Ferro_Velho.Repositorio
{
    public class Contexto : IDisposable
    {
        private readonly SqlConnection conexao;

        public Contexto()
        {
            conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString);
            conexao.Open();
        }

        public void ExecutaComando(string strQuery, List<SqlParameter> param)
        {
            var cmd = new SqlCommand
            {
                CommandText = strQuery,
                CommandType = CommandType.Text,
                Connection = conexao
            };
            foreach (var parametro in param)
            {
                cmd.Parameters.Add(parametro);
            }
            cmd.ExecuteNonQuery();
        }

        public SqlDataReader ExecutaComRetorno(string strQuery, List<SqlParameter> param)
        {
            var cmd = new SqlCommand(strQuery, conexao);
            foreach (var parametro in param)
            {
                cmd.Parameters.Add(parametro);
            }
            return cmd.ExecuteReader();
        }

        public void Dispose()
        {
            if (conexao.State == ConnectionState.Open)
                conexao.Close();
        }
    }
}
