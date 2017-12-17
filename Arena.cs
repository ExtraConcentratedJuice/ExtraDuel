using System;
using Rocket.Unturned.Player;
using UnityEngine;

namespace ExtraConcentratedJuice.ExtraDuel
{
    public class Arena
    {
        public ArenaGame game;
        public string name;
        public Vector3 pos1;
        public Vector3 pos2;
        public Rect rect;

        public Arena(Vector3 p1, Vector3 p2, string arenaName)
        {
            game = null;
            name = arenaName;
            pos1 = p1;
            pos2 = p2;

            rect = Rect.MinMaxRect(
                Math.Min(pos1.x, pos2.x),
                Math.Min(pos1.z, pos2.z),
                Math.Max(pos1.x, pos2.x),
                Math.Max(pos1.z, pos2.z)
                );
        }

        public Boolean IsInArena(Vector3 p) => rect.Contains(Util.Vector2(p), true);

        public void Teleport(UnturnedPlayer p)
        {
            Vector3 arenaCenter = rect.center;
            p.Teleport(new Vector3(arenaCenter.x, pos2.y + 3, arenaCenter.z), p.Rotation);
        }
    } 
}
