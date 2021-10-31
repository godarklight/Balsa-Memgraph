using System;
using System.IO;
using UnityEngine;
using DarkLog;

namespace Memgraph
{
    public class MemgraphMain : MonoBehaviour
    {
        ModLog log;
        MemgraphWindow window;
        StreamWriter sw;

        public void Start()
        {
            log = new ModLog("Memgraph");
            log.Log("Memgraph Start!");
            window = new MemgraphWindow();
            window.Start();
            sw = new StreamWriter("mem.csv");
            DontDestroyOnLoad(this);
                       
        }

        public void Update()
        {
            sw.WriteLine($"{ Time.realtimeSinceStartup },{ GC.GetTotalMemory(false) }");
            window.Update();
        }

        public void OnGUI()
        {
            long bytesOrig = GC.GetTotalMemory(false);
            window.Draw();
            long bytesNew = GC.GetTotalMemory(false);
            long bytesDiff = bytesNew - bytesOrig;
            if (bytesNew > bytesOrig)
            {
                log.Log("Bytes allocated: " + bytesDiff);
            }
        }
    }
}
