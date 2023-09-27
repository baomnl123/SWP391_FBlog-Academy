using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_FBlog.Models
{
    [Table("report_status")]
    public class ReportStatus
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("name")]
        [Required]
        public string Name { get; set; }

        [Column("description")]
        [Required]
        public string Description { get; set; }

        // Relation
        public ReportUser ReportUser { get; set; }
    }
}
