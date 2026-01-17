using SchoolDrawingSystemMD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDrawingSystemMD.Services
{
    public interface IFileService
    {
        Task<AllSchoolClasses> LoadData();
        Task SaveData(AllSchoolClasses allSchoolClasses);
           
        Task UpdateSchoolClass(SchoolClass schoolClass);
        Task DeleteSchoolClass(Guid schoolClassId);

        Task UpdateStudent(Student student);
        Task DeleteStudent(Guid studentClassId);
    }
}
