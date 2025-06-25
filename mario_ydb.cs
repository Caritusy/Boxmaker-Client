using System.Collections.Generic;
using UnityEngine;

public class mario_ydb : mario_block1
{
	public List<GameObject> m_s;

	private int m_time;

	private int m_state;

	private mario_point m_p = new mario_point();

	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_is_dzd = 0;
	}

	public override void reset()
	{
		try
		{
			for (int i = 0; i < m_s.Count; i++)
			{
				m_s[i].SetActive(value: false);
			}
			m_s[m_param[0]].SetActive(value: true);
			m_time = 100;
			m_p.set(m_pos.x * 2, m_pos.y * 2);
		}
		catch (System.Exception ex)
		{
            mario._instance.show_tip($"触发了反崩编辑器机制，异常于：{m_name}({this.m_init_pos.x},{this.m_init_pos.y})");
            Debug.Log(ex);
        }
	}

	public override bool be_left_hit(mario_obj obj, ref int px)
	{
		return false;
	}

	public override bool be_right_hit(mario_obj obj, ref int px)
	{
		return false;
	}

	public override bool be_top_hit(mario_obj obj, ref int py)
	{
		return false;
	}

	public override bool be_bottom_hit(mario_obj obj, ref int py)
	{
		if (m_param[0] == 2 && obj.m_main)
		{
			m_state = 1;
		}
		return base.be_bottom_hit(obj, ref py);
	}

	public override bool be_left_top_hit(mario_obj obj, ref int px, ref int py)
	{
		return false;
	}

	public override bool be_right_top_hit(mario_obj obj, ref int px, ref int py)
	{
		return false;
	}

	public override bool be_left_bottom_hit(mario_obj obj, ref int px, ref int py)
	{
		if (m_param[0] == 2 && obj.m_main)
		{
			m_state = 1;
		}
		return base.be_left_bottom_hit(obj, ref px, ref py);
	}

	public override bool be_right_bottom_hit(mario_obj obj, ref int px, ref int py)
	{
		if (m_param[0] == 2 && obj.m_main)
		{
			m_state = 1;
		}
		return base.be_right_bottom_hit(obj, ref px, ref py);
	}

	public override mario_point move()
	{
		mario_point mario_point2 = new mario_point();
		if (m_param[0] == 2)
		{
			if (m_state == 1)
			{
				mario_point2.y = -50;
				m_pos.y -= 50;
			}
		}
		else
		{
			int num = 60;
			if (m_time < 60)
			{
				num = m_time;
			}
			else if (m_time > 140)
			{
				num = 200 - m_time;
			}
			if (m_state == 0)
			{
				num = -num;
			}
			m_time++;
			if (m_time >= 200)
			{
				m_time = 0;
				m_state = 1 - m_state;
			}
			if (m_param[0] == 0)
			{
				m_p.x += num;
				int num2 = (mario_point2.x = m_p.x / 2 - m_pos.x);
				m_pos.x += num2;
			}
			else
			{
				m_p.y += num;
				int num3 = (mario_point2.y = m_p.y / 2 - m_pos.y);
				m_pos.y += num3;
			}
		}
		return mario_point2;
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
