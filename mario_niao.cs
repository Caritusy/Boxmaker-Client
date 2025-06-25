using System.Collections.Generic;

public class mario_niao : mario_attack_ex
{
	private int m_die_time;

	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_has_edit = true;
		m_bkcf = true;
	}

	public override void reset()
	{
		if (m_param[0] == 0)
		{
			set_fx(mario_fx.mf_left);
			m_velocity.x = -40;
		}
		else
		{
			set_fx(mario_fx.mf_right);
			m_velocity.x = 40;
		}
	}

	public override bool be_bottom_hit(mario_obj obj, ref int py)
	{
		if (obj.m_main)
		{
			obj.m_pvelocity.y = 150;
			obj.m_velocity.y = 0;
			play_mode._instance.caisi(0, tx: true, obj.m_pos.x, obj.m_bound.bottom);
			m_is_die = true;
			play_mode._instance.add_score(m_pos.x, m_pos.y, 200);
			mario._instance.play_sound("sound/caisi");
		}
		return false;
	}

	public override void change()
	{
		if (m_param[0] == 0)
		{
			m_param[0] = 1;
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

	public override void tupdate()
	{
		if (m_is_die)
		{
			if (m_die_time == 0)
			{
				play_anim("die");
			}
			if (m_die_time > 20)
			{
				m_velocity.y -= utils.g_g;
			}
			else
			{
				m_velocity.x = 0;
				m_velocity.y = 0;
			}
			m_die_time++;
		}
		else
		{
			play_anim("stand");
		}
	}
}
