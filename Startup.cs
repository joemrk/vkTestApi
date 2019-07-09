using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VkListener.Models;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkListener
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IVkApi>(sp => {
                var vk = new VkApi();
                vk.Authorize(new ApiAuthParams { AccessToken = Configuration["Vk:Token"] });
                return vk;
            });

            services.Configure<TelegramSettings>(Configuration.GetSection("Telega"));
            services.AddSingleton(r => r.GetRequiredService<IOptions<TelegramSettings>>().Value);



            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
