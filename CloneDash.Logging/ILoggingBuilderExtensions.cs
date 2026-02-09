using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CloneDash.Logging;

/// <summary>Extended methods for <see cref="ILoggingBuilder" />.</summary>
public static class ILoggingBuilderExtensions
{
    /// <summary>Add <see cref="FileLoggerProvider" /> to <see cref="ILoggingBuilder" />.</summary>
    public static ILoggingBuilder AddFileLogger(this ILoggingBuilder builder, IConfigurationSection loggingSection)
    {
        IConfigurationSection logLevelSection = loggingSection.GetRequiredSection("LogLevel");
        builder.Services.AddOptions<Dictionary<string, LogLevel>>()
                        .Configure(logLevelSection.Bind);
        string providerAlias = typeof(FileLoggerProvider).GetCustomAttribute<ProviderAliasAttribute>()?.Alias ??
                               typeof(FileLoggerProvider).FullName!;
        IConfigurationSection fileSection = loggingSection.GetSection(providerAlias);
        builder.Services.AddOptions<FileLoggerConfiguration>()
                        .Configure(fileSection.Bind);
        builder.Services.AddSingleton<ILoggerProvider, FileLoggerProvider>();
        return builder;
    }
}
