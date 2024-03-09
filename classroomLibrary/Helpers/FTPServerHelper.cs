using NuGet.ProjectModel;
using System.Net;

namespace classroomLibrary.Helpers
{
    public static class FTPServerHelper
    {
        public static bool upload(string filename,string server, string username, string password,string title)
        {
			WebClient client = new WebClient();
			client.Credentials = new NetworkCredential(username, password);
			var url = $"ftp://{server}/www/vepi.ru/wp-content/uploads/page/sveden/objects/vrn/dannie/kabinet/2019/{title}.csv";
			client.UploadFile(new Uri(url), filename);
			return true;
		}
    }
}
