using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Suppliers")]
    public class Suppliers
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên NCC không được để trống")]
        [Display(Name = "Tên NCC")]

        public string Name { get; set; }
        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }
        [Display(Name = "Link rút gọn")]
        public string Slug { get; set; }

        [Display(Name = "sắp xếp")]

        public int? Order{ get; set; }
        [Display(Name = "Tên đầy đủ")]
        public string FullName { get; set; }
        [Display(Name = "số điện thoại")]
        public string Phone { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mô tả loại sản phẩm không được để trống")]
        [Display(Name = "Mô tả")]
        public string MetaDesc { get; set; }
        [Required(ErrorMessage = "Từ khóa loại sản phẩm không được để trống")]
        [Display(Name = "Từ khóa")]
        public string MetaKey { get; set; }
        [Required(ErrorMessage = "Người tạo loại sản phẩm không được để trống")]
        [Display(Name = "Người tạo")]
        public DateTime CreateAt { get; set; }
        [Required(ErrorMessage = "Ngày tạo loại sản phẩm không được để trống")]
        [Display(Name = "Ngày tạo")]
        public int CreateBy { get; set; }
        [Required(ErrorMessage = "Người cập nhật không được để trống")]
        [Display(Name = "Người cập nhật")]
        public int UpdateBy { get; set; }
        [Required(ErrorMessage = "Ngày cập nhật không được để trống")]
        [Display(Name = "Ngày cập nhật")]
        public DateTime UpdateAt { get; set; }

        [Display(Name = "Trạng thái")]
        public int? Status { get; set; }




    }
}
