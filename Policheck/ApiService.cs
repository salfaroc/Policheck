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
using static Org.BouncyCastle.Asn1.Cmp.Challenge;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    public async Task<List<Merito>> ObtenerMeritosAsync(string placa)
    {
        try
        {
            string url = $"{_url}/verMeritos/{placa}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);
                if (jsonResponse == null || jsonResponse.meritos == null)
                    throw new Exception("Respuesta de la API no válida.");
                List<Merito> meritos = jsonResponse.meritos.ToObject<List<Merito>>();
                return meritos;
            }
            else
            {
                throw new Exception($"Error al obtener méritos. Código de estado: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    public async Task<List<Rango>> GetRangoAsync()
    {
        try
        {

            string url = "";

            url = $"{_url}/cargarRangos";



            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);

                if (jsonResponse == null || jsonResponse.rango == null)
                    throw new Exception("Respuesta de la API no válida.");

                List<Rango> rango = jsonResponse.rango.ToObject<List<Rango>>();
                return rango;
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

    public async Task<List<Distrito>> GetDistritosAsync()
    {

        try
        {

            string url = "";

            url = $"{_url}/cargarDistritos";


            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);

                if (jsonResponse == null || jsonResponse.distrito == null)
                    throw new Exception("Respuesta de la API no válida.");

                List<Distrito> distrito = jsonResponse.distrito.ToObject<List<Distrito>>();
                return distrito;
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

    public async Task<int> CrearFuncionario(string numeroPlaca, string contrasena, string dni, string genero, string nombre,
                                    string fechaNacimiento, string correo, string telefono, string turno, string rango,
                                    string distrito, string primerApellido, string segundoApellido)
    {
        try
        {

            var registroData = new
            {

                    numPlaca = numeroPlaca,
                    contras = contrasena,
                    dni= dni,
                    nombre = nombre,
                    apellido1 = primerApellido,
                    apellido2 = segundoApellido,
                    genero = genero,
                    fechaNacimiento = fechaNacimiento,
                    rango = rango,
                    correo = correo,
                    telefono = telefono,
                    distrito = distrito,
                    turno = turno


            };


            string jsonContent = System.Text.Json.JsonSerializer.Serialize(registroData);


            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


            HttpResponseMessage response = await _httpClient.PostAsync($"{_url}/altaFuncionario", content);


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

}






