using UnityEngine;

public class clear_gui_sub : MonoBehaviour
{
	private float m_x;

	private float m_y;

	private float m_yy;

	private float m_time;

	public void reset(float y)
	{
		m_x = base.transform.localPosition.x;
		m_y = base.transform.localPosition.y;
		m_yy = y;
		m_time = 0f;
	}

	private void Update()
	{
		if (m_time >= 0f)
		{
			m_time += Time.deltaTime;
		}
		float time = m_time;
		if (!(m_time < 0f))
		{
			if (m_time < 0.1f)
			{
				float y = m_y;
				float num = m_yy + (m_yy - m_y) * 0.2f;
				base.transform.localPosition = new Vector3(m_x, y + (num - y) * (time / 0.1f), 0f);
			}
			else if (m_time < 0.3f)
			{
				time -= 0.1f;
				float num2 = m_yy + (m_yy - m_y) * 0.2f;
				float num3 = m_y + (m_yy - m_y) * 0.85f;
				base.transform.localPosition = new Vector3(m_x, num2 + (num3 - num2) * (time / 0.2f), 0f);
			}
			else if (m_time < 0.5f)
			{
				time -= 0.3f;
				float num4 = m_y + (m_yy - m_y) * 0.85f;
				float num5 = m_yy + (m_yy - m_y) * 0.1f;
				base.transform.localPosition = new Vector3(m_x, num4 + (num5 - num4) * (time / 0.2f), 0f);
			}
			else if (m_time < 0.7f)
			{
				time -= 0.5f;
				float num6 = m_yy + (m_yy - m_y) * 0.1f;
				float num7 = m_y + (m_yy - m_y) * 0.95f;
				base.transform.localPosition = new Vector3(m_x, num6 + (num7 - num6) * (time / 0.2f), 0f);
			}
			else if (m_time < 0.9f)
			{
				time -= 0.7f;
				float num8 = m_y + (m_yy - m_y) * 0.95f;
				float yy = m_yy;
				base.transform.localPosition = new Vector3(m_x, num8 + (yy - num8) * (time / 0.2f), 0f);
			}
			else if (m_time > 0f)
			{
				base.transform.localPosition = new Vector3(m_x, m_yy, 0f);
				m_time = -1f;
			}
		}
	}
}
