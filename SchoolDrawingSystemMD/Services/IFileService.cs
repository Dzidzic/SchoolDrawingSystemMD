using SchoolDrawingSystemMD.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDrawingSystemMD.Services
{
    public interface IFileService
    {
        Task<ObservableCollection<SchoolClass>> LoadData();
        Task SaveData(ObservableCollection<SchoolClass> allSchoolClasses);
    }
}
