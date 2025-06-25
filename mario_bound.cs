using System;
using System.Collections.Generic;

[Serializable]
public class mario_bound
{
	public int left;

	public int right;

	public int top;

	public int bottom;

	public int left_div;

	public int right_div;

	public int top_div;

	public int bottom_div;

	public bool bs_yx = true;

	[NonSerialized]
	private List<mario_bound> bs;

	public mario_bound()
	{
	}

	public mario_bound(int l, int r, int t, int b)
	{
		set_bound(l, r, t, b, yx: true);
	}

	public mario_bound(int l, int r, int t, int b, bool yx)
	{
		set_bound(l, r, t, b, yx);
	}

	public void set_bound(int l, int r, int t, int b, bool yx)
	{
		left = l;
		right = r;
		top = t;
		bottom = b;
		bs_yx = yx;
	}

	public List<mario_bound> carve(mario_bound b)
	{
		int num = (left + right) / 2;
		int num2 = (top + bottom) / 2;
		if (bs == null)
		{
			bs = new List<mario_bound>();
			bs.Add(new mario_bound(0, 0, 0, 0, yx: false));
			bs.Add(new mario_bound(0, 0, 0, 0, yx: false));
			bs.Add(new mario_bound(0, 0, 0, 0, yx: false));
			bs.Add(new mario_bound(0, 0, 0, 0, yx: false));
		}
		if (num2 <= b.top && num2 > b.bottom)
		{
			if (num > b.right)
			{
				bs[0].set_bound(b.left, b.right, b.top, num2, yx: true);
				bs[1].set_bound(b.left, b.right, num2 - 1, b.bottom, yx: true);
				bs[2].bs_yx = false;
				bs[3].bs_yx = false;
			}
			else if (num <= b.left)
			{
				bs[0].set_bound(b.left, b.right, b.top, num2, yx: true);
				bs[1].set_bound(b.left, b.right, num2 - 1, b.bottom, yx: true);
				bs[2].bs_yx = false;
				bs[3].bs_yx = false;
			}
			else
			{
				bs[0].set_bound(b.left, num - 1, b.top, num2, yx: true);
				bs[1].set_bound(num, b.right, b.top, num2, yx: true);
				bs[2].set_bound(b.left, num - 1, num2 - 1, b.bottom, yx: true);
				bs[3].set_bound(num, b.right, num2 - 1, b.bottom, yx: true);
			}
		}
		else if (num2 > b.top)
		{
			if (num <= b.right && num > b.left)
			{
				bs[0].set_bound(b.left, num - 1, b.top, b.bottom, yx: true);
				bs[1].set_bound(num, b.right, b.top, b.bottom, yx: true);
				bs[2].bs_yx = false;
				bs[3].bs_yx = false;
			}
		}
		else if (num2 <= b.bottom && num <= b.right && num > b.left)
		{
			bs[0].set_bound(b.left, num - 1, b.top, b.bottom, yx: true);
			bs[1].set_bound(num, b.right, b.top, b.bottom, yx: true);
			bs[2].bs_yx = false;
			bs[3].bs_yx = false;
		}
		return bs;
	}
}
