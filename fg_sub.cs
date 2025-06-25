using UnityEngine;

public class fg_sub : MonoBehaviour
{
	public GameObject m_text;

	public GameObject m_lock;

	public GameObject m_name;

	public GameObject m_gou;

	public int m_id;

	public bool m_lk;

	public void reset(int id)
	{
		m_id = id;
		set_gou(flag: false);
		s_t_fg s_t_fg2 = game_data._instance.get_t_fg(id);
		Texture2D mainTexture = Resources.Load("texture/back/back_" + id) as Texture2D;
		GetComponent<UITexture>().mainTexture = mainTexture;
		m_name.GetComponent<UILabel>().text = s_t_fg2.name;
		if (s_t_fg2.tj > mario._instance.m_self.job_level)
		{
			m_lk = true;
			m_lock.SetActive(value: true);
			m_text.GetComponent<UILabel>().text = s_t_fg2.desc;
		}
		else
		{
			m_lk = false;
			m_lock.SetActive(value: false);
		}
	}

	public void set_gou(bool flag)
	{
		m_gou.SetActive(flag);
	}
}
