using UnityEngine;

public class archor_list : MonoBehaviour
{
	public GameObject m_touxiang;

	public GameObject m_gq;

	public GameObject m_name;

	public GameObject m_map_name;

	public void reset(int icon, string gq, string name, string map_name)
	{
		m_touxiang.GetComponent<UISprite>().spriteName = game_data._instance.get_t_touxiang(icon);
		m_name.GetComponent<UILabel>().text = name;
		m_gq.GetComponent<UISprite>().spriteName = game_data._instance.get_t_guojia(gq);
		m_map_name.GetComponent<UILabel>().text = map_name;
	}
}
