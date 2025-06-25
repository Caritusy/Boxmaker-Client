using UnityEngine;

public class edit_gui_fg : MonoBehaviour
{
	private int m_x;

	private int m_y;

	private int m_time;

	public void reset(int x, int y)
	{
		m_x = x;
		m_y = y;
	}

	private void FixedUpdate()
	{
		m_time++;
		if (m_time == (m_x - m_y) * 2 + 20)
		{
			GetComponent<TweenRotation>().enabled = true;
		}
		else if (m_time == (m_x - m_y) * 2 + 40)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
