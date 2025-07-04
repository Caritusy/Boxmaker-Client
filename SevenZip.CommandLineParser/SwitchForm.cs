namespace SevenZip.CommandLineParser;

public class SwitchForm
{
	public string IDString;

	public SwitchType Type;

	public bool Multi;

	public int MinLen;

	public int MaxLen;

	public string PostCharSet;

	public SwitchForm(string idString, SwitchType type, bool multi, int minLen, int maxLen, string postCharSet)
	{
		IDString = idString;
		Type = type;
		Multi = multi;
		MinLen = minLen;
		MaxLen = maxLen;
		PostCharSet = postCharSet;
	}

	public SwitchForm(string idString, SwitchType type, bool multi, int minLen)
		: this(idString, type, multi, minLen, 0, string.Empty)
	{
	}

	public SwitchForm(string idString, SwitchType type, bool multi)
		: this(idString, type, multi, 0)
	{
	}
}
