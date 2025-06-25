using System;
using System.Collections.Generic;
using protocol.game;
using UnityEngine;

public class player_select_gui : MonoBehaviour, IMessage
{
	public GameObject m_back;

	public GameObject m_back1;

	public GameObject m_back2;

	public GameObject m_left_view_big;

	public GameObject m_left_view;

	public GameObject m_left_view1;

	public GameObject m_left_panel_big;

	public GameObject m_left_panel;

	public GameObject m_left_panel1;

	public GameObject m_right_view;

	public GameObject m_play_select_big;

	public GameObject m_play_select_sub;

	public GameObject m_play_select_sub1;

	private int m_lan;

	private List<GameObject> m_second = new List<GameObject>();

	public GameObject m_map_level;

	public GameObject m_map_exp;

	public GameObject m_rq;

	public GameObject m_tgl;

	public GameObject m_texture;

	public GameObject m_map_win;

	public GameObject m_map_name;

	public GameObject m_map_id;

	public GameObject m_map_zuo_touxiang;

	public GameObject m_map_zuo_name;

	public GameObject m_map_zuo_guojia;

	public GameObject m_map_dz;

	public GameObject m_map_tg;

	public GameObject m_map_cy;

	public GameObject m_map_sc_text;

	private map_info m_mi;

	private int m_id;

	private List<comment> m_comments;

	public GameObject m_search;

	public GameObject m_search_text;

	public GameObject m_pinglun;

	public GameObject m_pinglun_text;

	private int m_page_up;

	private int m_page_down;

	private int m_max_page;

	private int m_page_type = -1;

	private int m_page_fx;

	public GameObject m_pl_panel;

	public GameObject m_pl_sub;

	public GameObject m_npl;

	public GameObject m_up_tuo;

	public GameObject m_down_tuo;

	private GameObject m_tp_up_tuo;

	private GameObject m_tp_down_tuo;

	public GameObject m_br_start;

	public GameObject m_br_continue;

	public GameObject m_br_jd;

	public GameObject m_br_life;

	public GameObject m_br_index;

	public GameObject m_br_select;

	public GameObject m_br_text;

	public GameObject m_br_bk;

	public List<Texture> m_br_texs;

	public GameObject m_br_select_panel;

	public List<GameObject> m_br_locks;

	private int m_bhard;

	private void Start()
	{
		cmessage_center._instance.add_handle(this);
		mario._instance.show_user(base.gameObject);
		UIScrollView component = m_left_view1.GetComponent<UIScrollView>();
		component.onDragFinished = (UIScrollView.OnDragNotification)Delegate.Combine(component.onDragFinished, new UIScrollView.OnDragNotification(OnDragFinished));
	}

	private void OnDestroy()
	{
		cmessage_center._instance.remove_handle(this);
	}

	private void OnEnable()
	{
		if (m_lan == 0)
		{
			m_back.SetActive(value: true);
			m_back1.SetActive(value: false);
			m_back2.SetActive(value: false);
			reset_big();
		}
		else if (m_mi != null)
		{
			cmsg_view_comment cmsg_view_comment = new cmsg_view_comment();
			cmsg_view_comment.id = m_mi.id;
			net_http._instance.send_msg(opclient_t.OPCODE_VIEW_COMMENT, cmsg_view_comment, restart: true, string.Empty, 10f);
		}
	}

	private void OnDragFinished()
	{
		if (m_page_type == -1)
		{
			return;
		}
		UIScrollView component = m_left_view1.GetComponent<UIScrollView>();
		Vector3 vector = component.panel.CalculateConstrainOffset(component.bounds.min, component.bounds.max);
		if (vector.y > 10f)
		{
			if (m_page_up > 0)
			{
				m_page_up--;
				m_page_fx = -1;
				cmsg_view_map cmsg_view_map = new cmsg_view_map();
				cmsg_view_map.index = m_page_up;
				cmsg_view_map.type = m_page_type;
				cmsg_view_map.ver = 2;
				net_http._instance.send_msg(opclient_t.OPCODE_VIEW_MAP, cmsg_view_map, restart: true, string.Empty, 10f);
			}
		}
		else if (vector.y < -10f && m_page_down < m_max_page)
		{
			m_page_down++;
			m_page_fx = 1;
			cmsg_view_map cmsg_view_map2 = new cmsg_view_map();
			cmsg_view_map2.index = m_page_down;
			cmsg_view_map2.type = m_page_type;
			cmsg_view_map2.ver = 2;
			net_http._instance.send_msg(opclient_t.OPCODE_VIEW_MAP, cmsg_view_map2, restart: true, string.Empty, 10f);
		}
	}

