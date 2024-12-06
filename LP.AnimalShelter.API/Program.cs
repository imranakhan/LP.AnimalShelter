
using LP.AnimalShelter.API.Interface;
using LP.AnimalShelter.API.Models;

namespace LP.AnimalShelter.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<IShelter, Shelter>(s =>
            {
                var numberOfLargeKernels = builder.Configuration.GetValue<int>("LargeKernels");
                var numberOfMediumKernels = builder.Configuration.GetValue<int>("MediumKernels");
                var numberOfSmallKernels = builder.Configuration.GetValue<int>("SmallKernels");
                return new Shelter(numberOfLargeKernels, numberOfMediumKernels, numberOfSmallKernels);
            });

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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
