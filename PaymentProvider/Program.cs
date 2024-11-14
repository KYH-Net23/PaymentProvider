using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using PaymentProvider.Contexts;
using PaymentProvider.Repositories;
using PaymentProvider.Services;
using Stripe;

namespace PaymentProvider
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Key Vault
            var vaultUri = new Uri($"{builder.Configuration["KeyVault"]!}");

            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddAzureKeyVault(vaultUri, new VisualStudioCredential());
            }
            else if (builder.Environment.IsProduction())
            {
                builder.Configuration.AddAzureKeyVault(vaultUri, new DefaultAzureCredential());
            }

            builder.Services.AddHttpClient();
            builder.Services.AddDbContext<EmailSessionDbContext>(options => options.UseSqlServer(builder.Configuration["SessionDbKey"]));
            builder.Services.AddScoped<EmailSessionRepository>();
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddScoped<EmailService>(sp =>
            {
                var repo = sp.GetRequiredService<EmailSessionRepository>();
                var connectionString = builder.Configuration["EmailSecret"];
                return new EmailService(repo, connectionString!);
            });
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            StripeConfiguration.ApiKey = builder.Configuration["StripeSecret"];

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
