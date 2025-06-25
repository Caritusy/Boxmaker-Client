using System.Collections.Generic;
using protocol.game;
using UnityEngine;

public class shop_gui : MonoBehaviour, IMessage
{
	public GameObject m_sub_shop;

	public List<GameObject> m_ss;

	public GameObject m_shop_view;

	private int m_index;

	private int m_buy_id;

	private List<GameObject> m_shop_items = new List<GameObject>();

	private bool m_shop_init;

	private float m_pay_time;

	public GameObject m_zs;

	private void Start()
	{
		cmessage_center._instance.add_handle(this);
	}

	private void OnDestroy()
	{
		cmessage_center._instance.remove_handle(this);
	}

	private void OnEnable()
	{
		InvokeRepeating("time", 0f, 1f);
	}

	private void OnDisable()
	{
		CancelInvoke("time");
	}

	public void reset(int index)
	{
		m_index = index;
		base.gameObject.SetActive(value: true);
		m_ss[m_index].GetComponent<UIToggle>().value = true;
		mario._instance.remove_child(m_shop_view);
		m_shop_items.Clear();
		int num = 0;
		foreach (KeyValuePair<int, s_t_shop> item in game_data._instance.m_t_shop)
		{
			s_t_shop value = item.Value;
			if (value.slot == index)
			{
				GameObject gameObject = (GameObject)Object.Instantiate(m_sub_shop);
				gameObject.transform.parent = m_shop_view.transform;
				gameObject.transform.localPosition = new Vector3(-310 + num * 200, 0f, 0f);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject.GetComponent<UIButtonMessage>().target = base.gameObject;
				gameObject.GetComponent<shop_sub>().reset(value);
				gameObject.SetActive(value: true);
				m_shop_items.Add(gameObject);
				num++;
			}
		}
		m_shop_view.GetComponent<UIScrollView>().ResetPosition();
	}

	public void message(s_message message)
	{
		if (message.m_type == "recharge_load_end")
		{
			m_shop_init = true;
			reset(m_index);
		}
		if (message.m_type == "recharge_android_success")
		{
			m_pay_time = 3f;
		}
		if (message.m_type == "recharge_web")
		{
			cmsg_pay cmsg_pay = new cmsg_pay();
			cmsg_pay.channel = game_data._instance.m_channel;
			net_http._instance.send_msg(opclient_t.OPCODE_PAY, cmsg_pay, restart: true, string.Empty, 10f);
		}
	}

	public void net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.OPCODE_SHOP_BUY || message.m_opcode == opclient_t.OPCODE_GOOGLE_PAY)
		{
			s_t_shop s_t_shop2 = game_data._instance.get_t_shop(m_buy_id);
			if (s_t_shop2.type != 1)
			{
				mario._instance.m_self.jewel -= s_t_shop2.price;
				mario_tool._instance.onJewelConsume(s_t_shop2.desc, s_t_shop2.price);
			}
			mario._instance.show_tip(game_data._instance.get_language_string("user_gui_hd") + s_t_shop2.name);
			if (s_t_shop2.type == 1)
			{
				mario._instance.m_self.jewel += s_t_shop2.def;
				mario_tool._instance.onJewelGet(s_t_shop2.desc, s_t_shop2.def);
			}
			else if (s_t_shop2.type != 2)
			{
				if (s_t_shop2.type == 3)
				{
					mario._instance.m_self.testify = s_t_shop2.def;
				}
				else if (s_t_shop2.type == 4)
				{
					if (mario._instance.m_self.exp_time > timer.now())
					{
						mario._instance.m_self.exp_time += (ulong)((long)s_t_shop2.def * 86400000L);
					}
					else
					{
						mario._instance.m_self.exp_time = timer.now() + (ulong)((long)s_t_shop2.def * 86400000L);
					}
				}
			}
			for (int i = 0; i < m_shop_items.Count; i++)
			{
				m_shop_items[i].GetComponent<shop_sub>().reset();
			}
		}
		if (message.m_opcode == opclient_t.OPCODE_PAY)
		{
			if (message.m_res == -1)
			{
				s_message s_message2 = new s_message();
				s_message2.m_type = "recharge_web";
				mario._instance.show_double_dialog_box(game_data._instance.get_language_string("user_gui_wzfdd"), s_message2);
			}
			else
			{
				smsg_pay smsg_pay = net_http._instance.parse_packet<smsg_pay>(message.m_byte);
				mario._instance.show_tip(string.Format(game_data._instance.get_language_string("user_gui_hdzs"), smsg_pay.jewel));
				mario._instance.m_self.jewel += smsg_pay.jewel;
				mario_tool._instance.onJewelGet("安卓支付", smsg_pay.jewel);
			}
		}
	}

	private void click(GameObject obj)
	{
		if (obj.name == "close")
		{
			GetComponent<ui_show_anim>().hide_ui();
		}
	}

	private void select_shop(GameObject obj)
	{
		int index = int.Parse(obj.name.Substring(1, obj.name.Length - 1));
		reset(index);
	}

	private void click_item(GameObject obj)
	{
		s_t_shop t_shop = obj.GetComponent<shop_sub>().m_t_shop;
		m_buy_id = t_shop.id;
		if (t_shop.type == 1)
		{
			if (!Application.isEditor)
			{
				string text = mario._instance.m_self.userid.ToString() + timer.now();
				string url = $"http://mario.web.yymoon.com/recharge/pay.php?WIDout_trade_no={text}&WIDsubject={t_shop.desc}&WIDtotal_fee={t_shop.price}&WINotify_url={mario._instance.m_self.notify_uri}&WIextra_common_param={mario._instance.m_self.openid}";
				Application.OpenURL(url);
				s_message s_message2 = new s_message();
				s_message2.m_type = "recharge_web";
				mario._instance.show_double_dialog_box(game_data._instance.get_language_string("user_gui_zfdd"), s_message2);
			}
		}
		else if (t_shop.type != 1 && mario._instance.m_self.jewel < t_shop.price)
		{
			mario._instance.show_tip(game_data._instance.get_language_string("user_gui_zsbz"));
		}
		else
		{
			cmsg_shop_buy cmsg_shop_buy = new cmsg_shop_buy();
			cmsg_shop_buy.id = t_shop.id;
			net_http._instance.send_msg(opclient_t.OPCODE_SHOP_BUY, cmsg_shop_buy, restart: true, string.Empty, 10f);
		}
	}

	private void time()
	{
		m_ss[m_index].GetComponent<UIToggle>().value = true;
		m_zs.GetComponent<UILabel>().text = mario._instance.m_self.jewel.ToString();
	}

	private void Update()
	{
		if (m_pay_time > 0f)
		{
			m_pay_time -= Time.deltaTime;
			if (m_pay_time <= 0f)
			{
				m_pay_time = 0f;
				cmsg_pay cmsg_pay = new cmsg_pay();
				cmsg_pay.channel = game_data._instance.m_channel;
				net_http._instance.send_msg(opclient_t.OPCODE_PAY, cmsg_pay, restart: true, string.Empty, 10f);
			}
		}
	}
}
