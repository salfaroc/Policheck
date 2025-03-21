﻿
using Policheck.Models;
using Policheck.Views;
using System.Data;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System;
using System.IO;
using System.Windows.Media.Imaging;


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
            lbl_MiNumeroPlaca.Content = placaSTR;
            string pass = pwdContraseña.Password;

            // Validación para asegurarse que ambos campos no estén vacíos
            if (string.IsNullOrWhiteSpace(placaSTR) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Por favor, complete ambos campos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validación para asegurarse de que la placa sea un número válido
            if (!int.TryParse(placaSTR, out int placa))
            {
                MessageBox.Show("Debe introducir un número de placa válido.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Llamada al servicio con los valores válidos de placa y contraseña
            var response = await _apiService.LoginAsync(placa, pass); // Asumiendo que placa es int y password es string
            switch (response.Resultado) // usuarioTipo puede ser un valor que defines según el tipo de usuario
            {
                case 1: // Éxito para usuario normal con rango autorizado
                    MessageBox.Show($"Bienvenido agente {placa}", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    funcionario.NumeroPlaca = placa.ToString();

                    // Llamar a la función para establecer la visibilidad para un usuario normal
                    VisibilidaSegunRango(false); // En este caso, es un usuario sin rango de admin
                    PlayLoginSound(true);
                    // Guardar el token si lo necesitas
                    if (response.Token != null)
                    {
                        funcionario.Token = response.Token; // Asumiendo que tienes una clase App con una propiedad estática
                    }
                    break;

                case 0: // Éxito para admin o usuario sin rango
                    MessageBox.Show("Bienvenido agente", "Info", MessageBoxButton.OK, MessageBoxImage.Warning);
                    funcionario.NumeroPlaca = placa.ToString();
                    PlayLoginSound(true);
                    // Llamar a la función para establecer la visibilidad para un admin
                    VisibilidaSegunRango(true); // En este caso, es un administrador
                    break;

                case -1: // Usuario no existe o admin inactivo
                    MessageBox.Show("Usuario o contraseña incorrecta", "ERROR!!", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;

                case -2: // Placa vacía o credenciales incorrectas (admin)
                    MessageBox.Show("Campo placa vacío", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;

                case -3: // Contraseña vacía
                    MessageBox.Show("Campo contraseña vacío", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;

                default: // Resultado inesperado
                    MessageBox.Show("Error desconocido al iniciar sesión", "ERROR!!", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }


        private void VisibilidaSegunRango(bool isAdmin)
        {
            // Ocultar o mostrar elementos comunes
            Vbx_InicioSesion.Visibility = Visibility.Hidden;
            mnu_Inicial.Visibility = Visibility.Visible;
            Img_Menu.Visibility = Visibility.Visible;
            Vbx_MenuBotones.Visibility = Visibility.Visible;

            // Dependiendo del tipo de usuario (Admin o Usuario normal)
            if (isAdmin)
            {
                grdMiPlaca.Visibility = Visibility.Visible;
                MnuItm_AltaFunc.Visibility = Visibility.Collapsed;
                SeparatorFunc.Visibility = Visibility.Collapsed;
                SeparatorVer.Visibility = Visibility.Collapsed;
                MnuItm_VerFunc.Visibility = Visibility.Collapsed;
            }
            else
            {
                grdMiPlaca.Visibility = Visibility.Visible;
            }
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("¿Estás seguro de que quieres salir?", "Confirmar salida", MessageBoxButton.YesNo, MessageBoxImage.Question
            );


            if (result == MessageBoxResult.Yes)
            {
                
                this.Close();

            }

        }


        //-----------------Botones de menu----------------
        private void BtnPerfil(object sender, RoutedEventArgs e)
        {
            Formulario_Perfil();
            Img_Menu.Visibility = Visibility.Collapsed;
            grdMiPlaca.Visibility = Visibility.Collapsed;
            Vbx_MenuBotones.Visibility = Visibility.Collapsed;
            RellenarDatos();
        }
        private void BtnAltaFuncionario(object sender, RoutedEventArgs e)
        {
            pagina = 2;
            mnu_Inicial.Visibility = Visibility.Hidden;
            Vbx_Funcionario.Visibility = Visibility.Visible;
            Vbx_AccionesFuncionario.Visibility = Visibility.Visible;
            CargarDistritos();
            CargarRangos();
            Img_Menu.Visibility = Visibility.Collapsed;
            grdMiPlaca.Visibility = Visibility.Collapsed;
            Vbx_MenuBotones.Visibility = Visibility.Collapsed;


        }

        private void BtnCrearInciencia(object sender, RoutedEventArgs e)
        {
            pagina = 3;
            mnu_Inicial.Visibility = Visibility.Hidden;
            Vbx_Incidencias.Visibility = Visibility.Visible;
            Vbx_AccionesIncidencia.Visibility = Visibility.Visible;
            txtNumeroPlacaInc.Text = funcionario.NumeroPlaca;
            Img_Menu.Visibility = Visibility.Collapsed;
            grdMiPlaca.Visibility = Visibility.Collapsed;
            Vbx_MenuBotones.Visibility = Visibility.Collapsed;
        }

        private void BtnAltaCiudadano(object sender, RoutedEventArgs e)
        {
            pagina = 4;
            mnu_Inicial.Visibility = Visibility.Hidden;
            Vbx_Ciudadano.Visibility = Visibility.Visible;
            Vbx_AccionesCiudadano.Visibility = Visibility.Visible;
            txtNumeroPlacaCiu.Text = funcionario.NumeroPlaca;
            Img_Menu.Visibility = Visibility.Collapsed;
            grdMiPlaca.Visibility = Visibility.Collapsed;
            Vbx_MenuBotones.Visibility = Visibility.Collapsed;

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
            Img_Menu.Visibility = Visibility.Collapsed;
            grdMiPlaca.Visibility = Visibility.Collapsed;
            Vbx_MenuBotones.Visibility = Visibility.Collapsed;
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
                Img_Menu.Visibility = Visibility.Collapsed;
                grdMiPlaca.Visibility = Visibility.Collapsed;
                Vbx_MenuBotones.Visibility = Visibility.Collapsed;
                PlayLoginSound(false);
            }
        }


        private void Btn_VerCiudadanos(object sender, RoutedEventArgs e)
        {
            string dni = null;
            pagina = 6;
            CargarCiudadanos(dni);
            Vbx_Datos.Visibility = Visibility.Visible;
            mnu_Inicial.Visibility = Visibility.Hidden;
            Vbx_AccionesDatos.Visibility = Visibility.Visible;
            btn_MasDetalles.Visibility = Visibility.Collapsed;
            Img_Menu.Visibility = Visibility.Collapsed;
            Vbx_FormularioCiudadano.Visibility = Visibility.Visible;
            cmbxCategoriaDatoDen.Visibility = Visibility.Collapsed;
            lblcategoria.Visibility = Visibility.Collapsed;
            btnModificardatos.Visibility = Visibility.Collapsed;
            grdMiPlaca.Visibility = Visibility.Collapsed;
            Vbx_MenuBotones.Visibility = Visibility.Collapsed;
        }

        private void Btn_VerFuncionarios(object sender, RoutedEventArgs e)
        {
            string placa = null;

            pagina = 7;
            CargarFuncionarios(placa);
            Vbx_Datos.Visibility = Visibility.Visible;
            mnu_Inicial.Visibility = Visibility.Hidden;
            Vbx_AccionesDatos.Visibility = Visibility.Visible;
            btn_MasDetalles.Visibility = Visibility.Collapsed;
            Img_Menu.Visibility = Visibility.Collapsed;
            Vbx_FormularioFuncionario.Visibility = Visibility.Visible;
            cmbxCategoriaDatoDen.Visibility = Visibility.Collapsed;
            lblcategoria.Visibility = Visibility.Collapsed;
            btnModificardatos.Visibility = Visibility.Visible;
            grdMiPlaca.Visibility = Visibility.Collapsed;
            Vbx_MenuBotones.Visibility = Visibility.Collapsed;
        }

        private void Btn_VerDenuncias(object sender, RoutedEventArgs e)
        {

            string dni = null;
            string categoria = null;
            pagina = 8;
            Vbx_Datos.Visibility = Visibility.Visible;
            Vbx_FormularioDenuncia.Visibility = Visibility.Visible;
            mnu_Inicial.Visibility = Visibility.Hidden;
            Img_Menu.Visibility = Visibility.Collapsed;
            Vbx_AccionesDatos.Visibility = Visibility.Visible;
            CargarDenuncias(dni, categoria);
            CargarCategoriaDenuncia();
            cmbxCategoriaDatoDen.Visibility = Visibility.Visible;
            lblcategoria.Visibility = Visibility.Visible;
            btnModificardatos.Visibility = Visibility.Visible;
            grdMiPlaca.Visibility = Visibility.Collapsed;
            Vbx_MenuBotones.Visibility = Visibility.Collapsed;
            btn_MasDetalles.Visibility = Visibility.Visible;
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
                grdMiPlaca.Visibility = Visibility.Visible;
                Img_Menu.Visibility = Visibility.Visible;
                Vbx_MenuBotones.Visibility = Visibility.Visible;
                ToggleBackground(true);
            }
            else if (pagina == 2)
            {
                mnu_Inicial.Visibility = Visibility.Visible;
                Vbx_Funcionario.Visibility = Visibility.Hidden;
                Vbx_AccionesFuncionario.Visibility = Visibility.Hidden;
                grdMiPlaca.Visibility = Visibility.Visible;
                Img_Menu.Visibility = Visibility.Visible;
                Vbx_MenuBotones.Visibility = Visibility.Visible;
                ToggleBackground(true);
            }
            else if (pagina == 3)
            {
                mnu_Inicial.Visibility = Visibility.Visible;
                Vbx_Incidencias.Visibility = Visibility.Hidden;
                Vbx_AccionesIncidencia.Visibility = Visibility.Hidden;
                grdMiPlaca.Visibility = Visibility.Visible;
                Img_Menu.Visibility = Visibility.Visible;
                Vbx_MenuBotones.Visibility = Visibility.Visible;
                ToggleBackground(true);
            }
            else if (pagina == 4)
            {
                mnu_Inicial.Visibility = Visibility.Visible;
                Vbx_Ciudadano.Visibility = Visibility.Hidden;
                Vbx_AccionesCiudadano.Visibility = Visibility.Hidden;
                grdMiPlaca.Visibility = Visibility.Visible;
                Img_Menu.Visibility = Visibility.Visible;
                Vbx_MenuBotones.Visibility = Visibility.Visible;
                ToggleBackground(true);
            }
            else if (pagina == 5)
            {
                mnu_Inicial.Visibility = Visibility.Visible;
                Vbx_Denuncia.Visibility = Visibility.Hidden;
                Vbx_AccionesDenuncia.Visibility = Visibility.Hidden;
                grdMiPlaca.Visibility = Visibility.Visible;
                Img_Menu.Visibility = Visibility.Visible;
                Vbx_MenuBotones.Visibility = Visibility.Visible;
                ToggleBackground(true);
            }
            else if (pagina == 6)
            {
                Vbx_Datos.Visibility = Visibility.Collapsed;
                mnu_Inicial.Visibility = Visibility.Visible;
                Vbx_AccionesDatos.Visibility = Visibility.Collapsed;
                Vbx_FormularioCiudadano.Visibility = Visibility.Collapsed;
                Img_Menu.Visibility = Visibility.Visible;
                grdMiPlaca.Visibility = Visibility.Visible;
                Img_Menu.Visibility = Visibility.Visible;
                Vbx_MenuBotones.Visibility = Visibility.Visible;

                ToggleBackground(true);
            }
            else if (pagina == 7)
            {
                Vbx_Datos.Visibility = Visibility.Collapsed;
                mnu_Inicial.Visibility = Visibility.Visible;
                Vbx_AccionesDatos.Visibility = Visibility.Collapsed;
                Vbx_FormularioFuncionario.Visibility = Visibility.Collapsed;
                Img_Menu.Visibility = Visibility.Visible;
                btn_Confirmar.Visibility = Visibility.Collapsed;
                cmbx_RangoFuncionario.Visibility = Visibility.Collapsed;
                cmbx_TurnoFuncionario.Visibility = Visibility.Collapsed;
                cmbx_DistritoFuncionario.Visibility = Visibility.Collapsed;
                btnModificardatos.IsEnabled = true;
                grdMiPlaca.Visibility = Visibility.Visible;
                Img_Menu.Visibility = Visibility.Visible;
                Vbx_MenuBotones.Visibility = Visibility.Visible;
                ToggleBackground(true);
            }
            else if (pagina == 8)
            {
                Vbx_Datos.Visibility = Visibility.Collapsed;
                mnu_Inicial.Visibility = Visibility.Visible;
                Vbx_AccionesDatos.Visibility = Visibility.Collapsed;
                Vbx_FormularioDenuncia.Visibility = Visibility.Collapsed;
                Img_Menu.Visibility = Visibility.Visible;
                btnModificardatos.IsEnabled = true;
                grdMiPlaca.Visibility = Visibility.Visible;
                Img_Menu.Visibility = Visibility.Visible;
                Vbx_MenuBotones.Visibility = Visibility.Visible;

                ToggleBackground(true);
            }

        }


        //-----------------Apartado Carga y Seleccion de datos----------------
        
        
        private async void CargarFuncionarios(string placa)
        {
            try
            {
               

                var funcionarios = await _apiService.GetFuncionarioAsync(placa);

                // Filtrar solo las propiedades necesarias
                var funcionariosFiltrados = funcionarios.Select(f => new
                {
                    f.NumeroPlaca,
                    f.NombreCompleto,
                    f.DNI,
                    f.Genero,
                    f.EdadActual,
                    f.Correo,
                    f.Telefono,
                    f.Turno,
                    f.Rango,
                    f.Distrito
              
                }).ToList();

                DtGrd_Datos.ItemsSource = funcionariosFiltrados;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async void CargarDenuncias(string dni, string categoria)
        {
            try
            {
                var denuncias = await _apiService.GetDenunciaAsync(dni, categoria);

                // Filtrar solo las propiedades necesarias
                var denunciasFiltradas = denuncias.Select(d => new
                {   d.Id_Denuncia,
                    d.DNICiudadano,
                    d.NombreCiudadano,
                    d.CategoriaDenuncia,
                    d.Titulo,
                    d.Descripcion,
                    d.Direccion,
                    d.CP,
                }).ToList();

                DtGrd_Datos.ItemsSource = denunciasFiltradas;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private async void CargarRangos()
        {
            try
            {
                var rangos = await _apiService.GetRangoAsync();
                cmbx_Rango.ItemsSource = rangos;
                cmbx_Rango.DisplayMemberPath = "Nombre";
                cmbx_RangoFuncionario.ItemsSource = rangos;
                cmbx_RangoFuncionario.DisplayMemberPath = "Nombre";
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
                cmbx_DistritoFuncionario.ItemsSource = distritos;
                cmbx_DistritoFuncionario.DisplayMemberPath = "Nombre";
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
                cmbxCategoriaDatoDen.ItemsSource = categorias;
                cmbxCategoriaDatoDen.DisplayMemberPath = "Nombre";


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async void CargarCiudadanos(string dni)
        {
            try
            {
                var ciudadanos = await _apiService.GetCiudadanosAsync(dni);

                // Filtrar solo las propiedades necesarias
                var ciudadanosFiltrados = ciudadanos.Select(c => new
                {
                    c.DNI,
                    c.NombreCompleto,
                    c.Genero,
                    c.EdadActual,
                    c.Direccion,
                    c.NombreDelito,
                    c.CantidadDelitos,
                    c.Multa_Economica,
                    c.Tipo_Delito
                }).ToList();

                DtGrd_Datos.ItemsSource = ciudadanosFiltrados;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        private void SeleccionTurno(object sender, SelectionChangedEventArgs e)
        {
            if (cmbx_Turno.SelectedItem is ComboBoxItem turnoItem)
            {
                txtTurno.Text = turnoItem.Content.ToString();
            }

            if (cmbx_TurnoFuncionario.SelectedItem is ComboBoxItem turnoFunItem)
            {
                txtTurnoFun.Text = turnoFunItem.Content.ToString();
            }
        }


        private void SeleccionRango(object sender, SelectionChangedEventArgs e)
        {

            if (cmbx_Rango.SelectedItem is Rango rangoseleccionado)
            {
                txtRango.Text = rangoseleccionado.Nombre;
               
            }

            if (cmbx_RangoFuncionario.SelectedItem is Rango rangoseleccionado2)
            {
                txtRangoFun.Text = rangoseleccionado2.Nombre;
            }



        }

        private void SeleccionDistrito(object sender, SelectionChangedEventArgs e)
        {

            if (cmbx_Distrito.SelectedItem is Distrito distritoseleccionado)
            {
                txtDistrito.Text = distritoseleccionado.Nombre;
                
            }

            if (cmbx_DistritoFuncionario.SelectedItem is Distrito distritoseleccionado2)
            {
                txtDistritoFun.Text = distritoseleccionado2.Nombre;
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

        
        private void  Btn_Modificar(object sender, RoutedEventArgs e)
        {

            if (pagina == 7)
            {

                CargarDistritos();
                CargarRangos();

                btn_Confirmar.Visibility = Visibility.Visible;
                cmbx_DistritoFuncionario.Visibility = Visibility.Visible;
                cmbx_RangoFuncionario.Visibility = Visibility.Visible;
                cmbx_TurnoFuncionario.Visibility = Visibility.Visible;

                Button button = sender as Button;


                button.Click += Btn_Confirmar;

                btnModificardatos.IsEnabled = false;

            }
            else if (pagina == 8)
            {
                btn_Confirmar.Visibility = Visibility.Visible;
                txtDireccionDenuncia.IsReadOnly = false;
                txtCpDenuncia.IsReadOnly = false;
                txttituloDenuncia.IsReadOnly = false;
                 txtdescripcionDenuncia.IsReadOnly = false;

                Button button = sender as Button;
                button.Click += Btn_Confirmar;
                btnModificardatos.IsEnabled = false;
            }
            
         
            
        }
        
    
        private async void Btn_Confirmar(object sender, RoutedEventArgs e)
        {
            if (pagina == 7)
            {
                string placa = txtNumPlacaFun.Text;
                string Rango = txtRangoFun.Text;
                string Distrito = txtDistritoFun.Text;
                string Turno = txtTurnoFun.Text;
                string Correo = txtCorreoFun.Text;
                string Telefono = txtTelefonoFun.Text;



                try
                {
                    int resultado = await _apiService.ActualizarFuncionarioAsync(placa, Distrito, Turno, Rango, Correo, Telefono);

                    if (resultado == 0)
                    {
                        placa = null;
                        MessageBox.Show("Datos actualizados correctamente", "Actualización exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                        CargarFuncionarios(placa);

                        
                        btn_Confirmar.Visibility = Visibility.Collapsed;

                        // Oculta los ComboBox
                        cmbx_DistritoFuncionario.SelectedIndex = -1;
                        cmbx_RangoFuncionario.SelectedIndex = -1;
                        cmbx_TurnoFuncionario.SelectedIndex = -1;
                        cmbx_DistritoFuncionario.Visibility = Visibility.Collapsed;
                        cmbx_RangoFuncionario.Visibility = Visibility.Collapsed;
                        cmbx_TurnoFuncionario.Visibility = Visibility.Collapsed;

                        VaciarCamposFuncionario();
                        // Opcional: hacer los TextBox de solo lectura nuevamente
                   
                        txtRangoFun.IsReadOnly = true;
                        txtDistritoFun.IsReadOnly = true;
                        txtTurnoFun.IsReadOnly = true;
                        btnModificardatos.IsEnabled = true;

                        txtNumeroPlaca.Text = "";
                        txtRangoFun.Text = "";
                        txtDistritoFun.Text = "";
                        txtTurnoFun.Text = "";
                        txtCorreoFun.Text = "";
                        txtTelefonoFun.Text = "";
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            else if (pagina == 8)
            {

                string IdDenuncia = txtNumeroDenuncia.Text;
                string direccion = txtDireccionDenuncia.Text;
                string CP = txtCpDenuncia.Text;
                string titulo = txttituloDenuncia.Text;
                string descripcion = txtdescripcionDenuncia.Text;

                try
                {
                    int resultado = await _apiService.ModificarDenunciaAsync(IdDenuncia, direccion, CP, titulo, descripcion);
                    if (resultado == 0)
                    {
                        IdDenuncia = null;
                        MessageBox.Show("Datos actualizados correctamente", "Actualización exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                        CargarDenuncias(null, null);
                        btn_Confirmar.Visibility = Visibility.Collapsed;
                        btnModificardatos.IsEnabled = true;

                        txtNumeroDenuncia.Text = "";  
                        txtDireccionDenuncia.Text = "";
                        txtCpDenuncia.Text = "";
                        txttituloDenuncia.Text = "";
                        txtdescripcionDenuncia.Text = "";


                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }


            }
        }


        private void Dgv_SeleccionarDatos(object sender, SelectionChangedEventArgs e)
        {
            if (DtGrd_Datos.SelectedItem != null)
            {



                var row = DtGrd_Datos.SelectedItem;

                if (pagina == 7)
                {
                    
                    txtNumPlacaFun.Text = row.GetType().GetProperty("NumeroPlaca")?.GetValue(row, null)?.ToString() ?? "";
                    txtDNIFun.Text = row.GetType().GetProperty("DNI")?.GetValue(row, null)?.ToString() ?? "";
                    txtGeneroFuncionario.Text = row.GetType().GetProperty("Genero")?.GetValue(row, null)?.ToString() ?? "";
                    txtNomFunc.Text = row.GetType().GetProperty("NombreCompleto")?.GetValue(row, null)?.ToString() ?? "";
                    txtEdadFuncionario.Text = row.GetType().GetProperty("EdadActual")?.GetValue(row, null)?.ToString() ?? "";
                    txtCorreoFun.Text = row.GetType().GetProperty("Correo")?.GetValue(row, null)?.ToString() ?? "";
                    txtTelefonoFun.Text = row.GetType().GetProperty("Telefono")?.GetValue(row, null)?.ToString() ?? "";
                    txtTurnoFun.Text = row.GetType().GetProperty("Turno")?.GetValue(row, null)?.ToString() ?? "";
                    txtRangoFun.Text = row.GetType().GetProperty("Rango")?.GetValue(row, null)?.ToString() ?? "";
                    txtDistritoFun.Text = row.GetType().GetProperty("Distrito")?.GetValue(row, null)?.ToString() ?? "";
                }
                else if (pagina == 8)
                {
                    txtNumeroDenuncia.Text = row.GetType().GetProperty("Id_Denuncia")?.GetValue(row, null)?.ToString() ?? "";
                    txtdniDenenuncia.Text = row.GetType().GetProperty("DNICiudadano")?.GetValue(row, null)?.ToString() ?? "";
                    txtNombreDenuncia.Text = row.GetType().GetProperty("NombreCiudadano")?.GetValue(row, null)?.ToString() ?? "";
                    txttituloDenuncia.Text = row.GetType().GetProperty("Titulo")?.GetValue(row, null)?.ToString() ?? "";
                    txtdescripcionDenuncia.Text = row.GetType().GetProperty("Descripcion")?.GetValue(row, null)?.ToString() ?? "";
                    txtcategoriaDen.Text = row.GetType().GetProperty("CategoriaDenuncia")?.GetValue(row, null)?.ToString() ?? "";
                    txtDireccionDenuncia.Text = row.GetType().GetProperty("Direccion")?.GetValue(row, null)?.ToString() ?? "";
                    txtCpDenuncia.Text = row.GetType().GetProperty("CP")?.GetValue(row, null)?.ToString() ?? "";
                }
                else if (pagina == 6)
                {
                    txtDNIC.Text = row.GetType().GetProperty("DNI")?.GetValue(row, null)?.ToString() ?? "";
                    txtNombreC.Text = row.GetType().GetProperty("NombreCompleto")?.GetValue(row, null)?.ToString() ?? "";
                    txtGeneroC.Text = row.GetType().GetProperty("Genero")?.GetValue(row, null)?.ToString() ?? "";
                    txtEdadC.Text = row.GetType().GetProperty("EdadActual")?.GetValue(row, null)?.ToString() ?? "";
                    txtDireccionC.Text = row.GetType().GetProperty("Direccion")?.GetValue(row, null)?.ToString() ?? "";
                    txtNombreDelitoC.Text = row.GetType().GetProperty("NombreDelito")?.GetValue(row, null)?.ToString() ?? "";
                    txtCantidadDelitosC.Text = row.GetType().GetProperty("CantidadDelitos")?.GetValue(row, null)?.ToString() ?? "";
                    txtMultaEconomicaC.Text = row.GetType().GetProperty("Multa_Economica")?.GetValue(row, null)?.ToString() ?? "";
                    txtCodigoPostalC.Text = row.GetType().GetProperty("Tipo_Delito")?.GetValue(row, null)?.ToString() ?? "";

                }
            }
        }

        private void VaciarCamposFuncionario()
        {

            txtNombreFunc.Text = "";
            txtNumeroPlaca.Text = "";
            txtDNI.Text = "";
            txtbx_Genero.Text = "";
            txtbx_FechNac.Text = "";
            txtCorreo.Text = "";
            txtTelefono.Text = "";
            txtTurno.Text = "";
            txtRango.Text = "";
            txtDistrito.Text = "";
        }
    
        
        private void FiltroGeneral(object sender, RoutedEventArgs e)
        {
           

            if (pagina == 6)
            {
            string dni = txtbx_buscar.Text;
            CargarCiudadanos(dni);
            }
            else if (pagina == 7)
            {
                string placa = txtbx_buscar.Text;
                CargarFuncionarios(placa);

            }
            else if (pagina == 8)
            {
                string dni = txtbx_buscar.Text;
                string categoria = cmbxCategoriaDatoDen.Text;
                if (String.IsNullOrEmpty(categoria))
                {
                    categoria = null;
                    CargarDenuncias(dni,categoria);
                }
                else
                {
                    dni = null;
                    CargarDenuncias(dni,categoria);
                }
               
            }

        }

        private async void Btn_Restablecer(object sender, RoutedEventArgs e)
        {

            if (pagina == 7)
            {
                string placa = null;
                txtbx_buscar.Text = "";
                CargarFuncionarios(placa);

                txtNumeroPlaca.Text = "";
                txtRangoFun.Text = "";
                txtDistritoFun.Text = "";
                txtTurnoFun.Text = "";
                txtCorreoFun.Text = "";
                txtTelefonoFun.Text = "";
            }
            else if (pagina == 8)
            {
                string dni = null;
                string categoria = null;
                txtbx_buscar.Text = "";
                CargarDenuncias(dni, categoria);

                txtNumeroDenuncia.Text = "";
                txtDireccionDenuncia.Text = "";
                txtCpDenuncia.Text = "";
                txttituloDenuncia.Text = "";
                txtdescripcionDenuncia.Text = "";
                txtdniDenenuncia.Text = "";
                txtNombreDenuncia.Text = "";
                txtcategoriaDen.Text = "";
                



            }
            else if (pagina == 6)
            {
                string dni = null;
                txtbx_buscar.Text = "";
                CargarCiudadanos(dni);

            }
        }

        private void Btn_MasDetalles(object sender, RoutedEventArgs e)
        {
            // Obtener el valor del campo de texto
            string id = txtNumeroDenuncia.Text;

            // Comprobar si el valor es nulo o está vacío, y en ese caso asignar "0"
            if (string.IsNullOrEmpty(id))
            {
                id = "0"; // Asigna "0" si está vacío o es null
            }

            // Crear la ventana de detalles con el id
            VentanaDatosDetallados ventanaDatosDetallados = new VentanaDatosDetallados(id);

            // Mostrar la ventana
            ventanaDatosDetallados.Show();
        }


        //private void PlayLoginSound(bool entrar)
        //{
        //    // Usar ruta relativa basada en la carpeta del proyecto
        //    string soundPath;
        //    if (entrar == true)
        //    {
        //        soundPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sonidos\introWnXp.wav");
        //    }
        //    else
        //    {
        //        soundPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sonidos\cierrewinXp.wav");
        //    }
        //        // Usando SoundPlayer para reproducir el sonido
        //        SoundPlayer player = new SoundPlayer(soundPath);
        //    player.Play();

        //}



        private void PlayLoginSound(bool entrar)
        {
            string soundPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                entrar ? @"Sonidos\introWnXp.wav" : @"Sonidos\cierrewinXp.wav");

            Console.WriteLine($"Ruta del sonido: {soundPath}");

            if (!File.Exists(soundPath))
            {
                Console.WriteLine("⚠️ Archivo de sonido no encontrado.");
                return;
            }

            try
            {
                SoundPlayer player = new SoundPlayer(soundPath);
                player.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al reproducir sonido: {ex.Message}");
            }
        }





    }
}
