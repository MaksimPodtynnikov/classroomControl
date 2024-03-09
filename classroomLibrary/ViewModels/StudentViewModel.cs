using classroomLibrary.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace classroomLibrary.ViewModels
{
	public class StudentViewModel
	{
        public int? telegram_id { set; get; }
        public int groupId { set; get; }
        public int? id { set; get; }
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Введите имя")]
        [MinLength(2, ErrorMessage = "Минимальная длина должна быть больше 2 символов")]
        public string name { set; get; }
        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Введите фамилию")]
        [MinLength(2, ErrorMessage = "Минимальная длина должна быть больше 2 символов")]
        public string family { set; get; }
        [Display(Name = "Отчество")]
        [Required(ErrorMessage = "Введите отчество")]
        [MinLength(2, ErrorMessage = "Минимальная длина должна быть больше 2 символов")]
        public string? patronymic { set; get; }
        [Display(Name = "Группа")]
        public virtual Group? group { set; get; }
        public IEnumerable<Group>? AllGroups { get; set; }
		public string getNameShort()
		{
			return family + " " + name.FirstOrDefault() + "." + patronymic.FirstOrDefault() + ".";
		}
	}
}
