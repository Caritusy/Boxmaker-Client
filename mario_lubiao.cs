using System.Collections.Generic;

public class mario_lubiao : mario_obj
{
	public override void reset()
	{
		set_angles(0f, 0f, m_param[0] * 45);
	}

	public override void change()
	{
		if (m_param[0] < 7)
		{
			List<int> param;
			List<int> list = (param = m_param);
			int index;
			int index2 = (index = 0);
			index = param[index];
			list[index2] = index + 1;
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
