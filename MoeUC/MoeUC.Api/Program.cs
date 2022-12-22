using System.NetPro;
using MoeUC.Core.Infrastructure.Dependency;
using MoeUC.Core.Infrastructure.StartupConfigs;

var builder = WebApplication.CreateBuilder(args);

var typeFinder = new WebAppTypeFinder(new TypeFinderOption()
    {
        MountePath = ".\\"
    }, builder.Configuration,
    new NetProFileProvider(builder.Environment));

var dependencyRegistrars = typeFinder.FindClassesOfType<IDependencyRegistrar>(true);
var registrarInstances = dependencyRegistrars
    .Select(c => (IDependencyRegistrar)Activator.CreateInstance(c)!)
    .OrderBy(c => c.Order)
    .ToList();

foreach (var registrar in registrarInstances)
{
    registrar.Register(builder.Services, typeFinder, builder.Configuration);
}

// Run service startup configs
var startupConfigs = typeFinder.FindClassesOfType<IMoeStartup>(true);
var configInstances = startupConfigs
    .Select(c => (IMoeStartup)Activator.CreateInstance(c)!)
    .OrderBy(c => c.Order)
    .ToList();

// configure services
foreach (var item in configInstances)
{
    item.ConfigureServices(builder.Services, builder.Configuration);
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

ApplicationContext.Init(app.Services);

// Configure StartUps
foreach (var item in configInstances)
{
    item.Configure(app);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
