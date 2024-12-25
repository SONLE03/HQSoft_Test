using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HQSoft_EX01.Models
{
    [Table("Books")]
    public class Books
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string Title { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        public int AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public Authors Author { get; set; }
        [Column(TypeName = "datetime2")]

        public DateTime PublishedDate { get; set; }
    }
}
