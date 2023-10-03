﻿using Amazon.Runtime;
using EscaladaApi.Contracts;
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

builder.Services.AddSingleton<ISQLiteConnectionFactory>(s =>
    new SQLiteConnectionFactory("Data Source=escalabot.db"));

builder.Services.AddSingleton(new BasicAWSCredentials(
    SecretsHelper.GetS3AccessKey, SecretsHelper.GetS3SecretAccessKey));

builder.Services.AddSingleton<ITelegramConnectionFactory>(s =>
    new TelegramConnectionFactory(SecretsHelper.GetTelegramApiToken));

builder.Services.AddSingleton<ITelegramBotHandler, TelegramBotHandler>();
builder.Services.Decorate<ITelegramBotHandler, TelegramBotCallBackHandler>();
builder.Services.Decorate<ITelegramBotHandler, TelegramBotSubscribeHandler>();

builder.Services.AddSingleton<IContext, CreationContext>();
builder.Services.AddSingleton<ITraceInfoViewer, TraceInfoViewer>();

builder.Services.AddSingleton<ICommandBuilder, CommandBuilder>();

builder.Services.AddSingleton<IRegisterRepository, RegisterRepository>();
builder.Services.AddSingleton<IProblemRepository, ProblemRepository>();
builder.Services.AddSingleton<ISubscribeRepository, SubscribeRepository>();
builder.Services.AddSingleton<IAdminRepository, AdminRepository>();

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

builder.Services.AddTransient<CreationContext>();
builder.Services.AddTransient<RegisterContext>();
builder.Services.AddTransient<UnknownContext>();
builder.Services.AddTransient<ViewProblemContext>();
builder.Services.AddTransient<WelcomeContext>();

builder.Services.AddHostedService<TelegramHostedService>();

builder.Services.AddLogging(configure => configure.AddSerilog());

using var host = builder.Build();

await host.RunAsync();
