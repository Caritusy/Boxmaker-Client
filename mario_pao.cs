using System.Collections.Generic;
using UnityEngine;

public class mario_pao : mario_block
{
	private int m_z;

	private mario_obj m_pd;

	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_bkcf = true;
	}

	public override void tupdate()
	{
		if (play_mode._instance.m_main_char == null)
		{
			return;
		}
		if (play_mode._instance.m_time % 250 == 100 && m_pd == null)
		{
			mario._instance.play_sound("sound/pao");
			play_anim("pao");
			List<int> list = new List<int>();
			list.Add(0);
			list.Add(0);
			list.Add(0);
			list.Add(0);
			if (m_z == 0)
			{
				m_pd = play_mode._instance.create_mario_obj_ex("mario_paodan", null, list, -1, -1, m_pos.x + 640, m_pos.y);
				m_pd.m_velocity.set(200, 0);
			}
			else if (m_z == 45)
			{
				m_pd = play_mode._instance.create_mario_obj_ex("mario_paodan", null, list, -1, -1, m_pos.x + 450, m_pos.y + 450);
				m_pd.m_velocity.set(100, 100);
			}
			else if (m_z == 90)
			{
				m_pd = play_mode._instance.create_mario_obj_ex("mario_paodan", null, list, -1, -1, m_pos.x, m_pos.y + 640);
				m_pd.m_velocity.set(0, 200);
			}
			else if (m_z == 135)
			{
				m_pd = play_mode._instance.create_mario_obj_ex("mario_paodan", null, list, -1, -1, m_pos.x - 450, m_pos.y + 450);
				m_pd.m_velocity.set(-100, 100);
			}
			else if (m_z == 180)
			{
				m_pd = play_mode._instance.create_mario_obj_ex("mario_paodan", null, list, -1, -1, m_pos.x - 640, m_pos.y);
				m_pd.m_velocity.set(-200, 0);
			}
			else if (m_z == 225)
			{
				m_pd = play_mode._instance.create_mario_obj_ex("mario_paodan", null, list, -1, -1, m_pos.x - 450, m_pos.y - 450);
				m_pd.m_velocity.set(-100, -100);
			}
			else if (m_z == 270)
			{
				m_pd = play_mode._instance.create_mario_obj_ex("mario_paodan", null, list, -1, -1, m_pos.x, m_pos.y - 640);
				m_pd.m_velocity.set(0, -200);
			}
			else
			{
				m_pd = play_mode._instance.create_mario_obj_ex("mario_paodan", null, list, -1, -1, m_pos.x + 450, m_pos.y - 450);
				m_pd.m_velocity.set(100, -100);
			}
			m_pd.reset();
		}
		int num = play_mode._instance.m_main_char.m_pos.x - m_pos.x;
		int num2 = play_mode._instance.m_main_char.m_pos.y - m_pos.y;
		if (num < 0)
		{
			if (num2 > 0)
			{
				if (num2 * 100 < -num * 41)
				{
					m_z = 180;
				}
				else if (num2 * 100 < -num * 241)
				{
					m_z = 135;
				}
				else
				{
					m_z = 90;
				}
			}
			else if (-num2 * 100 < -num * 41)
			{
				m_z = 180;
			}
			else if (-num2 * 100 < -num * 241)
			{
				m_z = 225;
			}
			else
			{
				m_z = 270;
			}
		}
		else if (num2 > 0)
		{
			if (num2 * 100 < num * 41)
			{
				m_z = 0;
			}
			else if (num2 * 100 < num * 241)
			{
				m_z = 45;
			}
			else
			{
				m_z = 90;
			}
		}
		else if (-num2 * 100 < num * 41)
		{
			m_z = 0;
		}
		else if (-num2 * 100 < num * 241)
		{
			m_z = 315;
		}
		else
		{
			m_z = 270;
		}
		base.transform.FindChild("pao").localEulerAngles = new Vector3(0f, 0f, m_z);
		if (m_shadow != null)
		{
			m_shadow.transform.FindChild("pao").localEulerAngles = new Vector3(0f, 0f, m_z);
		}
	}
}
