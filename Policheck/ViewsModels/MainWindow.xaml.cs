﻿using MySql.Data.MySqlClient;
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


        //-----------------Parte Login----------------
        private void BtnEntrar(object sender, RoutedEventArgs e)
        {
            Acceder();
        }
        private async void Acceder()
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
                MnuItm_AltaFunc.Visibility = Visibility.Collapsed;
                SeparatorFunc.Visibility = Visibility.Collapsed;

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


        //-----------------Botones de menu----------------
        private void BtnPerfil(object sender, RoutedEventArgs e)
        {
            Formulario_Perfil();
            
            RellenarDatos();
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

        private void BtnCrearInciencia(object sender, RoutedEventArgs e)
        {
            pagina = 3;
            mnu_Inicial.Visibility = Visibility.Hidden;
            Vbx_Incidencias.Visibility = Visibility.Visible;
            Vbx_AccionesIncidencia.Visibility = Visibility.Visible;
            txtNumeroPlacaInc.Text = funcionario.NumeroPlaca;

        }

        private void BtnAltaCiudadano(object sender, RoutedEventArgs e)
        {
            pagina = 4;
            mnu_Inicial.Visibility = Visibility.Hidden;
            Vbx_Ciudadano.Visibility = Visibility.Visible;
            Vbx_AccionesCiudadano.Visibility = Visibility.Visible;
            txtNumeroPlacaCiu.Text = funcionario.NumeroPlaca;

            CargarEstadosJudiciales();
        }

        private void BtnDenuncia(object sender, RoutedEventArgs e)
        {
            pagina = 5;
            mnu_Inicial.Visibility = Visibility.Hidden;
            Vbx_Denuncia.Visibility = Visibility.Visible;
            Vbx_AccionesDenuncia.Visibility = Visibility.Visible;
            CargarCategoriaDenuncia();
            CargarDistritos();
        }
        private void Btn_Meritos(object sender, RoutedEventArgs e)
        {
            Meritos meritos = new Meritos(funcionario);
            meritos.Show();
        }

        private void Btn_CerrarSesion(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Esta seguro de que quiere cerrrar sesion? ", "Cerrar Sesion", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.No)
            {
                Vbx_InicioSesion.Visibility = Visibility.Visible;
                ToggleBackground(true);
                mnu_Inicial.Visibility = Visibility.Hidden;
                txtPlaca.Text = "";
                pwdContraseña.Password = "";
            }
        }


        //---------------Botones de creacion----------------



        //---------------Apartado de creacion de funcionarios----------------
        private async void BtnCrearFuncionario(object sender, RoutedEventArgs e)
            {

                Funcionario funcionario = new Funcionario();

                funcionario.NumeroPlaca = txtNumeroPlaca.Text;
                funcionario.Contrasena = passFuncionario.Password;
                funcionario.DNI = txtDNI.Text;
                funcionario.Genero = cmbxGenero.Text;
                funcionario.NombreCompleto = txtNombreFunc.Text;
                string Fecha = datpick_FechaNacimiento.SelectedDate.Value.ToString("yyyy-MM-dd");
                funcionario.Correo = txtCorreo.Text;
                funcionario.Telefono = txtTelefono.Text;
                funcionario.Turno = txtTurno.Text;
                funcionario.Rango = txtRango.Text;
                funcionario.Distrito = txtDistrito.Text;
                funcionario.PrimerApellido = txtPrimerApell.Text;
                funcionario.SegundoApellido = txtSegunApell.Text;

               int resultado = await _apiService.CrearFuncionario(funcionario.NumeroPlaca, funcionario.Contrasena, funcionario.DNI, funcionario.Genero, funcionario.NombreCompleto, 
                  Fecha, funcionario.Correo, funcionario.Telefono, funcionario.Turno, funcionario.Rango, funcionario.Distrito, funcionario.PrimerApellido, funcionario.SegundoApellido);


                if (resultado == 0)
                {
                    MessageBox.Show("Funcionario creado exitosamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);

                    txtNumeroPlaca.Text = "";
                    passFuncionario.Password = "";
                    txtDNI.Text = "";
                    cmbxGenero.Text = "";
                    txtNombreFunc.Text = "";
                    datpick_FechaNacimiento.SelectedDate = null;
                    txtCorreo.Text = "";
                    txtTelefono.Text = "";
                    txtTurno.Text = "";
                    txtRango.Text = "";
                    txtDistrito.Text = "";
                    txtPrimerApell.Text = "";
                    txtSegunApell.Text = "";
                    cmbxGenero.SelectedIndex = -1;
                    cmbx_Turno.SelectedIndex = -1;
                    cmbx_Rango.SelectedIndex = -1;
                    cmbx_Distrito.SelectedIndex = -1;
                }
                else
                {
                    if (resultado == -1)
                    {
                        MessageBox.Show("El DNI ya existe.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (resultado == -2)
                    {
                        MessageBox.Show("El DNI no puede ser nulo o vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (resultado == -3)
                    {
                        MessageBox.Show("El nombre no puede ser nulo o vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (resultado == -4)
                    {
                        MessageBox.Show("El primer apellido no puede ser nulo o vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (resultado == -5)
                    {
                        MessageBox.Show("El segundo apellido no puede ser nulo o vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (resultado == -6)
                    {
                        MessageBox.Show("El género es obligatorio y debe ser 'M' o 'F'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (resultado == -7)
                    {
                        MessageBox.Show("La fecha de nacimiento es obligatoria.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (resultado == -8)
                    {
                        MessageBox.Show("El rango no puede ser nulo o vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (resultado == -9)
                    {
                        MessageBox.Show("El correo electrónico no puede ser nulo o vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (resultado == -10)
                    {
                        MessageBox.Show("El teléfono no puede ser nulo o vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (resultado == -11)
                    {
                        MessageBox.Show("El distrito no puede ser nulo o vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (resultado == -12)
                    {
                        MessageBox.Show("El turno debe ser 'Mañana', 'Tarde' o 'Noche'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (resultado == -13)
                    {
                        MessageBox.Show("El número de placa no puede ser nulo o vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (resultado == -14)
                    {
                        MessageBox.Show("El distrito no fue encontrado en la base de datos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (resultado == -15)
                    {
                        MessageBox.Show("El rango no fue encontrado en la base de datos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Ocurrió un error desconocido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    }
                }

            private void BtnPlacAleatorio(object sender, RoutedEventArgs e)
            {
                string numPlaca =  funcionario.GeneradorDePlacas();

                txtNumeroPlaca.Text = numPlaca;
            }


            //-----------------Apartado de creacion de incidencias----------------
            private async void BtnCrearIncidencia(object sender, RoutedEventArgs e)
            {
            Incidencia incidencia = new Incidencia();

      
            incidencia.Titulo = txtTitulo.Text;
            incidencia.Descripcion = txtDescripcion.Text;
            incidencia.Tipo = cmbxTipo.Text;

            int resultado = await _apiService.CrearIncienciaAsync(funcionario.NumeroPlaca,incidencia.Titulo, incidencia.Descripcion, incidencia.Tipo);

            if (resultado == 0)
            {
                MessageBox.Show("Incidencia creada exitosamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);

                txtTitulo.Text = "";
                txtDescripcion.Text = "";
                cmbxTipo.Text = "";
                cmbxTipo.SelectedIndex = -1;
            }
            else
            {
                if (resultado == -1)
                {
                    MessageBox.Show("El título no puede ser nulo o vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (resultado == -2)
                {
                    MessageBox.Show("La descripción no puede ser nula o vacía.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (resultado == -3)
                {
                    MessageBox.Show("El tipo no puede ser nulo o vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (resultado == -4)
                {
                    MessageBox.Show("Numero de placa invalido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Ocurrió un error desconocido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
              
            }
        }


            //-----------------Apartado de creacion de ciudadanos----------------

            private async void BtnCrearCiudadano(object sender, RoutedEventArgs e)
            {
                Ciudadano ciudadano = new Ciudadano();

                ciudadano.DNI = txtDNICiu.Text;
                ciudadano.NombreCompleto = txtNombreCiu.Text;
                ciudadano.PrimerApellido = txtPrimerApellCiu.Text;
                ciudadano.SegundoApellido = txtSegunApellCiu.Text;
                ciudadano.Correo = txtCorreoCiu.Text;
                ciudadano.Telefono = txtTelefonoCiu.Text;
                ciudadano.EstadoJudcial = cmbx_EstadoJudicial.Text;
                ciudadano.Genero = cmbxGeneroCiu.Text;
                string Fecha = datpick_FechaNacimientoCiu.SelectedDate.Value.ToString("yyyy-MM-dd");
                ciudadano.Direccion = txtDireccionCiu.Text;

                int resultado = await _apiService.AltaCiudadanoAsync(funcionario.NumeroPlaca, ciudadano.DNI, ciudadano.NombreCompleto, ciudadano.PrimerApellido, ciudadano.SegundoApellido, ciudadano.Correo, ciudadano.Genero, Fecha, ciudadano.Telefono, ciudadano.Direccion, ciudadano.EstadoJudcial);
                  
                if (resultado == 0)
                {
                MessageBox.Show("Ciudadano creado exitosamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);



                txtDNICiu.Text = "";
                cmbxGeneroCiu.Text = "";
                txtNombreCiu.Text = "";
                datpick_FechaNacimientoCiu.SelectedDate = null;
                txtCorreoCiu.Text = "";
                txtTelefonoCiu.Text = "";
                txtPrimerApellCiu.Text = "";
                txtSegunApellCiu.Text = "";
                cmbxGeneroCiu.SelectedIndex = -1;
                cmbx_EstadoJudicial.SelectedIndex = -1;
                }

                else if (resultado == -1)
                    MessageBox.Show("El ciudadano ya existe.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else if (resultado == -2)
                    MessageBox.Show("El DNI no puede estar vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else if (resultado == -3)
                    MessageBox.Show("El nombre no puede estar vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else if (resultado == -4)
                    MessageBox.Show("El primer apellido no puede estar vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else if (resultado == -5)
                    MessageBox.Show("El segundo apellido no puede estar vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else if (resultado == -6)
                    MessageBox.Show("El género no puede estar vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else if (resultado == -7)
                    MessageBox.Show("La fecha de nacimiento no puede estar vacía.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else if (resultado == -8)
                    MessageBox.Show("El correo no puede estar vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else if (resultado == -9)
                    MessageBox.Show("El teléfono no puede estar vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else if (resultado == -10)
                    MessageBox.Show("La dirección no puede estar vacía.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else if (resultado == -11)
                    MessageBox.Show("El estado judicial no puede estar vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else if (resultado == -12)
                    MessageBox.Show("El correo debe contener '@'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else if (resultado == -13)
                    MessageBox.Show("El estado judicial no existe ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("Error desconocido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);


        }

            //-----------------Apartado de creacion de denuncias----------------
            private async void BtnCrearDenuncia(object sender, EventArgs e)
            {
                
                Denuncia denuncia = new Denuncia();

                denuncia.Direccion = txtDireccionDen.Text;
                denuncia.CP = txtCPDen.Text;
                denuncia.Distrito = cmbx_DistritoDen.Text;
                denuncia.Titulo = txtTituloDen.Text;
                denuncia.Descripcion = txtDescripcionDen.Text;
                denuncia.CategoriaDenuncia = cmbxCategoriaDen.Text;
                denuncia.DNICiudadano = txtDniDenuncia.Text;
                
                int resultado = await _apiService.CrearDenunciaAsync(denuncia.Direccion, denuncia.CP, denuncia.Distrito, denuncia.Titulo, denuncia.Descripcion, denuncia.CategoriaDenuncia, denuncia.DNICiudadano);




            if (resultado == 0)
            {
                MessageBox.Show("Denuncia creada exitosamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);


                txtDireccionDen.Text = "";
                txtCPDen.Text = "";
                txtDniDenuncia.Text = "";
                cmbx_DistritoDen.SelectedIndex = -1;
                txtTituloDen.Text = "";
                txtDescripcionDen.Text = "";
                cmbxCategoriaDen.SelectedIndex = -1;

            }
            else if (resultado == -1)
            {
                MessageBox.Show("Error: La dirección no puede estar vacía.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (resultado == -2)
            {
                MessageBox.Show("Error: El código postal no puede estar vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (resultado == -3)
            {
                MessageBox.Show("Error: El distrito no puede estar vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (resultado == -4)
            {
                MessageBox.Show("Error: El título no puede estar vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (resultado == -5)
            {
                MessageBox.Show("Error: La descripción no puede estar vacía.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (resultado == -6)
            {
                MessageBox.Show("Error: La categoría de denuncia no es válida.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (resultado == -7)
            {
                MessageBox.Show("Error: El DNI del ciudadano no puede estar vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (resultado == -8)
            {
                MessageBox.Show("Error: El distrito ingresado no es válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (resultado == -9)
            {
                MessageBox.Show("Error: El ciudadano no existe en la base de datos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Denuncia creada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        //---------------------------Boton Volver--------------------------------------------
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
            else if (pagina == 3)
            {
                mnu_Inicial.Visibility = Visibility.Visible;
                Vbx_Incidencias.Visibility = Visibility.Hidden;
                Vbx_AccionesIncidencia.Visibility = Visibility.Hidden;
                ToggleBackground(true);
            }
            else if (pagina == 4)
            {
                mnu_Inicial.Visibility = Visibility.Visible;
                Vbx_Ciudadano.Visibility = Visibility.Hidden;
                Vbx_AccionesCiudadano.Visibility = Visibility.Hidden;
                ToggleBackground(true);
            }
            else if (pagina == 5)
            {
                mnu_Inicial.Visibility = Visibility.Visible;
                Vbx_Denuncia.Visibility = Visibility.Hidden;
                Vbx_AccionesDenuncia.Visibility = Visibility.Hidden;
                ToggleBackground(true);
            }
           

        }


        //-----------------Apartado Carga y Seleccion de datos----------------
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
                cmbx_DistritoDen.ItemsSource = distritos;
                cmbx_DistritoDen.DisplayMemberPath = "Nombre";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async void CargarEstadosJudiciales()
        {
            try
            {
                var estados = await _apiService.GetEstadoJudicialAsync();
                cmbx_EstadoJudicial.ItemsSource = estados;
                cmbx_EstadoJudicial.DisplayMemberPath = "Nombre";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async void CargarCategoriaDenuncia()
        {
            try
            {
                var categorias = await _apiService.GetCategoriaDenunciaAsync();
                cmbxCategoriaDen.ItemsSource = categorias;
                cmbxCategoriaDen.DisplayMemberPath = "Nombre";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void SeleccionTurno(object sender, SelectionChangedEventArgs e)
        {
            if (cmbx_Turno.SelectedItem is ComboBoxItem comboBoxItem)
            {
                string turno = comboBoxItem.Content.ToString();
                txtTurno.Text = turno;
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

        private async void RellenarDatos()
        {

            txtbx_NPlaca.Text = funcionario.NumeroPlaca.ToString();
            // Espera a que la tarea termine y obtén los funcionarios
            var funcionarios = await _apiService.ObtenerDatosFuncionarioAsync(funcionario.NumeroPlaca);

            if (funcionarios != null && funcionarios.Count > 0)
            {
                var funcionario = funcionarios.First(); // Tomar el primer funcionario (o el único en este caso)

                // Ahora que tienes los datos del funcionario, rellena los campos
                txtbx_Nombre.Text = funcionario.NombreCompleto;
                pswd_contra.Password = funcionario.Contrasena;
                txtbx_Dni.Text = funcionario.DNI;
                txtbx_Genero.Text = funcionario.Genero;
                txtbx_FechNac.Text = funcionario.EdadActual.ToString();
                txtbx_Correo.Text = funcionario.Correo;
                txtbx_Telefono.Text = funcionario.Telefono.ToString(); // Telefono como int en JSON, convertimos a string
                txtbx_Turno.Text = funcionario.Turno;
                txtbx_Rango.Text = funcionario.Rango;
                txtbx_Distrito.Text = funcionario.Distrito;
                txtbx_Nombre.Text = funcionario.NombreCompleto;
            }
            else
            {
                MessageBox.Show("No se encontraron datos para el funcionario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        //---------------Funciones de diseño y Formularios----------------
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

        private void Formulario_Perfil()
        {
            pagina = 1;
            Vbx_Perfil.Visibility = Visibility.Visible;
            mnu_Inicial.Visibility = Visibility.Hidden;
        }

    }
}