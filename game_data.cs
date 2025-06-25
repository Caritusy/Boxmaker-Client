using System;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;
using protocol.game;
using protocol.map;
using UnityEngine;

public class game_data
{
	public const int m_self_map_ver = 2;

	public static game_data m_game_data;

	public string m_ver = "1.214";

	public string m_pt_ver = string.Empty;

	public string m_channel = string.Empty;

	public e_language m_lang;

	public string m_gonggao;

	public s_t_save_data m_save_data = new s_t_save_data();

	public int m_map_id;

	public map_data1 m_map_data;

	public List<List<List<s_t_mission_sub>>> m_arrays = new List<List<List<s_t_mission_sub>>>();

	public List<List<mario_point>> m_die_poses = new List<List<mario_point>>();

	public List<int> m_map_inputs = new List<int>();

	public Dictionary<string, s_t_language> m_t_language = new Dictionary<string, s_t_language>();

	public Dictionary<string, s_t_language> m_t_language_extra = new Dictionary<string, s_t_language>() 
	{
		{ "t_error_29",new s_t_language {id = "t_error_29",zw = "该账号已被封禁",ew = "This user was been banned" } }
	};

	public Dictionary<int, s_t_unit> m_t_unit = new Dictionary<int, s_t_unit>();

	public List<int> m_unit_sites = new List<int>();

	public int m_unit_num;

	public Dictionary<int, string> m_t_error = new Dictionary<int, string>();

	public Dictionary<int, s_t_view_map> m_t_view_map = new Dictionary<int, s_t_view_map>();

	public Dictionary<int, s_t_view_title> m_t_view_title = new Dictionary<int, s_t_view_title>();

	public Dictionary<int, string> m_t_touxiang = new Dictionary<int, string>();

	public Dictionary<string, string> m_t_guojia = new Dictionary<string, string>();

	public Dictionary<int, s_t_exp> m_t_exp = new Dictionary<int, s_t_exp>();

	public Dictionary<int, int> m_t_zm = new Dictionary<int, int>();

	public Dictionary<int, s_t_job_exp> m_t_job_exp = new Dictionary<int, s_t_job_exp>();

	public Dictionary<int, s_t_shop> m_t_shop = new Dictionary<int, s_t_shop>();

	public Dictionary<int, s_t_fg> m_t_fg = new Dictionary<int, s_t_fg>();

	public int m_fg_num;

	public HashSet<int> m_t_map = new HashSet<int>();

	public List<s_t_br> m_t_br = new List<s_t_br>();

	public Dictionary<int, s_t_key> m_t_key = new Dictionary<int, s_t_key>();

	public bool SUPERSCRECTDEBUG_INFKEYS = false;

	public static game_data _instance
	{
		get
		{
			if (m_game_data == null)
			{
				m_game_data = new game_data();
			}
			return m_game_data;
		}
	}

	public void load_native()
	{
		if (PlayerPrefs.HasKey("openid"))
		{
			m_save_data.openid = PlayerPrefs.GetString("openid");
			m_save_data.openkey = PlayerPrefs.GetString("openkey");
		}
		if (PlayerPrefs.HasKey("is_bgm"))
		{
			m_save_data.is_bgm = PlayerPrefs.GetInt("is_bgm");
			m_save_data.is_sound = PlayerPrefs.GetInt("is_sound");
			if (m_save_data.is_bgm == 0)
			{
				mario._instance.disable_bgm();
			}
			if (m_save_data.is_sound == 0)
			{
				mario._instance.disable_sound();
			}
		}
		else
		{
			m_save_data.is_bgm = 1;
			m_save_data.is_sound = 1;
		}
		if (PlayerPrefs.HasKey("is_full"))
		{
			m_save_data.is_full = PlayerPrefs.GetInt("is_full");
		}
		else
		{
			m_save_data.is_full = 0;
		}
		if (PlayerPrefs.HasKey("fbl"))
		{
			m_save_data.fbl = PlayerPrefs.GetInt("fbl");
		}
		else
		{
			m_save_data.fbl = 0;
		}
		mario._instance.change_ff(m_save_data.fbl, m_save_data.is_full);
		m_save_data.keys = new List<int>();
		if (PlayerPrefs.HasKey("up"))
		{
			m_save_data.keys.Add(PlayerPrefs.GetInt("up"));
			m_save_data.keys.Add(PlayerPrefs.GetInt("down"));
			m_save_data.keys.Add(PlayerPrefs.GetInt("left"));
			m_save_data.keys.Add(PlayerPrefs.GetInt("right"));
			m_save_data.keys.Add(PlayerPrefs.GetInt("jump"));
			m_save_data.keys.Add(PlayerPrefs.GetInt("action"));
		}
		else
		{
			for (int i = 0; i < 6; i++)
			{
				m_save_data.keys.Add(0);
			}
		}
	}

