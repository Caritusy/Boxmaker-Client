using System.Collections.Generic;
using UnityEngine;

public class mario_charater : mario_obj
{
	public GameObject m_hr;

	public bool m_is_guaiwu = true;

	private int m_chu_time;

	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_type = mario_type.mt_charater;
		m_bkcf = true;
		m_has_main_floor = false;
		m_bl.Add(0);
	}

	public override bool be_left_hit(mario_obj obj, ref int px)
	{
		if (obj.m_type != mario_type.mt_charater || m_main)
		{
			return false;
		}
		if (obj.m_main)
		{
			if (m_is_guaiwu)
			{
				obj.set_bl(0, 4);
			}
			return false;
		}
		if (obj.m_velocity.x < 0)
		{
			obj.m_velocity.x = 0;
			obj.change_way(mario_fx.mf_right);
		}
		if (m_velocity.x > 0)
		{
			m_velocity.x = 0;
			change_way(mario_fx.mf_left);
		}
		px = obj.m_pos.x;
		return true;
	}

	public override bool be_right_hit(mario_obj obj, ref int px)
	{
		if (obj.m_type != mario_type.mt_charater || m_main)
		{
			return false;
		}
		if (obj.m_main)
		{
			if (m_is_guaiwu)
			{
				obj.set_bl(0, 4);
			}
			return false;
		}
		if (obj.m_velocity.x > 0)
		{
			obj.m_velocity.x = 0;
			obj.change_way(mario_fx.mf_left);
		}
		if (m_velocity.x < 0)
		{
			m_velocity.x = 0;
			change_way(mario_fx.mf_right);
		}
		px = obj.m_pos.x;
		return true;
	}

	public override bool be_top_hit(mario_obj obj, ref int py)
	{
		if (obj.m_type != mario_type.mt_charater || m_main)
		{
			return false;
		}
		if (obj.m_main)
		{
			if (m_is_guaiwu)
			{
				obj.set_bl(0, 4);
			}
			return false;
		}
		if (obj.m_velocity.y > 0)
		{
			m_pvelocity.y = obj.m_velocity.y;
			obj.m_velocity.y = 0;
		}
		py = obj.m_pos.y;
		return true;
	}

	public override bool be_bottom_hit(mario_obj obj, ref int py)
	{
		if (obj.m_type != mario_type.mt_charater || m_main)
		{
			return false;
		}
		if (obj.m_main)
		{
			if (m_is_guaiwu)
			{
				obj.set_bl(0, 4);
			}
			return false;
		}
		if (obj.m_velocity.y < 0)
		{
			obj.m_velocity.y = 0;
		}
		py = obj.get_bottom_hit_pos(this);
		return true;
	}

	public override void be_hit(mario_obj obj)
	{
		if (obj.m_main && m_is_guaiwu)
		{
			obj.set_bl(0, 4);
		}
	}

	public override void change_way(mario_fx fx)
	{
		if (m_min_speed != 0)
		{
			if (fx == mario_fx.mf_left)
			{
				m_velocity.x = -m_min_speed;
			}
			else
			{
				m_velocity.x = m_min_speed;
			}
			set_fx(fx);
			change_nl_fx(fx);
		}
	}

	public void check_zhineng()
	{
		for (int i = 0; i < m_bnl_objs.Count; i++)
		{
			if (m_bnl_objs[i].m_type != mario_type.mt_charater && m_pos.x >= m_bnl_objs[i].m_bound.left && m_pos.x <= m_bnl_objs[i].m_bound.right)
			{
				if (m_fx == mario_fx.mf_left && !m_bnl_objs[i].m_nleft && m_bound.left + 10 <= m_bnl_objs[i].m_bound.left && m_bound.right - 10 < m_bnl_objs[i].m_bound.right)
				{
					change_way(mario_fx.mf_right);
				}
				else if (m_fx == mario_fx.mf_right && !m_bnl_objs[i].m_nright && m_bound.right - 10 >= m_bnl_objs[i].m_bound.right && m_bound.left + 10 > m_bnl_objs[i].m_bound.left)
				{
					change_way(mario_fx.mf_left);
				}
			}
		}
	}

	public override void set_bl(int index, int num)
	{
		base.set_bl(index, num);
		if (index == 0 && num == 1)
		{
			m_is_die = true;
			m_velocity.x = 0;
			m_velocity.y = 0;
			m_pos.y += 160;
			base.transform.FindChild("sprite").GetComponent<SpriteRenderer>().sortingOrder = 0;
		}
	}

	public override void tupdate()
	{
		if (m_bl[0] == 1)
		{
			m_chu_time++;
			m_pos.y += 20;
			if (m_chu_time > 24)
			{
				m_chu_time = 0;
				m_bl[0] = 0;
				m_is_die = false;
				base.transform.FindChild("sprite").GetComponent<SpriteRenderer>().sortingOrder = 100;
				reset();
			}
		}
	}
}
