using System.Runtime.InteropServices;
using UnityEngine;

public class mario_tool : MonoBehaviour
{
	public static mario_tool _instance;

	private string[] m_titles = new string[2] { "mario_tool_xdgq", "mario_tool_tlhf" };

	private string[] m_texts = new string[2] { "mario_tool_ywjz", "mario_tool_ndtl" };

	private int m_ad_num;

	private int m_ad_tnum = 5;

	[DllImport("__Internal")]
	private static extern void startMTA();

	[DllImport("__Internal")]
	private static extern void onJewelGet(string guid, string scene, string num);

	[DllImport("__Internal")]
	private static extern void onJewelConsume(string guid, string scene, string num);

	[DllImport("__Internal")]
	private static extern void onUserDo(string guid, string scene, string num);

	[DllImport("__Internal")]
	private static extern void onRaid(string guid, string scene, string num);

	[DllImport("__Internal")]
	private static extern void createNotify(string text, int secondsFromNow);

	[DllImport("__Internal")]
	private static extern void cancelNotify();

	[DllImport("__Internal")]
	private static extern void init_tool();

	[DllImport("__Internal")]
	private static extern string _getCountryCode();

	[DllImport("__Internal")]
	private static extern void _hfad();

	[DllImport("__Internal")]
	private static extern void _close_hfad();

	[DllImport("__Internal")]
	private static extern void _cyad();

	private void Awake()
	{
		_instance = this;
	}

	private void Start()
	{
		if (!Application.isEditor)
		{
			cancel_notify();
			int second = timer.dtnow().Second;
			int minute = timer.dtnow().Minute;
			int hour = timer.dtnow().Hour;
			if (hour < 12)
			{
				int num = (12 - hour) * 60 - minute;
				num = num * 60 - second;
				int num2 = Random.Range(0, 2);
				create_notify(game_data._instance.get_language_string(m_titles[num2]), game_data._instance.get_language_string(m_texts[num2]), game_data._instance.get_language_string(m_texts[num2]), num);
			}
			if (hour < 18)
			{
				int num3 = (18 - hour) * 60 - minute;
				num3 = num3 * 60 - second;
				int num4 = Random.Range(0, 2);
				create_notify(game_data._instance.get_language_string(m_titles[num4]), game_data._instance.get_language_string(m_texts[num4]), game_data._instance.get_language_string(m_texts[num4]), num3);
			}
			int num5 = (36 - hour) * 60 - minute;
			num5 = num5 * 60 - second;
			int num6 = Random.Range(0, 2);
			create_notify(game_data._instance.get_language_string(m_titles[num6]), game_data._instance.get_language_string(m_texts[num6]), game_data._instance.get_language_string(m_texts[num6]), num5);
			int num7 = (42 - hour) * 60 - minute;
			num7 = num7 * 60 - second;
			int num8 = Random.Range(0, 2);
			create_notify(game_data._instance.get_language_string(m_titles[num8]), game_data._instance.get_language_string(m_texts[num8]), game_data._instance.get_language_string(m_texts[num8]), num7);
			int num9 = (60 - hour) * 60 - minute;
			num9 = num9 * 60 - second;
			int num10 = Random.Range(0, 2);
			create_notify(game_data._instance.get_language_string(m_titles[num10]), game_data._instance.get_language_string(m_texts[num10]), game_data._instance.get_language_string(m_texts[num10]), num9);
			int num11 = (66 - hour) * 60 - minute;
			num11 = num11 * 60 - second;
			int num12 = Random.Range(0, 2);
			create_notify(game_data._instance.get_language_string(m_titles[num12]), game_data._instance.get_language_string(m_texts[num12]), game_data._instance.get_language_string(m_texts[num12]), num11);
		}
	}

	public void onJewelGet(string scene, int num)
	{
		if (!Application.isEditor)
		{
		}
	}

	public void onJewelConsume(string scene, int num)
	{
		if (!Application.isEditor)
		{
		}
	}

	public void onUserDo(string scene, int num)
	{
		if (!Application.isEditor)
		{
		}
	}

	public void onRaid(string scene, int num)
	{
		if (!Application.isEditor)
		{
		}
	}

	public void create_notify(string title, string text, string ticker, int secondsFromNow)
	{
		if (!Application.isEditor)
		{
		}
	}

	public void cancel_notify()
	{
		if (!Application.isEditor)
		{
		}
	}

	public void hfad()
	{
		if (!Application.isEditor)
		{
		}
	}

	public void close_hfad()
	{
		if (!Application.isEditor)
		{
		}
	}

	public void cyad()
	{
		if (!Application.isEditor)
		{
			m_ad_num++;
			if (m_ad_num >= m_ad_tnum)
			{
				m_ad_tnum += (int)((double)m_ad_tnum * 1.5);
			}
		}
	}
}
