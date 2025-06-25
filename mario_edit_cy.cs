using UnityEngine;

public class mario_edit_cy : MonoBehaviour
{
	[HideInInspector]
	public mario_point m_grid = new mario_point();

	public void reset(edit_cy ec)
	{
		m_grid.set(ec.p.x / utils.g_grid_size, ec.p.y / utils.g_grid_size);
		float num = 0.9f - (float)ec.num * 0.015f;
		if (num < 0.5f)
		{
			num = 0.5f;
		}
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, num);
		GetComponent<SpriteRenderer>().sprite = ec.sp;
	}
}
