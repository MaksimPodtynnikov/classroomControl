using classroomLibrary.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace classroomLibrary.ViewModels
{
	public class DepartmentViewModel
	{
        public int? id { set; get; }
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите название")]
        [MinLength(2, ErrorMessage = "Минимальная длина должна быть больше 2 символов")]
        public string title { set; get; }
        public int cityId { set; get; }
        [Display(Name = "Город")]
        public virtual City? city { set; get; }
        [Display(Name = "Должности")]
        public List<Post>? posts { set; get; }
        [Display(Name = "Классы")]
        public List<Classroom>? classrooms { set; get; }
        public IEnumerable<City>? AllCities { get; set; }
	}
}