	public void save_native()
	{
		PlayerPrefs.SetString("openid", m_save_data.openid);
		PlayerPrefs.SetString("openkey", m_save_data.openkey);
		PlayerPrefs.SetInt("is_bgm", m_save_data.is_bgm);
		PlayerPrefs.SetInt("is_sound", m_save_data.is_sound);
		PlayerPrefs.SetInt("is_full", m_save_data.is_full);
		PlayerPrefs.SetInt("fbl", m_save_data.fbl);
		if (m_save_data.is_bgm == 0)
		{
			mario._instance.disable_bgm();
		}
		else
		{
			mario._instance.enable_bgm();
		}
		if (m_save_data.is_sound == 0)
		{
			mario._instance.disable_sound();
		}
		else
		{
			mario._instance.enable_sound();
		}
		if (m_save_data.is_full == 0 && Screen.fullScreen)
		{
			Screen.fullScreen = false;
		}
		else if (m_save_data.is_full == 1 && !Screen.fullScreen)
		{
			Screen.fullScreen = true;
		}
		mario._instance.change_ff(m_save_data.fbl, m_save_data.is_full);
		PlayerPrefs.SetInt("up", m_save_data.keys[0]);
		PlayerPrefs.SetInt("down", m_save_data.keys[1]);
		PlayerPrefs.SetInt("left", m_save_data.keys[2]);
		PlayerPrefs.SetInt("right", m_save_data.keys[3]);
		PlayerPrefs.SetInt("jump", m_save_data.keys[4]);
		PlayerPrefs.SetInt("action", m_save_data.keys[5]);
		PlayerPrefs.Save();
	}

	public void delete_native()
	{
		m_save_data.openid = string.Empty;
		m_save_data.openkey = string.Empty;
		m_save_data.is_bgm = 1;
		m_save_data.is_sound = 1;
		m_save_data.keys.Clear();
		PlayerPrefs.DeleteAll();
	}

	private void new_world(int index)
	{
		m_die_poses[index].Clear();
		m_arrays[index].Clear();
		map_data_sub map_data_sub = new map_data_sub();
		map_data_sub.x_num = 20;
		map_data_sub.y_num = 10;
		map_data_sub.qd_y = 2;
		map_data_sub.zd_y = 2;
		map_data_sub.map_theme = UnityEngine.Random.Range(1, 5);
		m_map_data.maps.Add(map_data_sub);
		for (int i = 0; i < map_data_sub.y_num; i++)
		{
			List<s_t_mission_sub> list = new List<s_t_mission_sub>();
			for (int j = 0; j < map_data_sub.x_num; j++)
			{
				s_t_mission_sub s_t_mission_sub2 = new s_t_mission_sub();
				if (i < 2)
				{
					s_t_mission_sub2.type = 1;
				}
				else
				{
					s_t_mission_sub2.type = 0;
				}
				list.Add(s_t_mission_sub2);
			}
			m_arrays[index].Add(list);
		}
		m_arrays[index][2][1].type = 1000;
		m_arrays[index][2][18].type = 1001;
	}

