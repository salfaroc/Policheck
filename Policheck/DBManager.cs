using Policheck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Policheck
{
    internal class DBManager
    {


        public AcDatos _ad = new AcDatos();



        public DBManager() { }


        public void Inicio_Sesion(int nPlaca,string pass)
        {
          

            int res = _ad.PA_InicioSesion(nPlaca, pass);

            if (res == -1) {
                MessageBox.Show("Usuario o contraseña incorrectos", "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else if (res == -2)
            {
                MessageBox.Show("Campo numero placa vacio", "Advertencia!!!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (res == -3)
            {
                MessageBox.Show("Campo contraseña vacio vacio", "Advertencia!!!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (res == -4)
            {
                MessageBox.Show("ACCESO DENEGADO", "Advertencia!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Bienvanido", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }




        }


     

    }
}
