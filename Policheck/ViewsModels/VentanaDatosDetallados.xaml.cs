using Policheck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Policheck
{
    /// <summary>
    /// Lógica de interacción para VentanaDatosDetallados.xaml
    /// </summary>
    public partial class VentanaDatosDetallados : Window
    {
        private string idDenuncia;
        public VentanaDatosDetallados(string idDenuncia)
        {
            InitializeComponent();
            this.idDenuncia = idDenuncia;
            CargarDatosDenuncia(idDenuncia);
            ToggleBackground(true);
        }

        ApiService _apiService = new ApiService();


        private async void CargarDatosDenuncia(string idDenuncia)
        {
            try
            {
                // Obtener las denuncias desde la API
                var denuncias = await _apiService.GetDenunciaPorIdAsync(idDenuncia);

                // Filtrar solo las propiedades necesarias
                var denunciasDetalladas = denuncias.Select(d => new
                {
                    d.Id_Denuncia,
                    d.DNICiudadano,
                    d.NombreCiudadano,
                    d.CorreoElectronico,
                    d.Telefono,
                    d.DireccionDenunciante,
                    d.Genero,
                    d.EdadActual,
                    d.Direccion,
                    d.CP,
                    d.Distrito,
                    d.Titulo,
                    d.Descripcion,
                    d.CategoriaDenuncia
                }).ToList();

                // Asignar el resultado al DataGrid
                DtGrd_Datos.ItemsSource = denunciasDetalladas;
            }
            catch (Exception ex)
            {
                // Manejar el error
                MessageBox.Show($"Error: {ex.Message}");
            }

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
                    DatosDetallados.Background = brushFondo; // Asigna el fondo de imagen al Grid
                }

            }
            else
            {
                // Si 'mostrar' es falso, asignar un fondo blanco
                DatosDetallados.Background = Brushes.White; // Mantiene el fondo blanco cuando se oculta la imagen
            }
        }


    }









    
}
