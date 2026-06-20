using System;
using System.Collections.Generic;
using System.Text;

namespace PosManager.Domain
{
    public class Categoria
    {
        private int idCategoria;
        private string nombreCategoria;

        public Categoria()
        {

        }

        public Categoria(int idCategoria, string nombreCategoria)
        {
            this.idCategoria = idCategoria;
            this.nombreCategoria = nombreCategoria;
        }

        public int IdCategoria { get => idCategoria; set => idCategoria = value; }
        public string NombreCategoria { get => nombreCategoria; set => nombreCategoria = value; }
    }
}
