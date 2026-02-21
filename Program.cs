using StudentDashboardAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title       = "Student Dashboard API",
        Version     = "v1",
        Description = "Sequential vs Parallel async/await demonstration"
    });
});

builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddScoped<IStudentService, StudentService>();

var app = builder.Build();

app.UseCors();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Dashboard API v1");
    c.RoutePrefix = "swagger";
});

app.UseAuthorization();
app.MapControllers();
app.Run();
