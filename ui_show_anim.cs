using UnityEngine;

public class ui_show_anim : MonoBehaviour
{
	public GameObject m_obj;

	private void OnEnable()
	{
		show_ui();
	}

	public void show_ui()
	{
	}

	public void hide_ui()
	{
		base.gameObject.SetActive(value: false);
	}
}
