﻿using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Net.Providers.WS4Net;
using Discord.WebSocket;
using Discord.Addons.Interactive;
using Microsoft.Extensions.DependencyInjection;
using Nadja.Command;

namespace Nadja
{
    public class Bot
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private InteractiveService _internalService;
        private readonly string prefix = "$";

        public Bot()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                WebSocketProvider = WS4NetProvider.Instance,
            });
            _commands = new CommandService();
            _internalService = new InteractiveService(_client);

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton(_internalService)
                .BuildServiceProvider();

            Task.Run(async () => { await StartAsync(); });
        }

        private async Task StartAsync()
        {
            
            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, ConfigurationManager.AppSettings["DiscordBotToken"]);

            await _client.StartAsync();

            await Task.Delay(-1);
        }

        public async Task RegisterCommandsAsync()
        {
            _client.Ready += ClientReady;
            _client.MessageReceived += ClientMessageReceived;

            await _commands.AddModuleAsync<Commands>(_services);

        }

        private async Task ClientReady()
        {
            var server = _client.Guilds.SingleOrDefault(g => g.Name == "Black Survival");
            ITextChannel channel;
            if (server != null)
            {
                channel = server.TextChannels.Single(tc => tc.Name == "bot_testing");
                //if (server.TextChannels.All(tc => tc.Name != "sh-comp-logger"))
                //    channel = await server.CreateTextChannelAsync("sh-comp-logger");
                //else channel = server.TextChannels.Single(tc => tc.Name == "sh-comp-logger");
                if (channel != null) await channel.SendMessageAsync("Bonjour");
            }
        }

        private async Task ClientMessageReceived(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            if (message is null || message.Author.IsBot) return;

            int argPos = 0;

            if (message.HasStringPrefix(prefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(_client, message);

                var result = await _commands.ExecuteAsync(context, argPos, _services);

                if (!result.IsSuccess)
                    await context.Channel.SendMessageAsync(result.ErrorReason);


            }

        }
    }
}