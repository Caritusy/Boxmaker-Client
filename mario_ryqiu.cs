using System.Collections.Generic;
using UnityEngine;

public class mario_ryqiu : mario_attack_ex
{
	private int m_y;

	private int m_up;

	private int m_d_time;

	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_y = m_pos.y;
		if (!m_edit_mode)
		{
			m_pos.y = -2000;
			m_velocity.y = 200;
		}
		m_bkcf = true;
	}

	public override void tupdate()
	{
		m_d_time++;
		if (m_up == 0)
		{
			int num = m_y - m_pos.y;
			if (num <= 0)
			{
				m_up = 1;
				m_d_time = 0;
				m_velocity.y = 0;
				base.transform.FindChild("sprite").localEulerAngles = new Vector3(0f, 0f, 180f);
				if (m_shadow != null)
				{
					m_shadow.transform.FindChild("sprite").localEulerAngles = new Vector3(0f, 0f, 180f);
				}
			}
			else if (num > 2000)
			{
				m_velocity.y = 200;
			}
			else
			{
				m_velocity.y -= 10;
				if (m_velocity.y <= 10)
				{
					m_velocity.y = 10;
				}
			}
		}
		else if (m_up == 1)
		{
			int num2 = -2000 - m_pos.y;
			if (num2 >= 0)
			{
				m_up = 20;
				m_d_time = 0;
				m_velocity.y = 0;
				base.transform.FindChild("sprite").localEulerAngles = new Vector3(0f, 0f, 0f);
				if (m_shadow != null)
				{
					m_shadow.transform.FindChild("sprite").localEulerAngles = new Vector3(0f, 0f, 0f);
				}
			}
			else
			{
				m_velocity.y -= 10;
				if (m_velocity.y <= -200)
				{
					m_velocity.y = -200;
				}
			}
		}
		else if (m_up == 20 && m_d_time >= 100)
		{
			m_up = 0;
			m_d_time = 0;
			m_velocity.y = 200;
		}
	}
}
