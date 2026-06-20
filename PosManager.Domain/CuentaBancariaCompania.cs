using System;
using System.Collections.Generic;
using System.Text;

namespace PosManager.Domain
{
    public class CuentaBancariaCompania
    {
        private int id;
        private string numeroCuenta;
        private string tipoCuenta;
        private string tipoDivisa;
        private string estado;
        private string pais;
        private string provincia;
        private string nombreDueno;
        private string apellidosDueno;

        private Banco banco;
        private Compania compania;

        public CuentaBancariaCompania()
        {

        }

        public CuentaBancariaCompania(int id, string numeroCuenta,
            string tipoCuenta, string tipoDivisa, string estado,
            string pais, string provincia, string nombreDueno,
            string apellidosDueno, Banco banco, Compania compania)
        {
            this.id = id;
            this.numeroCuenta = numeroCuenta;
            this.tipoCuenta = tipoCuenta;
            this.tipoDivisa = tipoDivisa;
            this.estado = estado;
            this.pais = pais;
            this.provincia = provincia;
            this.nombreDueno = nombreDueno;
            this.apellidosDueno = apellidosDueno;
            this.banco = banco;
            this.compania = compania;
        }

        public int Id { get => id; set => id = value; }
        public string NumeroCuenta { get => numeroCuenta; set => numeroCuenta = value; }
        public string TipoCuenta { get => tipoCuenta; set => tipoCuenta = value; }
        public string TipoDivisa { get => tipoDivisa; set => tipoDivisa = value; }
        public string Estado { get => estado; set => estado = value; }
        public string Pais { get => pais; set => pais = value; }
        public string Provincia { get => provincia; set => provincia = value; }
        public string NombreDueno { get => nombreDueno; set => nombreDueno = value; }
        public string ApellidosDueno { get => apellidosDueno; set => apellidosDueno = value; }
        public Banco Banco { get => banco; set => banco = value; }
        public Compania Compania { get => compania; set => compania = value; }
    }
}
