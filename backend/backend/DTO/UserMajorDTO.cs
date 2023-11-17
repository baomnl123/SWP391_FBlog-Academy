using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace backend.DTO
{
    public class UserMajorDTO
    {
        public UserDTO? User { get; set; }
        public MajorDTO? Major { get; set; }
        public bool Status { get; set; }
    }
}
