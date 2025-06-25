using protocol.game;
using UnityEngine;

public class br_end_gui : MonoBehaviour
{
	public GameObject m_win;

	public GameObject m_lose;

	public GameObject m_z1;

	public GameObject m_z2;

	public GameObject m_main;

	public GameObject m_panel;

	public GameObject m_zz_panel;

	public GameObject m_exp;

	public GameObject m_ac_list;

	public GameObject m_thank;

	private float m_y;

	private float m_max_y;

	private int m_wait_time;

	private int m_state;

	private void OnEnable()
	{
		if (mario._instance.m_self.m_finish_type == 0)
		{
			m_win.SetActive(value: true);
			m_lose.SetActive(value: false);
			m_z1.GetComponent<sprite_anim>().play("die");
			m_z2.GetComponent<sprite_anim>().play("die");
		}
		else
		{
			m_win.SetActive(value: false);
			m_lose.SetActive(value: true);
			m_main.GetComponent<sprite_anim>().play("die");
		}
		smsg_mission_finish finish = mario._instance.m_self.m_finish;
		m_exp.GetComponent<UILabel>().text = "+" + finish.exp;
		mario._instance.m_self.add_exp(finish.exp);
		mario._instance.remove_child(m_zz_panel);
		for (int i = 0; i < finish.authors.Count; i++)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(m_ac_list);
			gameObject.transform.parent = m_zz_panel.transform;
			gameObject.transform.localPosition = new Vector3(0f, -320 - 120 * i, 0f);
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.GetComponent<archor_list>().reset(finish.authors[i].user_head, finish.authors[i].user_country, finish.authors[i].user_name, finish.authors[i].map_name);
			gameObject.SetActive(value: true);
		}
		m_panel.transform.localPosition = new Vector3(0f, 0f, 0f);
		m_y = 0f;
		m_max_y = 590 + finish.authors.Count * 120 - 20;
		m_thank.transform.localPosition = new Vector3(0f, 0f - m_max_y, 0f);
		m_wait_time = 100;
		m_state = 0;
	}

	private void FixedUpdate()
	{
		if (m_wait_time > 0)
		{
			m_wait_time--;
			if (m_wait_time == 0)
			{
				if (m_state == 0)
				{
					m_state = 1;
				}
				else if (m_state == 2)
				{
					mario._instance.change_state(e_game_state.egs_play_select, 1, delegate
					{
						base.gameObject.SetActive(value: false);
					});
				}
			}
		}
		if (m_state == 1)
		{
			m_y += 2f;
			if (m_y > m_max_y)
			{
				m_y = m_max_y;
				m_state = 2;
				m_wait_time = 100;
			}
			m_panel.transform.localPosition = new Vector3(0f, m_y, 0f);
		}
	}
}
