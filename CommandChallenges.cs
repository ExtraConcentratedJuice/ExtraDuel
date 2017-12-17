using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using Steamworks;
using UnityEngine;

namespace ExtraConcentratedJuice.ExtraDuel
{
    public class CommandChallenges : IRocketCommand
    {
        #region Properties
        
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "challenges";

        public string Help => "List all your challenges.";

        public string Syntax => "/challenges";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "extraduel.challenges" };

        #endregion
        
        public void Execute(IRocketPlayer caller, string[] args)
        {
            ExtraPlayer ep = ((UnturnedPlayer)caller).GetComponent<ExtraPlayer>();
            
            if (ep.challenges.Count <= 0)
            {
                UnturnedChat.Say(caller, Util.Translate("extraduel_challenges_none"), Color.yellow);
                return;
            }
            
            foreach (ulong c in ep.challenges.Keys)
            {
                UnturnedPlayer player = UnturnedPlayer.FromCSteamID(new CSteamID(c));
                UnturnedChat.Say(caller, Util.Translate("extraduel_challenges_message", player), Color.green);
            }
        }
    }
}