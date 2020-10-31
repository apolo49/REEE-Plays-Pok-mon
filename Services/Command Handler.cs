using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using REEE_Plays_Pokemon.Modules;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace REEE_Plays_Pokemon.Services
{
    // Retrieve client and CommandService instance via ctor
    internal class CommandHandler
    {
        private readonly IConfiguration _config;
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;

        public CommandHandler(IServiceProvider services)
        {
            // since we passed the services in, we can use GetRequiredService to pass them into the fields set earlier
            _config = services.GetRequiredService<IConfiguration>();
            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            // take action when we execute a command
            _commands.CommandExecuted += CommandExecutedAsync;

            // take action when we receive a message (so we can process it, and see if it is a valid command)
            _client.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            // register modules that are public and inherit ModuleBase<T>.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // ensures we don't process system/other bot messages
            if (!(rawMessage is SocketUserMessage message))
            {
                return;
            }

            if (message.Source != MessageSource.User)
            {
                return;
            }

            // sets the argument position away from the prefix we set
            var argPos = int.Parse("2");

            // get prefix from the configuration file
            List<string> prefixes = new List<string> { "/playpokemon ", "/pp ", "/pokemon ", "PlayPokemon ", "/PP ", "/Pokemon " };

            // determine if the message has a valid prefix, and adjust argPos based on prefix
            if (!(message.HasMentionPrefix(_client.CurrentUser, ref argPos) ||
                message.HasStringPrefix(prefixes[0], ref argPos) ||
                message.HasStringPrefix(prefixes[1], ref argPos) ||
                message.HasStringPrefix(prefixes[2], ref argPos) ||
                message.HasStringPrefix(prefixes[3], ref argPos) ||
                message.HasStringPrefix(prefixes[4], ref argPos) ||
                message.HasStringPrefix(prefixes[5], ref argPos)))
            {
                return;
            }

            var context = new SocketCommandContext(_client, message);

            // execute command if one is found that matches
            await _commands.ExecuteAsync(context, argPos, _services);
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // if a command isn't found, log that info to console and exit this method
            if (!command.IsSpecified)
            {
                System.Console.WriteLine($"Command failed to execute for [{command.GetValueOrDefault().Name}] <-> [{context.User}]!");
                return;
            }

            // log success to the console and exit this method
            if (result.IsSuccess)
            {
                System.Console.WriteLine($"Command [{command.GetValueOrDefault().Name}] executed for -> [{context.User}]");
                return;
            }

            // failure scenario, let's let the user know
            await context.Channel.SendMessageAsync($"Sorry {context.User}, ... something went wrong!");
        }
    }
}