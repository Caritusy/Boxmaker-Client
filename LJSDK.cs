using System.Collections.Generic;
using protocol.game;
using UnityEngine;

public class LJSDK : MonoBehaviour
{
	public static LJSDK _instance;

	public string m_channel;

	private int m_req_num;

	private s_t_shop m_t_shop;

	private void Awake()
	{
		_instance = this;
	}

	public void init()
	{
		if (Application.isEditor)
		{
			init_callback_success(string.Empty);
		}
		else
		{
			init_callback_success(string.Empty);
		}
	}

	private void init_callback_success(string res)
	{
		s_message s_message2 = new s_message();
		s_message2.m_type = "init_success";
		cmessage_center._instance.add_message(s_message2);
	}

	private void init_callback_failed(string res)
	{
		mario._instance.wait(flag: false, string.Empty);
		s_message s_message2 = new s_message();
		s_message2.m_type = "init_failed";
		mario._instance.show_single_dialog_box(res, s_message2);
	}

	public void login()
	{
	}

	private void login_callback_success(string res)
	{
	}

	private void login_callback_failed(string res)
	{
		s_message s_message2 = new s_message();
		s_message2.m_type = "login_fail";
		cmessage_center._instance.add_message(s_message2);
	}

	public void req_info()
	{
	}

	private void recharge_android_product(string s)
	{
		if (s != string.Empty)
		{
			string[] array = s.Split(' ');
			string text = array[1];
			string ios_desc = array[0];
			foreach (KeyValuePair<int, s_t_shop> item in game_data._instance.m_t_shop)
			{
				if (item.Value.code == text)
				{
					item.Value.ios_desc = ios_desc;
					break;
				}
			}
		}
		m_req_num--;
		if (m_req_num == 0)
		{
			mario._instance.wait(flag: false, string.Empty);
			s_message s_message2 = new s_message();
			s_message2.m_type = "recharge_load_end";
			cmessage_center._instance.add_message(s_message2);
		}
	}

	private void recharge_web_product(string s)
	{
		if (s != string.Empty)
		{
			int num = s.IndexOf(' ');
			string text = s.Substring(0, num);
			string json = s.Substring(num + 1);
			foreach (KeyValuePair<int, s_t_shop> item in game_data._instance.m_t_shop)
			{
				if (item.Value.code == text)
				{
					JsonData jsonData = JsonMapper.ToObject(json);
					float num2 = (long)jsonData["currency"]["usd_exchange_inverse"];
					string text2 = (string)jsonData["currency"]["user_currency"];
					json = (item.Value.price_my * num2).ToString("f2") + " " + text2;
					item.Value.ios_desc = json;
					break;
				}
			}
		}
		m_req_num--;
		if (m_req_num == 0)
		{
			mario._instance.wait(flag: false, string.Empty);
			s_message s_message2 = new s_message();
			s_message2.m_type = "recharge_load_end";
			cmessage_center._instance.add_message(s_message2);
		}
	}

	public void pay(s_t_shop t_shop)
	{
	}

	private void pay_callback_success(string res)
	{
		if (m_channel == "google")
		{
			cmsg_google_pay cmsg_google_pay = new cmsg_google_pay();
			cmsg_google_pay.id = m_t_shop.id;
			cmsg_google_pay.package_name = "com.moon.boxworld.google";
			cmsg_google_pay.product_id = m_t_shop.code;
			cmsg_google_pay.purchase_token = res;
			net_http._instance.send_msg(opclient_t.OPCODE_GOOGLE_PAY, cmsg_google_pay, restart: true, string.Empty, 10f);
		}
		else
		{
			s_message s_message2 = new s_message();
			s_message2.m_type = "recharge_android_success";
			cmessage_center._instance.add_message(s_message2);
		}
	}

	private void pay_callback_failed(string res)
	{
		mario._instance.wait(flag: false, string.Empty);
	}

	public void logout()
	{
	}

	private void logout_callback(string res)
	{
		s_message s_message2 = new s_message();
		s_message2.m_type = "lj_logout";
		cmessage_center._instance.add_message(s_message2);
	}

	public void exit()
	{
	}

	public void kill()
	{
	}

	public void doSetExtData()
	{
	}

	public void init_channel()
	{
	}

	private void init_channel_web_callback(string channel)
	{
		m_channel = channel;
		game_data._instance.m_channel = m_channel;
		game_data._instance.init_pt_ver();
	}

	private void web_callback(string type)
	{
		mario._instance.wait(flag: false, string.Empty);
		if (type == "1")
		{
			s_message s_message2 = new s_message();
			s_message2.m_type = "recharge_web";
			mario._instance.show_double_dialog_box(game_data._instance.get_language_string("user_gui_zfdd"), s_message2);
		}
	}

	public bool need_pt()
	{
		if (game_data._instance.m_channel == string.Empty || game_data._instance.m_channel == "yymoon" || game_data._instance.m_channel == "google" || game_data._instance.m_channel == "IOS_yymoon" || game_data._instance.m_channel == "web_yymoon" || game_data._instance.m_channel == "win_yymoon")
		{
			return false;
		}
		return true;
	}
}
