using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using Policheck.Models;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
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

       private ApiService _apiService = new ApiService();
        public MainWindow()
        {
            InitializeComponent();
            

        }
    
        Funcionario funcionario = new Funcionario();

        private void BtnEntrar(object sender, RoutedEventArgs e)
        {
            Acceder();
        }
        private void BtnPerfil(object sender, RoutedEventArgs e)
        {
            Formulario_Perfil();
            
            RellenarDatos();
        }

        private  async void Acceder()
        {
            string placaSTR = txtPlaca.Text;

            string pass = txtPass.Password;

            if (string.IsNullOrWhiteSpace(placaSTR) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Por favor, complete ambos campos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int placa = Convert.ToInt32(placaSTR);
            int res = await _apiService.LoginAsync(placa, pass);


            if (res == 1)
            {
                MessageBox.Show("Bienvenido agente ", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                funcionario.Placa = placa;
                brdr_Login.Visibility = Visibility.Hidden;
                mnu_Inicial.Visibility = Visibility.Visible;

            }
            else if (res == 0) 
            {
                MessageBox.Show("Bienvenido agente", "Info", MessageBoxButton.OK, MessageBoxImage.Warning);
                funcionario.Placa = placa;
                brdr_Login.Visibility = Visibility.Hidden;
                mnu_Inicial.Visibility = Visibility.Visible;
            }
            else if (res == -1)
            {
                MessageBox.Show("Usuario o contraseña incorrecta", "ERROR!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (res == -2)
            {
                MessageBox.Show("Campo placa vacio", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            else if (res == -3)
            {
                MessageBox.Show("Campo placa vacio", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }




        }



        private async void RellenarDatos()
        {
            // Espera a que la tarea termine y obtén los funcionarios
            var funcionarios = await _apiService.ObtenerDatosFuncionarioAsync(funcionario.Placa);

            if (funcionarios != null && funcionarios.Count > 0)
            {
                var funcionario = funcionarios.First(); // Tomar el primer funcionario (o el único en este caso)

                // Ahora que tienes los datos del funcionario, rellena los campos
                txtbx_NPlaca.Text = funcionario.Placa.ToString();
                txtbx_Nombre.Text = funcionario.Nombre;
                pswd_contra.Password = funcionario.Contrasenna;
                txtbx_1Apell.Text = funcionario.PrimerApellido;
                txtbx_2Apellido.Text = funcionario.SegundoApellido;
                txtbx_Genero.Text = funcionario.Genero;
                txtbx_FechNac.Text = funcionario.Edad; // Aquí si Edad es un string, lo estás colocando correctamente
                txtbx_Correo.Text = funcionario.Correo;
                txtbx_Telefono.Text = funcionario.Telefono;
                txtbx_Turno.Text = funcionario.Turno;
                txtbx_Rango.Text = funcionario.Rango;
                txtbx_Distrito.Text = funcionario.Distrito;
            }
            else
            {
                MessageBox.Show("No se encontraron datos para el funcionario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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