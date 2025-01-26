using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Policheck.Models
{
    internal class Funcionario
    {
        private static int nPlaca; // Número de Placa (estático, compartido entre todos los funcionarios)
        private string pass; // Contraseña
        private string dni;
        private string nombre;
        private string primerApellido;
        private string segundoApellido;
        private string genero;
        private string fechaNacimiento;
        private string correo;
        private string telefono;
        private string turno;
        private string rango;
        private string distrito;

        // Métodos para acceder a las variables (Getters y Setters)
        public int Placa
        {
            get { return nPlaca; }
            set { nPlaca = value; }
        }

        public string Contrasenna
        {
            get { return pass; }
            set { pass = value; }
        }

        public string DNI
        {
            get { return dni; }
            set { dni = value; }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public string PrimerApellido
        {
            get { return primerApellido; }
            set { primerApellido = value; }
        }

        public string SegundoApellido
        {
            get { return segundoApellido; }
            set { segundoApellido = value; }
        }

        public string Genero
        {
            get { return genero; }
            set { genero = value; }
        }

        public string FechaNacimiento
        {
            get { return fechaNacimiento; }
            set { fechaNacimiento = value; }
        }

        public string Correo
        {
            get { return correo; }
            set { correo = value; }
        }

        public string Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }

        public string Turno
        {
            get { return turno; }
            set { turno = value; }
        }

        public string Rango
        {
            get { return rango; }
            set { rango = value; }
        }

        public string Distrito
        {
            get { return distrito; }
            set { distrito = value; }
        }
    }
}
