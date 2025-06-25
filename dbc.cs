using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class dbc
{
	private int m_x_num;

	private int m_y_num;

	private byte[] m_data;

	private List<int> m_records = new List<int>();

	public void load_txt(string name)
	{
		m_x_num = 0;
		m_y_num = 0;
		m_records.Clear();
		TextAsset textAsset = Resources.Load("config/" + name) as TextAsset;
		if (textAsset == null)
		{
			Debug.Log("err config__" + name);
		}
		m_data = textAsset.bytes;
		for (int i = 0; i < m_data.Length; i++)
		{
			if (m_data[i] == 10)
			{
				m_y_num++;
			}
			if ((m_data[i] == 10 || m_data[i] == 9) && m_y_num > 1)
			{
				m_records.Add(i + 1);
			}
		}
		for (int j = 0; j < m_data.Length; j++)
		{
			if (m_data[j] == 9)
			{
				m_x_num++;
			}
			else if (m_data[j] == 10)
			{
				break;
			}
		}
		m_x_num++;
		m_y_num -= 2;
	}

	public string get(int x, int y)
	{
		int num = 0;
		int num2 = y * m_x_num + x;
		if (num2 >= m_records.Count)
		{
			return string.Empty;
		}
		for (int i = m_records[y * m_x_num + x]; m_data[i] != 9 && m_data[i] != 10 && m_data[i] != 13; i++)
		{
			num++;
		}
		num = 0;
		for (int j = m_records[y * m_x_num + x]; m_data[j] != 9 && m_data[j] != 10 && m_data[j] != 13; j++)
		{
			num++;
		}
		string @string = Encoding.UTF8.GetString(m_data, m_records[y * m_x_num + x], num);
		if (@string.Length == 0)
		{
			return "0";
		}
		return @string.Replace("{nn}", "\n");
	}

	public int get_x()
	{
		return m_x_num;
	}

	public int get_y()
	{
		return m_y_num;
	}
}
