using System.Collections.Generic;

public class mario_end : mario_obj
{
	private bool m_hit;

	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_type = mario_type.mt_attack;
		m_has_edit = true;
		m_has_floor = false;
	}

	public override void reset()
	{
		bool flag = false;
		if (m_world == game_data._instance.m_map_data.end_area)
		{
			flag = true;
		}
		base.transform.FindChild("sprite").gameObject.SetActive(flag);
		if (m_shadow != null)
		{
			m_shadow.transform.FindChild("sprite").gameObject.SetActive(flag);
		}
	}

	public override void be_hit(mario_obj obj)
	{
		if (obj.m_main && !m_hit && m_world == game_data._instance.m_map_data.end_area)
		{
			m_hit = true;
			obj.m_bl_objs.Add(this);
			play_mode._instance.m_state = 1;
			mario._instance.play_sound("sound/end");
		}
	}
}
