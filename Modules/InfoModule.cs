using Discord;
using Discord.Net;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace REEE_Plays_Pokemon.Modules
{
    public class InfoModule : ModuleBase
    {
        [Command("Help")]
        [Summary("Lists commands to use to play the pokémon game")]
        public Task Help() => HelpFunction();

        [Command("h")]
        [Summary("Lists commands to use to play the pokémon game")]
        public Task H() => HelpFunction();

        [Command("?")]
        [Summary("Lists commands to use to play the pokémon game")]
        public Task Question() => HelpFunction();

        private Task HelpFunction()
        {
            Console.WriteLine("run");
            return ReplyAsync("To invoke the bot use /PlayPokemon prefix, /PP or /Play.\nCommands:\n\t\u2022Help/?/help/h <*command*/*none*>: show help about the bot or a specific command\n\t\u2022move <*up*,*u*,*w*,*forward*,*down*,*s*,*back*,*left*,*l*,*a*,*right*,*d*>: move in the direction specified\n\t\u2022select/s/use/u: select an item or menu that is being hovered over, also the use command.\n\t\u2022SpecialItem/special/y: Use the selected special item\n\t\u2022Back/b/return/r: leave the current menu\n\t\u2022start/menu/x/m/e:Open the start menu");
        }
    }
}