using System.Collections.Generic;
using BMReborn.AntiCheat;
using protocol.game;
using UnityEngine;

public class play_gui : MonoBehaviour, IMessage
{
	public GameObject m_play_pause;

	public GameObject m_play_lose;

	public GameObject m_brplay_pause;

	public GameObject m_brplay_lose;

	private int m_win;

	public GameObject m_br_life;

	public GameObject m_pause;

	public GameObject m_play_input;

	public GameObject m_sm;

	public GameObject m_reply;

	public GameObject m_score_list;

	public GameObject m_score_text;

	public GameObject m_time_sp;

	public GameObject m_time_text;

	public GameObject m_ys_text;

	public GameObject m_br_life_text;

	public List<GameObject> m_buttons;

	private List<GameObject> m_scores = new List<GameObject>();

	public GameObject m_score_panel;

	public GameObject m_score_sub;

	public GameObject m_mask;

	public GameObject m_time_up;

	public GameObject m_chuan;

	public GameObject m_x_buttom;

	private static play_gui _instance;

	private void Start()
	{
		cmessage_center._instance.add_handle(this);
		_instance = this;
	}

	private void OnDestroy()
	{
		cmessage_center._instance.remove_handle(this);
	}

	private void OnEnable()
	{
		m_scores.Clear();
		mario._instance.remove_child(m_score_panel);
		m_play_pause.SetActive(value: false);
		m_play_lose.SetActive(value: false);
		m_brplay_pause.SetActive(value: false);
		m_brplay_lose.SetActive(value: false);
		m_mask.SetActive(value: false);
		m_sm.SetActive(value: false);
		m_time_up.SetActive(value: false);
		m_chuan.SetActive(value: false);
		m_x_buttom.SetActive(value: false);
		if (mario._instance.m_game_state == e_game_state.egs_play)
		{
			m_pause.SetActive(value: true);
			m_play_input.SetActive(value: true);
			m_reply.SetActive(value: false);
			m_score_list.SetActive(value: true);
			m_br_life.SetActive(value: false);
		}
		else if (mario._instance.m_game_state == e_game_state.egs_review)
		{
			m_pause.SetActive(value: true);
			m_play_input.SetActive(value: false);
			m_reply.SetActive(value: true);
			m_score_list.SetActive(value: true);
			m_br_life.SetActive(value: false);
		}
		else if (mario._instance.m_game_state == e_game_state.egs_edit)
		{
			m_pause.SetActive(value: false);
			m_play_input.SetActive(value: true);
			m_reply.SetActive(value: false);
			m_score_list.SetActive(value: false);
			m_br_life.SetActive(value: false);
		}
		else if (mario._instance.m_game_state == e_game_state.egs_edit_play)
		{
			m_pause.SetActive(value: true);
			m_play_input.SetActive(value: true);
			m_reply.SetActive(value: false);
			m_score_list.SetActive(value: true);
			m_br_life.SetActive(value: false);
		}
		else if (mario._instance.m_game_state == e_game_state.egs_edit_upload)
		{
			m_pause.SetActive(value: true);
			m_play_input.SetActive(value: true);
			m_reply.SetActive(value: false);
			m_score_list.SetActive(value: true);
			m_br_life.SetActive(value: false);
		}
		else if (mario._instance.m_game_state == e_game_state.egs_br_play)
		{
			m_pause.SetActive(value: true);
			m_play_input.SetActive(value: true);
			m_reply.SetActive(value: false);
			m_score_list.SetActive(value: true);
			m_br_life.SetActive(value: true);
		}
		if (utils.IsPC)
		{
			m_play_input.SetActive(value: false);
			if (mario._instance.m_self.level == 1)
			{
				m_sm.SetActive(value: true);
			}
		}
		m_time_sp.GetComponent<UISprite>().spriteName = "mode0" + (game_data._instance.m_map_data.mode + 1);
	}