	private void change_big()
	{
		if (m_lan != 0)
		{
			reset_big();
			m_lan = 0;
			m_left_panel_big.transform.localPosition = new Vector3(-300f, m_left_panel_big.transform.localPosition.y, m_left_panel_big.transform.localPosition.z);
			utils.add_pos_anim(m_left_panel_big, 0.5f, new Vector3(300f, 0f, 0f), 0f);
			m_left_panel.transform.localPosition = new Vector3(0f, m_left_panel.transform.localPosition.y, m_left_panel.transform.localPosition.z);
			utils.add_pos_anim(m_left_panel, 0.5f, new Vector3(300f, 0f, 0f), 0f);
		}
	}

	private void change_first(bool big)
	{
		if (m_lan != 1)
		{
			reset_first();
			m_lan = 1;
			if (big)
			{
				m_left_panel_big.transform.localPosition = new Vector3(0f, m_left_panel_big.transform.localPosition.y, m_left_panel_big.transform.localPosition.z);
				utils.add_pos_anim(m_left_panel_big, 0.5f, new Vector3(-300f, 0f, 0f), 0f);
				m_left_panel.transform.localPosition = new Vector3(300f, m_left_panel.transform.localPosition.y, m_left_panel.transform.localPosition.z);
				utils.add_pos_anim(m_left_panel, 0.5f, new Vector3(-300f, 0f, 0f), 0f);
			}
			else
			{
				m_left_panel.transform.localPosition = new Vector3(-300f, m_left_panel.transform.localPosition.y, m_left_panel.transform.localPosition.z);
				utils.add_pos_anim(m_left_panel, 0.5f, new Vector3(300f, 0f, 0f), 0f);
				m_left_panel1.transform.localPosition = new Vector3(0f, m_left_panel1.transform.localPosition.y, m_left_panel1.transform.localPosition.z);
				utils.add_pos_anim(m_left_panel1, 0.5f, new Vector3(300f, 0f, 0f), 0f);
			}
		}
	}

	private void change_second()
	{
		if (m_lan != 2)
		{
			m_lan = 2;
			m_left_panel.transform.localPosition = new Vector3(0f, m_left_panel.transform.localPosition.y, m_left_panel.transform.localPosition.z);
			utils.add_pos_anim(m_left_panel, 0.5f, new Vector3(-300f, 0f, 0f), 0f);
			m_left_panel1.transform.localPosition = new Vector3(300f, m_left_panel1.transform.localPosition.y, m_left_panel1.transform.localPosition.z);
			utils.add_pos_anim(m_left_panel1, 0.5f, new Vector3(-300f, 0f, 0f), 0f);
		}
	}

	private void reset_big()
	{
		mario._instance.remove_child(m_left_view_big);
		m_left_view_big.GetComponent<UIScrollView>().ResetPosition();
		int num = 0;
		foreach (KeyValuePair<int, s_t_view_title> item in game_data._instance.m_t_view_title)
		{
			s_t_view_title value = item.Value;
			bool flag = false;
			if (value.id == 200)
			{
				if (mario._instance.m_self.testify != 0)
				{
					continue;
				}
			}
			else if (value.id == 201)
			{
				if (mario._instance.m_self.testify != 1)
				{
					continue;
				}
			}
			else if (value.id == 202)
			{
				if (mario._instance.m_self.testify != 2)
				{
					continue;
				}
			}
			else if (value.id == 203 && mario._instance.m_self.testify != 3)
			{
				continue;
			}
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(m_play_select_big);
			gameObject.name = "big_sub";
			gameObject.transform.parent = m_left_view_big.transform;
			gameObject.transform.localPosition = new Vector3(-300f, 182 - 85 * num, 0f);
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.transform.FindChild("icon").GetComponent<UISprite>().spriteName = value.icon;
			gameObject.GetComponent<UIButtonMessage>().target = base.gameObject;
			if (value.id >= 200)
			{
				gameObject.GetComponent<play_select_big>().m_clevel = game_data._instance.get_zm(value.id - 199);
			}
			utils.add_pos_anim(gameObject, 0.5f, new Vector3(300f, 0f, 0f), (float)num * 0.05f);
			gameObject.GetComponent<play_select_big>().reset(value);
			gameObject.SetActive(value: true);
			num++;
		}
	}

