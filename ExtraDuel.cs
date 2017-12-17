using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;
using Random = System.Random;

namespace ExtraConcentratedJuice.ExtraDuel
{
    public class ExtraDuel : RocketPlugin<ExtraDuelConfig>
    {
        public static ExtraDuel instance;
        public List<Arena> arenaList;
        public List<ArenaGame> games;
        private DateTime lastUpdated;
        public const string arenaPath = "Plugins\\ExtraDuel\\arenas.dat";
        public static Random Random = new Random();

        protected override void Load()
        {
            instance = this;
            lastUpdated = DateTime.Now;
            games = new List<ArenaGame>();

            if (File.Exists(arenaPath))
            {
                try
                {
                    arenaList = DeserializeArena(arenaPath);
                }
                catch (SerializationException)
                {
                    arenaList = new List<Arena>();
                    Logger.Log("Deserialization of arenas datafile failed.");
                    Logger.Log("This is normal for the first run. If it persists, delete arenas.dat in this plugin's directory.");

                }
            }
            else
            {
                File.Create(arenaPath).Dispose();
                arenaList = new List<Arena>();
            }
            UnturnedPlayerEvents.OnPlayerUpdatePosition += PlayerUpdatedPosition;
            U.Events.OnPlayerDisconnected += OnDisconnected;
            DamageTool.playerDamaged += OnPlayerDamage;
        }

        protected override void Unload()
        {
        }
        
        private void OnDisconnected(UnturnedPlayer player)
        {
            foreach (SteamPlayer p in Provider.clients)
            {
                UnturnedPlayer.FromSteamPlayer(p).GetComponent<ExtraPlayer>().OnDisconnect(player);
            }
        }

        public void SerializeArena(string path)
        {
            List<SerializableArena> sArenaList = new List<SerializableArena>();
            foreach (Arena a in arenaList)
            {
                sArenaList.Add(new SerializableArena(a.pos1, a.pos2, a.name));
            }
            using (Stream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, sArenaList);
            }
        }
        public List<Arena> DeserializeArena(string path)
        {
            List<SerializableArena> sArenaList;
            List<Arena> list = new List<Arena>();
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                sArenaList = (List<SerializableArena>)binaryFormatter.Deserialize(stream);
            }
            foreach(SerializableArena a in sArenaList)
            {
                Vector3 p1 = new Vector3(a.pos1x, a.pos1y, a.pos1z);
                Vector3 p2 = new Vector3(a.pos2x, a.pos2y, a.pos2z);
                list.Add(new Arena(p1, p2, a.name));
            }
            return list;
        }

        private void PlayerUpdatedPosition(UnturnedPlayer p, Vector3 pos)
        {
            ExtraPlayer ep = p.GetComponent<ExtraPlayer>();
            
            if (!ArenaCheck(p, pos, ep.lastPosition))
                ep.lastPosition = pos;
        }

        private void OnPlayerDamage(Player player, ref EDeathCause cause, ref ELimb limb, ref CSteamID killer, ref Vector3 direction, ref float damage, ref float times, ref bool canDamage)
        {
            UnturnedPlayer uPlayer = UnturnedPlayer.FromPlayer(player);
            
            if (!uPlayer.HasPermission("extraduel.invulnerable")) return;
            
            damage = 0;
            canDamage = false;
        }

        public static bool ArenaCheck(UnturnedPlayer p, Vector3 pos, Vector3 prevPos)
        {
            foreach (Arena a in instance.arenaList)
                if (a.IsInArena(pos))
                    if (p.HasPermission("extraduel.enterarena"))
                    {
                        p.Teleport(prevPos, p.Rotation);
                        UnturnedChat.Say(p, "Get out of this arena!");
                        return true;
                    }
            
            return false;
        }

        public static bool ArenaExists(string name)
        {
            for (int i = 0; i < instance.arenaList.Count; i++)
            {
                if (instance.arenaList[i].name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool ArenaExists(Arena a)
        {
            for (int i = 0; i < instance.arenaList.Count; i++)
            {
                if (instance.arenaList[i].Equals(a))
                {
                    return true;
                }
            }
            return false;
        }

        public static Arena ArenaFromName(string name)
        {
            for (int i = 0; i < instance.arenaList.Count; i++)
            {
                if (instance.arenaList[i].name == name)
                {
                    return instance.arenaList[i];
                }
            }
            return null;
        }

        private void FixedUpdate()
        {
            if ((DateTime.Now - lastUpdated).TotalSeconds > Util.getConfig().updateTime)
            {
                for(int i = 0; i < games.Count; i++)
                {
                    if (!games[i].onGoing)
                    {
                        games.RemoveAt(i);
                    }
                }
            }
        }

        public override TranslationList DefaultTranslations => new TranslationList
        {
                    {"extraduel_invalid_player", "The specified player was not found."},
                    {"extraduel_self_invoke", "You cannot use this command on yourself."},
                    {"extraduel_teleport_out_of_arena", "You were in an arena when you logged in, so we teleported you out into a random spawn!"},
                    {"extraduel_no_position_set", "You have no positions set. Set a couple with /setarenaposition <1 or 2>"},
                    {"extraduel_setposition_success", "You have successfully set position {0} to {1}!"},
                    {"extraduel_definearena_success", "You have successfully defined an arena!"},
                    {"extraduel_definearena_fail_overlap", "This arena is overlapping with another arena."},
                    {"extraduel_definearena_fail_same_name", "There is already an arena with that name!"},
                    {"extraduel_removearena_success", "You have successfully removed that arena!"},
                    {"extraduel_removearena_fail_not_found", "No arena was found with that name."},
                    {"extraduel_arenalist_none", "No arenas were found."},
                    {"extraduel_challenge", "You have been challenged to a duel in arena '{0}' by {1}!"},
                    {"extraduel_challenges_none", "No challenges were found."},
                    {"extraduel_challenges_message", "Challenger: {0}"},
                    {"extraduel_challenge_success", "You have successfully challenged {0} to a duel!"},
                    {"extraduel_challenge_deny", "Your challenge to {0} has been denied."},
                    {"extraduel_no_challenge", "That player has no challenges open with you."},
                    {"extraduel_challenge_deny_success", "You have successfully denied {0}'s challenge!"},
                    {"extraduel_challenge_accept", "Your challenge to {0} has been accepted!"},
                    {"extraduel_challenge_terminated", "Challenge terminated: {0}, {1}"},
                    {"extraduel_challenge_off", "That player is not acceping challenges."},
                    {"extraduel_already_challenged", "You already have a challenge open with this player."},
                    {"extraduel_arenalist_message", "Arena: {0} | Location: {1}"}
                };
    }
}
