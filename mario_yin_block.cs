using System.Collections.Generic;
using UnityEngine;

public class mario_yin_block : mario_block
{
	private bool m_hit;

	private int m_hit_time;

	public GameObject m_s;

	private GameObject m_edit_obj;

	private int m_die_time;

	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_has_floor = false;
		m_has_edit = true;
		m_fxdiv = 1;
		m_is_static = false;
		m_is_dzd = 0;
	}

	public override void reset()
	{
		try
		{ 
			mario._instance.remove_child(m_s);
			if (m_shadow != null)
			{
				mario._instance.remove_child(m_shadow.GetComponent<mario_yin_block>().m_s);
			}
			m_edit_obj = null;
			if (m_param[0] != 0 && m_edit_mode)
			{
				s_t_unit s_t_unit2 = game_data._instance.get_t_unit(m_param[0]);
				string path = "unit/" + s_t_unit2.res + "/" + s_t_unit2.res;
				GameObject original = (GameObject)Resources.Load(path);
				m_edit_obj = (GameObject)Object.Instantiate(original);
				m_edit_obj.transform.parent = m_s.transform;
				m_edit_obj.transform.localScale = new Vector3(1f, 1f, 1f);
				mario_obj component = m_edit_obj.GetComponent<mario_obj>();
				component.m_edit_mode = true;
				List<int> list = new List<int>();
				list.Add(m_param[1]);
				list.Add(m_param[2]);
				list.Add(m_param[3]);
				list.Add(0);
				component.init(s_t_unit2.res, list, m_world, -1, -1, 0, 0);
			}
		}
		catch (System.Exception e)
		{
			Debug.LogError(e);
			mario._instance.show_tip($"触发了反崩编辑器机制，异常于：{m_name}({this.m_init_pos.x},{this.m_init_pos.y})");
		}
	}

	public override bool be_left_hit(mario_obj obj, ref int px)
	{
		if (obj.m_wgk && !m_hit)
		{
			dingchu();
		}
		if (!m_hit)
		{
			return false;
		}
		return base.be_left_hit(obj, ref px);
	}

	public override bool be_right_hit(mario_obj obj, ref int px)
	{
		if (obj.m_wgk && !m_hit)
		{
			dingchu();
		}
		if (!m_hit)
		{
			return false;
		}
		return base.be_right_hit(obj, ref px);
	}

	public override bool be_top_hit(mario_obj obj, ref int py)
	{
		if (obj.m_type != mario_type.mt_charater)
		{
			return false;
		}
		if (obj.m_main && !m_hit)
		{
			dingchu();
			for (int i = 0; i < m_nl_objs.Count; i++)
			{
				mario_obj mario_obj2 = m_nl_objs[i];
				if (mario_obj2.m_life == 1 || mario_obj2.m_wgk)
				{
					mario_obj2.set_bl(0, 4);
				}
				else
				{
					mario_obj2.m_pvelocity.y = 250;
				}
			}
		}
		if (!m_hit)
		{
			return false;
		}
		return base.be_top_hit(obj, ref py);
	}

	public override bool be_bottom_hit(mario_obj obj, ref int py)
	{
		if (!m_hit)
		{
			return false;
		}
		return base.be_bottom_hit(obj, ref py);
	}

	public override bool be_left_top_hit(mario_obj obj, ref int px, ref int py)
	{
		if (!m_hit)
		{
			return false;
		}
		return base.be_left_top_hit(obj, ref px, ref py);
	}

	public override bool be_right_top_hit(mario_obj obj, ref int px, ref int py)
	{
		if (!m_hit)
		{
			return false;
		}
		return base.be_right_top_hit(obj, ref px, ref py);
	}

	public override bool be_left_bottom_hit(mario_obj obj, ref int px, ref int py)
	{
		if (!m_hit)
		{
			return false;
		}
		return base.be_left_bottom_hit(obj, ref px, ref py);
	}

	public override bool be_right_bottom_hit(mario_obj obj, ref int px, ref int py)
	{
		if (!m_hit)
		{
			return false;
		}
		return base.be_right_bottom_hit(obj, ref px, ref py);
	}

	public override void be_hit(mario_obj obj)
	{
		if (m_hit)
		{
			base.be_hit(obj);
			if (obj.m_name == "mario_huoqiu_big" || (obj.m_name == "mario_luoci" && obj.m_param[0] != 0))
			{
				m_is_die = true;
				m_bkcf = true;
				play_anim("die");
				play_mode._instance.add_score(10);
				mario._instance.play_sound("sound/ding");
			}
		}
	}

	private void dingchu()
	{
		m_hit = true;
		m_is_static = true;
		m_has_floor = true;
		m_is_dzd = 2;
		m_fxdiv = 2;
		m_bkcf = true;
		if (m_param[0] == 0)
		{
			play_anim("hit1");
			mario._instance.play_sound("sound/coins");
		}
		else
		{
			s_t_unit s_t_unit2 = game_data._instance.get_t_unit(m_param[0]);
			play_anim("hit");
			mario._instance.play_sound("sound/dinfo");
			List<int> list = new List<int>();
			list.Add(m_param[1]);
			list.Add(m_param[2]);
			list.Add(m_param[3]);
			list.Add(0);
			mario_obj mario_obj2 = play_mode._instance.create_mario_obj(s_t_unit2.res, null, list, m_init_pos.x, m_init_pos.y);
			mario_obj2.set_bl(0, 1);
		}
		base.transform.FindChild("sprite").GetComponent<SpriteRenderer>().sortingOrder = 1;
	}

	public override void change()
	{
		if (!(m_edit_obj == null))
		{
			m_edit_obj.GetComponent<mario_obj>().change();
			for (int i = 0; i < 3; i++)
			{
				m_param[i + 1] = m_edit_obj.GetComponent<mario_obj>().m_param[i];
				game_data._instance.m_arrays[m_world][m_init_pos.y][m_init_pos.x].param[i + 1] = m_edit_obj.GetComponent<mario_obj>().m_param[i];
			}
		}
	}

	public override void tupdate()
	{
		if (m_is_die)
		{
			m_die_time++;
			if (m_die_time >= 50)
			{
				m_is_destory = 1;
			}
		}
		else if (m_hit)
		{
			m_hit_time++;
			if (m_hit_time == 20)
			{
				if (m_param[0] == 0)
				{
					play_mode._instance.add_score(m_pos.x, m_pos.y + 100, 100);
				}
				base.transform.FindChild("sprite").GetComponent<SpriteRenderer>().sortingOrder = 0;
			}
			else if (m_hit_time >= 30)
			{
				play_anim("stand");
			}
		}
		else
		{
			play_anim("yin");
		}
	}
}
