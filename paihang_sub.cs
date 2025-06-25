using protocol.game;
using UnityEngine;

public class paihang_sub : MonoBehaviour
{
	public GameObject m_rank_icon;

	public GameObject m_rank;

	public GameObject m_name;

	public GameObject m_level;

	public GameObject m_level_text;

	public GameObject m_time;

	public GameObject m_guojia;

	public map_point_rank m_msg;

	private int m_map_id;

	public void reset(int rank, int map_id, map_point_rank msg)
	{
		m_map_id = map_id;
		m_msg = msg;
		m_rank.GetComponent<UILabel>().text = rank.ToString();
		m_name.GetComponent<UILabel>().text = "[u]" + player.get_name(m_msg.user_id, m_msg.player_name, m_msg.visitor);
		s_t_exp s_t_exp2 = game_data._instance.get_t_exp(m_msg.player_level);
		m_level.GetComponent<UISprite>().spriteName = s_t_exp2.icon;
		m_level_text.GetComponent<UILabel>().text = m_msg.player_level.ToString();
		m_time.GetComponent<UILabel>().text = timer.get_game_time(m_msg.player_point);
		m_guojia.GetComponent<UISprite>().spriteName = game_data._instance.get_t_guojia(m_msg.player_country);
		switch (rank)
		{
		case 1:
			m_rank_icon.GetComponent<UISprite>().spriteName = "hz_01";
			break;
		case 2:
			m_rank_icon.GetComponent<UISprite>().spriteName = "hz_02";
			break;
		case 3:
			m_rank_icon.GetComponent<UISprite>().spriteName = "hz_03";
			break;
		default:
			m_rank_icon.GetComponent<UISprite>().spriteName = string.Empty;
			break;
		}
		if (m_msg.user_id == mario._instance.m_self.userid)
		{
			GetComponent<UISprite>().spriteName = "phb_list_frame001";
		}
	}

	private void click(GameObject obj)
	{
		if (obj.name == "hf")
		{
			cmsg_view_video cmsg_view_video = new cmsg_view_video();
			cmsg_view_video.map_id = m_map_id;
			cmsg_view_video.video_id = m_msg.video_id;
			net_http._instance.send_msg(opclient_t.OPCODE_VIEW_VIDEO, cmsg_view_video, restart: true, string.Empty, 10f);
		}
		if (obj.name == "name")
		{
			s_message s_message2 = new s_message();
			s_message2.m_type = "ph_look_player";
			s_message2.m_ints.Add(m_msg.user_id);
			cmessage_center._instance.add_message(s_message2);
		}
	}
}
