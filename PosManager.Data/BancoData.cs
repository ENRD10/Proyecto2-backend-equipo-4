using Microsoft.Data.SqlClient; // O System.Data.SqlClient según el plan que elegiste
using PosManager.Domain;
using System;
using System.Collections.Generic;

namespace PosManager.Data
{
    public class BancoData
    {
        private readonly string _connectionString;

        public BancoData(string connectionString)
        {
            _connectionString = connectionString;
        }

        // 1. GET - GetBancos
        public List<Banco> GetBancos()
        {
            var lista = new List<Banco>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT id, codigo_identificador_banco, nombre, email, ciudad, fax, zip, pais, direccion, telefono, estado_pais, activo FROM Banco WHERE activo = 1";
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

        // 2. GET - GetBanco
        public Banco? GetBanco(int id)
        {
            Banco? banco = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT id, codigo_identificador_banco, nombre, email, ciudad, fax, zip, pais, direccion, telefono, estado_pais, activo FROM Banco WHERE id = @id AND activo = 1";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        banco = Mapear(reader);
                    }
                }
            }
            return banco;
        }

        public bool ExisteBanco(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT COUNT(1) FROM Banco WHERE id = @id AND activo = 1";
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.Parameters.AddWithValue("@id", id);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public bool ExisteCodigoIdentificador(string codigo, int excludeId = 0)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT COUNT(1) FROM Banco WHERE codigo_identificador_banco = @codigo AND id <> @excludeId AND activo = 1";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@codigo", codigo);
                cmd.Parameters.AddWithValue("@excludeId", excludeId);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        // 3. POST - CreateBanco (Cambiado para que coincida con el endpoint)
        public Banco CreateBanco(Banco banco)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.Transaction = transaction;
                    cmd.CommandText = @"INSERT INTO Banco 
                        (codigo_identificador_banco, nombre, email, ciudad, fax, zip, pais, direccion, telefono, estado_pais, activo) 
                        OUTPUT INSERTED.id 
                        VALUES (@codigo, @nombre, @email, @ciudad, @fax, @zip, @pais, @direccion, @telefono, @estadoPais, @activo)";
                    cmd.CommandType = System.Data.CommandType.Text;

                    SetParametros(cmd, banco);

                    banco.Id = (int)cmd.ExecuteScalar();
                    transaction.Commit();
                    return banco;
                }
                catch (SqlException)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        // 4. PUT - EditBanco (Cambiado para que coincida con el endpoint)
        public Banco EditBanco(Banco banco)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.Transaction = transaction;
                    cmd.CommandText = @"UPDATE Banco SET 
                        codigo_identificador_banco = @codigo, nombre = @nombre, 
                        email = @email, ciudad = @ciudad, fax = @fax, 
                        zip = @zip, pais = @pais, direccion = @direccion, 
                        telefono = @telefono, estado_pais = @estadoPais, activo = @activo 
                        WHERE id = @id";
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.AddWithValue("@id", banco.Id);
                    SetParametros(cmd, banco);

                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                    return banco;
                }
                catch (SqlException)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        // 5. DELETE - DeleteBanco (Cambiado para que coincida con el endpoint)
        public void DeleteBanco(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.Transaction = transaction;
                    cmd.CommandText = "UPDATE Banco SET activo = 0 WHERE id = @id"; // Borrado lógico
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (SqlException)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private Banco Mapear(SqlDataReader reader)
        {
            return new Banco
            {
                Id = Convert.ToInt32(reader["id"]),
                CodigoIdentificador = reader["codigo_identificador_banco"].ToString()!,
                Nombre = reader["nombre"].ToString()!,
                Email = reader["email"] != DBNull.Value ? reader["email"].ToString() : null,
                Ciudad = reader["ciudad"] != DBNull.Value ? reader["ciudad"].ToString() : null,
                Fax = reader["fax"] != DBNull.Value ? reader["fax"].ToString() : null,
                Zip = reader["zip"] != DBNull.Value ? reader["zip"].ToString() : null,
                Pais = reader["pais"] != DBNull.Value ? reader["pais"].ToString() : null,
                Direccion = reader["direccion"] != DBNull.Value ? reader["direccion"].ToString() : null,
                Telefono = reader["telefono"] != DBNull.Value ? reader["telefono"].ToString() : null,
                EstadoPais = reader["estado_pais"] != DBNull.Value ? reader["estado_pais"].ToString() : null,
                Activo = Convert.ToBoolean(reader["activo"])
            };
        }

        private void SetParametros(SqlCommand cmd, Banco banco)
        {
            cmd.Parameters.AddWithValue("@codigo", (object?)banco.CodigoIdentificador ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@nombre", (object?)banco.Nombre ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object?)banco.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ciudad", (object?)banco.Ciudad ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@fax", (object?)banco.Fax ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@zip", (object?)banco.Zip ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@pais", (object?)banco.Pais ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@direccion", (object?)banco.Direccion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@telefono", (object?)banco.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@estadoPais", (object?)banco.EstadoPais ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@activo", banco.Activo);
        }

    }
}