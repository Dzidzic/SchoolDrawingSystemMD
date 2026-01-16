using SchoolDrawingSystemMD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDrawingSystemMD.ViewModels
{
    public class StudentViewModel
    {
        private readonly Student _model;
        public Student StudentInfo => _model;

        StudentViewModel(Student model)
        {
            _model = model;
        }
    }
}
