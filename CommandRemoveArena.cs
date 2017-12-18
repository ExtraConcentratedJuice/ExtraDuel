using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using UnityEngine;
using System.Linq;

namespace ExtraConcentratedJuice.ExtraDuel
{
    public class CommandRemoveArena : IRocketCommand
    {
        #region Properties
        
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "removearena";

        public string Help => "Removes an arena.";

        public string Syntax => "/removearena <arena name>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "extraduel.removearena" };

        #endregion
        
        public void Execute(IRocketPlayer caller, string[] args)
        {
            if (args.Length != 1)
            {
                UnturnedChat.Say(caller, Syntax, Color.red);
                return;
            }

            int index = ExtraDuel.instance.arenaList.FindIndex(x => x.name == args[0]);
            if (index > -1)
            {
                ExtraDuel.instance.arenaList.RemoveAt(index);
                UnturnedChat.Say(caller, Util.Translate("extraduel_removearena_success"), Color.green);
                ExtraDuel.instance.SerializeArena(ExtraDuel.arenaPath);
                return;
            }
            UnturnedChat.Say(caller, Util.Translate("extraduel_removearena_fail_not_found"), Color.red);
        }
    }
}