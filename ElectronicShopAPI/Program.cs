using ElectronicShopAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ElectronicShopAPI.Services;
using ElectronicShopAPI.Services.Classes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ElectronicShopAPI.Data.DataContext>(options =>
{
	options.UseSqlServer("Server=.;Database=electronic_shop;User Id=electronic_shop_app;Password=loginPassword123!;TrustServerCertificate=True;");
});
// Register application services
// Register service implementations from Services.Classes and interfaces from Services.Interfaces
builder.Services.AddScoped<IProductService, ElectronicShopAPI.Services.Classes.ProductService>();
builder.Services.AddScoped<ICustomerService, ElectronicShopAPI.Services.Classes.CustomerService>();
builder.Services.AddScoped<IOrderService, ElectronicShopAPI.Services.Classes.OrderService>();
builder.Services.AddScoped<IShoppingCartService, ElectronicShopAPI.Services.Classes.ShoppingCartService>();
builder.Services.AddScoped<IUserListService, ElectronicShopAPI.Services.Classes.UserListService>();
builder.Services.AddScoped<IUserListItemService, ElectronicShopAPI.Services.Classes.UserListItemService>();
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = jwtSettings["Issuer"],
		ValidAudience = jwtSettings["Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(key)
	};
});

builder.Services.AddCors(options =>
{
	options.AddPolicy("ReactAppPolicy", policy =>
	{
		policy.WithOrigins("http://localhost:5173") // Port Twojego Reacta (zmień jeśli masz inny)
			  .AllowAnyHeader()
			  .AllowAnyMethod();
	});
});
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo { Title = "Moje API", Version = "v1" });

	// 1. Definiujemy schemat bezpieczeństwa (jak Swagger ma pytać o token)
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Wprowadź token JWT w formacie: Bearer {twój_token}",
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		BearerFormat = "JWT",
		Scheme = "Bearer"
	});

	// 2. Wymagamy, aby Swagger stosował ten schemat globalnie do zablokowanych endpointów
	options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.AddAuthorization(options =>
{
	// Konfigurujemy polityki na podstawie nazw z enuma
	options.AddPolicy(Permission.Read.ToString(), policy =>
		policy.RequireClaim("Permission", Permission.Read.ToString(), Permission.AllPrivilages.ToString(), Permission.AllCustomers.ToString()));

	options.AddPolicy(Permission.Write.ToString(), policy =>
		policy.RequireClaim("Permission", Permission.Write.ToString(), Permission.AllPrivilages.ToString()));

	options.AddPolicy(Permission.Delete.ToString(), policy =>
		policy.RequireClaim("Permission", Permission.Delete.ToString(), Permission.AllPrivilages.ToString()));

	options.AddPolicy(Permission.ModifyProducts.ToString(), policy =>
		policy.RequireClaim("Permission", Permission.ModifyProducts.ToString(), Permission.AllPrivilages.ToString()));

	options.AddPolicy(Permission.AllCustomers.ToString(), policy =>
		policy.RequireClaim("Permission", Permission.AllCustomers.ToString(), Permission.AllPrivilages.ToString()));

	// Polityka dla samego boga-systemu (AllPrivilages)
	options.AddPolicy(Permission.AllPrivilages.ToString(), policy =>
		policy.RequireClaim("Permission", Permission.AllPrivilages.ToString()));
});
var app = builder.Build();
app.UseHttpsRedirection();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseCors("ReactAppPolicy");
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication(); // Kto to jest?
app.UseAuthorization();

app.MapControllers();

app.Run();
