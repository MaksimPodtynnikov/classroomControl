using classroomLibrary.Data.Models;

namespace classroomLibrary.Data.Interfaces
{
	public interface IClassEnvironments
	{
		IEnumerable<ClassEnvironment> ClassEnvironments { get; }
        Task Create(ClassEnvironment classEnviroment);
		ClassEnvironment Get(int id);
        IQueryable<ClassEnvironment> GetAll();
        Task<bool> Remove(ClassEnvironment classEnvironment);
		IEnumerable<ClassEnvironment> FindClassEnvironmentByClass(Classroom classroom);
		IEnumerable<ClassEnvironment> FindClassEnvironmentByEnvironment(Enviroment enviroment);
        Task<ClassEnvironment> Edit(ClassEnvironment classEnviroment);
	}
}
