namespace classroomLibrary.Data.Models
{
    public class EventGroup
    {
        public int id { set; get; }
        public int eventoId { set; get; }
        public int groupId { set; get; }
        public virtual Group group { set; get; }
        public virtual Event evento { set; get; }

    }
}
