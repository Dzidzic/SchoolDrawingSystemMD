using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDrawingSystemMD.Models
{
    public class AllSchoolClasses
    {
        public ObservableCollection<SchoolClass> SchoolClasses { get; set; } = [];
    }
}
