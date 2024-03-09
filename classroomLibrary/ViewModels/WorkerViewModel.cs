using classroomLibrary.Data.Models;
using classroomLibrary.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace classroomLibrary.ViewModels
{
	public class WorkerViewModel
	{
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
        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Введите логин")]
        [MinLength(2, ErrorMessage = "Минимальная длина должна быть больше 2 символов")]
        public string login { get; set; }
        public string password { get; set; }
        [Display(Name = "Почта")]
        public string? mail { get; set; }
        [Display(Name = "Телефон")]
        public string? phone { set; get; }
        public string? oneC_id { set; get; }
        public int postId { set; get; }
        [Display(Name = "Должность")]
        public virtual Post? post { set; get; }
        [Display(Name = "Мероприятия")]
        public List<Event>? events { set; get; }
        [Display(Name = "Роль")]
        public Role role { set; get; }
        public IEnumerable<Post>? AllPosts { get; set; }
		public string getNameShort()
		{
			return family + " " + name.FirstOrDefault() + "." + patronymic.FirstOrDefault() + ".";
		}
	}
}
