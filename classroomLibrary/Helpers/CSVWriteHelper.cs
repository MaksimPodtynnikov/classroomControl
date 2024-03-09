using classroomLibrary.Data.Models;

namespace classroomLibrary.Helpers
{
	public static class CSVWriteHelper
    {
		public static async Task<bool> CreateCSVFile(List<Classroom> classrooms, string strFilePath)
		{
			try
			{
				// Create the CSV file to which grid data will be exported.    
				StreamWriter sw = new StreamWriter(strFilePath, false,System.Text.Encoding.UTF8); 
				foreach (Classroom classroom in classrooms)
				{
					await sw.WriteAsync(classroom.department.city.title);
					await sw.WriteAsync("`");
					await sw.WriteAsync(classroom.title+ classroom.description!=null? " «"+classroom.description+ "»":"");
					await sw.WriteAsync("`");
					await sw.WriteAsync(classroom.getAllEnvironment());
					await sw.WriteAsync(sw.NewLine);
				}
				sw.Close();
				return true;
			}
			catch (Exception ex) { return false; }
		}
	}
}