	private void reset_first()
	{
		mario._instance.remove_child(m_left_view);
		m_left_view.GetComponent<UIScrollView>().ResetPosition();
		int num = 0;
		foreach (KeyValuePair<int, s_t_view_map> item in game_data._instance.m_t_view_map)
		{
			s_t_view_map value = item.Value;
			if ((value.id != 4 || mario._instance.m_self.testify >= 1) && (value.id != 5 || mario._instance.m_self.testify >= 2) && (value.id != 6 || mario._instance.m_self.testify >= 3) && (value.id != 7 || mario._instance.m_self.testify >= 4))
			{
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(m_play_select_sub);
				gameObject.name = "first_sub";
				gameObject.transform.parent = m_left_view.transform;
				gameObject.transform.localPosition = new Vector3(0f, 182 - 85 * num, 0f);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject.transform.FindChild("icon").GetComponent<UISprite>().spriteName = value.icon;
				gameObject.GetComponent<UIButtonMessage>().target = base.gameObject;
				gameObject.GetComponent<play_select_sub>().reset(value);
				gameObject.SetActive(value: true);
				num++;
			}
		}
	}

	private void clear_second()
	{
		mario._instance.remove_child(m_left_view1);
		m_left_view1.GetComponent<UIScrollView>().ResetPosition();
		m_second.Clear();
	}

	private void add_second(smsg_view_map msg, int fx)
	{
		int num = 0;
		switch (fx)
		{
		case -1:
		{
			if (m_tp_up_tuo != null)
			{
				UnityEngine.Object.Destroy(m_tp_up_tuo);
			}
			num = m_page_up;
			if (m_page_down - m_page_up < 3)
			{
				break;
			}
			for (int num3 = m_second.Count - 1; num3 >= 0; num3--)
			{
				if (m_second[num3].GetComponent<play_select_sub1>().m_page == m_page_down)
				{
					UnityEngine.Object.Destroy(m_second[num3]);
					m_second.RemoveAt(num3);
				}
			}
			m_page_down--;
			if (m_tp_down_tuo != null)
			{
				m_tp_down_tuo.transform.localPosition = new Vector3(0f, 182 - 850 * m_page_down - 850, 0f);
				break;
			}
			m_tp_down_tuo = (GameObject)UnityEngine.Object.Instantiate(m_down_tuo);
			m_tp_down_tuo.transform.parent = m_left_view1.transform;
			m_tp_down_tuo.transform.localPosition = new Vector3(0f, 182 - 850 * m_page_down - 850, 0f);
			m_tp_down_tuo.transform.localScale = new Vector3(1f, 1f, 1f);
			m_tp_down_tuo.SetActive(value: true);
			break;
		}
		case 1:
		{
			if (m_tp_down_tuo != null)
			{
				UnityEngine.Object.Destroy(m_tp_down_tuo);
			}
			num = m_page_down;
			if (m_page_down - m_page_up < 3)
			{
				break;
			}
			for (int num2 = m_second.Count - 1; num2 >= 0; num2--)
			{
				if (m_second[num2].GetComponent<play_select_sub1>().m_page == m_page_up)
				{
					UnityEngine.Object.Destroy(m_second[num2]);
					m_second.RemoveAt(num2);
				}
			}
			m_page_up++;
			if (m_tp_up_tuo != null)
			{
				m_tp_up_tuo.transform.localPosition = new Vector3(0f, 182 - 850 * m_page_up + 85, 0f);
				break;
			}
			m_tp_up_tuo = (GameObject)UnityEngine.Object.Instantiate(m_up_tuo);
			m_tp_up_tuo.transform.parent = m_left_view1.transform;
			m_tp_up_tuo.transform.localPosition = new Vector3(0f, 182 - 850 * m_page_up + 85, 0f);
			m_tp_up_tuo.transform.localScale = new Vector3(1f, 1f, 1f);
			m_tp_up_tuo.SetActive(value: true);
			break;
		}
		}
		int num4 = 0;
		for (int i = 0; i < msg.infos.Count; i++)
		{
			map_show ms = msg.infos[i];
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(m_play_select_sub1);
			gameObject.name = "small_sub";
			gameObject.transform.parent = m_left_view1.transform;
			gameObject.transform.localPosition = new Vector3(0f, 182 - 85 * num4 - 850 * num, 0f);
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.GetComponent<UIButtonMessage>().target = base.gameObject;
			gameObject.GetComponent<play_select_sub1>().reset(ms, num);
			gameObject.SetActive(value: true);
			m_second.Add(gameObject);
			num4++;
		}
		if (fx == -1)
		{
			if (num > 0)
			{
				m_tp_up_tuo = (GameObject)UnityEngine.Object.Instantiate(m_up_tuo);
				m_tp_up_tuo.transform.parent = m_left_view1.transform;
				m_tp_up_tuo.transform.localPosition = new Vector3(0f, 182 - 850 * num + 85, 0f);
				m_tp_up_tuo.transform.localScale = new Vector3(1f, 1f, 1f);
				m_tp_up_tuo.SetActive(value: true);
			}
		}
		else if (num < m_max_page - 1)
		{
			m_tp_down_tuo = (GameObject)UnityEngine.Object.Instantiate(m_down_tuo);
			m_tp_down_tuo.transform.parent = m_left_view1.transform;
			m_tp_down_tuo.transform.localPosition = new Vector3(0f, 182 - 850 * num - 850, 0f);
			m_tp_down_tuo.transform.localScale = new Vector3(1f, 1f, 1f);
			m_tp_down_tuo.SetActive(value: true);
		}
	}

