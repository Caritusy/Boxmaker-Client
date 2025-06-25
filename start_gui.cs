using System.Collections.Generic;
using UnityEngine;

public class start_gui : MonoBehaviour
{
	public GameObject m_panel;

	public GameObject m_start_gui_sub;

	public GameObject m_num;

	public GameObject m_touxiang;

	public GameObject m_gq;

	public GameObject m_name;

	public GameObject m_map_name;

	public GameObject m_jd;

	private List<GameObject> m_subs = new List<GameObject>();

	private GameObject m_big;

	private int m_wait_time;

	private int m_state;

	private void OnEnable()
	{
		m_num.GetComponent<UILabel>().text = "x " + mario._instance.m_self.br_life;
		m_touxiang.GetComponent<UISprite>().spriteName = game_data._instance.get_t_touxiang(mario._instance.m_self.br_user_head);
		m_name.GetComponent<UILabel>().text = mario._instance.m_self.br_user_name;
		m_gq.GetComponent<UISprite>().spriteName = game_data._instance.get_t_guojia(mario._instance.m_self.br_user_country);
		m_map_name.GetComponent<UILabel>().text = mario._instance.m_self.br_map_name;
		s_t_br s_t_br2 = game_data._instance.get_t_br(mario._instance.m_self.br_hard);
		m_jd.GetComponent<UILabel>().text = (mario._instance.m_self.br_index + 1).ToString() + "/" + s_t_br2.num;
		base.gameObject.SetActive(value: true);
		if (mario._instance.m_start_type == 0)
		{
			start();
		}
		else if (mario._instance.m_start_type == 1)
		{
			die();
		}
		else
		{
			next();
		}
	}

	public void start()
	{
		m_num.SetActive(value: false);
		mario._instance.remove_child(m_panel);
		m_subs.Clear();
		m_wait_time = 100;
		m_state = 0;
		for (int i = 0; i < 4; i++)
		{
			List<GameObject> list = new List<GameObject>();
			int num = 0;
			for (int num2 = 24; num2 >= 0; num2--)
			{
				GameObject gameObject = (GameObject)Object.Instantiate(m_start_gui_sub);
				gameObject.transform.parent = m_panel.transform;
				gameObject.transform.localPosition = new Vector3(-640f, -125 - 50 * i, 0f);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				num = gameObject.GetComponent<start_gui_sub>().run_to(-432 + num2 * 36, -125 - 50 * i, right: true, 1f, wait: true, num);
				if (num2 % 4 == 0)
				{
					gameObject.GetComponent<start_gui_sub>().m_sound = true;
				}
				gameObject.SetActive(value: true);
				if (i % 2 == 0)
				{
					list.Add(gameObject);
				}
				else
				{
					m_subs.Add(gameObject);
				}
			}
			for (int num3 = list.Count - 1; num3 >= 0; num3--)
			{
				m_subs.Add(list[num3]);
			}
			if (num > m_wait_time)
			{
				m_wait_time = num;
			}
		}
		m_wait_time += 80;
	}

	public void die()
	{
		int br_life = mario._instance.m_self.br_life;
		m_num.SetActive(value: false);
		mario._instance.remove_child(m_panel);
		m_subs.Clear();
		m_wait_time = 100;
		m_state = 0;
		int num = br_life / 25;
		if (num < 1)
		{
			num = 0;
		}
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 25; j++)
			{
				if (br_life > i * 25 + j)
				{
					GameObject gameObject = (GameObject)Object.Instantiate(m_start_gui_sub);
					gameObject.transform.parent = m_panel.transform;
					int num2 = -1;
					if (i % 2 == 1)
					{
						num2 = 1;
						gameObject.transform.localPosition = new Vector3(432 - j * 36, -125 - 50 * i, 0f);
					}
					else
					{
						gameObject.transform.localPosition = new Vector3(-432 + j * 36, -125 - 50 * i, 0f);
					}
					gameObject.transform.localScale = new Vector3(num2, 1f, 1f);
					if (i * 25 + j == 0)
					{
						gameObject.GetComponent<start_gui_sub>().m_sound = true;
					}
					gameObject.SetActive(value: true);
					m_subs.Add(gameObject);
				}
			}
		}
	}

	public void next()
	{
		int br_life = mario._instance.m_self.br_life;
		m_num.SetActive(value: true);
		mario._instance.remove_child(m_panel);
		m_subs.Clear();
		m_wait_time = 150;
		m_state = 3;
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 25; j++)
			{
				if (br_life - 1 > i * 25 + j)
				{
					GameObject gameObject = (GameObject)Object.Instantiate(m_start_gui_sub);
					gameObject.transform.parent = m_panel.transform;
					int num = -1;
					if (i % 2 == 1)
					{
						num = 1;
						gameObject.transform.localPosition = new Vector3(432 - j * 36, -125 - 50 * i, 0f);
					}
					else
					{
						gameObject.transform.localPosition = new Vector3(-432 + j * 36, -125 - 50 * i, 0f);
					}
					gameObject.transform.localScale = new Vector3(num, 1f, 1f);
					gameObject.SetActive(value: true);
				}
			}
		}
		GameObject gameObject2 = (GameObject)Object.Instantiate(m_start_gui_sub);
		gameObject2.transform.parent = m_panel.transform;
		gameObject2.transform.localPosition = new Vector3(0f, 2f, 0f);
		gameObject2.transform.localScale = new Vector3(2.5f, 2.5f, 1f);
		gameObject2.SetActive(value: true);
	}

	private void jump()
	{
		m_wait_time = 30;
		m_state = 1;
		m_big = m_subs[0];
		m_big.GetComponent<start_gui_sub>().jump();
	}

	private void run()
	{
		m_wait_time = 40;
		m_state = 2;
		m_big.GetComponent<start_gui_sub>().run_to(0f, 2f, right: true, 2.5f);
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 25; j++)
			{
				int num = i * 25 + j;
				if (num == 0 || m_subs.Count <= num)
				{
					continue;
				}
				if (i % 2 == 0)
				{
					if (j == 0)
					{
						m_subs[num].GetComponent<start_gui_sub>().run_to(-432 + j * 36, -125 - 50 * i + 50, right: false);
					}
					else
					{
						m_subs[num].GetComponent<start_gui_sub>().run_to(-432 + j * 36 - 36, -125 - 50 * i, right: false);
					}
				}
				else if (j == 0)
				{
					m_subs[num].GetComponent<start_gui_sub>().run_to(432 - j * 36, -125 - 50 * i + 50, right: true);
				}
				else
				{
					m_subs[num].GetComponent<start_gui_sub>().run_to(432 - j * 36 + 36, -125 - 50 * i, right: true);
				}
			}
		}
	}

	private void FixedUpdate()
	{
		if (m_wait_time <= 0)
		{
			return;
		}
		m_wait_time--;
		if (m_wait_time != 0)
		{
			return;
		}
		if (m_state == 0)
		{
			jump();
		}
		else if (m_state == 1)
		{
			run();
		}
		else if (m_state == 2)
		{
			m_num.SetActive(value: true);
			m_num.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
			utils.add_scale_anim(m_num, 0.3f, new Vector3(1f, 1f, 1f), 0f);
			m_state = 3;
			m_wait_time = 50;
		}
		else if (m_state == 3)
		{
			mario._instance.change_state(e_game_state.egs_br_play, 1, delegate
			{
				base.gameObject.SetActive(value: false);
			});
		}
	}
}
