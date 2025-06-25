using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BMReborn
{
    internal class mario_heihua : mario_charater
    {
        public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
        {
            base.init(name, param, world, x, y, xx, yy);
            m_life = 1;
            m_can_be_on_char = false;
        }

        public override void set_bl(int index, int num)
        {
            base.set_bl(index, num);
            if (index == 0 && num == 4)
            {
                m_is_die = true;
                m_velocity.x = UnityEngine.Random.Range(-50, 50);
                m_velocity.y = 150;
                mario._instance.play_sound("sound/zhuang");
                play_mode._instance.add_score(m_pos.x, m_pos.y, 500);
            }
        }

        public override void tupdate()
        {
            base.tupdate();
            if (m_bl[0] == 4)
            {
                m_velocity.y += utils.g_g;
            }
            else if (m_bl[0] == 0)
            {
                if (play_mode._instance.m_main_char.m_pos.x < m_pos.x)
                {
                    set_fx(mario_fx.mf_left);
                }
                else
                {
                    set_fx(mario_fx.mf_right);
                }
            }
        }
    }
}
                