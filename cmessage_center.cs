using System.Collections;
using UnityEngine;

public class cmessage_center : MonoBehaviour
{
	private ArrayList m_messages = new ArrayList();

	private ArrayList m_net_messages = new ArrayList();

	private ArrayList m_handles = new ArrayList();

	private ArrayList m_temp_messages = new ArrayList();

	public static cmessage_center _instance;

	private void Awake()
	{
		_instance = this;
	}

	public int get_message_num(string type)
	{
		int num = 0;
		for (int i = 0; i < m_messages.Count; i++)
		{
			s_message s_message2 = m_messages[i] as s_message;
			if (s_message2.m_type == type)
			{
				num++;
			}
		}
		return num;
	}

	public void add_message(s_message message)
	{
		if (message != null)
		{
			m_temp_messages.Add(message);
		}
	}

	public void add_net_message(s_net_message message)
	{
		m_net_messages.Add(message);
	}

	public void add_handle(IMessage message)
	{
		for (int i = 0; i < m_handles.Count; i++)
		{
			if (m_handles[i] == message)
			{
				return;
			}
		}
		m_handles.Add(message);
	}

	public void remove_handle(IMessage message)
	{
		int num = 0;
		while (num < m_handles.Count)
		{
			if (m_handles[num] == message)
			{
				m_handles.RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
	}

	private void message()
	{
		int num = 0;
		while (num < m_messages.Count)
		{
			s_message s_message2 = m_messages[num] as s_message;
			if (s_message2.time <= 0f)
			{
				m_messages.RemoveAt(num);
				int num2 = 0;
				while (num2 < m_handles.Count)
				{
					IMessage message = (IMessage)m_handles[num2];
					if (message != null)
					{
						message.message(s_message2);
						num2++;
					}
					else
					{
						m_handles.RemoveAt(num);
					}
				}
			}
			else
			{
				s_message2.time -= Time.deltaTime;
				num++;
			}
		}
	}

	private void net_message()
	{
		while (m_net_messages.Count > 0)
		{
			s_net_message s_net_message2 = m_net_messages[0] as s_net_message;
			m_net_messages.RemoveAt(0);
			for (int i = 0; i < m_handles.Count; i++)
			{
				((IMessage)m_handles[i])?.net_message(s_net_message2);
			}
		}
	}

	private void handle_message()
	{
		while (m_temp_messages.Count > 0)
		{
			m_messages.Add(m_temp_messages[0]);
			m_temp_messages.RemoveAt(0);
		}
		message();
		net_message();
	}

	private void Update()
	{
		handle_message();
	}
}