	private void reset_mission()
	{
		m_die_poses.Clear();
		m_arrays.Clear();
		m_map_data = new map_data1();
		m_map_data.mode = 0;
		m_map_data.time = 300;
		m_map_data.no_music = 0;
		m_map_data.end_area = 0;
		for (int i = 0; i < 3; i++)
		{
			m_die_poses.Add(new List<mario_point>());
			m_arrays.Add(new List<List<s_t_mission_sub>>());
		}
	}

	private void new_mission()
	{
		reset_mission();
		for (int i = 0; i < 3; i++)
		{
			new_world(i);
		}
	}

	public bool load_mission(int id, byte[] mapdata, List<int> x, List<int> y)
	{
		m_map_id = id;
		reset_mission();
		if (x != null)
		{
			for (int i = 0; i < x.Count; i++)
			{
				int index = x[i] / 10000000;
				m_die_poses[index].Add(new mario_point(x[i], y[i]));
			}
		}
		if (mapdata.Length != 0)
		{
			try
			{
				byte[] bytes = utils.Decompress(mapdata);
				map_data map_data = net_http._instance.parse_packet<map_data>(bytes);
				map_data_sub map_data_sub = new map_data_sub();
				map_data_sub.array = map_data.array;
				map_data_sub.x_num = map_data.x_num;
				map_data_sub.y_num = map_data.y_num;
				map_data_sub.qd_y = map_data.qd_y;
				map_data_sub.zd_y = map_data.zd_y;
				map_data_sub.map_theme = map_data.map_theme;
				m_map_data.map_ver = map_data.map_ver;
				m_map_data.mode = map_data.mode;
				m_map_data.time = map_data.time;
				m_map_data.no_music = map_data.no_music;
				m_map_data.end_area = 0;
				m_map_data.maps.Add(map_data_sub);
			}
			catch (Exception)
			{
				try
				{
					byte[] bytes2 = utils.Decompress(mapdata);
					m_map_data = net_http._instance.parse_packet<map_data1>(bytes2);
				}
				catch (Exception)
				{
					mario._instance.show_tip(get_language_string("game_data_dtjx"));
					new_mission();
					return false;
				}
			}
			try
			{
				if (m_map_data.map_ver > 2)
				{
					mario._instance.show_tip(get_language_string("game_data_dtbb"));
					new_mission();
					return false;
				}
				for (int j = 0; j < 3; j++)
				{
					if (j >= m_map_data.maps.Count)
					{
						new_world(j);
						continue;
					}
					MemoryStream memoryStream = new MemoryStream(m_map_data.maps[j].array);
					byte[] array = new byte[4];
					for (int k = 0; k < m_map_data.maps[j].y_num; k++)
					{
						List<s_t_mission_sub> list = new List<s_t_mission_sub>();
						for (int l = 0; l < m_map_data.maps[j].x_num; l++)
						{
							s_t_mission_sub s_t_mission_sub2 = new s_t_mission_sub();
							memoryStream.Read(array, 0, array.Length);
							s_t_mission_sub2.type = BitConverter.ToInt32(array, 0);
							if (s_t_mission_sub2.type != 0)
							{
								for (int m = 0; m < 4; m++)
								{
									memoryStream.Read(array, 0, array.Length);
									s_t_mission_sub2.param[m] = BitConverter.ToInt32(array, 0);
								}
							}
							s_t_unit s_t_unit2 = get_t_unit(s_t_mission_sub2.type);
							if (s_t_unit2 != null && mario._instance.m_self.m_review == 1 && s_t_unit2.review == 1)
							{
								s_t_mission_sub2.type = 0;
							}
							list.Add(s_t_mission_sub2);
						}
						m_arrays[j].Add(list);
					}
				}
			}
			catch (Exception)
			{
				mario._instance.show_tip(get_language_string("game_data_dtjx"));
				new_mission();
				return false;
			}
			return true;
		}
		new_mission();
		return true;
	}

