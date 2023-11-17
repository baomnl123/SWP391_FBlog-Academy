using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace backend.DTO
{
    public class UserSubjectDTO
    {
        public UserDTO? User { get; set; }
        public SubjectDTO? Subject { get; set; }
        public bool Status { get; set; }
    }
}
