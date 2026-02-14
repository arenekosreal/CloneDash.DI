using System.Reflection;

using CloneDash;
using CloneDash.Configuration;
using CloneDash.Logging;
using CloneDash.Path;
using CloneDash.Sdl;

using CsToml.Extensions.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using PathLib;

string assemblyTitle = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>()!.Title;
string assemblyVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
string appId = Assembly.GetExecutingAssembly().GetCustomAttributes<AssemblyMetadataAttribute>().First(attr => attr.Key == "ApplicationId").Value!;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

IPathProvider pathProvider;
if (OperatingSystem.IsOSPlatform("Windows"))
    pathProvider = new WindowsPathProvider(appId);
else if (OperatingSystem.IsOSPlatform("macOS"))
    pathProvider = new MacOSPathProvider(appId);
else
    pathProvider = new XdgPathProvider(appId);

FileStream? writableConfigFileStream = null;
IPath configTomlFile = pathProvider.GetWritableConfigPath("appconfig.toml");
if (configTomlFile.Exists())
{
    writableConfigFileStream = configTomlFile.Open(FileMode.Open);
    builder.Configuration.AddTomlStream(writableConfigFileStream);
}
IPath configTomlEnvironmentalFile = pathProvider.GetWritableConfigPath($"appconfig.{builder.Environment.EnvironmentName}.toml");
if (configTomlEnvironmentalFile.Exists())
{
    writableConfigFileStream = configTomlEnvironmentalFile.Open(FileMode.Open);
    builder.Configuration.AddTomlStream(writableConfigFileStream);
}
if (writableConfigFileStream is null)
{
    configTomlFile.Parent().Mkdir(makeParents: true);
    writableConfigFileStream = configTomlFile.Open(FileMode.CreateNew);
}

builder.Logging.Services.AddPathProvider(pathProvider);
builder.Logging.AddFileLogger(builder.Configuration.GetRequiredSection(nameof(builder.Logging)));

builder.Services.AddLocalization();
builder.Services.BuildServiceProvider(
    new ServiceProviderOptions()
    {
        ValidateScopes = true,
        ValidateOnBuild = true,
    }
);
builder.Services.AddPathProvider(pathProvider);
builder.Services.AddConfigurationWriter(writableConfigFileStream);
builder.Services.AddSdl(builder.Configuration, assemblyTitle, assemblyVersion, appId);
builder.Services.AddMainScene();

using IHost host = builder.Build();

await host.StartAsync();

// [STAThread] attribute on Main
if (OperatingSystem.IsWindows())
    Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
ISdl sdl = host.Services.GetRequiredService<ISdl>();
if (sdl.CurrentRendering is null)
    sdl.CurrentRendering = host.Services.GetRequiredService<IMainScene>();
else
    host.Services.GetRequiredService<ILogger>()
        .LogWarning("SDL has been configured to render {0} before now.", sdl.CurrentRendering);
await host.Services.GetRequiredService<ISdl>().RunUntilQuitAsync();

await host.StopAsync();
await host.WaitForShutdownAsync();
