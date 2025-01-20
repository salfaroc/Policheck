using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Policheck
{
    internal class DBManager
    {
        private string _serverData = "server=127.0.0.1;uid=root;pwd=1234;database=ColegioDB;port=3309";
        private MySqlConnection _conn = null;
    
        public DBManager()
        {
            this._conn = new MySqlConnection(this._serverData);
        }

        public void List<Funcionario> ObtenerFuncionarios()
        {
            Funcionario aux;
            List<Funcionario> funcionarios = new List<Funcionario>();

            this._conn.Open();

            MySqlCommand query = new MySqlCommand("select * from funcionarios", this._conn);
            MySqlDataReader reader = query.ExecuteReader();

            while (reader.Read())
            {
                aux = new Funcionario();
                aux.Placa = int.Parse(reader["placa"].ToString());
                aux.Pass = int.Parse(reader["pass"].ToString());

                funcionarios.Add(aux);
            }

            this._conn.Close();
            return funcionarios;

        }
    
    }

    

}
