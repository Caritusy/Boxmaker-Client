using System.Collections.Generic;
using UnityEngine;

public class mario_huogun_ex : mario_attack_ex
{
	public List<GameObject> m_s;

	private int m_d;

	public override void reset()
	{
		for (int i = 6; i < m_s.Count; i++)
		{
			if (i - 6 < m_param[0] / 2)
			{
				m_s[i].SetActive(value: true);
				if (m_shadow != null)
				{
					m_shadow.GetComponent<mario_huogun_ex>().m_s[i].SetActive(value: true);
				}
			}
			else
			{
				m_s[i].SetActive(value: false);
				if (m_shadow != null)
				{
					m_shadow.GetComponent<mario_huogun_ex>().m_s[i].SetActive(value: false);
				}
			}
		}
		m_d = -m_param[1] * 45;
		refresh(m_d);
	}

	public override void change()
	{
		if (m_param[0] < 7)
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

	public override void change1()
	{
		if (m_param[1] < 7)
		{
			List<int> param;
			List<int> list = (param = m_param);
			int index;
			int index2 = (index = 1);
			index = param[index];
			list[index2] = index + 1;
		}
		else
		{
			m_param[1] = 0;
		}
		if (m_unit != null)
		{
			game_data._instance.m_arrays[m_world][m_init_pos.y][m_init_pos.x].param[1] = m_param[1];
		}
		reset();
	}

	private void refresh(int d)
	{
		mario_point mario_point2 = utils.get_rxy(d);
		for (int i = 0; i < m_s.Count; i++)
		{
			m_s[i].transform.localPosition = new Vector3(mario_point2.x * i * 32 / 1000, mario_point2.y * i * 32 / 1000);
			m_s[i].transform.localEulerAngles = new Vector3(0f, 0f, -d * 20);
		}
		if (m_shadow != null)
		{
			for (int j = 0; j < m_s.Count; j++)
			{
				m_shadow.GetComponent<mario_huogun_ex>().m_s[j].transform.localPosition = new Vector3(mario_point2.x * j * 32 / 1000, mario_point2.y * j * 32 / 1000);
				m_shadow.GetComponent<mario_huogun_ex>().m_s[j].transform.localEulerAngles = new Vector3(0f, 0f, -d * 20);
			}
		}
	}

	public override void be_hit(mario_obj obj)
	{
		if (!obj.m_main)
		{
			return;
		}
		bool flag = false;
		mario_point mario_point2 = utils.get_rxy(m_d);
		for (int i = 0; i < m_s.Count; i++)
		{
			int num = mario_point2.x * i * 320 / 1000 - 100 + m_pos.x;
			int num2 = mario_point2.x * i * 320 / 1000 + 100 + m_pos.x;
			int num3 = mario_point2.y * i * 320 / 1000 + 100 + m_pos.y;
			int num4 = mario_point2.y * i * 320 / 1000 - 100 + m_pos.y;
			if (num2 >= obj.m_bound.left && num <= obj.m_bound.right && num3 >= obj.m_bound.bottom && num4 <= obj.m_bound.top)
			{
				if (i - 6 < m_param[0] / 2)
				{
					flag = true;
				}
				break;
			}
		}
		if (flag)
		{
			obj.set_bl(0, 4);
		}
	}

	public override void tupdate()
	{
		m_d = (-m_param[1] * 45 + play_mode._instance.m_time) % 360;
		if (m_param[0] % 2 == 0)
		{
			m_d = (-m_param[1] * 45 - play_mode._instance.m_time) % 360;
		}
		refresh(m_d);
	}
}
