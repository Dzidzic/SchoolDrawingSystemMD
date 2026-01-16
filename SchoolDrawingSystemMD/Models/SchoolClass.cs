using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDrawingSystemMD.Models
{
    public class SchoolClass
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Student> Students { get; set; } = [];
    }
}
