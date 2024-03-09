using Microsoft.EntityFrameworkCore;
using classroomLibrary.Data.Models;

namespace classroomLibrary.Data
{
    public class AppDBContent: DbContext
    {
        public AppDBContent(DbContextOptions<AppDBContent> options): base(options) {

        }
        public DbSet<Group> Groups { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<ClassEnvironment> ClassEnvironments { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enviroment> Enviroments { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventGroup> EventGroups { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<University> Universities { get; set;}
	}
}