	private void show_map(map_info mi)
	{
		m_back.SetActive(value: false);
		m_back1.SetActive(value: true);
		m_back2.SetActive(value: false);
		m_mi = mi;
		if (m_mi.difficulty > 0)
		{
			m_map_level.GetComponent<UISprite>().spriteName = "jbjb_" + m_mi.difficulty;
		}
		else
		{
			m_map_level.GetComponent<UISprite>().spriteName = "jbjb_" + utils.get_map_nd(m_mi.pas, m_mi.amount);
		}
		if (m_mi.finish == 0)
		{
			m_map_win.SetActive(value: false);
			m_map_exp.GetComponent<UILabel>().text = "EXP+" + utils.get_map_exp(m_mi.pas, m_mi.amount);
		}
		else
		{
			m_map_win.SetActive(value: true);
			m_map_exp.GetComponent<UILabel>().text = "EXP+2";
		}
		m_rq.GetComponent<UILabel>().text = m_mi.date;
		float num = 0f;
		if (m_mi.amount > 0)
		{
			num = (float)m_mi.pas / (float)m_mi.amount * 100f;
		}
		m_tgl.GetComponent<UILabel>().text = num.ToString("f2") + "%";
		m_texture.GetComponent<UITexture>().mainTexture = game_data._instance.mission_to_texture(m_mi.url);
		m_map_name.GetComponent<UILabel>().text = m_mi.name;
		m_map_id.GetComponent<UILabel>().text = m_mi.id.ToString();
		m_map_zuo_touxiang.GetComponent<UISprite>().spriteName = game_data._instance.get_t_touxiang(m_mi.head);
		m_map_zuo_name.GetComponent<UILabel>().text = "[u]" + m_mi.owner_name;
		m_map_zuo_guojia.GetComponent<UISprite>().spriteName = game_data._instance.get_t_guojia(m_mi.country);
		m_map_dz.GetComponent<UILabel>().text = m_mi.like.ToString("N0");
		m_map_tg.GetComponent<UILabel>().text = m_mi.pas.ToString("N0");
		m_map_cy.GetComponent<UILabel>().text = m_mi.amount.ToString("N0");
		if (m_mi.collect == 0)
		{
			m_map_sc_text.GetComponent<UILabel>().text = game_data._instance.get_language_string("play_select_gui_sc");
		}
		else
		{
			m_map_sc_text.GetComponent<UILabel>().text = game_data._instance.get_language_string("play_select_gui_qxsc");
		}
	}

	private void reset_pinlun(List<comment> comments)
	{
		m_comments = comments;
		mario._instance.remove_child(m_pl_panel);
		GameObject gameObject = null;
		for (int i = 0; i < m_comments.Count; i++)
		{
			GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(m_pl_sub);
			gameObject2.transform.parent = m_pl_panel.transform;
			gameObject2.name = m_comments[i].userid.ToString();
			gameObject2.transform.localPosition = new Vector3(0f, -245f, 0f);
			gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject2.transform.FindChild("touxiang").FindChild("icon").GetComponent<UISprite>()
				.spriteName = game_data._instance.get_t_touxiang(m_comments[i].head);
			gameObject2.transform.FindChild("touxiang").FindChild("name").GetComponent<UILabel>()
				.text = "[u]" + player.get_name(m_comments[i].userid, m_comments[i].name, m_comments[i].visitor);
			gameObject2.transform.FindChild("touxiang").FindChild("guoqi").GetComponent<UISprite>()
				.spriteName = game_data._instance.get_t_guojia(m_comments[i].country);
			gameObject2.transform.FindChild("text").GetComponent<UILabel>().text = m_comments[i].text;
			gameObject2.transform.FindChild("time").GetComponent<UILabel>().text = m_comments[i].date;
			gameObject2.SetActive(value: true);
			if (gameObject != null)
			{
				gameObject2.GetComponent<UIWidget>().topAnchor.target = gameObject.transform;
				gameObject2.GetComponent<UIWidget>().topAnchor.relative = 0f;
				gameObject2.GetComponent<UIWidget>().topAnchor.absolute = 5;
			}
			gameObject = gameObject2;
		}
		if (m_comments.Count == 0)
		{
			m_npl.SetActive(value: true);
		}
		else
		{
			m_npl.SetActive(value: false);
		}
		m_right_view.GetComponent<UIScrollView>().ResetPosition();
	}

