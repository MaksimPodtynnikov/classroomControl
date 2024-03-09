using classroomLibrary.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace classroomLibrary.ViewModels
{
	public class EventViewModel
	{
        public int? id { set; get; }
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите название")]
        [MinLength(2, ErrorMessage = "Минимальная длина должна быть больше 2 символов")]
        public string title { set; get; }
        [Display(Name = "Тип мероприятия")]
        [Required(ErrorMessage = "Введите тип мероприятия")]
        [MinLength(2, ErrorMessage = "Минимальная длина должна быть больше 2 символов")]
        public string? type { set; get; }
        [Display(Name = "Дата проведения")]
        [Required(ErrorMessage = "Введите дату проведения")]
        public DateTime date { get; set; }
        [Display(Name = "Номер пары")]
        [Required(ErrorMessage = "Введите номер пары")]
        public int lessonNumber { set; get; }
		[Display(Name = "Ответственный")]
		[Required(ErrorMessage = "Введите ответственного")]
		public int workerId { set; get; }

        public virtual Worker? worker { set; get; }
		[Display(Name = "Класс")]
		[Required(ErrorMessage = "Введите аудиторию")]
		public int classroomId { set; get; }

        public virtual Classroom? classroom { get; set; }
        [Display(Name = "Группы")]
        public IEnumerable<EventGroup>? eventGroups { set; get; }
		public IEnumerable<int>? eventGroupsId { set; get; }
		public string getLesson()
        {
            switch (lessonNumber)
            {
                case 1: return "8:30 - 10:05";
                case 2: return "10:15 - 11:50";
                case 3: return "12:10 - 13:45";
                case 4: return "13:55 - 15:30";
                case 5: return "15:50 - 17:25";
                case 6: return "17:35 - 19:10";
                default: return "не установлено";
            }
        }
        public IEnumerable<Worker>? AllWorkers { get; set; }
		public IEnumerable<Classroom>? AllClassrooms { get; set; }
		public IEnumerable<Group>? AllGroups { get; set; }
        public bool fromTable { get; set; }

	}
}
