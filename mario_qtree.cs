using System.Collections.Generic;

public class mario_qtree
{
	public const int max_object = 1;

	public const int max_level = 9;

	public int level;

	public List<List<mario_obj>> objects = new List<List<mario_obj>>();

	public mario_bound bound = new mario_bound();

	public mario_qtree parent;

	public List<mario_qtree> nodes = new List<mario_qtree>();

	public mario_qtree()
	{
		parent = null;
		level = 1;
		bound = new mario_bound(-20480, 143360, 143360, -20480);
		for (int i = 0; i < 4; i++)
		{
			nodes.Add(null);
		}
		for (int j = 0; j < 7; j++)
		{
			objects.Add(new List<mario_obj>());
		}
	}

	public mario_qtree(mario_qtree p, int l, mario_bound b)
	{
		parent = p;
		level = l;
		bound = b;
		for (int i = 0; i < 4; i++)
		{
			nodes.Add(null);
		}
		for (int j = 0; j < 7; j++)
		{
			objects.Add(new List<mario_obj>());
		}
	}

	public void split()
	{
		int num = (bound.left + bound.right) / 2;
		int num2 = (bound.top + bound.bottom) / 2;
		nodes[0] = new mario_qtree(this, level + 1, new mario_bound(num, bound.right, bound.top, num2));
		nodes[1] = new mario_qtree(this, level + 1, new mario_bound(bound.left, num, bound.top, num2));
		nodes[2] = new mario_qtree(this, level + 1, new mario_bound(bound.left, num, num2, bound.bottom));
		nodes[3] = new mario_qtree(this, level + 1, new mario_bound(num, bound.right, num2, bound.bottom));
	}

	public int get_index(mario_bound b)
	{
		int result = -1;
		int num = (bound.left + bound.right) / 2;
		int num2 = (bound.top + bound.bottom) / 2;
		bool flag = b.bottom >= num2 && b.top < bound.top;
		bool flag2 = b.top < num2 && b.bottom >= bound.bottom;
		bool flag3 = b.left >= bound.left && b.right < num;
		bool flag4 = b.right < bound.right && b.left >= num;
		if (flag)
		{
			if (flag3)
			{
				result = 1;
			}
			else if (flag4)
			{
				result = 0;
			}
		}
		else if (flag2)
		{
			if (flag3)
			{
				result = 2;
			}
			else if (flag4)
			{
				result = 3;
			}
		}
		return result;
	}

	public void insert(mario_obj obj)
	{
		if (nodes[0] != null)
		{
			int num = get_index(obj.m_bound);
			if (num != -1)
			{
				nodes[num].insert(obj);
				return;
			}
		}
		List<mario_obj> list = objects[(int)obj.m_type];
		list.Add(obj);
		obj.m_qtree = this;
		if (nodes[0] != null || list.Count <= 1 || level >= 9)
		{
			return;
		}
		split();
		for (int num2 = list.Count - 1; num2 >= 0; num2--)
		{
			mario_obj mario_obj2 = list[num2];
			int num3 = get_index(mario_obj2.m_bound);
			if (num3 != -1)
			{
				list.RemoveAt(num2);
				nodes[num3].insert(mario_obj2);
			}
		}
	}

	public bool empty()
	{
		for (int i = 0; i < objects.Count; i++)
		{
			if (objects[i].Count > 0)
			{
				return false;
			}
		}
		if (nodes[0] != null)
		{
			return false;
		}
		return true;
	}

