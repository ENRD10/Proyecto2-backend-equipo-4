using System;
using System.Collections.Generic;
using PosManager.Data;
using PosManager.Domain;

namespace PosManager.Business
{
    public class ClienteBusiness
    {
        private readonly ClienteData _clienteData;

        public ClienteBusiness(ClienteData clienteData)
        {
            _clienteData = clienteData;
        }

        public List<Cliente> GetClientes()
        {
            return _clienteData.GetClientes();
        }

        public Cliente? GetCliente(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de búsqueda debe ser mayor a cero.");

            return _clienteData.GetCliente(id);
        }
    }
}
