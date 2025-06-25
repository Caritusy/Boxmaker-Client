using System;

[Serializable]
public class mario_rect
{
	public int x;

	public int y;

	public int w;

	public int h;

	public mario_rect()
	{
	}

	public mario_rect(int x, int y, int w, int h)
	{
		this.x = x;
		this.y = y;
		this.w = w;
		this.h = h;
	}

	public void set(int x, int y, int w, int h)
	{
		this.x = x;
		this.y = y;
		this.w = w;
		this.h = h;
	}
}
