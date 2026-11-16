using System;
using System.ComponentModel.DataAnnotations;

namespace template.Models
{
  
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
      
    }


    public class UserMetadata
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản!")]
        [StringLength(50, ErrorMessage = "Tên tài khoản không được vượt quá 50 ký tự.")]
        [RegularExpression(@"^\S+$", ErrorMessage = "Tên tài khoản không được chứa khoảng trắng!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ Email!")]
        [EmailAddress(ErrorMessage = "Định dạng Email không hợp lệ!")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        [RegularExpression(@"^\S+$", ErrorMessage = "Mật khẩu không được chứa khoảng trắng!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn vai trò người dùng!")]
        public string Role { get; set; }

        [StringLength(255, ErrorMessage = "Địa chỉ tối đa 255 ký tự.")]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [StringLength(20, ErrorMessage = "Số điện thoại tối đa 20 ký tự.")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        public Nullable<System.DateTime> CreatedAt { get; set; }
    }
}