using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Policheck.Models;

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

            
            string jsonContent = System.Text.Json.JsonSerializer.Serialize(loginData);

           
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

    public async Task<List<Funcionario>> ObtenerDatosFuncionarioAsync(string placa)
    {
        try
        {
            string url = $"{_url}/verFuncionarioPlaca/{placa}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);

                if (jsonResponse == null || jsonResponse.funcionarios == null)
                    throw new Exception("Respuesta de la API no válida.");

                List<Funcionario> funcionarios = jsonResponse.funcionarios.ToObject<List<Funcionario>>();
                return funcionarios;
            }
            else
            {
                throw new Exception($"Error al obtener funcionarios. Código de estado: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

}






