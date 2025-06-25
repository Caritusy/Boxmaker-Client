using UnityEngine;

public class score : MonoBehaviour
{
	public int m_time;

	public GameObject m_text;

	public void reset(int s)
	{
		m_text.GetComponent<UILabel>().text = s.ToString();
	}

	public void update_ex()
	{
		m_time++;
	}
}
