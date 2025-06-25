using System.Collections.Generic;
using UnityEngine;

public class tip_gui : MonoBehaviour
{
	private List<tip_gui_sub> m_tips = new List<tip_gui_sub>();

	private int m_jd;

	public void add_text(string text)
	{
		if (m_tips.Count >= 3)
		{
			Object.Destroy(m_tips[0].tip);
			m_tips.RemoveAt(0);
		}
		tip_gui_sub tip_gui_sub2 = new tip_gui_sub();
		GameObject original = Resources.Load("ui/tip_gui_sub") as GameObject;
		GameObject gameObject = (GameObject)Object.Instantiate(original);
		gameObject.transform.FindChild("text").GetComponent<UILabel>().text = text;
		tip_gui_sub2.tip = gameObject;
		tip_gui_sub2.time = 0f;
		gameObject.transform.parent = base.gameObject.transform;
		if (m_jd == 0)
		{
			gameObject.transform.localPosition = new Vector3(0f, 180f, 0f);
			tip_gui_sub2.y = 180f;
		}
		else if (m_jd == 1)
		{
			gameObject.transform.localPosition = new Vector3(0f, 140f, 0f);
			tip_gui_sub2.y = 140f;
		}
		else if (m_jd == 2)
		{
			gameObject.transform.localPosition = new Vector3(0f, 100f, 0f);
			tip_gui_sub2.y = 140f;
			if (m_tips.Count == 1)
			{
				m_tips[0].tip.transform.localPosition = new Vector3(0f, 140f, 0f);
				m_tips[0].y = 180f;
			}
			else if (m_tips.Count == 2)
			{
				m_tips[0].tip.transform.localPosition = new Vector3(0f, 180f, 0f);
				m_tips[0].y = 220f;
				m_tips[1].tip.transform.localPosition = new Vector3(0f, 140f, 0f);
				m_tips[1].y = 180f;
			}
		}
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		m_tips.Add(tip_gui_sub2);
		if (m_jd < 2)
		{
			m_jd++;
		}
	}

	private void Update()
	{
		List<tip_gui_sub> list = new List<tip_gui_sub>();
		for (int i = 0; i < m_tips.Count; i++)
		{
			m_tips[i].time += Time.deltaTime;
			if (m_tips[i].time > 3f)
			{
				list.Add(m_tips[i]);
			}
			else if (m_tips[i].time >= 2f)
			{
				float alpha = 3f - m_tips[i].time;
				m_tips[i].tip.GetComponent<UISprite>().alpha = alpha;
			}
			if (m_tips[i].tip.transform.localPosition.y < m_tips[i].y)
			{
				m_tips[i].tip.transform.localPosition = new Vector3(0f, m_tips[i].tip.transform.localPosition.y + Time.deltaTime * 200f, 0f);
			}
			if (m_tips[i].tip.transform.localPosition.y >= m_tips[0].y)
			{
				m_tips[i].tip.transform.localPosition = new Vector3(0f, m_tips[i].y, 0f);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			Object.Destroy(list[j].tip);
			m_tips.Remove(list[j]);
		}
	}
}
