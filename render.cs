using System.Collections.Generic;
using UnityEngine;

public class render : MonoBehaviour, IMessage
{
	public GameObject m_rd;

	public GameObject m_rd1;

	public GameObject m_loading;

	public GameObject m_fuzhu;

	public GameObject m_play_mode;

	public GameObject m_edit_mode;

	private List<edit_cy> m_edit_cys = new List<edit_cy>();

	private void Start()
	{
		cmessage_center._instance.add_handle(this);
	}

	public void message(s_message message)
	{
		if (message.m_type == "play_mode")
		{
			m_edit_mode.SetActive(value: false);
			m_play_mode.SetActive(value: true);
			m_rd.SetActive(value: true);
			m_rd1.SetActive(value: true);
			m_fuzhu.SetActive(value: false);
			mario_point qpos = (mario_point)message.m_object[0];
			int world = 0;
			if (message.m_ints.Count > 0)
			{
				world = (int)message.m_ints[0];
			}
			int mode = 0;
			if (message.m_ints.Count > 1)
			{
				mode = (int)message.m_ints[1];
			}
			m_play_mode.GetComponent<play_mode>().reload(qpos, world, mode);
		}
		if (message.m_type == "close_play_mode")
		{
			m_play_mode.SetActive(value: false);
			if (mario._instance.m_game_state != e_game_state.egs_edit)
			{
				m_rd.SetActive(value: false);
				m_rd1.SetActive(value: false);
			}
		}
		if (message.m_type == "edit_mode")
		{
			m_play_mode.SetActive(value: false);
			m_edit_mode.SetActive(value: true);
			m_rd.SetActive(value: true);
			m_rd1.SetActive(value: true);
			m_fuzhu.SetActive(value: true);
			mario_point qpos2 = null;
			if (message.m_object.Count > 0)
			{
				qpos2 = (mario_point)message.m_object[0];
			}
			int world2 = 0;
			if (message.m_ints.Count > 0)
			{
				world2 = (int)message.m_ints[0];
			}
			m_edit_mode.GetComponent<edit_mode>().reload(qpos2, world2, m_edit_cys);
			m_edit_cys = new List<edit_cy>();
		}
		if (message.m_type == "close_edit_mode")
		{
			m_edit_mode.SetActive(value: false);
			m_rd.SetActive(value: false);
			m_rd1.SetActive(value: false);
		}
		if (message.m_type == "first_load")
		{
			foreach (KeyValuePair<int, s_t_unit> item in game_data._instance.m_t_unit)
			{
				s_t_unit value = item.Value;
				string res = value.res;
				string path = "unit/" + res + "/" + res;
				if (value != null && value.kfg == 1)
				{
					path = "unit/" + res + "/1/" + res;
				}
				GameObject original = (GameObject)Resources.Load(path);
				GameObject gameObject = (GameObject)Object.Instantiate(original);
				gameObject.transform.parent = m_loading.transform;
				gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			s_message s_message2 = new s_message();
			s_message2.m_type = "first_load_end";
			s_message2.time = 0.3f;
			cmessage_center._instance.add_message(s_message2);
		}
		if (message.m_type == "first_load_end")
		{
			mario._instance.remove_child(m_loading);
		}
		if (!(message.m_type == "edit_canying"))
		{
			return;
		}
		edit_cy edit_cy2 = (edit_cy)message.m_object[0];
		if (m_edit_cys.Count > 0)
		{
			edit_cy edit_cy3 = m_edit_cys[m_edit_cys.Count - 1];
			float num = Mathf.Sqrt((edit_cy3.p.x - edit_cy2.p.x) * (edit_cy3.p.x - edit_cy2.p.x) + (edit_cy3.p.y - edit_cy2.p.y) * (edit_cy3.p.y - edit_cy2.p.y));
			if (num < 640f)
			{
				return;
			}
		}
		m_edit_cys.Add(edit_cy2);
		if (m_edit_cys.Count > 50)
		{
			m_edit_cys.RemoveAt(0);
		}
	}

	public void net_message(s_net_message message)
	{
	}
}
