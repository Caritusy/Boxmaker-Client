using System.Collections.Generic;
using UnityEngine;

public class edit_gui : MonoBehaviour, IMessage
{
	private bool m_start;

	public GameObject m_tools;

	public GameObject m_switch;

	public GameObject m_fg;

	public GameObject m_save;

	public GameObject m_close;

	public GameObject m_time_tip;

	public GameObject m_ts;

	public GameObject m_ts1;

	public GameObject m_ts2;

	public GameObject m_ts3;

	public GameObject m_select;

	public GameObject m_select1;

	public GameObject m_end;

	public GameObject m_tip;

	public GameObject m_zuobiao;

	public GameObject m_left;

	public GameObject m_select2;

	private int m_index = 1;

	private int m_tuo_type;

	private Vector2 m_tuo_pos = default(Vector2);

	private int m_row;

	public GameObject m_row_num;

	public List<GameObject> m_selects;

	public List<GameObject> m_row_icons;

	public GameObject m_tool_xuan;

	public List<GameObject> m_mode_selects;

	public GameObject m_mode;

	public GameObject m_select_time;

	public GameObject m_time;

	public GameObject m_time_xuan;

	public GameObject m_fg_xuan;

	public mario_point m_change_point = new mario_point();

	private bool m_return;

	private bool m_need_save;

	private bool m_flag;

	public GameObject m_fg_view;

	public GameObject m_fg_sub;

	public List<GameObject> m_fg_subs;

	public GameObject m_joy_panel;

	public GameObject m_sm_panel;

	public GameObject m_sound;

	public GameObject m_tool_sub;

	public GameObject m_tool_view;

	public int m_world;

	public GameObject m_ending;

	public Joystick m_joy;

	private bool m_time_up;

	private int m_time_up_time;

	private bool m_time_down;

	private int m_time_down_time;

	private void Start()
	{
		cmessage_center._instance.add_handle(this);
		if (mario._instance.m_self.guide < 100)
		{
			mario._instance.m_self.guide = 0;
			jx1();
			s_message s_message2 = new s_message();
			s_message2.m_type = "play_mode";
			s_message2.m_object.Add(null);
			cmessage_center._instance.add_message(s_message2);
		}
		else
		{
			s_message s_message3 = new s_message();
			s_message3.m_type = "edit_mode";
			cmessage_center._instance.add_message(s_message3);
		}
		m_row = 0;
		reset_icon(is_change: true);
		change_time(game_data._instance.m_map_data.time);
		change_mode(game_data._instance.m_map_data.mode);
		reset_sound(game_data._instance.m_map_data.no_music);
		m_joy_panel.SetActive(!utils.IsPC);
		m_sm_panel.SetActive(utils.IsPC);
	}

	private void OnDestroy()
	{
		s_message s_message2 = new s_message();
		s_message2.m_type = "close_edit_mode";
		cmessage_center._instance.add_message(s_message2);
		cmessage_center._instance.remove_handle(this);
	}

