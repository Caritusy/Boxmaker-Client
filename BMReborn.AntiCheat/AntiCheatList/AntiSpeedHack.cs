using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BMReborn.AntiCheat.AntiCheatList
{
    internal class AntiSpeedHack : AntiCheatCore
    {
        DateTime OnStartMap = DateTime.Now;
        DateTime OnThisUpdate = DateTime.Now;
        DateTime OnLessTimeUpdate = DateTime.Now;
        int lastSecond = 0;
        int thisSecond = 0;
        long UpdateCount = 0;
        long ErrorCount = 0;
        readonly float AllowErrorRange = 0.2f;

        public AntiSpeedHack() 
        {
            Name = "NoSpeedHack";
            lastSecond = game_data._instance.m_map_data.time;
            Application.runInBackground = true;
        }

        public override void Update()
        {
            UpdateCount++;
            thisSecond = play_mode._instance.m_time;
            OnThisUpdate = DateTime.Now;
            //mario._instance.show_tip($"Last : {lastSecond} This : {thisSecond}");
            if (lastSecond != thisSecond && !play_mode._instance.m_pause)
            {
                lastSecond = thisSecond;
                if (!((OnThisUpdate - OnLessTimeUpdate).TotalMilliseconds < 60f && (OnThisUpdate - OnLessTimeUpdate).TotalMilliseconds > 5f))
                {
                    ErrorCount++;
                    //mario._instance.show_tip("BMAC : 变速监测++");
                }
                //mario._instance.show_tip($"{(OnThisUpdate - OnLessTimeUpdate).TotalMilliseconds}");
                OnLessTimeUpdate = DateTime.Now;
            }


        }

        public override void Punish()
        {
            play_gui.Return();
            mario._instance.show_tip("BMAC : 反作弊侦查到了你的变速行为，已经退出游戏。");
        }

        public override void Reset()
        {
            OnStartMap = DateTime.Now;
            OnThisUpdate = DateTime.Now;
            OnLessTimeUpdate = DateTime.Now;
            lastSecond = 0;
            thisSecond = 0;
            UpdateCount = 0;
            ErrorCount = 0;
        }

        public override bool IsGameCheated
        {
            get 
            {
                return ErrorCount > 75;
            }
        }
    }
}
