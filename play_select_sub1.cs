using protocol.game;
using UnityEngine;

public class play_select_sub1 : MonoBehaviour
{
	public GameObject m_icon;

	public GameObject m_title;

	public GameObject m_tg;

	public GameObject m_cs;

	public GameObject m_tgl;

	public GameObject m_win;

	public GameObject m_nd;

	public map_show m_ms;

	public int m_page;

	public void reset(map_show ms, int page)
	{
		m_page = page;
		m_ms = ms;
		m_title.GetComponent<UILabel>().text = ms.name;
		m_tg.GetComponent<UILabel>().text = ms.pas.ToString("N0");
		m_cs.GetComponent<UILabel>().text = ms.amount.ToString("N0");
		float num = 0f;
		if (ms.amount > 0)
		{
			num = (float)ms.pas / (float)ms.amount * 100f;
		}
		m_tgl.GetComponent<UILabel>().text = num.ToString("f2") + "%";
		m_icon.GetComponent<UITexture>().mainTexture = game_data._instance.mission_to_texture(ms.url);
		if (ms.difficulty > 0)
		{
			m_nd.GetComponent<UISprite>().spriteName = "jbjb_" + ms.difficulty;
		}
		else
		{
			m_nd.GetComponent<UISprite>().spriteName = "jbjb_" + utils.get_map_nd(ms.pas, ms.amount);
		}
		if (ms.finish == 1)
		{
			m_win.SetActive(value: true);
		}
		else
		{
			m_win.SetActive(value: false);
		}
	}

	public void reset(int finish, int pas, int amount, int like)
	{
		m_ms.finish = finish;
		m_ms.pas = pas;
		m_ms.amount = amount;
		m_ms.like = like;
		reset(m_ms, m_page);
	}
}
