using System;
using System.Collections.Generic;
using System.Text;

namespace PosManager.Domain
{
    public class Empleado : Persona
    {
        private string estadoCivil;
        private string telefonoOficina;
        private string telefonoMovil;
        private string passwordTemporal;
        private string numeroCuentaBancaria;
        private string nombreBancoCuenta;

        public Empleado()
        {

        }

        public Empleado(string estadoCivil,
            string telefonoOficina,
            string telefonoMovil,
            string passwordTemporal,
            string numeroCuentaBancaria,
            string nombreBancoCuenta)
        {
            this.estadoCivil = estadoCivil;
            this.telefonoOficina = telefonoOficina;
            this.telefonoMovil = telefonoMovil;
            this.passwordTemporal = passwordTemporal;
            this.numeroCuentaBancaria = numeroCuentaBancaria;
            this.nombreBancoCuenta = nombreBancoCuenta;
        }

        public string EstadoCivil { get => estadoCivil; set => estadoCivil = value; }
        public string TelefonoOficina { get => telefonoOficina; set => telefonoOficina = value; }
        public string TelefonoMovil { get => telefonoMovil; set => telefonoMovil = value; }
        public string PasswordTemporal { get => passwordTemporal; set => passwordTemporal = value; }
        public string NumeroCuentaBancaria { get => numeroCuentaBancaria; set => numeroCuentaBancaria = value; }
        public string NombreBancoCuenta { get => nombreBancoCuenta; set => nombreBancoCuenta = value; }
    }
}
