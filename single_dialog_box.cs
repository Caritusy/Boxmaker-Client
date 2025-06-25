using UnityEngine;

public class single_dialog_box : MonoBehaviour
{
	private s_message m_out_message;

	public GameObject m_text;

	public void reset(string text, s_message mes)
	{
		base.gameObject.SetActive(value: true);
		m_out_message = mes;
		m_text.GetComponent<UILabel>().text = text;
	}

	private void click(GameObject obj)
	{
		if (m_out_message != null)
		{
			cmessage_center._instance.add_message(m_out_message);
		}
		m_out_message = null;
		base.gameObject.GetComponent<ui_show_anim>().hide_ui();
	}
}
