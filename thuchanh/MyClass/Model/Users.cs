using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UsreName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Img { get; set; }
        [Required]
        public string Gender { get; set; }
        public string Role { get; set; }
        
        public string Address { get; set; }
    
        [Required]
        public int? CreateBy { get; set; }
        [Required]
        public DateTime? CreateAt { get; set; }
        [Required]
        public int? UpdateBy { get; set; }
        [Required]
        public DateTime? UpdateAt { get; set; }
        [Required]
        public int? Status { get; set; }




    }
}
