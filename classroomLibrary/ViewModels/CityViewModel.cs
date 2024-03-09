using classroomLibrary.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace classroomLibrary.ViewModels
{
	public class CityViewModel
	{
        public long? Id { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите название")]
        [MinLength(2, ErrorMessage = "Минимальная длина должна быть больше 2 символов")]
        public string title { get; set; }

        [Display(Name = "Синоним")]
        [MinLength(2, ErrorMessage = "Минимальная длина должна быть больше 2 символов")]
        public string city { get; set; }
		public List<Department>? departments { set; get; }
	}
}
