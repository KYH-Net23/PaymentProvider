using Azure.Identity;
using PaymentProvider.Services;
using Stripe;

namespace PaymentProvider
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient();
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var vaultUri = new Uri($"{builder.Configuration["KeyVault"]!}");

            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddAzureKeyVault(vaultUri, new VisualStudioCredential());
            }
            else if (builder.Environment.IsProduction())
            {
                builder.Configuration.AddAzureKeyVault(vaultUri, new DefaultAzureCredential());
            }
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
            app.UseEndpoints(e => e.MapControllers());

            //app.MapControllers();

            app.Run();
        }
    }
}
