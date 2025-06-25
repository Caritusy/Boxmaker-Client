using UnityEngine;

public class mario_huoqiu : mario_attack_ex1
{
	private mario_point m_dir = new mario_point();

	private mario_point m_yz = new mario_point();

	private int m_die_time;

	public override void reset()
	{
		if (m_velocity.x != 0 || m_velocity.y != 0)
		{
			if (m_pos.x == m_velocity.x && m_pos.y == m_velocity.y)
			{
				m_velocity.x = m_pos.x + 1;
				m_velocity.y = m_pos.y + 1;
			}
			m_dir.x = m_velocity.x - m_pos.x;
			m_dir.y = m_velocity.y - m_pos.y;
			if (m_dir.x > 10000 || m_dir.x < -10000 || m_dir.y > 10000 || m_dir.y < -10000)
			{
				m_is_destory = 1;
				return;
			}
			int num = (int)Mathf.Sqrt(m_dir.x * m_dir.x + m_dir.y * m_dir.y);
			m_dir.x = m_dir.x * 4000 / num;
			m_dir.y = m_dir.y * 4000 / num;
			m_yz.x = m_pos.x * 100;
			m_yz.y = m_pos.y * 100;
			m_velocity = new mario_point();
		}
	}

	public override void set_bl(int index, int num)
	{
		if (index == 0 && num == 4)
		{
			m_is_die = true;
		}
		if (index == 0 && num == 5)
		{
			m_is_die = true;
		}
	}

	public override void tupdate()
	{
		if (m_is_die)
		{
			if (m_die_time == 0)
			{
				play_anim("die");
				m_velocity.x = 0;
				m_velocity.y = 0;
			}
			else if (m_die_time > 20)
			{
				m_is_destory = 1;
			}
			m_die_time++;
		}
		else
		{
			m_yz.x += m_dir.x;
			m_yz.y += m_dir.y;
			m_velocity.x = m_yz.x / 100 - m_pos.x;
			m_velocity.y = m_yz.y / 100 - m_pos.y;
		}
	}
}
