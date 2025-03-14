using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Policheck.Models
{
    public class Ciudadano
    {

        [JsonProperty("DNI")]
        public string DNI { get; set; }

        [JsonProperty("Nombre_Completo")]
        public string NombreCompleto { get; set; }

        [JsonProperty("Genero")]
        public string Genero { get; set; }

        [JsonProperty("Edad_Actual")]
        public int EdadActual { get; set; }

        [JsonProperty("Direccion_Ciudadano")]
        public string Direccion { get; set; }

        [JsonProperty("Nombre_Delito")]
        public string NombreDelito { get; set; }

        [JsonProperty("Cantidad_Delitos")]
        public int CantidadDelitos { get; set; }

        [JsonProperty("Multa_Economica")]
        public int Multa_Economica { get; set; }

        [JsonProperty("Tipo_Delito")]
        public string Tipo_Delito { get; set; }


        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string EstadoJudcial { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }







    }
}
