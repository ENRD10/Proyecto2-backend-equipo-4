using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using PosManager.Domain;

namespace PosManager.Data
{
    public class ClienteData
    {
        private readonly string _connectionString;

        public ClienteData(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Cliente> GetClientes()
        {
            var lista = new List<Cliente>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"SELECT id, apellidos, nombre, numero_identificacion, login_name, password, direccion, 
                                           ciudad, nacionalidad,telefono_fijo, telefono_movil, email, photo, activo
                                    FROM Cliente WHERE activo = 1";
                cmd.CommandType = System.Data.CommandType.Text;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var cliente = Mapear(reader);
                        lista.Add(cliente);
                    }
                }
            }

            return lista;
        }

        public Cliente? GetCliente(int id)
        {
            Cliente? cliente = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"SELECT id, apellidos, nombre, numero_identificacion, login_name, password, direccion, 
                                           ciudad, nacionalidad,telefono_fijo, telefono_movil, email, photo, activo
                                    FROM Cliente WHERE id = @id AND activo = 1";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cliente = Mapear(reader);
                    }
                }
            }

            return cliente;
        }
        private Cliente Mapear(SqlDataReader reader)
        {
            var cliente = new Cliente
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

                TelefonoFijo = Convert.ToString(reader["telefono_fijo"]) ?? string.Empty,
                TelefonoMovil = Convert.ToString(reader["telefono_movil"]) ?? string.Empty
            };

            return cliente;
        }
    }
}