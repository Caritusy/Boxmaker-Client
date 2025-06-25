using System.Collections.Generic;

public class mario_men : mario_block
{
	private bool m_hit;

	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
	}

	public override bool be_left_hit(mario_obj obj, ref int px)
	{
		if (obj.m_type != mario_type.mt_charater)
		{
			return false;
		}
		check(obj);
		if (m_hit)
		{
			return false;
		}
		return base.be_left_hit(obj, ref px);
	}

	public override bool be_right_hit(mario_obj obj, ref int px)
	{
		if (obj.m_type != mario_type.mt_charater)
		{
			return false;
		}
		check(obj);
		if (m_hit)
		{
			return false;
		}
		return base.be_right_hit(obj, ref px);
	}

	public override bool be_top_hit(mario_obj obj, ref int py)
	{
		if (obj.m_type != mario_type.mt_charater)
		{
			return false;
		}
		check(obj);
		if (m_hit)
		{
			return false;
		}
		return base.be_top_hit(obj, ref py);
	}

	public override bool be_bottom_hit(mario_obj obj, ref int py)
	{
		if (obj.m_type != mario_type.mt_charater)
		{
			return false;
		}
		check(obj);
		if (m_hit)
		{
			return false;
		}
		return base.be_bottom_hit(obj, ref py);
	}

	public override bool be_left_top_hit(mario_obj obj, ref int px, ref int py)
	{
		if (obj.m_type != mario_type.mt_charater)
		{
			return false;
		}
		check(obj);
		if (m_hit)
		{
			return false;
		}
		return base.be_left_top_hit(obj, ref px, ref py);
	}

	public override bool be_right_top_hit(mario_obj obj, ref int px, ref int py)
	{
		if (obj.m_type != mario_type.mt_charater)
		{
			return false;
		}
		check(obj);
		if (m_hit)
		{
			return false;
		}
		return base.be_right_top_hit(obj, ref px, ref py);
	}

	public override bool be_left_bottom_hit(mario_obj obj, ref int px, ref int py)
	{
		if (obj.m_type != mario_type.mt_charater)
		{
			return false;
		}
		check(obj);
		if (m_hit)
		{
			return false;
		}
		return base.be_left_bottom_hit(obj, ref px, ref py);
	}

	public override bool be_right_bottom_hit(mario_obj obj, ref int px, ref int py)
	{
		if (obj.m_type != mario_type.mt_charater)
		{
			return false;
		}
		check(obj);
		if (m_hit)
		{
			return false;
		}
		return base.be_right_bottom_hit(obj, ref px, ref py);
	}

	public void check(mario_obj obj)
	{
		if (obj.m_main && !m_hit && play_mode._instance.m_ys > 0)
		{
			play_mode._instance.m_ys--;
			m_hit = true;
			m_is_static = false;
			m_has_floor = false;
			m_is_dzd = 0;
			m_bkcf = true;
			mario._instance.play_sound("sound/men");
		}
	}

	public override void tupdate()
	{
		if (!m_hit)
		{
			play_anim("stand");
		}
		else
		{
			play_anim("stand1");
		}
	}
}
