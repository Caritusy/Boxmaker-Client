using System.Collections.Generic;

public class mario_down_block : mario_block
{
	private int m_time;

	private bool m_state;

	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_is_static = false;
		m_is_dzd = 0;
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

	public override bool be_left_top_hit(mario_obj obj, ref int px, ref int py)
	{
		return false;
	}

	public override bool be_right_top_hit(mario_obj obj, ref int px, ref int py)
	{
		return false;
	}

	public override mario_point move()
	{
		mario_point mario_point2 = new mario_point();
		if (m_state)
		{
			mario_point2.y = -50;
			m_pos.y -= 50;
		}
		return mario_point2;
	}

	public override void tupdate()
	{
		bool flag = false;
		if (!m_state)
		{
			for (int i = 0; i < m_nl_objs.Count; i++)
			{
				if (m_nl_objs[i].m_main)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				m_time++;
				if (m_time >= 50)
				{
					m_state = true;
					m_is_static = false;
					m_bkcf = true;
				}
			}
			else if (m_time > 0)
			{
				m_time--;
			}
		}
		if (flag)
		{
			play_anim("dou");
		}
		else if (m_state)
		{
			play_anim("down");
		}
		else
		{
			play_anim("stand");
		}
	}
}