	private void OnDisable()
	{
		s_message s_message2 = new s_message();
		s_message2.m_type = "close_play_mode";
		cmessage_center._instance.add_message(s_message2);
		if (mario._instance.m_game_state != e_game_state.egs_br_start && mario._instance.m_game_state == e_game_state.egs_br_road)
		{
			mario._instance.play_mus("music/select");
		}
	}

	public void message(s_message message)
	{
		if (message.m_type == "play_win")
		{
			m_win = 0;
			if (mario._instance.m_game_state == e_game_state.egs_play)
			{
				List<int> input = (List<int>)message.m_object[0];
				cmsg_complete_map cmsg_complete_map = new cmsg_complete_map();
				cmsg_complete_map.suc = 0;
				cmsg_complete_map.point = (int)message.m_ints[0];
				cmsg_complete_map.time = (int)message.m_ints[1];
				cmsg_complete_map.video = game_data._instance.save_inputs(input);
				net_http._instance.send_msg(opclient_t.OPCODE_COMPLETE_MAP, cmsg_complete_map, restart: true, string.Empty, 10f);
			}
			else if (mario._instance.m_game_state == e_game_state.egs_review)
			{
				mario._instance.change_state(e_game_state.egs_play_select, 1, delegate
				{
					base.gameObject.SetActive(value: false);
				});
			}
			else if (mario._instance.m_game_state != e_game_state.egs_edit)
			{
				if (mario._instance.m_game_state == e_game_state.egs_edit_play)
				{
					mario._instance.change_state(e_game_state.egs_edit_select, 1, delegate
					{
						base.gameObject.SetActive(value: false);
					});
				}
				else if (mario._instance.m_game_state == e_game_state.egs_edit_upload)
				{
					s_message s_message2 = new s_message();
					s_message2.m_type = "play_gui_uplw_ok";
					s_message2.m_object.Add(message);
					s_message s_message3 = new s_message();
					s_message3.m_type = "play_gui_uplw_cancel";
					mario._instance.show_double_dialog_box(game_data._instance.get_language_string("play_gui_uplw"), s_message2, s_message3);
				}
				else if (mario._instance.m_game_state == e_game_state.egs_br_play)
				{
					cmsg_mission_success cmsg_mission_success = new cmsg_mission_success();
					cmsg_mission_success.point = (int)message.m_ints[0];
					cmsg_mission_success.time = (int)message.m_ints[1];
					net_http._instance.send_msg(opclient_t.OPCODE_MISSION_SUCCESS, cmsg_mission_success, restart: true, string.Empty, 10f);
				}
			}
		}
		if (message.m_type == "play_lose")
		{
			m_win = 1;
			if (mario._instance.m_game_state == e_game_state.egs_play)
			{
				mario_point mario_point2 = (mario_point)message.m_object[0];
				cmsg_complete_map cmsg_complete_map2 = new cmsg_complete_map();
				cmsg_complete_map2.suc = 1;
				cmsg_complete_map2.x = mario_point2.x;
				cmsg_complete_map2.y = mario_point2.y;
				net_http._instance.send_msg(opclient_t.OPCODE_COMPLETE_MAP, cmsg_complete_map2, restart: true, string.Empty, 10f);
			}
			else if (mario._instance.m_game_state == e_game_state.egs_review)
			{
				mario._instance.change_state(e_game_state.egs_play_select, 1, delegate
				{
					base.gameObject.SetActive(value: false);
				});
			}
			else if (mario._instance.m_game_state != e_game_state.egs_edit)
			{
				if (mario._instance.m_game_state == e_game_state.egs_edit_play)
				{
					m_play_lose.SetActive(value: true);
				}
				else if (mario._instance.m_game_state == e_game_state.egs_edit_upload)
				{
					m_play_lose.SetActive(value: true);
				}
				else if (mario._instance.m_game_state == e_game_state.egs_br_play)
				{
					mario_point mario_point3 = (mario_point)message.m_object[0];
					cmsg_mission_fail cmsg_mission_fail = new cmsg_mission_fail();
					cmsg_mission_fail.x = mario_point3.x;
					cmsg_mission_fail.y = mario_point3.y;
					net_http._instance.send_msg(opclient_t.OPCODE_MISSION_FAIL, cmsg_mission_fail, restart: true, string.Empty, 10f);
				}
			}
		}
		if (message.m_type == "add_score")
		{
			int num = (int)message.m_ints[0];
			int num2 = (int)message.m_ints[1];
			int s = (int)message.m_ints[2];
			GameObject gameObject = (GameObject)Object.Instantiate(m_score_sub);
			gameObject.transform.parent = m_score_panel.transform;
			gameObject.transform.localPosition = new Vector3(num, num2, 0f);
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.GetComponent<score>().reset(s);
			gameObject.SetActive(value: true);
			m_scores.Add(gameObject);
		}
		if (message.m_type == "time_up")
		{
			m_time_up.SetActive(value: true);
		}
		if (message.m_type == "play_gui_uplw_ok")
		{
			s_message s_message4 = (s_message)message.m_object[0];
			List<int> input2 = (List<int>)s_message4.m_object[0];
			cmsg_upload_map cmsg_upload_map = new cmsg_upload_map();
			cmsg_upload_map.id = game_data._instance.m_map_id;
			cmsg_upload_map.ver = 2;
			cmsg_upload_map.time = (int)s_message4.m_ints[1];
			cmsg_upload_map.video = game_data._instance.save_inputs(input2);
			net_http._instance.send_msg(opclient_t.OPCODE_UPLOAD_MAP, cmsg_upload_map, restart: true, string.Empty, 10f);
		}
		if (message.m_type == "play_gui_uplw_cancel")
		{
			cmsg_upload_map cmsg_upload_map2 = new cmsg_upload_map();
			cmsg_upload_map2.id = game_data._instance.m_map_id;
			cmsg_upload_map2.ver = 2;
			net_http._instance.send_msg(opclient_t.OPCODE_UPLOAD_MAP, cmsg_upload_map2, restart: true, string.Empty, 10f);
		}
	}

