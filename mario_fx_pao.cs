using System.Collections.Generic;
using UnityEngine;

public class mario_fx_pao : mario_charater
{
	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_life = 1;
		m_can_be_on_char = true;
	}

	public override void set_bl(int index, int num)
	{
		base.set_bl(index, num);
		if (index == 0 && num == 4)
		{
			m_is_die = true;
			m_velocity.x = Random.Range(-50, 50);
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
			m_velocity.y -= utils.g_g;
			play_anim("die1");
		}
		else if (m_bl[0] == 0 && !(play_mode._instance.m_main_char == null))
		{
			if (play_mode._instance.m_time % 150 == 90)
			{
				play_anim("attack");
			}
			if (play_mode._instance.m_time % 150 == 100)
			{
				List<int> list = new List<int>();
				list.Add(0);
				list.Add(0);
				list.Add(0);
				list.Add(0);
				mario_obj mario_obj2 = play_mode._instance.create_mario_obj_ex("mario_huoqiu", null, list, -1, -1, m_pos.x, m_pos.y + 100);
				mario_obj2.m_velocity.set(play_mode._instance.m_main_char.m_pos.x, play_mode._instance.m_main_char.m_pos.y);
				mario_obj2.reset();
			}
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
