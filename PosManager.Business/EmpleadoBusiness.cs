using System.Collections.Generic;
using PosManager.Data;
using PosManager.Domain;

namespace PosManager.Business
{
    public class EmpleadoBusiness
    {
        private readonly EmpleadoData _empleadoData;

        public EmpleadoBusiness(EmpleadoData empleadoData)
        {
            _empleadoData = empleadoData;
        }

        public List<Empleado> GetEmpleados()
        {
            return _empleadoData.GetEmpleados();
        }
    }
}