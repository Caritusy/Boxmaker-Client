using System.Collections.Generic;

public class mario_ci_block : mario_block
{
	private bool m_hit;

	private int m_die_time;

	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_has_edit = true;
		m_is_dzd = 2;
	}

	public override bool be_left_hit(mario_obj obj, ref int px)
	{
		if (obj.m_main)
		{
			obj.set_bl(0, 4);
			if (!m_hit)
			{
				m_hit = true;
				m_bkcf = true;
			}
		}
		return base.be_left_hit(obj, ref px);
	}

	public override bool be_right_hit(mario_obj obj, ref int px)
	{
		if (obj.m_main)
		{
			obj.set_bl(0, 4);
			if (!m_hit)
			{
				m_hit = true;
				m_bkcf = true;
			}
		}
		return base.be_right_hit(obj, ref px);
	}

	public override bool be_top_hit(mario_obj obj, ref int py)
	{
		if (obj.m_main)
		{
			obj.set_bl(0, 4);
			if (!m_hit)
			{
				m_hit = true;
				m_bkcf = true;
			}
		}
		return base.be_top_hit(obj, ref py);
	}

	public override bool be_bottom_hit(mario_obj obj, ref int py)
	{
		if (obj.m_main)
		{
			obj.set_bl(0, 4);
			if (!m_hit)
			{
				m_hit = true;
				m_bkcf = true;
			}
		}
		return base.be_bottom_hit(obj, ref py);
	}

	public override bool be_left_top_hit(mario_obj obj, ref int px, ref int py)
	{
		if (obj.m_main)
		{
			obj.set_bl(0, 4);
			if (!m_hit)
			{
				m_hit = true;
				m_bkcf = true;
			}
		}
		return base.be_left_top_hit(obj, ref px, ref py);
	}

	public override bool be_right_top_hit(mario_obj obj, ref int px, ref int py)
	{
		if (obj.m_main)
		{
			obj.set_bl(0, 4);
			if (!m_hit)
			{
				m_hit = true;
				m_bkcf = true;
			}
		}
		return base.be_right_top_hit(obj, ref px, ref py);
	}

	public override bool be_left_bottom_hit(mario_obj obj, ref int px, ref int py)
	{
		if (obj.m_main)
		{
			obj.set_bl(0, 4);
			if (!m_hit)
			{
				m_hit = true;
				m_bkcf = true;
			}
		}
		return base.be_left_bottom_hit(obj, ref px, ref py);
	}

	public override bool be_right_bottom_hit(mario_obj obj, ref int px, ref int py)
	{
		if (obj.m_main)
		{
			obj.set_bl(0, 4);
			if (!m_hit)
			{
				m_hit = true;
				m_bkcf = true;
			}
		}
		return base.be_right_bottom_hit(obj, ref px, ref py);
	}

	public override void be_hit(mario_obj obj)
	{
		base.be_hit(obj);
		if (obj.m_name == "mario_huoqiu_big" || (obj.m_name == "mario_luoci" && obj.m_param[0] != 0))
		{
			m_is_die = true;
			m_bkcf = true;
			play_anim("die");
			play_mode._instance.add_score(10);
			mario._instance.play_sound("sound/ding");
		}
	}

	public override void tupdate()
	{
		if (m_is_die)
		{
			m_die_time++;
			if (m_die_time >= 50)
			{
				m_is_destory = 1;
			}
		}
		else if (!m_hit)
		{
			play_anim("stand");
		}
		else
		{
			play_anim("stand1");
		}
	}
}
