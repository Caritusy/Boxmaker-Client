using System.Collections.Generic;

public class mario_attack_ex : mario_obj
{
	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_type = mario_type.mt_attack_ex;
	}

	public override void be_hit(mario_obj obj)
	{
		if (obj.m_main)
		{
			obj.set_bl(0, 4);
		}
	}
}
