using System;
using UnityEngine;
using DarkLog;

namespace Memgraph
{
    public class MemgraphMain : MonoBehaviour
    {
        ModLog log;
        MemgraphWindow window;

        public void Start()
        {
            log = new ModLog("Memgraph");
            log.Log("Memgraph Start!");
            window = new MemgraphWindow();
            window.Start();
            DontDestroyOnLoad(this);
        }

        public void Update()
        {
            window.Update();
        }

        public void OnGUI()
        {
            window.Draw();
        }
    }
}
