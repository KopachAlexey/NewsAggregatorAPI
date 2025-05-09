using Microsoft.EntityFrameworkCore;
using NewsAggregatorData;
using NewsAggregatorServices.Abstracts;
using NewsAggregatorServices.Implementations;
using NewsAggregatorServices.Services;
using Hangfire;
using NewsAggregatorMapping.Mappers;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using FluentValidation;
using NewsAggregatorModels.Models;
using NewsAggregatorModels.Validation;

namespace NewsAggregatorAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("AFINN.json", false);

            builder.Services.AddDbContext<NewsAggregatorContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("Alternative"));
            });


            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secret = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

            var authBuilder = builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            authBuilder.AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                //opt.Events = new JwtBearerEvents
                //{
                //    OnAuthenticationFailed = context =>
                //    {
                //        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                //        {
                //            context.Response.Headers.Add("Token-Expired", "true");
                //        }
                //        return Task.CompletedTask;
                //    }
                //};

            });

            AddServices(builder);

            AddMappers(builder);

            AddValidators(builder);

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(NewsAggregatorCQS.Commands.AddNewsCommands).Assembly);
            });

            builder.Services.AddHangfire(cfg =>
            {
                cfg.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangFireAlternative"));
            });

            builder.Services.AddHangfireServer();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo { Title = "NewsAggregator", Version = "v1" });

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Default", policy =>
                {
                    policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("Default");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireDashboard();

            app.MapControllers();

            app.Run();
        }

        private static void AddMappers(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<SourceMapper>();
            builder.Services.AddTransient<NewsMapper>();
            builder.Services.AddTransient<RoleMapper>();
            builder.Services.AddTransient<UserMapper>();
            builder.Services.AddTransient<TokenMapper>();
            builder.Services.AddTransient<CommentMapper>();
            builder.Services.AddTransient<OperationMapper>();
            builder.Services.AddTransient<UserCommentReactionMapper>();
            builder.Services.AddTransient<ReactionMapper>();
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ISourceServices, SourceServices>();
            builder.Services.AddScoped<INewsServices, NewsServices>();
            builder.Services.AddScoped<IRssNewsReader, RssNewsReader>();
            builder.Services.AddScoped<IRoleServices, RoleServices>();
            builder.Services.AddHttpClient<SourceScrapper, BeltaScrapper>();
            builder.Services.AddHttpClient<SourceScrapper, TelegrafScrapper>();
            builder.Services.AddHttpClient<SourceScrapper, RiaScrapper>();
            builder.Services.AddTransient<ISourceScrapperFactory, SourceScrapperFactory>();
            builder.Services.AddScoped<INewsAggregator, NewsAggregator>();
            builder.Services.AddScoped<INewsPagginator, NewsPagginator>();
            builder.Services.AddScoped<ICronJobSetting, BeltaAggregationSetting>();
            builder.Services.AddScoped<ICronJobSetting, TelegrafAggregationSetting>();
            builder.Services.AddScoped<ICronJobSetting, RiaAggregationSetting>();
            builder.Services.AddScoped<ICronJobSetting, NewsRatingSetting>();
            builder.Services.AddScoped<ICronJobSetting, DelExpiredRefreshTokenSetting>();
            builder.Services.AddScoped<ICronJobSettingFactory, CronJobSettingFactory>();
            builder.Services.AddTransient<IHtmlRemover, HtmlRemover>();
            builder.Services.AddTransient<IWhiteSpaceRemover, WhiteSpaceRemover>();
            builder.Services.AddScoped<ITextlemmatizer, MyStemTextlemmatizer>();
            builder.Services.AddScoped<ILemmasRater, AFINNLemmasRater>();
            builder.Services.AddScoped<INewsRater, NewsRater>();
            builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<ICommentServices, CommentServices>();
            builder.Services.AddTransient<IPasswordHashing, PBKDF2PasswordHashing>();
            builder.Services.AddScoped<ITokenServices, JwtTokenServices>();
            builder.Services.AddScoped<IBackgroundJobServices, HangfireServices>();
            builder.Services.AddScoped<ICommentReactionServices, CommentReactionServices>();
        }

        private static void AddValidators(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IValidator<GetNewsPageRequest>, NewsPageValidator>();
            builder.Services.AddScoped<IValidator<AddCommentRequest>, CommentValidator>();
            builder.Services.AddScoped<IValidator<UpdateTokensRequest>, TokensValidator>();
            builder.Services.AddScoped<IValidator<AddUserRequest>, UserValidator>();
            builder.Services.AddScoped<IValidator<ReactionToCommentRequest>, ReactionToCommentRequestValidator>();
        }

    }
}
