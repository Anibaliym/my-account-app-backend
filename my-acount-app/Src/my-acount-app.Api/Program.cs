using FluentValidation;
using MyAccountApp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using MyAccountApp.Application.Interfaces;
using MyAccountApp.Application.Services;
using System.Text.Json;
using MyAccountApp.Infrastructure.Context;
using MyAccountApp.Infrastructure.Repositories;
using MyAccountApp.Application.Validations.User;
using MyAccountApp.Application.ViewModels.User;
using MyAccountApp.Application.ViewModels.Account;
using MyAccountApp.Application.Validations.Account;
using MyAccountApp.Application.ViewModels.Sheet;
using MyAccountApp.Application.Validations.GenericValidation;
using MyAccountApp.Application.Validations.Card;
using MyAccountApp.Application.ViewModels.Card;
using MyAccountApp.Application.Validations.Vignette;
using MyAccountApp.Application.ViewModels.Vignette;
using MyAccountApp.Application.Validations.Sheet;
using MyAccountApp.Application.Validations.UserSecurity;
using MyAccountApp.Application.ViewModels.UserSecurity;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configurar la cadena de conexión a la base de datos.
string connectionString = builder.Configuration.GetConnectionString("my_account_appDb");

// Configurar DbContext
builder.Services.AddDbContext<my_account_appAppDbContext>(options => options.UseNpgsql(connectionString));

// Registrar los repositorios como servicios
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ISheetRepository, SheetRepository>();
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IVignetteRepository, VignetteRepository>();
builder.Services.AddScoped<IUserSecurityRepository, UserSecurityRepository>();

// Registrar servicios de aplicación
builder.Services.AddScoped<IUserAppService, UserAppService>();
builder.Services.AddScoped<IAccountAppService, AccountAppService>();
builder.Services.AddScoped<ISheetAppService, SheetAppService>();
builder.Services.AddScoped<ICardAppService, CardAppService>();
builder.Services.AddScoped<IVignetteAppService, VignetteAppService>();
builder.Services.AddScoped<IDomainServices, DomainServices>();

// Registrar AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Registrar FluentValidation
builder.Services.AddScoped<IValidator<UserCreateViewModel>, UserCreateValidator>();
builder.Services.AddScoped<IValidator<UserUpdateViewModel>, UserUpdateValidator>();
builder.Services.AddScoped<IValidator<CreateAccountViewModel>, AccountCreateValidator>();
builder.Services.AddScoped<IValidator<UpdateAccountViewModel>, AccountUpdateValidator>();
builder.Services.AddScoped<IValidator<CreateSheetViewModel>, SheetCrearValidator>();
builder.Services.AddScoped<IValidator<UpdateSheetViewModel>, SheetActualizarValidator>();
builder.Services.AddScoped<IValidator<CreateCardViewModel>, CardCreateValidator>();
builder.Services.AddScoped<IValidator<UpdateCardViewModel>, CardUpdateValidator>();
builder.Services.AddScoped<IValidator<VignetteCreateViewModel>, VignetteCreateValidator>();
builder.Services.AddScoped<IValidator<VignetteViewModel>, VignetteUpdateValidator>();
builder.Services.AddScoped<IValidator<UserSecurityCreateViewModel>, UserSecurityCreateValidator>();

builder.Services.AddScoped<IValidator<Guid>, IdValidator>();//validador generico



builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy.WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilitar CORS
app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();