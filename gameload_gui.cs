using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class gameload_gui : MonoBehaviour, IMessage
{
	public JsonData m_version_www;

	private string m_file;

	private WWW m_www;

	public GameObject m_string;

	private float m_wait_update_res;

	private string m_boxmaker_apk;

	private string m_common_url = "http://mario.oss.yymoon.com/android/";

	private void Start()
	{
		cmessage_center._instance.add_handle(this);
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		update_res();
	}

	private void OnDestroy()
	{
		cmessage_center._instance.remove_handle(this);
	}

	private IEnumerator download_height_ver(string file)
	{
		m_www = new WWW(file);
		while (!m_www.isDone)
		{
			yield return new WaitForSeconds(0.1f);
		}
		if (m_www.error != null)
		{
			Debug.Log("http error : " + m_www.error);
			StopAllCoroutines();
			StartCoroutine(download_height_ver(file));
			yield break;
		}
		m_boxmaker_apk = string.Concat(str2: m_version_www["ver"].ToString(), str0: Application.persistentDataPath, str1: "/boxmaker_", str3: ".apk");
		FileStream output = new FileStream(m_boxmaker_apk, FileMode.Create);
		output.Write(m_www.bytes, 0, m_www.bytes.Length);
		output.Flush();
		output.Close();
		Invoke("install", 1f);
	}

	public void message(s_message message)
	{
		if (message.m_type == "gameload_button_ok")
		{
			int num = int.Parse(m_version_www["type"].ToString());
			string text = m_version_www["open_url"].ToString();
			switch (num)
			{
			case 1:
				Application.OpenURL(text);
				Application.Quit();
				return;
			case 2:
				StartCoroutine(download_height_ver(text));
				break;
			}
		}
		if (message.m_type == "gameload_button_cancel")
		{
			string ver = m_version_www["min_ver"].ToString();
			if (get_ver(ver) <= get_ver(game_data._instance.m_ver))
			{
				StartCoroutine(gonggao());
			}
			else
			{
				Application.Quit();
			}
		}
	}

	public void net_message(s_net_message message)
	{
	}

	private void login_game()
	{
		StopAllCoroutines();
		mario._instance.change_state(e_game_state.egs_loading, 0, delegate
		{
			UnityEngine.Object.Destroy(base.gameObject);
		});
	}

	private void update_res()
	{
		m_wait_update_res = 0f;
		StopAllCoroutines();
		StartCoroutine(download_res());
	}

	private IEnumerator download_res()
	{
		string _ver = m_common_url + game_data._instance.m_channel + "/version.json";
		WWW _version_www = new WWW(_ver);
		while (!_version_www.isDone)
		{
			yield return new WaitForSeconds(0.1f);
		}
		if (_version_www.error != null)
		{
			Debug.Log("http error : " + _version_www.error);
			update_res();
			yield break;
		}
		m_version_www = JsonMapper.ToObject(_version_www.text);
		string ver = m_version_www["ver"].ToString();
		string min_ver = m_version_www["min_ver"].ToString();
		s_message mes_ok = new s_message
		{
			m_type = "gameload_button_ok"
		};
		s_message mes_cancel = new s_message
		{
			m_type = "gameload_button_cancel"
		};
		if (get_ver(min_ver) > get_ver(game_data._instance.m_ver))
		{
			mario._instance.show_double_dialog_box(string.Format(game_data._instance.get_language_string("game_load_gui_bbgj"), ver), mes_ok, mes_cancel);
		}
		else if (get_ver(ver) > get_ver(game_data._instance.m_ver))
		{
			mario._instance.show_double_dialog_box(string.Format(game_data._instance.get_language_string("game_load_gui_fxxbb"), ver), mes_ok, mes_cancel);
		}
		else
		{
			StartCoroutine(gonggao());
		}
	}

	private int get_ver(string ver)
	{
		//Discarded unreachable code: IL_002f, IL_003c
		try
		{
			string[] array = ver.Split('.');
			return int.Parse(array[0]) * 1000 + int.Parse(array[1]);
		}
		catch (Exception)
		{
			return 0;
		}
	}

	private IEnumerator gonggao()
	{
		string _gg = m_common_url + game_data._instance.m_channel + "/gonggao.txt";
		WWW _www = new WWW(_gg);
		while (!_www.isDone)
		{
			yield return new WaitForSeconds(0.1f);
		}
		if (_www.error != null)
		{
			Debug.Log("http error : " + _www.error);
			update_res();
		}
		else
		{
			game_data._instance.m_gonggao = _www.text;
			login_game();
		}
	}

	private void install()
	{
		Application.Quit();
	}

	private void Update()
	{
		string text = game_data._instance.get_language_string("game_load_gui_gxwj");
		if (m_www != null)
		{
			if (m_www.progress > 0f)
			{
				string arg = m_www.url.Substring(m_www.url.LastIndexOf("/") + 1);
				int num = (int)(m_www.progress * 100f);
				text = string.Format(game_data._instance.get_language_string("game_load_gui_gxwj1"), arg, num);
			}
			m_string.GetComponent<UILabel>().text = text;
		}
		else if (m_version_www == null)
		{
			m_wait_update_res += Time.deltaTime;
			if (m_wait_update_res > 15f)
			{
				update_res();
			}
			m_string.GetComponent<UILabel>().text = string.Format(game_data._instance.get_language_string("game_load_gui_bbxx"), (int)m_wait_update_res);
		}
	}
}
