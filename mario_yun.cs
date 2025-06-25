using System.Collections.Generic;

public class mario_yun : mario_block
{
	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_is_static = false;
		m_is_dzd = 0;
	}

	public override bool be_left_hit(mario_obj obj, ref int px)
	{
		return false;
	}

	public override bool be_right_hit(mario_obj obj, ref int px)
	{
		return false;
	}

	public override bool be_top_hit(mario_obj obj, ref int py)
	{
		return false;
	}

	public override bool be_left_top_hit(mario_obj obj, ref int px, ref int py)
	{
		return false;
	}

	public override bool be_right_top_hit(mario_obj obj, ref int px, ref int py)
	{
		return false;
	}
}
