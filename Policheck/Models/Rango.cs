using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Policheck.Models
{
    public class Rango
    {
        [JsonProperty("Nombre")]
        public string Nombre { get; set; }
    }
}
