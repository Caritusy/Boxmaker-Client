using System.Collections.Generic;
using UnityEngine;

public class mario_chilun : mario_attack_ex
{
	private int m_db;

	public override void reset()
	{
		if (m_param[0] == 0)
		{
			m_collider.m_rect.h = 960;
			m_collider.m_rect.w = 960;
			m_db = 650;
			set_scale(0.5f, 0.5f, 1f);
		}
		else if (m_param[0] == 1)
		{
			m_collider.m_rect.h = 1920;
			m_collider.m_rect.w = 1920;
			m_db = 1100;
			set_scale(1f, 1f, 1f);
		}
		else if (m_param[0] == 2)
		{
			m_collider.m_rect.h = 3840;
			m_collider.m_rect.w = 3840;
			m_db = 2000;
			set_scale(2f, 2f, 1f);
		}
		else
		{
			m_collider.m_rect.h = 480;
			m_collider.m_rect.w = 480;
			m_db = 325;
			set_scale(0.25f, 0.25f, 1f);
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

	public override void be_hit(mario_obj obj)
	{
		if (obj.m_main)
		{
			int num = (int)Mathf.Sqrt((m_pos.x - obj.m_pos.x) * (m_pos.x - obj.m_pos.x) + (m_pos.y - obj.m_pos.y) * (m_pos.y - obj.m_pos.y));
			if (num < m_db)
			{
				obj.set_bl(0, 4);
			}
		}
	}
}
