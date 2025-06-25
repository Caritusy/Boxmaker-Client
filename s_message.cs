using System.Collections;
using System.Collections.Generic;

public class s_message
{
	public long m_guid;

	public bool m_remove;

	public string m_type;

	public float time;

	public s_message m_next;

	public ArrayList m_floats = new ArrayList();

	public ArrayList m_ints = new ArrayList();

	public ArrayList m_string = new ArrayList();

	public ArrayList m_object = new ArrayList();

	public ArrayList m_long = new ArrayList();

	public List<bool> m_bools = new List<bool>();
}
