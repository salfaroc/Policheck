using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using Policheck.Models;
using Policheck.Views;
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

        
        public MainWindow()
        {
            InitializeComponent();
            

        }
    
        Funcionario funcionario = new Funcionario();
        ApiService _apiService = new ApiService();

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
                MessageBox.Show("Bienvenido agente " + $"{placa}", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                funcionario.NumeroPlaca = placa.ToString();
                brdr_Login.Visibility = Visibility.Hidden;
                mnu_Inicial.Visibility = Visibility.Visible;

            }
            else if (res == 0) 
            {
                MessageBox.Show("Bienvenido agente", "Info", MessageBoxButton.OK, MessageBoxImage.Warning);
                funcionario.NumeroPlaca = placa.ToString();
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

            txtbx_NPlaca.Text = funcionario.NumeroPlaca.ToString();
            // Espera a que la tarea termine y obtén los funcionarios
            var funcionarios = await _apiService.ObtenerDatosFuncionarioAsync(funcionario.NumeroPlaca);

            if (funcionarios != null && funcionarios.Count > 0)
            {
                var funcionario = funcionarios.First(); // Tomar el primer funcionario (o el único en este caso)

                // Ahora que tienes los datos del funcionario, rellena los campos
               // Cambié NumeroPlaca por Numero_Placa
                txtbx_Nombre.Text = funcionario.NombreCompleto; // Cambié NombreCompleto por Nombre_Completo
                pswd_contra.Password = funcionario.Contrasena;
                txtbx_Dni.Text = funcionario.DNI;// Mantiene el nombre igual
                txtbx_Genero.Text = funcionario.Genero; // Cambié Genero a la propiedad correcta
                txtbx_FechNac.Text = funcionario.EdadActual.ToString(); // Cambié EdadActual por Edad_Actual
                txtbx_Correo.Text = funcionario.Correo; // Igual
                txtbx_Telefono.Text = funcionario.Telefono.ToString(); // Telefono como int en JSON, convertimos a string
                txtbx_Turno.Text = funcionario.Turno; // Igual
                txtbx_Rango.Text = funcionario.Rango; // Igual
                txtbx_Distrito.Text = funcionario.Distrito;
                txtbx_Nombre.Text = funcionario.NombreCompleto;// Igual
            }
            else
            {
                MessageBox.Show("No se encontraron datos para el funcionario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Formulario_Perfil()
        {
            lbl_NumeroPlaca.Visibility = Visibility.Visible;
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
            pswd_contra.Visibility = Visibility.Visible;
            txtbx_Rango.Visibility = Visibility.Visible;
            txtbx_Turno.Visibility = Visibility.Visible;
            txtbx_Genero.Visibility = Visibility.Visible;
            txtbx_Correo.Visibility = Visibility.Visible;
            txtbx_NPlaca.Visibility = Visibility.Visible;
            txtbx_Distrito.Visibility = Visibility.Visible;
            img_Perfil.Visibility = Visibility.Visible;
            btn_Meritos.Visibility = Visibility.Visible;
        }
        private void Btn_Meritos(object sender, RoutedEventArgs e)
        {
            Meritos meritos = new Meritos(funcionario);
            meritos.Show();
        }


    }
}