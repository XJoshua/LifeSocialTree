using System;
using System.Collections.Generic;
using QFramework;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
    public class GameData
    {
        //private static GameData data;
        
        private int score;
        public int Score => score;

        private int round;
        public int Round => round;

        private bool isGameEnd = false;
        public bool IsGameEnd => isGameEnd;

    }
}