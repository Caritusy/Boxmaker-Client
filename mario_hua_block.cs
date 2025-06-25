public class mario_hua_block : mario_block
{
	public override void reset()
	{
		if (m_param[0] == 0)
		{
			set_fx(mario_fx.mf_right);
		}
		else
		{
			set_fx(mario_fx.mf_left);
		}
	}

	public override mario_point move()
	{
		mario_point mario_point2 = new mario_point();
		if (m_param[0] == 0)
		{
			mario_point2.x = 50;
		}
		else
		{
			mario_point2.x = -50;
		}
		return mario_point2;
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
}
