using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ApplicationUser
{
    public class ApplicationUser : IApplicationUser
    {
        public ApplicationUser()
        {
            
        }
        public Guid SystemUserId => throw new NotImplementedException();

        public Usuarios UsuarioLogeado => throw new NotImplementedException();

        public ClaimsPrincipal GetUser()
        {
            throw new NotImplementedException();
        }
    }
}