	public void get_mission_data(ref byte[] data, ref byte[] url)
	{
		m_map_data.map_ver = 2;
		byte[] array = new byte[4];
		MemoryStream memoryStream;
		for (int i = 0; i < 3; i++)
		{
			memoryStream = new MemoryStream();
			for (int j = 0; j < m_map_data.maps[i].y_num; j++)
			{
				for (int k = 0; k < m_map_data.maps[i].x_num; k++)
				{
					array = BitConverter.GetBytes(m_arrays[i][j][k].type);
					memoryStream.Write(array, 0, array.Length);
					for (int l = 0; l < m_arrays[i][j][k].param.Count; l++)
					{
						array = BitConverter.GetBytes(m_arrays[i][j][k].param[l]);
						memoryStream.Write(array, 0, array.Length);
					}
				}
			}
			m_map_data.maps[i].array = memoryStream.ToArray();
		}
		memoryStream = new MemoryStream();
		Serializer.Serialize(memoryStream, m_map_data);
		data = utils.Compress(memoryStream.ToArray());
		map_url map_url = new map_url();
		map_url.map_theme = m_map_data.maps[0].map_theme;
		int num = m_map_data.maps[0].qd_y - 2;
		int num2 = m_map_data.maps[0].qd_y + 8;
		if (num2 > m_map_data.maps[0].y_num)
		{
			num2 = m_map_data.maps[0].y_num;
			num = num2 - 10;
		}
		memoryStream = new MemoryStream();
		for (int m = num; m < num2; m++)
		{
			for (int n = 0; n < 15; n++)
			{
				array = BitConverter.GetBytes(m_arrays[0][m][n].type);
				memoryStream.Write(array, 0, array.Length);
			}
		}
		map_url.array = memoryStream.ToArray();
		memoryStream = new MemoryStream();
		Serializer.Serialize(memoryStream, map_url);
		url = utils.Compress(memoryStream.ToArray());
	}

	public void save_mission()
	{
		byte[] data = null;
		byte[] url = null;
		get_mission_data(ref data, ref url);
		cmsg_save_map cmsg_save_map = new cmsg_save_map();
		cmsg_save_map.id = m_map_id;
		cmsg_save_map.mapdata = data;
		cmsg_save_map.url = url;
		net_http._instance.send_msg(opclient_t.OPCODE_SAVE_MAP, cmsg_save_map, restart: true, string.Empty, 10f);
	}

	public void save_mission_ex()
	{
		byte[] data = null;
		byte[] url = null;
		get_mission_data(ref data, ref url);
		cmsg_complete_guide cmsg_complete_guide = new cmsg_complete_guide();
		cmsg_complete_guide.data = data;
		cmsg_complete_guide.url = url;
		net_http._instance.send_msg(opclient_t.OPCODE_COMPLETE_GUIDE, cmsg_complete_guide, restart: true, string.Empty, 10f);
	}

