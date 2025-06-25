public class mario_gold : mario_attack_ex
{
	private int m_die_time;

	public override void be_hit(mario_obj obj)
	{
		if (obj.m_main)
		{
			mario._instance.play_sound("sound/coins");
			m_is_die = true;
			m_bkcf = true;
			play_anim("die");
			play_mode._instance.add_score(m_pos.x, m_pos.y, 100);
		}
	}

	public override void tupdate()
	{
		if (m_is_die)
		{
			m_die_time++;
			if (m_die_time > 50)
			{
				m_is_destory = 1;
			}
		}
	}
}
