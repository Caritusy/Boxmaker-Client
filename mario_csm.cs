using System.Collections.Generic;
using UnityEngine;

public class mario_csm : mario_attack_ex
{
	private int m_cs_time;

	private bool m_hit;

	public List<Sprite> m_s;

	public override void reset()
	{
		try
		{
			base.transform.FindChild("fx").GetComponent<SpriteRenderer>().sprite = m_s[m_param[0]];
		}
		catch (System.Exception ex)
		{
            mario._instance.show_tip($"触发了反崩编辑器机制，异常于：{m_name}({this.m_init_pos.x},{this.m_init_pos.y})");
			Debug.Log(ex);
        }
		//base.transform.FindChild("fx").GetComponent<SpriteRenderer>().color = Color.red;
		//base.transform.
	}

	public override void be_hit(mario_obj obj)
	{
		if (obj.m_main && obj.m_pos.x > m_bound.left && obj.m_pos.x < m_bound.right && obj.m_pos.y > m_bound.bottom && obj.m_pos.y < m_bound.top)
		{
			m_hit = true;
			play_mode._instance.show_chuan(this);
		}
	}

	public override void tupdate()
	{
		if (m_cs_time > 0)
		{
			m_cs_time--;
			play_anim("chuan");
		}
		else if (m_hit)
		{
			play_anim("hit");
		}
		else
		{
			play_anim("stand");
		}
		m_hit = false;
	}
}
