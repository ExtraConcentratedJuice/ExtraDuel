using System;
using UnityEngine;

namespace ExtraConcentratedJuice.ExtraDuel
{
    [Serializable]
    public class SerializableArena
    {
        public string name;
        public float pos1x;
        public float pos1y;
        public float pos1z;
        public float pos2x;
        public float pos2y;
        public float pos2z;

        public SerializableArena(Vector3 p1, Vector3 p2, string arenaName)
        {
            name = arenaName;
            pos1x = p1.x;
            pos1y = p1.y;
            pos1z = p1.z;
            pos2x = p2.x;
            pos2y = p2.y;
            pos2z = p2.z;
        }
    }
}
