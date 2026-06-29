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

        public Banco? GetBanco(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de búsqueda debe ser mayor a cero.");

            return _bancoData.GetBanco(id);
        }
        public Banco CreateBanco(Banco banco)
        {
            ValidarEntidad(banco);

            if (_bancoData.ExisteCodigoIdentificador(banco.CodigoIdentificador))
                throw new InvalidOperationException($"Ya existe un banco con el código '{banco.CodigoIdentificador}'.");

            banco.Activo = true;
            _bancoData.CreateBanco(banco);
            return banco;
        }

        public Banco EditBanco(Banco banco)
        {
            if (banco.Id <= 0)
                throw new ArgumentException("El ID del banco a modificar es inválido.");

            if (!_bancoData.ExisteBanco(banco.Id))
                throw new KeyNotFoundException($"No se encontró el banco con ID {banco.Id}.");

            ValidarEntidad(banco);

            if (_bancoData.ExisteCodigoIdentificador(banco.CodigoIdentificador, banco.Id))
                throw new InvalidOperationException($"Ya existe otro banco con el código '{banco.CodigoIdentificador}'.");

            _bancoData.EditBanco(banco);
            return banco;
        }

        public void DeleteBanco(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de eliminación debe ser mayor a cero.");

            if (!_bancoData.ExisteBanco(id))
                throw new KeyNotFoundException($"No se encontró el banco con ID {id}.");

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