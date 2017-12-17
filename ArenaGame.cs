using Rocket.Unturned.Player;
using UnityEngine;

namespace ExtraConcentratedJuice.ExtraDuel
{
    public class ArenaGame
    {
        public UnturnedPlayer participant1;
        public UnturnedPlayer participant2;
        public Vector3 previousPos1;
        public Vector3 previousPos2;
        public Arena arena;
        public bool onGoing;

        public ArenaGame(UnturnedPlayer p1, UnturnedPlayer p2, Arena a)
        {
            participant1 = p1;
            participant2 = p2;
            previousPos1 = p1.Position;
            previousPos2 = p2.Position;

            arena = a;
            onGoing = true;
        }
    }
}
