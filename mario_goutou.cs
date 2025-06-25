using System.Collections.Generic;
using UnityEngine;

public class mario_goutou : mario_charater
{
	private int m_die_time;

	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_life = 1;
		m_min_speed = 20;
		m_can_be_on_char = true;
	}

	public override void reset()
	{
		if (m_param[0] % 2 == 0)
		{
			set_fx(mario_fx.mf_left);
		}
		else
		{
			set_fx(mario_fx.mf_right);
		}
		if (m_param[0] / 2 == 0)
		{
			m_hr.SetActive(value: false);
		}
		else
		{
			m_hr.SetActive(value: true);
		}
	}

	public override bool be_bottom_hit(mario_obj obj, ref int py)
	{
		if (m_bl[0] == 1)
		{
			return false;
		}
		if (obj.m_main)
		{
			obj.m_pvelocity.y = 150;
			if (obj.m_velocity.y < 0)
			{
				obj.m_velocity.y = 0;
			}
			play_mode._instance.caisi(0, tx: true, obj.m_pos.x, obj.m_bound.bottom);
			m_is_die = true;
			play_mode._instance.add_score(m_pos.x, m_pos.y, 200);
			mario._instance.play_sound("sound/caisi");
			py = obj.get_bottom_hit_pos(this);
			return true;
		}
		return base.be_bottom_hit(obj, ref py);
	}

	public override void be_hit(mario_obj obj)
	{
		if (m_bl[0] != 1)
		{
			base.be_hit(obj);
		}
	}

	public override void change()
	{
		if (m_param[0] < 3)
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

	public override void set_bl(int index, int num)
	{
		base.set_bl(index, num);
		if (index == 0 && num == 4)
		{
			m_is_die = true;
			m_velocity.x = Random.Range(-50, 50);
			m_velocity.y = 150;
			mario._instance.play_sound("sound/zhuang");
			play_mode._instance.add_score(m_pos.x, m_pos.y, 200);
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
		else
		{
			if (m_bl[0] != 0)
			{
				return;
			}
			if (m_is_die)
			{
				if (m_die_time == 0)
				{
					play_anim("die");
				}
				if (m_die_time > 20)
				{
					m_velocity.y -= utils.g_g;
				}
				else
				{
					m_velocity.x = 0;
					m_velocity.y = 0;
				}
				m_die_time++;
			}
			else
			{
				play_anim("stand");
				if (m_param[0] / 2 == 1)
				{
					check_zhineng();
				}
			}
		}
	}
}
