using UnityEngine;

public class play_select_sub : MonoBehaviour
{
	public GameObject m_icon;

	public GameObject m_title;

	public int m_type;

	public void reset(s_t_view_map t_view_map)
	{
		m_type = t_view_map.id;
		m_icon.GetComponent<UISprite>().spriteName = t_view_map.icon;
		m_title.GetComponent<UILabel>().text = t_view_map.name;
	}
}
