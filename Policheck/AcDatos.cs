using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using Policheck.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace Policheck
{
    internal class AcDatos
    {
        private string _cadena = "server=127.0.0.1;uid=root;pwd=1234;database=policheck;port=3309";
        private string _cadena2 = "server=127.0.0.1;uid=root;pwd=1234;database=policheck;port=3306";
        private MySqlConnection _conn;
        MySqlCommand _cmd;
        private int valida = 0;
        private string clave;
        public AcDatos()
        {
            try
            {
               
                _conn = new MySqlConnection(_cadena);
                _conn.Open(); 
                Console.WriteLine("Conexión establecida con el puerto 3309.");
                _conn.Close();
                valida = 1;
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
                    valida = 2;
                }
                catch (MySqlException ex2)
                {

                    Console.WriteLine($"Error en la conexión con el puerto 3306: {ex2.Message}");
                    throw new Exception("No se pudo establecer la conexión con ninguna de las cadenas proporcionadas.");
                }
            }
            if (valida == 1)
            {

                clave = _cadena;


            }
            else if (valida == 2) { 

                clave = _cadena2;
            }
          
        }


        


        public string ClaveAcceso
        {
            get { return clave.ToString(); }
            
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


        public void Perfil(int placa)
        {
            List<Funcionario> listaFuncionarios = new List<Funcionario>();

            _conn.Open();
            _cmd = new MySqlCommand();
            _cmd.Connection = _conn;

            string consulta = "SELECT NumeroPlaca, contra ,dni,nombre,prapellido,segapellido,genero,nacimiento,correo,telefono,turno,Rango,Distrito FROM VistaFuncionario where NumeroPlaca = @NumeroPlaca ";
            _cmd.CommandText = consulta;

            _cmd.Parameters.AddWithValue("@NumeroPlaca",placa);

            MySqlDataReader reader = _cmd.ExecuteReader();


            while (reader.Read())
            {
                Funcionario funcionario = new Funcionario();

                funcionario.Placa = Convert.ToInt32(reader["NumeroPlaca"]); 
                funcionario.Contrasenna = reader["contra"].ToString(); 
                funcionario.DNI = reader["dni"].ToString(); 
                funcionario.Nombre = reader["nombre"].ToString(); 
                funcionario.PrimerApellido = reader["prapellido"].ToString(); 
                funcionario.SegundoApellido = reader["segapellido"].ToString(); 
                funcionario.Genero = reader["genero"].ToString(); 
                funcionario.FechaNacimiento = reader["nacimiento"].ToString(); 
                funcionario.Correo = reader["correo"].ToString(); 
                funcionario.Telefono = reader["telefono"].ToString(); 
                funcionario.Turno = reader["turno"].ToString(); 
                funcionario.Rango = reader["Rango"].ToString(); 
                funcionario.Distrito = reader["Distrito"].ToString();



                listaFuncionarios.Add(funcionario);
            }

            reader.Close();
            _conn.Close();
        }



    }

    

}
