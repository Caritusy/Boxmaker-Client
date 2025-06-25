using BMReborn.AntiCheat;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class play_mode : MonoBehaviour
{
	public Camera m_cam;

	public GameObject m_bj;

	public GameObject m_other;

	public GameObject m_back;

	public GameObject m_main;

	public GameObject m_shadow;

	public List<bool> m_now_inputs = new List<bool>();

	private List<bool> m_per_inputs = new List<bool>();

	private List<List<mario_obj>> m_aobjs = new List<List<mario_obj>>();

	private List<mario_obj> m_mobjs = new List<mario_obj>();

	private List<List<mario_obj>> m_xyobjs = new List<List<mario_obj>>();

	public mario_qtree m_qt = new mario_qtree();

	public mario_point m_roll = new mario_point();

	private mario_point m_grid = new mario_point();

	private int m_y_roll_target;

	public mario_obj m_main_char;

	private mario_scene m_scene;

	private int m_left;

	private int m_left_time;

	private int m_right;

	private int m_right_time;

	private int m_shache_type;

	public int m_shache_time;

	private bool m_jump;

	private bool m_sjump;

	private List<int> m_inputs = new List<int>();

	private List<int> m_good_inputs = new List<int>();

	private bool m_zr;

	private bool m_caisi;

	private int m_caisi_y;

	public int m_paqiang;

	private bool m_paqiang_jump;

	private int m_jump_state;

	public int m_jump_num;

	private int m_jump_num_time;

	private int m_jump_x;

	private bool m_chuan;

	public int m_time;

	public int m_total_time;

	private int m_mode;

	private int m_rindex;

	public int m_state;

	public GameObject m_render;

	private mario_point m_qpos;

	private List<HashSet<int>> m_has_create = new List<HashSet<int>>();

	public bool m_pause;

	public mario_point m_die_pos = new mario_point();

	public int m_score;

	public int m_ys;

	private int m_show_cha;

	private int m_show_cha_time;

	private bool m_has_time_tx;

	private int m_time_tx_time;

	private int m_zhuiji_x;

	private GameObject m_zhuiji;

	private List<mario_obj> bobjs = new List<mario_obj>();

	private List<mario_obj> tobjs = new List<mario_obj>();

	private HashSet<int> m_need_calc = new HashSet<int>();

	private List<Dictionary<int, List<mario_obj>>> m_delete_objs = new List<Dictionary<int, List<mario_obj>>>();

	public int m_world;

	private mario_obj m_csm;

	public static play_mode _instance;

	private void Awake()
	{
		_instance = this;
	}

	public void reload()
	{
		m_qpos = null;
		reset(0);
	}

	public void reload(mario_point qpos, int world, int mode)
	{
		m_qpos = qpos;
		m_mode = mode;
		reset(world);
	}

	public void reload_self(mario_point qpos, int world)
	{
		m_qpos = qpos;
		reset(world, all: false);
	}

	public void reset(int world, bool all = true)
	{
		m_world = world;
		if (all)
		{
			if (m_mode == 0)
			{
				m_inputs = new List<int>();
				if (m_good_inputs.Count > 0)
				{
					m_inputs.AddRange(m_good_inputs);
				}
			}
			else
			{
				m_inputs = game_data._instance.m_map_inputs;
			}
			m_has_create.Clear();
			m_delete_objs.Clear();
			for (int i = 0; i < 3; i++)
			{
				m_has_create.Add(new HashSet<int>());
				m_delete_objs.Add(new Dictionary<int, List<mario_obj>>());
			}
			m_now_inputs.Clear();
			m_per_inputs.Clear();
			for (int j = 0; j < 4; j++)
			{
				m_now_inputs.Add(item: false);
				m_per_inputs.Add(item: false);
			}
			m_rindex = 0;
			m_time = 0;
			m_total_time = 0;
			m_score = 0;
			m_ys = game_data._instance.SUPERSCRECTDEBUG_INFKEYS ? 999999 : 0;
			m_need_calc.Clear();
			mario._instance.remove_child(m_main);
			mario._instance.remove_child(m_shadow);
			mario._instance.remove_child(m_other);
			m_left = 0;
			m_right = 0;
			m_has_time_tx = false;
			m_time_tx_time = 0;
		}
		m_mobjs.Clear();
		m_aobjs.Clear();
		for (int k = 0; k < 7; k++)
		{
			m_aobjs.Add(new List<mario_obj>());
		}
		m_xyobjs.Clear();
		for (int l = 0; l < game_data._instance.m_map_data.maps[m_world].y_num; l++)
		{
			List<mario_obj> list = new List<mario_obj>();
			for (int m = 0; m < game_data._instance.m_map_data.maps[m_world].x_num; m++)
			{
				list.Add(null);
			}
			m_xyobjs.Add(list);
		}
		m_qt = new mario_qtree();
		if ((bool)m_zhuiji)
		{
			Object.Destroy(m_zhuiji);
			m_zhuiji = null;
		}
		m_roll = new mario_point();
		m_y_roll_target = 0;
		m_main_char = null;
		m_left_time = 0;
		m_right_time = 0;
		m_shache_type = 0;
		m_shache_time = 0;
		m_jump = false;
		m_sjump = false;
		m_caisi = false;
		m_paqiang = 0;
		m_paqiang_jump = false;
		m_jump_state = 0;
		m_jump_num = 0;
		m_jump_num_time = 0;
		m_chuan = false;
		m_show_cha = 0;
		m_show_cha_time = 0;
		m_state = 0;
		m_pause = false;
		m_grid = new mario_point();
		m_csm = null;
		m_zr = false;
		int x = 1;
		int num = game_data._instance.m_map_data.maps[m_world].qd_y;
		if (m_qpos != null)
		{
			x = m_qpos.x / utils.g_grid_size;
			num = m_qpos.y / utils.g_grid_size;
		}
		if (num < 0)
		{
			num = 0;
		}
		else if (num >= game_data._instance.m_map_data.maps[m_world].y_num)
		{
			num = game_data._instance.m_map_data.maps[m_world].y_num - 1;
		}
		m_main_char = create_mario_obj("mario_main", null, new List<int>(), x, num);
		m_roll.x = m_main_char.m_pos.x;
		m_roll.y = m_main_char.m_pos.y - utils.g_roll_y;
		m_y_roll_target = m_roll.y;
		if (m_roll.y + utils.g_height > game_data._instance.m_map_data.maps[m_world].y_num * utils.g_grid_size)
		{
			m_roll.y = game_data._instance.m_map_data.maps[m_world].y_num * utils.g_grid_size - utils.g_height;
		}
		if (m_roll.y < 0)
		{
			m_roll.y = 0;
		}
		m_grid.x = m_roll.x / utils.g_grid_size;
		m_grid.y = (m_roll.y + utils.g_roll_y) / utils.g_grid_size;
		for (int n = m_grid.x - utils.g_start_x; n <= m_grid.x + utils.g_start_x; n++)
		{
			if (n < 0 || n >= game_data._instance.m_map_data.maps[m_world].x_num)
			{
				continue;
			}
			for (int num2 = m_grid.y - utils.g_start_y; num2 <= m_grid.y + utils.g_start_y; num2++)
			{
				if (num2 >= 0 && num2 < game_data._instance.m_map_data.maps[m_world].y_num)
				{
					do_create(n, num2);
				}
			}
		}
		create_scene();
		if (game_data._instance.m_map_data.mode != 0)
		{
			m_zhuiji_x = m_main_char.m_pos.x - utils.g_grid_size * 5;
			GameObject original = (GameObject)Resources.Load("unit/other/zhuiji");
			m_zhuiji = (GameObject)Object.Instantiate(original);
			m_zhuiji.transform.parent = m_other.transform;
			m_zhuiji.transform.localPosition = new Vector3(m_zhuiji_x / 10, 0f, 0f);
			m_zhuiji.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		update_ex();
		check_create();
	}

	public void create_scene()
	{
		mario._instance.remove_child(m_back);
		string path = "scene/" + game_data._instance.m_map_data.maps[m_world].map_theme + "/scene";
		GameObject original = (GameObject)Resources.Load(path);
		GameObject gameObject = (GameObject)Object.Instantiate(original);
		gameObject.transform.parent = m_back.transform;
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		m_scene = gameObject.GetComponent<mario_scene>();
	}

	public mario_obj create_mario_obj(string name, s_t_unit unit, List<int> param, int x, int y)
	{
		return create_mario_obj_ex(name, unit, param, x, y, utils.g_grid_size * x + utils.g_grid_size / 2, utils.g_grid_size * y + utils.g_grid_size / 2);
	}

	public mario_obj create_mario_obj_ex(string name, s_t_unit unit, List<int> param, int x, int y, int xx, int yy)
	{
		try
		{
			string path = "unit/" + name + "/" + name;
			if (unit != null && unit.kfg == 1)
			{
				path = "unit/" + name + "/" + game_data._instance.m_map_data.maps[m_world].map_theme + "/" + name;
			}
			GameObject original = (GameObject)Resources.Load(path);
			GameObject gameObject = (GameObject)Object.Instantiate(original);
			gameObject.transform.parent = m_main.transform;
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			mario_obj component = gameObject.GetComponent<mario_obj>();
			component.m_unit = unit;
			Transform transform = gameObject.transform.FindChild("fx");
			if (transform != null)
			{
				transform.gameObject.SetActive(value: false);
			}
			component.init(name, param, m_world, x, y, xx, yy);
			add_obj(component);
			component.create_shadow(m_shadow);
			component.m_is_new = true;
			component.check_is_start(m_grid);
			return component;
		}
		catch (Exception ex)
		{
			Debug.LogError(ex);
			return null;
		}
	}

	private void add_obj(mario_obj obj)
	{
		m_mobjs.Add(obj);
		if (obj.m_init_pos.x != -1)
		{
			m_xyobjs[obj.m_init_pos.y][obj.m_init_pos.x] = obj;
		}
		if (obj.m_type != 0)
		{
			m_aobjs[(int)obj.m_type].Add(obj);
			m_qt.insert(obj);
		}
	}

	private void remove_obj(mario_obj obj)
	{
		m_mobjs.Remove(obj);
		if (obj.m_init_pos.x != -1)
		{
			m_xyobjs[obj.m_init_pos.y][obj.m_init_pos.x] = null;
		}
		if (obj.m_type != 0)
		{
			m_aobjs[(int)obj.m_type].Remove(obj);
			if (obj.m_qtree != null)
			{
				obj.m_qtree.remove(obj);
				obj.m_qtree = null;
			}
		}
	}

	private mario_obj get_obj(int x, int y)
	{
		if (x < 0 || x >= game_data._instance.m_map_data.maps[m_world].x_num || y < 0 || y >= game_data._instance.m_map_data.maps[m_world].y_num)
		{
			return null;
		}
		return m_xyobjs[y][x];
	}

	public void refresh_obj(mario_obj obj)
	{
		if (obj.m_type == mario_obj.mario_type.mt_null)
		{
			return;
		}
		obj.reset_bound();
		if (obj.m_pos.x != obj.m_per_pos.x || obj.m_pos.y != obj.m_per_pos.y)
		{
			if (obj.m_qtree != null)
			{
				obj.m_qtree.remove(obj);
				obj.m_qtree = null;
			}
			m_qt.insert(obj);
		}
	}

	private void check_input()
	{
		bool flag = false;
		if (m_good_inputs.Count > 0 && !m_zr && m_good_inputs[m_good_inputs.Count - 2] >= m_time)
		{
			flag = true;
		}
		if (m_mode == 0 && !flag)
		{
			if (m_now_inputs[0] && !m_per_inputs[0])
			{
				m_inputs.Add(m_time);
				m_inputs.Add(0);
				if (m_left_time > 0)
				{
					m_left = 2;
				}
				else
				{
					m_left = 1;
				}
				m_left_time = 15;
			}
			if (!m_now_inputs[0] && m_per_inputs[0])
			{
				m_inputs.Add(m_time);
				m_inputs.Add(1);
				m_left = 0;
				m_shache_time = 0;
			}
			if (m_now_inputs[1] && !m_per_inputs[1])
			{
				m_inputs.Add(m_time);
				m_inputs.Add(2);
				if (m_right_time > 0)
				{
					m_right = 2;
				}
				else
				{
					m_right = 1;
				}
				m_right_time = 15;
			}
			if (!m_now_inputs[1] && m_per_inputs[1])
			{
				m_inputs.Add(m_time);
				m_inputs.Add(3);
				m_right = 0;
				m_shache_time = 0;
			}
			if (m_now_inputs[2] && !m_per_inputs[2])
			{
				m_inputs.Add(m_time);
				m_inputs.Add(4);
				m_jump = true;
			}
			if (!m_now_inputs[2] && m_per_inputs[2])
			{
				m_inputs.Add(m_time);
				m_inputs.Add(5);
				m_jump = false;
			}
			if (m_now_inputs[3] && !m_per_inputs[3])
			{
				m_inputs.Add(m_time);
				m_inputs.Add(6);
				m_chuan = true;
			}
			if (!m_now_inputs[3] && m_per_inputs[3])
			{
				m_inputs.Add(m_time);
				m_inputs.Add(7);
				m_chuan = false;
			}
		}
		else
		{
			while (m_rindex < m_inputs.Count && m_inputs[m_rindex] == m_time)
			{
				m_rindex++;
				if (m_inputs[m_rindex] == 0)
				{
					if (m_left_time > 0)
					{
						m_left = 2;
					}
					else
					{
						m_left = 1;
					}
					m_left_time = 15;
				}
				if (m_inputs[m_rindex] == 1)
				{
					m_left = 0;
					m_shache_time = 0;
				}
				if (m_inputs[m_rindex] == 2)
				{
					if (m_right_time > 0)
					{
						m_right = 2;
					}
					else
					{
						m_right = 1;
					}
					m_right_time = 15;
				}
				if (m_inputs[m_rindex] == 3)
				{
					m_right = 0;
					m_shache_time = 0;
				}
				if (m_inputs[m_rindex] == 4)
				{
					m_jump = true;
				}
				if (m_inputs[m_rindex] == 5)
				{
					m_jump = false;
				}
				if (m_inputs[m_rindex] == 6)
				{
					m_chuan = true;
				}
				if (m_inputs[m_rindex] == 7)
				{
					m_chuan = false;
				}
				m_rindex++;
			}
		}
		for (int i = 0; i < m_now_inputs.Count; i++)
		{
			m_per_inputs[i] = m_now_inputs[i];
		}
		if (m_main_char != null && m_state == 0 && !m_main_char.is_die() && m_main_char.m_no_mc_time == 0)
		{
			if (m_left > 0)
			{
				m_main_char.set_fx(mario_obj.mario_fx.mf_left);
			}
			else if (m_right > 0)
			{
				m_main_char.set_fx(mario_obj.mario_fx.mf_right);
			}
		}
		if (m_chuan && m_csm != null)
		{
			int w = 0;
			int x = 0;
			int y = 0;
			if (game_data._instance.get_csm(m_world, m_csm.m_init_pos.x, m_csm.m_init_pos.y, ref w, ref x, ref y))
			{
				m_pause = true;
				m_csm.GetComponent<mario_obj>().play_anim("chuan");
				mario._instance.play_sound("sound/men");
				mario._instance.show_play_mask(delegate
				{
					int yin_time = m_main_char.GetComponent<mario_main>().m_yin_time;
					int big = m_main_char.GetComponent<mario_main>().m_big;
					for (int j = 0; j < m_mobjs.Count; j++)
					{
						m_mobjs[j].m_is_destory = 2;
					}
					check_delete();
					if (w != m_world)
					{
						if (m_has_time_tx)
						{
							mario._instance.play_mus(game_data._instance.get_map_music(w), loop: true, 1 - game_data._instance.m_map_data.no_music, 1.5f);
						}
						else
						{
							mario._instance.play_mus(game_data._instance.get_map_music(w), loop: true, 1 - game_data._instance.m_map_data.no_music);
						}
					}
					reload_self(new mario_point(x * utils.g_grid_size, y * utils.g_grid_size), w);
					m_main_char.GetComponent<mario_main>().set_cs(big, yin_time);
				});
			}
		}
		m_chuan = false;
		m_paqiang_jump = false;
		if (m_paqiang > 0 && m_jump)
		{
			m_paqiang_jump = true;
			m_main_char.m_velocity.y = 0;
			m_main_char.m_pvelocity.y = 0;
			m_jump_state = 100;
			m_jump_num = 0;
			if (m_paqiang == 1)
			{
				m_main_char.m_velocity.x = 120;
				m_main_char.set_fx(mario_obj.mario_fx.mf_right);
				_instance.effect("hit", m_main_char.m_pos.x - 310, m_main_char.m_pos.y - utils.g_grid_size / 2);
			}
			else
			{
				m_main_char.m_velocity.x = -120;
				m_main_char.set_fx(mario_obj.mario_fx.mf_left);
				_instance.effect("hit", m_main_char.m_pos.x + 310, m_main_char.m_pos.y - utils.g_grid_size / 2);
			}
			m_main_char.m_no_mc_time = 20;
			mario._instance.play_sound_ex("sound/yo");
			mario._instance.play_sound("sound/jump", 0.5f);
			m_sjump = false;
		}
		else if (m_caisi && m_jump)
		{
			m_main_char.m_velocity.y = 0;
			m_main_char.m_pvelocity.y = m_caisi_y;
			m_jump_state = 100;
			m_jump_num = 0;
			m_jump_x = ((m_main_char.m_velocity.x <= 0) ? (-m_main_char.m_velocity.x) : m_main_char.m_velocity.x);
			float vols = 1f;
			if (m_main_char.m_velocity.x < -80 || m_main_char.m_velocity.x > 80)
			{
				m_jump_num++;
				m_jump_num_time = 10;
				if (m_jump_num == 2)
				{
					mario._instance.play_sound_ex("sound/ya");
					vols = 0.5f;
				}
				else if (m_jump_num == 3)
				{
					mario._instance.play_sound_ex("sound/yaho");
					vols = 0.5f;
				}
				if (m_jump_num >= 4)
				{
					m_jump_num = 1;
				}
			}
			else
			{
				m_jump_num = 0;
				m_jump_num_time = 0;
			}
			mario._instance.play_sound("sound/jump", vols);
			m_sjump = false;
		}
		else if (m_jump_state == 0 && m_jump)
		{
			m_jump_state = 100;
			m_jump_x = ((m_main_char.m_velocity.x <= 0) ? (-m_main_char.m_velocity.x) : m_main_char.m_velocity.x);
			float vols2 = 1f;
			if (m_main_char.m_velocity.x < -80 || m_main_char.m_velocity.x > 80)
			{
				m_jump_num++;
				m_jump_num_time = 10;
				if (m_jump_num == 2)
				{
					mario._instance.play_sound_ex("sound/ya");
					vols2 = 0.5f;
				}
				else if (m_jump_num == 3)
				{
					mario._instance.play_sound_ex("sound/yaho");
					vols2 = 0.5f;
				}
				if (m_jump_num >= 4)
				{
					m_jump_num = 1;
				}
			}
			else
			{
				m_jump_num = 0;
				m_jump_num_time = 0;
			}
			mario._instance.play_sound("sound/jump", vols2);
			m_sjump = false;
		}
		if (m_main_char.m_pvelocity.y < 0)
		{
			m_jump_state = 1;
			m_jump = false;
		}
		if (m_jump_state >= 100 && !m_jump)
		{
			m_sjump = true;
		}
		int num = m_jump_num - 1;
		if (num < 0)
		{
			num = 0;
		}
		if (m_jump_state >= 102)
		{
			m_jump_state++;
			m_main_char.m_velocity.y += 10 + num;
		}
		else if (m_jump_state >= 100)
		{
			m_jump_state++;
			m_main_char.m_velocity.y += 90 + m_jump_x / 20;
		}
		if (m_jump_state > 120 + num * 2)
		{
			m_jump_state = 1;
			m_jump = false;
		}
		if (m_jump_state >= 102 && m_sjump)
		{
			m_jump_state = 1;
		}
	}

	private void check_zhli()
	{
		for (int i = 0; i < m_aobjs[3].Count; i++)
		{
			mario_obj mario_obj2 = m_aobjs[3][i];
			if (mario_obj2.is_die() || !mario_obj2.m_is_start)
			{
				continue;
			}
			bool flag = false;
			int num = 0;
			if (mario_obj2 == m_main_char && m_jump_state >= 100)
			{
				flag = true;
			}
			if (!flag && mario_obj2.m_velocity.y != 0)
			{
				flag = true;
			}
			if (!flag)
			{
				bool flag2 = false;
				bobjs.Clear();
				mario_obj2.m_is_on_char = false;
				m_qt.retrive_floor(mario_obj2, ref bobjs);
				for (int j = 0; j < bobjs.Count; j++)
				{
					mario_obj mario_obj3 = bobjs[j];
					if (!(mario_obj2 == mario_obj3) && !mario_obj3.is_die() && !mario_obj3.m_main && (!mario_obj2.m_main || mario_obj3.m_has_main_floor) && mario_obj2.check_on_floor(mario_obj3))
					{
						flag2 = true;
						mario_obj2.m_is_on_floor = true;
						mario_obj3.m_nl_objs.Add(mario_obj2);
						mario_obj2.m_bnl_objs.Add(mario_obj3);
						if (mario_obj3.m_can_be_on_char)
						{
							mario_obj2.m_is_on_char = true;
						}
						if (mario_obj2 == m_main_char)
						{
							m_jump_state = 0;
						}
						if (mario_obj3.m_mocali > num)
						{
							num = mario_obj3.m_mocali;
						}
					}
				}
				if (!flag2)
				{
					flag = true;
				}
			}
			if (flag && mario_obj2.m_is_on_floor)
			{
				mario_obj2.m_velocity.x += mario_obj2.m_per_nl_point.x;
				mario_obj2.m_velocity.y += mario_obj2.m_per_nl_point.y;
				mario_obj2.m_is_on_floor = false;
			}
			if (mario_obj2.m_main)
			{
				if (flag)
				{
					m_shache_time = 0;
					if (m_main_char.m_no_mc_time <= 0)
					{
						if (m_left == 1)
						{
							m_main_char.m_is_move = true;
							if (m_main_char.m_velocity.x <= -84)
							{
								m_main_char.m_velocity.x += 4;
							}
							else if (m_main_char.m_velocity.x <= -80)
							{
								m_main_char.m_velocity.x = -80;
							}
							else if (m_main_char.m_velocity.x <= 0)
							{
								m_main_char.m_velocity.x -= 2;
							}
							else
							{
								m_main_char.m_velocity.x -= 10;
							}
						}
						else if (m_left == 2)
						{
							m_main_char.m_is_move = true;
							if (m_main_char.m_velocity.x <= -164)
							{
								m_main_char.m_velocity.x += 4;
							}
							else if (m_main_char.m_velocity.x <= -160)
							{
								m_main_char.m_velocity.x = -160;
							}
							else if (m_main_char.m_velocity.x <= 0)
							{
								if (m_main_char.m_velocity.x <= -80)
								{
									m_main_char.m_velocity.x--;
								}
								else
								{
									m_main_char.m_velocity.x -= 3;
								}
							}
							else
							{
								m_main_char.m_velocity.x -= 12;
							}
						}
						else if (m_right == 1)
						{
							m_main_char.m_is_move = true;
							if (m_main_char.m_velocity.x >= 84)
							{
								m_main_char.m_velocity.x -= 4;
							}
							else if (m_main_char.m_velocity.x >= 80)
							{
								m_main_char.m_velocity.x = 80;
							}
							else if (m_main_char.m_velocity.x >= 0)
							{
								m_main_char.m_velocity.x += 2;
							}
							else
							{
								m_main_char.m_velocity.x += 10;
							}
						}
						else if (m_right == 2)
						{
							m_main_char.m_is_move = true;
							if (m_main_char.m_velocity.x >= 164)
							{
								m_main_char.m_velocity.x -= 164;
							}
							else if (m_main_char.m_velocity.x >= 160)
							{
								m_main_char.m_velocity.x = 160;
							}
							else if (m_main_char.m_velocity.x >= 0)
							{
								if (m_main_char.m_velocity.x > 80)
								{
									m_main_char.m_velocity.x++;
								}
								else
								{
									m_main_char.m_velocity.x += 3;
								}
							}
							else
							{
								m_main_char.m_velocity.x += 12;
							}
						}
					}
				}
				else
				{
					if (m_main_char.m_no_mc_time <= 0)
					{
						if (m_left == 1)
						{
							m_main_char.m_is_move = true;
							if (m_shache_type == 0 && m_shache_time > 0)
							{
								m_shache_time--;
								if (m_main_char.m_velocity.x - num * 3 / 2 < 0)
								{
									m_main_char.m_velocity.x = 0;
								}
								else
								{
									m_main_char.m_velocity.x -= num * 3 / 2;
									m_shache_time = 10;
								}
							}
							else if (m_main_char.m_velocity.x < -80 - num / 2)
							{
								m_main_char.m_velocity.x += num / 2;
							}
							else if (m_main_char.m_velocity.x <= -80)
							{
								m_main_char.m_velocity.x = -80;
							}
							else if (m_main_char.m_velocity.x <= 0)
							{
								m_main_char.m_velocity.x -= 3;
							}
							else if (m_main_char.m_velocity.x >= 100)
							{
								m_shache_type = 0;
								m_shache_time = 5;
							}
							else if (m_main_char.m_velocity.x - num * 2 < 0)
							{
								m_main_char.m_velocity.x = 0;
							}
							else
							{
								m_main_char.m_velocity.x -= num * 2;
							}
						}
						else if (m_left == 2)
						{
							m_main_char.m_is_move = true;
							if (m_shache_type == 0 && m_shache_time > 0)
							{
								m_shache_time--;
								if (m_main_char.m_velocity.x - num * 3 / 2 < 0)
								{
									m_main_char.m_velocity.x = 0;
								}
								else
								{
									m_main_char.m_velocity.x -= num * 3 / 2;
									m_shache_time = 10;
								}
							}
							else if (m_main_char.m_velocity.x < -160 - num / 2)
							{
								m_main_char.m_velocity.x += num / 2;
							}
							else if (m_main_char.m_velocity.x <= -160)
							{
								m_main_char.m_velocity.x = -160;
							}
							else if (m_main_char.m_velocity.x <= 0)
							{
								if (m_main_char.m_velocity.x < -80)
								{
									m_main_char.m_velocity.x--;
								}
								else
								{
									m_main_char.m_velocity.x -= 3;
								}
							}
							else if (m_main_char.m_velocity.x >= 100)
							{
								m_shache_type = 0;
								m_shache_time = 5;
							}
							else if (m_main_char.m_velocity.x - num * 3 < 0)
							{
								m_main_char.m_velocity.x = 0;
							}
							else
							{
								m_main_char.m_velocity.x -= num * 3;
							}
						}
						else if (m_right == 1)
						{
							m_main_char.m_is_move = true;
							if (m_shache_type == 1 && m_shache_time > 0)
							{
								m_shache_time--;
								if (m_main_char.m_velocity.x + num * 3 / 2 > 0)
								{
									m_main_char.m_velocity.x = 0;
								}
								else
								{
									m_main_char.m_velocity.x += num * 3 / 2;
									m_shache_time = 10;
								}
							}
							else if (m_main_char.m_velocity.x > 80 + num / 2)
							{
								m_main_char.m_velocity.x -= num / 2;
							}
							else if (m_main_char.m_velocity.x >= 80)
							{
								m_main_char.m_velocity.x = 80;
							}
							else if (m_main_char.m_velocity.x >= 0)
							{
								m_main_char.m_velocity.x += 3;
							}
							else if (m_main_char.m_velocity.x <= -100)
							{
								m_shache_type = 1;
								m_shache_time = 5;
							}
							else if (m_main_char.m_velocity.x + num * 2 > 0)
							{
								m_main_char.m_velocity.x = 0;
							}
							else
							{
								m_main_char.m_velocity.x += num * 2;
							}
						}
						else if (m_right == 2)
						{
							m_main_char.m_is_move = true;
							if (m_shache_type == 1 && m_shache_time > 0)
							{
								m_shache_time--;
								if (m_main_char.m_velocity.x + num * 3 / 2 > 0)
								{
									m_main_char.m_velocity.x = 0;
								}
								else
								{
									m_main_char.m_velocity.x += num * 3 / 2;
									m_shache_time = 10;
								}
							}
							else if (m_main_char.m_velocity.x > 160 + num / 2)
							{
								m_main_char.m_velocity.x -= num / 2;
							}
							else if (m_main_char.m_velocity.x >= 160)
							{
								m_main_char.m_velocity.x = 160;
							}
							else if (m_main_char.m_velocity.x >= 0)
							{
								if (m_main_char.m_velocity.x > 80)
								{
									m_main_char.m_velocity.x++;
								}
								else
								{
									m_main_char.m_velocity.x += 3;
								}
							}
							else if (m_main_char.m_velocity.x <= -100)
							{
								m_shache_type = 1;
								m_shache_time = 5;
							}
							else if (m_main_char.m_velocity.x + num * 3 > 0)
							{
								m_main_char.m_velocity.x = 0;
							}
							else
							{
								m_main_char.m_velocity.x += num * 3;
							}
						}
						else if (m_main_char.m_velocity.x < 0)
						{
							if (m_main_char.m_velocity.x >= -15)
							{
								m_main_char.m_velocity.x++;
							}
							else
							{
								m_main_char.m_velocity.x += num / 2;
							}
						}
						else if (m_main_char.m_velocity.x > 0)
						{
							if (m_main_char.m_velocity.x <= 15)
							{
								m_main_char.m_velocity.x--;
							}
							else
							{
								m_main_char.m_velocity.x -= num / 2;
							}
						}
					}
					if (m_main_char.m_velocity.x == 0 && m_shache_time == 0)
					{
						m_main_char.m_is_move = false;
					}
				}
				if (m_main_char.m_is_on_floor && !m_main_char.m_per_is_on_floor)
				{
					if (m_right == 0 && m_main_char.m_velocity.x < 0)
					{
						m_main_char.set_fx(mario_obj.mario_fx.mf_left);
					}
					else if (m_left == 0 && m_main_char.m_velocity.x > 0)
					{
						m_main_char.set_fx(mario_obj.mario_fx.mf_right);
					}
				}
				continue;
			}
			if (!mario_obj2.m_per_is_on_char && mario_obj2.m_is_on_char && !mario_obj2.m_wgk)
			{
				mario_obj2.m_velocity.set(0, 0);
			}
			if ((mario_obj2.m_is_on_char && !mario_obj2.m_wgk) || mario_obj2.m_no_mc_time > 0)
			{
				continue;
			}
			if (mario_obj2.m_fx == mario_obj.mario_fx.mf_right)
			{
				if (mario_obj2.m_velocity.x > mario_obj2.m_min_speed + num)
				{
					mario_obj2.m_velocity.x -= num;
				}
				else if (mario_obj2.m_velocity.x < mario_obj2.m_min_speed - num)
				{
					mario_obj2.m_velocity.x += num;
				}
				else
				{
					mario_obj2.m_velocity.x = mario_obj2.m_min_speed;
				}
			}
			else if (mario_obj2.m_velocity.x < -mario_obj2.m_min_speed - num)
			{
				mario_obj2.m_velocity.x += num;
			}
			else if (mario_obj2.m_velocity.x > -mario_obj2.m_min_speed + num)
			{
				mario_obj2.m_velocity.x -= num;
			}
			else
			{
				mario_obj2.m_velocity.x = -mario_obj2.m_min_speed;
			}
		}
	}

	private void check_paqiang()
	{
		m_paqiang = 0;
		if (m_main_char.m_is_on_floor || m_main_char.m_velocity.y >= 0)
		{
			return;
		}
		if (m_left > 0)
		{
			bobjs.Clear();
			m_qt.retrive_left(m_main_char, ref bobjs);
			for (int i = 0; i < bobjs.Count; i++)
			{
				mario_obj mario_obj2 = bobjs[i];
				if (m_main_char == mario_obj2 || mario_obj2.is_die() || mario_obj2.m_main)
				{
					continue;
				}
				if (mario_obj2.m_unit != null)
				{
					mario_obj mario_obj3 = get_obj(mario_obj2.m_init_pos.x + 1, mario_obj2.m_init_pos.y - 1);
					if (mario_obj3 != null && mario_obj3.m_type == mario_obj.mario_type.mt_block)
					{
						continue;
					}
					mario_obj3 = get_obj(mario_obj2.m_init_pos.x + 1, mario_obj2.m_init_pos.y - 2);
					if (mario_obj3 != null && mario_obj3.m_type == mario_obj.mario_type.mt_block)
					{
						continue;
					}
				}
				if (!m_main_char.check_left_floor(mario_obj2))
				{
					continue;
				}
				m_paqiang = 1;
				break;
			}
		}
		else
		{
			if (m_right <= 0)
			{
				return;
			}
			bobjs.Clear();
			m_qt.retrive_right(m_main_char, ref bobjs);
			for (int j = 0; j < bobjs.Count; j++)
			{
				mario_obj mario_obj4 = bobjs[j];
				if (m_main_char == mario_obj4 || mario_obj4.is_die() || mario_obj4.m_main)
				{
					continue;
				}
				if (mario_obj4.m_unit != null)
				{
					mario_obj mario_obj5 = get_obj(mario_obj4.m_init_pos.x - 1, mario_obj4.m_init_pos.y - 1);
					if (mario_obj5 != null && mario_obj5.m_type == mario_obj.mario_type.mt_block)
					{
						continue;
					}
					mario_obj5 = get_obj(mario_obj4.m_init_pos.x - 1, mario_obj4.m_init_pos.y - 2);
					if (mario_obj5 != null && mario_obj5.m_type == mario_obj.mario_type.mt_block)
					{
						continue;
					}
				}
				if (!m_main_char.check_right_floor(mario_obj4))
				{
					continue;
				}
				m_paqiang = 2;
				break;
			}
		}
	}

	private void check_state()
	{
		m_caisi = false;
		for (int i = 0; i < m_aobjs[3].Count; i++)
		{
			mario_obj mario_obj2 = m_aobjs[3][i];
			mario_obj2.m_velocity.x += mario_obj2.m_pvelocity.x;
			mario_obj2.m_velocity.y += mario_obj2.m_pvelocity.y;
			mario_obj2.m_pvelocity = new mario_point();
		}
		check_zhli();
		check_paqiang();
		for (int j = 0; j < m_aobjs[3].Count; j++)
		{
			mario_obj mario_obj3 = m_aobjs[3][j];
			if (mario_obj3.m_is_on_floor || mario_obj3.is_die() || !mario_obj3.m_is_start)
			{
				continue;
			}
			if (mario_obj3.m_main && m_paqiang > 0)
			{
				if (mario_obj3.m_velocity.y > -70)
				{
					mario_obj3.m_velocity.y -= 10;
				}
				else if (mario_obj3.m_velocity.y < -90)
				{
					mario_obj3.m_velocity.y += 10;
				}
				else
				{
					mario_obj3.m_velocity.y = -80;
				}
			}
			else
			{
				mario_obj3.m_velocity.y -= 15;
				if (mario_obj3.m_velocity.y < -160)
				{
					mario_obj3.m_velocity.y = -160;
				}
			}
		}
		for (int k = 0; k < m_aobjs[1].Count; k++)
		{
			mario_obj mario_obj4 = m_aobjs[1][k];
			mario_obj4.do_move();
		}
		for (int l = 0; l < m_aobjs[2].Count; l++)
		{
			mario_obj mario_obj5 = m_aobjs[2][l];
			mario_obj5.do_move();
		}
		for (int m = 0; m < m_aobjs[3].Count; m++)
		{
			mario_obj mario_obj6 = m_aobjs[3][m];
			mario_obj6.m_nl_point = new mario_point();
			if (mario_obj6.m_bnl_objs.Count > 0)
			{
				int num = 0;
				int num2 = 0;
				int num3 = -999999;
				for (int n = 0; n < mario_obj6.m_bnl_objs.Count; n++)
				{
					mario_point nl_calc_point = mario_obj6.m_bnl_objs[n].get_nl_calc_point();
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
				if (mario_obj6.m_no_mc_time > 0)
				{
					num4 = 0;
				}
				mario_obj6.m_nl_point.set(num4, num3);
				if (!mario_obj6.m_per_is_on_floor && (mario_obj6.m_main || !mario_obj6.m_is_on_char))
				{
					mario_obj6.m_velocity.x -= num4;
				}
			}
			mario_obj6.m_per_nl_point.set(mario_obj6.m_nl_point.x, mario_obj6.m_nl_point.y);
			if (mario_obj6.m_per_nl_point.y < 0)
			{
				mario_obj6.m_per_nl_point.y = 0;
			}
		}
		for (int num5 = 0; num5 < m_mobjs.Count; num5++)
		{
			mario_obj mario_obj7 = m_mobjs[num5];
			if (mario_obj7.is_die() || mario_obj7.m_is_start)
			{
				int num6 = mario_obj7.m_velocity.x + mario_obj7.m_nl_point.x;
				int num7 = mario_obj7.m_velocity.y + mario_obj7.m_nl_point.y;
				if (num6 != 0 || num7 != 0)
				{
					mario_obj7.m_pos.x += num6;
					mario_obj7.m_pos.y += num7;
				}
				refresh_obj(mario_obj7);
			}
		}
		for (int num8 = 0; num8 < m_aobjs[3].Count; num8++)
		{
			mario_obj mario_obj8 = m_aobjs[3][num8];
			if (!mario_obj8.m_main && !mario_obj8.m_wgk)
			{
				check_hit(mario_obj8, mario_obj.mario_type.mt_charater);
				check_hit(mario_obj8, mario_obj.mario_type.mt_block1);
				check_hit(mario_obj8, mario_obj.mario_type.mt_block);
				check_hit(mario_obj8, mario_obj.mario_type.mt_attack);
			}
		}
		for (int num9 = 0; num9 < m_aobjs[3].Count; num9++)
		{
			mario_obj mario_obj9 = m_aobjs[3][num9];
			if (mario_obj9.m_wgk)
			{
				check_hit(mario_obj9, mario_obj.mario_type.mt_charater);
				check_hit(mario_obj9, mario_obj.mario_type.mt_block1);
				check_hit(mario_obj9, mario_obj.mario_type.mt_block);
				check_hit(mario_obj9, mario_obj.mario_type.mt_attack);
			}
		}
		for (int num10 = 0; num10 < m_aobjs[3].Count; num10++)
		{
			mario_obj mario_obj10 = m_aobjs[3][num10];
			if (mario_obj10.m_wgk)
			{
				check_hit(mario_obj10, mario_obj.mario_type.mt_charater, all: false, wgk: true);
			}
		}
		if (m_main_char != null)
		{
			check_hit(m_main_char, mario_obj.mario_type.mt_charater);
			check_hit(m_main_char, mario_obj.mario_type.mt_block1);
			check_hit(m_main_char, mario_obj.mario_type.mt_block);
			check_hit(m_main_char, mario_obj.mario_type.mt_attack);
			check_hit(m_main_char, mario_obj.mario_type.mt_attack_ex);
			check_hit(m_main_char, mario_obj.mario_type.mt_attack_ex1);
		}
		for (int num11 = 0; num11 < m_aobjs[6].Count; num11++)
		{
			mario_obj mo = m_aobjs[6][num11];
			check_hit(mo, mario_obj.mario_type.mt_block, all: false);
		}
		int num12 = m_main_char.m_velocity.x + m_main_char.m_nl_point.x;
		if (num12 < 0 && m_main_char.m_pos.x + num12 < utils.g_grid_size / 2)
		{
			m_main_char.m_velocity.x = 0;
			num12 = 0;
			m_main_char.m_pos.x = utils.g_grid_size / 2;
		}
		else if (num12 > 0 && m_main_char.m_pos.x + num12 >= game_data._instance.m_map_data.maps[m_world].x_num * utils.g_grid_size - utils.g_grid_size / 2)
		{
			m_main_char.m_velocity.x = 0;
			num12 = 0;
			m_main_char.m_pos.x = game_data._instance.m_map_data.maps[m_world].x_num * utils.g_grid_size - utils.g_grid_size / 2 - 1;
		}
		if (m_main_char.m_velocity.y != 0 && m_jump_state == 0)
		{
			m_jump_state = 1;
			m_jump_num = 0;
			m_jump_num_time = 0;
		}
		if (m_jump_num_time > 0 && m_jump_state == 0)
		{
			m_jump_num_time--;
			if (m_jump_num_time <= 0)
			{
				m_jump_num = 0;
			}
		}
	}

	private void check_hit_make_list_wgk(mario_obj mo1, bool wgk)
	{
		for (int i = 0; i < bobjs.Count; i++)
		{
			mario_obj mario_obj2 = bobjs[i];
			if (!(mo1 == mario_obj2) && wgk == mario_obj2.m_wgk && mo1.hit(mario_obj2))
			{
				tobjs.Add(bobjs[i]);
			}
		}
	}

	private void check_hit_make_list_normal(mario_obj mo1)
	{
		for (int i = 0; i < bobjs.Count; i++)
		{
			mario_obj mario_obj2 = bobjs[i];
			if (!(mo1 == mario_obj2) && mo1.hit(mario_obj2))
			{
				tobjs.Add(mario_obj2);
			}
		}
	}

	private void check_hit_make_list(mario_obj mo1, mario_obj.mario_type type, bool wgk)
	{
		bobjs.Clear();
		tobjs.Clear();
		if (m_aobjs[(int)type].Count <= 5)
		{
			bobjs.AddRange(m_aobjs[(int)type]);
		}
		else
		{
			mo1.m_qtree.retrive(mo1, ref bobjs, (int)type);
		}
		if (mo1.m_wgk)
		{
			check_hit_make_list_wgk(mo1, wgk);
		}
		else
		{
			check_hit_make_list_normal(mo1);
		}
	}

	private void check_hit(mario_obj mo1, mario_obj.mario_type type, bool all = true, bool wgk = false)
	{
		if (mo1.is_die() || !mo1.m_is_start)
		{
			return;
		}
		check_hit_make_list(mo1, type, wgk);
		if (all)
		{
			if (type != mario_obj.mario_type.mt_charater)
			{
				check_left(mo1);
				check_right(mo1);
				check_top(mo1);
				check_bottom(mo1, type);
			}
			else
			{
				check_top(mo1);
				check_bottom(mo1, type);
				check_left(mo1);
				check_right(mo1);
			}
			if (mo1.m_main)
			{
				check_left_top(mo1);
				check_right_top(mo1);
				check_left_bottom(mo1);
				check_right_bottom(mo1);
			}
		}
		for (int i = 0; i < tobjs.Count; i++)
		{
			mario_obj mario_obj2 = tobjs[i];
			if (!mario_obj2.is_die() && mo1.hit(mario_obj2))
			{
				mario_obj2.be_hit(mo1);
			}
		}
	}

	private void check_left(mario_obj mo1)
	{
		bool flag = false;
		int num = 0;
		for (int i = 0; i < tobjs.Count; i++)
		{
			mario_obj mario_obj2 = tobjs[i];
			if (mario_obj2.m_nright && mario_obj2.m_unit != null && mario_obj2.m_unit.is_static == 1)
			{
				mario_obj mario_obj3 = get_obj(mario_obj2.m_init_pos.x + 1, mario_obj2.m_init_pos.y);
				if ((bool)mario_obj3 && !mario_obj3.is_die() && mario_obj3.m_is_static)
				{
					continue;
				}
			}
			if (mario_obj2.is_die() || !mo1.left_hit(mario_obj2))
			{
				continue;
			}
			int px = 0;
			if (mario_obj2.be_left_hit(mo1, ref px))
			{
				if (!flag)
				{
					num = px;
					flag = true;
				}
				else if (px > num)
				{
					num = px;
				}
			}
		}
		if (flag)
		{
			mo1.m_pos.x = num;
			refresh_obj(mo1);
		}
	}

	private void check_right(mario_obj mo1)
	{
		bool flag = false;
		int num = 0;
		for (int i = 0; i < tobjs.Count; i++)
		{
			mario_obj mario_obj2 = tobjs[i];
			if (mario_obj2.m_nleft && mario_obj2.m_unit != null && mario_obj2.m_unit.is_static == 1)
			{
				mario_obj mario_obj3 = get_obj(mario_obj2.m_init_pos.x - 1, mario_obj2.m_init_pos.y);
				if ((bool)mario_obj3 && !mario_obj3.is_die() && mario_obj3.m_is_static)
				{
					continue;
				}
			}
			if (mario_obj2.is_die() || !mo1.right_hit(mario_obj2))
			{
				continue;
			}
			int px = 0;
			if (mario_obj2.be_right_hit(mo1, ref px))
			{
				if (!flag)
				{
					num = px;
					flag = true;
				}
				else if (px < num)
				{
					num = px;
				}
			}
		}
		if (flag)
		{
			mo1.m_pos.x = num;
			refresh_obj(mo1);
		}
	}

	private void check_top(mario_obj mo1)
	{
		bool flag = false;
		int num = 0;
		for (int i = 0; i < tobjs.Count; i++)
		{
			mario_obj mario_obj2 = tobjs[i];
			if (mario_obj2.is_die() || !mo1.top_hit(mario_obj2))
			{
				continue;
			}
			int py = 0;
			if (mario_obj2.be_top_hit(mo1, ref py))
			{
				if (!flag)
				{
					num = py;
					flag = true;
				}
				else if (py < num)
				{
					num = py;
				}
			}
		}
		if (flag)
		{
			mo1.m_pos.y = num;
			refresh_obj(mo1);
		}
	}

	private void check_bottom(mario_obj mo1, mario_obj.mario_type type)
	{
		bool flag = false;
		int num = 0;
		for (int i = 0; i < tobjs.Count; i++)
		{
			mario_obj mario_obj2 = tobjs[i];
			if (mario_obj2.is_die() || !mo1.bottom_hit(mario_obj2))
			{
				continue;
			}
			int py = 0;
			if (mario_obj2.be_bottom_hit(mo1, ref py))
			{
				if (!flag)
				{
					num = py;
					flag = true;
				}
				else if (py > num)
				{
					num = py;
				}
			}
		}
		if (flag)
		{
			mo1.m_pos.y = num;
			refresh_obj(mo1);
		}
		for (int j = 0; j < mo1.m_bnl_objs.Count; j++)
		{
			mario_obj mario_obj3 = mo1.m_bnl_objs[j];
			if (!mario_obj3.is_die() && type == mario_obj3.m_type)
			{
				int py2 = 0;
				mario_obj3.be_bottom_hit(mo1, ref py2);
			}
		}
	}

	private void check_left_top(mario_obj mo1)
	{
		bool flag = false;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < tobjs.Count; i++)
		{
			mario_obj mario_obj2 = tobjs[i];
			if (mario_obj2.is_die() || !mo1.left_top_hit(mario_obj2))
			{
				continue;
			}
			int px = 0;
			int py = 0;
			if (!mario_obj2.be_left_top_hit(mo1, ref px, ref py))
			{
				continue;
			}
			if (!flag)
			{
				num = px;
				num2 = py;
				flag = true;
				continue;
			}
			if (px > num)
			{
				num = px;
			}
			if (py < num2)
			{
				num2 = py;
			}
		}
		if (flag)
		{
			mo1.m_pos.x = num;
			mo1.m_pos.y = num2;
			refresh_obj(mo1);
		}
	}

	private void check_right_top(mario_obj mo1)
	{
		bool flag = false;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < tobjs.Count; i++)
		{
			mario_obj mario_obj2 = tobjs[i];
			if (mario_obj2.is_die() || !mo1.right_top_hit(mario_obj2))
			{
				continue;
			}
			int px = 0;
			int py = 0;
			if (!mario_obj2.be_right_top_hit(mo1, ref px, ref py))
			{
				continue;
			}
			if (!flag)
			{
				num = px;
				num2 = py;
				flag = true;
				continue;
			}
			if (px < num)
			{
				num = px;
			}
			if (py < num2)
			{
				num2 = py;
			}
		}
		if (flag)
		{
			mo1.m_pos.x = num;
			mo1.m_pos.y = num2;
			refresh_obj(mo1);
		}
	}

	private void check_left_bottom(mario_obj mo1)
	{
		bool flag = false;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < tobjs.Count; i++)
		{
			mario_obj mario_obj2 = tobjs[i];
			if (mario_obj2.is_die() || !mo1.left_bottom_hit(mario_obj2))
			{
				continue;
			}
			int px = 0;
			int py = 0;
			if (!mario_obj2.be_left_bottom_hit(mo1, ref px, ref py))
			{
				continue;
			}
			if (!flag)
			{
				num = px;
				num2 = py;
				flag = true;
				continue;
			}
			if (px > num)
			{
				num = px;
			}
			if (py > num2)
			{
				num2 = py;
			}
		}
		if (flag)
		{
			mo1.m_pos.x = num;
			mo1.m_pos.y = num2;
			refresh_obj(mo1);
		}
	}

	private void check_right_bottom(mario_obj mo1)
	{
		bool flag = false;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < tobjs.Count; i++)
		{
			mario_obj mario_obj2 = tobjs[i];
			if (mario_obj2.is_die() || !mo1.right_bottom_hit(mario_obj2))
			{
				continue;
			}
			int px = 0;
			int py = 0;
			if (!mario_obj2.be_right_bottom_hit(mo1, ref px, ref py))
			{
				continue;
			}
			if (!flag)
			{
				num = px;
				num2 = py;
				flag = true;
				continue;
			}
			if (px < num)
			{
				num = px;
			}
			if (py > num2)
			{
				num2 = py;
			}
		}
		if (flag)
		{
			mo1.m_pos.x = num;
			mo1.m_pos.y = num2;
			refresh_obj(mo1);
		}
	}

	private void check_create()
	{
		if (m_main_char == null)
		{
			return;
		}
		int num = m_main_char.m_pos.x - m_roll.x;
		if (num < 2 && num > -2)
		{
			m_roll.x = m_main_char.m_pos.x;
		}
		else if (num < 5 && num > 0)
		{
			m_roll.x++;
		}
		else if (num > -5 && num < 0)
		{
			m_roll.x--;
		}
		else
		{
			m_roll.x += num / 5;
		}
		if (m_main_char.m_is_on_floor || m_paqiang_jump)
		{
			m_y_roll_target = m_main_char.m_pos.y - utils.g_roll_y;
		}
		num = m_main_char.m_pos.y - utils.g_roll_y - m_roll.y;
		if (num < 0 && num > -40)
		{
			m_roll.y = m_main_char.m_pos.y - utils.g_roll_y;
		}
		else if (num > -100 && num < 0)
		{
			m_roll.y -= 10;
		}
		else if (num < 0)
		{
			m_roll.y += num / 10;
		}
		else
		{
			num = m_y_roll_target - m_roll.y;
			if (num > 0 && num < 40)
			{
				m_roll.y = m_y_roll_target;
			}
			else if (num < 250 && num > 0)
			{
				m_roll.y += 10;
			}
			else if (num > 0)
			{
				m_roll.y += num / 25;
			}
		}
		if (m_roll.y + utils.g_height > game_data._instance.m_map_data.maps[m_world].y_num * utils.g_grid_size)
		{
			m_roll.y = game_data._instance.m_map_data.maps[m_world].y_num * utils.g_grid_size - utils.g_height;
		}
		if (m_roll.y < 0)
		{
			m_roll.y = 0;
		}
		HashSet<int> hashSet = new HashSet<int>();
		int num2 = m_roll.x / utils.g_grid_size;
		if (num2 != m_grid.x)
		{
			int num3 = num2 + utils.g_start_x;
			if (m_grid.x > num2)
			{
				num3 = num2 - utils.g_start_x;
			}
			if (num3 >= 0 && num3 < game_data._instance.m_map_data.maps[m_world].x_num)
			{
				for (int i = m_grid.y - utils.g_start_y; i <= m_grid.y + utils.g_start_y; i++)
				{
					if (i >= 0 && i < game_data._instance.m_map_data.maps[m_world].y_num)
					{
						int item = i * game_data._instance.m_map_data.maps[m_world].x_num + num3;
						hashSet.Add(item);
						do_create(num3, i);
					}
				}
			}
			m_grid.x = num2;
		}
		int num4 = (m_roll.y + utils.g_roll_y) / utils.g_grid_size;
		if (num4 != m_grid.y)
		{
			int num5 = num4 + utils.g_start_y;
			if (m_grid.y > num4)
			{
				num5 = num4 - utils.g_start_y;
			}
			if (num5 >= 0 && num5 < game_data._instance.m_map_data.maps[m_world].y_num)
			{
				for (int j = m_grid.x - utils.g_start_x; j <= m_grid.x + utils.g_start_x; j++)
				{
					if (j >= 0 && j < game_data._instance.m_map_data.maps[m_world].x_num)
					{
						int item2 = num5 * game_data._instance.m_map_data.maps[m_world].x_num + j;
						if (!hashSet.Contains(item2))
						{
							hashSet.Add(item2);
							do_create(j, num5);
						}
					}
				}
			}
			m_grid.y = num4;
		}
		m_need_calc.Clear();
		for (int k = 0; k < m_aobjs[3].Count; k++)
		{
			mario_obj mario_obj2 = m_aobjs[3][k];
			int x = mario_obj2.m_grid.x;
			int y = mario_obj2.m_grid.y;
			for (int l = y - 1; l <= y + 1; l++)
			{
				if (l < 0 || l >= game_data._instance.m_map_data.maps[m_world].y_num)
				{
					continue;
				}
				for (int m = x - 1; m <= x + 1; m++)
				{
					if (m >= 0 && m < game_data._instance.m_map_data.maps[m_world].x_num && mario_obj2.m_is_start)
					{
						int item3 = l * game_data._instance.m_map_data.maps[m_world].x_num + m;
						m_need_calc.Add(item3);
						if (!hashSet.Contains(item3))
						{
							hashSet.Add(item3);
							do_create(m, l);
						}
					}
				}
			}
		}
	}

	private void do_create(int i, int j)
	{
		int num = j * game_data._instance.m_map_data.maps[m_world].x_num + i;
		if (m_delete_objs[m_world].ContainsKey(num))
		{
			List<mario_obj> list = m_delete_objs[m_world][num];
			for (int k = 0; k < list.Count; k++)
			{
				mario_obj mario_obj2 = list[k];
				add_obj(mario_obj2);
				mario_obj2.gameObject.SetActive(value: true);
				if (mario_obj2.m_shadow != null)
				{
					mario_obj2.m_shadow.SetActive(value: true);
				}
				mario_obj2.m_is_destory = 0;
				mario_obj2.m_is_new = true;
				mario_obj2.check_is_start(m_grid);
			}
			m_delete_objs[m_world].Remove(num);
		}
		s_t_unit s_t_unit2 = game_data._instance.get_t_unit(game_data._instance.m_arrays[m_world][j][i].type);
		if (s_t_unit2 != null && !m_has_create[m_world].Contains(num))
		{
			create_mario_obj(s_t_unit2.res, s_t_unit2, game_data._instance.m_arrays[m_world][j][i].param, i, j);
			m_has_create[m_world].Add(num);
		}
	}

	public bool need_calc(int i, int j)
	{
		int item = j * game_data._instance.m_map_data.maps[m_world].x_num + i;
		return m_need_calc.Contains(item);
	}

	private void check_delete()
	{
		List<mario_obj> list = new List<mario_obj>();
		for (int i = 0; i < m_mobjs.Count; i++)
		{
			if (m_mobjs[i].m_is_destory > 0)
			{
				list.Add(m_mobjs[i]);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j] == m_main_char)
			{
				m_main_char = null;
			}
			if (list[j].m_is_destory == 1 || list[j].m_unit == null || !list[j].m_bkcf)
			{
				if (list[j].m_unit != null && !list[j].m_bkcf)
				{
					m_has_create[m_world].Remove(list[j].m_init_pos.y * game_data._instance.m_map_data.maps[m_world].x_num + list[j].m_init_pos.x);
				}
				remove_obj(list[j]);
				list[j].destroy_shadow();
				Object.Destroy(list[j].gameObject);
				continue;
			}
			remove_obj(list[j]);
			list[j].check_state(m_grid);
			list[j].gameObject.SetActive(value: false);
			if (list[j].m_shadow != null)
			{
				list[j].m_shadow.SetActive(value: false);
			}
			int key = list[j].m_grid.y * game_data._instance.m_map_data.maps[m_world].x_num + list[j].m_grid.x;
			if (!m_delete_objs[m_world].ContainsKey(key))
			{
				m_delete_objs[m_world].Add(key, new List<mario_obj>());
			}
			m_delete_objs[m_world][key].Add(list[j]);
		}
	}

	private void update_ex()
	{
        AntiCheatManager.RunAntiCheatDetect();
        for (int i = 0; i < m_mobjs.Count; i++)
		{
			m_mobjs[i].update_ex(m_grid);
		}
		m_scene.update_ex(m_roll);
		if (game_data._instance.m_map_data.mode != 0)
		{
			if (game_data._instance.m_map_data.mode == 1)
			{
				m_zhuiji_x += 20;
			}
			else if (game_data._instance.m_map_data.mode == 2)
			{
				m_zhuiji_x += 40;
			}
			m_zhuiji.transform.localPosition = new Vector3(m_zhuiji_x / 10, (m_roll.y + utils.g_roll_y) / 10);
			if (m_zhuiji_x >= m_main_char.m_pos.x)
			{
				m_main_char.m_is_die = true;
			}
		}
	}

	private void FixedUpdate()
	{
		if (m_pause)
		{
			return;
		}
		if (mario._instance.m_game_state == e_game_state.egs_edit && mario._instance.m_self.guide == 0 && m_main_char != null && m_main_char.m_pos.x > 22400)
		{
			s_message s_message2 = new s_message();
			s_message2.m_type = "jx_1";
			mario._instance.show_xsjx_dialog_box(game_data._instance.get_language_string("play_mode_zkql"), s_message2);
			m_pause = true;
			return;
		}
		if (m_state == 0 && m_main_char != null && !m_main_char.is_die() && m_main_char.m_bl[1] == 0)
		{
			m_time++;
			m_total_time = m_time;
			if (m_left_time > 0)
			{
				m_left_time--;
			}
			if (m_right_time > 0)
			{
				m_right_time--;
			}
			check_input();
			m_csm = null;
			if (m_pause)
			{
				return;
			}
			if (mario._instance.m_game_state != e_game_state.egs_edit)
			{
				if (m_time / 50 >= game_data._instance.m_map_data.time)
				{
					m_main_char.m_is_die = true;
					s_message s_message3 = new s_message();
					s_message3.m_type = "time_up";
					cmessage_center._instance.add_message(s_message3);
				}
				if (!m_has_time_tx && m_time / 50 >= game_data._instance.m_map_data.time - 100)
				{
					m_has_time_tx = true;
					mario._instance.pause_mus();
					mario._instance.play_sound_ex1("sound/hurry");
				}
				if (m_has_time_tx)
				{
					m_time_tx_time++;
					if (m_time_tx_time == 150)
					{
						mario._instance.continue_mus(1.5f);
					}
				}
			}
		}
		if ((bool)m_main_char)
		{
			if (!m_main_char.is_die() && m_state == 0 && m_main_char.m_bl[1] == 0)
			{
				check_state();
				update_ex();
				check_delete();
				check_create();
			}
			else if (m_main_char.m_bl[1] != 0)
			{
				m_main_char.update_ex(m_grid);
				check_delete();
			}
			else if (m_main_char.is_die())
			{
				m_main_char.m_pos.x += m_main_char.m_velocity.x;
				m_main_char.m_pos.y += m_main_char.m_velocity.y;
				m_main_char.update_ex(m_grid);
				check_delete();
			}
			else
			{
				m_main_char.update_ex(m_grid);
				check_delete();
			}
		}
		else if (m_state == 0)
		{
			m_state = 101;
		}
		if (m_render.transform.localScale.x < 1f)
		{
			float num = m_render.transform.localScale.x + 0.01f;
			m_render.transform.localScale = new Vector3(num, num, 1f);
		}
		else if (m_render.transform.localScale.x > 1f)
		{
			m_render.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		if (m_state == 100)
		{
			if (mario._instance.m_game_state != e_game_state.egs_edit)
			{
				m_state = 150;
				m_time_tx_time = 0;
				mario._instance.play_mus_ex1("music/win", loop: false);
			}
			else
			{
				win();
				m_state = 999;
			}
		}
		else if (m_state == 150)
		{
			m_time_tx_time++;
			if (game_data._instance.m_map_data.time > m_time / 50)
			{
				m_time += 50;
				add_score(50);
				if (game_data._instance.m_map_data.time > 990)
				{
					int num = Math.Abs(990 - game_data._instance.m_map_data.time);
					m_time += 50 * num;
					add_score(50 * num);
				}
				if (game_data._instance.m_map_data.time > m_time / 50)
				{
					m_time += 50;
					add_score(50);
				}
				mario._instance.play_sound("sound/js");
			}
			else if (m_time_tx_time >= 200)
			{
				win();
				m_state = 999;
			}
		}
		else if (m_state == 101)
		{
			lose();
			m_state = 999;
		}
		if (m_show_cha > 0)
		{
			show_cha();
		}
	}

	public void win()
	{
		if (mario._instance.m_game_state == e_game_state.egs_edit)
		{
			if (mario._instance.m_self.guide == 3)
			{
				mario._instance.m_self.guide = 4;
			}
			s_message s_message2 = new s_message();
			s_message2.m_type = "edit_mode";
			mario_point value = new mario_point(m_die_pos.x % 10000000, m_die_pos.y);
			s_message2.m_object.Add(value);
			s_message2.m_ints.Add(m_world);
			cmessage_center._instance.add_message(s_message2);
		}
		else
		{
			s_message s_message3 = new s_message();
			s_message3.m_type = "play_win";
			s_message3.m_object.Add(m_inputs);
			s_message3.m_ints.Add(m_score);
			s_message3.m_ints.Add(m_total_time);
			cmessage_center._instance.add_message(s_message3);
		}
	}

	public void lose()
	{
		if (mario._instance.m_game_state == e_game_state.egs_edit)
		{
			if (mario._instance.m_self.guide < 100)
			{
				s_message s_message2 = new s_message();
				s_message2.m_type = "edit_mode";
				cmessage_center._instance.add_message(s_message2);
				return;
			}
			s_message s_message3 = new s_message();
			s_message3.m_type = "edit_mode";
			mario_point value = new mario_point(m_die_pos.x % 10000000, m_die_pos.y);
			s_message3.m_object.Add(value);
			s_message3.m_ints.Add(m_world);
			cmessage_center._instance.add_message(s_message3);
		}
		else
		{
			s_message s_message4 = new s_message();
			s_message4.m_type = "play_lose";
			s_message4.m_object.Add(m_die_pos);
			cmessage_center._instance.add_message(s_message4);
			game_data._instance.m_die_poses[m_world].Add(new mario_point(m_die_pos.x, m_die_pos.y));
		}
	}

	public void start_cha()
	{
		if (m_show_cha == 0)
		{
			m_show_cha = 1;
		}
	}

	private void show_cha()
	{
		m_show_cha_time++;
		if (m_show_cha_time % 3 != 0 || mario._instance.m_game_state == e_game_state.egs_edit)
		{
			return;
		}
		mario_point mario_point2 = new mario_point(m_die_pos.x % 10000000, m_die_pos.y);
		if (m_show_cha > 1)
		{
			bool flag = true;
			while (flag)
			{
				if (m_show_cha > game_data._instance.m_die_poses[m_world].Count)
				{
					m_show_cha = 0;
					return;
				}
				mario_point2 = new mario_point(game_data._instance.m_die_poses[m_world][m_show_cha - 2].x % 10000000, game_data._instance.m_die_poses[m_world][m_show_cha - 2].y);
				if (mario_point2.y < 0)
				{
					mario_point2.y = 0;
				}
				int num = mario_point2.x / utils.g_grid_size - m_grid.x;
				int num2 = mario_point2.y / utils.g_grid_size - m_grid.y;
				flag = ((num <= -utils.g_active_x || num >= utils.g_active_x || num2 < -utils.g_active_y || num2 > utils.g_active_y) ? true : false);
				m_show_cha++;
			}
		}
		else
		{
			m_show_cha++;
		}
		if (m_show_cha == 2)
		{
			GameObject original = (GameObject)Resources.Load("unit/other/cha_big");
			GameObject gameObject = (GameObject)Object.Instantiate(original);
			gameObject.transform.parent = m_other.transform;
			gameObject.transform.localPosition = new Vector3(mario_point2.x / 10, mario_point2.y / 10);
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else
		{
			GameObject original2 = (GameObject)Resources.Load("unit/other/cha_small");
			GameObject gameObject2 = (GameObject)Object.Instantiate(original2);
			gameObject2.transform.parent = m_other.transform;
			gameObject2.transform.localPosition = new Vector3(mario_point2.x / 10, mario_point2.y / 10);
			gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	public void add_score(int score)
	{
		m_score += score;
	}

	public void add_score(int x, int y, int score)
	{
		m_score += score;
		s_message s_message2 = new s_message();
		s_message2.m_type = "add_score";
		s_message2.m_ints.Add(x / 10);
		s_message2.m_ints.Add(y / 10);
		s_message2.m_ints.Add(score);
		cmessage_center._instance.add_message(s_message2);
	}

	public void caisi(int y, bool tx = false, int xx = 0, int yy = 0)
	{
		m_caisi = true;
		m_caisi_y = y;
		m_main_char.m_is_on_floor = true;
		if (tx)
		{
			effect("hit", xx, yy);
		}
	}

	public void effect(string name, int xx, int yy)
	{
		GameObject original = (GameObject)Resources.Load("unit/other/" + name);
		GameObject gameObject = (GameObject)Object.Instantiate(original);
		gameObject.transform.parent = m_main.transform;
		gameObject.transform.localPosition = new Vector3(xx / 10, yy / 10);
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	public void ding()
	{
		m_jump_state = 1;
		m_sjump = true;
		m_jump = false;
	}

	public void show_chuan(mario_obj obj)
	{
		m_csm = obj;
	}

	public bool can_show_chuan()
	{
		return m_csm != null;
	}

	private void Update()
	{
		if (mario._instance.key_down(KeyCode.LeftArrow))
		{
			m_now_inputs[0] = true;
		}
		if (mario._instance.key_up(KeyCode.LeftArrow))
		{
			m_now_inputs[0] = false;
		}
		if (mario._instance.key_down(KeyCode.RightArrow))
		{
			m_now_inputs[1] = true;
		}
		if (mario._instance.key_up(KeyCode.RightArrow))
		{
			m_now_inputs[1] = false;
		}
		if (mario._instance.key_down(KeyCode.Z))
		{
			m_now_inputs[2] = true;
		}
		if (mario._instance.key_up(KeyCode.Z))
		{
			m_now_inputs[2] = false;
		}
		if (mario._instance.key_down(KeyCode.X))
		{
			m_now_inputs[3] = true;
		}
		if (mario._instance.key_up(KeyCode.X))
		{
			m_now_inputs[3] = false;
		}
		if (Application.isEditor && Input.GetKeyUp(KeyCode.E))
		{
			m_good_inputs = new List<int>();
			m_good_inputs.AddRange(m_inputs);
		}
		if (Application.isEditor && Input.GetKeyUp(KeyCode.R))
		{
			m_zr = true;
			for (int i = 0; i < m_inputs.Count / 2; i++)
			{
				if (m_inputs[i * 2] > m_time)
				{
					m_inputs.RemoveRange(i * 2, m_inputs.Count - i * 2);
				}
			}
		}
		m_cam.transform.localPosition = new Vector3(m_roll.x / 10, (m_roll.y + utils.g_roll_y) / 10);
	}
}
