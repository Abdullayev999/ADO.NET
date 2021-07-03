using Academy.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Base.Servieces
{
    public interface IStudentClientRepository
    {
        Student GetById(int id);
        Student Registation(string Login, string Password);
        Student Login(string Login, string Password);
        AuthenticationUser Authentication(string login, string password, Func<string, string, Student> action);
        AuthenticationUser Authentication(Guid token);
        void LogOut(int id);
    }
}
