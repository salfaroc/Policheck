using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Policheck.Models
{
   public class Merito
    {

        [JsonProperty("Merito")]
        public string NombreMerito { get; set; }
    }
}
