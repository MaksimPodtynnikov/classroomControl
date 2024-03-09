using classroomLibrary.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace classroomLibrary.ViewModels
{
	public class PostViewModel
	{
        public int? id { set; get; }
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите название")]
        [MinLength(2, ErrorMessage = "Минимальная длина должна быть больше 2 символов")]
        public string title { set; get; }
        public int departmentId { set; get; }
        [Display(Name = "Ответственный отдел")]
        public Department? department { set; get; }
        [Display(Name = "Сотрудники")]
        public List<Worker>? workers { set; get; }
        public IEnumerable<Department>? AllDepartments { get; set; }
	}
}
