using System.Collections.Generic;
using protocol.game;
using UnityEngine;

public class login_gui : MonoBehaviour, IMessage
{
	public GameObject m_user;

	public GameObject m_user_tip;

	public GameObject m_zhuce;

	public GameObject m_qiehuan;

	public GameObject m_cname;

	public GameObject m_main;

	private int m_index = 1;

	public GameObject m_select;

	public GameObject m_select1;

	public GameObject m_qh_zh;

	public GameObject m_qh_mm;

	public GameObject m_zc_zh;

	public GameObject m_zc_mm;

	public GameObject m_zc_nc;

	public GameObject m_zc_cc;

	public GameObject m_zc_ccn;

	public GameObject m_guojia;

	public GameObject m_guojia_panel;

	public GameObject m_guojia_gj;

	private int m_guojia_type;

	public GameObject m_cn_nc;

	public GameObject m_cname_cc;

	public GameObject m_cname_ccn;

	public GameObject m_name;

	public GameObject m_level;

	public GameObject m_level_text;

	public GameObject m_exp;

	public GameObject m_exp_text;

	public GameObject m_touxiang;

	public GameObject m_guoqi;

	public List<GameObject> m_texts;

	public List<GameObject> m_txs;

	public GameObject m_zm;

	public GameObject m_dj;

	public GameObject m_icon_panel;

	public GameObject m_sys;

	public GameObject m_lbm;

	public GameObject m_lbm_text;

	public GameObject m_about;

	public GameObject m_ver;

	public GameObject m_cn_sys_panel;

	public GameObject m_en_sys_panel;

