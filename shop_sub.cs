using UnityEngine;

public class shop_sub : MonoBehaviour
{
	public s_t_shop m_t_shop;

	public GameObject m_num;

	public GameObject m_yuan;

	public GameObject m_icon;

	public GameObject m_tb;

	public GameObject m_title;

	public GameObject m_desc;

	public void reset()
	{
		reset(m_t_shop);
	}

	public void reset(s_t_shop t_shop)
	{
		m_t_shop = t_shop;
		m_num.SetActive(value: true);
		m_num.GetComponent<UILabel>().text = m_t_shop.price.ToString();
		if (m_t_shop.type == 1)
		{
			m_yuan.SetActive(value: true);
			m_icon.SetActive(value: false);
			if (m_t_shop.ios_desc != string.Empty)
			{
				m_desc.GetComponent<UILabel>().text = m_t_shop.ios_desc;
				m_desc.SetActive(value: true);
				m_yuan.SetActive(value: false);
				m_num.SetActive(value: false);
			}
		}
		else
		{
			m_yuan.SetActive(value: false);
			m_icon.SetActive(value: true);
		}
		GetComponent<UISprite>().spriteName = m_t_shop.db;
		m_tb.GetComponent<UISprite>().spriteName = m_t_shop.icon;
		m_tb.GetComponent<UISprite>().MakePixelPerfect();
		m_title.GetComponent<UILabel>().text = m_t_shop.name;
	}
}
