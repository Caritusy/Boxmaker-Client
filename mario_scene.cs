using System.Collections.Generic;
using UnityEngine;

public class mario_scene : MonoBehaviour
{
	public List<GameObject> m_cengs;

	public List<int> m_moves;

	public List<int> m_lens;

	public void update_ex(mario_point roll)
	{
		for (int i = 0; i < m_cengs.Count; i++)
		{
			int num = roll.x / 10 / m_moves[i] % m_lens[i];
			int num2 = roll.y / 10 / m_moves[i];
			m_cengs[i].transform.localPosition = new Vector3(-num, -num2);
		}
	}
}
