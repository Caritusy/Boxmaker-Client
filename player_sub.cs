using protocol.game;
using UnityEngine;

public class player_sub : MonoBehaviour
{
	public GameObject m_texture;

	public GameObject m_name;

	public GameObject m_text;

	private int m_id;

	private GameObject m_player_gui;

	public void reset(int type, int id, string name, byte[] url, int def, string time, GameObject obj)
	{
		m_id = id;
		m_player_gui = obj;
		m_texture.GetComponent<UITexture>().mainTexture = game_data._instance.mission_to_texture(url);
		m_name.GetComponent<UILabel>().text = name;
		string text = string.Empty;
		switch (type)
		{
		case 1:
			text = string.Format(game_data._instance.get_language_string("play_sub_type1"), time, def);
			break;
		case 2:
			text = string.Format(game_data._instance.get_language_string("play_sub_type2"), time);
			break;
		case 3:
			text = string.Format(game_data._instance.get_language_string("play_sub_type3"), def.ToString("N0"));
			break;
		}
		m_text.GetComponent<UILabel>().text = text;
	}

	private void click(GameObject obj)
	{
		m_player_gui.GetComponent<ui_show_anim>().hide_ui();
		cmsg_view_comment cmsg_view_comment = new cmsg_view_comment();
		cmsg_view_comment.id = m_id;
		net_http._instance.send_msg(opclient_t.OPCODE_VIEW_COMMENT, cmsg_view_comment, restart: true, string.Empty, 10f);
	}
}
