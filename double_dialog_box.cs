using UnityEngine;

public class double_dialog_box : MonoBehaviour
{
	private s_message m_out_message;

	private s_message m_out_message1;

	public GameObject m_text;

	public void reset(string text, s_message mes, s_message mes1)
	{
		base.gameObject.SetActive(value: true);
		m_out_message = mes;
		m_out_message1 = mes1;
		m_text.GetComponent<UILabel>().text = text;
	}

	private void click(GameObject obj)
	{
		if (obj.name == "ok")
		{
			if (m_out_message != null)
			{
				cmessage_center._instance.add_message(m_out_message);
			}
			m_out_message = null;
			base.gameObject.GetComponent<ui_show_anim>().hide_ui();
		}
		if (obj.name == "cancel")
		{
			if (m_out_message1 != null)
			{
				cmessage_center._instance.add_message(m_out_message1);
			}
			m_out_message1 = null;
			base.gameObject.GetComponent<ui_show_anim>().hide_ui();
		}
	}
}
