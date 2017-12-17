using System;
using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using UnityEngine;

namespace ExtraConcentratedJuice.ExtraDuel
{
    public class CommandSetArenaPosition : IRocketCommand
    {
        #region Properties
        
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "setarenaposition";

        public string Help => "Sets your positions before defining an arena.";

        public string Syntax => "/setarenaposition <1 or 2>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "extraduel.setposition" };

        #endregion
        
        public void Execute(IRocketPlayer caller, string[] args)
        {
            if (args.Length != 1)
            {
                UnturnedChat.Say(caller, Syntax, Color.red);
                return;
            }

            UnturnedPlayer uPlayer = (UnturnedPlayer)caller;
            if (Int32.TryParse(args[0], out int posNumber))
            {
                switch (posNumber)
                {
                    case 1:
                    {
                        Vector3 position = uPlayer.Position;
                        ExtraPlayer player = uPlayer.GetComponent<ExtraPlayer>();
                        player.selectedPos1 = position;
                        UnturnedChat.Say(Util.Translate("extraduel_setposition_success", 1, position.ToString()),
                            Color.green);
                        break;
                    }
                    case 2:
                    {
                        Vector3 position = uPlayer.Position;
                        ExtraPlayer player = uPlayer.GetComponent<ExtraPlayer>();
                        player.selectedPos2 = position;
                        UnturnedChat.Say(Util.Translate("extraduel_setposition_success", 2, position.ToString()),
                            Color.green);
                        break;
                    }
                    default:
                        UnturnedChat.Say(caller, Syntax, Color.red);
                        break;
                }
            }
            else
                UnturnedChat.Say(caller, Syntax, Color.red);
        }
    }
}