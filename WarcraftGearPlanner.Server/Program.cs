﻿using AutoMapper;
using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WarcraftGearPlanner.Server.Data;
using WarcraftGearPlanner.Server.Data.Entities;
using WarcraftGearPlanner.Server.Data.Repositories;
using WarcraftGearPlanner.Server.Services.InventoryTypes;
using WarcraftGearPlanner.Server.Services.ItemClasses;
using WarcraftGearPlanner.Server.Services.ItemQualities;
using WarcraftGearPlanner.Server.Services.Items;
using WarcraftGearPlanner.Server.Services.Realms;
using WarcraftGearPlanner.Shared.Models.Items;
using WarcraftGearPlanner.Shared.Models.Realms;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(contextOptions =>
	contextOptions.UseSqlServer(
		builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"),
		sqlOptions => sqlOptions.EnableRetryOnFailure(3))
);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddOpenTelemetry().UseAzureMonitor(monitorOptions =>
{
	monitorOptions.SamplingRatio = 0.75F;
	monitorOptions.ConnectionString = builder.Configuration.GetSection("AzureMonitor")["ConnectionString"];
	monitorOptions.Credential = builder.Environment.IsDevelopment()
		? new DefaultAzureCredential()
		: new ManagedIdentityCredential();
});

builder.Services.AddScoped<IInventoryTypeService, InventoryTypeService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IItemClassService, ItemClassService>();
builder.Services.AddScoped<IItemQualityService, ItemQualityService>();
builder.Services.AddScoped<IRealmService, RealmService>();

builder.Services.AddScoped<IValidator<InventoryType>, InventoryTypeValidator>();
builder.Services.AddScoped<IValidator<Item>, ItemValidator>();
builder.Services.AddScoped<IValidator<ItemClass>, ItemClassValidator>();
builder.Services.AddScoped<IValidator<ItemQuality>, ItemQualityValidator>();
builder.Services.AddScoped<IValidator<Realm>, RealmValidator>();

builder.Services.AddScoped<IRepository<ItemEntity>, ItemRepository>();
builder.Services.AddScoped<IRepository<ItemQualityEntity>, ItemQualityRepository>();
builder.Services.AddScoped<IRepository<InventoryTypeEntity>, InventoryTypeRepository>();
builder.Services.AddScoped<IRepository<ItemClassEntity>, ItemClassRepository>();
builder.Services.AddScoped<IRepository<ItemSubclassEntity>, ItemSubclassRepository>();
builder.Services.AddScoped<IRepository<RealmEntity>, RealmRepository>();

builder.Services
	.AddControllers(options =>
	{
		options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
	})
	.AddJsonOptions(options =>
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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.Services.GetRequiredService<IMapper>().ConfigurationProvider.AssertConfigurationIsValid();
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
