using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using PosManager.Domain;

namespace PosManager.Data
{
    public class EmpleadoData
    {
        private readonly string _connectionString;

        public EmpleadoData(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Empleado> GetEmpleados()
        {
            var lista = new List<Empleado>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"SELECT id, apellidos, nombre, numero_identificacion, login_name, password, direccion, ciudad, nacionalidad,
                                           telefono_oficina, telefono_movil, email, photo, activo, estado_civil, password_temporal,
                                           numero_cuenta_bancaria, nombre_banco_cuenta
                                    FROM Empleado WHERE activo = 1";
                cmd.CommandType = System.Data.CommandType.Text;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(Mapear(reader));
                    }
                }
            }

            return lista;
        }

        public List<Empleado> GetEmpleadosByNombre(string nombre)
        {
            var lista = new List<Empleado>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"SELECT id, apellidos, nombre, numero_identificacion, login_name, password, direccion, ciudad, nacionalidad,
                                           telefono_oficina, telefono_movil, email, photo, activo, estado_civil, password_temporal,
                                           numero_cuenta_bancaria, nombre_banco_cuenta
                                    FROM Empleado WHERE nombre LIKE @nombre AND activo = 1";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@nombre", $"%{nombre}%");

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(Mapear(reader));
                    }
                }
            }

            return lista;
        }
        private Empleado Mapear(SqlDataReader reader)
        {
            return new Empleado
            {
                Id = Convert.ToInt32(reader["id"]),
                Apellidos = Convert.ToString(reader["apellidos"]) ?? string.Empty,
                Nombre = Convert.ToString(reader["nombre"]) ?? string.Empty,
                NumeroIdentificacion = Convert.ToString(reader["numero_identificacion"]) ?? string.Empty,
                LoginName = Convert.ToString(reader["login_name"]) ?? string.Empty,
                Password = Convert.ToString(reader["password"]) ?? string.Empty,
                Direccion = Convert.ToString(reader["direccion"]) ?? string.Empty,
                Ciudad = Convert.ToString(reader["ciudad"]) ?? string.Empty,
                Nacionalidad = Convert.ToString(reader["nacionalidad"]) ?? string.Empty,
                Email = Convert.ToString(reader["email"]) ?? string.Empty,
                Photo = Convert.ToString(reader["photo"]) ?? string.Empty,
                Activo = Convert.ToBoolean(reader["activo"]),

                EstadoCivil = Convert.ToString(reader["estado_civil"]) ?? string.Empty,
                TelefonoOficina = Convert.ToString(reader["telefono_oficina"]) ?? string.Empty,
                TelefonoMovil = Convert.ToString(reader["telefono_movil"]) ?? string.Empty,
                PasswordTemporal = Convert.ToString(reader["password_temporal"]) ?? string.Empty,
                NumeroCuentaBancaria = Convert.ToString(reader["numero_cuenta_bancaria"]) ?? string.Empty,
                NombreBancoCuenta = Convert.ToString(reader["nombre_banco_cuenta"]) ?? string.Empty
            };
        }
    }
}
