using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Policheck.Models
{
    public class Denuncia
    {
        [JsonProperty("DNI")]
        public string DNICiudadano { get; set; }
        [JsonProperty("Nombre_Completo")]
        public string NombreCiudadano { get; set; }
        [JsonProperty("Titulo_Denuncia")]
        public string Titulo { get; set; }
        [JsonProperty("Descripcion_Denuncia")]
        public string Descripcion { get; set; }

        [JsonProperty("Categoria_Denuncia")]
        public string CategoriaDenuncia { get; set; }

        public string Direccion { get; set; }
        public string CP { get; set; }
        public string Distrito { get; set; }

    }
}
