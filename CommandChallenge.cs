using System;
using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using UnityEngine;

namespace ExtraConcentratedJuice.ExtraDuel
{
    public class CommandChallenge : IRocketCommand
    {
        #region Properties
        
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "challenge";

        public string Help => "Challenge a player to a duel.";

        public string Syntax => "/challenge <player> <arena>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "extraduel.challenge" };

        #endregion
        
        public void Execute(IRocketPlayer caller, string[] args)
        {
            if (args.Length != 2)
            {
                UnturnedChat.Say(caller, Syntax, Color.red);
                return;
            }
            
            UnturnedPlayer challenged = UnturnedPlayer.FromName(args[0]);
            if (challenged == null)
            {
                UnturnedChat.Say(caller, Util.Translate("extraduel_invalid_player"), Color.red);
                return;
            }
            
            UnturnedPlayer p = (UnturnedPlayer)caller;
            if (challenged.CSteamID.m_SteamID == p.CSteamID.m_SteamID)
            {
                UnturnedChat.Say(caller, Util.Translate("extraduel_self_invoke"), Color.red);
                return;
            }

            ExtraPlayer ep = challenged.GetComponent<ExtraPlayer>();
            if (!ep.acceptingChallengers)
            {
                UnturnedChat.Say(caller, Util.Translate("extraduel_challenges_off"), Color.red);
                return;
            }
            if (!ExtraDuel.ArenaExists(args[1]))
            {
                UnturnedChat.Say(caller, Util.Translate("extraduel_removearena_fail_not_found"), Color.red);
                return;
            }

            if (ep.challenges.ContainsKey(p.CSteamID.m_SteamID))
            {
                UnturnedChat.Say(caller, Util.Translate("extraduel_already_challenged"), Color.red);
                return;
            }

            ep.CreateChallenge(challenged, DateTime.Now.AddSeconds(20), ExtraDuel.ArenaFromName(args[1]));
            UnturnedChat.Say(caller, Util.Translate("extraduel_challenge_success", challenged.DisplayName), Color.green);
        }
    }
}