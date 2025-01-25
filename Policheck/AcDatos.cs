using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Policheck
{
    internal class AcDatos
    {
        private string _cadena = "server=127.0.0.1;uid=root;pwd=1234;database=policheck;port=3309";
        private string _cadena2 = "server=127.0.0.1;uid=root;pwd=1234;database=policheck;port=3306";
        private MySqlConnection _conn;
        MySqlCommand _cmd;

        public AcDatos()
        {
            try
            {
               
                _conn = new MySqlConnection(_cadena);
                _conn.Open(); 
                Console.WriteLine("Conexión establecida con el puerto 3309.");
                _conn.Close();
            }
            catch (MySqlException ex1)
            {
                Console.WriteLine($"Error en la conexión con el puerto 3309: {ex1.Message}");

                try
                {
                    
                    _conn = new MySqlConnection(_cadena2);
                    _conn.Open(); 
                    Console.WriteLine("Conexión establecida con el puerto 3306.");
                    _conn.Close();
                }
                catch (MySqlException ex2)
                {

                    Console.WriteLine($"Error en la conexión con el puerto 3306: {ex2.Message}");
                    throw new Exception("No se pudo establecer la conexión con ninguna de las cadenas proporcionadas.");
                }
            }
          
        }

        //public void List<Funcionario> ObtenerFuncionarios()
        //{
        //    Funcionario aux;
        //    List<Funcionario> funcionarios = new List<Funcionario>();

        //    this._conn.Open();

        //    MySqlCommand query = new MySqlCommand("select * from funcionarios", this._conn);
        //    MySqlDataReader reader = query.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        aux = new Funcionario();
        //        aux.Placa = int.Parse(reader["placa"].ToString());
        //        aux.Pass = int.Parse(reader["pass"].ToString());

        //        funcionarios.Add(aux);
        //    }

        //    this._conn.Close();
        //    return funcionarios;

        //}
    

        public int PA_InicioSesion(int nPlaca, string contrasenna)
        {
            int resultado = -99;


            try
            {
                _conn.Open();
                _cmd = new MySqlCommand();
                MainWindow mainWindow = new MainWindow(_cmd);
                _cmd.Connection = _conn;
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.CommandText = "InicioSesion";


                _cmd.Parameters.AddWithValue("_numPlaca", nPlaca);
                _cmd.Parameters["_numPlaca"].Direction = ParameterDirection.Input;

                _cmd.Parameters.AddWithValue("_pass", contrasenna);
                _cmd.Parameters["_pass"].Direction = ParameterDirection.Input;

                _cmd.Parameters.AddWithValue("_resultado", MySqlDbType.Int32);
                _cmd.Parameters["_resultado"].Direction = ParameterDirection.Output;

                _cmd.ExecuteNonQuery();


                resultado = (int)_cmd.Parameters["_resultado"].Value;
                _conn.Close();

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                return resultado;
            }
            
            return resultado;




        }





    }

    

}
