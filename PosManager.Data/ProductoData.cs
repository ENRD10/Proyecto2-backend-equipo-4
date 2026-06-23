using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using PosManager.Domain;

namespace PosManager.Data
{
    public class ProductoData
    {
        private readonly string _connectionString;

        public ProductoData(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Producto> GetProductos()
        {
            var lista = new List<Producto>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();

                // JOIN con Producto_Categoria y Categoria
                cmd.CommandText = @"
                    SELECT p.product_id, p.nombre_etiqueta, p.descripcion, p.existencia_en_stock, p.existencia_limite_para_almacenar,
                           p.precio_neto, p.precio_minimo, p.precio_minimo_con_impuesto, p.fecha_creacion, p.puede_venderse, p.puede_comprarse,
                           p.impuesto_valor_agregado, p.impuesto_local_1, p.impuesto_local_2, p.tiene_impuesto, p.codigo_barra, p.imagen,
                           p.codigo_contable_ventas, p.codigo_contable_compras, p.peso, p.unidad_peso, p.longitud, p.unidad_longitud,
                           p.superficie, p.unidad_superficie, p.volumen, p.unidad_volumen,
                           c.id_categoria, c.nombre_categoria
                    FROM Producto p
                    LEFT JOIN Producto_Categoria pc ON p.product_id = pc.fk_producto
                    LEFT JOIN Categoria c ON pc.fk_categoria = c.id_categoria";

                cmd.CommandType = System.Data.CommandType.Text;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var productosDict = new Dictionary<int, Producto>();

                    while (reader.Read())
                    {
                        int productId = Convert.ToInt32(reader["product_id"]);

                        if (!productosDict.ContainsKey(productId))
                        {
                            var producto = MapearProducto(reader);
                            producto.Categorias = new List<Categoria>();
                            productosDict[productId] = producto;
                        }

                        // Si hay categoría asociada, la agregamos
                        if (reader["id_categoria"] != DBNull.Value)
                        {
                            var categoria = new Categoria
                            {
                                IdCategoria = Convert.ToInt32(reader["id_categoria"]),
                                NombreCategoria = Convert.ToString(reader["nombre_categoria"]) ?? string.Empty
                            };
                            productosDict[productId].Categorias.Add(categoria);
                        }
                    }

                    lista.AddRange(productosDict.Values);
                }
            }

            return lista;
        }

        private Producto MapearProducto(SqlDataReader reader)
        {
            return new Producto
            {
                ProductId = Convert.ToInt32(reader["product_id"]),
                NombreEtiqueta = Convert.ToString(reader["nombre_etiqueta"]) ?? string.Empty,
                Descripcion = Convert.ToString(reader["descripcion"]) ?? string.Empty,
                ExistenciaEnStock = Convert.ToInt32(reader["existencia_en_stock"]),
                ExistenciaLimiteParaAlmacenar = Convert.ToInt32(reader["existencia_limite_para_almacenar"]),
                PrecioNeto = Convert.ToDecimal(reader["precio_neto"]),
                PrecioMinimo = Convert.ToDecimal(reader["precio_minimo"]),
                PrecioMinimoConImpuesto = Convert.ToDecimal(reader["precio_minimo_con_impuesto"]),
                FechaCreacion = Convert.ToDateTime(reader["fecha_creacion"]),
                PuedeVenderse = Convert.ToBoolean(reader["puede_venderse"]),
                PuedeComprarse = Convert.ToBoolean(reader["puede_comprarse"]),
                ImpuestoValorAgregado = Convert.ToDecimal(reader["impuesto_valor_agregado"]),
                ImpuestoLocal1 = Convert.ToDecimal(reader["impuesto_local_1"]),
                ImpuestoLocal2 = Convert.ToDecimal(reader["impuesto_local_2"]),
                TieneImpuesto = Convert.ToBoolean(reader["tiene_impuesto"]),
                CodigoBarra = Convert.ToString(reader["codigo_barra"]) ?? string.Empty,
                Imagen = Convert.ToString(reader["imagen"]) ?? string.Empty,
                CodigoContableVentas = Convert.ToString(reader["codigo_contable_ventas"]) ?? string.Empty,
                CodigoContableCompras = Convert.ToString(reader["codigo_contable_compras"]) ?? string.Empty,
                Peso = Convert.ToDecimal(reader["peso"]),
                UnidadPeso = Convert.ToString(reader["unidad_peso"]) ?? string.Empty,
                Longitud = Convert.ToDecimal(reader["longitud"]),
                UnidadLongitud = Convert.ToString(reader["unidad_longitud"]) ?? string.Empty,
                Superficie = Convert.ToDecimal(reader["superficie"]),
                UnidadSuperficie = Convert.ToString(reader["unidad_superficie"]) ?? string.Empty,
                Volumen = Convert.ToDecimal(reader["volumen"])
            };
        }
    }
}