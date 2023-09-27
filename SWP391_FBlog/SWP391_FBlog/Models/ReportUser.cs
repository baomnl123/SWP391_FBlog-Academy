using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_FBlog.Models
{
    [Table("report_user")]
    public class ReportUser
    {
        [Key]
        [Column("reporter_id")]
        [Required]
        public int ReporterId { get; set; }
        
        [Key]
        [Column("reported_id")]
        [Required]
        public int ReportedId { get; set; }

        [Column("admin_id")]
        [Required]
        public string Name { get; set; }

        [Column("status")]
        [Required]
        public bool Status{ get; set; }

        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; }

        // Relation
        public ICollection<ReportStatus> ReportStatus { get; set; }
    }
}
