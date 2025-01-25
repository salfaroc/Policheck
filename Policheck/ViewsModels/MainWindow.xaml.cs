using Org.BouncyCastle.Utilities;
using Policheck.Models;
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
        DBManager _dbmanager = new DBManager();

     

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


                Funcionario funcionario = new Funcionario();
                funcionario.Placa = placa;


                brdr_Login.Visibility = Visibility.Hidden;
                mnu_Inicial.Visibility = Visibility.Visible;

            }

        }


        private void Btn_Perfil(object sender, RoutedEventArgs e) {
           Formulario_Perfil();
        
            
        
        
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