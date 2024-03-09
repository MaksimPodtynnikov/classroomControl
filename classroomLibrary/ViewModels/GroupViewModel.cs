using classroomLibrary.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace classroomLibrary.ViewModels
{
	public class GroupViewModel
	{
        public int? id { set; get; }
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите название")]
        [MinLength(2, ErrorMessage = "Минимальная длина должна быть больше 2 символов")]
        public string title { set; get; }
        [Display(Name = "Направление")]
        [Required(ErrorMessage = "Введите направление")]
        [MinLength(2, ErrorMessage = "Минимальная длина должна быть больше 2 символов")]
        public string? direction { set; get; }
        [Display(Name = "Факультет")]
        [Required(ErrorMessage = "Введите факультет")]
        [MinLength(2, ErrorMessage = "Минимальная длина должна быть больше 2 символов")]
        public string? faculty { set; get; }
        [Display(Name = "Год начала")]
        [Required(ErrorMessage = "Введите год")]
        public DateTime yearStart { set; get; }
        [Display(Name = "Год окончания")]
        [Required(ErrorMessage = "Введите год")]
        public DateTime yearEnd { set; get; }
        [Display(Name = "Студенты")]
        public List<Student>? students { set; get; }
        [Display(Name = "Мероприятия")]
        public List<EventGroup>? eventGroups { set; get; }
        [Display(Name = "Университет")]
        [Required(ErrorMessage = "Выберите университет")]
        public int universityId { set; get; }

        public virtual University? university { set; get; }
        public IEnumerable<University>? AllUniversities { get; set; }
	}
}
