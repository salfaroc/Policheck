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
using static Mysqlx.Crud.Order.Types;
using System.Net;
using Policheck;

public class ApiService
{
    private HttpClient _httpClient = new HttpClient();
    private string _url = "http://127.0.0.1:5000"; // URL de la API Flask

    public async Task<ResultadoLogin> LoginAsync(int numPlaca, string password)
    {
        try
        {
            string passHash = Encriptar(password);

            var loginData = new
            {
                numPlaca = numPlaca,
                pass = passHash
            };

            string jsonContent = System.Text.Json.JsonSerializer.Serialize(loginData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync($"{_url}/inicio-sesion", content);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                JsonNode jsonResponse = JsonNode.Parse(responseBody);

                // Extraer el resultado como int
                int resultado = jsonResponse["resultado"].GetValue<int>();

                // Extraer el token como string (si existe)
                string? token = null;
                if (jsonResponse["token"] != null)
                {
                    token = jsonResponse["token"].GetValue<string>();
                }

                return new ResultadoLogin
                {
                    Resultado = resultado,
                    Token = token
                };
            }
            else
            {
                throw new Exception($"Error de autenticación. Código de estado: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new ResultadoLogin
            {
                Resultado = -1, // Valor por defecto en caso de error
                Token = null
            };
        }
    }
    public async Task<List<Funcionario>> ObtenerDatosFuncionarioAsync(string placa)
    {
        try
        {
            string url = "";

            if (placa != null)
            {
                url = $"{_url}/verFuncionarioPlaca/{placa}";
            }
            else
            {
                url = $"{_url}/verFuncionarioGeneral";
            }

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

            string passHash = Encriptar(contrasena);


            var registroData = new
            {

                
                    numPlaca = numeroPlaca,
                    contras = passHash,
                    dni = dni,
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

    public async Task<int> CrearIncienciaAsync(string numeroPlaca, string titulo, string descripcion, string tipo)
    {
        try
        {
            var incidenciaData = new
            {
                numPlaca = numeroPlaca,
                titulo = titulo,
                descripcion = descripcion,
                tipo = tipo
            };

            string jsonContent = System.Text.Json.JsonSerializer.Serialize(incidenciaData);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync($"{_url}/crearIncidencia", content);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                JsonNode jsonResponse = JsonNode.Parse(responseBody);
                int resultado = jsonResponse["resultado"].GetValue<int>();
                return resultado;
            }
            else
            {
                throw new Exception($"Error al crear incidencia. Código de estado: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return -1;
        }
    }

    public async Task<List<EstadoJudicial>>GetEstadoJudicialAsync()
    {
        try
        {
            string url = $"{_url}/cargarEstadoJudicial";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);
                if (jsonResponse == null || jsonResponse.estadoJudicial == null)
                    throw new Exception("Respuesta de la API no válida.");
                List<EstadoJudicial> estadoJudicial = jsonResponse.estadoJudicial.ToObject<List<EstadoJudicial>>();
                return estadoJudicial;
            }
            else
            {
                throw new Exception($"Error al obtener estado judicial. Código de estado: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }
    

    
   
    public async Task<List<Funcionario>> GetFuncionarioAsync(string Nplaca)
    {
        try
        {
            string url = "";

            if (Nplaca != null)
            {
                url = $"{_url}/verFuncionarioPlaca/{Nplaca}";
            }
            else
            {
                url = $"{_url}/verFuncionarioGeneral";
            }

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
    
    // ------------------------ CIUDADANOS ----------------
    public async Task<List<Ciudadano>>GetCiudadanosAsync(string dni)
    {
        try
        {
            string url = "";
            if (dni != null)
            {
                url = $"{_url}/verCiudadanosDni/{dni}";
            }
            else
            {
                url = $"{_url}/verCiudadanos";
            }


           
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);
                if (jsonResponse == null || jsonResponse.ciudadanos == null)
                    throw new Exception("Respuesta de la API no válida.");
                List<Ciudadano> ciudadanos = jsonResponse.ciudadanos.ToObject<List<Ciudadano>>();
                return ciudadanos;
            }
            else
            {
                throw new Exception($"Error al obtener ciudadanos. Código de estado: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

   

    public async Task<int> AltaCiudadanoAsync(string num_placa, string dni, string nombre, string apellido1, string apellido2,
                                           string correo, string genero, string fecha_nacimiento, string telefono,
                                           string direccion, string estado_judicial)
    {
        try
        {
            var ciudadanoData = new
            {
                numPlaca = num_placa,
                dni = dni,
                nombre = nombre,
                apellido1 = apellido1,
                apellido2 = apellido2,
                genero = genero,
                fechaNacimiento = fecha_nacimiento,
                correo = correo,
                telefono = telefono,
                direccion = direccion,
                estadoJudicial = estado_judicial
            };
            string jsonContent = System.Text.Json.JsonSerializer.Serialize(ciudadanoData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync($"{_url}/altaCiudadano", content);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                JsonNode jsonResponse = JsonNode.Parse(responseBody);
                int resultado = jsonResponse["resultado"].GetValue<int>();
                return resultado;
            }
            else
            {
                throw new Exception($"Error al dar de alta ciudadano. Código de estado: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return -1;
        }
    }


    // ------------------------ DENUNCIAS ----------------
  
    public async Task<List<Denuncia>> GetDenunciaAsync(string dni, string categoria)
    {
        string url = " ";
        try
        {
            if (dni != null)
            {
                 url = $"{_url}/verDenunciasDni/{dni}";
            }
            else if (categoria != null)
            {
                url = $"{_url}/verDenunciasCategoria/{categoria}";
            }
            else
            {
                url = $"{_url}/verDenuncias";
            }

                HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);

                if (jsonResponse == null || jsonResponse.denuncias == null)
                    throw new Exception("Respuesta de la API no válida.");

                List<Denuncia> denuncias = jsonResponse.denuncias.ToObject<List<Denuncia>>();
                return denuncias;
            }
            else
            {
                throw new Exception($"Error al obtener denuncias por DNI. Código: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    public async Task<int> CrearDenunciaAsync(string direccion, string cp, string distrito, string titulo, string descripcion, string categoria_denuncia, string dni_ciudadano)
    {
        try
        {
            var ciudadanoData = new
            {
                direccion = direccion,
                cp = cp,
                distrito = distrito,
                titulo = titulo,
                descripcion = descripcion,
                categoria_denuncia = categoria_denuncia,
                dni_ciudadano = dni_ciudadano
            };
            string jsonContent = System.Text.Json.JsonSerializer.Serialize(ciudadanoData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync($"{_url}/altaDenuncia", content);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                JsonNode jsonResponse = JsonNode.Parse(responseBody);
                int resultado = jsonResponse["resultado"].GetValue<int>();
                return resultado;
            }
            else
            {
                throw new Exception($"Error al dar de alta ciudadano. Código de estado: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return -1;
        }
    }
    // para el buscador
  
    // para crear nueva denuncia
    public async Task<List<Datos>> GetCategoriaDenunciaAsync()
    {
        try
        {
            string url = $"{_url}/cargarCategoriaDenuncia";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);
                if (jsonResponse == null || jsonResponse.categoriaDenuncia == null)
                    throw new Exception("Respuesta de la API no válida.");
                List<Datos> categoriaDenuncia = jsonResponse.categoriaDenuncia.ToObject<List<Datos>>();
                return categoriaDenuncia;
            }
            else
            {
                throw new Exception($"Error al obtener categoría de denuncia. Código de estado: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    // -----------------------------------------------------
    public async Task<int> ActualizarFuncionarioAsync(string placa, string distrito, string turno, string rango, string correo, string telefono)
    {
        try
        {
            string url = $"{_url}/actualizarDatos";


            var funcionarioData = new
            {
                numPlaca = placa,
                distrito = distrito,
                turno = turno,
                rango = rango,
                correo = correo,
                telefono = telefono,


            };


            string jsonContent = System.Text.Json.JsonSerializer.Serialize(funcionarioData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);

                int resultado = jsonResponse?.resultado;

                return resultado;
            }
            else
            {
                throw new Exception($"Error al actualizar funcionario. Código de estado: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return -1;
        }
    }

    public string Encriptar(string _cadenaAencriptar)
    {
        string result = string.Empty;
        byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
        result = Convert.ToBase64String(encryted);
        return result;
    }


}






