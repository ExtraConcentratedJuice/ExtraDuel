using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using UnityEngine;

namespace ExtraConcentratedJuice.ExtraDuel
{
    public class CommandDefineArena : IRocketCommand
    {
        #region Properties
        
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "definearena";

        public string Help => "Define an arena with the current set positions.";

        public string Syntax => "/definearena <name>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "extraduel.definearena" };

        #endregion
        
        public void Execute(IRocketPlayer caller, string[] args)
        {
            if (args.Length != 1)
            {
                UnturnedChat.Say(caller, Syntax, Color.red);
                return;
            }
            
            ExtraPlayer player = ((UnturnedPlayer)caller).GetComponent<ExtraPlayer>();
            if (player.selectedPos1 == null || player.selectedPos2 == null)
            {
                UnturnedChat.Say(caller, Util.Translate("extraduel_no_position_set"), Color.red);
                return;
            }
            
            Arena newArena = new Arena(player.selectedPos1, player.selectedPos2, args[0]);
            foreach (Arena a in ExtraDuel.instance.arenaList)
            {
                if (a.rect.Overlaps(newArena.rect))
                {
                    UnturnedChat.Say(caller, Util.Translate("extraduel_definearena_fail_overlap"), Color.red);
                    return;
                }
                
                if (a.name != newArena.name) continue;
                
                UnturnedChat.Say(caller, Util.Translate("extraduel_definearena_fail_same_name"), Color.red);
                return;
            }
            
            ExtraDuel.instance.arenaList.Add(newArena);
            UnturnedChat.Say(caller, Util.Translate("extraduel_definearena_success"), Color.green);
            ExtraDuel.instance.SerializeArena(ExtraDuel.arenaPath);
        }
    }
}