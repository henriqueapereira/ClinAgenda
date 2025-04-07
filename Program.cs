using ClinAgenda.src.Application.UseCases;
using ClinAgenda.src.Core.Interfaces;
using ClinAgenda.src.Infrastructure.Repositories;
using ClinAgendaAPI.StatusUseCase;
using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

// Configuração da conexão com MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<MySqlConnection>(_ => new MySqlConnection(connectionString));

builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<StatusUseCase>();

builder.Services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
builder.Services.AddScoped<SpecialtyUseCase>();

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<PatientUseCase>();

builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<DoctorUseCase>();

builder.Services.AddScoped<IDoctorSpecialtyRepository, DoctorSpecialtyRepository>();

builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<AppointmentUseCase>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