	public void net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.OPCODE_COMPLETE_MAP)
		{
			smsg_complete_map smsg_complete_map = net_http._instance.parse_packet<smsg_complete_map>(message.m_byte);
			if (m_win == 0)
			{
				bool next = false;
				if (smsg_complete_map.mapid != 0)
				{
					mario._instance.m_self.mapid = smsg_complete_map.mapid;
					mario._instance.m_self.support = smsg_complete_map.support;
					int num = game_data._instance.get_zm(smsg_complete_map.support);
					if (num > 0 && num <= mario._instance.m_self.level)
					{
						next = true;
					}
				}
				mario._instance.m_self.add_exp(smsg_complete_map.exp + smsg_complete_map.extra_exp);
				if (mario._instance.m_self.m_review == 0)
				{
					base.gameObject.SetActive(value: false);
					mario._instance.show_clear_gui(smsg_complete_map.exp, smsg_complete_map.extra_exp, smsg_complete_map.rank, smsg_complete_map.testify, next);
				}
				else
				{
					mario._instance.change_state(e_game_state.egs_play_select, 1, delegate
					{
						base.gameObject.SetActive(value: false);
					});
				}
			}
			else
			{
				m_play_lose.SetActive(value: true);
			}
		}
		if (message.m_opcode == opclient_t.OPCODE_REPLAY_MAP)
		{
			if (message.m_res == -1)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("play_gui_tlbz"));
				msg_life_error msg_life_error = net_http._instance.parse_packet<msg_life_error>(message.m_byte);
				mario._instance.m_self.set_reset_time(msg_life_error.server_time, msg_life_error.life_time);
				return;
			}
			mario_tool._instance.onRaid(game_data._instance.m_map_id.ToString(), 1);
			m_play_pause.GetComponent<ui_show_anim>().hide_ui();
			m_play_lose.GetComponent<ui_show_anim>().hide_ui();
			mario._instance.show_play_mask(delegate
			{
				play_mode._instance.reload();
				m_time_up.SetActive(value: false);
			});
			mario._instance.play_mus_ex(game_data._instance.get_map_music(0), loop: true, 1 - game_data._instance.m_map_data.no_music);
		}
		if (message.m_opcode == opclient_t.OPCODE_UPLOAD_MAP)
		{
			mario._instance.show_tip(game_data._instance.get_language_string("play_gui_sccg"));
			mario._instance.m_self.add_job_exp(100);
			mario._instance.m_self.upload++;
			mario._instance.change_state(e_game_state.egs_edit_select, 1, delegate
			{
				base.gameObject.SetActive(value: false);
			});
		}
		if (message.m_opcode == opclient_t.OPCODE_MISSION_FAIL)
		{
			if (message.m_res == -1)
			{
				try
				{
					mario._instance.m_self.m_finish = net_http._instance.parse_packet<smsg_mission_finish>(message.m_byte);
					mario._instance.m_self.m_finish_type = 1;
					mario._instance.m_self.br_start = 0;
					mario._instance.m_self.set_br(0, string.Empty, string.Empty, string.Empty);
					mario._instance.change_state(e_game_state.egs_br_end, 1, delegate
					{
						base.gameObject.SetActive(value: false);
					});
				}
				catch (System.Exception e)
				{
					Debug.Log(e);
				}
			}
			else
			{
				m_brplay_lose.SetActive(value: true);
			}
		}
		if (message.m_opcode == opclient_t.OPCODE_MISSION_REPLAY)
		{
			mario._instance.m_self.br_life--;
			mario._instance.m_start_type = 1;
			mario._instance.change_state(e_game_state.egs_br_start, 1, delegate
			{
				base.gameObject.SetActive(value: false);
			});
		}
		if (message.m_opcode == opclient_t.OPCODE_MISSION_SUCCESS)
		{
			if (message.m_res == -1)
			{
				mario._instance.m_self.m_finish = net_http._instance.parse_packet<smsg_mission_finish>(message.m_byte);
				mario._instance.m_self.m_finish_type = 0;
				mario._instance.m_self.br_start = 2;
				mario._instance.m_self.set_br(0, string.Empty, string.Empty, string.Empty);
				mario._instance.show_clear_gui(next: false);
				base.gameObject.SetActive(value: false);
			}
			else
			{
				smsg_mission_play smsg_mission_play = net_http._instance.parse_packet<smsg_mission_play>(message.m_byte);
				mario._instance.m_self.set_br(smsg_mission_play.user_head, smsg_mission_play.user_country, smsg_mission_play.user_name, smsg_mission_play.map_name);
				game_data._instance.load_mission(-1, smsg_mission_play.map_data, smsg_mission_play.x, smsg_mission_play.y);
				mario._instance.m_self.br_index++;
				mario._instance.m_start_type = 2;
				mario._instance.show_clear_gui(next: true);
				base.gameObject.SetActive(value: false);
			}
		}
	}

	private void pause()
	{
		if (!play_mode._instance.m_pause)
		{
			play_mode._instance.m_pause = true;
			if (mario._instance.m_game_state == e_game_state.egs_br_play)
			{
				m_brplay_pause.SetActive(value: true);
			}
			else
			{
				m_play_pause.SetActive(value: true);
			}
		}
		else
		{
			play_mode._instance.m_pause = false;
			if (mario._instance.m_game_state == e_game_state.egs_br_play)
			{
				m_brplay_pause.GetComponent<ui_show_anim>().hide_ui();
			}
			else
			{
				m_play_pause.GetComponent<ui_show_anim>().hide_ui();
			}
		}
	}

	private void restart()
	{
		if (mario._instance.m_game_state == e_game_state.egs_play)
		{
			cmsg_replay_map obj = new cmsg_replay_map();
			net_http._instance.send_msg(opclient_t.OPCODE_REPLAY_MAP, obj, restart: true, string.Empty, 10f);
			return;
		}
		if (mario._instance.m_game_state == e_game_state.egs_br_play)
		{
			if (mario._instance.m_self.br_life <= 1)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("play_gui_smbz"));
				return;
			}
			cmsg_mission_replay obj2 = new cmsg_mission_replay();
			net_http._instance.send_msg(opclient_t.OPCODE_MISSION_REPLAY, obj2, restart: true, string.Empty, 10f);
			return;
		}
		m_play_lose.GetComponent<ui_show_anim>().hide_ui();
		m_play_pause.GetComponent<ui_show_anim>().hide_ui();
		mario._instance.show_play_mask(delegate
		{
			play_mode._instance.reload();
			m_time_up.SetActive(value: false);
		});
		mario._instance.play_mus_ex(game_data._instance.get_map_music(0), loop: true, 1 - game_data._instance.m_map_data.no_music);
	}

	public static void Return()
	{
		play_gui._instance.click(new GameObject() { name = "return"} );
	}

	private void click(GameObject obj)
	{
		if (obj.name == "pause")
		{
			pause();
		}
		if (obj.name == "restart")
		{
			restart();
		}
		if (obj.name == "continue")
		{
			pause();
		}
		if (!(obj.name == "return"))
		{
			return;
		}
		if (mario._instance.m_game_state == e_game_state.egs_play)
		{
			mario._instance.change_state(e_game_state.egs_play_select, 1, delegate
			{
				base.gameObject.SetActive(value: false);
			});
		}
		else if (mario._instance.m_game_state == e_game_state.egs_review)
		{
			mario._instance.change_state(e_game_state.egs_play_select, 1, delegate
			{
				base.gameObject.SetActive(value: false);
			});
		}
		else if (mario._instance.m_game_state == e_game_state.egs_edit_play)
		{
			mario._instance.change_state(e_game_state.egs_edit_select, 1, delegate
			{
				base.gameObject.SetActive(value: false);
			});
		}
		else if (mario._instance.m_game_state == e_game_state.egs_edit_upload)
		{
			mario._instance.change_state(e_game_state.egs_edit_select, 1, delegate
			{
				base.gameObject.SetActive(value: false);
			});
		}
		else if (mario._instance.m_game_state == e_game_state.egs_br_play)
		{
			mario._instance.change_state(e_game_state.egs_play_select, 1, delegate
			{
				base.gameObject.SetActive(value: false);
			});
		}
	}

	private void br_end()
	{
		mario._instance.m_self.m_finish_type = 1;
		mario._instance.change_state(e_game_state.egs_br_end, 1, delegate
		{
			base.gameObject.SetActive(value: false);
		});
	}

	private void calc_touch()
	{
        if (Application.isEditor || utils.IsPC)
        {
            return;
        }
        List<Vector3> list = new List<Vector3>();
        for (int i = 0; i < m_buttons.Count; i++)
        {
            Vector3 zero = Vector3.zero;
            Transform parent = m_buttons[i].transform;
            do
            {
                zero += parent.localPosition;
                parent = parent.parent;
            }
            while (parent.gameObject != UICamera.currentCamera.gameObject);
            list.Add(zero);
        }
        if (list.Count == 0)
        {
            return;
        }
        List<bool> list2 = new List<bool>();
        for (int j = 0; j < list.Count; j++)
        {
            list2.Add(false);
        }
        for (int k = 0; k < Input.touchCount; k++)
        {
            Vector3 vector = Input.GetTouch(k).position;
            Vector3 position = UICamera.currentCamera.ScreenToWorldPoint(new Vector3(vector.x, vector.y, 0f));
            position = UICamera.currentCamera.transform.InverseTransformPoint(position);
            for (int l = 0; l < list.Count; l++)
            {
                if ((list[l] - position).magnitude < 100f)
                {
                    list2[l] = true;
                }
            }
        }
        for (int m = 0; m < list.Count; m++)
        {
            if (m == 3)
            {
                if (list2[m] && !play_mode._instance.m_now_inputs[m])
                {
                    m_buttons[m].GetComponent<UISprite>().spriteName = "xtb_jinmen_01";
                    play_mode._instance.m_now_inputs[m] = true;
                }
                else if (!list2[m] && play_mode._instance.m_now_inputs[m])
                {
                    m_buttons[m].GetComponent<UISprite>().spriteName = "xtb_jinmen";
                    play_mode._instance.m_now_inputs[m] = false;
                }
            }
            else if (list2[m] && !play_mode._instance.m_now_inputs[m])
            {
                m_buttons[m].GetComponent<UISprite>().spriteName = "xtb_fx_01";
                play_mode._instance.m_now_inputs[m] = true;
            }
            else if (!list2[m] && play_mode._instance.m_now_inputs[m])
            {
                m_buttons[m].GetComponent<UISprite>().spriteName = "xtb_fx";
                play_mode._instance.m_now_inputs[m] = false;
            }
        }
    }

	private void FixedUpdate()
	{
		if (play_mode._instance == null)
		{
			return;
		}
		int num = game_data._instance.m_map_data.time - play_mode._instance.m_time / 50;
		if (num < 0)
		{
			num = 0;
		}
		string text = num.ToString();
		while (text.Length < 3)
		{
			text = "0" + text;
		}
		m_time_text.GetComponent<UILabel>().text = text;
		text = play_mode._instance.m_score.ToString();
		while (text.Length < 9)
		{
			text = "0" + text;
		}
		m_score_text.GetComponent<UILabel>().text = text;
		m_ys_text.GetComponent<UILabel>().text = "x" + play_mode._instance.m_ys;
		m_br_life_text.GetComponent<UILabel>().text = "x" + mario._instance.m_self.br_life;
		calc_touch();
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < m_scores.Count; i++)
		{
			m_scores[i].GetComponent<score>().update_ex();
			if (m_scores[i].GetComponent<score>().m_time > 50)
			{
				list.Add(m_scores[i]);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			m_scores.Remove(list[j]);
			Object.Destroy(list[j]);
		}
		m_score_panel.transform.localPosition = new Vector3(-play_mode._instance.m_roll.x / 10, (-play_mode._instance.m_roll.y - utils.g_roll_y) / 10);
		if (play_mode._instance.can_show_chuan())
		{
			if (utils.IsPC)
			{
				if (!m_x_buttom.activeSelf)
				{
					m_x_buttom.SetActive(value: true);
				}
				
			}
			else
			{
                if (!m_chuan.activeSelf)
                {
                    m_chuan.SetActive(true);
                }
            }
		}
		else if (m_x_buttom.activeSelf || m_chuan.activeSelf)
		{
			if (utils.IsPC)
			{
				m_x_buttom.SetActive(value: false);
			}
			else
			{
				m_chuan.SetActive(false);
            }
		}
	}

	private void Update()
	{
		if (!(play_mode._instance == null))
		{
			if (utils.IsPC)
			{
				if (mario._instance.key_down(KeyCode.LeftArrow) || mario._instance.key_down(KeyCode.RightArrow) || mario._instance.key_down(KeyCode.Z))
				{
					m_sm.SetActive(value: false);
				}
			}
			if (mario._instance.key_down(KeyCode.Escape))
			{
				pause();
			}
			if ((m_play_pause.activeSelf || m_play_lose.activeSelf || m_brplay_pause.activeSelf || m_brplay_lose.activeSelf) && mario._instance.key_down(KeyCode.X))
			{
				restart();
			}
		}
	}
}
