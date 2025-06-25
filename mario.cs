using System.Collections.Generic;
using protocol.game;
using UnityEngine;

public class mario : MonoBehaviour
{
	public delegate void ChangeStateHandle();

	public static mario _instance;

	public GameObject m_uiroot;

	public Camera m_ui_camera;

	public Font m_efont;

	public player m_self;

	private List<AudioSource> m_play_sounds = new List<AudioSource>();

	private int m_play_sound_index;

	private AudioSource m_play_sound_ex;

	private AudioSource m_play_sound_ex1;

	private bool m_enable_sound = true;

	public AudioSource m_play_mus;

	private string m_mus_name;

	private bool m_loop_mus = true;

	private bool m_stop_mus;

	private float m_pitch_mus = 1f;

	private float m_vols = 1f;

	public int m_width = 960;

	public int m_height = 640;

	public int m_start_type;

	public Texture m_mouse;

	private float m_heng;

	private float m_per_heng;

	private float m_shu;

	private float m_per_shu;

	public e_game_state m_game_state;

	public GameObject m_main;

	public GameObject m_other;

	private GameObject m_wait_gui;

	private GameObject m_single_dialog_box;

	private GameObject m_xsjx_dialog_box;

	private GameObject m_double_dialog_box;

	private GameObject m_tip_gui;

	private GameObject m_play_gui;

	private GameObject m_user_gui;

	private GameObject m_mask_gui;

	private GameObject m_play_mask_gui;

	private GameObject m_player_gui;

	private GameObject m_clear_gui;

	private GameObject m_road_gui;

	private GameObject m_start_gui;

	private GameObject m_br_end_gui;

	private GameObject m_paihang_gui;

	private GameObject m_shop_gui;

	private GameObject m_gameload_gui;

	private GameObject m_loading_gui;

	private GameObject m_login_gui;

	private GameObject m_edit_gui;

	private GameObject m_play_select_gui;

	private GameObject m_edit_select_gui;

	private void Awake()
	{
		_instance = this;
	}

	private void Start()
	{
		Screen.showCursor = false;
		Application.targetFrameRate = 120;
		if (!utils.IsPC)
		{
            m_width = Screen.width * m_height / Screen.height;
            Screen.SetResolution(m_width, 640, true);
        }
		game_data._instance.init();
	}

	public void change_ff(int id, int full)
	{
		if (utils.IsPC)
		{
			if (id == 0)
			{
				m_width = 960;
			}
			else
			{
				m_width = 1136;
			}
			if (full == 0)
			{
				Screen.SetResolution(m_width, 640, fullscreen: false);
			}
			else
			{
				Screen.SetResolution(m_width, 640, fullscreen: true);
			}
		}
	}

	public void ginit_callbak()
	{
		init_ui();
		if (0 == 0)
		{
			change_state(e_game_state.egs_loading, 0, null);
		}
	}

