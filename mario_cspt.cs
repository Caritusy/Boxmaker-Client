using System.Collections.Generic;
using UnityEngine;

public class mario_cspt : mario_block1
{
	private bool m_hit;

	private mario_point m_next = new mario_point();

	public GameObject m_fx1;

	public GameObject m_fx2;

	private int m_hit_time;

	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_is_dzd = 0;
	}

	public override void reset()
	{
		if (m_param[0] % 2 == 0)
		{
			set_fx(mario_fx.mf_right);
		}
		else
		{
			set_fx(mario_fx.mf_left);
		}
		if (m_param[0] < 2)
		{
			m_fx1.SetActive(value: true);
			m_fx2.SetActive(value: false);
		}
		else
		{
			m_fx1.SetActive(value: false);
			m_fx2.SetActive(value: true);
		}
		for (int i = 0; i < 4; i++)
		{
			int num = m_init_pos.x + utils.csg_points[i * 2, 0] / 2;
			int num2 = m_init_pos.y + utils.csg_points[i * 2, 1] / 2;
			int num3 = m_init_pos.x + utils.csg_points[i * 2 + 1, 0] / 2;
			int num4 = m_init_pos.y + utils.csg_points[i * 2 + 1, 1] / 2;
			if (num < 0 || num >= game_data._instance.m_map_data.maps[m_world].x_num || num2 < 0 || num2 >= game_data._instance.m_map_data.maps[m_world].y_num || num3 < 0 || num3 >= game_data._instance.m_map_data.maps[m_world].x_num || num4 < 0 || num4 >= game_data._instance.m_map_data.maps[m_world].y_num || game_data._instance.m_arrays[m_world][num2][num].type != utils.g_csg || game_data._instance.m_arrays[m_world][num4][num3].type != utils.g_csg)
			{
				continue;
			}
			if (game_data._instance.m_arrays[m_world][num2][num].param[2] == num3 && game_data._instance.m_arrays[m_world][num2][num].param[3] == num4)
			{
				if (m_param[0] % 2 == 0)
				{
					m_next = new mario_point(num3, num4);
				}
				else
				{
					m_next = new mario_point(num, num2);
				}
				break;
			}
			if (game_data._instance.m_arrays[m_world][num4][num3].param[2] == num && game_data._instance.m_arrays[m_world][num4][num3].param[3] == num2)
			{
				if (m_param[0] % 2 == 1)
				{
					m_next = new mario_point(num3, num4);
				}
				else
				{
					m_next = new mario_point(num, num2);
				}
				break;
			}
		}
	}

	public override bool be_left_hit(mario_obj obj, ref int px)
	{
		return false;
	}

	public override bool be_right_hit(mario_obj obj, ref int px)
	{
		return false;
	}

	public override bool be_top_hit(mario_obj obj, ref int py)
	{
		return false;
	}

	public override bool be_bottom_hit(mario_obj obj, ref int py)
	{
		if (obj.m_type != mario_type.mt_charater)
		{
			return false;
		}
		m_hit = true;
		return base.be_bottom_hit(obj, ref py);
	}

	public override bool be_left_top_hit(mario_obj obj, ref int px, ref int py)
	{
		return false;
	}

	public override bool be_right_top_hit(mario_obj obj, ref int px, ref int py)
	{
		return false;
	}

	public override bool be_left_bottom_hit(mario_obj obj, ref int px, ref int py)
	{
		if (obj.m_type != mario_type.mt_charater)
		{
			return false;
		}
		m_hit = true;
		return base.be_left_bottom_hit(obj, ref px, ref py);
	}

	public override bool be_right_bottom_hit(mario_obj obj, ref int px, ref int py)
	{
		if (obj.m_type != mario_type.mt_charater)
		{
			return false;
		}
		m_hit = true;
		return base.be_right_bottom_hit(obj, ref px, ref py);
	}

	public override mario_point move()
	{
		mario_point mario_point2 = new mario_point();
		if (m_hit)
		{
			m_hit_time++;
			if (m_hit_time < 5)
			{
				return mario_point2;
			}
			int num = utils.g_grid_size * m_next.x + utils.g_grid_size / 2;
			int num2 = utils.g_grid_size * m_next.y + utils.g_grid_size / 2;
			if (num > m_pos.x)
			{
				m_pos.x += 40 * (m_param[0] / 2 + 1);
				mario_point2.x += 40 * (m_param[0] / 2 + 1);
			}
			else if (num < m_pos.x)
			{
				m_pos.x -= 40 * (m_param[0] / 2 + 1);
				mario_point2.x -= 40 * (m_param[0] / 2 + 1);
			}
			if (num2 > m_pos.y)
			{
				m_pos.y += 40 * (m_param[0] / 2 + 1);
				mario_point2.y += 40 * (m_param[0] / 2 + 1);
			}
			else if (num2 < m_pos.y)
			{
				m_pos.y -= 40 * (m_param[0] / 2 + 1);
				mario_point2.y -= 40 * (m_param[0] / 2 + 1);
			}
			if (num == m_pos.x && num2 == m_pos.y)
			{
				if (m_fx == mario_fx.mf_left)
				{
					int num3 = game_data._instance.m_arrays[m_world][m_next.y][m_next.x].param[0];
					int num4 = game_data._instance.m_arrays[m_world][m_next.y][m_next.x].param[1];
					if (num3 == 0 && num4 == 0)
					{
						m_fx = mario_fx.mf_right;
						num3 = game_data._instance.m_arrays[m_world][m_next.y][m_next.x].param[2];
						num4 = game_data._instance.m_arrays[m_world][m_next.y][m_next.x].param[3];
					}
					m_next = new mario_point(num3, num4);
				}
				else
				{
					int num5 = game_data._instance.m_arrays[m_world][m_next.y][m_next.x].param[2];
					int num6 = game_data._instance.m_arrays[m_world][m_next.y][m_next.x].param[3];
					if (num5 == 0 && num6 == 0)
					{
						m_fx = mario_fx.mf_left;
						num5 = game_data._instance.m_arrays[m_world][m_next.y][m_next.x].param[0];
						num6 = game_data._instance.m_arrays[m_world][m_next.y][m_next.x].param[1];
					}
					m_next = new mario_point(num5, num6);
				}
			}
		}
		return mario_point2;
	}

	public override void change()
	{
		if (m_param[0] < 3)
		{
			m_param[0] = m_param[0] + 1;
		}
		else
		{
			m_param[0] = 0;
		}
		if (m_unit != null)
		{
			game_data._instance.m_arrays[m_world][m_init_pos.y][m_init_pos.x].param[0] = m_param[0];
		}
		reset();
	}
}
