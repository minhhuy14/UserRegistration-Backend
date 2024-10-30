using Microsoft.EntityFrameworkCore;
using UserRegistration_Backend.Data;

string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173",
                              "http://localhost:5173/signup").AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

builder.Services.AddDbContext<UserRegistration_BackendContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserRegistration_BackendContext") ?? throw new InvalidOperationException("Connection string 'UserRegistration_BackendContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
