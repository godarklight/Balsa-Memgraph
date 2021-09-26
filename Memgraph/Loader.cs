using System;
using BalsaCore;
using UnityEngine;
using DarkLog;

namespace Memgraph
{
    [BalsaAddon]
    public class Loader
    {
        private static GameObject go;
        private static MonoBehaviour mod;

        //Game start
        [BalsaAddonInit]
        public static void BalsaInit()
        {
            //Move to menu load if you want to load later.
            if (go == null)
            {
                go = new GameObject();
                mod = go.AddComponent<MemgraphMain>();
            }
        }

        //Game exit
        [BalsaAddonFinalize]
        public static void BalsaFinalize()
        {
        }
    }
}