	public void remove(mario_obj obj)
	{
		bool flag = false;
		List<mario_obj> list = objects[(int)obj.m_type];
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] == obj)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			list.Remove(obj);
			if (empty() && parent != null)
			{
				parent.check_combine();
			}
		}
	}

	public void check_combine()
	{
		if (nodes[0] == null)
		{
			return;
		}
		for (int i = 0; i < nodes.Count; i++)
		{
			if (!nodes[i].empty())
			{
				return;
			}
		}
		for (int j = 0; j < nodes.Count; j++)
		{
			nodes[j] = null;
		}
		if (empty() && parent != null)
		{
			parent.check_combine();
		}
	}

	public void retrive(mario_obj obj, ref List<mario_obj> objs, int type)
	{
		retrive_ex(obj.m_bound, ref objs, type);
		for (mario_qtree mario_qtree2 = obj.m_qtree.parent; mario_qtree2 != null; mario_qtree2 = mario_qtree2.parent)
		{
			objs.AddRange(mario_qtree2.objects[type]);
		}
	}

	public void retrive_floor(mario_obj obj, ref List<mario_obj> objs)
	{
		retrive_ex_floor(obj.get_floor_bound(), ref objs);
		for (mario_qtree mario_qtree2 = obj.m_qtree.parent; mario_qtree2 != null; mario_qtree2 = mario_qtree2.parent)
		{
			objs.AddRange(mario_qtree2.objects[1]);
			objs.AddRange(mario_qtree2.objects[2]);
			objs.AddRange(mario_qtree2.objects[4]);
			objs.AddRange(mario_qtree2.objects[3]);
		}
	}

	public void retrive_left(mario_obj obj, ref List<mario_obj> objs)
	{
		retrive_ex_fx(obj.get_left_bound(), ref objs);
		for (mario_qtree mario_qtree2 = obj.m_qtree.parent; mario_qtree2 != null; mario_qtree2 = mario_qtree2.parent)
		{
			objs.AddRange(mario_qtree2.objects[1]);
			objs.AddRange(mario_qtree2.objects[2]);
		}
	}

	public void retrive_right(mario_obj obj, ref List<mario_obj> objs)
	{
		retrive_ex_fx(obj.get_right_bound(), ref objs);
		for (mario_qtree mario_qtree2 = obj.m_qtree.parent; mario_qtree2 != null; mario_qtree2 = mario_qtree2.parent)
		{
			objs.AddRange(mario_qtree2.objects[1]);
			objs.AddRange(mario_qtree2.objects[2]);
		}
	}

	public void retrive_ex(mario_bound b, ref List<mario_obj> objs, int type)
	{
		if (nodes[0] != null)
		{
			int num = get_index(b);
			if (num != -1)
			{
				nodes[num].retrive_ex(b, ref objs, type);
			}
			else
			{
				List<mario_bound> list = bound.carve(b);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].bs_yx)
					{
						num = get_index(list[i]);
						if (num != -1)
						{
							nodes[num].retrive_ex(list[i], ref objs, type);
						}
					}
				}
			}
		}
		objs.AddRange(objects[type]);
	}

	public void retrive_ex_floor(mario_bound b, ref List<mario_obj> objs)
	{
		if (nodes[0] != null)
		{
			int num = get_index(b);
			if (num != -1)
			{
				nodes[num].retrive_ex_floor(b, ref objs);
			}
			else
			{
				List<mario_bound> list = bound.carve(b);
				for (int i = 0; i < list.Count; i++)
				{
					num = get_index(list[i]);
					if (num != -1)
					{
						nodes[num].retrive_ex_floor(list[i], ref objs);
					}
				}
			}
		}
		objs.AddRange(objects[1]);
		objs.AddRange(objects[2]);
		objs.AddRange(objects[4]);
		objs.AddRange(objects[3]);
	}

	public void retrive_ex_fx(mario_bound b, ref List<mario_obj> objs)
	{
		if (nodes[0] != null)
		{
			int num = get_index(b);
			if (num != -1)
			{
				nodes[num].retrive_ex_fx(b, ref objs);
			}
			else
			{
				List<mario_bound> list = bound.carve(b);
				for (int i = 0; i < list.Count; i++)
				{
					num = get_index(list[i]);
					if (num != -1)
					{
						nodes[num].retrive_ex_fx(list[i], ref objs);
					}
				}
			}
		}
		objs.AddRange(objects[1]);
		objs.AddRange(objects[2]);
	}
}
