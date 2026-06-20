using System;
using System.Collections.Generic;
using System.Text;

namespace PosManager.Domain
{
    public abstract class Persona
    {
        private int id;
        private string nombre;
        private string apellidos;
        private string numeroIdentificacion;
        private string loginName;
        private string password;
        private string passwordEncriptada;
        private string direccion;
        private string ciudad;
        private string nacionalidad;
        private string email;
        private string photo;
        private bool activo;

        public Persona()
        {

        }

        public Persona(int id, string nombre, string apellidos,
            string numeroIdentificacion, string loginName, string password,
            string passwordEncriptada, string direccion,
            string ciudad, string nacionalidad, string email,
            string photo, bool activo)
        {
            this.id = id;
            this.nombre = nombre;
            this.apellidos = apellidos;
            this.numeroIdentificacion = numeroIdentificacion;
            this.loginName = loginName;
            this.password = password;
            this.passwordEncriptada = passwordEncriptada;
            this.direccion = direccion;
            this.ciudad = ciudad;
            this.nacionalidad = nacionalidad;
            this.email = email;
            this.photo = photo;
            this.activo = activo;
        }

        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellidos { get => apellidos; set => apellidos = value; }
        public string NumeroIdentificacion { get => numeroIdentificacion; set => numeroIdentificacion = value; }
        public string LoginName { get => loginName; set => loginName = value; }
        public string Password { get => password; set => password = value; }
        public string PasswordEncriptada { get => passwordEncriptada; set => passwordEncriptada = value; }
        public string Direccion { get => direccion; set => direccion = value; }
        public string Ciudad { get => ciudad; set => ciudad = value; }
        public string Nacionalidad { get => nacionalidad; set => nacionalidad = value; }
        public string Email { get => email; set => email = value; }
        public string Photo { get => photo; set => photo = value; }
        public bool Activo { get => activo; set => activo = value; }
    }
}
