using System.Collections.Generic;

public class mario_block1 : mario_block
{
	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_type = mario_type.mt_block1;
		m_bkcf = true;
		m_mocali = 12;
	}
}
