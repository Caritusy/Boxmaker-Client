using BMReborn.AntiCheat.AntiCheatList;
using System;
using System.Collections.Generic;
using System.Text;

namespace BMReborn.AntiCheat
{
    internal class AntiCheatManager
    {
        private static readonly bool AntiCheatEnabled = false;

        public static List<AntiCheatCore> AntiCheats = new List<AntiCheatCore>() 
        {
            new AntiSpeedHack(),
        };

        public static void RunAntiCheatDetect()
        {
            if (!AntiCheatEnabled) return;
            foreach (var ac in AntiCheats)
            {
                ac.Update();
                if (ac.IsGameCheated)
                {
                    ac.Punish();
                    ac.Reset();
                }
            }
        }
    }
}
