using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Policheck.Models
{
    class Ciudadano
    {

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

        public string Direccion { get; set; }

        public string EstadoJudcial { get; set; }
        public string PrimerApellido { get; set; }

        public string SegundoApellido { get; set; }







    }
}
