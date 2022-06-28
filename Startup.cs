using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using razorweb.IdentityServer4;
using razorweb.models;
using razorweb.Security.Requirements;

namespace razorweb
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

            services.AddOptions();
            var mailsetting = Configuration.GetSection("MailSettings");
            services.Configure<MailSettings>(mailsetting);
            services.AddSingleton<IEmailSender, SendMailService>();


            services.AddRazorPages();
            services.AddControllersWithViews();
            services.AddDbContext<MyBlogContext>(options => {
                string connectString = Configuration.GetConnectionString("MyBlogContext");
                options.UseSqlServer(connectString);
            });
            //Cấu hình cho IdentityServer4
            // var builder = services.AddIdentityServer(options =>
            //                        {
            //                            options.Events.RaiseErrorEvents       = true;
            //                            options.Events.RaiseInformationEvents = true;
            //                            options.Events.RaiseFailureEvents     = true;
            //                            options.Events.RaiseSuccessEvents     = true;
            //                        })
            //                       .AddInMemoryApiResources(Config.Apis)
            //                       .AddInMemoryClients(Config.Clients)
            //                       .AddInMemoryIdentityResources(Config.Ids)
            //                       .AddAspNetIdentity<AppUser>();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Knowledge Space API", Version = "v1" });
            });
            //kiểm tra requiredment cho identity
            // services.AddSingleton<IAuthenticationHandler, AppAuthorizationHandler>();
            // Dang ky Identity
            services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<MyBlogContext>()
                    .AddDefaultTokenProviders();
            // services.AddDefaultIdentity<AppUser>()
            //         .AddEntityFrameworkStores<MyBlogContext>()
            //         .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options => {
                // options.Cookie.HttpOnly = true;
                // options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.LoginPath        = "/dang-nhap/";
                options.LogoutPath       = $"/logout/";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });
            //Policy Authorization
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("AllowEditRole",
                    policyBuider =>
                    {
                        //Condition
                        policyBuider.RequireAuthenticatedUser();
                        // policyBuider.RequireRole("Admin");
                        //Claim
                        policyBuider.RequireClaim("blog", "get");
                        //Claims

                    });
                //Sử dụng dịch vụ để có thể quản lý điều kiện của policy
                // opt.AddPolicy("InGenZ",
                //     policyBuider =>
                //     {
                //         //Condition
                //         policyBuider.RequireAuthenticatedUser();
                //         policyBuider.Requirements.Add(new GenZRequirement());
                //     });
            });
            
            // Truy cập IdentityOptions
            services.Configure<IdentityOptions> (options => {
                // Thiết lập về Password
                options.Password.RequireDigit = false; // Không bắt phải có số
                options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
                options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
                options.Password.RequireUppercase = false; // Không bắt buộc chữ in
                options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
                options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

                // Cấu hình Lockout - khóa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (5); // Khóa 5 phút
                options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lần thì khóa
                options.Lockout.AllowedForNewUsers = true;

                // Cấu hình về User.
                options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;  // Email là duy nhất

                // Cấu hình đăng nhập.
                options.SignIn.RequireConfirmedEmail = false;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
                options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại


            });        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


        }
    }
}

/*
CREATE, READ, UPDATE, DELETE (CRUD)

dotnet aspnet-codegenerator razorpage -m razorweb.models.Article -dc razorweb.models.MyBlogContext -outDir Pages/Blog -udl --referenceScriptLibraries


Identity:
    - Athentication: Xác định danh tính  -> Login, Logout ...
    - Authorization: Xác thực quyền truy cập
    - Quản lý user: Sign Up, User, Role  ...

 * Role-based Authorization
 * Policy - based
 * Claims - based
    -> Đặc tính của một đối tượng
    

 /Identity/Account/Login
 /Identity/Account/Manage

 dotnet aspnet-codegenerator identity -dc razorweb.models.MyBlogContext
 

*/