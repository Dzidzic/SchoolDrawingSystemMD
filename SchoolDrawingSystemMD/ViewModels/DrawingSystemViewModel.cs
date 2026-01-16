using SchoolDrawingSystemMD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDrawingSystemMD.ViewModels
{
    public class DrawingSystemViewModel
    {
        private readonly DrawingSystem _model;
        public DrawingSystem DrawingSystemInfo => _model;
        DrawingSystemViewModel(DrawingSystem model)
        {
            _model = model;
        }
    }
}
