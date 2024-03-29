using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using DoStuff.DAL;
using DoStuff.Services.Facebook;
using DoStuff.Services.Users;
using DoStuff.Models.Settings;

namespace DoStuff.API
{
    public class Startup
    {
        private readonly JwtSettings _jwtSettings;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _jwtSettings = new JwtSettings();

            configuration.Bind("JsonWebTokenKeys", _jwtSettings);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TodoListContext>(options => options.UseSqlite(Configuration.GetConnectionString("DB")));
            services.AddControllers()
                .AddNewtonsoftJson(o => {
                    o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(Configuration.GetValue<string>("ClientUrl"));
                        builder.WithHeaders(HeaderNames.ContentType, HeaderNames.Authorization, HeaderNames.AccessControlAllowOrigin);
                        builder.AllowAnyMethod();
                    });
            });
            services.AddSingleton(_jwtSettings);
            services.AddHttpClient();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IFacebookAuthService, FacebookAuthService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