	public void remove_child(GameObject obj)
	{
		if (!(obj == null))
		{
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < obj.transform.childCount; i++)
			{
				list.Add(obj.transform.GetChild(i).gameObject);
			}
			for (int j = 0; j < list.Count; j++)
			{
				Object.Destroy(list[j]);
			}
			list.Clear();
		}
	}

	public bool get_mouse_button()
	{
		if (Input.touchCount == 1)
		{
			return true;
		}
		return Input.GetMouseButton(0);
	}

	public Vector2 get_mouse_position()
	{
		Vector2 vector = default(Vector3);
		vector = ((Input.touchCount <= 0) ? ((Vector2)Input.mousePosition) : Input.GetTouch(0).position);
		float num = m_uiroot.GetComponent<UIRoot>().activeHeight;
		vector.x *= num / (float)Screen.height;
		vector.y *= num / (float)Screen.height;
		return vector;
	}

	private void init_ui()
	{
		GameObject original = Resources.Load("ui/wait_gui") as GameObject;
		m_wait_gui = (GameObject)Object.Instantiate(original);
		m_wait_gui.transform.parent = m_other.transform;
		m_wait_gui.transform.localPosition = new Vector3(0f, 0f, 0f);
		m_wait_gui.transform.localScale = new Vector3(1f, 1f, 1f);
		original = Resources.Load("ui/single_dialog_box") as GameObject;
		m_single_dialog_box = (GameObject)Object.Instantiate(original);
		m_single_dialog_box.transform.parent = m_other.transform;
		m_single_dialog_box.transform.localPosition = new Vector3(0f, 0f, 0f);
		m_single_dialog_box.transform.localScale = new Vector3(1f, 1f, 1f);
		original = Resources.Load("ui/xsjx_dialog_box") as GameObject;
		m_xsjx_dialog_box = (GameObject)Object.Instantiate(original);
		m_xsjx_dialog_box.transform.parent = m_other.transform;
		m_xsjx_dialog_box.transform.localPosition = new Vector3(0f, 0f, 0f);
		m_xsjx_dialog_box.transform.localScale = new Vector3(1f, 1f, 1f);
		original = Resources.Load("ui/double_dialog_box") as GameObject;
		m_double_dialog_box = (GameObject)Object.Instantiate(original);
		m_double_dialog_box.transform.parent = m_other.transform;
		m_double_dialog_box.transform.localPosition = new Vector3(0f, 0f, 0f);
		m_double_dialog_box.transform.localScale = new Vector3(1f, 1f, 1f);
		original = Resources.Load("ui/tip_gui") as GameObject;
		m_tip_gui = (GameObject)Object.Instantiate(original);
		m_tip_gui.transform.parent = m_other.transform;
		m_tip_gui.transform.localPosition = new Vector3(0f, 0f, 0f);
		m_tip_gui.transform.localScale = new Vector3(1f, 1f, 1f);
		original = Resources.Load("ui/play_gui") as GameObject;
		m_play_gui = (GameObject)Object.Instantiate(original);
		m_play_gui.transform.parent = m_main.transform;
		m_play_gui.transform.localPosition = new Vector3(0f, 0f, 0f);
		m_play_gui.transform.localScale = new Vector3(1f, 1f, 1f);
		original = Resources.Load("ui/user_gui") as GameObject;
		m_user_gui = (GameObject)Object.Instantiate(original);
		m_user_gui.transform.parent = m_main.transform;
		m_user_gui.transform.localPosition = new Vector3(0f, 0f, 0f);
		m_user_gui.transform.localScale = new Vector3(1f, 1f, 1f);
		original = Resources.Load("ui/mask_gui") as GameObject;
		m_mask_gui = (GameObject)Object.Instantiate(original);
		m_mask_gui.transform.parent = m_other.transform;
		m_mask_gui.transform.localPosition = new Vector3(0f, 0f, 0f);
		m_mask_gui.transform.localScale = new Vector3(1f, 1f, 1f);
		original = Resources.Load("ui/play_mask_gui") as GameObject;
		m_play_mask_gui = (GameObject)Object.Instantiate(original);
		m_play_mask_gui.transform.parent = m_other.transform;
		m_play_mask_gui.transform.localPosition = new Vector3(0f, 0f, 0f);
		m_play_mask_gui.transform.localScale = new Vector3(1f, 1f, 1f);
		original = Resources.Load("ui/player_gui") as GameObject;
		m_player_gui = (GameObject)Object.Instantiate(original);
		m_player_gui.transform.parent = m_main.transform;
		m_player_gui.transform.localPosition = new Vector3(0f, 0f, 0f);
		m_player_gui.transform.localScale = new Vector3(1f, 1f, 1f);
		original = Resources.Load("ui/clear_gui") as GameObject;
		m_clear_gui = (GameObject)Object.Instantiate(original);
		m_clear_gui.transform.parent = m_other.transform;
		m_clear_gui.transform.localPosition = new Vector3(0f, 0f, 0f);
		m_clear_gui.transform.localScale = new Vector3(1f, 1f, 1f);
		original = Resources.Load("ui/road_gui") as GameObject;
		m_road_gui = (GameObject)Object.Instantiate(original);
		m_road_gui.transform.parent = m_other.transform;
		m_road_gui.transform.localPosition = new Vector3(0f, 0f, 0f);
		m_road_gui.transform.localScale = new Vector3(1f, 1f, 1f);
		original = Resources.Load("ui/start_gui") as GameObject;
		m_start_gui = (GameObject)Object.Instantiate(original);
		m_start_gui.transform.parent = m_other.transform;
		m_start_gui.transform.localPosition = new Vector3(0f, 0f, 0f);
		m_start_gui.transform.localScale = new Vector3(1f, 1f, 1f);
		original = Resources.Load("ui/br_end_gui") as GameObject;
		m_br_end_gui = (GameObject)Object.Instantiate(original);
		m_br_end_gui.transform.parent = m_other.transform;
		m_br_end_gui.transform.localPosition = new Vector3(0f, 0f, 0f);
		m_br_end_gui.transform.localScale = new Vector3(1f, 1f, 1f);
		original = Resources.Load("ui/paihang_gui") as GameObject;
		m_paihang_gui = (GameObject)Object.Instantiate(original);
		m_paihang_gui.transform.parent = m_other.transform;
		m_paihang_gui.transform.localPosition = new Vector3(0f, 0f, 0f);
		m_paihang_gui.transform.localScale = new Vector3(1f, 1f, 1f);
		original = Resources.Load("ui/shop_gui") as GameObject;
		m_shop_gui = (GameObject)Object.Instantiate(original);
		m_shop_gui.transform.parent = m_other.transform;
		m_shop_gui.transform.localPosition = new Vector3(0f, 0f, 0f);
		m_shop_gui.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	public void change_state(e_game_state state, int type, ChangeStateHandle handle, List<int> param = null)
	{
		m_game_state = state;
		if (m_game_state != e_game_state.egs_gameload)
		{
			if (m_game_state == e_game_state.egs_loading || m_game_state == e_game_state.egs_login)
			{
				play_mus("music/login");
			}
			else if (m_game_state == e_game_state.egs_edit_select || m_game_state == e_game_state.egs_play_select)
			{
				play_mus("music/select");
			}
			else if (m_game_state == e_game_state.egs_edit)
			{
				play_mus("music/select", loop: true, 0.5f);
			}
			else if (m_game_state == e_game_state.egs_br_road || m_game_state == e_game_state.egs_br_end)
			{
				play_mus("music/road");
			}
			else if (m_game_state == e_game_state.egs_br_start)
			{
				stop_mus();
			}
			else
			{
				play_mus(game_data._instance.get_map_music(0), loop: true, 1 - game_data._instance.m_map_data.no_music);
			}
		}
		switch (type)
		{
		case 0:
			if (handle != null)
			{
				handle();
			}
			end_change_state();
			break;
		case 1:
			show_mask(delegate
			{
				handle();
				end_change_state();
			});
			break;
		case 2:
			show_play_mask(delegate
			{
				handle();
				end_change_state();
			});
			break;
		case 3:
			show_mask(delegate
			{
				handle();
				end_change_state();
			}, param[0], param[1]);
			break;
		}
	}

	private void end_change_state()
	{
		m_user_gui.SetActive(value: false);
		if (m_game_state == e_game_state.egs_gameload)
		{
			if (m_gameload_gui == null)
			{
				GameObject original = Resources.Load("ui/gameload_gui") as GameObject;
				m_gameload_gui = (GameObject)Object.Instantiate(original);
				m_gameload_gui.transform.parent = m_main.transform;
				m_gameload_gui.transform.localPosition = new Vector3(0f, 0f, 0f);
				m_gameload_gui.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			else
			{
				m_gameload_gui.SetActive(value: true);
			}
		}
		else if (m_game_state == e_game_state.egs_loading)
		{
			if (m_loading_gui == null)
			{
				GameObject original2 = Resources.Load("ui/loading_gui") as GameObject;
				m_loading_gui = (GameObject)Object.Instantiate(original2);
				m_loading_gui.transform.parent = m_main.transform;
				m_loading_gui.transform.localPosition = new Vector3(0f, 0f, 0f);
				m_loading_gui.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			else
			{
				m_loading_gui.SetActive(value: true);
			}
		}
		else if (m_game_state == e_game_state.egs_login)
		{
			if (m_login_gui == null)
			{
				GameObject original3 = Resources.Load("ui/login_gui") as GameObject;
				m_login_gui = (GameObject)Object.Instantiate(original3);
				m_login_gui.transform.parent = m_main.transform;
				m_login_gui.transform.localPosition = new Vector3(0f, 0f, 0f);
				m_login_gui.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			else
			{
				m_login_gui.SetActive(value: true);
			}
		}
		else if (m_game_state == e_game_state.egs_edit_select)
		{
			if (m_edit_select_gui == null)
			{
				GameObject original4 = Resources.Load("ui/edit_select_gui") as GameObject;
				m_edit_select_gui = (GameObject)Object.Instantiate(original4);
				m_edit_select_gui.transform.parent = m_main.transform;
				m_edit_select_gui.transform.localPosition = new Vector3(0f, 0f, 0f);
				m_edit_select_gui.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			else
			{
				m_edit_select_gui.SetActive(value: true);
			}
			show_user(m_edit_select_gui);
			if (_instance.m_self.guide < 100)
			{
				m_edit_select_gui.GetComponent<edit_select_gui>().guide();
			}
		}
		else if (m_game_state == e_game_state.egs_edit)
		{
			if (m_edit_gui == null)
			{
				GameObject original5 = Resources.Load("ui/edit_gui") as GameObject;
				m_edit_gui = (GameObject)Object.Instantiate(original5);
				m_edit_gui.transform.parent = m_main.transform;
				m_edit_gui.transform.localPosition = new Vector3(0f, 0f, 0f);
				m_edit_gui.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			else
			{
				m_edit_gui.SetActive(value: true);
			}
		}
		else if (m_game_state == e_game_state.egs_play_select)
		{
			if (m_play_select_gui == null)
			{
				GameObject original6 = Resources.Load("ui/play_select_gui") as GameObject;
				m_play_select_gui = (GameObject)Object.Instantiate(original6);
				m_play_select_gui.transform.parent = m_main.transform;
				m_play_select_gui.transform.localPosition = new Vector3(0f, 0f, 0f);
				m_play_select_gui.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			else
			{
				m_play_select_gui.SetActive(value: true);
			}
			m_paihang_gui.GetComponent<paihang_gui>().check();
			show_user(m_play_select_gui);
		}
		else if (m_game_state == e_game_state.egs_play)
		{
			m_play_gui.SetActive(value: true);
			s_message s_message2 = new s_message();
			s_message2.m_type = "play_mode";
			s_message2.m_object.Add(null);
			cmessage_center._instance.add_message(s_message2);
		}
		else if (m_game_state == e_game_state.egs_edit_play)
		{
			m_play_gui.SetActive(value: true);
			s_message s_message3 = new s_message();
			s_message3.m_type = "play_mode";
			s_message3.m_object.Add(null);
			cmessage_center._instance.add_message(s_message3);
		}
		else if (m_game_state == e_game_state.egs_edit_upload)
		{
			m_play_gui.SetActive(value: true);
			s_message s_message4 = new s_message();
			s_message4.m_type = "play_mode";
			s_message4.m_object.Add(null);
			cmessage_center._instance.add_message(s_message4);
		}
		else if (m_game_state == e_game_state.egs_review)
		{
			m_play_gui.SetActive(value: true);
			s_message s_message5 = new s_message();
			s_message5.m_type = "play_mode";
			s_message5.m_object.Add(null);
			s_message5.m_ints.Add(0);
			s_message5.m_ints.Add(1);
			cmessage_center._instance.add_message(s_message5);
		}
		else if (m_game_state == e_game_state.egs_br_play)
		{
			m_play_gui.SetActive(value: true);
			s_message s_message6 = new s_message();
			s_message6.m_type = "play_mode";
			s_message6.m_object.Add(null);
			cmessage_center._instance.add_message(s_message6);
		}
		else if (m_game_state == e_game_state.egs_br_road)
		{
			show_road_gui();
		}
		else if (m_game_state == e_game_state.egs_br_start)
		{
			show_start_gui();
		}
		else if (m_game_state == e_game_state.egs_br_end)
		{
			show_br_end();
		}
	}

	public void wait(bool flag, string text = "")
	{
		m_wait_gui.GetComponent<wait_gui>().reset(flag, text);
	}

	public void show_single_dialog_box(string text, s_message message)
	{
		m_single_dialog_box.GetComponent<single_dialog_box>().reset(text, message);
	}

	public void show_double_dialog_box(string text, s_message message, s_message message_cencel = null)
	{
		m_double_dialog_box.GetComponent<double_dialog_box>().reset(text, message, message_cencel);
	}

	public void show_xsjx_dialog_box(string text, s_message message)
	{
		m_xsjx_dialog_box.GetComponent<single_dialog_box>().reset(text, message);
	}

	public void show_tip(string text)
	{
		m_tip_gui.GetComponent<tip_gui>().add_text(text);
	}

	public void show_user(GameObject obj)
	{
		m_user_gui.SetActive(value: true);
		m_user_gui.GetComponent<user_gui>().change_event(obj);
	}

	public void show_mask(ChangeStateHandle handle)
	{
		m_mask_gui.GetComponent<mask_gui>().reset(handle);
	}

	public void show_mask(ChangeStateHandle handle, int x, int y)
	{
		m_mask_gui.GetComponent<mask_gui>().reset(handle, 1, x, y);
	}

	public void show_play_mask(ChangeStateHandle handle)
	{
		m_play_mask_gui.GetComponent<play_mask_gui>().reset(handle);
	}

	public void show_play_gui()
	{
		m_play_gui.SetActive(value: true);
	}

	public void hide_play_gui()
	{
		m_play_gui.SetActive(value: false);
	}

	public void show_paihang_gui(smsg_view_map_point_rank msg, int id)
	{
		m_paihang_gui.SetActive(value: true);
		m_paihang_gui.GetComponent<paihang_gui>().reset(msg, id);
	}

	public void hide_paihang_gui()
	{
		m_paihang_gui.GetComponent<ui_show_anim>().hide_ui();
	}

	public void show_shop_gui(int type)
	{
		m_shop_gui.GetComponent<shop_gui>().reset(type);
	}

	public void show_player_gui(smsg_view_player msg)
	{
		m_player_gui.SetActive(value: true);
		m_player_gui.GetComponent<player_gui>().reset(msg);
	}

	public void show_clear_gui(bool next)
	{
		m_clear_gui.GetComponent<clear_gui>().reset(1, 0, 0, 0, 0, next);
	}

	public void show_clear_gui(int exp, int eexp, int rank, int testify, bool next)
	{
		m_clear_gui.GetComponent<clear_gui>().reset(0, exp, eexp, rank, testify, next);
	}

	public void hide_clear_gui()
	{
		m_clear_gui.SetActive(value: false);
	}

	public void show_road_gui()
	{
		m_road_gui.SetActive(value: true);
	}

	public void show_start_gui()
	{
		m_start_gui.SetActive(value: true);
	}

	public void show_br_end()
	{
		m_br_end_gui.SetActive(value: true);
	}

	public void play_mus(string name, bool loop = true, float vols = 1f, float pitch = 1f)
	{
		if (name != m_mus_name)
		{
			m_stop_mus = true;
		}
		else
		{
			m_play_mus.loop = loop;
			m_play_mus.volume = vols;
		}
		m_mus_name = name;
		m_loop_mus = loop;
		m_vols = vols;
		m_pitch_mus = pitch;
		if (m_play_mus.pitch == 0f)
		{
			m_play_mus.volume = 0f;
		}
	}

	public void play_mus_ex(string name, bool loop = true, float vols = 1f, float pitch = 1f)
	{
		m_stop_mus = true;
		m_mus_name = name;
		m_loop_mus = loop;
		m_vols = vols;
		m_pitch_mus = pitch;
		if (m_play_mus.pitch == 0f)
		{
			m_play_mus.volume = 0f;
		}
	}

	public void play_mus_ex1(string name, bool loop = true, float vols = 1f, float pitch = 1f)
	{
		AudioClip audioClip = Resources.Load(name) as AudioClip;
		if (!(audioClip == null))
		{
			m_stop_mus = false;
			m_mus_name = name;
			m_loop_mus = loop;
			m_vols = vols;
			m_pitch_mus = pitch;
			m_play_mus.clip = audioClip;
			m_play_mus.loop = m_loop_mus;
			m_play_mus.pitch = m_pitch_mus;
			m_play_mus.volume = m_vols;
			m_play_mus.Play();
		}
	}

	public void stop_mus()
	{
		m_stop_mus = true;
		m_mus_name = string.Empty;
	}

	public void pause_mus()
	{
		m_play_mus.Pause();
	}

	public void continue_mus(float speed)
	{
		m_pitch_mus = speed;
		m_play_mus.pitch = m_pitch_mus;
		m_play_mus.Play();
	}

	public void enable_bgm()
	{
		m_play_mus.enabled = true;
	}

	public void disable_bgm()
	{
		m_play_mus.enabled = false;
	}

	public void enable_sound()
	{
		m_enable_sound = true;
	}

	public void disable_sound()
	{
		m_enable_sound = false;
	}

	public void play_sound(string name, float vols = 1f)
	{
		if (!m_enable_sound)
		{
			return;
		}
		AudioClip audioClip = Resources.Load(name) as AudioClip;
		if (!(audioClip == null))
		{
			if (m_play_sounds.Count < m_play_sound_index + 1)
			{
				m_play_sounds.Add(base.transform.gameObject.AddComponent<AudioSource>());
			}
			m_play_sounds[m_play_sound_index].clip = audioClip;
			m_play_sounds[m_play_sound_index].volume = vols;
			m_play_sounds[m_play_sound_index].Play();
			m_play_sound_index++;
			if (m_play_sound_index >= 5)
			{
				m_play_sound_index = 0;
			}
		}
	}

	public void play_sound_ex(string name, float vols = 1f)
	{
		if (!m_enable_sound)
		{
			return;
		}
		AudioClip audioClip = Resources.Load(name) as AudioClip;
		if (!(audioClip == null))
		{
			if (m_play_sound_ex == null)
			{
				m_play_sound_ex = base.transform.gameObject.AddComponent<AudioSource>();
			}
			m_play_sound_ex.clip = audioClip;
			m_play_sound_ex.volume = vols;
			m_play_sound_ex.Play();
		}
	}

	public void play_sound_ex1(string name, float vols = 1f)
	{
		if (!m_enable_sound)
		{
			return;
		}
		AudioClip audioClip = Resources.Load(name) as AudioClip;
		if (!(audioClip == null))
		{
			if (m_play_sound_ex1 == null)
			{
				m_play_sound_ex1 = base.transform.gameObject.AddComponent<AudioSource>();
			}
			m_play_sound_ex1.clip = audioClip;
			m_play_sound_ex1.volume = vols;
			m_play_sound_ex1.Play();
		}
	}

	public KeyCode find_key(KeyCode kc)
	{
		KeyCode result = kc;
		if (kc == KeyCode.UpArrow && game_data._instance.m_save_data.keys[0] != 0)
		{
			result = (KeyCode)game_data._instance.m_save_data.keys[0];
		}
		else if (kc == KeyCode.DownArrow && game_data._instance.m_save_data.keys[1] != 0)
		{
			result = (KeyCode)game_data._instance.m_save_data.keys[1];
		}
		else if (kc == KeyCode.LeftArrow && game_data._instance.m_save_data.keys[2] != 0)
		{
			result = (KeyCode)game_data._instance.m_save_data.keys[2];
		}
		else if (kc == KeyCode.RightArrow && game_data._instance.m_save_data.keys[3] != 0)
		{
			result = (KeyCode)game_data._instance.m_save_data.keys[3];
		}
		else if (kc == KeyCode.Z && game_data._instance.m_save_data.keys[4] != 0)
		{
			result = (KeyCode)game_data._instance.m_save_data.keys[4];
		}
		else if (kc == KeyCode.X && game_data._instance.m_save_data.keys[5] != 0)
		{
			result = (KeyCode)game_data._instance.m_save_data.keys[5];
		}
		return result;
	}

	public bool key_down(KeyCode kc)
	{
		switch (kc)
		{
		case KeyCode.LeftArrow:
			if (Input.GetKeyDown(_instance.find_key(KeyCode.LeftArrow)))
			{
				return true;
			}
			if (m_per_heng > -0.1f && m_heng < -0.1f)
			{
				return true;
			}
			break;
		case KeyCode.RightArrow:
			if (Input.GetKeyDown(_instance.find_key(KeyCode.RightArrow)))
			{
				return true;
			}
			if (m_per_heng < 0.1f && m_heng > 0.1f)
			{
				return true;
			}
			break;
		case KeyCode.UpArrow:
			if (Input.GetKeyDown(_instance.find_key(KeyCode.UpArrow)))
			{
				return true;
			}
			if (m_per_shu > -0.1f && m_shu < -0.1f)
			{
				return true;
			}
			break;
		case KeyCode.DownArrow:
			if (Input.GetKeyDown(_instance.find_key(KeyCode.DownArrow)))
			{
				return true;
			}
			if (m_per_shu < 0.1f && m_shu > 0.1f)
			{
				return true;
			}
			break;
		case KeyCode.Z:
			if (Input.GetKeyDown(_instance.find_key(KeyCode.Z)))
			{
				return true;
			}
			if (Input.GetKeyDown(_instance.find_key(KeyCode.Joystick1Button1)))
			{
				return true;
			}
			break;
		case KeyCode.X:
			if (Input.GetKeyDown(_instance.find_key(KeyCode.X)))
			{
				return true;
			}
			if (Input.GetKeyDown(_instance.find_key(KeyCode.Joystick1Button2)))
			{
				return true;
			}
			break;
		case KeyCode.Escape:
			if (Input.GetKeyDown(_instance.find_key(KeyCode.Escape)))
			{
				return true;
			}
			if (Input.GetKeyDown(_instance.find_key(KeyCode.Joystick1Button7)))
			{
				return true;
			}
			break;
		}
		return false;
	}

	public bool key_up(KeyCode kc)
	{
		switch (kc)
		{
		case KeyCode.LeftArrow:
			if (Input.GetKeyUp(_instance.find_key(KeyCode.LeftArrow)))
			{
				return true;
			}
			if (m_per_heng < -0.1f && m_heng > -0.1f)
			{
				return true;
			}
			break;
		case KeyCode.RightArrow:
			if (Input.GetKeyUp(_instance.find_key(KeyCode.RightArrow)))
			{
				return true;
			}
			if (m_per_heng > 0.1f && m_heng < 0.1f)
			{
				return true;
			}
			break;
		case KeyCode.UpArrow:
			if (Input.GetKeyUp(_instance.find_key(KeyCode.UpArrow)))
			{
				return true;
			}
			if (m_per_shu < -0.1f && m_shu > -0.1f)
			{
				return true;
			}
			break;
		case KeyCode.DownArrow:
			if (Input.GetKeyUp(_instance.find_key(KeyCode.DownArrow)))
			{
				return true;
			}
			if (m_per_shu > 0.1f && m_shu < 0.1f)
			{
				return true;
			}
			break;
		case KeyCode.Z:
			if (Input.GetKeyUp(_instance.find_key(KeyCode.Z)))
			{
				return true;
			}
			if (Input.GetKeyUp(_instance.find_key(KeyCode.Joystick1Button1)))
			{
				return true;
			}
			break;
		case KeyCode.X:
			if (Input.GetKeyUp(_instance.find_key(KeyCode.X)))
			{
				return true;
			}
			if (Input.GetKeyUp(_instance.find_key(KeyCode.Joystick1Button2)))
			{
				return true;
			}
			break;
		case KeyCode.Escape:
			if (Input.GetKeyUp(_instance.find_key(KeyCode.Escape)))
			{
				return true;
			}
			if (Input.GetKeyUp(_instance.find_key(KeyCode.Joystick1Button7)))
			{
				return true;
			}
			break;
		}
		return false;
	}

	private void Update()
	{
		if (m_self != null)
		{
			m_self.update();
		}
		if (m_stop_mus)
		{
			if (m_play_mus.volume > 0f)
			{
				m_play_mus.volume -= Time.deltaTime;
			}
			if (m_play_mus.volume <= 0f)
			{
				m_stop_mus = false;
				m_play_mus.volume = 0f;
				m_play_mus.Stop();
				if (m_mus_name.Length > 0)
				{
					AudioClip audioClip = Resources.Load(m_mus_name) as AudioClip;
					if (audioClip != null)
					{
						m_play_mus.clip = audioClip;
						m_play_mus.loop = m_loop_mus;
						m_play_mus.pitch = m_pitch_mus;
						m_play_mus.Play();
					}
				}
			}
		}
		else if (m_play_mus.isPlaying && m_play_mus.volume < m_vols)
		{
			m_play_mus.volume += Time.deltaTime;
			if (m_play_mus.volume > m_vols)
			{
				m_play_mus.volume = m_vols;
			}
		}
		m_per_heng = m_heng;
		m_heng = Input.GetAxis("heng");
		m_per_shu = m_shu;
		m_shu = Input.GetAxis("shu");
	}

	private void OnGUI()
	{
		if (utils.IsPC)
		{
			Vector3 mousePosition = Input.mousePosition;
			GUI.DrawTexture(new Rect(mousePosition.x - 10f, (float)Screen.height - mousePosition.y - 5f, 48f, 48f), m_mouse);
		}
	}
}
