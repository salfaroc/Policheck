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
        [JsonProperty("IdDenuncia")]
        public int Id_Denuncia { get; set; }

        [JsonProperty("DNI")]
        public string DNICiudadano { get; set; }

        [JsonProperty("Nombre_Completo")]
        public string NombreCiudadano { get; set; }

        [JsonProperty("Correo_Electronico")]
        public string CorreoElectronico { get; set; }

        [JsonProperty("Telefono")]
        public string Telefono { get; set; }

        [JsonProperty("Direccion_Denunciante")]
        public string DireccionDenunciante { get; set; }

        [JsonProperty("Genero")]
        public string Genero { get; set; }

        [JsonProperty("EDAD_ACTUAL")]
        public int EdadActual { get; set; }

        [JsonProperty("Direccion_Comisaria")]
        public string Direccion { get; set; }

        [JsonProperty("Codigo_Postal")]
        public string CP { get; set; }

        [JsonProperty("Distrito")]
        public string Distrito { get; set; }

        [JsonProperty("Titulo_Denuncia")]
        public string Titulo { get; set; }

        [JsonProperty("Descripcion_Denuncia")]
        public string Descripcion { get; set; }

        [JsonProperty("Categoria_Denuncia")]
        public string CategoriaDenuncia { get; set; }

    }
}
