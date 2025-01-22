using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Policheck.Models
{
    internal class Funcionario
    {

        private static int NPlaca;
        private string Pass;

        public int Placa {

            get { return NPlaca; }
            set { NPlaca = value; }

        }
        public string Contrasenna
        {
            get { return Pass; }
            set { Pass = value; }
        }
    }
}
