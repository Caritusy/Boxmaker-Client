using System.Collections.Generic;
using UnityEngine;

public class mario_obj : MonoBehaviour
{
	public enum mario_type
	{
		mt_null,
		mt_block,
		mt_block1,
		mt_charater,
		mt_attack,
		mt_attack_ex,
		mt_attack_ex1,
		mt_end
	}

	public enum mario_fx
	{
		mf_left,
		mf_right,
		mf_up
	}

	[HideInInspector]
	public string m_name = string.Empty;

	[HideInInspector]
	public s_t_unit m_unit;

	[HideInInspector]
	public List<int> m_param;

	[HideInInspector]
	public mario_type m_type;

	[HideInInspector]
	public mario_point m_pos = new mario_point();

	[HideInInspector]
	public mario_point m_grid = new mario_point();

	[HideInInspector]
	public mario_point m_per_pos = new mario_point();

	[HideInInspector]
	public mario_point m_per_grid = new mario_point();

	[HideInInspector]
	public Animator m_animator;

	[HideInInspector]
	public mario_anim m_mario_anim;

	[HideInInspector]
	public mario_fx m_fx = mario_fx.mf_right;

	[HideInInspector]
	public mario_point m_velocity = new mario_point();

	[HideInInspector]
	public mario_point m_pvelocity = new mario_point();

	[HideInInspector]
	public mario_collider m_collider;

	[HideInInspector]
	public mario_bound m_bound = new mario_bound();

	[HideInInspector]
	public mario_bound m_per_bound = new mario_bound();

	[HideInInspector]
	public bool m_is_die;

	[HideInInspector]
	public bool m_is_static = true;

	[HideInInspector]
	public int m_is_dzd = 1;

	[HideInInspector]
	public bool m_main;

	[HideInInspector]
	public bool m_wgk;

	[HideInInspector]
	public int m_life;

	[HideInInspector]
	public bool m_is_on_floor;

	[HideInInspector]
	public bool m_per_is_on_floor;

	[HideInInspector]
	public bool m_can_be_on_char;

	[HideInInspector]
	public bool m_is_on_char;

	[HideInInspector]
	public bool m_per_is_on_char;

	[HideInInspector]
	public int m_is_destory;

	[HideInInspector]
	public bool m_bkcf;

	[HideInInspector]
	public List<mario_obj> m_nl_objs = new List<mario_obj>();

	[HideInInspector]
	public List<mario_obj> m_bnl_objs = new List<mario_obj>();

	[HideInInspector]
	public bool m_is_calc_nl;

	[HideInInspector]
	public mario_point m_nl_calc_point = new mario_point();

	[HideInInspector]
	public mario_point m_nl_point = new mario_point();

	[HideInInspector]
	public mario_point m_per_nl_point = new mario_point();

	[HideInInspector]
	public int m_fxdiv = 1;

	[HideInInspector]
	public int m_mocali = 4;

	[HideInInspector]
	public int m_min_speed;

	[HideInInspector]
	public bool m_nleft;

	[HideInInspector]
	public bool m_nright;

	[HideInInspector]
	public bool m_has_floor = true;

	[HideInInspector]
	public bool m_has_main_floor = true;

	[HideInInspector]
	public mario_point m_init_pos = new mario_point();

	[HideInInspector]
	public int m_world;

	[HideInInspector]
	public bool m_edit_mode;

	[HideInInspector]
	public bool m_has_edit;

	[HideInInspector]
	public bool m_is_move;

	[HideInInspector]
	public GameObject m_shadow;

	[HideInInspector]
	public mario_obj m_shadow_obj;

	[HideInInspector]
	public List<int> m_bl = new List<int>();

	[HideInInspector]
	public List<mario_obj> m_bl_objs = new List<mario_obj>();

	[HideInInspector]
	public mario_qtree m_qtree;

	[HideInInspector]
	public int m_no_mc_time;

	[HideInInspector]
	public bool m_is_start;

	[HideInInspector]
	public bool m_is_new;

