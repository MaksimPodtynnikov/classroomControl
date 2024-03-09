using classroomLibrary.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace classroomLibrary.ViewModels
{
    public class ClassViewModel
    {
        [Display(Name = "Картинка")]
        public IFormFile? image { set; get; }
        public int? id { set; get; }
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите название")]
        [MinLength(2, ErrorMessage = "Минимальная длина должна быть больше 2 символов")]
        public string title { set; get; }
        [Display(Name = "Вместительность")]
        [Required(ErrorMessage = "Введите вместительность")]
        public int capacity { set; get; }
        [Display(Name = "Фото")]
        public string? photo { set; get; }
        [Display(Name = "События")]
        public List<Event>? events { get; set; }
        [Display(Name = "Обеспечение")]
        public List<ClassEnvironment>? classEnvironments {get; set;}
        [Display(Name = "Отдел")]
        public Department? department { get; set; }
        public int departmentId { get; set; }
		public IEnumerable<Department>? AllDepartments { get; set; }
		public IEnumerable<Enviroment>? AllEnviroments { get; set; }
		public IEnumerable<Event>? AllEvents { get; set; }
		public List<ClassEnvironment>? enviroments { set; get; }
		[Display(Name = "Описание")]
		public string? description {  set; get; }
	}
}
