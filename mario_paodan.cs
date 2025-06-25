using System;
using UnityEngine;

public class mario_paodan : mario_attack_ex
{
	private mario_point m_dir = new mario_point();

	private mario_point m_yz = new mario_point();

	public override void reset()
	{
		m_dir.x = m_velocity.x;
		m_dir.y = m_velocity.y;
		m_yz.x = m_pos.x * 100;
		m_yz.y = m_pos.y * 100;
		set_vr();
	}

	public override bool be_bottom_hit(mario_obj obj, ref int py)
	{
		if (obj.m_main)
		{
			obj.m_pvelocity.y = 150;
			obj.m_velocity.y = 0;
			play_mode._instance.caisi(0, tx: true, obj.m_pos.x, obj.m_bound.bottom);
			m_is_die = true;
			play_mode._instance.add_score(m_pos.x, m_pos.y, 100);
			mario._instance.play_sound("sound/caisi");
		}
		return false;
	}

	private void set_vr()
	{
		int num = (int)Mathf.Sqrt(m_dir.x * m_dir.x * 100 + m_dir.y * m_dir.y * 100);
		if (num != 0)
		{
			m_yz.x += m_dir.x * 4000 / num * 10;
			m_yz.y += m_dir.y * 4000 / num * 10;
			m_velocity.x = m_yz.x / 100 - m_pos.x;
			m_velocity.y = m_yz.y / 100 - m_pos.y;
			float z = Mathf.Atan2(m_dir.y, m_dir.x) * 180f / (float)Math.PI;
			set_angles(0f, 0f, z);
		}
	}

	public override void tupdate()
	{
		if (m_is_die)
		{
			m_velocity.x = 0;
			m_velocity.y -= utils.g_g;
			return;
		}
		if (play_mode._instance.m_main_char != null)
		{
			int x = play_mode._instance.m_main_char.m_pos.x;
			int y = play_mode._instance.m_main_char.m_pos.y;
			x -= m_pos.x;
			y -= m_pos.y;
			int num = utils.atan(x, y);
			int num2 = utils.atan(m_dir.x, m_dir.y);
			if (num < num2)
			{
				int num3 = num2 - num;
				num2 = ((num3 >= 180) ? (num2 + 1) : (num2 - 1));
			}
			else
			{
				int num4 = num - num2;
				num2 = ((num4 >= 180) ? (num2 - 1) : (num2 + 1));
			}
			num2 = (num2 + 360) % 360;
			m_dir = utils.tan(num2);
		}
		set_vr();
	}
}
