using UnityEngine;

public class road_gui_sub : MonoBehaviour
{
	private int m_state;

	private float m_x;

	private float m_y;

	private float m_speed_x;

	private float m_speed_y;

	public void run_to(float x, float y)
	{
		m_state = 1;
		m_x = x;
		m_y = y;
		m_speed_x = 0f;
		m_speed_y = 0f;
		GetComponent<sprite_anim>().play("run");
	}

	private void FixedUpdate()
	{
		if (m_state != 1)
		{
			return;
		}
		float x = base.transform.localPosition.x;
		float y = base.transform.localPosition.y;
		float num = m_x - x;
		float num2 = m_y - y;
		if (num > -0.0001f && num < 0.0001f && num2 > -0.0001f && num2 < 0.0001f)
		{
			m_state = 0;
			GetComponent<sprite_anim>().play("stand");
			return;
		}
		float num3 = num / 5f;
		float num4 = num2 / 5f;
		if (num < 0f)
		{
			if (m_speed_x - 1f > num3)
			{
				m_speed_x -= 1f;
			}
			else
			{
				m_speed_x = num3;
			}
			if (m_speed_x > -1f)
			{
				m_speed_x = -1f;
			}
			else if (m_speed_x < -50f)
			{
				m_speed_x = -50f;
			}
			if (num >= m_speed_x)
			{
				m_speed_x = num;
			}
		}
		else if (num > 0f)
		{
			if (m_speed_x + 1f < num3)
			{
				m_speed_x += 1f;
			}
			else
			{
				m_speed_x = num3;
			}
			if (m_speed_x < 1f)
			{
				m_speed_x = 1f;
			}
			else if (m_speed_x > 50f)
			{
				m_speed_x = 50f;
			}
			if (num <= m_speed_x)
			{
				m_speed_x = num;
			}
		}
		else
		{
			m_speed_x = 0f;
		}
		if (num2 < 0f)
		{
			if (m_speed_y - 1f > num4)
			{
				m_speed_y -= 1f;
			}
			else
			{
				m_speed_y = num4;
			}
			if (m_speed_y > -1f)
			{
				m_speed_y = -1f;
			}
			else if (m_speed_y < -50f)
			{
				m_speed_y = -50f;
			}
			if (num2 >= m_speed_y)
			{
				m_speed_y = num2;
			}
		}
		else if (num2 > 0f)
		{
			if (m_speed_y + 1f < num4)
			{
				m_speed_y += 1f;
			}
			else
			{
				m_speed_y = num4;
			}
			if (m_speed_y < 1f)
			{
				m_speed_y = 1f;
			}
			else if (m_speed_x > 50f)
			{
				m_speed_x = 50f;
			}
			if (num2 <= m_speed_y)
			{
				m_speed_y = num2;
			}
		}
		else
		{
			m_speed_y = 0f;
		}
		base.transform.localPosition = new Vector3(x + m_speed_x, y + m_speed_y, 0f);
	}
}
