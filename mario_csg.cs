using System.Collections.Generic;
using UnityEngine;

public class mario_csg : mario_obj
{
	public List<Sprite> m_sprites;

	public override void reset()
	{
		if (m_param[0] == 0 && m_param[1] == 0)
		{
			base.transform.FindChild("sprite").GetComponent<SpriteRenderer>().sprite = m_sprites[0];
			base.transform.FindChild("sprite").GetComponent<SpriteRenderer>().sortingOrder = 1;
		}
		else if (m_param[0] < m_init_pos.x && m_param[1] == m_init_pos.y)
		{
			base.transform.FindChild("sprite").GetComponent<SpriteRenderer>().sprite = m_sprites[1];
			base.transform.FindChild("sprite").GetComponent<SpriteRenderer>().sortingOrder = 0;
		}
		else if (m_param[0] > m_init_pos.x && m_param[1] == m_init_pos.y)
		{
			base.transform.FindChild("sprite").GetComponent<SpriteRenderer>().sprite = m_sprites[2];
			base.transform.FindChild("sprite").GetComponent<SpriteRenderer>().sortingOrder = 0;
		}
		else if (m_param[0] == m_init_pos.x && m_param[1] > m_init_pos.y)
		{
			base.transform.FindChild("sprite").GetComponent<SpriteRenderer>().sprite = m_sprites[3];
			base.transform.FindChild("sprite").GetComponent<SpriteRenderer>().sortingOrder = 0;
		}
		else if (m_param[0] == m_init_pos.x && m_param[1] < m_init_pos.y)
		{
			base.transform.FindChild("sprite").GetComponent<SpriteRenderer>().sprite = m_sprites[4];
			base.transform.FindChild("sprite").GetComponent<SpriteRenderer>().sortingOrder = 0;
		}
		else if (m_param[0] < m_init_pos.x && m_param[1] < m_init_pos.y)
		{
			base.transform.FindChild("sprite").GetComponent<SpriteRenderer>().sprite = m_sprites[5];
			base.transform.FindChild("sprite").GetComponent<SpriteRenderer>().sortingOrder = 0;
		}
		else if (m_param[0] > m_init_pos.x && m_param[1] < m_init_pos.y)
		{
			base.transform.FindChild("sprite").GetComponent<SpriteRenderer>().sprite = m_sprites[6];
			base.transform.FindChild("sprite").GetComponent<SpriteRenderer>().sortingOrder = 0;
		}
		else if (m_param[0] > m_init_pos.x && m_param[1] > m_init_pos.y)
		{
			base.transform.FindChild("sprite").GetComponent<SpriteRenderer>().sprite = m_sprites[7];
			base.transform.FindChild("sprite").GetComponent<SpriteRenderer>().sortingOrder = 0;
		}
		else if (m_param[0] < m_init_pos.x && m_param[1] > m_init_pos.y)
		{
			base.transform.FindChild("sprite").GetComponent<SpriteRenderer>().sprite = m_sprites[8];
			base.transform.FindChild("sprite").GetComponent<SpriteRenderer>().sortingOrder = 0;
		}
	}
}
