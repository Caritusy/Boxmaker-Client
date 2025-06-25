using System.Collections.Generic;

public class mario_block_defualt : mario_block
{
	private int m_die_time;

	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_is_dzd = 2;
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
	}
}
