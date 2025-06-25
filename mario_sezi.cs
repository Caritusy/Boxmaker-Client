using System.Collections.Generic;
using UnityEngine;

public class mario_sezi : mario_block
{
	private bool m_hit;

	private int m_hit_time;

	private int m_num = 1;

	public GameObject m_s;

	private GameObject m_edit_obj;

	public override void reset()
	{
		try
		{ 
			mario._instance.remove_child(m_s);
			if (m_shadow != null)
			{
				mario._instance.remove_child(m_shadow.GetComponent<mario_sezi>().m_s);
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

	public override bool be_top_hit(mario_obj obj, ref int py)
	{
		if (obj.m_main && !m_hit)
		{
			m_hit = true;
			dingchu();
		}
		return base.be_top_hit(obj, ref py);
	}

	private void dingchu()
	{
		m_bkcf = true;
		if (m_param[0] == 0)
		{
			play_anim("hit");
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

	public override void tupdate()
	{
		if (!m_hit)
		{
			m_num++;
			if (m_num > 6)
			{
				m_num = 1;
			}
			play_anim("stand");
			return;
		}
		m_hit_time++;
		if (m_hit_time == 20)
		{
			play_mode._instance.add_score(m_pos.x, m_pos.y, 500 * m_num);
			base.transform.FindChild("sprite").GetComponent<SpriteRenderer>().sortingOrder = 0;
			play_anim(m_num.ToString());
		}
	}
}
