using protocol.game;
using UnityEngine;

public class user_gui : MonoBehaviour
{
	public GameObject m_name;

	public GameObject m_level;

	public GameObject m_level_text;

	public GameObject m_exp;

	public GameObject m_exp_text;

	public GameObject m_zm;

	public GameObject m_touxiang;

	public GameObject m_guoqi;

	public GameObject m_jewel;

	public GameObject m_close;

	private void OnEnable()
	{
		InvokeRepeating("time", 0f, 1f);
		reset_touxiang();
	}

	private void OnDisable()
	{
		CancelInvoke("time");
	}

	public void change_event(GameObject obj)
	{
		m_close.GetComponent<UIButtonMessage>().target = obj;
	}

	private void click(GameObject obj)
	{
		if (obj.name == "life_add")
		{
			mario._instance.show_shop_gui(1);
		}
		if (obj.name == "jewel_add")
		{
			mario._instance.show_shop_gui(0);
		}
		if (obj.name == "user" && mario._instance.m_game_state == e_game_state.egs_play_select)
		{
			cmsg_view_player cmsg_view_player = new cmsg_view_player();
			cmsg_view_player.userid = mario._instance.m_self.userid;
			net_http._instance.send_msg(opclient_t.OPCODE_VIEW_PLAYER, cmsg_view_player, restart: true, string.Empty, 10f);
		}
	}

	private void reset_touxiang()
	{
		m_name.GetComponent<UILabel>().text = mario._instance.m_self.get_name();
		m_touxiang.GetComponent<UISprite>().spriteName = game_data._instance.get_t_touxiang(mario._instance.m_self.head);
		m_guoqi.GetComponent<UISprite>().spriteName = game_data._instance.get_t_guojia(mario._instance.m_self.nationality);
		s_t_exp s_t_exp2 = game_data._instance.get_t_exp(mario._instance.m_self.level);
		m_level.GetComponent<UISprite>().spriteName = s_t_exp2.icon;
		m_level_text.GetComponent<UILabel>().text = mario._instance.m_self.level.ToString();
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
		m_jewel.GetComponent<UILabel>().text = mario._instance.m_self.jewel.ToString();
		m_zm.GetComponent<UISprite>().spriteName = "wjtx_jb0" + mario._instance.m_self.testify;
	}

	private void time()
	{
		reset_touxiang();
	}
}
