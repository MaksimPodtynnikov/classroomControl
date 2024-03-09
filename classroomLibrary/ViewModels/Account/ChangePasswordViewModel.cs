using System.ComponentModel.DataAnnotations;

namespace classroomLibrary.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Укажите логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [MinLength(5, ErrorMessage = "Пароль должен быть больше или равен 5 символов")]
        public string NewPassword { get; set; }
    }
}
