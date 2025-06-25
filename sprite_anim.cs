using System.Collections.Generic;
using UnityEngine;

public class sprite_anim : MonoBehaviour
{
	public List<sprite_anim_sub> m_subs;

	private int m_play_index = -1;

	private string m_s = string.Empty;

	private string m_name = string.Empty;

	private int m_time;

	private bool m_pause;

	public bool has_anim(string name)
	{
		for (int i = 0; i < m_subs.Count; i++)
		{
			if (m_subs[i].name == name)
			{
				return true;
			}
		}
		return false;
	}

	public string get_name()
	{
		return m_name;
	}

	public void play(string name)
	{
		int num = -1;
		for (int i = 0; i < m_subs.Count; i++)
		{
			if (m_subs[i].name == name)
			{
				num = i;
				break;
			}
		}
		if (num != -1)
		{
			play_index(num);
		}
	}

	public void play_index(int index)
	{
		if (index >= 0 && index < m_subs.Count && index != m_play_index)
		{
			if (m_play_index != -1)
			{
				m_subs[m_play_index].render.spriteName = m_s;
			}
			m_play_index = index;
			m_s = m_subs[m_play_index].render.spriteName;
			m_subs[m_play_index].render.spriteName = m_subs[m_play_index].ss[0];
			m_time = 0;
			m_name = m_subs[m_play_index].render.name;
		}
	}

	public void pause()
	{
		m_pause = true;
	}

	public void conti()
	{
		m_pause = false;
	}

	private void FixedUpdate()
	{
		if (m_play_index == -1 || m_pause)
		{
			return;
		}
		int speed = m_subs[m_play_index].speed;
		int num = m_time * speed / 50;
		m_time++;
		int num2 = m_time * speed / 50;
		bool flag = false;
		if (m_subs[m_play_index].once)
		{
			if (num > m_subs[m_play_index].ss.Count - 1)
			{
				num = m_subs[m_play_index].ss.Count - 1;
			}
			if (num2 > m_subs[m_play_index].ss.Count - 1)
			{
				flag = true;
				num2 = m_subs[m_play_index].ss.Count - 1;
			}
		}
		else
		{
			num %= m_subs[m_play_index].ss.Count;
			num2 %= m_subs[m_play_index].ss.Count;
		}
		if (num != num2)
		{
			m_subs[m_play_index].render.spriteName = m_subs[m_play_index].ss[num2];
		}
		if (flag && m_subs[m_play_index].link != string.Empty)
		{
			play(m_subs[m_play_index].link);
		}
	}
}
