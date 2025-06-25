using System.Collections.Generic;
using UnityEngine;

public class mario_luobo : mario_charater
{
	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_is_guaiwu = false;
	}

	public override void reset()
	{
		if (m_param[0] == 0)
		{
			m_min_speed = 0;
			set_fx(mario_fx.mf_left);
			base.transform.FindChild("fx").localEulerAngles = new Vector3(0f, 0f, -90f);
		}
		else if (m_param[0] == 1)
		{
			m_min_speed = 40;
			set_fx(mario_fx.mf_left);
			base.transform.FindChild("fx").localEulerAngles = new Vector3(0f, 0f, 0f);
		}
		else
		{
			m_min_speed = 40;
			set_fx(mario_fx.mf_right);
			base.transform.FindChild("fx").localEulerAngles = new Vector3(0f, 0f, 0f);
		}
	}

	public override void be_hit(mario_obj obj)
	{
		if (obj.m_main)
		{
			m_is_destory = 1;
			obj.set_bl(1, 50);
			obj.set_bl(0, 3);
			play_mode._instance.caisi(0);
			mario._instance.play_sound("sound/get");
			play_mode._instance.add_score(m_pos.x, m_pos.y, 1000);
		}
	}

	public override void change()
	{
		if (m_param[0] < 2)
		{
			List<int> param;
			List<int> list = (param = m_param);
			int index;
			int index2 = (index = 0);
			index = param[index];
			list[index2] = index + 1;
		}
		else
		{
			m_param[0] = 0;
		}
		if (m_unit != null)
		{
			game_data._instance.m_arrays[m_world][m_init_pos.y][m_init_pos.x].param[0] = m_param[0];
		}
		reset();
	}
}
