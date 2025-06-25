using System.Collections.Generic;

public class s_t_mission_sub
{
	private int _type;

	public List<int> param = new List<int>();

	public int type
	{
		get
		{
			return _type;
		}
		set
		{
			if (_type == value)
			{
				return;
			}
			_type = value;
			param.Clear();
			if (_type != 0)
			{
				for (int i = 0; i < 4; i++)
				{
					param.Add(0);
				}
			}
		}
	}
}
