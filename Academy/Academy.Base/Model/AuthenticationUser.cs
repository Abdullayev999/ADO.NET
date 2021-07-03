using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Base.Model
{
    public class AuthenticationUser
    {
        public Guid Token { get; set; }
        public Student student { get; set; } 
    }
}
