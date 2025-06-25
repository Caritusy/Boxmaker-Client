using System;

[Serializable]
public class mario_point
{
	public int x;

	public int y;

	public mario_point()
	{
	}

	public mario_point(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public void set(int x, int y)
	{
		this.x = x;
		this.y = y;
	}
}
