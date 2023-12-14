using System.Text.Json.Serialization;
using WarcraftGearPlanner.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IBattleNetService, BattleNetService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
	options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
});
builder.Services.AddCors(options =>
{
	options.AddPolicy("CorsPolicy", policy =>
	{
		policy.WithOrigins("http://localhost:4200", "https://mturchik.github.io")
			.AllowAnyMethod()
			.AllowAnyHeader();
	});
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

app.UseCors("CorsPolicy");
app.Use(async (context, next) =>
{
	var version = typeof(Program).Assembly.GetName().Version?.ToString() ?? "ERROR";
	context.Response.Headers.Append("x-api-version", version);
	await next();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
