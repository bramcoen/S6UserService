using DataInterface;
using MongoDBRepository;
using UserService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options => {
    options.AddPolicy("default", build => build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader());
});
builder.Services.AddControllers(); 
builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddSingleton<RabbitMQService>();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
