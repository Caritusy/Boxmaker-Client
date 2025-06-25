using System.Collections.Generic;

public class mario_start : mario_obj
{
	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_has_edit = true;
	}
}
