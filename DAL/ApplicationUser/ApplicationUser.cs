using DAL.Entities;
using Loyola_ERP.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        private readonly SchoolManagementContext _context;
        private readonly IHttpContextAccessor accessor;
        public ApplicationUser(SchoolManagementContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            this.accessor = accessor;
        }
        public ClaimsPrincipal GetUser()
        {
            return accessor?.HttpContext?.User;
        }

        public Guid SystemUserId
        {
            get
            {
                if (!string.IsNullOrEmpty(accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value + ""))
                {
                    return new Guid(accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }

                return Guid.Empty;
            }
        }

        public Usuarios UsuarioLogeado => throw new NotImplementedException();
    }
}
