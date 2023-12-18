using Amazon.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EscaladaApi.Persistence;
using EscaladaBot.Contracts;
using EscaladaBot.HostedServices;
using EscaladaBot.Persistence;
using EscaladaBot.Services;
using EscaladaBot.Services.BotCommands;
using EscaladaBot.Services.Contexts;
using EscaladaBot.Services.Handlers;
using EscaladaBot.Services.Helpers;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IConnectionFactory>(s =>
    new PostgresConnectionFactory(SecretsHelper.PostgreConnectionString));

builder.Services.AddSingleton<IRepositoryFactory, RepositoryFactory>();

builder.Services.AddSingleton(new BasicAWSCredentials(
    SecretsHelper.GetS3AccessKey, SecretsHelper.GetS3SecretAccessKey));

builder.Services.AddSingleton<ITelegramConnectionFactory>(s =>
    new TelegramConnectionFactory(SecretsHelper.GetTelegramApiToken));

builder.Services.AddSingleton<ITelegramBotHandler, TelegramBotHandler>();
builder.Services.Decorate<ITelegramBotHandler, TelegramBotCallBackHandler>();
builder.Services.Decorate<ITelegramBotHandler, TelegramBotSubscribeHandler>();
builder.Services.Decorate<ITelegramBotHandler, TelegramBotVoiceCallBackHandler>();

builder.Services.AddSingleton<IContext, CreationContext>();

builder.Services.AddSingleton<ICommandBuilder, CommandBuilder>();

builder.Services.AddSingleton<IRegisterRepository, RegisterRepository>();
builder.Services.AddSingleton<IVoiceRepository, VoiceRepository>();
builder.Services.AddSingleton<IProblemRepository, ProblemRepository>();
builder.Services.AddSingleton<ISubscribeRepository, SubscribeRepository>();
builder.Services.AddSingleton<IAdminRepository, AdminRepository>();
builder.Services.AddSingleton<IStatisticsRepository, StatisticsRepository>();

builder.Services.AddTransient<IFileStore, FileStore>();

builder.Services.AddSingleton<IProblemCreatorStateStore, ProblemCreatorStateStore>();
builder.Services.AddSingleton<IContextSwitcher, ContextSwitcher>();

builder.Services.AddTransient<IProblemViewer, ProblemViewer>();

builder.Services.AddTransient<CreateCommand>();
builder.Services.AddTransient<AddImagesCommand>();
builder.Services.AddTransient<AddImagesStartCommand>();
builder.Services.AddTransient<EndCommand>();
builder.Services.AddTransient<GetDatesCommand>();
builder.Services.AddTransient<UnknownCommand>();
builder.Services.AddTransient<ViewProblemCommand>();
builder.Services.AddTransient<WelcomeCommand>();
builder.Services.AddTransient<AddVoiceCommand>();
builder.Services.AddTransient<ViewStatisticsCommand>();

builder.Services.AddTransient<CreationContext>();
builder.Services.AddTransient<RegisterContext>();
builder.Services.AddTransient<UnknownContext>();
builder.Services.AddTransient<ViewProblemContext>();
builder.Services.AddTransient<WelcomeContext>();
builder.Services.AddTransient<VoiceContext>();
builder.Services.AddTransient<ViewStatisticsContext>();

builder.Services.AddHostedService<TelegramHostedService>();

builder.Services.AddLogging(configure => configure.AddSerilog());

using var host = builder.Build();

var factory = host.Services.GetService<IRepositoryFactory>();
await factory?.Build()!;

await host.RunAsync();