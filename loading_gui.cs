using UnityEngine;

public class loading_gui : MonoBehaviour, IMessage
{
	private void Start()
	{
		cmessage_center._instance.add_handle(this);
	}

	private void OnDestroy()
	{
		cmessage_center._instance.remove_handle(this);
	}

	private void OnEnable()
	{
		mario._instance.wait(flag: true, game_data._instance.get_language_string("loading_gui_zzjz"));
		s_message s_message2 = new s_message();
		s_message2.m_type = "first_load";
		s_message2.time = 0.1f;
		cmessage_center._instance.add_message(s_message2);
	}

	public void message(s_message message)
	{
		if (message.m_type == "first_load_end")
		{
			LJSDK._instance.init();
		}
		if (message.m_type == "init_success")
		{
			mario._instance.wait(flag: false, string.Empty);
			mario._instance.change_state(e_game_state.egs_login, 0, delegate
			{
				Object.Destroy(base.gameObject);
			});
		}
		if (message.m_type == "init_failed")
		{
			mario._instance.change_state(e_game_state.egs_login, 0, delegate
			{
				Object.Destroy(base.gameObject);
			});
		}
	}

	public void net_message(s_net_message message)
	{
	}
}
