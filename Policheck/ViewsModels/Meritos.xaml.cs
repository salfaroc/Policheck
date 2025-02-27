using Policheck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Policheck.Views
{
    /// <summary>
    /// Lógica de interacción para Meritos.xaml
    /// </summary>
    public partial class Meritos : Window
    {
        private Funcionario funcionario;
        private ApiService _apiService = new ApiService();

        public Meritos() // Constructor vacío para evitar errores en XAML
        {
            InitializeComponent();
        }

        public Meritos(Funcionario funcionario) : this() // Llama al constructor vacío primero
        {
            this.funcionario = funcionario; // Usa "this." para asignar correctamente
            CargarMeritos();
        }

        private async void CargarMeritos()
        {
            try
            {
                if (funcionario == null || string.IsNullOrEmpty(funcionario.NumeroPlaca))
                {
                    MessageBox.Show("No hay un funcionario seleccionado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Obtener la lista de méritos de la API
                var meritos = await _apiService.ObtenerMeritosAsync(funcionario.NumeroPlaca);

                // Verificar si la lista es null o vacía
                if (meritos == null || meritos.Count == 0)
                {
                    MessageBox.Show("No se encontraron méritos.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                // Asignar la lista de méritos al DataGrid
                DataGridMeritos.ItemsSource = meritos;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }

}
