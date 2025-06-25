using protocol.game;
using UnityEngine;

public class clear_gui : MonoBehaviour, IMessage
{
	public GameObject m_sub;

	public GameObject m_panel;

	public GameObject m_text1;

	public GameObject m_text2;

	public GameObject m_map_clear;

	public GameObject m_next_panel;

	public GameObject m_next_panel1;

	public GameObject m_next_panel_def;

	public GameObject m_touxiang;

	public GameObject m_gq;

	public GameObject m_name;

	public GameObject m_map_name;

	public GameObject m_touxiang1;

	public GameObject m_gq1;

	public GameObject m_name1;

	public GameObject m_map_name1;

	public GameObject m_l2;

	public GameObject m_l4;

	public GameObject m_like;

	public GameObject m_ydz;

	private int m_index;

	private int m_time;

	private bool m_next;

	private int m_type;

	private void Start()
	{
		cmessage_center._instance.add_handle(this);
	}

	private void OnDestroy()
	{
		cmessage_center._instance.remove_handle(this);
	}

	public void reset(int type, int exp, int eexp, int rank, int testify, bool next)
	{
		m_type = type;
		m_next = next;
		mario._instance.play_mus_ex1("music/clear", loop: false);
		base.gameObject.SetActive(value: true);
		GetComponent<Animator>().enabled = true;
		m_map_clear.SetActive(value: true);
		m_next_panel.SetActive(value: false);
		m_next_panel1.SetActive(value: false);
		m_next_panel_def.SetActive(value: false);
		m_like.SetActive(value: true);
		m_ydz.SetActive(value: false);
		mario._instance.remove_child(m_panel);
		m_index = 0;
		m_time = 0;
		if (m_type == 0)
		{
			string text = "exp +" + exp;
			if (eexp > 0)
			{
				text = text + "(+" + eexp + ")";
			}
			m_text1.GetComponent<UILabel>().text = text;
			if (testify > 0)
			{
				mario._instance.m_self.testify = testify;
				m_text2.GetComponent<UILabel>().text = string.Format(game_data._instance.get_language_string("clear_gui_hdzm"), testify);
			}
			else if (rank == 0)
			{
				m_text2.GetComponent<UILabel>().text = game_data._instance.get_language_string("clear_gui_whdpm");
			}
			else
			{
				m_text2.GetComponent<UILabel>().text = string.Format(game_data._instance.get_language_string("clear_gui_hdpm"), rank);
			}
			m_touxiang.GetComponent<UISprite>().spriteName = game_data._instance.get_t_touxiang(mario._instance.m_self.per_user_head);
			m_name.GetComponent<UILabel>().text = mario._instance.m_self.per_user_name;
			m_gq.GetComponent<UISprite>().spriteName = game_data._instance.get_t_guojia(mario._instance.m_self.per_user_country);
			m_map_name.GetComponent<UILabel>().text = mario._instance.m_self.per_map_name;
			m_l2.SetActive(value: true);
			m_l4.SetActive(value: false);
		}
		else
		{
			m_touxiang1.GetComponent<UISprite>().spriteName = game_data._instance.get_t_touxiang(mario._instance.m_self.per_user_head);
			m_name1.GetComponent<UILabel>().text = mario._instance.m_self.per_user_name;
			m_gq1.GetComponent<UISprite>().spriteName = game_data._instance.get_t_guojia(mario._instance.m_self.per_user_country);
			m_map_name1.GetComponent<UILabel>().text = mario._instance.m_self.per_map_name;
			m_l2.SetActive(value: false);
			m_l4.SetActive(value: true);
		}
	}

	private void FixedUpdate()
	{
		if (m_index < 40 && m_index % 2 == 0)
		{
			int num = m_index / 2;
			GameObject gameObject = (GameObject)Object.Instantiate(m_sub);
			gameObject.transform.parent = m_panel.transform;
			gameObject.transform.localPosition = new Vector3((num - 10) * 60, 445f, 1f);
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.GetComponent<clear_gui_sub>().reset(270f);
			gameObject.SetActive(value: true);
			GameObject gameObject2 = (GameObject)Object.Instantiate(m_sub);
			gameObject2.transform.parent = m_panel.transform;
			gameObject2.transform.localPosition = new Vector3((num - 10) * 60, -445f, 1f);
			gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject2.GetComponent<clear_gui_sub>().reset(-270f);
			gameObject2.SetActive(value: true);
		}
		m_index++;
		if (m_time == 170)
		{
			if (m_next)
			{
				GetComponent<Animator>().enabled = false;
				m_map_clear.SetActive(value: false);
				if (m_type == 0)
				{
					m_next_panel.SetActive(value: true);
				}
				else
				{
					m_next_panel1.SetActive(value: true);
				}
			}
			else if (m_type == 0)
			{
				GetComponent<Animator>().enabled = false;
				m_map_clear.SetActive(value: false);
				m_next_panel_def.SetActive(value: true);
			}
			else
			{
				mario._instance.change_state(e_game_state.egs_br_end, 1, delegate
				{
					base.gameObject.SetActive(value: false);
				});
			}
		}
		m_time++;
	}

	private void click(GameObject obj)
	{
		if (obj.name == "ok")
		{
			if (m_type == 0)
			{
				cmsg_play_map cmsg_play_map = new cmsg_play_map();
				cmsg_play_map.id = mario._instance.m_self.mapid;
				net_http._instance.send_msg(opclient_t.OPCODE_PLAY_MAP, cmsg_play_map, restart: true, string.Empty, 10f);
			}
			else
			{
				mario._instance.m_start_type = 2;
				mario._instance.change_state(e_game_state.egs_br_road, 1, delegate
				{
					base.gameObject.SetActive(value: false);
				});
			}
		}
		if (obj.name == "cancel")
		{
			mario._instance.change_state(e_game_state.egs_play_select, 1, delegate
			{
				base.gameObject.SetActive(value: false);
			});
		}
		if (obj.name == "exit")
		{
			mario._instance.change_state(e_game_state.egs_play_select, 1, delegate
			{
				base.gameObject.SetActive(value: false);
			});
		}
		if (obj.name == "like")
		{
			cmsg_map_like obj2 = new cmsg_map_like();
			net_http._instance.send_msg(opclient_t.OPCODE_MAP_LIKE, obj2, restart: true, string.Empty, 10f);
		}
	}

	public void message(s_message message)
	{
	}

	public void net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.OPCODE_MAP_LIKE)
		{
			m_like.SetActive(value: false);
			m_ydz.SetActive(value: true);
		}
	}
}