	public Texture2D mission_to_texture(byte[] data)
	{
		if (data.Length == 0)
		{
			return Resources.Load("texture/back/back") as Texture2D;
		}
		try
		{
			byte[] bytes = utils.Decompress(data);
			map_url map_url = net_http._instance.parse_packet<map_url>(bytes);
			Texture2D texture2D = new Texture2D(360, 240, TextureFormat.RGBA32, mipmap: false);
			Texture2D texture2D2 = Resources.Load("texture/back/back_" + map_url.map_theme) as Texture2D;
			texture2D.SetPixels(0, 0, 360, 240, texture2D2.GetPixels());
			MemoryStream memoryStream = new MemoryStream(map_url.array);
			byte[] array = new byte[4];
			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < 15; j++)
				{
					memoryStream.Read(array, 0, array.Length);
					int num = BitConverter.ToInt32(array, 0);
					if (num <= 0)
					{
						continue;
					}
					s_t_unit s_t_unit2 = _instance.get_t_unit(num);
					if (s_t_unit2 == null || (mario._instance.m_self.m_review == 1 && s_t_unit2.review == 1))
					{
						continue;
					}
					Texture2D texture2D3 = ((s_t_unit2.kfg != 1) ? (Resources.Load("texture/" + s_t_unit2.icon) as Texture2D) : (Resources.Load("texture/" + s_t_unit2.icon + "_" + map_url.map_theme) as Texture2D));
					Color[] pixels = texture2D3.GetPixels();
					for (int k = 0; k < 24; k++)
					{
						for (int l = 0; l < 24; l++)
						{
							Color color = pixels[k * 24 + l];
							Color pixel = texture2D.GetPixel(j * 24 + l, i * 24 + k);
							color.r = color.r * color.a + pixel.r * (1f - color.a);
							color.g = color.g * color.a + pixel.g * (1f - color.a);
							color.b = color.b * color.a + pixel.b * (1f - color.a);
							color.a = 1f;
							texture2D.SetPixel(j * 24 + l, i * 24 + k, color);
						}
					}
				}
			}
			texture2D.Apply(updateMipmaps: false);
			return texture2D;
		}
		catch (Exception)
		{
			return Resources.Load("texture/back/back") as Texture2D;
		}
	}

	public void load_inputs(byte[] video_data)
	{
		m_map_inputs = new List<int>();
		try
		{
			byte[] array = utils.Decompress(video_data);
			int num = array.Length / 4 - 10;
			MemoryStream memoryStream = new MemoryStream(array);
			byte[] array2 = new byte[4];
			for (int i = 0; i < 10; i++)
			{
				memoryStream.Read(array2, 0, array2.Length);
			}
			for (int j = 0; j < num; j++)
			{
				memoryStream.Read(array2, 0, array2.Length);
				int item = BitConverter.ToInt32(array2, 0);
				m_map_inputs.Add(item);
			}
		}
		catch (Exception)
		{
			m_map_inputs.Clear();
		}
	}

	public byte[] save_inputs(List<int> input)
	{
		MemoryStream memoryStream = new MemoryStream();
		for (int i = 0; i < 10; i++)
		{
			byte[] bytes = BitConverter.GetBytes(0);
			memoryStream.Write(bytes, 0, bytes.Length);
		}
		for (int j = 0; j < input.Count; j++)
		{
			byte[] bytes2 = BitConverter.GetBytes(input[j]);
			memoryStream.Write(bytes2, 0, bytes2.Length);
		}
		return utils.Compress(memoryStream.ToArray());
	}

	public string get_map_music(int index)
	{
		s_t_fg s_t_fg2 = get_t_fg(m_map_data.maps[index].map_theme);
		if (s_t_fg2 == null)
		{
			return string.Empty;
		}
		return s_t_fg2.music;
	}

	public bool get_csm(int iw, int ix, int iy, ref int w, ref int x, ref int y)
	{
		int num = m_arrays[iw][iy][ix].param[0];
		int num2 = 1 - m_arrays[iw][iy][ix].param[1];
		for (int i = 0; i < m_arrays.Count; i++)
		{
			for (int j = 0; j < m_map_data.maps[i].y_num; j++)
			{
				for (int k = 0; k < m_map_data.maps[i].x_num; k++)
				{
					if (m_arrays[i][j][k].type == utils.g_csm && m_arrays[i][j][k].param[0] == num && m_arrays[i][j][k].param[1] == num2)
					{
						w = i;
						x = k;
						y = j;
						return true;
					}
				}
			}
		}
		return false;
	}

	public int get_new_csm()
	{
		HashSet<int> hashSet = new HashSet<int>();
		for (int i = 0; i < m_arrays.Count; i++)
		{
			for (int j = 0; j < m_map_data.maps[i].y_num; j++)
			{
				for (int k = 0; k < m_map_data.maps[i].x_num; k++)
				{
					if (m_arrays[i][j][k].type == utils.g_csm)
					{
						hashSet.Add(m_arrays[i][j][k].param[0]);
					}
				}
			}
		}
		for (int l = 0; l < 15; l++)
		{
			if (!hashSet.Contains(l))
			{
				return l;
			}
		}
		return -1;
	}

	public int get_unit_num(int world, int type)
	{
		int num = 0;
		for (int i = 0; i < m_map_data.maps[world].y_num; i++)
		{
			for (int j = 0; j < m_map_data.maps[world].x_num; j++)
			{
				if (m_arrays[world][i][j].type == type)
				{
					num++;
				}
			}
		}
		return num;
	}

	public string get_language_string(string id)
	{
		if (!m_t_language.ContainsKey(id))
		{
			return string.Empty;
		}
		if (m_lang == e_language.el_chinese)
		{
			if (id == "login_gui_shez")
			{
				return "官方群信息";
			}
			return m_t_language[id].zw;
		}
		return m_t_language[id].ew;
	}

	public s_t_unit get_t_unit(int id)
	{
		if (!m_t_unit.ContainsKey(id))
		{
			return null;
		}
		return m_t_unit[id];
	}

	public s_t_unit get_t_unit_by_site(int site)
	{
		if (site < 0 || site >= m_unit_sites.Count)
		{
			return null;
		}
		int key = m_unit_sites[site];
		if (!m_t_unit.ContainsKey(key))
		{
			return null;
		}
		return m_t_unit[key];
	}

	public string get_t_error(int id)
	{
		if (!m_t_error.ContainsKey(id))
		{
			return string.Empty;
		}
		return m_t_error[id];
	}

	public s_t_view_map get_t_view_map(int id)
	{
		if (!m_t_view_map.ContainsKey(id))
		{
			return null;
		}
		return m_t_view_map[id];
	}

	public s_t_view_title get_t_view_title(int id)
	{
		if (!m_t_view_title.ContainsKey(id))
		{
			return null;
		}
		return m_t_view_title[id];
	}

	public string get_t_touxiang(int id)
	{
		if (!m_t_touxiang.ContainsKey(id))
		{
			return string.Empty;
		}
		return m_t_touxiang[id];
	}

	public string get_t_guojia(string code)
	{
		if (!m_t_guojia.ContainsKey(code))
		{
			return "gq_000";
		}
		return m_t_guojia[code];
	}

	public s_t_exp get_t_exp(int level)
	{
		if (!m_t_exp.ContainsKey(level))
		{
			return null;
		}
		return m_t_exp[level];
	}

	public int get_zm(int zm)
	{
		if (!m_t_zm.ContainsKey(zm))
		{
			return 0;
		}
		return m_t_zm[zm];
	}

	public s_t_job_exp get_t_job_exp(int level)
	{
		if (!m_t_job_exp.ContainsKey(level))
		{
			return null;
		}
		return m_t_job_exp[level];
	}

	public s_t_shop get_t_shop(int id)
	{
		if (!m_t_shop.ContainsKey(id))
		{
			return null;
		}
		return m_t_shop[id];
	}

	public s_t_fg get_t_fg(int id)
	{
		if (!m_t_fg.ContainsKey(id))
		{
			return null;
		}
		return m_t_fg[id];
	}

	public bool is_test_map(int id)
	{
		return m_t_map.Contains(id);
	}

	public s_t_br get_t_br(int id)
	{
		if (id <= 0)
		{
			return null;
		}
		if (id > m_t_br.Count)
		{
			return m_t_br[m_t_br.Count - 1];
		}
		return m_t_br[id - 1];
	}

	public s_t_key get_t_key(int kc)
	{
		if (!m_t_key.ContainsKey(kc))
		{
			return null;
		}
		return m_t_key[kc];
	}

	public void init()
	{
		load_native();
		dbc dbc2 = new dbc();
		dbc2.load_txt("t_language");
		for (int i = 0; i < dbc2.get_y(); i++)
		{
			s_t_language s_t_language2 = new s_t_language();
			s_t_language2.id = dbc2.get(0, i);
			s_t_language2.zw = dbc2.get(1, i);
			s_t_language2.ew = dbc2.get(2, i);
			m_t_language[s_t_language2.id] = s_t_language2;
		}
		dbc2.load_txt("t_unit");
		for (int j = 0; j < dbc2.get_y(); j++)
		{
			s_t_unit s_t_unit2 = new s_t_unit();
			s_t_unit2.id = int.Parse(dbc2.get(0, j));
			s_t_unit2.name = dbc2.get(1, j);
			s_t_unit2.icon = dbc2.get(2, j);
			s_t_unit2.res = dbc2.get(3, j);
			s_t_unit2.yc = 0;
			s_t_unit2.kfg = int.Parse(dbc2.get(5, j));
			s_t_unit2.is_static = int.Parse(dbc2.get(6, j));
			s_t_unit2.fwt = int.Parse(dbc2.get(7, j));
			s_t_unit2.review = int.Parse(dbc2.get(8, j));
			s_t_unit2.is_sw = int.Parse(dbc2.get(9, j));
			s_t_unit2.max_num = int.Parse(dbc2.get(10, j));
			m_t_unit[s_t_unit2.id] = s_t_unit2;

			if (s_t_unit2.id == 204)
			{
				//mario._instance.show_tip("加载204了"); 
			}
			if (s_t_unit2.yc == 0)
			{
				m_unit_sites.Add(s_t_unit2.id);
				m_unit_num++;
			}
		}
		dbc2.load_txt("t_error");
		for (int k = 0; k < dbc2.get_y(); k++)
		{
			int key = int.Parse(dbc2.get(0, k));
			string value = get_language_string(dbc2.get(1, k));
			m_t_error[key] = value;
		}
		dbc2.load_txt("t_view_map");
		for (int l = 0; l < dbc2.get_y(); l++)
		{
			s_t_view_map s_t_view_map2 = new s_t_view_map();
			s_t_view_map2.id = int.Parse(dbc2.get(0, l));
			s_t_view_map2.name = get_language_string(dbc2.get(1, l));
			s_t_view_map2.icon = dbc2.get(2, l);
			m_t_view_map[s_t_view_map2.id] = s_t_view_map2;
		}
		dbc2.load_txt("t_view_title");
		for (int m = 0; m < dbc2.get_y(); m++)
		{
			s_t_view_title s_t_view_title2 = new s_t_view_title();
			s_t_view_title2.id = int.Parse(dbc2.get(0, m));
			s_t_view_title2.name = get_language_string(dbc2.get(1, m));
			s_t_view_title2.icon = dbc2.get(2, m);
			m_t_view_title[s_t_view_title2.id] = s_t_view_title2;
		}
		dbc2.load_txt("t_touxiang");
		for (int n = 0; n < dbc2.get_y(); n++)
		{
			int key2 = int.Parse(dbc2.get(0, n));
			string value2 = dbc2.get(1, n);
			m_t_touxiang[key2] = value2;
		}
		dbc2.load_txt("t_guojia");
		for (int num = 0; num < dbc2.get_y(); num++)
		{
			string key3 = dbc2.get(0, num);
			string value3 = dbc2.get(2, num);
			m_t_guojia[key3] = value3;
		}
		dbc2.load_txt("t_exp");
		int num2 = 0;
		for (int num3 = 0; num3 < dbc2.get_y(); num3++)
		{
			s_t_exp s_t_exp2 = new s_t_exp();
			s_t_exp2.level = int.Parse(dbc2.get(0, num3));
			s_t_exp2.exp = int.Parse(dbc2.get(1, num3));
			s_t_exp2.zm = int.Parse(dbc2.get(2, num3));
			s_t_exp2.icon = dbc2.get(3, num3);
			s_t_exp2.max_exp = int.Parse(dbc2.get(4, num3));
			m_t_exp[s_t_exp2.level] = s_t_exp2;
			if (s_t_exp2.zm != num2)
			{
				num2 = s_t_exp2.zm;
				m_t_zm[num2] = s_t_exp2.level;
			}
		}
		dbc2.load_txt("t_job_exp");
		for (int num4 = 0; num4 < dbc2.get_y(); num4++)
		{
			s_t_job_exp s_t_job_exp2 = new s_t_job_exp();
			s_t_job_exp2.level = int.Parse(dbc2.get(0, num4));
			s_t_job_exp2.exp = int.Parse(dbc2.get(1, num4));
			m_t_job_exp[s_t_job_exp2.level] = s_t_job_exp2;
		}
		dbc2.load_txt("t_shop");
		for (int num5 = 0; num5 < dbc2.get_y(); num5++)
		{
			s_t_shop s_t_shop2 = new s_t_shop();
			s_t_shop2.id = int.Parse(dbc2.get(0, num5));
			s_t_shop2.slot = int.Parse(dbc2.get(1, num5));
			s_t_shop2.name = get_language_string(dbc2.get(2, num5));
			s_t_shop2.type = int.Parse(dbc2.get(3, num5));
			s_t_shop2.price = int.Parse(dbc2.get(4, num5));
			s_t_shop2.price_my = float.Parse(dbc2.get(5, num5));
			s_t_shop2.icon = dbc2.get(6, num5);
			s_t_shop2.db = dbc2.get(7, num5);
			s_t_shop2.def = int.Parse(dbc2.get(8, num5));
			s_t_shop2.code = dbc2.get(9, num5);
			s_t_shop2.desc = get_language_string(dbc2.get(10, num5));
			m_t_shop[s_t_shop2.id] = s_t_shop2;
		}
		dbc2.load_txt("t_fg");
		for (int num6 = 0; num6 < dbc2.get_y(); num6++)
		{
			s_t_fg s_t_fg2 = new s_t_fg();
			s_t_fg2.id = int.Parse(dbc2.get(0, num6));
			s_t_fg2.name = get_language_string(dbc2.get(1, num6));
			s_t_fg2.tj = int.Parse(dbc2.get(2, num6));
			s_t_fg2.desc = get_language_string(dbc2.get(3, num6));
			s_t_fg2.music = dbc2.get(4, num6);
			m_t_fg[s_t_fg2.id] = s_t_fg2;
			m_fg_num++;
		}
		dbc2.load_txt("t_map");
		for (int num7 = 0; num7 < dbc2.get_y(); num7++)
		{
			int item = int.Parse(dbc2.get(0, num7));
			m_t_map.Add(item);
		}
		dbc2.load_txt("t_br");
		for (int num8 = 0; num8 < dbc2.get_y(); num8++)
		{
			s_t_br s_t_br2 = new s_t_br();
			s_t_br2.id = int.Parse(dbc2.get(0, num8));
			s_t_br2.name = get_language_string(dbc2.get(2, num8));
			s_t_br2.num = int.Parse(dbc2.get(3, num8));
			s_t_br2.desc = get_language_string(dbc2.get(4, num8));
			s_t_br2.unlock = get_language_string(dbc2.get(6, num8));
			m_t_br.Add(s_t_br2);
		}
		dbc2.load_txt("t_key");
		for (int num9 = 0; num9 < dbc2.get_y(); num9++)
		{
			s_t_key s_t_key2 = new s_t_key();
			s_t_key2.code = int.Parse(dbc2.get(0, num9));
			s_t_key2.name = dbc2.get(1, num9);
			m_t_key.Add(s_t_key2.code, s_t_key2);
		}
		new_mission();
		if (Application.isEditor)
		{
			m_channel = string.Empty;
		}
		else
		{
			m_channel = string.Empty;
		}
		init_pt_ver();
	}

	public void init_pt_ver()
	{
		m_pt_ver = m_channel + "_" + m_ver;
		mario._instance.ginit_callbak();
	}
}