	private void Start()
	{
		cmessage_center._instance.add_handle(this);
		m_ver.GetComponent<UILabel>().text = "Ver " + game_data._instance.m_ver;
		if (game_data._instance.m_lang == e_language.el_chinese)
		{
			m_cn_sys_panel.SetActive(true);
			m_en_sys_panel.SetActive(false);
		}
		else
		{
			m_cn_sys_panel.SetActive(false);
			m_en_sys_panel.SetActive(true);
		}
		int num = 0;
		int num2 = 0;
		foreach (string key in game_data._instance.m_t_guojia.Keys)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(m_guojia_gj);
			gameObject.transform.parent = m_guojia_panel.transform;
			gameObject.transform.localPosition = new Vector3(-380 + 140 * num2, 230 - 50 * num, 0f);
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.name = key;
			gameObject.transform.FindChild("name").GetComponent<UILabel>().text = key;
			gameObject.GetComponent<UISprite>().spriteName = game_data._instance.m_t_guojia[key];
			gameObject.SetActive(true);
			num2++;
			if (num2 >= 6)
			{
				num2 = 0;
				num++;
			}
		}
	}

	private void OnDestroy()
	{
		cmessage_center._instance.remove_handle(this);
	}

	private void OnEnable()
	{
		m_user.SetActive(false);
		m_main.SetActive(false);
		m_dj.SetActive(false);
		m_icon_panel.SetActive(false);
	}

	private void reset()
	{
		m_dj.SetActive(false);
		if (mario._instance.m_self == null)
		{
			if (LJSDK._instance.need_pt())
			{
				mario._instance.wait(true, game_data._instance.get_language_string("login_gui_dlpt"));
				LJSDK._instance.login();
			}
			else
			{
				cmsg_login cmsg_login = new cmsg_login();
				cmsg_login.openid = game_data._instance.m_save_data.openid;
				cmsg_login.openkey = game_data._instance.m_save_data.openkey;
				cmsg_login.nationality = string.Empty;
				cmsg_login.ver = game_data._instance.m_pt_ver;
				cmsg_login.channel = game_data._instance.m_channel;
				net_http._instance.send_msg(opclient_t.OPCODE_LOGIN, cmsg_login, true, game_data._instance.get_language_string("login_gui_zzlj"), 10f);
			}
		}
		else
		{
			m_user.SetActive(true);
			m_main.SetActive(true);
			m_icon_panel.SetActive(true);
			reset_user();
		}
		m_index = Random.Range(1, 7);
		m_select.transform.localPosition = m_txs[m_index - 1].transform.localPosition;
	}

	private void reset_user()
	{
		m_name.GetComponent<UILabel>().text = mario._instance.m_self.get_name();
		s_t_exp s_t_exp2 = game_data._instance.get_t_exp(mario._instance.m_self.level);
		m_level.GetComponent<UISprite>().spriteName = s_t_exp2.icon;
		m_level_text.GetComponent<UILabel>().text = mario._instance.m_self.level.ToString();
		m_touxiang.GetComponent<UISprite>().spriteName = game_data._instance.get_t_touxiang(mario._instance.m_self.head);
		m_guoqi.GetComponent<UISprite>().spriteName = game_data._instance.get_t_guojia(mario._instance.m_self.nationality);
		s_t_exp2 = game_data._instance.get_t_exp(mario._instance.m_self.level + 1);
		if (s_t_exp2 != null)
		{
			float value = (float)mario._instance.m_self.exp / (float)s_t_exp2.exp;
			m_exp.GetComponent<UIProgressBar>().value = value;
			m_exp_text.GetComponent<UILabel>().text = mario._instance.m_self.exp + "/" + s_t_exp2.exp;
		}
		else
		{
			m_exp.GetComponent<UIProgressBar>().value = 1f;
			m_exp_text.GetComponent<UILabel>().text = mario._instance.m_self.exp + "/--";
		}
		if (mario._instance.m_self.visitor == 1)
		{
			m_user_tip.SetActive(true);
		}
		else
		{
			m_user_tip.SetActive(false);
		}
		m_zm.GetComponent<UISprite>().spriteName = "wjtx_jb0" + mario._instance.m_self.testify;
	}

	private void clear_text()
	{
		for (int i = 0; i < m_texts.Count; i++)
		{
			m_texts[i].GetComponent<UIInput>().value = string.Empty;
		}
	}

	private void click(GameObject obj)
	{
		if (obj.name == "user")
		{
			if (!Application.isEditor && LJSDK._instance.need_pt())
			{
				LJSDK._instance.logout();
				return;
			}
			m_main.SetActive(false);
			if (mario._instance.m_self.visitor == 1)
			{
				clear_text();
				m_zhuce.SetActive(true);
			}
			else
			{
				clear_text();
				m_qiehuan.SetActive(true);
			}
		}
		if (obj.name == "close_zc")
		{
			m_zhuce.GetComponent<ui_show_anim>().hide_ui();
			m_main.SetActive(true);
		}
		if (obj.name == "close_qh")
		{
			m_qiehuan.GetComponent<ui_show_anim>().hide_ui();
			m_main.SetActive(true);
		}
		if (obj.name == "qhzh")
		{
			m_zhuce.GetComponent<ui_show_anim>().hide_ui();
			clear_text();
			m_qiehuan.SetActive(true);
		}
		if (obj.name == "qhok")
		{
			string value = m_qh_zh.GetComponent<UIInput>().value;
			string value2 = m_qh_mm.GetComponent<UIInput>().value;
			if (value == string.Empty)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("login_gui_zhbk"));
				return;
			}
			if (value2 == string.Empty)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("login_gui_mmbk"));
				return;
			}
			cmsg_change_account cmsg_change_account = new cmsg_change_account();
			cmsg_change_account.openid = value;
			cmsg_change_account.openkey = value2;
			cmsg_change_account.channel = game_data._instance.m_channel;
			net_http._instance.send_msg(opclient_t.OPCODE_CHANGE_ACCOUNT, cmsg_change_account, true, string.Empty, 10f);
		}
		if (obj.name == "zcok")
		{
			string value3 = m_zc_nc.GetComponent<UIInput>().value;
			string value4 = m_zc_zh.GetComponent<UIInput>().value;
			string value5 = m_zc_mm.GetComponent<UIInput>().value;
			string text = m_zc_ccn.GetComponent<UILabel>().text;
			if (value3 == string.Empty)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("login_gui_ncbk"));
				return;
			}
			if (value4 == string.Empty)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("login_gui_zhbk"));
				return;
			}
			if (value5 == string.Empty)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("login_gui_mmbk"));
				return;
			}
			if (text == string.Empty)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("login_gui_gjbk"));
				return;
			}
			cmsg_register cmsg_register = new cmsg_register();
			cmsg_register.openid = value4;
			cmsg_register.openkey = value5;
			cmsg_register.nickname = value3;
			cmsg_register.head = m_index;
			cmsg_register.nationality = text;
			net_http._instance.send_msg(opclient_t.OPCODE_REGISTER, cmsg_register, true, string.Empty, 10f);
		}
		if (obj.name == "zccc")
		{
			m_guojia.SetActive(true);
			m_zhuce.SetActive(false);
			m_guojia_type = 0;
		}
		if (obj.name == "close_gj")
		{
			m_guojia.SetActive(false);
			if (m_guojia_type == 0)
			{
				m_zhuce.SetActive(true);
			}
			else
			{
				m_cname.SetActive(true);
			}
		}
		if (obj.name == "cnok")
		{
			string value6 = m_cn_nc.GetComponent<UIInput>().value;
			string text2 = m_cname_ccn.GetComponent<UILabel>().text;
			if (value6 == string.Empty)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("login_gui_ncbk"));
				return;
			}
			if (text2 == string.Empty)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("login_gui_gjbk"));
				return;
			}
			cmsg_change_name cmsg_change_name = new cmsg_change_name();
			cmsg_change_name.nickname = value6;
			cmsg_change_name.head = m_index;
			cmsg_change_name.nationality = text2;
			net_http._instance.send_msg(opclient_t.OPCODE_CHANGE_NAME, cmsg_change_name, true, string.Empty, 10f);
		}
		if (obj.name == "cncc")
		{
			m_guojia.SetActive(true);
			m_cname.SetActive(false);
			m_guojia_type = 1;
		}
		if (obj.name == "play")
		{
			mario._instance.change_state(e_game_state.egs_play_select, 1, delegate
			{
				Object.Destroy(base.gameObject);
			});
		}
		if (obj.name == "edit")
		{
			mario._instance.change_state(e_game_state.egs_edit_select, 1, delegate
			{
				Object.Destroy(base.gameObject);
			});
		}
		if (obj.name == "sys")
		{
			//if (mario._instance.m_self.m_review == 1)
			//{
				//return;
			//}
			m_main.SetActive(false);
            //m_lbm.GetComponent<UILabel>().text = "���Բ˵�";
            m_sys.SetActive(true);

		}
		if (obj.name == "close_sys")
		{
			m_sys.GetComponent<ui_show_anim>().hide_ui();
			m_main.SetActive(true);
		}
		if (obj.name == "lbm")
		{
			m_lbm_text.GetComponent<UIInput>().value = string.Empty;
			m_lbm_text.GetComponent<UIInput>().characterLimit = 99999;
            m_lbm.SetActive(true);
		}
		if (obj.name == "close_lbm")
		{
			m_lbm.GetComponent<ui_show_anim>().hide_ui();
		}
		if (obj.name == "about")
		{
            string a = "";
			//foreach (var c in m_about.GetComponentsInChildren<Component>())
			//{
			//a += c.GetType() + "\n";
			//}
			//BMReborn.WindowsProxy.MessageBox($"�ؼ���Ϣ��{a}");
			var abd = m_cn_sys_panel.transform.GetChild(0);
			abd.GetComponent<UILabel>().text = "QQȺ��312336775";
            var abc = m_cn_sys_panel.transform.GetChild(1);
			abc.GetComponent<UILabel>().text = "�ٶ����ɣ�����ʮ���ˣ�";
            //var abc = m_cn_sys_panel.GetComponentsInChildren<Component>();
            //foreach (var c in abc)
            //{
            //a += c.GetType() + "\n";
            //}
            //BMReborn.WindowsProxy.MessageBox($"�ؼ���Ϣ��{m_cn_sys_panel.GetType()}\n {a} \n {abc.Length} \n {abd}");

            m_about.SetActive(true);
		}
		if (obj.name == "close_about")
		{
			m_about.GetComponent<ui_show_anim>().hide_ui();
		}
		if (obj.name == "lbmok")
		{
			string value7 = m_lbm_text.GetComponent<UIInput>().value;
			if (value7 == string.Empty)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("login_gui_lbmk"));
				return;
			}
			//if (!value7.StartsWith("[SECRETSUPERDEBUGCHEATALLOWS]"))
			//{
				//cmsg_libao cmsg_libao = new cmsg_libao();
				//cmsg_libao.code = value7;
				//net_http._instance.send_msg(opclient_t.OPCODE_LIBAO, cmsg_libao, true, string.Empty, 10f);
			//}
			string cc = value7;
			if (cc == "[SECRETSUPERDEBUGCHEATALLOWS]INFKEY" && mario._instance.m_self.userid == 0)
			{
				game_data._instance.SUPERSCRECTDEBUG_INFKEYS = !game_data._instance.SUPERSCRECTDEBUG_INFKEYS;
                mario._instance.show_tip($"{(game_data._instance.SUPERSCRECTDEBUG_INFKEYS ? "�ѿ���" : "�ѹر�")}����Կ��");
            }
            else if (mario._instance.m_self.userid != 0)
            {
                mario._instance.show_tip("���㻹���������룿��ƨ�ԣ�");
                return;
            }
            else
			{
                mario._instance.show_tip("�����벻����");
                return;
            }
		}
		if (obj.name == "facebook")
		{
			Application.OpenURL("https://www.facebook.com/myboxmaker/");
		}
		if (obj.name == "twitter")
		{
			Application.OpenURL("https://twitter.com/myboxmaker");
		}
		if (obj.name == "go")
		{
			mario._instance.play_sound("sound/quan");
			List<int> list = new List<int>();
			list.Add(0);
			list.Add(-20);
			mario._instance.change_state(e_game_state.egs_edit_select, 3, delegate
			{
				Object.Destroy(base.gameObject);
			}, list);
		}
	}

	private void select_guojia(GameObject obj)
	{
		string text = obj.name;
		string spriteName = game_data._instance.get_t_guojia(text);
		m_guojia.SetActive(false);
		if (m_guojia_type == 0)
		{
			m_zhuce.SetActive(true);
			m_zc_cc.GetComponent<UISprite>().spriteName = spriteName;
			m_zc_ccn.GetComponent<UILabel>().text = text;
		}
		else
		{
			m_cname.SetActive(true);
			m_cname_cc.GetComponent<UISprite>().spriteName = spriteName;
			m_cname_ccn.GetComponent<UILabel>().text = text;
		}
	}

	private void select_tx(GameObject obj)
	{
		m_index = int.Parse(obj.name);
		m_select.transform.localPosition = obj.transform.localPosition;
	}

	private void select_tx1(GameObject obj)
	{
		m_index = int.Parse(obj.name);
		m_select1.transform.localPosition = obj.transform.localPosition;
	}

	public void message(s_message message)
	{
		if (message.m_type == "lj_logout")
		{
			m_user.SetActive(false);
			m_main.SetActive(false);
			m_icon_panel.SetActive(false);
		}
		if (message.m_type == "login_fail")
		{
			mario._instance.wait(false, string.Empty);
			m_dj.SetActive(true);
		}
	}

	public void net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.OPCODE_LOGIN || message.m_opcode == opclient_t.OPCODE_LOGIN_ANDROID || message.m_opcode == opclient_t.OPCODE_CHANGE_ACCOUNT || message.m_opcode == opclient_t.OPCODE_REGISTER)
		{
			if (message.m_res == -1)
			{
				mario._instance.show_single_dialog_box(game_data._instance.get_language_string("login_gui_feif"), null);
				return;
			}
			smsg_login smsg_login = net_http._instance.parse_packet<smsg_login>(message.m_byte);
			player player2 = new player(smsg_login);
			if (mario._instance.m_self != null)
			{
				player2.m_review = mario._instance.m_self.m_review;
			}
			mario._instance.m_self = player2;
			game_data._instance.m_save_data.openid = smsg_login.openid;
			game_data._instance.m_save_data.openkey = smsg_login.openkey;
			game_data._instance.save_native();
			if (player2.nationality != string.Empty)
			{
				string spriteName = game_data._instance.get_t_guojia(player2.nationality);
				m_zc_cc.GetComponent<UISprite>().spriteName = spriteName;
				m_zc_ccn.GetComponent<UILabel>().text = player2.nationality;
				m_cname_cc.GetComponent<UISprite>().spriteName = spriteName;
				m_cname_ccn.GetComponent<UILabel>().text = player2.nationality;
			}
			if (message.m_opcode == opclient_t.OPCODE_LOGIN)
			{
				m_user.SetActive(true);
				m_main.SetActive(true);
				m_icon_panel.SetActive(true);
				mario._instance.show_tip(game_data._instance.get_language_string("login_gui_dlcg"));
				player2.m_review = smsg_login.review;
			}
			else if (message.m_opcode == opclient_t.OPCODE_LOGIN_ANDROID)
			{
				if (player2.visitor == 1)
				{
					m_cname.SetActive(true);
				}
				else
				{
					m_user.SetActive(true);
					m_main.SetActive(true);
					m_icon_panel.SetActive(true);
					mario._instance.show_tip(game_data._instance.get_language_string("login_gui_dlcg"));
				}
			}
			else if (message.m_opcode == opclient_t.OPCODE_CHANGE_ACCOUNT)
			{
				m_qiehuan.GetComponent<ui_show_anim>().hide_ui();
				m_main.SetActive(true);
				mario._instance.show_tip(game_data._instance.get_language_string("login_gui_qhcg"));
			}
			else if (message.m_opcode == opclient_t.OPCODE_REGISTER)
			{
				m_zhuce.GetComponent<ui_show_anim>().hide_ui();
				m_main.SetActive(true);
				mario._instance.show_tip(game_data._instance.get_language_string("login_gui_wscg"));
			}
			reset_user();
		}
		if (message.m_opcode == opclient_t.OPCODE_CHANGE_NAME)
		{
			string value = m_cn_nc.GetComponent<UIInput>().value;
			string text = m_cname_ccn.GetComponent<UILabel>().text;
			mario._instance.m_self.name = value;
			mario._instance.m_self.head = m_index;
			mario._instance.m_self.nationality = text;
			mario._instance.m_self.visitor = 0;
			m_cname.GetComponent<ui_show_anim>().hide_ui();
			m_user.SetActive(true);
			m_main.SetActive(true);
			m_icon_panel.SetActive(true);
			mario._instance.show_tip(game_data._instance.get_language_string("login_gui_wscg"));
			reset_user();
		}
		if (message.m_opcode == opclient_t.OPCODE_LIBAO)
		{
			smsg_libao smsg_libao = net_http._instance.parse_packet<smsg_libao>(message.m_byte);
			string text2 = string.Format(game_data._instance.get_language_string("login_gui_gtlb"), smsg_libao.life);
			mario._instance.show_tip(text2);
			m_lbm.GetComponent<ui_show_anim>().hide_ui();
		}
	}
}
