using System.Collections.Generic;
using UnityEngine;

public class mario_yinfu : mario_block
{
	public GameObject m_sp;

	public List<GameObject> m_ss = new List<GameObject>();

	public override void reset()
	{
		try
		{ 
			if (m_edit_mode)
			{
				m_sp.SetActive(value: false);
				for (int i = 0; i < m_ss.Count; i++)
				{
					m_ss[i].SetActive(value: false);
				}
				m_ss[m_param[0]].SetActive(value: true);
			}
		}
		catch (System.Exception e)
		{
			Debug.LogError(e);
			mario._instance.show_tip($"触发了反崩编辑器机制，异常于：{m_name}({this.m_init_pos.x},{this.m_init_pos.y})");
		}
	}

	public override bool be_bottom_hit(mario_obj obj, ref int py)
	{
		if (obj.m_type != mario_type.mt_charater)
		{
			return false;
		}
		if (obj.m_main)
		{
			play_mode._instance.caisi(80);
			obj.m_pvelocity.y = 240;
		}
		else
		{
			obj.m_pvelocity.y = 300;
		}
		if (obj.m_velocity.y < 0)
		{
			obj.m_velocity.y = 0;
		}
		py = obj.get_bottom_hit_pos(this);
		play_anim("hit");
		play_yinfu();
		return true;
	}

	public override bool be_left_bottom_hit(mario_obj obj, ref int px, ref int py)
	{
		if (obj.m_type != mario_type.mt_charater)
		{
			return false;
		}
		if (obj.m_main)
		{
			play_mode._instance.caisi(80);
			obj.m_pvelocity.y = 240;
		}
		else
		{
			obj.m_pvelocity.y = 300;
		}
		if (obj.m_velocity.y < 0)
		{
			obj.m_velocity.y = 0;
		}
		px = obj.m_pos.x;
		py = obj.get_bottom_hit_pos(this);
		play_anim("hit");
		play_yinfu();
		return true;
	}

	public override bool be_right_bottom_hit(mario_obj obj, ref int px, ref int py)
	{
		if (obj.m_type != mario_type.mt_charater)
		{
			return false;
		}
		if (obj.m_main)
		{
			play_mode._instance.caisi(80);
			obj.m_pvelocity.y = 240;
		}
		else
		{
			obj.m_pvelocity.y = 300;
		}
		if (obj.m_velocity.y < 0)
		{
			obj.m_velocity.y = 0;
		}
		px = obj.m_pos.x;
		py = obj.get_bottom_hit_pos(this);
		play_anim("hit");
		play_yinfu();
		return true;
	}

	public override void change()
	{
		if (m_param[0] < 4)
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
		play_yinfu();
		reset();
	}

	private void play_yinfu()
	{
		mario._instance.play_sound("sound/yf/" + m_param[0] + "-" + (m_init_pos.y + 1));
	}
}
