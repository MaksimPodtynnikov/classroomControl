using classroomLibrary.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace classroomLibrary.ViewModels
{
	public class EnvironmentViewModel
	{
        public int? id { set; get; }
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите название")]
        [MinLength(2, ErrorMessage = "Минимальная длина должна быть больше 2 символов")]
        public string title { set; get; }
        [Display(Name = "Описание")]
        [MinLength(2, ErrorMessage = "Минимальная длина должна быть больше 2 символов")]
        public string? description { set; get; }
        [Display(Name = "Классы")]
        public List<ClassEnvironment>? classes { set; get; }
    }
}
