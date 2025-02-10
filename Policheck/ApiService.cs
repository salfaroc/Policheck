using Policheck.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;

public class ApiService
{
    private HttpClient _httpClient = new HttpClient();
    private string _url = "http://127.0.0.1:5000"; // URL de la API Flask

    public async Task<int> LoginAsync(int numPlaca, string password)
    {
        try
        {
           
            var loginData = new
            {
                numPlaca = numPlaca,
                pass = password
            };

            
            string jsonContent = JsonSerializer.Serialize(loginData);

           
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

           
            HttpResponseMessage response = await _httpClient.PostAsync($"{_url}/inicio-sesion", content);

       
            if (response.IsSuccessStatusCode)
            {
         
                string responseBody = await response.Content.ReadAsStringAsync();

                JsonNode jsonResponse = JsonNode.Parse(responseBody);

               
                int resultado = jsonResponse["resultado"].GetValue<int>();

                return resultado;

            }
            else
            {
                
                throw new Exception($"Error de autenticación. Código de estado: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
           
            Console.WriteLine($"Error: {ex.Message}");
            return -1; 
        }
    }

    internal async Task<List<Funcionario>> ObtenerDatosFuncionarioAsync(int numPlaca)
    {
        try
        {
            // Realizar la solicitud HTTP GET
            HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/verFuncionario/{numPlaca}");

            if (response.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como una cadena JSON
                string responseBody = await response.Content.ReadAsStringAsync();

                // Parsear el JSON de la respuesta
                JsonNode jsonResponse = JsonNode.Parse(responseBody);

                // Extraer la lista de funcionarios del JSON
                var funcionariosJson = jsonResponse["funcionarios"].AsArray();

                // Deserializar el JSON en una lista de objetos Funcionario
                List<Funcionario> funcionarios = funcionariosJson.Select(funcionario => new Funcionario
                {
                    Placa = funcionario["Numero_Placa"].GetValue<int>(),
                    Contrasenna = funcionario["Contrasena"].ToString(),
                    DNI = funcionario["DNI"].ToString(),
                    Nombre = funcionario["Nombre_Completo"].ToString(),
                    PrimerApellido = funcionario["Primer_Apellido"].ToString(),
                    SegundoApellido = funcionario["Segundo_Apellido"].ToString(),
                    Genero = funcionario["Genero"].ToString(),
                    Edad = funcionario["Edad_Actual"].ToString(),
                    Correo = funcionario["Correo"].ToString(),
                    Telefono = funcionario["Telefono"].ToString(),
                    Turno = funcionario["Turno"].ToString(),
                    Rango = funcionario["Rango"].ToString(),
                    Distrito = funcionario["Distrito"].ToString()
                    
                }).ToList();

                return funcionarios;
            }
            else
            {
                throw new Exception($"Error al obtener los datos del funcionario. Código de estado: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }





}