	private void click(GameObject obj)
	{
		if (mario._instance.m_self.guide == 200)
		{
			if (obj.name != "big_sub")
			{
				mario._instance.show_tip(game_data._instance.get_language_string("play_select_gui_jxbs"));
				return;
			}
			int type = obj.GetComponent<play_select_big>().m_type;
			if (type != 200)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("play_select_gui_jxbs"));
				return;
			}
		}
		if (mario._instance.m_self.guide == 201 && obj.name != "play")
		{
			mario._instance.show_tip(game_data._instance.get_language_string("play_select_gui_jxpl"));
			return;
		}
		if (obj.name == "close")
		{
			mario._instance.change_state(e_game_state.egs_login, 1, delegate
			{
				UnityEngine.Object.Destroy(base.gameObject);
			});
		}
		if (obj.name == "big_sub" && m_lan == 0)
		{
			int type2 = obj.GetComponent<play_select_big>().m_type;
			if (type2 >= 200)
			{
				int clevel = obj.GetComponent<play_select_big>().m_clevel;
				if (clevel > mario._instance.m_self.level)
				{
					s_t_view_title s_t_view_title2 = game_data._instance.get_t_view_title(type2);
					string text = string.Format(game_data._instance.get_language_string("play_select_gui_ddjv"), clevel, s_t_view_title2.name);
					mario._instance.show_tip(text);
					return;
				}
				if (mario._instance.m_self.guide == 200)
				{
					obj.GetComponent<play_select_big>().hide_shou();
					mario._instance.m_self.guide = 201;
				}
				cmsg_view_comment cmsg_view_comment = new cmsg_view_comment();
				cmsg_view_comment.id = mario._instance.m_self.mapid;
				net_http._instance.send_msg(opclient_t.OPCODE_VIEW_COMMENT, cmsg_view_comment, restart: true, string.Empty, 10f);
			}
			else
			{
				switch (type2)
				{
				case 1:
					change_first(big: true);
					break;
				case 2:
				{
					cmsg_mission_view obj2 = new cmsg_mission_view();
					net_http._instance.send_msg(opclient_t.OPCODE_MISSION_VIEW, obj2, restart: true, string.Empty, 10f);
					break;
				}
				}
			}
		}
		if (obj.name == "first_sub" && m_lan == 1)
		{
			m_page_type = obj.GetComponent<play_select_sub>().m_type;
			m_page_up = 0;
			m_page_down = 0;
			m_page_fx = 0;
			cmsg_view_map cmsg_view_map = new cmsg_view_map();
			cmsg_view_map.index = 0;
			cmsg_view_map.type = m_page_type;
			cmsg_view_map.ver = 2;
			net_http._instance.send_msg(opclient_t.OPCODE_VIEW_MAP, cmsg_view_map, restart: true, string.Empty, 10f);
		}
		if (obj.name == "return")
		{
			if (m_lan == 2)
			{
				m_page_type = -1;
				change_first(big: false);
			}
			else if (m_lan == 1)
			{
				m_page_type = -1;
				change_big();
			}
			else
			{
				mario._instance.change_state(e_game_state.egs_login, 1, delegate
				{
					UnityEngine.Object.Destroy(base.gameObject);
				});
			}
		}
		if (obj.name == "small_sub")
		{
			map_show ms = obj.GetComponent<play_select_sub1>().m_ms;
			cmsg_view_comment cmsg_view_comment2 = new cmsg_view_comment();
			cmsg_view_comment2.id = ms.id;
			net_http._instance.send_msg(opclient_t.OPCODE_VIEW_COMMENT, cmsg_view_comment2, restart: true, string.Empty, 10f);
		}
		if (obj.name == "play")
		{
			m_id = m_mi.id;
			cmsg_play_map cmsg_play_map = new cmsg_play_map();
			cmsg_play_map.id = m_id;
			net_http._instance.send_msg(opclient_t.OPCODE_PLAY_MAP, cmsg_play_map, restart: true, string.Empty, 10f);
		}
		if (obj.name == "search")
		{
			if (m_lan == 0)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("play_select_gui_zxsc"));
				return;
			}
			m_search.SetActive(value: true);
			m_search_text.GetComponent<UIInput>().value = string.Empty;
		}
		if (obj.name == "search_ok")
		{
			string value = m_search_text.GetComponent<UIInput>().value;
			if (value == string.Empty)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("play_select_gui_gjzk"));
				return;
			}
			m_page_type = -1;
			cmsg_search_map cmsg_search_map = new cmsg_search_map();
			cmsg_search_map.name = value;
			net_http._instance.send_msg(opclient_t.OPCODE_SEARCH_MAP, cmsg_search_map, restart: true, string.Empty, 10f);
		}
		if (obj.name == "search_close")
		{
			m_search.GetComponent<ui_show_anim>().hide_ui();
		}
		if (obj.name == "shc")
		{
			cmsg_favorite_map cmsg_favorite_map = new cmsg_favorite_map();
			cmsg_favorite_map.id = m_mi.id;
			net_http._instance.send_msg(opclient_t.OPCODE_FAVORITE_MAP, cmsg_favorite_map, restart: true, string.Empty, 10f);
		}
		if (obj.name == "pl")
		{
			m_pinglun.SetActive(value: true);
			m_pinglun_text.GetComponent<UIInput>().value = string.Empty;
		}
		if (obj.name == "pinglun_ok")
		{
			string value2 = m_pinglun_text.GetComponent<UIInput>().value;
			if (value2 == string.Empty)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("play_select_gui_plwk"));
				return;
			}
			cmsg_comment cmsg_comment = new cmsg_comment();
			cmsg_comment.id = m_mi.id;
			cmsg_comment.text = value2;
			net_http._instance.send_msg(opclient_t.OPCODE_COMMENT, cmsg_comment, restart: true, string.Empty, 10f);
		}
		if (obj.name == "pinglun_close")
		{
			m_pinglun.GetComponent<ui_show_anim>().hide_ui();
		}
		if (obj.name == "ph")
		{
			cmsg_view_map_point_rank cmsg_view_map_point_rank = new cmsg_view_map_point_rank();
			cmsg_view_map_point_rank.map_id = m_mi.id;
			net_http._instance.send_msg(opclient_t.OPCODE_VIEW_MAP_POINT_RANK, cmsg_view_map_point_rank, restart: true, string.Empty, 10f);
		}
		if (obj.name == "touxiang")
		{
			look_player(m_mi.owner_id);
		}
		if (obj.name == "br_start")
		{
			cmsg_mission_start cmsg_mission_start = new cmsg_mission_start();
			cmsg_mission_start.hard = m_bhard;
			net_http._instance.send_msg(opclient_t.OPCODE_MISSION_START, cmsg_mission_start, restart: true, string.Empty, 10f);
		}
		if (obj.name == "br_continue")
		{
			cmsg_mission_continue obj3 = new cmsg_mission_continue();
			net_http._instance.send_msg(opclient_t.OPCODE_MISSION_CONTINUE, obj3, restart: true, string.Empty, 10f);
		}
		if (obj.name == "download")
		{
			s_message s_message2 = new s_message();
			s_message2.m_type = "player_select_gui_download";
			mario._instance.show_double_dialog_box(game_data._instance.get_language_string("play_select_gui_dself"), s_message2);
		}
		if (obj.name == "br_drop")
		{
			s_message s_message3 = new s_message();
			s_message3.m_type = "player_select_gui_br_drop";
			mario._instance.show_double_dialog_box(game_data._instance.get_language_string("play_select_gui_br_drop"), s_message3);
		}
	}

	private void select_pl(GameObject obj)
	{
		int uid = int.Parse(obj.transform.parent.name);
		look_player(uid);
	}

	private void look_player(int uid)
	{
		cmsg_view_player cmsg_view_player = new cmsg_view_player();
		cmsg_view_player.userid = uid;
		net_http._instance.send_msg(opclient_t.OPCODE_VIEW_PLAYER, cmsg_view_player, restart: true, string.Empty, 10f);
	}

	private void show_br()
	{
		if (mario._instance.m_self.br_start == 1)
		{
			m_br_start.SetActive(value: false);
			m_br_continue.SetActive(value: true);
			m_br_jd.SetActive(value: true);
			m_br_select_panel.SetActive(value: false);
			m_br_life.GetComponent<UILabel>().text = "x " + mario._instance.m_self.br_life;
			m_br_index.GetComponent<UILabel>().text = mario._instance.m_self.br_index + 1 + "/8";
			m_br_bk.GetComponent<UITexture>().mainTexture = m_br_texs[mario._instance.m_self.br_hard - 1];
			s_t_br s_t_br2 = game_data._instance.get_t_br(mario._instance.m_self.br_hard);
			m_br_text.GetComponent<UILabel>().text = s_t_br2.desc;
			return;
		}
		m_br_start.SetActive(value: true);
		m_br_continue.SetActive(value: false);
		m_br_jd.SetActive(value: false);
		m_br_select_panel.SetActive(value: true);
		m_br_bk.GetComponent<UITexture>().mainTexture = m_br_texs[0];
		m_br_select.transform.localPosition = new Vector3(-140f, 150f, 0f);
		m_bhard = 1;
		s_t_br s_t_br3 = game_data._instance.get_t_br(1);
		m_br_text.GetComponent<UILabel>().text = s_t_br3.desc;
		for (int i = 0; i < m_br_locks.Count; i++)
		{
			if (mario._instance.m_self.br_max + 1 >= i + 2)
			{
				m_br_locks[i].SetActive(value: false);
			}
			else
			{
				m_br_locks[i].SetActive(value: true);
			}
		}
	}

	private void select_br(GameObject obj)
	{
		int num = int.Parse(obj.name.Substring(1));
		s_t_br s_t_br2 = game_data._instance.get_t_br(num);
		if (num > mario._instance.m_self.br_max + 1)
		{
			mario._instance.show_tip(s_t_br2.unlock);
			return;
		}
		m_bhard = num;
		m_br_select.transform.localPosition = obj.transform.localPosition;
		m_br_bk.GetComponent<UITexture>().mainTexture = m_br_texs[m_bhard - 1];
		m_br_text.GetComponent<UILabel>().text = s_t_br2.desc;
	}

	public void message(s_message message)
	{
		if (message.m_type == "commit_play")
		{
			m_id = m_mi.id;
			cmsg_play_map cmsg_play_map = new cmsg_play_map();
			cmsg_play_map.id = m_id;
			net_http._instance.send_msg(opclient_t.OPCODE_PLAY_MAP, cmsg_play_map, restart: true, string.Empty, 10f);
		}
		if (message.m_type == "player_select_gui_download")
		{
			cmsg_download_map cmsg_download_map = new cmsg_download_map();
			cmsg_download_map.id = m_mi.id;
			net_http._instance.send_msg(opclient_t.OPCODE_DOWNLOAD_MAP, cmsg_download_map, restart: true, string.Empty, 10f);
		}
		if (message.m_type == "player_select_gui_br_drop")
		{
			cmsg_mission_continue obj = new cmsg_mission_continue();
			net_http._instance.send_msg(opclient_t.OPCODE_MISSION_DROP, obj, restart: true, string.Empty, 10f);
		}
	}

	public void net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.OPCODE_VIEW_MAP)
		{
			smsg_view_map smsg_view_map = net_http._instance.parse_packet<smsg_view_map>(message.m_byte);
			m_max_page = smsg_view_map.page;
			if (m_page_fx == 0)
			{
				clear_second();
			}
			add_second(smsg_view_map, m_page_fx);
			change_second();
		}
		if (message.m_opcode == opclient_t.OPCODE_VIEW_COMMENT)
		{
			smsg_view_comment smsg_view_comment = net_http._instance.parse_packet<smsg_view_comment>(message.m_byte);
			show_map(smsg_view_comment.infos);
			reset_pinlun(smsg_view_comment.comments);
			m_right_view.GetComponent<UIScrollView>().ResetPosition();
			for (int i = 0; i < m_second.Count; i++)
			{
				if (m_second[i].GetComponent<play_select_sub1>().m_ms.id == smsg_view_comment.infos.id)
				{
					m_second[i].GetComponent<play_select_sub1>().reset(smsg_view_comment.infos.finish, smsg_view_comment.infos.pas, smsg_view_comment.infos.amount, smsg_view_comment.infos.like);
				}
			}
			mario._instance.m_self.set_per(smsg_view_comment.infos.head, smsg_view_comment.infos.country, smsg_view_comment.infos.owner_name, smsg_view_comment.infos.name);
		}
		if (message.m_opcode == opclient_t.OPCODE_PLAY_MAP)
		{
			if (mario._instance.m_self.guide == 201)
			{
				mario._instance.m_self.guide = 202;
			}
			if (message.m_res == -1)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("play_select_gui_tlbz"));
				msg_life_error msg_life_error = net_http._instance.parse_packet<msg_life_error>(message.m_byte);
				mario._instance.m_self.set_reset_time(msg_life_error.server_time, msg_life_error.life_time);
				return;
			}
			smsg_play_map smsg_play_map = net_http._instance.parse_packet<smsg_play_map>(message.m_byte);
			mario_tool._instance.onRaid(m_id.ToString(), 1);
			if (!game_data._instance.load_mission(m_id, smsg_play_map.map_data, smsg_play_map.x, smsg_play_map.y))
			{
				return;
			}
			mario._instance.change_state(e_game_state.egs_play, 2, delegate
			{
				if (base.gameObject.activeSelf)
				{
					base.gameObject.SetActive(value: false);
				}
				else
				{
					mario._instance.hide_clear_gui();
				}
			});
		}
		if (message.m_opcode == opclient_t.OPCODE_FAVORITE_MAP)
		{
			smsg_favorite_map smsg_favorite_map = net_http._instance.parse_packet<smsg_favorite_map>(message.m_byte);
			m_mi.favorite = smsg_favorite_map.num;
			m_mi.collect = 1 - m_mi.collect;
			show_map(m_mi);
		}
		if (message.m_opcode == opclient_t.OPCODE_COMMENT)
		{
			mario._instance.show_tip(game_data._instance.get_language_string("play_select_gui_fbpl"));
			m_pinglun.GetComponent<ui_show_anim>().hide_ui();
			smsg_comment smsg_comment = net_http._instance.parse_packet<smsg_comment>(message.m_byte);
			m_comments.Insert(0, smsg_comment.comment);
			if (m_comments.Count > 10)
			{
				m_comments.RemoveAt(m_comments.Count - 1);
			}
			reset_pinlun(m_comments);
		}
		if (message.m_opcode == opclient_t.OPCODE_SEARCH_MAP)
		{
			m_search.GetComponent<ui_show_anim>().hide_ui();
			smsg_view_map smsg_view_map2 = net_http._instance.parse_packet<smsg_view_map>(message.m_byte);
			if (smsg_view_map2.infos.Count == 0)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("play_select_gui_mzdt"));
				return;
			}
			clear_second();
			add_second(smsg_view_map2, 0);
			change_second();
		}
		if (message.m_opcode == opclient_t.OPCODE_VIEW_MAP_POINT_RANK)
		{
			smsg_view_map_point_rank msg = net_http._instance.parse_packet<smsg_view_map_point_rank>(message.m_byte);
			mario._instance.show_paihang_gui(msg, m_mi.id);
		}
		if (message.m_opcode == opclient_t.OPCODE_VIEW_VIDEO)
		{
			smsg_view_video smsg_view_video = net_http._instance.parse_packet<smsg_view_video>(message.m_byte);
			if (!game_data._instance.load_mission(m_mi.id, smsg_view_video.map_data, null, null))
			{
				return;
			}
			game_data._instance.load_inputs(smsg_view_video.video_data);
			mario._instance.change_state(e_game_state.egs_review, 2, delegate
			{
				base.gameObject.SetActive(value: false);
				mario._instance.hide_paihang_gui();
			});
		}
		if (message.m_opcode == opclient_t.OPCODE_VIEW_PLAYER)
		{
			smsg_view_player msg2 = net_http._instance.parse_packet<smsg_view_player>(message.m_byte);
			mario._instance.show_player_gui(msg2);
		}
		if (message.m_opcode == opclient_t.OPCODE_MISSION_VIEW)
		{
			smsg_mission_view smsg_mission_view = net_http._instance.parse_packet<smsg_mission_view>(message.m_byte);
			mario._instance.m_self.br_life = smsg_mission_view.life;
			mario._instance.m_self.br_index = smsg_mission_view.index;
			mario._instance.m_self.br_start = smsg_mission_view.start;
			mario._instance.m_self.br_hard = smsg_mission_view.hard;
			mario._instance.m_self.br_max = smsg_mission_view.br_max;
			m_back.SetActive(value: false);
			m_back1.SetActive(value: false);
			m_back2.SetActive(value: true);
			show_br();
		}
		if (message.m_opcode == opclient_t.OPCODE_MISSION_START || message.m_opcode == opclient_t.OPCODE_MISSION_CONTINUE)
		{
			if (message.m_opcode == opclient_t.OPCODE_MISSION_START)
			{
				mario._instance.m_self.br_index = 0;
				mario._instance.m_self.br_life = 100;
				mario._instance.m_self.br_start = 1;
				mario._instance.m_self.br_hard = m_bhard;
				mario._instance.m_start_type = 0;
			}
			else
			{
				mario._instance.m_start_type = 2;
			}
			smsg_mission_play smsg_mission_play = net_http._instance.parse_packet<smsg_mission_play>(message.m_byte);
			mario._instance.m_self.set_br(smsg_mission_play.user_head, smsg_mission_play.user_country, smsg_mission_play.user_name, smsg_mission_play.map_name);
			game_data._instance.load_mission(-1, smsg_mission_play.map_data, smsg_mission_play.x, smsg_mission_play.y);
			mario._instance.change_state(e_game_state.egs_br_road, 2, delegate
			{
				base.gameObject.SetActive(value: false);
			});
		}
		if (message.m_opcode == opclient_t.OPCODE_DOWNLOAD_MAP)
		{
			mario._instance.m_self.download_num++;
			mario._instance.show_tip(game_data._instance.get_language_string("play_select_gui_dlok"));
		}
		if (message.m_opcode == opclient_t.OPCODE_MISSION_DROP)
		{
			mario._instance.m_self.br_start = 0;
			show_br();
		}
	}
}
