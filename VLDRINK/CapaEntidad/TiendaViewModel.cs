using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class TiendaViewModel
    {
        public IPagedList<Producto> Productos { get; set; }

        public List<Categoria> Categorias { get; set; }

        public TiendaViewModel()
        {
            Categorias = new List<Categoria>();
        }
    }
}
