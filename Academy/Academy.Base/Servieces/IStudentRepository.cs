using Academy.Base.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Base.Servieces
{
    // CRUD ( CREATE , Read , Update , Delete )
    public interface IStudentRepository
    {
        IEnumerable<Student> GetAll();
     //   DataTable GetAllDataTable();
        Student GetById(int id);
        int Create(Student student);
        bool Update(Student student);
        void Delete(int id);
    }
}
