using System.Collections.Generic;
using UnityEngine;

public class play_mask_gui : MonoBehaviour
{
	public GameObject m_sub;

	public GameObject m_panel;

	private List<GameObject> m_objs = new List<GameObject>();

	private float m_time;

	private int m_type;

	private mario.ChangeStateHandle m_handle;

	public void reset(mario.ChangeStateHandle handle)
	{
		m_handle = handle;
		base.gameObject.SetActive(value: true);
		m_time = 0f;
		m_type = 0;
		m_objs.Clear();
		mario._instance.remove_child(m_panel);
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				GameObject gameObject = (GameObject)Object.Instantiate(m_sub);
				gameObject.transform.parent = m_panel.transform;
				gameObject.transform.localPosition = new Vector3(-560 + j * 160, -240 + i * 160, 0f);
				gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
				gameObject.SetActive(value: true);
				m_objs.Add(gameObject);
			}
		}
		for (int k = 0; k < m_objs.Count; k++)
		{
			float delay = (float)(k / 8 + k % 8) * 0.05f + 1f;
			utils.add_scale_anim(m_objs[k], 0.2f, new Vector3(1f, 1f, 1f), delay);
		}
	}

	public void finish()
	{
		m_handle();
	}

	private void Update()
	{
		m_time += Time.deltaTime;
		if (m_type == 0 && m_time > 1.8f)
		{
			m_type = 1;
			m_time = 0f;
		}
		if (m_type == 1 && m_time > 0.5f)
		{
			m_type = 2;
			m_time = 0f;
			finish();
		}
		if (m_type == 2 && m_time > 0.1f)
		{
			m_type = 3;
			m_time = 0f;
			for (int i = 0; i < m_objs.Count; i++)
			{
				float delay = (float)(i / 8 + i % 8) * 0.05f;
				utils.add_scale_anim(m_objs[i], 0.2f, new Vector3(0f, 0f, 0f), delay);
			}
		}
		if (m_type == 3 && m_time > 0.8f)
		{
			m_type = 4;
			base.gameObject.SetActive(value: false);
		}
	}
}
