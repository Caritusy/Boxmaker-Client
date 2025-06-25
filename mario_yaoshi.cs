using System.Collections.Generic;
using UnityEngine;

public class mario_yaoshi : mario_charater
{
	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_has_edit = true;
		m_is_guaiwu = false;
		//m_is_guaiwu = m_param[0] == 1;
		//if (m_param[0] == 1) m_is_guaiwu = true;
	}

	public override void reset()
	{
		if (m_param[0] == 1)
		{
			set_fx(mario_fx.mf_up);
			//m_is_guaiwu = true;
		}
		else
		{
			set_fx(mario_fx.mf_right);
			//m_is_guaiwu = false;
		}
	}

	public override void be_hit(mario_obj obj)
	{
		if (obj.m_main)
		{
			//if (m_param[0] == 1) m_is_guaiwu = true;
			m_is_destory = 1;
			play_mode._instance.caisi(0);
			mario._instance.play_sound("sound/get");
			play_mode._instance.add_score(m_pos.x, m_pos.y, 1000);
			if (m_param[0] != 1) play_mode._instance.m_ys++;
			else play_mode._instance.m_ys--;
		}
	}
	public override void change()
	{
		if (m_param[0] != 1)
		{
			m_param[0] = 1;
			//m_is_guaiwu = true;
		}
		else
		{
			m_param[0] = 0;
			//m_is_guaiwu = false;
		}
		if (m_unit != null)
		{
			game_data._instance.m_arrays[m_world][m_init_pos.y][m_init_pos.x].param[0] = m_param[0];
		}
		mario._instance.show_tip($"isKiller:{m_param[0] == 1}");
		reset();
	}
}
