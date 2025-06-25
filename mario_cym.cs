using System.Collections.Generic;
using UnityEngine;

public class mario_cym : mario_obj
{
	public GameObject m_s;

	private GameObject m_edit_obj;

	private List<GameObject> m_objs = new List<GameObject>();

	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_bkcf = true;
	}

	public override void reset()
	{
		try
		{ 
			mario._instance.remove_child(m_s);
			if (m_shadow != null)
			{
				mario._instance.remove_child(m_shadow.GetComponent<mario_cym>().m_s);
			}
			m_edit_obj = null;
			if (m_param[0] != 0 && m_edit_mode)
			{
				s_t_unit s_t_unit2 = game_data._instance.get_t_unit(m_param[0]);
				string path = "unit/" + s_t_unit2.res + "/" + s_t_unit2.res;
				GameObject original = (GameObject)Resources.Load(path);
				m_edit_obj = (GameObject)Object.Instantiate(original);
				m_edit_obj.transform.parent = m_s.transform;
				m_edit_obj.transform.localScale = new Vector3(1f, 1f, 1f);
				mario_obj component = m_edit_obj.GetComponent<mario_obj>();
				component.m_edit_mode = true;
				List<int> list = new List<int>();
				list.Add(m_param[1]);
				list.Add(m_param[2]);
				list.Add(m_param[3]);
				list.Add(0);
				component.init(s_t_unit2.res, list, m_world, -1, -1, 0, 0);
			}
		}
		catch (System.Exception e)
		{
			Debug.LogError(e);
			mario._instance.show_tip($"触发了反崩编辑器机制，异常于：{m_name}({this.m_init_pos.x},{this.m_init_pos.y})");
		}
	}

	public override void change()
	{
		if (!(m_edit_obj == null))
		{
			m_edit_obj.GetComponent<mario_obj>().change();
			for (int i = 0; i < 3; i++)
			{
				m_param[i + 1] = m_edit_obj.GetComponent<mario_obj>().m_param[i];
				game_data._instance.m_arrays[m_world][m_init_pos.y][m_init_pos.x].param[i + 1] = m_edit_obj.GetComponent<mario_obj>().m_param[i];
			}
		}
	}

	private void dingchu()
	{
		if (m_param[0] == 0)
		{
			return;
		}
		while (m_objs.Count < 3)
		{
			m_objs.Add(null);
		}
		int num = -1;
		for (int i = 0; i < m_objs.Count; i++)
		{
			if (m_objs[i] == null)
			{
				num = i;
				break;
			}
		}
		if (num != -1)
		{
			s_t_unit s_t_unit2 = game_data._instance.get_t_unit(m_param[0]);
			List<int> list = new List<int>();
			list.Add(m_param[1]);
			list.Add(m_param[2]);
			list.Add(m_param[3]);
			list.Add(0);
			mario_obj mario_obj2 = play_mode._instance.create_mario_obj(s_t_unit2.res, null, list, m_init_pos.x, m_init_pos.y);
			m_objs[num] = mario_obj2.gameObject;
		}
	}

	public override void tupdate()
	{
		if (play_mode._instance.m_time % 250 == 50)
		{
			dingchu();
		}
	}
}
