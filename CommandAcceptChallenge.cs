using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using UnityEngine;

namespace ExtraConcentratedJuice.ExtraDuel
{
    public class CommandAcceptChallenge : IRocketCommand
    {
        #region Properties
        
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "acceptchallenge";

        public string Help => "Accept a challenge from a player.";

        public string Syntax => "/acceptchallenge <player>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "extraduel.acceptchallenge" };

        #endregion
        
        public void Execute(IRocketPlayer caller, string[] args)
        {
            if (args.Length != 1)
            {
                UnturnedChat.Say(caller, Syntax, Color.red);
                return;
            }
            
            UnturnedPlayer challenger = UnturnedPlayer.FromName(args[0]);
            if (challenger == null)
            {
                UnturnedChat.Say(caller, Util.Translate("extraduel_invalid_player"), Color.red);
                return;
            }
            
            ExtraPlayer ep = ((UnturnedPlayer)caller).GetComponent<ExtraPlayer>();
            if (!ep.challenges.ContainsKey(challenger.CSteamID.m_SteamID))
            {
                UnturnedChat.Say(caller, Util.Translate("extraduel_no_challenge"), Color.red);
            }
            
        }
    }
}