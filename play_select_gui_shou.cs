using UnityEngine;

public class play_select_gui_shou : MonoBehaviour
{
	public GameObject m_shou;

	private void OnEnable()
	{
		if (mario._instance.m_self.guide == 201)
		{
			m_shou.SetActive(value: true);
		}
		else
		{
			m_shou.SetActive(value: false);
		}
	}
}
