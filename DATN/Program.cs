using DATN.Models;
using DATN.Services.CategoryService;
using DATN.Services.DeviceService;
using DATN.Services.Email;
using DATN.Services.ItemService;
using DATN.Services.RoleService;
using DATN.Services.UserService;
using DATN.Services.Warehouse;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<DeviceContext>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IITemService, ItemServicecs>();

var myOrigins = "_myOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            policy.WithOrigins("http://192.168.0.110:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            //policy.WithOrigins(" https://7368-2405-4803-c85d-4790-185a-9278-94a4-ca30.ngrok-free.app").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            //policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors(myOrigins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
