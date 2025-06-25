using protocol.game;
using UnityEngine;

public class paihang_gui : MonoBehaviour, IMessage
{
	public GameObject m_ph_view;

	public GameObject m_ph_sub;

	public GameObject m_l1;

	private bool m_vv;

	private void Start()
	{
		cmessage_center._instance.add_handle(this);
	}

	private void OnDestroy()
	{
		cmessage_center._instance.remove_handle(this);
	}

	public void check()
	{
		if (m_vv)
		{
			m_vv = false;
			base.gameObject.SetActive(value: true);
		}
	}

	public void reset(smsg_view_map_point_rank msg, int id)
	{
		m_l1.SetActive(value: true);
		mario._instance.remove_child(m_ph_view);
		for (int i = 0; i < msg.ranks.Count; i++)
		{
			map_point_rank msg2 = msg.ranks[i];
			GameObject gameObject = (GameObject)Object.Instantiate(m_ph_sub);
			gameObject.transform.parent = m_ph_view.transform;
			gameObject.transform.localPosition = new Vector3(0f, 170 - i * 75, 0f);
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.GetComponent<paihang_sub>().reset(i + 1, id, msg2);
			gameObject.SetActive(value: true);
		}
		m_ph_view.GetComponent<UIScrollView>().ResetPosition();
	}

	private void look_player(int uid)
	{
		cmsg_view_player cmsg_view_player = new cmsg_view_player();
		cmsg_view_player.userid = uid;
		net_http._instance.send_msg(opclient_t.OPCODE_VIEW_PLAYER, cmsg_view_player, restart: true, string.Empty, 10f);
	}

	private void click(GameObject obj)
	{
		if (obj.name == "close")
		{
			GetComponent<ui_show_anim>().hide_ui();
		}
	}

	public void message(s_message message)
	{
		if (message.m_type == "ph_look_player")
		{
			int uid = (int)message.m_ints[0];
			GetComponent<ui_show_anim>().hide_ui();
			look_player(uid);
		}
	}

	public void net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.OPCODE_VIEW_VIDEO)
		{
			m_vv = true;
		}
	}
}
