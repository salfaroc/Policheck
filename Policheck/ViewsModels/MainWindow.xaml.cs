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


    }
}