
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using yu_geo_api.Repository;
using yu_geo_api.Services;

namespace yu_geo_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*Captura a chave de criptografia das configurações*/
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings").GetSection("SecreteKey").Value);


            var connection = Configuration["AppSettings:ConnectionString"];
            services.AddDbContext<Contexto>(options =>
                options.UseSqlite(connection)
            );

            services.AddCors();
            /* Adiciona as politicas globais para a aplicação*/
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));

            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            /* Adiciona as configurações para a classe com mesmo nome (Para ser utilizado mais facilmente em tempo de execução)*/
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            /* Injeção de dependencias */
            services.AddTransient<UserServices>();
            services.AddTransient<AuthServices>();
            services.AddTransient<UserRepository>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();

            /* Configura o esquema de autenticação como JWT */
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            /* Configura os perfis de autorização da aplicação*/
            services.AddAuthorization(z =>
            {
                z.AddPolicy("user", policy => policy.RequireClaim("Store", "user"));
                z.AddPolicy("admin", policy => policy.RequireClaim("Store", "admin"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            /* Configura para que a aplicação utilize os Cors */
            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
            /* Configura a aplicação para ele utilizar a autenticação */
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
