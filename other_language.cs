using UnityEngine;

public class other_language : MonoBehaviour
{
	public string text;

	private void Awake()
	{
		if (game_data._instance.m_lang == e_language.el_english)
		{
			GetComponent<UILabel>().trueTypeFont = mario._instance.m_efont;
		}
		if (text != string.Empty)
		{
			GetComponent<UILabel>().text = game_data._instance.get_language_string(text);
		}
	}
}
