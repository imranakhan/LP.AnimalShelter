
using LP.AnimalShelter.API.Models;
using LP.AnimalShelter.API.Services;
using System.Text.Json.Serialization;

namespace LP.AnimalShelter.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Using a singleton instance of Shelter as a Data store to store changes on every run
            builder.Services.AddSingleton<Shelter, Shelter>(s =>
            {
                var numberOfLargeKernels = builder.Configuration.GetValue<int>("LargeKennels");
                var numberOfMediumKernels = builder.Configuration.GetValue<int>("MediumKennels");
                var numberOfSmallKernels = builder.Configuration.GetValue<int>("SmallKennels");
                return new Shelter(numberOfLargeKernels, numberOfMediumKernels, numberOfSmallKernels);
            });
            builder.Services.AddScoped<IShelterService, ShelterService>();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
