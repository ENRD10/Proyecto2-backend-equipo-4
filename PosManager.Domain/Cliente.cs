using System;
using System.Collections.Generic;
using System.Text;

namespace PosManager.Domain
{
    public class Cliente : Persona
    {
        private string telefonoFijo;
        private string telefonoMovil;

        public Cliente()
        {

        }

        public Cliente(string telefonoFijo, string telefonoMovil)
        {
            this.telefonoFijo = telefonoFijo;
            this.telefonoMovil = telefonoMovil;
        }

        public string TelefonoFijo { get => telefonoFijo; set => telefonoFijo = value; }
        public string TelefonoMovil { get => telefonoMovil; set => telefonoMovil = value; }
    }
}
