﻿using System;
using UnityEngine;

namespace Memgraph
{
    public class MemgraphWindow
    {
        //Not sure if we have to return the same items with DrawContent, but to be safe we'll only change the visibility in Update()
        private const int UPDATE_FREQUENCY_HZ = 10;
        private const int WIDTH = 400;
        private const int HEIGHT = 400;
        private string rateString = "";
        private string maxString = "";
        private long lastTime = 0;
        private int currentPos = 0;
        private long[] memory = new long[WIDTH];
        private long maxMemory;
        private bool safeDraw = true;
        private bool draw = true;
        private Rect dragRect;
        private Rect windowPos;
        private Texture2D tex;

        public void Start()
        {
            dragRect = new Rect(0, 0, 10000, 20);
            windowPos = new Rect(100, 100, WIDTH + 20, HEIGHT + 50);
            tex = new Texture2D(WIDTH, HEIGHT, TextureFormat.RGBA32, false);
            for (int posX = 0; posX < WIDTH; posX++)
            {
                for (int posY = 0; posY < HEIGHT; posY++)
                {
                    tex.SetPixel(posX, posY, Color.black);
                }
            }
            tex.Apply();
        }

        public void Update()
        {
            safeDraw = draw;
            long newTime = (long)(Time.realtimeSinceStartup * UPDATE_FREQUENCY_HZ);
            if (newTime != lastTime)
            {
                lastTime = newTime;
                UpdateTexture();
            }
        }

        public void Draw()
        {
            if (safeDraw)
            {
                //Random number generated by spamming the keyboard
                windowPos = GUI.Window(45791742, windowPos, DrawContent, "Memgraph");
            }
        }

        private void DrawContent(int windowID)
        {
            GUI.DragWindow(dragRect);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label(rateString);
            GUILayout.Space(20);
            GUILayout.Label(maxString);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Close"))
            {
                draw = false;
            }
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Box(tex, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false), GUILayout.Width(WIDTH), GUILayout.Height(HEIGHT));
            GUILayout.EndVertical();
        }

        private void UpdateTexture()
        {
            //Decrease scale
            if (memory[currentPos] == maxMemory)
            {
                memory[currentPos] = 1;
                RecalculateMax();
                RedrawTexture();
            }

            //Increase scale
            memory[currentPos] = GC.GetTotalMemory(false);
            if (memory[currentPos] > maxMemory)
            {
                maxMemory = memory[currentPos];
                RedrawTexture();
            }

            //Draw new part
            int redlinePos = currentPos + 1;
            if (redlinePos == WIDTH)
            {
                redlinePos = 0;
            }
            long pixelsGreen = (memory[currentPos] * HEIGHT) / maxMemory;
            for (int posY = 0; posY < HEIGHT; posY++)
            {
                if (posY < pixelsGreen)
                {
                    tex.SetPixel(currentPos, posY, Color.green);
                }
                else
                {
                    tex.SetPixel(currentPos, posY, Color.black);
                }
                tex.SetPixel(redlinePos, posY, Color.red);
            }
            tex.Apply();

            //Figure out the rate
            int previousPos = currentPos - 1;
            if (previousPos == -1)
            {
                previousPos = WIDTH - 1;
            }
            long rateKBS = ((memory[currentPos] - memory[previousPos]) * UPDATE_FREQUENCY_HZ) / 1024;

            //Gui text, better do this once rather than every frame.
            rateString = $"Rate: {rateKBS}KB/s";
            maxString = $"Current/Max: {memory[currentPos] / 1048576}/{maxMemory / 1048576} MB";

            //Increment write pos
            currentPos++;
            if (currentPos == WIDTH)
            {
                currentPos = 0;
            }
        }

        private void RecalculateMax()
        {
            maxMemory = 0;
            for (int i = 0; i < memory.Length; i++)
            {
                if (memory[i] > maxMemory)
                {
                    maxMemory = memory[i];
                }
            }
        }

        private void RedrawTexture()
        {
            for (int posX = 0; posX < WIDTH; posX++)
            {
                long pixelsGreen = (memory[posX] * HEIGHT) / maxMemory;
                for (int posY = 0; posY < WIDTH; posY++)
                {
                    if (posY < pixelsGreen)
                    {
                        tex.SetPixel(posX, posY, Color.green);
                    }
                    else
                    {
                        tex.SetPixel(posX, posY, Color.black);
                    }
                }
            }
        }
    }
}
