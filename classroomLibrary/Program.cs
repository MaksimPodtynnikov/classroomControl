using classroomLibrary.Data.Models;
using classroomLibrary.Data;
using classroomLibrary.Data.Interfaces;
using classroomLibrary.Data.Repository;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using classroomLibrary.Services.Interfaces;
using classroomLibrary.Services.Implementations;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
string _confString = builder.Configuration.GetConnectionString("Umo1Connection");
builder.Services.AddDbContext<AppDBContent>(op=>op.UseSqlServer(_confString));
builder.Services.AddTransient<ICities, CityRepository>();
builder.Services.AddTransient<IClassEnvironments, ClassEnviromentRepository>();
builder.Services.AddTransient<IClassrooms, ClassroomRepository>();
builder.Services.AddTransient<IDepartments, DepartmentRepository>();
builder.Services.AddTransient<IEnvironments, EnviromentRepository>();
builder.Services.AddTransient<IEventGroups, EventGroupRepository>();
builder.Services.AddTransient<IEvents, EventRepository>();
builder.Services.AddTransient<IGroups, GroupRepository>();
builder.Services.AddTransient<IPosts, PostRepository>();
builder.Services.AddTransient<IStudents, StudentRepository>();
builder.Services.AddTransient<IWorkers, WorkerRepository>();
builder.Services.AddTransient<IUniversities, UniversityRepository>();
builder.Services.AddScoped<ICitiesService, CitiesService>();
builder.Services.AddScoped<IClassEnviromentsService, ClassEnvironmentsService>();
builder.Services.AddScoped<IClassroomsService, ClassroomsService>();
builder.Services.AddScoped<IDepartmentsService, DepartmentsService>();
builder.Services.AddScoped<IEnvironmentsService, EnvironmentsService>();
builder.Services.AddScoped<IEventGroupsService, EventGroupsService>();
builder.Services.AddScoped<IEventsService, EventsService>();
builder.Services.AddScoped<IGroupsService, GroupsService>();
builder.Services.AddScoped<IPostsService, PostsService>();
builder.Services.AddScoped<IStudentsService, StudentsService>();
builder.Services.AddScoped<IWorkersService, WorkersService>();
builder.Services.AddMvc();
builder.Services.AddControllers(options => options.EnableEndpointRouting = false);
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
        options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
    });
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	AppDBContent content = scope.ServiceProvider.GetService<AppDBContent>();
	DBObjects.Initial(content);
}
app.UseDeveloperExceptionPage();
app.UseStatusCodePages();
app.UseStaticFiles();
app.UseMvcWithDefaultRoute();
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.Run();
//WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().Build().Run();

