using PosManager.Data;
using PosManager.Domain;
using System;
using System.Collections.Generic;

namespace PosManager.Business
{
    public class BancoBusiness
    {
        private readonly BancoData _bancoData;

        public BancoBusiness(BancoData bancoData)
        {
            _bancoData = bancoData;
        }

        public List<Banco> GetBancos()
        {
            return _bancoData.GetBancos();
        }

        public Banco GetBanco(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de búsqueda debe ser mayor a cero.");

            return _bancoData.GetBanco(id);
        }

        public Banco CreateBanco(Banco banco)
        {
            ValidarEntidad(banco);
            banco.Activo = true; // Todo banco nuevo nace activo
            _bancoData.CreateBanco(banco);
            return banco;
        }

        public Banco EditBanco(Banco banco)
        {
            if (banco.Id <= 0)
                throw new ArgumentException("El ID del banco a modificar es inválido.");

            ValidarEntidad(banco);
            _bancoData.EditBanco(banco);
            return banco;
        }

        public void DeleteBanco(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de eliminación debe ser mayor a cero.");

            _bancoData.DeleteBanco(id);
        }

        private void ValidarEntidad(Banco banco)
        {
            if (banco == null)
                throw new ArgumentNullException(nameof(banco), "La información del banco no puede estar vacía.");

            if (string.IsNullOrWhiteSpace(banco.CodigoIdentificador))
                throw new ArgumentException("El Código Identificador del banco es obligatorio.");

            if (string.IsNullOrWhiteSpace(banco.Nombre))
                throw new ArgumentException("El Nombre del banco es obligatorio.");
        }
    }
}