using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDrawingSystemMD.Models
{
    public partial class Student : ObservableObject
    {
        public Guid Id { get; set; }

        [ObservableProperty]
        private int _studentNumber;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        [ObservableProperty]
        private bool _isPresent;

        [ObservableProperty]
        private int _drawCooldown;

    }
}
