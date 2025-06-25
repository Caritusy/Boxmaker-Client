using System.Collections.Generic;

public class mario_ydpt : mario_block1
{
	private bool m_hit;

	private int m_hit_time;

	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_is_dzd = 0;
	}

	public override void reset()
	{
		if (m_param[0] % 2 == 0)
		{
			set_fx(mario_fx.mf_right);
		}
		else
		{
			set_fx(mario_fx.mf_left);
		}
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

	public override bool be_bottom_hit(mario_obj obj, ref int py)
	{
		if (obj.m_type != mario_type.mt_charater)
		{
			return false;
		}
		m_hit = true;
		return base.be_bottom_hit(obj, ref py);
	}

	public override bool be_left_top_hit(mario_obj obj, ref int px, ref int py)
	{
		return false;
	}

	public override bool be_right_top_hit(mario_obj obj, ref int px, ref int py)
	{
		return false;
	}

	public override bool be_left_bottom_hit(mario_obj obj, ref int px, ref int py)
	{
		if (obj.m_type != mario_type.mt_charater)
		{
			return false;
		}
		m_hit = true;
		return base.be_left_bottom_hit(obj, ref px, ref py);
	}

	public override bool be_right_bottom_hit(mario_obj obj, ref int px, ref int py)
	{
		if (obj.m_type != mario_type.mt_charater)
		{
			return false;
		}
		m_hit = true;
		return base.be_right_bottom_hit(obj, ref px, ref py);
	}

	public override mario_point move()
	{
		mario_point mario_point2 = new mario_point();
		if (m_hit)
		{
			m_hit_time++;
			if (m_hit_time < 5)
			{
				return mario_point2;
			}
			if (m_param[0] % 2 == 0)
			{
				mario_point2.x = (m_param[0] + 1) * 50;
				m_pos.x += (m_param[0] + 1) * 50;
			}
			else
			{
				mario_point2.x = (m_param[0] + 1) * -50;
				m_pos.x -= (m_param[0] + 1) * 50;
			}
		}
		return mario_point2;
	}

	public override void change()
	{
		if (m_param[0] < 10)
		{
			m_param[0] += 1;
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
}
