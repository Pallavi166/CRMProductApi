using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    
        public interface IJwtService
        {
            string GenerateToken(string username, string role);

 
            string GenerateRefreshToken();
        }


    }
    

