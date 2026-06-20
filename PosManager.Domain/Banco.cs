using System;
using System.Collections.Generic;
using System.Text;

namespace PosManager.Domain
{
    public class Banco
    {
        private int id;
        private string codigoIdentificador;
        private string nombre;
        private string email;
        private string ciudad;
        private string fax;
        private string zip;
        private string pais;
        private string direccion;
        private string telefono;
        private string estadoPais;
        private bool activo;

        public Banco()
        {

        }

        public Banco(int id, string codigoIdentificador, string nombre,
            string email, string ciudad, string fax, string zip,
            string pais, string direccion, string telefono,
            string estadoPais, bool activo)
        {
            this.id = id;
            this.codigoIdentificador = codigoIdentificador;
            this.nombre = nombre;
            this.email = email;
            this.ciudad = ciudad;
            this.fax = fax;
            this.zip = zip;
            this.pais = pais;
            this.direccion = direccion;
            this.telefono = telefono;
            this.estadoPais = estadoPais;
            this.activo = activo;
        }

        public int Id { get => id; set => id = value; }
        public string CodigoIdentificador { get => codigoIdentificador; set => codigoIdentificador = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Email { get => email; set => email = value; }
        public string Ciudad { get => ciudad; set => ciudad = value; }
        public string Fax { get => fax; set => fax = value; }
        public string Zip { get => zip; set => zip = value; }
        public string Pais { get => pais; set => pais = value; }
        public string Direccion { get => direccion; set => direccion = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string EstadoPais { get => estadoPais; set => estadoPais = value; }
        public bool Activo { get => activo; set => activo = value; }
    }
}
