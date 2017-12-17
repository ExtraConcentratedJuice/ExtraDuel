using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using UnityEngine;

namespace ExtraConcentratedJuice.ExtraDuel
{
    public class CommandArenaList : IRocketCommand
    {
        #region Properties
        
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "arenalist";

        public string Help => "List all arenas.";

        public string Syntax => "/arenalist";

        public List<string> Aliases => new List<string> { "listarena", "arenas" };

        public List<string> Permissions => new List<string> { "extraduel.arenalist" };

        #endregion
        
        public void Execute(IRocketPlayer caller, string[] args)
        {
            if (ExtraDuel.instance.arenaList.Count <= 0)
            {
                UnturnedChat.Say(caller, Util.Translate("extraduel_arenalist_none"), Color.yellow);
                return;
            }

            foreach (Arena a in ExtraDuel.instance.arenaList)
                UnturnedChat.Say(caller,
                    Util.Translate("extraduel_arenalist_message", a.name, a.pos1.ToString()),
                    Color.green);
        }
    }
}