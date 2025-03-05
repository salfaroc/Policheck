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

        int pagina = 0;
        public MainWindow()
        {
            InitializeComponent();
            ToggleBackground(true);

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

            string pass = pwdContraseña.Password;

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
                Vbx_InicioSesion.Visibility = Visibility.Hidden;
                mnu_Inicial.Visibility = Visibility.Visible;

            }
            else if (res == 0) 
            {
                MessageBox.Show("Bienvenido agente", "Info", MessageBoxButton.OK, MessageBoxImage.Warning);
                funcionario.NumeroPlaca = placa.ToString();
                Vbx_InicioSesion.Visibility = Visibility.Hidden;
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
            pagina = 1;
            Vbx_Perfil.Visibility = Visibility.Visible;
            mnu_Inicial.Visibility = Visibility.Hidden; 
        }
        private void Btn_Meritos(object sender, RoutedEventArgs e)
        {
            Meritos meritos = new Meritos(funcionario);
            meritos.Show();
        }


        private void ToggleBackground(bool mostrar)
        {
            if (mostrar)
            {


                // Asignar el fondo de imagen
                ImageBrush brushFondo = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("pack://application:,,,/Imagenes/imagenfondo2.png")),
                    Stretch = Stretch.Fill // Ajustar el fondo al tamaño del Grid
                };

                // Condición para mostrar logo o fondo de imagen
                if (mostrar)
                {
                    MainGrid.Background = brushFondo; // Asigna el fondo de imagen al Grid
                }

            }
            else
            {
                // Si 'mostrar' es falso, asignar un fondo blanco
                MainGrid.Background = Brushes.White; // Mantiene el fondo blanco cuando se oculta la imagen
            }
        }


        private void BtnAltaFuncionario(object sender, RoutedEventArgs e)
        {
            pagina = 2;
            mnu_Inicial.Visibility = Visibility.Hidden;
            Vbx_Funcionario.Visibility = Visibility.Visible;
            Vbx_Acciones.Visibility = Visibility.Visible;
            CargarDistritos();
            CargarRangos();


        }

        private async void BtnCrearFuncionario(object sender, RoutedEventArgs e)
        {


            funcionario.NumeroPlaca = txtNumeroPlaca.Text;
            funcionario.Contrasena =pswd_contra.Password;
            funcionario.DNI = txtDNI.Text;
            funcionario.Genero = txtGenero.Text;
            funcionario.NombreCompleto = txtbx_Nombre.Text;
            funcionario.EdadActual = ;
            funcionario.Correo = txtCorreo.Text;
            funcionario.Telefono = txtTelefono.Text;
            funcionario.Turno = txtTurno.Text;
            funcionario.Rango = txtRango.Text;
            funcionario.Distrito = txtDistrito.Text;
            int resultado = await _apiService.CrearFuncionarioAsync();

        }

        private void GeneradorDePlacas(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            int placa = random.Next(100000, 999999);
            txtNumeroPlaca.Text = placa.ToString();
        }

        private async void CargarRangos()
        {
            try
            {
                var rangos = await _apiService.GetRangoAsync();
                cmbx_Rango.ItemsSource = rangos;
                cmbx_Rango.DisplayMemberPath = "Nombre";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async void CargarDistritos()
        {
            try
            {
                var distritos = await _apiService.GetDistritosAsync();
                cmbx_Distrito.ItemsSource = distritos;
                cmbx_Distrito.DisplayMemberPath = "Nombre";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void SeleccionRango(object sender, SelectionChangedEventArgs e)
        {

            if (cmbx_Rango.SelectedItem is Rango rangoseleccionado)
            {
                txtRango.Text = rangoseleccionado.Nombre;
            }


        }

        private void SeleccionDistrito(object sender, SelectionChangedEventArgs e)
        {

            if (cmbx_Distrito.SelectedItem is Distrito distritoseleccionado)
            {
                txtDistrito.Text = distritoseleccionado.Nombre;
            }
        }

        private void Btn_Volver(object sender, RoutedEventArgs e)
        {
            if (pagina == 1)
            {
                mnu_Inicial.Visibility = Visibility.Visible;
                Vbx_Perfil.Visibility = Visibility.Hidden;
                ToggleBackground(true);
            }
            else if (pagina == 2)
            {
                mnu_Inicial.Visibility = Visibility.Visible;
                Vbx_Funcionario.Visibility = Visibility.Hidden;
                Vbx_Acciones.Visibility = Visibility.Hidden;
                ToggleBackground(true);
            }



        }
    }
}