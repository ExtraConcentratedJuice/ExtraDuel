using System;
using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace ExtraConcentratedJuice.ExtraDuel
{
    public class ExtraPlayer : UnturnedPlayerComponent
    {
        public bool isInArena;
        public ArenaGame game;
        public Vector3 selectedPos1;
        public Vector3 selectedPos2;
        public Vector3 lastPosition;
        public Dictionary<ulong, Challenge> challenges;
        public bool acceptingChallengers;
        public DateTime lastUpdated;

        private Dictionary<TerminationReasons, string> Reasons = new Dictionary<TerminationReasons, string>
        {
            { TerminationReasons.DENIED, "Your challenge was denied." },
            { TerminationReasons.ENTERED_CHALLENGE, "The challenged player entered a duel." },
            { TerminationReasons.EXPIRED, "Your challenge with that player has expired." },
            { TerminationReasons.LEAVE, "That player has left." },
            { TerminationReasons.TOGGLE_CHALLENGES, "That player has turned their challenges off." }
        };

        public enum TerminationReasons
        {
            DENIED,
            EXPIRED,
            LEAVE,
            TOGGLE_CHALLENGES,
            ENTERED_CHALLENGE
        }

        protected override void Load()
        {
            acceptingChallengers = true;
            lastUpdated = DateTime.Now;
            challenges = new Dictionary<ulong, Challenge>();
            isInArena = false;
            
            if (!Player.HasPermission("extraduel.can_spawn_in_arena"))
            {
                foreach (Arena a in ExtraDuel.instance.arenaList)
                {
                    if (!a.IsInArena(Player.Position)) continue;
                    
                    int rando = ExtraDuel.Random.Next(LevelPlayers.spawns.Count);
                    PlayerSpawnpoint spawn = LevelPlayers.spawns[rando];
                    
                    Player.Teleport(spawn.point, Player.Rotation);
                    break;
                }
            }
            lastPosition = Player.Position;
        }

        public void OnDisconnect(UnturnedPlayer player)
        {
            if (challenges.ContainsKey(player.CSteamID.m_SteamID))
                TryTerminateChallenge(player.CSteamID.m_SteamID, TerminationReasons.LEAVE);
        }

        private void FixedUpdate()
        {
            foreach (ulong key in challenges.Keys)
                if ((challenges[key].expiry - DateTime.Now).TotalSeconds < 0)
                    TryTerminateChallenge(key, TerminationReasons.EXPIRED);
        }

        public void TryTerminateChallenge(ulong SteamID, TerminationReasons TReason)
        {
            if (!challenges.TryGetValue(SteamID, out Challenge v)) return;

            string Reason = Reasons[TReason] ?? "No defined reason.";

            UnturnedChat.Say(v.challenger, Util.Translate("extraduel_challenge_terminated", v.challenged.DisplayName, Reason), Color.red);
            challenges.Remove(SteamID);
        }

        public void CreateChallenge(UnturnedPlayer p, DateTime expiry, Arena arena)
        {
            ExtraPlayer ep = p.GetComponent<ExtraPlayer>();
            ep.challenges.Add(Player.CSteamID.m_SteamID, new Challenge(Player, p, arena, expiry));
            UnturnedChat.Say(p, Util.Translate("extraduel_challenge", arena.name, Player.DisplayName), Color.yellow);
        }
    }

}
