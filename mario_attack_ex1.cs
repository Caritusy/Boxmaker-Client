using System.Collections.Generic;

public class mario_attack_ex1 : mario_attack_ex
{
	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_type = mario_type.mt_attack_ex1;
		m_bl.Add(0);
	}
}
