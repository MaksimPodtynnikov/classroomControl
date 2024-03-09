using classroomLibrary.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace classroomLibrary.ViewModels
{
	public class UniversityViewModel
	{
        public int? id { get; set; }
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите название")]
        [MinLength(2, ErrorMessage = "Минимальная длина должна быть больше 2 символов")]
        public string title { get; set; }
        [Display(Name = "Группы")]
        public List<Group>? groups { set; get; }
    }
}
