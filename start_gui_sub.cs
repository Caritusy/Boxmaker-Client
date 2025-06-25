using UnityEngine;

public class start_gui_sub : MonoBehaviour
{
	private int m_state;

	private float m_x;

	private float m_y;

	private float m_speed_x;

	private float m_speed_y;

	private int m_wait;

	private int m_time;

	public bool m_sound;

	public int run_to(float x, float y, bool right, float scale = 1f, bool wait = false, int wait_time = 0)
	{
		m_state = 1;
		m_time = 0;
		m_x = x;
		m_y = y;
		m_speed_x = 0f;
		m_speed_y = 0f;
		GetComponent<sprite_anim>().play("run");
		if (right)
		{
			base.transform.localScale = new Vector3(scale, scale, 1f);
		}
		else
		{
			base.transform.localScale = new Vector3(0f - scale, scale, 1f);
		}
		if (wait)
		{
			m_wait = wait_time + Random.Range(3, 8);
		}
		return m_wait;
	}

	public void jump()
	{
		m_state = 2;
		m_time = 0;
		m_speed_y = 40f;
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		GetComponent<sprite_anim>().play("jump");
		mario._instance.play_sound("sound/jump");
	}

	private void FixedUpdate()
	{
		if (m_wait > 0)
		{
			m_wait--;
		}
		else if (m_state == 2)
		{
			m_time++;
			m_speed_y -= 4f;
			base.transform.localPosition = new Vector3(base.transform.localPosition.x + 10f, base.transform.localPosition.y + m_speed_y, 0f);
			base.transform.localScale = new Vector3(1f + (float)m_time / 10f, 1f + (float)m_time / 10f, 1f);
			if (m_time >= 15)
			{
				GetComponent<sprite_anim>().play("stand");
				m_state = 0;
			}
		}
		else
		{
			if (m_state != 1)
			{
				return;
			}
			if (m_time % 50 == 0 && m_sound)
			{
				mario._instance.play_sound("sound/step");
			}
			m_time++;
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
}
