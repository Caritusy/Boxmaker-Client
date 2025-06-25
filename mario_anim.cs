using System.Collections.Generic;
using UnityEngine;

public class mario_anim : MonoBehaviour
{
	public List<mario_anim_sub> m_subs;

	private int m_play_index = -1;

	private Sprite m_s;

	private string m_name = string.Empty;

	private int m_time;

	private int m_z;

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

	public bool do_defualt()
	{
		int num = -1;
		for (int i = 0; i < m_subs.Count; i++)
		{
			if (m_subs[i].defualt)
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			return false;
		}
		play_index(num);
		return true;
	}

	public void play(string name, int speed = -1)
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
			if (speed != -1)
			{
				m_subs[m_play_index].speed = speed;
			}
		}
	}

	public void play_index(int index)
	{
		if (index >= 0 && index < m_subs.Count && index != m_play_index)
		{
			if (m_play_index != -1)
			{
				m_subs[m_play_index].render.sprite = m_s;
			}
			m_play_index = index;
			m_s = m_subs[m_play_index].render.sprite;
			m_subs[m_play_index].render.sprite = m_subs[m_play_index].ss[0];
			m_time = 0;
			m_z = 0;
			m_name = m_subs[m_play_index].name;
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

	public Sprite get_sprite()
	{
		return m_subs[m_play_index].render.sprite;
	}

	private void FixedUpdate()
	{
		if (m_play_index == -1 || m_pause)
		{
			return;
		}
		int speed = m_subs[m_play_index].speed;
		m_time += speed;
		int z = m_z;
		m_z += m_time / 50;
		bool flag = false;
		if (m_subs[m_play_index].once)
		{
			if (m_z > m_subs[m_play_index].ss.Count - 1)
			{
				flag = true;
				m_z = m_subs[m_play_index].ss.Count - 1;
			}
		}
		else
		{
			m_z %= m_subs[m_play_index].ss.Count;
		}
		if (z != m_z)
		{
			m_time %= 50;
			m_subs[m_play_index].render.sprite = m_subs[m_play_index].ss[m_z];
		}
		if (flag && m_subs[m_play_index].link != string.Empty)
		{
			play(m_subs[m_play_index].link);
		}
	}
}
