using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Policheck.Models
{
    public  class Funcionario
    {
        [JsonProperty("Numero_Placa")]
        public  string NumeroPlaca { get; set; }

        [JsonProperty("Contrasena")]
        public string Contrasena { get; set; }

        [JsonProperty("DNI")]
        public string DNI { get; set; }

        [JsonProperty("Genero")]
        public string Genero { get; set; }

        [JsonProperty("Nombre_Completo")]
        public string NombreCompleto { get; set; }

        [JsonProperty("Edad_Actual")]
        public int EdadActual { get; set; }

        [JsonProperty("Correo")]
        public string Correo { get; set; }

        [JsonProperty("Telefono")]
        public string Telefono { get; set; }

        [JsonProperty("Turno")]
        public string Turno { get; set; }

        [JsonProperty("Rango")]
        public string Rango { get; set; }

        [JsonProperty("Distrito")]
        public string Distrito { get; set; }


        public string PrimerApellido { get; set; }

        public string SegundoApellido { get; set; }

        public string GeneradorDePlacas()
        {
            Random random = new Random();
            int placa = random.Next(100000, 999999);
            return placa.ToString();
        }


    }

}
