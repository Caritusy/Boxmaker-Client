using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class edit_mode : MonoBehaviour
{
	private HashSet<int> m_has_create = new HashSet<int>();

	private List<mario_obj> m_objs = new List<mario_obj>();

	public mario_obj m_main_char;

	private mario_scene m_scene;

	private bool m_left;

	private bool m_right;

	private bool m_up;

	private bool m_down;

	private int m_time;

	public mario_point m_roll = new mario_point();

	private mario_point m_grid = new mario_point();

	public Camera m_cam;

	public GameObject m_fuzhu;

	public GameObject m_other;

	public GameObject m_render;

	public GameObject m_back;

	public GameObject m_main;

	public GameObject m_shadow;

	public mario_point m_qpos;

	private int m_add_qz_type;

	private int m_add_qz_x;

	private int m_add_qz_y;

	private mario_point m_pre_csg;

	private List<GameObject> m_hongs = new List<GameObject>();

	private GameObject m_lv;

	private GameObject m_ban;

	private GameObject m_ban1;

	private GameObject m_zhuiji;

	private int m_yf;

	private GameObject m_dlg;

	public int m_world;

	private bool m_first_csm;

	private int m_csm_id;

	private int m_csm_w;

	private int m_csm_x;

	private int m_csm_y;

	private int m_edit_cys_time;

	private int m_edit_cys_num;

	private int m_edit_cys_tnum;

	private List<edit_cy> m_ecs;

	public static edit_mode _instance;

	private void Awake()
	{
		_instance = this;
		Joystick.On_JoystickHolding += joy_hold;
		Joystick.On_JoystickMoveEnd += joy_move_end;
	}

	public void reload(mario_point qpos, int world, List<edit_cy> ecs)
	{
		m_edit_cys_time = 0;
		m_edit_cys_num = 0;
		m_edit_cys_tnum = ecs.Count;
		m_ecs = new List<edit_cy>();
		m_ecs.AddRange(ecs);
		for (int i = 0; i < m_ecs.Count; i++)
		{
			m_ecs[i].num = m_ecs.Count - 1 - i;
		}
		m_qpos = qpos;
		reset(world);
	}

	public void reload(int world)
	{
		m_qpos = new mario_point(m_main_char.m_pos.x, m_main_char.m_pos.y);
		reset(world);
	}

	public IEnumerator reload_fg(int world)
	{
		yield return new WaitForEndOfFrame();
		int w = (int)((float)mario._instance.m_width / 0.8f + 0.01f);
		int h = (int)((float)mario._instance.m_height / 0.8f + 0.01f);
		RenderTexture rd = new RenderTexture(w, h, 0);
		m_cam.targetTexture = rd;
		m_cam.Render();
		RenderTexture.active = rd;
		Texture2D tex = new Texture2D(w, h, TextureFormat.RGB24, mipmap: false);
		tex.ReadPixels(new Rect(0f, 0f, w, h), 0, 0);
		tex.Apply();
		yield return tex;
		m_cam.targetTexture = null;
		RenderTexture.active = null;
		UnityEngine.Object.Destroy(rd);
		int rx = m_roll.x;
		int ry = m_roll.y;
		m_qpos = new mario_point(m_main_char.m_pos.x, m_main_char.m_pos.y);
		bool b = m_first_csm;
		reset(world);
		m_first_csm = b;
		for (int i = 0; i < 24; i++)
		{
			for (int j = 0; j < 12; j++)
			{
				int x = w / 2 + (i - 12) * 64 - rx % utils.g_grid_size / 10;
				int y = h / 2 + (j - 6) * 64 - ry % utils.g_grid_size / 10;
				if (x >= 0 && y >= 0 && x + 63 < w && y + 63 < h)
				{
					GameObject res = (GameObject)Resources.Load("unit/other/fg");
					GameObject obj = (GameObject)UnityEngine.Object.Instantiate(res);
					obj.transform.parent = m_fuzhu.transform;
					obj.transform.localPosition = new Vector3((i - 12) * utils.g_grid_size / 10 + 32, (j - 6) * utils.g_grid_size / 10 + 32, 0f);
					obj.transform.localScale = new Vector3(1f, 1f, 1f);
					obj.GetComponent<edit_gui_fg>().reset(i, j);
					Texture2D tex2 = new Texture2D(64, 64, TextureFormat.RGB24, mipmap: false);
					tex2.SetPixels(tex.GetPixels(x, y, 64, 64));
					tex2.Apply(updateMipmaps: false);
					Sprite sp = Sprite.Create(tex2, new Rect(0f, 0f, 64f, 64f), new Vector2(0.5f, 0.5f), 1f);
					obj.GetComponent<SpriteRenderer>().sprite = sp;
				}
			}
		}
		m_main_char.m_pos = new mario_point(m_qpos.x, m_qpos.y);
	}

	public void reload_jump(mario_point qpos, int world)
	{
		m_qpos = qpos;
		bool first_csm = m_first_csm;
		reset(world);
		m_first_csm = first_csm;
	}

	public void reload_self(int world)
	{
		bool first_csm = m_first_csm;
		m_qpos = null;
		reset(world);
		m_first_csm = first_csm;
	}

	private void reset(int world)
	{
		m_world = world;
		m_dlg = null;
		mario._instance.remove_child(m_main);
		mario._instance.remove_child(m_fuzhu);
		m_hongs.Clear();
		mario._instance.remove_child(m_other);
		mario._instance.remove_child(m_shadow);
		m_has_create.Clear();
		m_objs.Clear();
		m_zhuiji = null;
		m_roll = new mario_point();
		m_grid = new mario_point();
		m_left = false;
		m_right = false;
		m_up = false;
		m_down = false;
		m_time = 0;
		m_add_qz_type = 0;
		m_add_qz_x = 0;
		m_add_qz_y = 0;
		m_yf = 0;
		m_first_csm = false;
		reset_db();
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
		del_obj(m_main_char);
		m_roll.x = m_main_char.m_pos.x;
		m_roll.y = m_main_char.m_pos.y - utils.g_roll_y;
		//if (m_roll.y + utils.g_height > 30 * utils.g_grid_size)
		//{
			//m_roll.y = 30 * utils.g_grid_size - utils.g_height;
		//}
		if (m_roll.y < 0)
		{
			m_roll.y = 0;
		}
		m_grid.x = m_roll.x / utils.g_grid_size;
		m_grid.y = (m_roll.y + utils.g_roll_y) / utils.g_grid_size;
		for (int i = m_grid.x - utils.g_active_x; i <= m_grid.x + utils.g_active_x; i++)
		{
			if (i < 0 || i >= game_data._instance.m_map_data.maps[m_world].x_num)
			{
				continue;
			}
			for (int j = m_grid.y - utils.g_active_y; j <= m_grid.y + utils.g_active_y; j++)
			{
				if (j >= 0 && j < game_data._instance.m_map_data.maps[m_world].y_num)
				{
					do_create(i, j);
				}
			}
		}
		reset_db();
		create_scene();
		for (int k = 0; k < 24; k++)
		{
			for (int l = 0; l < 11; l++)
			{
				GameObject original = (GameObject)Resources.Load("unit/other/fuzhu");
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(original);
				gameObject.transform.parent = m_fuzhu.transform;
				gameObject.transform.localPosition = new Vector3((k - 12) * utils.g_grid_size / 10, (l * utils.g_grid_size - utils.g_height / 2) / 10, 0f);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
		GameObject original2 = (GameObject)Resources.Load("unit/other/box");
		m_lv = (GameObject)UnityEngine.Object.Instantiate(original2);
		m_lv.transform.parent = m_other.transform;
		m_lv.transform.localPosition = new Vector3(0f, 0f, 0f);
		m_lv.transform.localScale = new Vector3(1f, 0.2f, 1f);
		m_lv.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f, 1f);
		m_lv.SetActive(value: false);
		for (int m = 0; m < 8; m++)
		{
			original2 = (GameObject)Resources.Load("unit/other/box");
			GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(original2);
			gameObject2.transform.parent = m_other.transform;
			gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
			gameObject2.transform.localScale = new Vector3(4f, 4f, 1f);
			gameObject2.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f, 0.25f);
			gameObject2.SetActive(value: false);
			m_hongs.Add(gameObject2);
		}
		GameObject original3 = (GameObject)Resources.Load("unit/other/box");
		m_ban = (GameObject)UnityEngine.Object.Instantiate(original3);
		m_ban.transform.parent = m_other.transform;
		m_ban.transform.localPosition = new Vector3(0f, 0f, 0f);
		m_ban.transform.localScale = new Vector3(4f, 120f, 1f);
		m_ban.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.25f);
		m_ban1 = (GameObject)UnityEngine.Object.Instantiate(original3);
		m_ban1.transform.parent = m_other.transform;
		m_ban1.transform.localPosition = new Vector3(0f, 0f, 0f);
		m_ban1.transform.localScale = new Vector3(4f, 120f, 1f);
		m_ban1.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.25f);
		reset_zhuiji();
		if (m_edit_cys_num == m_edit_cys_tnum)
		{
			for (int n = 0; n < m_ecs.Count; n++)
			{
				create_cy(n);
			}
		}
		if (mario._instance.m_self.guide > 0 && mario._instance.m_self.guide <= 2)
		{
			create_xy();
		}
	}

	public void reset_ending()
	{
		for (int i = 0; i < m_objs.Count; i++)
		{
			if (m_objs[i].m_name == "mario_end")
			{
				m_objs[i].reset();
			}
		}
	}

	private void add_obj(mario_obj obj)
	{
		m_objs.Add(obj);
		m_has_create.Add(obj.m_init_pos.y * utils.g_max_x + obj.m_init_pos.x);
	}

	private void del_obj(mario_obj obj)
	{
		m_objs.Remove(obj);
		m_has_create.Remove(obj.m_init_pos.y * utils.g_max_x + obj.m_init_pos.x);
	}

	public void create_scene()
	{
		mario._instance.remove_child(m_back);
		string path = "scene/" + game_data._instance.m_map_data.maps[m_world].map_theme + "/scene";
		GameObject original = (GameObject)Resources.Load(path);
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(original);
		gameObject.transform.parent = m_back.transform;
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		m_scene = gameObject.GetComponent<mario_scene>();
	}

	public mario_obj create_mario_obj(string name, s_t_unit unit, List<int> param, int x, int y, bool bb = false)
	{
		string path = "unit/" + name + "/" + name;
		if (unit != null && unit.kfg == 1)
		{
			path = "unit/" + name + "/" + game_data._instance.m_map_data.maps[m_world].map_theme + "/" + name;
		}
		GameObject original = (GameObject)Resources.Load(path);
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(original);
		gameObject.transform.parent = m_main.transform;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		mario_obj component = gameObject.GetComponent<mario_obj>();
		component.m_unit = unit;
		component.m_edit_mode = true;
		component.init(name, param, m_world, x, y, utils.g_grid_size * x + utils.g_grid_size / 2, utils.g_grid_size * y + utils.g_grid_size / 2);
		component.create_shadow(m_shadow);
		add_obj(component);
		if (bb && unit != null && unit.id != 1)
		{
			Vector3 vector = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
			gameObject.transform.localScale = new Vector3(vector.x * 0.7f, vector.y * 0.7f, vector.z * 0.7f);
			TweenScale.Begin(gameObject, 0.1f, new Vector3(vector.x, vector.y, vector.z));
		}
		return component;
	}

	public void create_xy()
	{
		int num = utils.jx_block.Length / 3;
		for (int i = 0; i < num; i++)
		{
			create_mario_obj_xy(utils.jx_block[i * 3 + 2], utils.jx_block[i * 3], utils.jx_block[i * 3 + 1]);
		}
		string path = "unit/other/dlg";
		if (game_data._instance.m_lang != 0)
		{
			path = "unit/other/dlg1";
		}
		GameObject original = (GameObject)Resources.Load(path);
		m_dlg = (GameObject)UnityEngine.Object.Instantiate(original);
		m_dlg.transform.parent = m_other.transform;
		m_dlg.transform.localPosition = new Vector3(0f, 0f, 0f);
		m_dlg.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	public void set_dlg(int x, int y)
	{
		m_dlg.transform.localPosition = new Vector3(x * utils.g_grid_size / 10 + 20, (y + 1) * utils.g_grid_size / 10 - 20, 0f);
	}

	private void create_mario_obj_xy(int id, int x, int y)
	{
		s_t_unit s_t_unit2 = game_data._instance.get_t_unit(id);
		string path = "unit/" + s_t_unit2.res + "/" + s_t_unit2.res;
		if (s_t_unit2.kfg == 1)
		{
			path = "unit/" + s_t_unit2.res + "/" + game_data._instance.m_map_data.maps[m_world].map_theme + "/" + s_t_unit2.res;
		}
		GameObject original = (GameObject)Resources.Load(path);
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(original);
		gameObject.transform.parent = m_main.transform;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.localPosition = new Vector3((utils.g_grid_size * x + utils.g_grid_size / 2) / 10, (utils.g_grid_size * y + utils.g_grid_size / 2) / 10);
		Transform transform = gameObject.transform.FindChild("fx");
		if (transform != null)
		{
			transform.gameObject.SetActive(value: false);
		}
		gameObject.GetComponent<mario_obj>().format_shadow(gameObject.transform, "shader/gray", -20);
	}

	public void add_obj(int x, int y, int type)
	{
		//if (!can_add_ex(x, y))
		//{
		//	//mario._instance.show_tip(game_data._instance.get_language_string("edit_mode_qdzd"));
		//}
		//else 
		if (type > 0)
		{
			s_t_unit s_t_unit2 = game_data._instance.get_t_unit(type);
			//if (s_t_unit2.is_sw == 1 && !check_sw(x, y))
			//{
			//	mario._instance.show_tip(game_data._instance.get_language_string("edit_mode_tqsw"));
			//}
			//else 
			if (type == 1)
			{
				if (y < game_data._instance.m_map_data.maps[m_world].y_num && game_data._instance.m_arrays[m_world][y][x].type != 0)
				{
					mario._instance.show_tip(game_data._instance.get_language_string("edit_mode_wffz"));
					return;
				}
				for (int i = 0; i < y; i++)
				{
					if (i < game_data._instance.m_map_data.maps[m_world].y_num && game_data._instance.m_arrays[m_world][i][x].type != 0 && game_data._instance.m_arrays[m_world][i][x].type != 1)
					{
						mario._instance.show_tip(game_data._instance.get_language_string("edit_mode_xfcz"));
						return;
					}
				}
				add_y(y);
				for (int j = 0; j <= y; j++)
				{
					if (game_data._instance.m_arrays[m_world][j][x].type == 0)
					{
						game_data._instance.m_arrays[m_world][j][x].type = type;
						add_init(x, j, type);
					}
				}
				reset_db();
			}
			else if (type == utils.g_csg)
			{
				if (m_pre_csg == null)
				{
					if (y < game_data._instance.m_map_data.maps[m_world].y_num && game_data._instance.m_arrays[m_world][y][x].type == utils.g_csg)
					{
						if (game_data._instance.m_arrays[m_world][y][x].param[2] == 0)
						{
							m_pre_csg = new mario_point(x, y);
						}
					}
					else if (y < game_data._instance.m_map_data.maps[m_world].y_num && game_data._instance.m_arrays[m_world][y][x].type != 0)
					{
						mario._instance.show_tip(game_data._instance.get_language_string("edit_mode_wffz"));
					}
					else if (has_csg(x, y, -1, -1))
					{
						mario._instance.show_tip(game_data._instance.get_language_string("edit_mode_wffz"));
					}
					else
					{
						add_y(y);
						game_data._instance.m_arrays[m_world][y][x].type = type;
						add_init(x, y, type);
						m_pre_csg = new mario_point(x, y);
					}
				}
				else if (y >= game_data._instance.m_map_data.maps[m_world].y_num || game_data._instance.m_arrays[m_world][y][x].type == 0)
				{
					int num = m_pre_csg.x - x;
					int num2 = m_pre_csg.y - y;
					bool flag = false;
					if ((num == 2 && num2 == 0) || (num == -2 && num2 == 0) || (num == 0 && num2 == 2) || (num == 0 && num2 == -2) || (num == 2 && num2 == 2) || (num == -2 && num2 == 2) || (num == 2 && num2 == -2) || (num == -2 && num2 == -2))
					{
						flag = true;
					}
					if (flag && can_csg(m_pre_csg.x, m_pre_csg.y, x, y))
					{
						add_y(y);
						game_data._instance.m_arrays[m_world][y][x].type = type;
						game_data._instance.m_arrays[m_world][m_pre_csg.y][m_pre_csg.x].param[2] = x;
						game_data._instance.m_arrays[m_world][m_pre_csg.y][m_pre_csg.x].param[3] = y;
						game_data._instance.m_arrays[m_world][y][x].param[0] = m_pre_csg.x;
						game_data._instance.m_arrays[m_world][y][x].param[1] = m_pre_csg.y;
						add_init(x, y, type);
						m_pre_csg = new mario_point(x, y);
					}
				}
			}
			else if (type == utils.g_cspt)
			{
				if (y < game_data._instance.m_map_data.maps[m_world].y_num && game_data._instance.m_arrays[m_world][y][x].type != 0)
				{
					mario._instance.show_tip(game_data._instance.get_language_string("edit_mode_wffz"));
					return;
				}
				if (!has_csg(x, y, -1, -1))
				{
					mario._instance.show_tip(game_data._instance.get_language_string("edit_mode_xcsd"));
					return;
				}
				add_y(y);
				game_data._instance.m_arrays[m_world][y][x].type = type;
				add_init(x, y, type);
			}
			else if (type == utils.g_hg)
			{
				if (y < game_data._instance.m_map_data.maps[m_world].y_num && game_data._instance.m_arrays[m_world][y][x].type == utils.g_hg)
				{
					change1_init(x, y);
					return;
				}
				if (y < game_data._instance.m_map_data.maps[m_world].y_num && game_data._instance.m_arrays[m_world][y][x].type != 0)
				{
					mario._instance.show_tip(game_data._instance.get_language_string("edit_mode_wffz"));
					return;
				}
				add_y(y);
				game_data._instance.m_arrays[m_world][y][x].type = type;
				add_init(x, y, type);
			}
			else if (type == utils.g_csm)
			{
				if (y < game_data._instance.m_map_data.maps[m_world].y_num && game_data._instance.m_arrays[m_world][y][x].type != 0)
				{
					mario._instance.show_tip(game_data._instance.get_language_string("edit_mode_wffz"));
					return;
				}
				add_y(y);
				if (!m_first_csm)
				{
					m_csm_id = game_data._instance.get_new_csm();
					if (m_csm_id == -1)
					{
						mario._instance.show_tip(game_data._instance.get_language_string("edit_mode_ccsm"));
						return;
					}
					m_csm_w = m_world;
					m_csm_x = x;
					m_csm_y = y;
					game_data._instance.m_arrays[m_world][y][x].type = type;
					game_data._instance.m_arrays[m_world][y][x].param[0] = m_csm_id;
					m_first_csm = true;
				}
				else
				{
					game_data._instance.m_arrays[m_world][y][x].type = type;
					game_data._instance.m_arrays[m_world][y][x].param[0] = m_csm_id;
					game_data._instance.m_arrays[m_world][y][x].param[1] = 1;
					m_first_csm = false;
				}
				add_init(x, y, type);
			}
			else if (y < game_data._instance.m_map_data.maps[m_world].y_num && game_data._instance.m_arrays[m_world][y][x].type != 0)
			{
				s_t_unit s_t_unit3 = game_data._instance.get_t_unit(game_data._instance.m_arrays[m_world][y][x].type);
				bool flag2 = false;
				if (game_data._instance.m_arrays[m_world][y][x].param[0] == 0)
				{
					if (s_t_unit3.fwt == 1 && s_t_unit2.fwt >= 10)
					{
						flag2 = true;
					}
					else if (s_t_unit3.fwt == 2 && s_t_unit2.fwt == 11)
					{
						flag2 = true;
					}
				}
				flag2 = true;
				if (game_data._instance.m_arrays[m_world][y][x].type == 110 || game_data._instance.m_arrays[m_world][y][x].type == 112) flag2 = false;
				if (flag2)
				{
					game_data._instance.m_arrays[m_world][y][x].param[0] = type;
					set_init(x, y, type);
				}
				else
				{
					mario._instance.show_tip(game_data._instance.get_language_string("edit_mode_wffz"));
				}
			}
			else if (s_t_unit2.max_num > 0 && game_data._instance.get_unit_num(m_world, type) >= s_t_unit2.max_num)
			{
				mario._instance.show_tip(game_data._instance.get_language_string("edit_mode_zdsl"));
			}
			else
			{
				add_y(y);
				game_data._instance.m_arrays[m_world][y][x].type = type;
				add_init(x, y, type);
			}
		}
		else if (type == 0 && y < game_data._instance.m_map_data.maps[m_world].y_num)
		{
			del_obj(x, y);
		}
		else if (type == -1 && y < game_data._instance.m_map_data.maps[m_world].y_num && game_data._instance.m_arrays[m_world][y][x].type != 0)
		{
			change_init(x, y);
		}
	}

	private void del_obj(int x, int y, bool flag = false)
	{
		//|| game_data._instance.m_arrays[m_world][y][x].type >= 1000
		if (game_data._instance.m_arrays[m_world][y][x].type == 0 )
		{
			return;
		}
		s_t_unit s_t_unit2 = game_data._instance.get_t_unit(game_data._instance.m_arrays[m_world][y][x].type);
		if (!flag && s_t_unit2.fwt == 1 && game_data._instance.m_arrays[m_world][y][x].param[0] != 0)
		{
			for (int i = 0; i < game_data._instance.m_arrays[m_world][y][x].param.Count; i++)
			{
				game_data._instance.m_arrays[m_world][y][x].param[i] = 0;
			}
			set_init(x, y, 0);
		}
		else if (game_data._instance.m_arrays[m_world][y][x].type == 1)
		{
			for (int num = game_data._instance.m_map_data.maps[m_world].y_num - 1; num >= y; num--)
			{
				if (game_data._instance.m_arrays[m_world][num][x].type == 1)
				{
					game_data._instance.m_arrays[m_world][num][x].type = 0;
					del_init(x, num);
					del_y(num);
				}
			}
			reset_db();
		}
		else if (game_data._instance.m_arrays[m_world][y][x].type == utils.g_csg)
		{
			int num2 = x;
			int num3 = y;
			int num4 = game_data._instance.m_arrays[m_world][y][x].param[0];
			int num5 = game_data._instance.m_arrays[m_world][y][x].param[1];
			if (num4 != 0 || num5 != 0)
			{
				int num6 = num4 + (num2 - num4) / 2;
				int num7 = num5 + (num3 - num5) / 2;
				if (game_data._instance.m_arrays[m_world][num7][num6].type == utils.g_cspt)
				{
					game_data._instance.m_arrays[m_world][num7][num6].type = 0;
					del_init(num6, num7);
					del_y(num7);
				}
				game_data._instance.m_arrays[m_world][num5][num4].param[2] = 0;
				game_data._instance.m_arrays[m_world][num5][num4].param[3] = 0;
			}
			while (num2 != 0 || num3 != 0)
			{
				int num8 = num2;
				int num9 = num3;
				num2 = game_data._instance.m_arrays[m_world][num9][num8].param[2];
				num3 = game_data._instance.m_arrays[m_world][num9][num8].param[3];
				if (num2 != 0 || num3 != 0)
				{
					int num10 = num8 + (num2 - num8) / 2;
					int num11 = num9 + (num3 - num9) / 2;
					if (game_data._instance.m_arrays[m_world][num11][num10].type == utils.g_cspt)
					{
						game_data._instance.m_arrays[m_world][num11][num10].type = 0;
						del_init(num10, num11);
						del_y(num11);
					}
				}
				game_data._instance.m_arrays[m_world][num9][num8].type = 0;
				del_init(num8, num9);
				del_y(num9);
			}
		}
		else if (game_data._instance.m_arrays[m_world][y][x].type == utils.g_csm)
		{
			if (m_first_csm && game_data._instance.m_arrays[m_world][y][x].param[0] == m_csm_id)
			{
				m_first_csm = false;
			}
			else
			{
				int w = 0;
				int x2 = 0;
				int y2 = 0;
				game_data._instance.get_csm(m_world, x, y, ref w, ref x2, ref y2);
				game_data._instance.m_arrays[w][y2][x2].type = 0;
				if (w == m_world)
				{
					del_init(x2, y2);
					del_y(y2);
				}
			}
			game_data._instance.m_arrays[m_world][y][x].type = 0;
			del_init(x, y);
			del_y(y);
		}
		else
		{
			game_data._instance.m_arrays[m_world][y][x].type = 0;
			del_init(x, y);
			del_y(y);
		}
	}

	private bool can_add(int x, int y)
	{
		if (x < 0 || x >= game_data._instance.m_map_data.maps[m_world].x_num || y < 0 || y >= game_data._instance.m_map_data.maps[m_world].y_num)
		{
			return false;
		}
		if (x < 3 && y < game_data._instance.m_map_data.maps[m_world].qd_y)
		{
			return false;
		}
		if (x >= game_data._instance.m_map_data.maps[m_world].x_num - 3 && y < game_data._instance.m_map_data.maps[m_world].zd_y)
		{
			return false;
		}
		return true;
	}

	private bool can_add_ex(int x, int y)
	{
		if (x < 0 || x >= game_data._instance.m_map_data.maps[m_world].x_num || y < 0 || y >= utils.g_max_y)
		{
			return false;
		}
		if (x < 3 && y < game_data._instance.m_map_data.maps[m_world].qd_y)
		{
			return false;
		}
		if (x >= game_data._instance.m_map_data.maps[m_world].x_num - 3 && y < game_data._instance.m_map_data.maps[m_world].zd_y)
		{
			return false;
		}
		return true;
	}

	public void not_add_obj()
	{
		try
		{
			m_pre_csg = null;
			m_lv.SetActive(value: false);
			for (int i = 0; i < m_hongs.Count; i++)
			{
				m_hongs[i].SetActive(value: false);
			}
		}
		catch (Exception e) 
		{
			Debug.LogError(e);
		}
	}

	public void set_mpos(int x, int y)
	{
		if (m_pre_csg == null)
		{
			return;
		}
		int num = m_pre_csg.x * utils.g_grid_size + utils.g_grid_size / 2;
		int num2 = m_pre_csg.y * utils.g_grid_size + utils.g_grid_size / 2;
		int num3 = (num + x) / 2;
		int num4 = (num2 + y) / 2;
		float num5 = (float)(x - num) / 10f;
		float num6 = (float)(y - num2) / 10f;
		float num7 = Mathf.Sqrt(num5 * num5 + num6 * num6) / (float)utils.g_grid_size * 10f;
		float num8 = Mathf.Atan2(num6, num5);
		num8 = num8 * 180f / (float)Math.PI;
		m_lv.transform.localPosition = new Vector3(num3 / 10, num4 / 10, 0f);
		m_lv.transform.localScale = new Vector3(num7 * 4f, 0.2f, 0f);
		m_lv.transform.localEulerAngles = new Vector3(0f, 0f, num8);
		m_lv.SetActive(value: true);
		for (int i = 0; i < m_hongs.Count; i++)
		{
			int num9 = m_pre_csg.x + utils.csg_points[i, 0];
			int num10 = m_pre_csg.y + utils.csg_points[i, 1];
			if (can_csg(m_pre_csg.x, m_pre_csg.y, num9, num10))
			{
				m_hongs[i].SetActive(value: true);
				num9 = num9 * utils.g_grid_size + utils.g_grid_size / 2;
				num10 = num10 * utils.g_grid_size + utils.g_grid_size / 2;
				m_hongs[i].transform.localPosition = new Vector3(num9 / 10, num10 / 10, 0f);
			}
			else
			{
				m_hongs[i].SetActive(value: false);
			}
		}
	}

	private bool can_csg(int x1, int y1, int x2, int y2)
	{
		if (!can_add_ex(x2, y2))
		{
			return false;
		}
		if (y2 < game_data._instance.m_map_data.maps[m_world].y_num && game_data._instance.m_arrays[m_world][y2][x2].type != 0)
		{
			return false;
		}
		if (has_csg(x2, y2, x1, y1))
		{
			return false;
		}
		int num = x1 + (x2 - x1) / 2;
		int num2 = y1 + (y2 - y1) / 2;
		if (!can_add_ex(num, num2))
		{
			return false;
		}
		if (num2 < game_data._instance.m_map_data.maps[m_world].y_num && game_data._instance.m_arrays[m_world][num2][num].type == utils.g_csg)
		{
			return false;
		}
		if (has_csg(num, num2, x1, y1))
		{
			return false;
		}
		return true;
	}

	private bool has_csg(int x, int y, int px, int py)
	{
		for (int i = 0; i < 4; i++)
		{
			int num = x + utils.csg_points[i * 2, 0] / 2;
			int num2 = y + utils.csg_points[i * 2, 1] / 2;
			int num3 = x + utils.csg_points[i * 2 + 1, 0] / 2;
			int num4 = y + utils.csg_points[i * 2 + 1, 1] / 2;
			if ((num != px || num2 != py) && (num3 != px || num4 != py) && can_add(num, num2) && can_add(num3, num4) && game_data._instance.m_arrays[m_world][num2][num].type == utils.g_csg && game_data._instance.m_arrays[m_world][num4][num3].type == utils.g_csg)
			{
				if (game_data._instance.m_arrays[m_world][num2][num].param[0] == num3 && game_data._instance.m_arrays[m_world][num2][num].param[1] == num4)
				{
					return true;
				}
				if (game_data._instance.m_arrays[m_world][num2][num].param[2] == num3 && game_data._instance.m_arrays[m_world][num2][num].param[3] == num4)
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool check_sw(int x, int y)
	{
		int num = 0;
		for (int i = x - utils.g_active_x; i <= x + utils.g_active_x; i++)
		{
			if (i < 0 || i >= game_data._instance.m_map_data.maps[m_world].x_num)
			{
				continue;
			}
			for (int j = y - utils.g_active_y; j <= y + utils.g_active_y; j++)
			{
				if (j >= 0 && j < game_data._instance.m_map_data.maps[m_world].y_num)
				{
					s_t_unit s_t_unit2 = game_data._instance.get_t_unit(game_data._instance.m_arrays[m_world][j][i].type);
					if (s_t_unit2 != null && s_t_unit2.is_sw == 1)
					{
						num++;
					}
				}
			}
		}
		if (num >= 30)
		{
			return false;
		}
		return true;
	}

	private void reset_db()
	{
		for (int i = 0; i < m_objs.Count; i++)
		{
			mario_obj component = m_objs[i].GetComponent<mario_obj>();
			if (component.m_unit != null && component.m_unit.id == 1)
			{
				component.change();
			}
		}
	}

	public void reset_zhuiji()
	{
		if (game_data._instance.m_map_data.mode != 0 && m_zhuiji == null)
		{
			GameObject original = (GameObject)Resources.Load("unit/other/zhuiji");
			m_zhuiji = (GameObject)UnityEngine.Object.Instantiate(original);
			m_zhuiji.transform.parent = m_other.transform;
			m_zhuiji.transform.localPosition = new Vector3((m_main_char.m_pos.x - utils.g_grid_size * 5) / 10, utils.g_height / 2 / 10, 0f);
			m_zhuiji.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else if (game_data._instance.m_map_data.mode == 0 && m_zhuiji != null)
		{
			UnityEngine.Object.Destroy(m_zhuiji);
			m_zhuiji = null;
		}
	}

	public void check_mission()
	{
		if (m_first_csm)
		{
			game_data._instance.m_arrays[m_csm_w][m_csm_y][m_csm_x].type = 0;
			if (m_world == m_csm_w)
			{
				del_init(m_csm_x, m_csm_y);
				del_y(m_csm_y);
			}
			m_first_csm = false;
			mario._instance.show_tip(game_data._instance.get_language_string("edit_mode_qcsm"));
		}
	}

	private void add_y(int y)
	{
		if (y < game_data._instance.m_map_data.maps[m_world].y_num)
		{
			return;
		}
		for (int i = game_data._instance.m_arrays[m_world].Count; i < y + 1; i++)
		{
			List<s_t_mission_sub> list = new List<s_t_mission_sub>();
			for (int j = 0; j < game_data._instance.m_map_data.maps[m_world].x_num; j++)
			{
				s_t_mission_sub s_t_mission_sub2 = new s_t_mission_sub();
				s_t_mission_sub2.type = 0;
				list.Add(s_t_mission_sub2);
			}
			game_data._instance.m_arrays[m_world].Add(list);
		}
		game_data._instance.m_map_data.maps[m_world].y_num = y + 1;
	}

	private void del_y(int y)
	{
		while (y == game_data._instance.m_map_data.maps[m_world].y_num - 1 && y > utils.g_min_y && y > game_data._instance.m_map_data.maps[m_world].qd_y + 4 && y > game_data._instance.m_map_data.maps[m_world].zd_y + 4)
		{
			bool flag = false;
			for (int i = 0; i < game_data._instance.m_map_data.maps[m_world].x_num; i++)
			{
				if (game_data._instance.m_arrays[m_world][y][i].type != 0)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				game_data._instance.m_map_data.maps[m_world].y_num--;
				game_data._instance.m_arrays[m_world].RemoveAt(y);
			}
			y--;
		}
	}

	private void add_init(int x, int y, int type)
	{
		s_t_unit s_t_unit2 = game_data._instance.get_t_unit(type);
		if (s_t_unit2 == null)
		{
			return;
		}
		create_mario_obj(s_t_unit2.res, s_t_unit2, game_data._instance.m_arrays[m_world][y][x].param, x, y, bb: true);
		if (s_t_unit2 != null && s_t_unit2.id == utils.g_yinfu)
		{
			mario._instance.play_sound("sound/yf/0-" + (y + 1));
		}
		else if (s_t_unit2 == null || s_t_unit2.id != 1)
		{
			utils.do_yfu(utils.yuepu[m_yf * 2], utils.yuepu[m_yf * 2 + 1]);
			m_yf++;
			if (m_yf * 2 >= utils.yuepu.Length)
			{
				m_yf = 0;
			}
		}
	}

	private void del_init(int x, int y)
	{
		for (int num = m_objs.Count - 1; num >= 0; num--)
		{
			mario_obj mario_obj2 = m_objs[num];
			if (mario_obj2.m_init_pos.x == x && mario_obj2.m_init_pos.y == y)
			{
				if (mario_obj2.m_unit != null && mario_obj2.m_unit.id != 1)
				{
					utils.do_yfu(utils.yuepu[m_yf * 2], utils.yuepu[m_yf * 2 + 1]);
					m_yf++;
					if (m_yf * 2 >= utils.yuepu.Length)
					{
						m_yf = 0;
					}
				}
				del_obj(mario_obj2);
				mario_obj2.destroy_shadow();
				UnityEngine.Object.Destroy(mario_obj2.gameObject);
			}
		}
	}

	private void change_init(int x, int y)
	{
		for (int i = 0; i < m_objs.Count; i++)
		{
			if (m_objs[i].m_unit != null && m_objs[i].m_init_pos.x == x && m_objs[i].m_init_pos.y == y)
			{
				m_objs[i].GetComponent<mario_obj>().change();
				break;
			}
		}
	}

	private void change1_init(int x, int y)
	{
		for (int i = 0; i < m_objs.Count; i++)
		{
			if (m_objs[i].m_unit != null && m_objs[i].m_init_pos.x == x && m_objs[i].m_init_pos.y == y)
			{
				m_objs[i].GetComponent<mario_obj>().change1();
				break;
			}
		}
	}

	private void set_init(int x, int y, int type)
	{
		for (int i = 0; i < m_objs.Count; i++)
		{
			if (m_objs[i].m_unit != null && m_objs[i].m_init_pos.x == x && m_objs[i].m_init_pos.y == y)
			{
				for (int j = 0; j < m_objs[i].GetComponent<mario_obj>().m_param.Count; j++)
				{
					m_objs[i].GetComponent<mario_obj>().m_param[j] = 0;
				}
				m_objs[i].GetComponent<mario_obj>().m_param[0] = type;
				m_objs[i].GetComponent<mario_obj>().reset();
				break;
			}
		}
	}

	public void add_qz(int type, int x, int y)
	{
		m_add_qz_type = type;
		m_add_qz_x = x;
		m_add_qz_y = y;
	}

	private void update_ex()
	{
		try
		{
			for (int i = 0; i < m_objs.Count; i++)
			{
				m_objs[i].update_ex(m_grid);
			}
			m_main_char.update_ex(m_grid);
			m_scene.update_ex(m_roll);
			if (game_data._instance.m_map_data.mode != 0)
			{
				m_zhuiji.transform.localPosition = new Vector3((m_roll.x - utils.g_grid_size * 5) / 10, (m_roll.y + utils.g_roll_y) / 10, 0f);
			}
			m_fuzhu.transform.localPosition = new Vector3(-m_roll.x % utils.g_grid_size / 10, -m_roll.y % utils.g_grid_size / 10, 0f);
			m_ban.transform.localPosition = new Vector3(-utils.g_grid_size / 2 / 10, utils.g_grid_size * 15 / 10, 0f);
			m_ban1.transform.localPosition = new Vector3((game_data._instance.m_map_data.maps[m_world].x_num * utils.g_grid_size + utils.g_grid_size / 2) / 10, utils.g_grid_size * 15 / 10, 0f);
		}
		catch (Exception ex)
		{
			Debug.LogError(ex);
		}
	}

	private bool update_edit_cy()
	{
		if (m_edit_cys_num < m_edit_cys_tnum)
		{
			m_edit_cys_time++;
			bool flag = false;
			if (m_edit_cys_num == 0)
			{
				if (m_edit_cys_time == 20)
				{
					flag = true;
				}
			}
			else if (m_edit_cys_time == 3)
			{
				flag = true;
			}
			while (flag)
			{
				m_edit_cys_time = 0;
				flag = !create_cy(m_edit_cys_num);
				m_edit_cys_num++;
				if (m_edit_cys_num >= m_edit_cys_tnum)
				{
					break;
				}
			}
			return false;
		}
		return true;
	}

	private void pingyi()
	{
		if (m_add_qz_type > 0)
		{
			bool flag = false;
			if (m_add_qz_type == 1)
			{
				if (m_add_qz_y < 0 && game_data._instance.m_map_data.maps[m_world].qd_y > 2)
				{
					flag = true;
					int y = m_main_char.m_pos.y;
					m_main_char.m_pos.y -= 40;
					if (m_time > 30)
					{
						m_main_char.m_pos.y -= 80;
					}
					if (y / utils.g_grid_size != m_main_char.m_pos.y / utils.g_grid_size)
					{
						int qd_y = game_data._instance.m_map_data.maps[m_world].qd_y;
						game_data._instance.m_map_data.maps[m_world].qd_y--;
						game_data._instance.m_arrays[m_world][qd_y][1].type = 0;
						del_init(1, qd_y);
						del_y(qd_y + 4);
						for (int i = 0; i < 3; i++)
						{
							game_data._instance.m_arrays[m_world][game_data._instance.m_map_data.maps[m_world].qd_y][i].type = 0;
							del_init(i, game_data._instance.m_map_data.maps[m_world].qd_y);
						}
						game_data._instance.m_arrays[m_world][game_data._instance.m_map_data.maps[m_world].qd_y][1].type = 1000;
						add_init(1, game_data._instance.m_map_data.maps[m_world].qd_y, 1000);
						reset_db();
					}
				}
				if (m_add_qz_y > 0 && game_data._instance.m_map_data.maps[m_world].qd_y < utils.g_max_y - 5)
				{
					flag = true;
					int y2 = m_main_char.m_pos.y;
					m_main_char.m_pos.y += 40;
					if (m_time > 30)
					{
						m_main_char.m_pos.y += 80;
					}
					if (y2 / utils.g_grid_size != m_main_char.m_pos.y / utils.g_grid_size)
					{
						int qd_y2 = game_data._instance.m_map_data.maps[m_world].qd_y;
						game_data._instance.m_map_data.maps[m_world].qd_y++;
						del_obj(1, qd_y2 + 1, flag: true);
						for (int j = 0; j < 3; j++)
						{
							del_obj(j, qd_y2, flag: true);
						}
						add_y(game_data._instance.m_map_data.maps[m_world].qd_y + 4);
						del_init(1, qd_y2);
						for (int k = 0; k < 3; k++)
						{
							game_data._instance.m_arrays[m_world][qd_y2][k].type = 1;
							add_init(k, qd_y2, 1);
						}
						game_data._instance.m_arrays[m_world][game_data._instance.m_map_data.maps[m_world].qd_y][1].type = 1000;
						add_init(1, game_data._instance.m_map_data.maps[m_world].qd_y, 1000);
						reset_db();
					}
				}
			}
			else
			{
				if (m_add_qz_y < 0 && game_data._instance.m_map_data.maps[m_world].zd_y > 2)
				{
					flag = true;
					int y3 = m_main_char.m_pos.y;
					m_main_char.m_pos.y -= 40;
					if (m_time > 30)
					{
						m_main_char.m_pos.y -= 80;
					}
					if (y3 / utils.g_grid_size != m_main_char.m_pos.y / utils.g_grid_size)
					{
						int zd_y = game_data._instance.m_map_data.maps[m_world].zd_y;
						game_data._instance.m_map_data.maps[m_world].zd_y--;
						game_data._instance.m_arrays[m_world][zd_y][game_data._instance.m_map_data.maps[m_world].x_num - 2].type = 0;
						del_init(game_data._instance.m_map_data.maps[m_world].x_num - 2, zd_y);
						del_y(zd_y + 4);
						for (int l = 0; l < 3; l++)
						{
							game_data._instance.m_arrays[m_world][game_data._instance.m_map_data.maps[m_world].zd_y][game_data._instance.m_map_data.maps[m_world].x_num - l - 1].type = 0;
							del_init(game_data._instance.m_map_data.maps[m_world].x_num - l - 1, game_data._instance.m_map_data.maps[m_world].zd_y);
						}
						game_data._instance.m_arrays[m_world][game_data._instance.m_map_data.maps[m_world].zd_y][game_data._instance.m_map_data.maps[m_world].x_num - 2].type = 1001;
						add_init(game_data._instance.m_map_data.maps[m_world].x_num - 2, game_data._instance.m_map_data.maps[m_world].zd_y, 1001);
						reset_db();
					}
				}
				if (m_add_qz_y > 0 && game_data._instance.m_map_data.maps[m_world].zd_y < utils.g_max_y - 5)
				{
					flag = true;
					int y4 = m_main_char.m_pos.y;
					m_main_char.m_pos.y += 40;
					if (m_time > 30)
					{
						m_main_char.m_pos.y += 80;
					}
					if (y4 / utils.g_grid_size != m_main_char.m_pos.y / utils.g_grid_size)
					{
						int zd_y2 = game_data._instance.m_map_data.maps[m_world].zd_y;
						game_data._instance.m_map_data.maps[m_world].zd_y++;
						del_obj(game_data._instance.m_map_data.maps[m_world].x_num - 2, zd_y2 + 1, flag: true);
						for (int m = 0; m < 3; m++)
						{
							del_obj(game_data._instance.m_map_data.maps[m_world].x_num - m - 1, zd_y2, flag: true);
						}
						add_y(game_data._instance.m_map_data.maps[m_world].zd_y + 4);
						del_init(game_data._instance.m_map_data.maps[m_world].x_num - 2, zd_y2);
						for (int n = 0; n < 3; n++)
						{
							game_data._instance.m_arrays[m_world][zd_y2][game_data._instance.m_map_data.maps[m_world].x_num - n - 1].type = 1;
							add_init(game_data._instance.m_map_data.maps[m_world].x_num - n - 1, zd_y2, 1);
						}
						game_data._instance.m_arrays[m_world][game_data._instance.m_map_data.maps[m_world].zd_y][game_data._instance.m_map_data.maps[m_world].x_num - 2].type = 1001;
						add_init(game_data._instance.m_map_data.maps[m_world].x_num - 2, game_data._instance.m_map_data.maps[m_world].zd_y, 1001);
						reset_db();
					}
				}
				if (m_add_qz_x < 0 && game_data._instance.m_map_data.maps[m_world].x_num > utils.g_min_x)
				{
					flag = true;
					int x = m_main_char.m_pos.x;
					m_main_char.m_pos.x -= 40;
					if (m_time > 30)
					{
						m_main_char.m_pos.x -= 80;
					}
					if (x / utils.g_grid_size != m_main_char.m_pos.x / utils.g_grid_size)
					{
						for (int num = 0; num < game_data._instance.m_map_data.maps[m_world].zd_y; num++)
						{
							for (int num2 = 1; num2 < 4; num2++)
							{
								del_obj(game_data._instance.m_map_data.maps[m_world].x_num - num2 - 1, num, flag: true);
							}
						}
						for (int num3 = 0; num3 < game_data._instance.m_map_data.maps[m_world].y_num; num3++)
						{
							del_obj(game_data._instance.m_map_data.maps[m_world].x_num - 1, num3, flag: true);
						}
						del_obj(game_data._instance.m_map_data.maps[m_world].x_num - 3, game_data._instance.m_map_data.maps[m_world].zd_y, flag: true);
						del_init(game_data._instance.m_map_data.maps[m_world].x_num - 2, game_data._instance.m_map_data.maps[m_world].zd_y);
						game_data._instance.m_arrays[m_world][game_data._instance.m_map_data.maps[m_world].zd_y][game_data._instance.m_map_data.maps[m_world].x_num - 2].type = 0;
						game_data._instance.m_map_data.maps[m_world].x_num--;
						for (int num4 = 0; num4 < game_data._instance.m_map_data.maps[m_world].y_num; num4++)
						{
							game_data._instance.m_arrays[m_world][num4].RemoveAt(game_data._instance.m_map_data.maps[m_world].x_num);
						}
						for (int num5 = 0; num5 < game_data._instance.m_map_data.maps[m_world].zd_y; num5++)
						{
							for (int num6 = 2; num6 >= 0; num6--)
							{
								game_data._instance.m_arrays[m_world][num5][game_data._instance.m_map_data.maps[m_world].x_num - num6 - 1].type = 1;
								add_init(game_data._instance.m_map_data.maps[m_world].x_num - num6 - 1, num5, 1);
							}
						}
						game_data._instance.m_arrays[m_world][game_data._instance.m_map_data.maps[m_world].zd_y][game_data._instance.m_map_data.maps[m_world].x_num - 2].type = 1001;
						add_init(game_data._instance.m_map_data.maps[m_world].x_num - 2, game_data._instance.m_map_data.maps[m_world].zd_y, 1001);
						reset_db();
					}
				}
				if (m_add_qz_x > 0 && game_data._instance.m_map_data.maps[m_world].x_num < utils.g_max_x)
				{
					flag = true;
					int x2 = m_main_char.m_pos.x;
					m_main_char.m_pos.x += 40;
					if (m_time > 30)
					{
						m_main_char.m_pos.x += 80;
					}
					if (x2 / utils.g_grid_size != m_main_char.m_pos.x / utils.g_grid_size)
					{
						for (int num7 = 0; num7 < game_data._instance.m_map_data.maps[m_world].zd_y; num7++)
						{
							for (int num8 = 0; num8 < 3; num8++)
							{
								del_obj(game_data._instance.m_map_data.maps[m_world].x_num - num8 - 1, num7, flag: true);
							}
						}
						del_obj(game_data._instance.m_map_data.maps[m_world].x_num - 1, game_data._instance.m_map_data.maps[m_world].zd_y, flag: true);
						del_init(game_data._instance.m_map_data.maps[m_world].x_num - 2, game_data._instance.m_map_data.maps[m_world].zd_y);
						game_data._instance.m_arrays[m_world][game_data._instance.m_map_data.maps[m_world].zd_y][game_data._instance.m_map_data.maps[m_world].x_num - 2].type = 0;
						game_data._instance.m_map_data.maps[m_world].x_num++;
						for (int num9 = 0; num9 < game_data._instance.m_map_data.maps[m_world].y_num; num9++)
						{
							s_t_mission_sub s_t_mission_sub2 = new s_t_mission_sub();
							s_t_mission_sub2.type = 0;
							game_data._instance.m_arrays[m_world][num9].Add(s_t_mission_sub2);
						}
						for (int num10 = 0; num10 < game_data._instance.m_map_data.maps[m_world].zd_y; num10++)
						{
							for (int num11 = 2; num11 >= 0; num11--)
							{
								game_data._instance.m_arrays[m_world][num10][game_data._instance.m_map_data.maps[m_world].x_num - num11 - 1].type = 1;
								add_init(game_data._instance.m_map_data.maps[m_world].x_num - num11 - 1, num10, 1);
							}
						}
						game_data._instance.m_arrays[m_world][game_data._instance.m_map_data.maps[m_world].zd_y][game_data._instance.m_map_data.maps[m_world].x_num - 2].type = 1001;
						add_init(game_data._instance.m_map_data.maps[m_world].x_num - 2, game_data._instance.m_map_data.maps[m_world].zd_y, 1001);
						reset_db();
					}
				}
			}
			if (flag)
			{
				m_time++;
			}
			else
			{
				m_time = 0;
			}
			return;
		}
		bool flag2 = false;
		if (m_left)
		{
			flag2 = true;
			m_main_char.m_pos.x -= 40;
			if (m_time > 30)
			{
				m_main_char.m_pos.x -= 80;
			}
		}
		if (m_right)
		{
			flag2 = true;
			m_main_char.m_pos.x += 40;
			if (m_time > 30)
			{
				m_main_char.m_pos.x += 80;
			}
		}
		if (m_up)
		{
			flag2 = true;
			m_main_char.m_pos.y += 40;
			if (m_time > 30)
			{
				m_main_char.m_pos.y += 80;
			}
		}
		if (m_down)
		{
			flag2 = true;
			m_main_char.m_pos.y -= 40;
			if (m_time > 30)
			{
				m_main_char.m_pos.y -= 80;
			}
		}
		if (flag2)
		{
			m_time++;
		}
		else
		{
			m_time = 0;
		}
	}

	private void check_roll()
	{
		if (m_main_char.m_pos.x < 0)
		{
			m_main_char.m_pos.x = 0;
		}
		if (m_main_char.m_pos.x >= game_data._instance.m_map_data.maps[m_world].x_num * utils.g_grid_size)
		{
			m_main_char.m_pos.x = game_data._instance.m_map_data.maps[m_world].x_num * utils.g_grid_size - 1;
		}
		if (m_main_char.m_pos.y < 0)
		{
			m_main_char.m_pos.y = 0;
		}
		if (m_main_char.m_pos.y >= utils.g_max_y * utils.g_grid_size)
		{
			m_main_char.m_pos.y = utils.g_max_y * utils.g_grid_size - 1;
		}
		m_roll.x = m_main_char.m_pos.x;
		m_roll.y = m_main_char.m_pos.y - utils.g_roll_y;
		if (m_roll.y + utils.g_height > utils.g_max_y * utils.g_grid_size)
		{
			m_roll.y = utils.g_max_y * utils.g_grid_size - utils.g_height;
		}
		if (m_roll.y < 0)
		{
			m_roll.y = 0;
		}
		bool flag = false;
		int num = m_roll.x / utils.g_grid_size;
		if (num != m_grid.x)
		{
			flag = true;
			int num2 = m_grid.x + utils.g_active_x;
			if (m_grid.x > num)
			{
				num2 = m_grid.x - utils.g_active_x;
			}
			if (num2 >= 0 && num2 < game_data._instance.m_map_data.maps[m_world].x_num)
			{
				for (int i = m_grid.y - utils.g_active_y; i <= m_grid.y + utils.g_active_y; i++)
				{
					if (i >= 0 && i < game_data._instance.m_map_data.maps[m_world].y_num)
					{
						do_create(num2, i);
					}
				}
			}
			m_grid.x = num;
		}
		int num3 = (m_roll.y + utils.g_roll_y) / utils.g_grid_size;
		if (num3 != m_grid.y)
		{
			flag = true;
			int num4 = m_grid.y + utils.g_active_y;
			if (m_grid.y > num3)
			{
				num4 = m_grid.y - utils.g_active_y;
			}
			if (num4 >= 0 && num4 < game_data._instance.m_map_data.maps[m_world].y_num)
			{
				for (int j = m_grid.x - utils.g_active_x; j <= m_grid.x + utils.g_active_x; j++)
				{
					if (j >= 0 && j < game_data._instance.m_map_data.maps[m_world].x_num)
					{
						do_create(j, num4);
					}
				}
			}
			m_grid.y = num3;
		}
		if (flag)
		{
			do_delete();
			reset_db();
		}
	}

	private void FixedUpdate()
	{
		if (!(m_main_char == null))
		{
			if (update_edit_cy())
			{
				pingyi();
			}
			check_roll();
			update_ex();
			if (m_render.transform.localScale.x > 0.8f)
			{
				float num = m_render.transform.localScale.x - 0.01f;
				m_render.transform.localScale = new Vector3(num, num, 1f);
			}
			else if (m_render.transform.localScale.x < 0.8f)
			{
				m_render.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
			}
		}
	}

	private bool create_cy(int index)
	{
		edit_cy edit_cy2 = m_ecs[index];
		if (edit_cy2.world != m_world)
		{
			return false;
		}
		GameObject original = (GameObject)Resources.Load("unit/other/cy");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(original);
		gameObject.transform.parent = m_other.transform;
		gameObject.transform.localPosition = new Vector3(edit_cy2.p.x / 10, edit_cy2.p.y / 10, 0f);
		if (edit_cy2.fx == 0)
		{
			gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		else
		{
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		gameObject.GetComponent<mario_edit_cy>().reset(edit_cy2);
		int num = edit_cy2.p.x / utils.g_grid_size - m_grid.x;
		int num2 = edit_cy2.p.y / utils.g_grid_size - m_grid.y;
		if (num > utils.g_active_x || num < -utils.g_active_x || num2 > utils.g_active_y || num2 < -utils.g_active_y)
		{
			return false;
		}
		return true;
	}

	private void do_create(int x, int y)
	{
		int item = y * utils.g_max_x + x;
		s_t_unit s_t_unit2 = game_data._instance.get_t_unit(game_data._instance.m_arrays[m_world][y][x].type);
		if (s_t_unit2 != null && !m_has_create.Contains(item))
		{
			create_mario_obj(s_t_unit2.res, s_t_unit2, game_data._instance.m_arrays[m_world][y][x].param, x, y);
		}
	}

	private void do_delete()
	{
		List<mario_obj> list = new List<mario_obj>();
		for (int i = 0; i < m_objs.Count; i++)
		{
			mario_obj mario_obj2 = m_objs[i];
			int num = mario_obj2.m_grid.x - m_grid.x;
			int num2 = mario_obj2.m_grid.y - m_grid.y;
			if (num > utils.g_active_x || num < -utils.g_active_x || num2 > utils.g_active_y || num2 < -utils.g_active_y)
			{
				list.Add(mario_obj2);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j] == m_main_char)
			{
				m_main_char = null;
			}
			del_obj(list[j]);
			list[j].destroy_shadow();
			UnityEngine.Object.Destroy(list[j].gameObject);
		}
	}

	private void Update()
	{
		if (mario._instance.key_down(KeyCode.LeftArrow))
		{
			m_left = true;
		}
		if (mario._instance.key_up(KeyCode.LeftArrow))
		{
			m_left = false;
		}
		if (mario._instance.key_down(KeyCode.RightArrow))
		{
			m_right = true;
		}
		if (mario._instance.key_up(KeyCode.RightArrow))
		{
			m_right = false;
		}
		if (mario._instance.key_down(KeyCode.UpArrow))
		{
			m_up = true;
		}
		if (mario._instance.key_up(KeyCode.UpArrow))
		{
			m_up = false;
		}
		if (mario._instance.key_down(KeyCode.DownArrow))
		{
			m_down = true;
		}
		if (mario._instance.key_up(KeyCode.DownArrow))
		{
			m_down = false;
		}
		m_cam.transform.localPosition = new Vector3(m_roll.x / 10, (m_roll.y + utils.g_roll_y) / 10);
	}

	private void joy_hold(Joystick joy)
	{
		float num = joy.Axis2Angle();
		if ((double)num > -157.5 && (double)num <= -112.5)
		{
			m_up = false;
			m_down = true;
			m_left = true;
			m_right = false;
		}
		else if ((double)num > -112.5 && (double)num <= -67.5)
		{
			m_up = false;
			m_down = false;
			m_left = true;
			m_right = false;
		}
		else if ((double)num > -67.5 && (double)num <= -22.5)
		{
			m_up = true;
			m_down = false;
			m_left = true;
			m_right = false;
		}
		else if ((double)num > -22.5 && (double)num <= 22.5)
		{
			m_up = true;
			m_down = false;
			m_left = false;
			m_right = false;
		}
		else if ((double)num > 22.5 && (double)num <= 67.5)
		{
			m_up = true;
			m_down = false;
			m_left = false;
			m_right = true;
		}
		else if ((double)num > 67.5 && (double)num <= 112.5)
		{
			m_up = false;
			m_down = false;
			m_left = false;
			m_right = true;
		}
		else if ((double)num > 112.5 && (double)num <= 157.5)
		{
			m_up = false;
			m_down = true;
			m_left = false;
			m_right = true;
		}
		else
		{
			m_up = false;
			m_down = true;
			m_left = false;
			m_right = false;
		}
	}

	private void joy_move_end(Joystick joy)
	{
		m_up = false;
		m_down = false;
		m_left = false;
		m_right = false;
	}
}
