using System.ComponentModel;
using System.Globalization;
using System.Resources;

namespace LzmaAlone.Properties;

internal class Resources
{
	private static ResourceManager _resMgr;

	private static CultureInfo _resCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static ResourceManager ResourceManager
	{
		get
		{
			if (_resMgr == null)
			{
				ResourceManager resMgr = new ResourceManager("Resources", typeof(Resources).Assembly);
				_resMgr = resMgr;
			}
			return _resMgr;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static CultureInfo Culture
	{
		get
		{
			return _resCulture;
		}
		set
		{
			_resCulture = value;
		}
	}

	internal Resources()
	{
	}
}
