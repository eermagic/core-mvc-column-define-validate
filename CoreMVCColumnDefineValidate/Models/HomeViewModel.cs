using CoreMVCColumnDefineValidate.ProjectClass;
using System.ComponentModel.DataAnnotations;

namespace CoreMVCColumnDefineValidate.Models
{
    public class HomeViewModel
    {
        public class Student : ModelBase
        {
            public string? StudentID { get; set; }

            public string? PID { get; set; }

            public string? Name { get; set; }

            public string? Marks { get; set; }

            public string? Email { get; set; }

            public string? Mobile { get; set; }

            public string? CreateDate { get; set; }
        }

        public class AddSaveOut
        {
            public string? ErrMsg { get; set; }
            public string? ResultMsg { get; set; }
        }
    }
}
