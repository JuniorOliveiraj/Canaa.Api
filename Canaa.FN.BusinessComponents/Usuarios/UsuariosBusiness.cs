using Canaa.Infra.Entities.Context;
using Canaa.Infra.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.FN.BusinessComponents.Usuarios
{
    public class UsuariosBusiness : IUsuariosBusiness
    {
        private readonly ApplicationDbContext _context;

        public UsuariosBusiness(ApplicationDbContext context)
        {
            _context = context;
        }
   

        public async Task<Z_USUARIO> BuscarPorIdAsync(int id)
        {
            var usuario = await _context.Z_USUARIOs.FindAsync(id);
            return usuario;
        }

    

    }
}