	private void reset_icon(bool is_change = false)
	{
		if (is_change)
		{
			change_row(0);
		}
		else
		{
			change_row(m_row);
		}
		int num = (game_data._instance.m_unit_num + 7) / 8;
		mario._instance.remove_child(m_tool_view);
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(m_tool_sub);
			gameObject.name = (i + 1).ToString();
			gameObject.transform.parent = m_tool_view.transform;
			gameObject.transform.localPosition = new Vector3(0f, 170 - 80 * i, 0f);
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.SetActive(value: true);
			gameObject.transform.FindChild("row").FindChild("Label").GetComponent<UILabel>()
				.text = (i + 1).ToString();
			for (int j = 1; j <= 8; j++)
			{
				int num2 = i * 8 + j;
				s_t_unit s_t_unit2 = game_data._instance.get_t_unit_by_site(num2 - 1);
				if (s_t_unit2 == null)
				{
					gameObject.transform.FindChild(j.ToString()).FindChild("icon").GetComponent<UISprite>()
						.spriteName = "";
				}
				else if (s_t_unit2.kfg == 1)
				{
					gameObject.transform.FindChild(j.ToString()).FindChild("icon").GetComponent<UISprite>()
						.spriteName = s_t_unit2.icon + "_" + game_data._instance.m_map_data.maps[m_world].map_theme;
				}
				else
				{
					gameObject.transform.FindChild(j.ToString()).FindChild("icon").GetComponent<UISprite>()
						.spriteName = s_t_unit2.icon;
				}
			}
		}
		if (m_world != game_data._instance.m_map_data.end_area)
		{
			m_ending.SetActive(value: true);
		}
		else
		{
			m_ending.SetActive(value: false);
		}
	}

	private void jx1()
	{
		m_switch.SetActive(value: false);
		m_fg.SetActive(value: false);
		m_save.SetActive(value: false);
		m_close.SetActive(value: false);
		m_time_tip.SetActive(value: false);
		m_left.SetActive(value: false);
	}

	private void show_fg()
	{
		m_fg_xuan.SetActive(value: true);
		m_fg_subs.Clear();
		mario._instance.remove_child(m_fg_view);
		for (int i = 0; i < game_data._instance.m_fg_num; i++)
		{
			int num = i + 1;
			GameObject gameObject = (GameObject)Object.Instantiate(m_fg_sub);
			gameObject.transform.parent = m_fg_view.transform;
			gameObject.transform.localPosition = new Vector3(-94 + 380 * i, 0f, 0f);
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.GetComponent<fg_sub>().reset(num);
			if (num == game_data._instance.m_map_data.maps[m_world].map_theme)
			{
				gameObject.GetComponent<fg_sub>().set_gou(flag: true);
			}
			gameObject.SetActive(value: true);
			m_fg_subs.Add(gameObject);
		}
		int num2 = game_data._instance.m_map_data.maps[m_world].map_theme * 380 - 476;
		if (num2 < 0)
		{
			num2 = 0;
		}
		else if (game_data._instance.m_fg_num == game_data._instance.m_map_data.maps[m_world].map_theme)
		{
			num2 = game_data._instance.m_map_data.maps[m_world].map_theme * 380 - 564;
		}
		m_fg_view.transform.localPosition = new Vector3(-num2, 0f, 0f);
		m_fg_view.GetComponent<UIPanel>().clipOffset = new Vector2(num2, 0f);
	}

	private void select_area(GameObject obj)
	{
		int num = int.Parse(obj.name.Substring(5, 1));
		s_message s_message2 = new s_message();
		s_message2.m_type = "edit_qiehuan_area";
		s_message2.m_ints.Add(num - 1);
		mario._instance.show_double_dialog_box(string.Format(game_data._instance.get_language_string("edit_gui_qhqy"), num), s_message2);
	}

	private void select_fg(GameObject obj)
	{
		if (obj.GetComponent<fg_sub>().m_lk && !Application.isEditor)
		{
			return;
		}
		int id = obj.GetComponent<fg_sub>().m_id;
		if (id == game_data._instance.m_map_data.maps[m_world].map_theme)
		{
			m_fg_xuan.SetActive(value: false);
			return;
		}
		game_data._instance.m_map_data.maps[m_world].map_theme = id;
		for (int i = 0; i < m_fg_subs.Count; i++)
		{
			if (i + 1 != id)
			{
				m_fg_subs[i].GetComponent<fg_sub>().set_gou(flag: false);
			}
			else
			{
				m_fg_subs[i].GetComponent<fg_sub>().set_gou(flag: true);
			}
		}
		reset_icon();
		StartCoroutine(edit_mode._instance.reload_fg(m_world));
		m_need_save = true;
		m_fg_xuan.SetActive(value: false);
		m_flag = true;
	}

	public void click(GameObject obj)
	{
		if (obj.name == "switch" && mario._instance.m_self.guide != 4)
		{
			m_ts2.SetActive(value: false);
			if (!m_start)
			{
				edit_mode._instance.check_mission();
				s_message s_message2 = new s_message();
				s_message2.m_type = "play_mode";
				if (mario._instance.m_self.guide == 2)
				{
					mario._instance.m_self.guide = 3;
				}
				if (mario._instance.m_self.guide < 100)
				{
					s_message2.m_object.Add(null);
				}
				else
				{
					s_message2.m_object.Add(edit_mode._instance.m_main_char.m_pos);
					s_message2.m_ints.Add(m_world);
				}
				cmessage_center._instance.add_message(s_message2);
			}
			else
			{
				play_mode._instance.m_die_pos = play_mode._instance.m_main_char.m_pos;
				play_mode._instance.lose();
			}
		}
		else if (mario._instance.m_self.guide == 2)
		{
			mario._instance.show_tip(game_data._instance.get_language_string("edit_gui_djsw"));
			return;
		}
		if (obj.name == "save")
		{
			m_ts3.SetActive(value: false);
			if (mario._instance.m_self.guide == 4)
			{
				edit_mode._instance.check_mission();
				game_data._instance.save_mission_ex();
			}
			else if (m_need_save)
			{
				edit_mode._instance.check_mission();
				game_data._instance.save_mission();
				m_need_save = false;
			}
		}
		else if (mario._instance.m_self.guide == 4)
		{
			mario._instance.show_tip(game_data._instance.get_language_string("edit_gui_djbc"));
			return;
		}
		if (obj.name == "fg")
		{
			show_fg();
		}
		if (obj.name == "close_fg")
		{
			m_fg_xuan.SetActive(value: false);
			m_flag = true;
		}
		if (obj.name == "erase")
		{
			m_index = 0;
			m_select1.transform.localPosition = obj.transform.localPosition;
			m_select.SetActive(value: false);
			m_select1.SetActive(value: true);
		}
		if (obj.name == "change")
		{
			m_index = -1;
			m_select1.transform.localPosition = obj.transform.localPosition;
			m_select.SetActive(value: false);
			m_select1.SetActive(value: true);
		}
		if (obj.name == "down")
		{
			m_ts.SetActive(value: false);
			m_tool_xuan.SetActive(value: true);
		}
		if (obj.name == "close_xuan")
		{
			m_tool_xuan.SetActive(value: false);
			m_flag = true;
		}
		if (obj.name == "time_tip")
		{
			m_time_xuan.SetActive(value: true);
		}
		if (obj.name == "close_time")
		{
			m_time_xuan.SetActive(value: false);
			m_flag = true;
		}
		if (obj.name == "return")
		{
			s_message s_message3 = new s_message();
			s_message3.m_type = "edit_return_ok";
			mario._instance.show_double_dialog_box(game_data._instance.get_language_string("edit_gui_sfqd"), s_message3);
		}
		if (obj.name == "sound")
		{
			reset_sound(1 - game_data._instance.m_map_data.no_music);
			m_need_save = true;
		}
		if (obj.name == "ending")
		{
			game_data._instance.m_map_data.end_area = m_world;
			edit_mode._instance.reset_ending();
			m_ending.SetActive(value: false);
			mario._instance.show_tip(game_data._instance.get_language_string("edit_gui_swzd"));
			m_need_save = true;
		}
	}

	private void press(GameObject obj)
	{
		if (obj.name == "up_time")
		{
			m_time_up = true;
			m_time_up_time = 0;
			change_time(game_data._instance.m_map_data.time + 10);
			m_need_save = true;
		}
		if (obj.name == "down_time")
		{
			m_time_down = true;
			m_time_down_time = 0;
			change_time(game_data._instance.m_map_data.time - 10);
			m_need_save = true;
		}
	}

	private void release(GameObject obj)
	{
		if (obj.name == "up_time")
		{
			m_time_up = false;
			m_time_up_time = 0;
		}
		if (obj.name == "down_time")
		{
			m_time_down = false;
			m_time_down_time = 0;
		}
	}

	public void line_press(GameObject obj)
	{
		int num = (int)((mario._instance.get_mouse_position().x - (float)(Screen.width * utils.g_height / 10 / Screen.height / 2) + 200f) / 2f);
		if (num < 0)
		{
			num = 0;
		}
		else if (num >= game_data._instance.m_map_data.maps[m_world].x_num)
		{
			num = game_data._instance.m_map_data.maps[m_world].x_num - 1;
		}
		edit_mode._instance.reload_jump(new mario_point(num * utils.g_grid_size + utils.g_grid_size / 2, edit_mode._instance.m_main_char.m_pos.y), m_world);
	}

	public void select(GameObject obj)
	{
		int num = int.Parse(obj.name);
		int num2 = m_row * 8 + num;
		s_t_unit s_t_unit2 = game_data._instance.get_t_unit_by_site(num2 - 1);
		if (s_t_unit2 != null)
		{
			m_index = num;
			m_select.transform.localPosition = obj.transform.localPosition;
			m_select.SetActive(value: true);
			m_select1.SetActive(value: false);
		}
	}

	public void select_xuan(GameObject obj)
	{
		int num = int.Parse(obj.name);
		int num2 = int.Parse(obj.transform.parent.name);
		int num3 = (num2 - 1) * 8 + num;
		s_t_unit s_t_unit2 = game_data._instance.get_t_unit_by_site(num3 - 1);
		if (s_t_unit2 != null)
		{
			m_index = num;
			m_select.transform.localPosition = m_selects[m_index - 1].transform.localPosition;
			m_select.SetActive(value: true);
			m_select1.SetActive(value: false);
			m_tool_xuan.SetActive(value: false);
			change_row(num2 - 1);
			m_flag = true;
		}
	}

	public void change_row(int row)
	{
		if ((row - 1) * 8 >= game_data._instance.m_unit_num)
		{
			return;
		}
		m_row = row;
		m_row_num.GetComponent<UILabel>().text = (row + 1).ToString();
		for (int i = 0; i < 8; i++)
		{
			s_t_unit s_t_unit2 = game_data._instance.get_t_unit_by_site(m_row * 8 + i);
			if (s_t_unit2 == null)
			{
				m_row_icons[i].GetComponent<UISprite>().spriteName = string.Empty;
			}
			else if (s_t_unit2.kfg == 1)
			{
				m_row_icons[i].GetComponent<UISprite>().spriteName = s_t_unit2.icon + "_" + game_data._instance.m_map_data.maps[m_world].map_theme;
			}
			else
			{
				m_row_icons[i].GetComponent<UISprite>().spriteName = s_t_unit2.icon;
			}
		}
	}

	private void select_mode(GameObject obj)
	{
		int mode = int.Parse(obj.name) - 1;
		change_mode(mode);
		m_need_save = true;
	}

	private void change_mode(int mode)
	{
		for (int i = 0; i < 3; i++)
		{
			m_mode_selects[i].SetActive(value: false);
		}
		m_mode_selects[mode].SetActive(value: true);
		switch (mode)
		{
		case 0:
			m_mode.GetComponent<UISprite>().spriteName = "mode01";
			break;
		case 1:
			m_mode.GetComponent<UISprite>().spriteName = "mode02";
			break;
		default:
			m_mode.GetComponent<UISprite>().spriteName = "mode03";
			break;
		}
		game_data._instance.m_map_data.mode = mode;
		if (edit_mode._instance != null)
		{
			edit_mode._instance.reset_zhuiji();
		}
	}

	private void reset_sound(int no_sound)
	{
		game_data._instance.m_map_data.no_music = no_sound;
		if (no_sound == 0)
		{
			m_sound.GetComponent<UISprite>().spriteName = "xlb_001";
		}
		else
		{
			m_sound.GetComponent<UISprite>().spriteName = "xlb_002";
		}
	}

	private void change_time(int time)
	{
		m_time.GetComponent<UILabel>().text = time.ToString();
		m_select_time.GetComponent<UILabel>().text = time.ToString();
		game_data._instance.m_map_data.time = time;
	}

	public void message(s_message message)
	{
		if (message.m_type == "play_mode")
		{
			m_start = true;
			m_switch.GetComponent<UISprite>().spriteName = "edit-1";
			m_tools.SetActive(value: false);
			mario._instance.show_play_gui();
			mario._instance.play_mus(game_data._instance.get_map_music(m_world), loop: true, 1 - game_data._instance.m_map_data.no_music);
		}
		if (message.m_type == "edit_mode")
		{
			m_start = false;
			m_switch.GetComponent<UISprite>().spriteName = "play-1";
			m_tools.SetActive(value: true);
			mario._instance.hide_play_gui();
			mario._instance.play_mus("music/select", loop: true, 0.5f);
			if (message.m_ints.Count > 0)
			{
				m_world = (int)message.m_ints[0];
				m_select2.transform.localPosition = new Vector3(60f, 160 - 70 * m_world);
			}
			if (mario._instance.m_self.guide == 4)
			{
				s_message s_message2 = new s_message();
				s_message2.m_type = "jx_4";
				mario._instance.show_xsjx_dialog_box(game_data._instance.get_language_string("edit_gui_hlbc"), s_message2);
			}
		}
		if (message.m_type == "edit_return_ok")
		{
			if (m_need_save)
			{
				s_message s_message3 = new s_message();
				s_message3.m_type = "edit_return_save_ok";
				s_message s_message4 = new s_message();
				s_message4.m_type = "edit_return_save_cancel";
				mario._instance.show_double_dialog_box(game_data._instance.get_language_string("edit_gui_sfbc"), s_message3, s_message4);
			}
			else
			{
				mario._instance.change_state(e_game_state.egs_edit_select, 1, delegate
				{
					Object.Destroy(base.gameObject);
				});
			}
			m_return = true;
		}
		if (message.m_type == "edit_return_save_ok")
		{
			edit_mode._instance.check_mission();
			game_data._instance.save_mission();
			m_need_save = false;
		}
		if (message.m_type == "edit_return_save_cancel")
		{
			mario._instance.change_state(e_game_state.egs_edit_select, 1, delegate
			{
				Object.Destroy(base.gameObject);
			});
		}
		if (message.m_type == "jx_1")
		{
			play_mode._instance.lose();
			s_message s_message5 = new s_message();
			s_message5.m_type = "jx_2";
			mario._instance.show_xsjx_dialog_box(game_data._instance.get_language_string("edit_gui_xyts"), s_message5);
		}
		if (message.m_type == "jx_2")
		{
			mario._instance.m_self.guide = 1;
			edit_mode._instance.create_xy();
			m_ts.SetActive(value: true);
			m_ts1.SetActive(value: true);
		}
		if (message.m_type == "jx_3")
		{
			m_switch.SetActive(value: true);
			m_ts2.SetActive(value: true);
		}
		if (message.m_type == "jx_4")
		{
			m_save.SetActive(value: true);
			m_ts3.SetActive(value: true);
		}
		if (message.m_type == "jx_5")
		{
			mario._instance.change_state(e_game_state.egs_edit_select, 1, delegate
			{
				Object.Destroy(base.gameObject);
			});
		}
		if (message.m_type == "edit_qiehuan_area")
		{
			m_world = (int)message.m_ints[0];
			m_select2.transform.localPosition = new Vector3(60f, 160 - 70 * m_world);
			reset_icon();
			edit_mode._instance.reload_self(m_world);
		}
	}

	public void net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.OPCODE_SAVE_MAP && m_return)
		{
			mario._instance.change_state(e_game_state.egs_edit_select, 1, delegate
			{
				Object.Destroy(base.gameObject);
			});
		}
		if (message.m_opcode == opclient_t.OPCODE_COMPLETE_GUIDE)
		{
			mario._instance.m_self.guide = 200;
			s_message s_message2 = new s_message();
			s_message2.m_type = "jx_5";
			mario._instance.show_xsjx_dialog_box(game_data._instance.get_language_string("edit_gui_wcjx"), s_message2);
		}
	}

	private void FixedUpdate()
	{
		if (mario._instance.m_self.guide == 1)
		{
			int num = utils.jx_block.Length / 3;
			for (int i = 0; i < num; i++)
			{
				int num2 = utils.jx_block[i * 3];
				int num3 = utils.jx_block[i * 3 + 1];
				int num4 = utils.jx_block[i * 3 + 2];
				if (num2 >= game_data._instance.m_map_data.maps[m_world].x_num)
				{
					return;
				}
				if (game_data._instance.m_arrays[m_world][num3][num2].type != num4)
				{
					edit_mode._instance.set_dlg(num2, num3);
					return;
				}
			}
			mario._instance.m_self.guide = 2;
			s_message s_message2 = new s_message();
			s_message2.m_type = "jx_3";
			mario._instance.show_xsjx_dialog_box(game_data._instance.get_language_string("edit_gui_swdt"), s_message2);
		}
		if (m_time_up)
		{
			m_time_up_time++;
			if (m_time_up_time >= 30 && m_time_up_time % 5 == 0)
			{
				change_time(game_data._instance.m_map_data.time + 10);
				m_need_save = true;
			}
		}
		if (m_time_down)
		{
			m_time_down_time++;
			if (m_time_down_time >= 30 && m_time_down_time % 5 == 0)
			{
				change_time(game_data._instance.m_map_data.time - 10);
				m_need_save = true;
			}
		}
	}

	private void Update()
	{
		if (m_start || edit_mode._instance == null || edit_mode._instance.m_main_char == null)
		{
			return;
		}
		if (m_joy.IsHolding)
		{
			m_ts1.SetActive(value: false);
			return;
		}
		if (utils.IsPC)
		{
			if (mario._instance.key_down(KeyCode.LeftArrow) || mario._instance.key_down(KeyCode.RightArrow) || mario._instance.key_down(KeyCode.Z))
			{
				m_sm_panel.SetActive(value: false);
			}
		}
		if (mario._instance.get_mouse_button())
		{
			if (mario._instance.m_self.guide == 2)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("edit_gui_djsw"));
				return;
			}
			if (mario._instance.m_self.guide == 4)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("edit_gui_djbc"));
				return;
			}
			Vector2 mouse_position = mario._instance.get_mouse_position();
			bool flag = true;
			if (mouse_position.x <= 96f || mouse_position.x >= (float)(Screen.width * utils.g_height / Screen.height - 96) || mouse_position.y <= 48f || mouse_position.y >= 560f)
			{
				flag = false;
			}
			Ray ray = mario._instance.m_ui_camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out var _, 20f))
			{
				m_flag = true;
			}
			if (m_flag)
			{
				flag = false;
			}
			float num = mouse_position.x - (float)(Screen.width * utils.g_height / 10 / Screen.height / 2);
			float num2 = mouse_position.y - 64f;
			int num3 = (int)((double)num / 0.8 * 10.0 + (double)edit_mode._instance.m_roll.x);
			int num4 = (int)((double)num2 / 0.8 * 10.0 + (double)edit_mode._instance.m_roll.y);
			int num5 = num3 / utils.g_grid_size;
			int num6 = num4 / utils.g_grid_size;
			if (num5 == 1 && num6 == game_data._instance.m_map_data.maps[m_world].qd_y && m_tuo_type == 0)
			{
				m_tuo_type = 1;
				m_tuo_pos = mouse_position;
			}
			else if (num5 == game_data._instance.m_map_data.maps[m_world].x_num - 2 && num6 == game_data._instance.m_map_data.maps[m_world].zd_y && m_tuo_type == 0)
			{
				m_tuo_type = 2;
				m_tuo_pos = mouse_position;
			}
			else if (m_tuo_type == 0 && flag && (m_change_point.x != num5 || m_change_point.y != num6))
			{
				m_change_point = new mario_point(num5, num6);
				int num7 = m_index;
				if (m_index > 0)
				{
					num7 = m_row * 8 + m_index;
					s_t_unit s_t_unit2 = game_data._instance.get_t_unit_by_site(num7 - 1);
					num7 = s_t_unit2.id;
				}
				if (num7 != 0 && mario._instance.m_self.guide == 1)
				{
					bool flag2 = false;
					int num8 = utils.jx_block.Length / 3;
					for (int i = 0; i < num8; i++)
					{
						int num9 = utils.jx_block[i * 3];
						int num10 = utils.jx_block[i * 3 + 1];
						int num11 = utils.jx_block[i * 3 + 2];
						if (num10 == num6 && num9 == num5)
						{
							flag2 = true;
							if (num7 != num11)
							{
								mario._instance.show_tip(game_data._instance.get_language_string("edit_gui_zlbd"));
								return;
							}
						}
					}
					if (!flag2)
					{
						mario._instance.show_tip(game_data._instance.get_language_string("edit_gui_qaxy"));
						return;
					}
				}
				edit_mode._instance.add_obj(num5, num6, num7);
				m_need_save = true;
			}
			if (m_tuo_type > 0)
			{
				int num12 = 0;
				if (mouse_position.x - m_tuo_pos.x > 50f)
				{
					num12 = 1;
				}
				else if (mouse_position.x - m_tuo_pos.x < -50f)
				{
					num12 = -1;
				}
				int num13 = 0;
				if (mouse_position.y - m_tuo_pos.y > 50f)
				{
					num13 = 1;
				}
				else if (mouse_position.y - m_tuo_pos.y < -50f)
				{
					num13 = -1;
				}
				if (num12 != 0 || num13 != 0)
				{
					edit_mode._instance.add_qz(m_tuo_type, num12, num13);
					m_need_save = true;
				}
			}
			else
			{
				edit_mode._instance.add_qz(0, 0, 0);
			}
			edit_mode._instance.set_mpos(num3, num4);
		}
		else
		{
			m_change_point = new mario_point();
			m_tuo_type = 0;
			edit_mode._instance.not_add_obj();
			edit_mode._instance.add_qz(0, 0, 0);
			m_flag = false;
		}
		m_end.transform.localPosition = new Vector3(game_data._instance.m_map_data.maps[m_world].x_num * 2 - 200 + 32, 42f, 0f);
		m_tip.transform.localPosition = new Vector3(edit_mode._instance.m_main_char.m_pos.x / utils.g_grid_size * 2 - 200, 42f, 0f);
		m_zuobiao.GetComponent<UILabel>().text = "x:" + edit_mode._instance.m_main_char.m_pos.x / utils.g_grid_size + " y:" + edit_mode._instance.m_main_char.m_pos.y / utils.g_grid_size;
	}
}
