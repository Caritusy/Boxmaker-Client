using System.Collections.Generic;
using UnityEngine;

public class mario_huangguike_big : mario_charater
{
	private bool m_hit;

	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_wgk = true;
		m_min_speed = 0;
		m_mocali = 12;
		m_can_be_on_char = true;
		m_has_main_floor = true;
	}

	public override bool be_left_hit(mario_obj obj, ref int px)
	{
		if (m_hit)
		{
			return false;
		}
		if (obj.m_main)
		{
			ht(obj);
			if (obj.m_velocity.x < 0)
			{
				obj.m_velocity.x = 0;
			}
			px = obj.get_left_hit_pos(this);
			return true;
		}
		return base.be_left_hit(obj, ref px);
	}

	public override bool be_right_hit(mario_obj obj, ref int px)
	{
		if (m_hit)
		{
			return false;
		}
		if (obj.m_main)
		{
			ht(obj);
			if (obj.m_velocity.x > 0)
			{
				obj.m_velocity.x = 0;
			}
			px = obj.get_right_hit_pos(this);
			return true;
		}
		return base.be_right_hit(obj, ref px);
	}

	public override bool be_top_hit(mario_obj obj, ref int py)
	{
		if (m_hit)
		{
			return false;
		}
		return base.be_top_hit(obj, ref py);
	}

	public override bool be_bottom_hit(mario_obj obj, ref int py)
	{
		if (obj.m_main)
		{
			obj.m_velocity.y = 0;
			py = obj.get_bottom_hit_pos(this);
			if (!m_hit)
			{
				play_mode._instance.caisi(0, tx: true, obj.m_pos.x, obj.m_bound.bottom);
				ht(obj);
			}
			return true;
		}
		return base.be_bottom_hit(obj, ref py);
	}

	public override void be_hit(mario_obj obj)
	{
		if (m_hit)
		{
			if (obj.m_wgk)
			{
				if (obj.m_name == "mario_wuguike_big" || obj.m_name == "mario_huangguike_big")
				{
					set_bl(0, 4);
				}
				obj.set_bl(0, 4);
			}
			else if (obj.m_main)
			{
				base.be_hit(obj);
			}
			else if (obj.m_life == 1)
			{
				obj.set_bl(0, 4);
			}
		}
		else
		{
			base.be_hit(obj);
		}
	}

	private void ht(mario_obj obj)
	{
		if (!m_hit)
		{
			m_hit = true;
			play_mode._instance.add_score(m_pos.x, m_pos.y, 200);
			m_min_speed = 100;
			if (obj.m_pos.x < m_pos.x)
			{
				set_fx(mario_fx.mf_right);
				m_velocity.x = m_min_speed;
			}
			else
			{
				set_fx(mario_fx.mf_left);
				m_velocity.x = -m_min_speed;
			}
			mario._instance.play_sound("sound/zhuang");
		}
	}

	public override void change()
	{
		if (m_param[0] == 0)
		{
			m_param[0] = 1;
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
		else if (m_bl[0] == 0)
		{
			if (!m_hit)
			{
				play_anim("stand");
				m_velocity.x = 0;
			}
			else
			{
				play_anim("run");
			}
		}
	}
}