	public void set_fx(mario_fx fx)
	{
		if (fx != m_fx)
		{
			m_fx = fx;
			if (m_fx == mario_fx.mf_right)
			{
				set_scale(1f, 1f, 1f);
			}
			else if (m_fx == mario_fx.mf_left)
			{
				set_scale(-1f, 1f, 1f);
			}
			else if (m_fx == mario_fx.mf_up)
			{
				set_scale(1f, -1f, 1f);
			}
		}
	}

	public mario_bound get_floor_bound()
	{
		return new mario_bound(m_bound.left, m_bound.right, m_bound.top, m_bound.bottom - 1);
	}

	public mario_bound get_left_bound()
	{
		return new mario_bound(m_bound.left - 1, m_bound.right, m_bound.top, m_bound.bottom);
	}

	public mario_bound get_right_bound()
	{
		return new mario_bound(m_bound.left, m_bound.right + 1, m_bound.top, m_bound.bottom);
	}

	public bool is_die()
	{
		return m_is_die || m_is_destory > 0;
	}

	public virtual void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		m_name = name;
		m_param = param;
		m_collider = GetComponent<mario_collider>();
		m_animator = GetComponent<Animator>();
		m_mario_anim = GetComponent<mario_anim>();
		if (m_mario_anim != null)
		{
			if (m_mario_anim.do_defualt())
			{
				if (m_animator != null)
				{
					m_animator.enabled = false;
				}
			}
			else
			{
				m_mario_anim.enabled = false;
			}
		}
		m_pos.set(xx, yy);
		reset_bound();
		record_per();
		set_pos();
		m_world = world;
		m_init_pos.set(x, y);
		reset();
		if (x == -1)
		{
			return;
		}
		if (x > 0)
		{
			int type = game_data._instance.m_arrays[m_world][y][x - 1].type;
			if (type != 0)
			{
				s_t_unit s_t_unit2 = game_data._instance.get_t_unit(type);
				if (s_t_unit2.is_static == 1)
				{
					m_nleft = true;
				}
			}
		}
		if (x >= game_data._instance.m_map_data.maps[m_world].x_num - 1)
		{
			return;
		}
		int type2 = game_data._instance.m_arrays[m_world][y][x + 1].type;
		if (type2 != 0)
		{
			s_t_unit s_t_unit3 = game_data._instance.get_t_unit(type2);
			if (s_t_unit3.is_static == 1)
			{
				m_nright = true;
			}
		}
	}

	public virtual void reset()
	{
	}

	public void create_shadow(GameObject back)
	{
		if (m_shadow != null)
		{
			Object.Destroy(m_shadow);
		}
		GameObject gameObject = (GameObject)Object.Instantiate(base.gameObject);
		gameObject.transform.parent = back.transform;
		gameObject.transform.localPosition = base.transform.localPosition;
		gameObject.transform.localScale = base.transform.localScale;
		gameObject.transform.localEulerAngles = base.transform.localEulerAngles;
		m_shadow = gameObject;
		m_shadow_obj = m_shadow.GetComponent<mario_obj>();
		if (m_shadow_obj.m_mario_anim != null)
		{
			if (m_shadow_obj.m_mario_anim.do_defualt())
			{
				if (m_shadow_obj.m_animator != null)
				{
					m_shadow_obj.m_animator.enabled = false;
				}
			}
			else
			{
				m_shadow_obj.m_mario_anim.enabled = false;
			}
		}
		format_shadow(m_shadow.transform, "shader/shadow", -10);
	}

	public void format_shadow(Transform ts, string name, int layer)
	{
		SpriteRenderer component = ts.GetComponent<SpriteRenderer>();
		if (component != null)
		{
			Shader shader = Resources.Load(name, typeof(Shader)) as Shader;
			component.sharedMaterial = new Material(shader);
			component.sortingOrder = layer;
		}
		for (int i = 0; i < ts.childCount; i++)
		{
			format_shadow(ts.GetChild(i), name, layer);
		}
	}

	public void destroy_shadow()
	{
		if (m_shadow != null)
		{
			Object.Destroy(m_shadow);
		}
	}

	public void reset_bound()
	{
		m_grid.x = m_pos.x / utils.g_grid_size;
		m_grid.y = m_pos.y / utils.g_grid_size;
		if (!(m_collider == null))
		{
			m_bound.left = m_pos.x + m_collider.m_rect.x - m_collider.m_rect.w / 2;
			m_bound.right = m_pos.x + m_collider.m_rect.x + m_collider.m_rect.w / 2 - 1;
			m_bound.top = m_pos.y + m_collider.m_rect.y + m_collider.m_rect.h / 2 - 1;
			m_bound.bottom = m_pos.y + m_collider.m_rect.y - m_collider.m_rect.h / 2;
			int num = ((m_fxdiv == 1) ? 1 : (m_fxdiv - 1));
			m_bound.left_div = m_pos.x + m_collider.m_rect.x - m_collider.m_rect.w / 2 * num / m_fxdiv;
			m_bound.right_div = m_pos.x + m_collider.m_rect.x + m_collider.m_rect.w / 2 * num / m_fxdiv - 1;
			m_bound.top_div = m_pos.y + m_collider.m_rect.y + m_collider.m_rect.h / 2 * num / m_fxdiv - 1;
			m_bound.bottom_div = m_pos.y + m_collider.m_rect.y - m_collider.m_rect.h / 2 * num / m_fxdiv;
		}
	}

	private void record_per()
	{
		m_per_pos.set(m_pos.x, m_pos.y);
		m_per_grid.set(m_grid.x, m_grid.y);
		m_per_is_on_floor = m_is_on_floor;
		m_per_is_on_char = m_is_on_char;
		m_per_bound.left = m_bound.left;
		m_per_bound.right = m_bound.right;
		m_per_bound.top = m_bound.top;
		m_per_bound.bottom = m_bound.bottom;
		m_per_bound.left_div = m_bound.left_div;
		m_per_bound.right_div = m_bound.right_div;
		m_per_bound.top_div = m_bound.top_div;
		m_per_bound.bottom_div = m_bound.bottom_div;
	}

	public bool check_on_floor(mario_obj obj)
	{
		if (!obj.m_has_floor)
		{
			return false;
		}
		if (m_collider == null || obj.m_collider == null)
		{
			return false;
		}
		return m_bound.bottom - 1 == obj.m_bound.top && m_bound.right >= obj.m_bound.left && m_bound.left <= obj.m_bound.right;
	}

	public bool check_left_floor(mario_obj obj)
	{
		if (!obj.m_has_floor)
		{
			return false;
		}
		if (m_collider == null || obj.m_collider == null)
		{
			return false;
		}
		return m_bound.left - 1 == obj.m_bound.right && m_bound.top >= obj.m_bound.bottom && m_bound.top <= obj.m_bound.top;
	}

	public bool check_right_floor(mario_obj obj)
	{
		if (!obj.m_has_floor)
		{
			return false;
		}
		if (m_collider == null || obj.m_collider == null)
		{
			return false;
		}
		return m_bound.right + 1 == obj.m_bound.left && m_bound.top >= obj.m_bound.bottom && m_bound.top <= obj.m_bound.top;
	}

	public bool left_hit(mario_obj obj)
	{
		if (m_collider == null || obj.m_collider == null)
		{
			return false;
		}
		if (m_main)
		{
			return m_bound.top >= obj.m_bound.bottom_div && m_bound.bottom <= obj.m_bound.top_div && m_per_bound.left > obj.m_per_bound.right && m_bound.left <= obj.m_bound.right;
		}
		return m_bound.top >= obj.m_bound.bottom && m_bound.bottom <= obj.m_bound.top && m_per_bound.left > obj.m_per_bound.right && m_bound.left <= obj.m_bound.right;
	}

	public bool right_hit(mario_obj obj)
	{
		if (m_collider == null || obj.m_collider == null)
		{
			return false;
		}
		if (m_main)
		{
			return m_bound.top >= obj.m_bound.bottom_div && m_bound.bottom <= obj.m_bound.top_div && m_per_bound.right < obj.m_per_bound.left && m_bound.right >= obj.m_bound.left;
		}
		return m_bound.top >= obj.m_bound.bottom && m_bound.bottom <= obj.m_bound.top && m_per_bound.right < obj.m_per_bound.left && m_bound.right >= obj.m_bound.left;
	}

	public bool top_hit(mario_obj obj)
	{
		if (m_collider == null || obj.m_collider == null)
		{
			return false;
		}
		if (m_main)
		{
			return m_bound.right >= obj.m_bound.left_div && m_bound.left <= obj.m_bound.right_div && m_per_bound.top < obj.m_per_bound.bottom && m_bound.top >= obj.m_bound.bottom;
		}
		return m_bound.right >= obj.m_bound.left && m_bound.left <= obj.m_bound.right && m_per_bound.top < obj.m_per_bound.bottom && m_bound.top >= obj.m_bound.bottom;
	}

	public bool bottom_hit(mario_obj obj)
	{
		if (m_collider == null || obj.m_collider == null)
		{
			return false;
		}
		if (m_main)
		{
			return m_bound.right >= obj.m_bound.left_div && m_bound.left <= obj.m_bound.right_div && m_per_bound.bottom > obj.m_per_bound.top && m_bound.bottom <= obj.m_bound.top;
		}
		return m_bound.right >= obj.m_bound.left && m_bound.left <= obj.m_bound.right && m_per_bound.bottom > obj.m_per_bound.top && m_bound.bottom <= obj.m_bound.top;
	}

	public bool left_top_hit(mario_obj obj)
	{
		if (m_collider == null || obj.m_collider == null)
		{
			return false;
		}
		bool flag = m_bound.top < obj.m_bound.bottom_div && m_bound.top >= obj.m_bound.bottom && m_per_bound.left > obj.m_per_bound.right && m_bound.left <= obj.m_bound.right;
		bool flag2 = m_bound.left > obj.m_bound.right_div && m_bound.left <= obj.m_bound.right && m_per_bound.top < obj.m_per_bound.bottom && m_bound.top >= obj.m_bound.bottom;
		bool flag3 = m_per_bound.left > obj.m_per_bound.right && m_bound.left <= obj.m_bound.right && m_per_bound.top < obj.m_per_bound.bottom && m_bound.top >= obj.m_bound.bottom;
		return flag || flag2 || flag3;
	}

	public bool right_top_hit(mario_obj obj)
	{
		if (m_collider == null || obj.m_collider == null)
		{
			return false;
		}
		bool flag = m_bound.top < obj.m_bound.bottom_div && m_bound.top >= obj.m_bound.bottom && m_per_bound.right < obj.m_per_bound.left && m_bound.right >= obj.m_bound.left;
		bool flag2 = m_bound.right < obj.m_bound.left_div && m_bound.right >= obj.m_bound.left && m_per_bound.top < obj.m_per_bound.bottom && m_bound.top >= obj.m_bound.bottom;
		bool flag3 = m_per_bound.right < obj.m_per_bound.left && m_bound.right >= obj.m_bound.left && m_per_bound.top < obj.m_per_bound.bottom && m_bound.top >= obj.m_bound.bottom;
		return flag || flag2 || flag3;
	}

	public bool left_bottom_hit(mario_obj obj)
	{
		if (m_collider == null || obj.m_collider == null)
		{
			return false;
		}
		bool flag = m_bound.bottom > obj.m_bound.top_div && m_bound.bottom <= obj.m_bound.top && m_per_bound.left > obj.m_per_bound.right && m_bound.left <= obj.m_bound.right;
		bool flag2 = m_bound.left > obj.m_bound.right_div && m_bound.left <= obj.m_bound.right && m_per_bound.bottom > obj.m_per_bound.top && m_bound.bottom <= obj.m_bound.top;
		bool flag3 = m_per_bound.left > obj.m_per_bound.right && m_bound.left <= obj.m_bound.right && m_per_bound.bottom > obj.m_per_bound.top && m_bound.bottom <= obj.m_bound.top;
		return flag || flag2 || flag3;
	}

	public bool right_bottom_hit(mario_obj obj)
	{
		if (m_collider == null || obj.m_collider == null)
		{
			return false;
		}
		bool flag = m_bound.bottom > obj.m_bound.top_div && m_bound.bottom <= obj.m_bound.top && m_per_bound.right < obj.m_per_bound.left && m_bound.right >= obj.m_bound.left;
		bool flag2 = m_bound.right < obj.m_bound.left_div && m_bound.right >= obj.m_bound.left && m_per_bound.bottom > obj.m_per_bound.top && m_bound.bottom <= obj.m_bound.top;
		bool flag3 = m_per_bound.right < obj.m_per_bound.left && m_bound.right >= obj.m_bound.left && m_per_bound.bottom > obj.m_per_bound.top && m_bound.bottom <= obj.m_bound.top;
		return flag || flag2 || flag3;
	}

	public bool hit(mario_obj obj)
	{
		if (m_collider == null || obj.m_collider == null)
		{
			return false;
		}
		return m_bound.right >= obj.m_bound.left && m_bound.left <= obj.m_bound.right && m_bound.top >= obj.m_bound.bottom && m_bound.bottom <= obj.m_bound.top;
	}

	public int get_left_hit_pos(mario_obj obj)
	{
		if (m_collider == null || obj.m_collider == null)
		{
			return 0;
		}
		return obj.m_pos.x + obj.m_collider.m_rect.x + obj.m_collider.m_rect.w / 2 + m_collider.m_rect.w / 2 - m_collider.m_rect.x;
	}

	public int get_right_hit_pos(mario_obj obj)
	{
		if (m_collider == null || obj.m_collider == null)
		{
			return 0;
		}
		return obj.m_pos.x + obj.m_collider.m_rect.x - obj.m_collider.m_rect.w / 2 - m_collider.m_rect.w / 2 - m_collider.m_rect.x;
	}

	public int get_top_hit_pos(mario_obj obj)
	{
		if (m_collider == null || obj.m_collider == null)
		{
			return 0;
		}
		return obj.m_pos.y + obj.m_collider.m_rect.y - obj.m_collider.m_rect.h / 2 - m_collider.m_rect.h / 2 - m_collider.m_rect.y;
	}

	public int get_bottom_hit_pos(mario_obj obj)
	{
		if (m_collider == null || obj.m_collider == null)
		{
			return 0;
		}
		return obj.m_pos.y + obj.m_collider.m_rect.y + obj.m_collider.m_rect.h / 2 + m_collider.m_rect.h / 2 - m_collider.m_rect.y;
	}

	protected string get_anim_name()
	{
		if (m_mario_anim != null)
		{
			return m_mario_anim.get_name();
		}
		return string.Empty;
	}

	public void play_anim(string name, int speed = -1)
	{
		if (m_mario_anim != null && m_mario_anim.has_anim(name))
		{
			if (m_animator != null && m_animator.enabled)
			{
				m_animator.enabled = false;
				if (m_shadow_obj != null)
				{
					m_shadow_obj.m_animator.enabled = false;
				}
			}
			if (!m_mario_anim.enabled)
			{
				m_mario_anim.enabled = true;
				if (m_shadow_obj != null)
				{
					m_shadow_obj.m_mario_anim.enabled = true;
				}
			}
			m_mario_anim.play(name, speed);
			if (m_shadow_obj != null)
			{
				m_shadow_obj.m_mario_anim.play(name, speed);
			}
		}
		else
		{
			if (!(m_animator != null))
			{
				return;
			}
			if (m_mario_anim != null && m_mario_anim.enabled)
			{
				m_mario_anim.enabled = false;
				if (m_shadow_obj != null)
				{
					m_shadow_obj.m_mario_anim.enabled = false;
				}
			}
			if (!m_animator.enabled)
			{
				m_animator.enabled = true;
				if (m_shadow_obj != null)
				{
					m_shadow_obj.m_animator.enabled = true;
				}
			}
			if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName(name))
			{
				m_animator.Play(name);
				if (m_shadow_obj != null)
				{
					m_shadow_obj.m_animator.Play(name);
				}
			}
		}
	}

	protected void stop_anim()
	{
		if (m_mario_anim != null && m_mario_anim.enabled)
		{
			m_mario_anim.enabled = false;
			if (m_shadow_obj != null)
			{
				m_shadow_obj.m_mario_anim.enabled = false;
			}
		}
		if (m_animator != null && m_animator.enabled)
		{
			m_animator.enabled = false;
			if (m_shadow_obj != null)
			{
				m_shadow_obj.m_animator.enabled = false;
			}
		}
	}

	protected void pause_anim()
	{
		if (m_mario_anim != null)
		{
			m_mario_anim.pause();
			if (m_shadow_obj != null)
			{
				m_shadow_obj.m_mario_anim.pause();
			}
		}
		if (m_animator != null)
		{
			m_animator.speed = 0f;
			if (m_shadow_obj != null)
			{
				m_shadow_obj.m_animator.speed = 0f;
			}
		}
	}

	protected void continue_anim()
	{
		if (m_mario_anim != null)
		{
			m_mario_anim.conti();
			if (m_shadow_obj != null)
			{
				m_shadow_obj.m_mario_anim.conti();
			}
		}
		if (m_animator != null)
		{
			m_animator.speed = 1f;
			if (m_shadow_obj != null)
			{
				m_shadow_obj.m_animator.speed = 1f;
			}
		}
	}

	public void check_is_start(mario_point grid)
	{
		int num = m_grid.x - grid.x;
		int num2 = m_grid.y - grid.y;
		if (m_unit == null)
		{
			m_is_start = true;
		}
		if (m_main)
		{
			m_is_start = true;
		}
		else if (num >= -utils.g_start_x && num <= utils.g_start_x && num2 >= -utils.g_start_y && num2 <= utils.g_start_y)
		{
			m_is_start = true;
		}
		else if (num < -utils.g_del_x || num > utils.g_del_x || num2 < -utils.g_del_y || num2 > utils.g_del_y)
		{
			m_is_start = false;
		}
	}

	public void check_state(mario_point grid)
	{
		m_is_new = false;
		int num = m_grid.x - grid.x;
		int num2 = m_grid.y - grid.y;
		if (!Application.isEditor && !m_main)
		{
			if (num >= -utils.g_active_x && num <= utils.g_active_x && num2 >= -utils.g_active_y && num2 <= utils.g_active_y)
			{
				if (!base.gameObject.activeSelf)
				{
					base.gameObject.SetActive(value: true);
					if (m_shadow != null)
					{
						m_shadow.gameObject.SetActive(value: true);
					}
				}
			}
			else if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(value: false);
				if (m_shadow != null)
				{
					m_shadow.gameObject.SetActive(value: false);
				}
			}
		}
		check_is_start(grid);
		if (m_unit != null && m_unit.id == utils.g_ryqiu)
		{
			if (num < -utils.g_del_x || num > utils.g_del_x)
			{
				m_is_destory = 2;
			}
		}
		else if (!m_main && (num < -utils.g_start_x || num > utils.g_start_x || num2 < -utils.g_start_y || num2 > utils.g_start_y) && !play_mode._instance.need_calc(m_grid.x, m_grid.y))
		{
			m_is_destory = 2;
		}
		if (m_main)
		{
			if (m_pos.y < -100)
			{
				m_is_die = true;
			}
		}
		else if ((m_unit == null || m_unit.id != utils.g_ryqiu) && m_pos.y < -utils.g_grid_size)
		{
			m_is_destory = 1;
		}
	}

	public virtual void tupdate()
	{
	}

	public void update_ex(mario_point grid)
	{
		if (!m_edit_mode)
		{
			check_state(grid);
			tupdate();
		}
		if (m_edit_mode && m_has_edit)
		{
			play_anim("edit");
		}
		if (m_per_pos.x != m_pos.x || m_per_pos.y != m_pos.y)
		{
			set_pos();
		}
		if (m_no_mc_time > 0)
		{
			m_no_mc_time--;
		}
		record_per();
		m_nl_objs.Clear();
		m_bnl_objs.Clear();
		m_is_calc_nl = false;
	}

	private void set_pos()
	{
		base.transform.localPosition = new Vector3((float)m_pos.x / 10f, (float)m_pos.y / 10f, 0f);
		if (m_shadow != null)
		{
			m_shadow.transform.localPosition = new Vector3((float)m_pos.x / 10f, (float)m_pos.y / 10f, 0f);
		}
	}

	protected void set_angles(float x, float y, float z)
	{
		base.transform.localEulerAngles = new Vector3(x, y, z);
		if (m_shadow != null)
		{
			m_shadow.transform.localEulerAngles = new Vector3(x, y, z);
		}
	}

	protected void set_scale(float x, float y, float z)
	{
		base.transform.localScale = new Vector3(x, y, z);
		if (m_shadow != null)
		{
			m_shadow.transform.localScale = new Vector3(x, y, z);
		}
	}

	public virtual bool be_left_hit(mario_obj obj, ref int px)
	{
		return false;
	}

	public virtual bool be_right_hit(mario_obj obj, ref int px)
	{
		return false;
	}

	public virtual bool be_top_hit(mario_obj obj, ref int py)
	{
		return false;
	}

	public virtual bool be_bottom_hit(mario_obj obj, ref int py)
	{
		return false;
	}

	public virtual bool be_left_top_hit(mario_obj obj, ref int px, ref int py)
	{
		return false;
	}

	public virtual bool be_right_top_hit(mario_obj obj, ref int px, ref int py)
	{
		return false;
	}

	public virtual bool be_left_bottom_hit(mario_obj obj, ref int px, ref int py)
	{
		return false;
	}

	public virtual bool be_right_bottom_hit(mario_obj obj, ref int px, ref int py)
	{
		return false;
	}

	public virtual void be_hit(mario_obj obj)
	{
	}

	public virtual mario_point move()
	{
		return new mario_point();
	}

	public void do_move()
	{
		m_is_calc_nl = true;
		m_nl_calc_point = move();
	}

	protected void change_nl_fx(mario_fx fx)
	{
		if (m_type != mario_type.mt_charater)
		{
			return;
		}
		for (int i = 0; i < m_nl_objs.Count; i++)
		{
			if (m_nl_objs[i].m_min_speed != 0)
			{
				m_nl_objs[i].set_fx(fx);
			}
			m_nl_objs[i].change_nl_fx(fx);
		}
	}

	public mario_point get_nl_calc_point()
	{
		if (m_is_calc_nl)
		{
			return m_nl_calc_point;
		}
		if (m_bnl_objs.Count > 0)
		{
			int num = 0;
			int num2 = 0;
			int num3 = -999999;
			for (int i = 0; i < m_bnl_objs.Count; i++)
			{
				mario_point nl_calc_point = m_bnl_objs[i].get_nl_calc_point();
				if (nl_calc_point.x > 0 && nl_calc_point.x > num)
				{
					num = nl_calc_point.x;
				}
				if (nl_calc_point.x < 0 && nl_calc_point.x < num2)
				{
					num2 = nl_calc_point.x;
				}
				if (nl_calc_point.y > num3)
				{
					num3 = nl_calc_point.y;
				}
			}
			int num4 = num + num2;
			m_nl_calc_point = new mario_point(m_velocity.x + num4, m_velocity.y + num3);
		}
		else
		{
			m_nl_calc_point = new mario_point(m_velocity.x, m_velocity.y);
		}
		m_is_calc_nl = true;
		return m_nl_calc_point;
	}

	public virtual void change()
	{
	}

	public virtual void change1()
	{
	}

	public virtual void set_bl(int index, int num)
	{
		m_bl[index] = num;
	}

	public virtual void change_way(mario_fx fx)
	{
	}

	public Renderer GetRenderer()
	{
		return this.renderer;
	}
}
