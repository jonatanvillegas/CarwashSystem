using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ApplicationUser
{
    public interface IApplicationUser
    {
        ClaimsPrincipal GetUser();

        Guid SystemUserId { get; }

        Usuarios UsuarioLogeado { get; }
    }
}
