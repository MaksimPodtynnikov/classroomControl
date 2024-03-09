using Microsoft.EntityFrameworkCore;
using classroomLibrary.Data.Interfaces;
using classroomLibrary.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace classroomLibrary.Data.Repository
{
    public class ClassroomRepository:IClassrooms
    {
        private readonly AppDBContent appDBContent;
        public ClassroomRepository(AppDBContent appDBContent)
        {
            this.appDBContent = appDBContent;
        }

        public IEnumerable<Classroom> Classrooms => appDBContent.Classrooms.Include(c=>c.department).Include(c=>c.enviroments);

		public async Task Create(Classroom classroom)
		{
            await appDBContent.Classrooms.AddAsync(classroom);
            await appDBContent.SaveChangesAsync();
        }

		public async Task Remove(Classroom classroom)
		{
            appDBContent.Classrooms.Remove(classroom);
            await appDBContent.SaveChangesAsync();
        }

		public async Task<Classroom> Edit(Classroom classroom)
		{
            appDBContent.Classrooms.Update(classroom);
            await appDBContent.SaveChangesAsync();

            return classroom;
        }

		public IEnumerable<Classroom> FindClassroomsByCity(City city)=>appDBContent.Classrooms.Where(p=>p.department.city==city).AsQueryable().Include(t=>t.department);
        public IQueryable<Classroom> GetAll() => appDBContent.Classrooms.Include(c => c.department).Include(c => c.enviroments);

        public Classroom Get(int id)=> appDBContent.Classrooms.Include(c => c.department).Include(c=>c.enviroments).ThenInclude(c=>c.enviroment).Include(c=>c.events).FirstOrDefault(p => p.id == id);
        
    }
}
