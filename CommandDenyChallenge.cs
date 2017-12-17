using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using UnityEngine;

namespace ExtraConcentratedJuice.ExtraDuel
{
    public class CommandDenyChallenge : IRocketCommand
    {
        #region Properties
        
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "denychallenge";

        public string Help => "Deny a challenge from a player.";

        public string Syntax => "/denychallenge <player>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "extraduel.denychallenge" };

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
                return;
            }
            
            ep.TryTerminateChallenge(challenger.CSteamID.m_SteamID, ExtraPlayer.TerminationReasons.DENIED);
            UnturnedChat.Say(caller, Util.Translate("extraduel_challenge_deny_success"), Color.green);
        }
    }
}