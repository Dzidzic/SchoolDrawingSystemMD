using SchoolDrawingSystemMD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDrawingSystemMD.ViewModels
{
    public class SchoolClassViewModel
    {
        private readonly SchoolClass _model;
        public SchoolClass SchoolClassInfo => _model;

        SchoolClassViewModel(SchoolClass model)
        {
            _model = model;
        }
    }
}
