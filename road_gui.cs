using System.Collections.Generic;
using UnityEngine;

public class road_gui : MonoBehaviour
{
	public List<road_gui_road> m_roads = new List<road_gui_road>();

	public GameObject m_zhu1;

	public GameObject m_zhu2;

	public GameObject m_zhu3;

	public GameObject m_zhu4;

	public GameObject m_road1;

	public GameObject m_road2;

	public GameObject m_c2;

	public GameObject m_main;

	public GameObject m_life;

	public GameObject m_jd;

	public Vector3 m_qd;

	private int m_type;

	private int m_wait_time;

	private int m_state;

	private int m_index;

	private void OnEnable()
	{
		reset();
		if (mario._instance.m_start_type == 0)
		{
			start();
		}
		else
		{
			next();
		}
	}

	private void OnDisable()
	{
		GetComponent<Animator>().enabled = false;
	}

	private void reset()
	{
		m_type = 0;
		if (mario._instance.m_self.br_hard > 2)
		{
			m_type = 1;
		}
		m_index = mario._instance.m_self.br_index;
		if (m_type == 0)
		{
			m_road1.SetActive(value: true);
			m_road2.SetActive(value: false);
		}
		else
		{
			m_road1.SetActive(value: false);
			m_road2.SetActive(value: true);
		}
		m_c2.transform.localPosition = m_roads[m_type].m_c2;
		m_life.GetComponent<UILabel>().text = "x " + mario._instance.m_self.br_life;
		s_t_br s_t_br2 = game_data._instance.get_t_br(mario._instance.m_self.br_hard);
		m_jd.GetComponent<UILabel>().text = (mario._instance.m_self.br_index + 1).ToString() + "/" + s_t_br2.num;
	}

	public void start()
	{
		m_wait_time = 75;
		m_state = 0;
		m_index = 0;
		m_main.SetActive(value: false);
		m_main.transform.localScale = new Vector3(1f, 1f, 1f);
		for (int i = 0; i < m_roads[m_type].m_lubiao.Count; i++)
		{
			m_roads[m_type].m_lubiao[i].SetActive(value: false);
		}
	}

	public void next()
	{
		m_wait_time = 75;
		m_state = 2;
		m_main.SetActive(value: true);
		if (m_index == 0)
		{
			m_main.transform.localPosition = m_qd;
		}
		else
		{
			m_main.transform.localPosition = m_roads[m_type].m_points[m_index - 1];
			if (m_roads[m_type].m_fxs[m_index - 1])
			{
				m_main.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			else
			{
				m_main.transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
		for (int i = 0; i <= m_index; i++)
		{
			m_roads[m_type].m_lubiao[i].SetActive(value: true);
		}
		for (int j = m_index + 1; j < m_roads[m_type].m_lubiao.Count; j++)
		{
			m_roads[m_type].m_lubiao[j].SetActive(value: false);
		}
	}

	private void zhu1()
	{
		m_zhu1.GetComponent<sprite_anim>().play("stand");
		m_zhu2.GetComponent<sprite_anim>().play("stand");
	}

	private void zhu2()
	{
		m_zhu3.GetComponent<sprite_anim>().play("stand");
		m_zhu4.GetComponent<sprite_anim>().play("stand");
	}

	private void end()
	{
		GetComponent<Animator>().Play("road_main");
		m_state = 1;
	}

	private void end1()
	{
		GetComponent<Animator>().enabled = false;
		m_state = 2;
		m_wait_time = 30;
		m_roads[m_type].m_lubiao[0].SetActive(value: true);
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
			GetComponent<Animator>().enabled = true;
			if (m_type == 0)
			{
				m_road1.SetActive(value: true);
				m_road2.SetActive(value: false);
				GetComponent<Animator>().Play("road_gui1");
			}
			else
			{
				m_road1.SetActive(value: false);
				m_road2.SetActive(value: true);
				GetComponent<Animator>().Play("road_gui2");
			}
		}
		else if (m_state == 2)
		{
			m_main.GetComponent<road_gui_sub>().run_to(m_roads[m_type].m_points[m_index].x, m_roads[m_type].m_points[m_index].y);
			m_wait_time = 50;
			m_state = 3;
		}
		else if (m_state == 3)
		{
			mario._instance.play_sound("sound/quan");
			List<int> list = new List<int>();
			list.Add((int)m_roads[m_type].m_points[m_index].x);
			list.Add((int)m_roads[m_type].m_points[m_index].y);
			mario._instance.change_state(e_game_state.egs_br_start, 3, delegate
			{
				base.gameObject.SetActive(value: false);
			}, list);
		}
	}
}
