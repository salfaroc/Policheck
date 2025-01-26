using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using Policheck.Models;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Policheck
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


        }
        private DBManager _dbmanager = new DBManager();
        private AcDatos acDatos = new AcDatos();
        Funcionario funcionario = new Funcionario();


        private void BtnEntrar(object sender, RoutedEventArgs e)
        {
            Acceder();
        }

        private void Acceder()
        {
            string placaSTR = txtPlaca.Text;

            string pass = txtPass.Password;

            if (string.IsNullOrWhiteSpace(placaSTR) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Por favor, complete ambos campos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                int placa = Convert.ToInt32(placaSTR);
                _dbmanager.Inicio_Sesion(placa, pass);
                funcionario.Placa = placa;
                brdr_Login.Visibility = Visibility.Hidden;
                mnu_Inicial.Visibility = Visibility.Visible;

            }

        }


        private void Btn_Perfil(object sender, RoutedEventArgs e) {
            Formulario_Perfil();

            AcDatos acDatos = new AcDatos();

            MySqlConnection _conn = new MySqlConnection(acDatos.ClaveAcceso);
            _conn.Open();

            string consulta = "SELECT NumeroPlaca, contra ,dni, nombre, prapellido, segapellido, genero, nacimiento, correo, telefono, turno, Rango, Distrito " +
                              "FROM VistaFuncionario WHERE NumeroPlaca = @NumeroPlaca";

            MySqlCommand _cmd = new MySqlCommand(consulta, _conn);
            _cmd.Parameters.AddWithValue("@NumeroPlaca", funcionario.Placa);

            MySqlDataReader reader = _cmd.ExecuteReader();

            if (reader.Read())
            {
                // Asignamos los valores de la consulta a los controles de la interfaz
                txtbx_Telefono.Text = reader["telefono"].ToString();
                txtbx_Nombre.Text = reader["nombre"].ToString();
                txtbx_Dni.Text = reader["dni"].ToString();
                txtbx_FechNac.Text = reader["nacimiento"].ToString();
                txtbx_1Apell.Text = reader["prapellido"].ToString();
                pswd_contra.Password = reader["contra"].ToString();
                txtbx_Rango.Text = reader["Rango"].ToString();
                txtbx_Turno.Text = reader["turno"].ToString();
                txtbx_2Apellido.Text = reader["segapellido"].ToString();
                txtbx_Genero.Text = reader["genero"].ToString();
                txtbx_Correo.Text = reader["correo"].ToString();
                txtbx_NPlaca.Text = reader["NumeroPlaca"].ToString();
                txtbx_Distrito.Text = reader["Distrito"].ToString();
            }

            _conn.Close();
        }

    


        private void Formulario_Perfil()
        {
            lbl_NumeroPlaca.Visibility = Visibility.Visible;
            lbl_SegundoApellido.Visibility = Visibility.Visible;
            lbl_PrimerApellido.Visibility = Visibility.Visible;
            lbl_Nombre.Visibility = Visibility.Visible;
            lbl_DNI.Visibility = Visibility.Visible;
            lbl_Contrasena.Visibility = Visibility.Visible;
            lbl_Genero.Visibility = Visibility.Visible;
            lbl_FechaNacimiento.Visibility = Visibility.Visible;
            lbl_Rango.Visibility = Visibility.Visible;
            lbl_Correo.Visibility = Visibility.Visible;
            lbl_Telefono.Visibility = Visibility.Visible;
            lbl_Distrito.Visibility = Visibility.Visible;
            lbl_Turno.Visibility = Visibility.Visible;
            lbl_VerMisMeritos.Visibility = Visibility.Visible;

            txtbx_Telefono.Visibility = Visibility.Visible;
            txtbx_Nombre.Visibility = Visibility.Visible;
            txtbx_Dni.Visibility = Visibility.Visible;
            txtbx_FechNac.Visibility = Visibility.Visible;
            txtbx_1Apell.Visibility = Visibility.Visible;
            pswd_contra.Visibility = Visibility.Visible;
            txtbx_Rango.Visibility = Visibility.Visible;
            txtbx_Turno.Visibility = Visibility.Visible;
            txtbx_2Apellido.Visibility = Visibility.Visible;
            txtbx_Genero.Visibility = Visibility.Visible;
            txtbx_Correo.Visibility = Visibility.Visible;
            txtbx_NPlaca.Visibility = Visibility.Visible;
            txtbx_Distrito.Visibility = Visibility.Visible;
            img_Perfil.Visibility = Visibility.Visible;
            btn_Meritos.Visibility = Visibility.Visible;
        }

    }
}