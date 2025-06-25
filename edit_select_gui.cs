using System.Collections.Generic;
using protocol.game;
using UnityEngine;

public class edit_select_gui : MonoBehaviour, IMessage
{
	public GameObject m_clip;

	public GameObject m_panel;

	public GameObject m_info;

	public GameObject m_cn;

	public GameObject m_cn_text;

	public GameObject m_name;

	public GameObject m_rq;

	public GameObject m_texture;

	private List<GameObject> m_clips = new List<GameObject>();

	private List<edit_data> m_ed = new List<edit_data>();

	private int m_index;

	private int m_player_type;

	private string m_cname;

	public GameObject m_level;

	public GameObject m_exp_bar;

	public GameObject m_exp_text;

	public GameObject m_tip;

	private void Start()
	{
		cmessage_center._instance.add_handle(this);
		reset();
	}

	private void OnDestroy()
	{
		cmessage_center._instance.remove_handle(this);
	}

	public void reset()
	{
		mario._instance.show_user(base.gameObject);
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				GameObject gameObject = (GameObject)Object.Instantiate(m_clip);
				gameObject.name = "clip" + (i * 4 + j);
				gameObject.transform.parent = m_panel.transform;
				gameObject.transform.localPosition = new Vector3(-345 + 230 * j, 100 - 160 * i, 0f);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject.GetComponent<UIButtonMessage>().target = base.gameObject;
				gameObject.SetActive(value: true);
				m_clips.Add(gameObject);
			}
		}
		cmsg_view_edit obj = new cmsg_view_edit();
		net_http._instance.send_msg(opclient_t.OPCODE_VIEW_EDIT, obj, restart: true, string.Empty, 10f);
	}

	private void OnEnable()
	{
		if (mario._instance.m_self.guide < 100)
		{
			mario._instance.m_self.job_level = 1;
			mario._instance.m_self.job_exp = 0;
			reset_exp();
		}
		else if (m_info.gameObject.activeSelf)
		{
			cmsg_view_edit_single cmsg_view_edit_single = new cmsg_view_edit_single();
			cmsg_view_edit_single.map_id = m_ed[m_index].id;
			net_http._instance.send_msg(opclient_t.OPCODE_VIEW_EDIT_SINGLE, cmsg_view_edit_single, restart: true, string.Empty, 10f);
			reset_exp();
		}
		else
		{
			cmsg_view_edit obj = new cmsg_view_edit();
			net_http._instance.send_msg(opclient_t.OPCODE_VIEW_EDIT, obj, restart: true, string.Empty, 10f);
		}
	}

	public void guide()
	{
		TextAsset textAsset = Resources.Load("mission/jx") as TextAsset;
		game_data._instance.load_mission(0, textAsset.bytes, null, null);
		mario._instance.change_state(e_game_state.egs_edit, 0, delegate
		{
			base.gameObject.SetActive(value: false);
		});
	}

	private void reset_exp()
	{
		m_level.GetComponent<UILabel>().text = mario._instance.m_self.job_level.ToString();
		s_t_job_exp s_t_job_exp2 = game_data._instance.get_t_job_exp(mario._instance.m_self.job_level + 1);
		if (s_t_job_exp2 != null)
		{
			float value = (float)mario._instance.m_self.job_exp / (float)s_t_job_exp2.exp;
			m_exp_bar.GetComponent<UIProgressBar>().value = value;
			m_exp_text.GetComponent<UILabel>().text = mario._instance.m_self.job_exp + "/" + s_t_job_exp2.exp;
		}
		else
		{
			m_exp_bar.GetComponent<UIProgressBar>().value = 1f;
			m_exp_text.GetComponent<UILabel>().text = mario._instance.m_self.job_exp + "/--";
		}
		s_t_job_exp2 = game_data._instance.get_t_job_exp(mario._instance.m_self.job_level);
	}

	private void reset_info()
	{
		m_rq.GetComponent<UILabel>().text = m_ed[m_index].date;
		m_name.GetComponent<UILabel>().text = m_ed[m_index].name;
		m_texture.GetComponent<UITexture>().mainTexture = game_data._instance.mission_to_texture(m_ed[m_index].url);
	}

	private void reset_index()
	{
		if (m_ed[m_index].id == 0)
		{
			m_clips[m_index].transform.FindChild("name").GetComponent<UILabel>().text = string.Empty;
			m_clips[m_index].transform.FindChild("Texture").GetComponent<UITexture>().mainTexture = game_data._instance.mission_to_texture(new byte[0]);
			m_clips[m_index].transform.FindChild("state").gameObject.SetActive(value: false);
			m_info.GetComponent<ui_show_anim>().hide_ui();
			return;
		}
		m_clips[m_index].transform.FindChild("name").GetComponent<UILabel>().text = m_ed[m_index].name;
		m_clips[m_index].transform.FindChild("Texture").GetComponent<UITexture>().mainTexture = game_data._instance.mission_to_texture(m_ed[m_index].url);
		if (m_ed[m_index].upload == 1)
		{
			m_clips[m_index].transform.FindChild("state").gameObject.SetActive(value: true);
		}
		reset_info();
	}

	public void message(s_message message)
	{
		if (message.m_type == "edit_delete_map")
		{
			cmsg_delete_map cmsg_delete_map = new cmsg_delete_map();
			cmsg_delete_map.id = m_ed[m_index].id;
			net_http._instance.send_msg(opclient_t.OPCODE_DELETE_MAP, cmsg_delete_map, restart: true, string.Empty, 10f);
		}
		if (message.m_type == "edit_upload_map")
		{
			m_player_type = 2;
			cmsg_play_edit_map cmsg_play_edit_map = new cmsg_play_edit_map();
			cmsg_play_edit_map.id = m_ed[m_index].id;
			net_http._instance.send_msg(opclient_t.OPCODE_PLAY_EDIT_MAP, cmsg_play_edit_map, restart: true, string.Empty, 10f);
		}
		if (message.m_type == "jx_6")
		{
			mario._instance.change_state(e_game_state.egs_play_select, 1, delegate
			{
				Object.Destroy(base.gameObject);
			});
		}
	}

	public void net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.OPCODE_VIEW_EDIT)
		{
			smsg_view_edit smsg_view_edit = net_http._instance.parse_packet<smsg_view_edit>(message.m_byte);
			m_ed = smsg_view_edit.infos;
			for (int i = 0; i < smsg_view_edit.infos.Count; i++)
			{
				edit_data edit_data = smsg_view_edit.infos[i];
				if (edit_data.id != 0)
				{
					m_clips[i].transform.FindChild("name").GetComponent<UILabel>().text = edit_data.name;
					m_clips[i].transform.FindChild("Texture").GetComponent<UITexture>().mainTexture = game_data._instance.mission_to_texture(edit_data.url);
					if (edit_data.upload == 1)
					{
						m_clips[i].transform.FindChild("state").gameObject.SetActive(value: true);
					}
				}
			}
			mario._instance.m_self.job_level = smsg_view_edit.level;
			mario._instance.m_self.job_exp = smsg_view_edit.exp;
			reset_exp();
			if (mario._instance.m_self.guide == 200)
			{
				s_message s_message2 = new s_message();
				s_message2.m_type = "jx_6";
				mario._instance.show_xsjx_dialog_box(game_data._instance.get_language_string("edit_gui_pljx"), s_message2);
			}
		}
		if (message.m_opcode == opclient_t.OPCODE_CREATE_MAP)
		{
			smsg_create_map smsg_create_map = net_http._instance.parse_packet<smsg_create_map>(message.m_byte);
			m_ed[m_index] = smsg_create_map.map;
			reset_index();
			m_info.SetActive(value: true);
		}
		if (message.m_opcode == opclient_t.OPCODE_PLAY_EDIT_MAP)
		{
			smsg_play_edit_map smsg_play_edit_map = net_http._instance.parse_packet<smsg_play_edit_map>(message.m_byte);
			if (!game_data._instance.load_mission(m_ed[m_index].id, smsg_play_edit_map.mapdata, null, null))
			{
				return;
			}
			if (m_player_type == 0)
			{
				mario._instance.change_state(e_game_state.egs_edit, 1, delegate
				{
					base.gameObject.SetActive(value: false);
				});
			}
			else if (m_player_type == 1)
			{
				mario._instance.change_state(e_game_state.egs_edit_play, 2, delegate
				{
					base.gameObject.SetActive(value: false);
				});
			}
			else if (m_player_type == 2)
			{
				mario._instance.change_state(e_game_state.egs_edit_upload, 2, delegate
				{
					base.gameObject.SetActive(value: false);
				});
			}
		}
		if (message.m_opcode == opclient_t.OPCODE_DELETE_MAP)
		{
			m_ed[m_index].id = 0;
			reset_index();
		}
		if (message.m_opcode == opclient_t.OPCODE_CHANGE_MAP_NAME)
		{
			m_cn.GetComponent<ui_show_anim>().hide_ui();
			m_ed[m_index].name = m_cname;
			reset_index();
		}
		if (message.m_opcode == opclient_t.OPCODE_VIEW_EDIT_SINGLE)
		{
			smsg_view_edit_single smsg_view_edit_single = net_http._instance.parse_packet<smsg_view_edit_single>(message.m_byte);
			m_ed[m_index] = smsg_view_edit_single.info;
			reset_index();
		}
	}

	private void click(GameObject obj)
	{
		if (obj.name == "close")
		{
			mario._instance.change_state(e_game_state.egs_login, 1, delegate
			{
				Object.Destroy(base.gameObject);
			});
		}
		if (obj.name == "close_info")
		{
			m_info.GetComponent<ui_show_anim>().hide_ui();
		}
		if (obj.name == "edit")
		{
			if (m_ed[m_index].upload == 1)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("edit_select_gui_wfxg"));
				return;
			}
			m_player_type = 0;
			cmsg_play_edit_map cmsg_play_edit_map = new cmsg_play_edit_map();
			cmsg_play_edit_map.id = m_ed[m_index].id;
			net_http._instance.send_msg(opclient_t.OPCODE_PLAY_EDIT_MAP, cmsg_play_edit_map, restart: true, string.Empty, 10f);
		}
		if (obj.name == "play")
		{
			m_player_type = 1;
			cmsg_play_edit_map cmsg_play_edit_map2 = new cmsg_play_edit_map();
			cmsg_play_edit_map2.id = m_ed[m_index].id;
			net_http._instance.send_msg(opclient_t.OPCODE_PLAY_EDIT_MAP, cmsg_play_edit_map2, restart: true, string.Empty, 10f);
		}
		if (obj.name == "upload")
		{
			if (mario._instance.m_self.visitor == 1)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("edit_select_gui_ykwf"));
				return;
			}
			if (m_ed[m_index].upload == 1)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("edit_select_gui_wfsc"));
				return;
			}
			if (m_ed[m_index].url.Length == 0)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("edit_select_gui_wfwbj"));
				return;
			}
			if (m_ed[m_index].name == "empty")
			{
				mario._instance.show_tip(game_data._instance.get_language_string("edit_select_gui_xgmz"));
				return;
			}
			s_message s_message2 = new s_message();
			s_message2.m_type = "edit_upload_map";
			mario._instance.show_double_dialog_box(game_data._instance.get_language_string("edit_select_gui_tgsc"), s_message2);
		}
		if (obj.name == "delete")
		{
			s_message s_message3 = new s_message();
			s_message3.m_type = "edit_delete_map";
			mario._instance.show_double_dialog_box(game_data._instance.get_language_string("edit_select_gui_sfsc"), s_message3);
		}
		if (obj.name == "cn")
		{
			if (m_ed[m_index].upload == 1)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("edit_select_gui_wfxg"));
				return;
			}
			m_cn_text.GetComponent<UIInput>().value = m_ed[m_index].name;
			m_cn.SetActive(value: true);
		}
		if (obj.name == "cnok")
		{
			string value = m_cn_text.GetComponent<UIInput>().value;
			if (value == string.Empty)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("edit_select_gui_mzbk"));
				return;
			}
			m_cname = value;
			cmsg_change_map_name cmsg_change_map_name = new cmsg_change_map_name();
			cmsg_change_map_name.id = m_ed[m_index].id;
			cmsg_change_map_name.name = value;
			net_http._instance.send_msg(opclient_t.OPCODE_CHANGE_MAP_NAME, cmsg_change_map_name, restart: true, string.Empty, 10f);
		}
		if (obj.name == "close_cn")
		{
			m_cn.GetComponent<ui_show_anim>().hide_ui();
		}
		if (obj.name == "tip")
		{
			m_tip.SetActive(value: true);
		}
		if (obj.name == "close_tip")
		{
			m_tip.GetComponent<ui_show_anim>().hide_ui();
		}
	}

	private void select(GameObject obj)
	{
		m_index = int.Parse(obj.name.Substring(4, obj.name.Length - 4));
		if (m_ed[m_index].id == 0)
		{
			cmsg_create_map cmsg_create_map = new cmsg_create_map();
			cmsg_create_map.index = m_index;
			net_http._instance.send_msg(opclient_t.OPCODE_CREATE_MAP, cmsg_create_map, restart: true, string.Empty, 10f);
		}
		else
		{
			m_info.SetActive(value: true);
			reset_info();
		}
	}
}
