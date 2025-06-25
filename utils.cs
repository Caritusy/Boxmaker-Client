using System;
using System.IO;
using SevenZip;
using SevenZip.Compression.LZMA;
using UnityEngine;

public class utils
{
	public static int g_grid_size = 640;

	public static int g_active_x = 10;

	public static int g_active_y = 6;

	public static int g_start_x = 12;

	public static int g_del_x = 20;

	public static int g_load_size = 2;

	public static int g_height = 6400;

	public static int g_roll_y = 3200;

	public static int g_max_x = 500;

	public static int g_max_y = 100;

	public static int g_min_x = 3;

	public static int g_min_y = 10;

	public static int g_yinfu = 110;

	public static int g_ryqiu = 111;

	public static int g_csg = 112;

	public static int g_cspt = 113;

	public static int g_hg = 120;

	public static int g_csm = 122;

	public static int g_g = 15;

    public static bool IsPC
    {
        get
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return false;
            }
            return true;
        }
    }

    private static int[] m_tan_arr = new int[180]
	{
		0, 174, 349, 524, 699, 874, 1051, 1227, 1405, 1583,
		1763, 1943, 2125, 2308, 2493, 2679, 2867, 3057, 3249, 3443,
		3639, 3838, 4040, 4244, 4452, 4663, 4877, 5095, 5317, 5543,
		5773, 6008, 6248, 6494, 6745, 7002, 7265, 7535, 7812, 8097,
		8390, 8692, 9004, 9325, 9656, 9999, 10355, 10723, 11106, 11503,
		11917, 12348, 12799, 13270, 13763, 14281, 14825, 15398, 16003, 16642,
		17320, 18040, 18807, 19626, 20503, 21445, 22460, 23558, 24750, 26050,
		27474, 29042, 30776, 32708, 34874, 37320, 40107, 43314, 47046, 51445,
		56712, 63137, 71153, 81443, 95143, 114300, 143006, 190811, 286362, 572899,
		999999999, -572899, -286362, -190811, -143006, -114300, -95143, -81443, -71153, -63137,
		-56712, -51445, -47046, -43314, -40107, -37320, -34874, -32708, -30776, -29042,
		-27474, -26050, -24750, -23558, -22460, -21445, -20503, -19626, -18807, -18040,
		-17320, -16642, -16003, -15398, -14825, -14281, -13763, -13270, -12799, -12348,
		-11917, -11503, -11106, -10723, -10355, -10000, -9656, -9325, -9004, -8692,
		-8390, -8097, -7812, -7535, -7265, -7002, -6745, -6494, -6248, -6008,
		-5773, -5543, -5317, -5095, -4877, -4663, -4452, -4244, -4040, -3838,
		-3639, -3443, -3249, -3057, -2867, -2679, -2493, -2308, -2125, -1943,
		-1763, -1583, -1405, -1227, -1051, -874, -699, -524, -349, -174
	};

	private static int[] m_rxy_arr = new int[720]
	{
		0, 1000, 17, 999, 34, 999, 52, 998, 69, 997,
		87, 996, 104, 994, 121, 992, 139, 990, 156, 987,
		173, 984, 190, 981, 207, 978, 224, 974, 241, 970,
		258, 965, 275, 961, 292, 956, 309, 951, 325, 945,
		342, 939, 358, 933, 374, 927, 390, 920, 406, 913,
		422, 906, 438, 898, 453, 891, 469, 882, 484, 874,
		499, 866, 515, 857, 529, 848, 544, 838, 559, 829,
		573, 819, 587, 809, 601, 798, 615, 788, 629, 777,
		642, 766, 656, 754, 669, 743, 681, 731, 694, 719,
		707, 707, 719, 694, 731, 681, 743, 669, 754, 656,
		766, 642, 777, 629, 788, 615, 798, 601, 809, 587,
		819, 573, 829, 559, 838, 544, 848, 529, 857, 515,
		866, 500, 874, 484, 882, 469, 891, 453, 898, 438,
		906, 422, 913, 406, 920, 390, 927, 374, 933, 358,
		939, 342, 945, 325, 951, 309, 956, 292, 961, 275,
		965, 258, 970, 241, 974, 224, 978, 207, 981, 190,
		984, 173, 987, 156, 990, 139, 992, 121, 994, 104,
		996, 87, 997, 69, 998, 52, 999, 34, 999, 17,
		1000, 0, 999, -17, 999, -34, 998, -52, 997, -69,
		996, -87, 994, -104, 992, -121, 990, -139, 987, -156,
		984, -173, 981, -190, 978, -207, 974, -224, 970, -241,
		965, -258, 961, -275, 956, -292, 951, -309, 945, -325,
		939, -342, 933, -358, 927, -374, 920, -390, 913, -406,
		906, -422, 898, -438, 891, -453, 882, -469, 874, -484,
		866, -499, 857, -515, 848, -529, 838, -544, 829, -559,
		819, -573, 809, -587, 798, -601, 788, -615, 777, -629,
		766, -642, 754, -656, 743, -669, 731, -681, 719, -694,
		707, -707, 694, -719, 681, -731, 669, -743, 656, -754,
		642, -766, 629, -777, 615, -788, 601, -798, 587, -809,
		573, -819, 559, -829, 544, -838, 529, -848, 515, -857,
		499, -866, 484, -874, 469, -882, 453, -891, 438, -898,
		422, -906, 406, -913, 390, -920, 374, -927, 358, -933,
		342, -939, 325, -945, 309, -951, 292, -956, 275, -961,
		258, -965, 241, -970, 224, -974, 207, -978, 190, -981,
		173, -984, 156, -987, 139, -990, 121, -992, 104, -994,
		87, -996, 69, -997, 52, -998, 34, -999, 17, -999,
		0, -1000, -17, -999, -34, -999, -52, -998, -69, -997,
		-87, -996, -104, -994, -121, -992, -139, -990, -156, -987,
		-173, -984, -190, -981, -207, -978, -224, -974, -241, -970,
		-258, -965, -275, -961, -292, -956, -309, -951, -325, -945,
		-342, -939, -358, -933, -374, -927, -390, -920, -406, -913,
		-422, -906, -438, -898, -453, -891, -469, -882, -484, -874,
		-500, -866, -515, -857, -529, -848, -544, -838, -559, -829,
		-573, -819, -587, -809, -601, -798, -615, -788, -629, -777,
		-642, -766, -656, -754, -669, -743, -681, -731, -694, -719,
		-707, -707, -719, -694, -731, -681, -743, -669, -754, -656,
		-766, -642, -777, -629, -788, -615, -798, -601, -809, -587,
		-819, -573, -829, -559, -838, -544, -848, -529, -857, -515,
		-866, -500, -874, -484, -882, -469, -891, -453, -898, -438,
		-906, -422, -913, -406, -920, -390, -927, -374, -933, -358,
		-939, -342, -945, -325, -951, -309, -956, -292, -961, -275,
		-965, -258, -970, -241, -974, -224, -978, -207, -981, -190,
		-984, -173, -987, -156, -990, -139, -992, -121, -994, -104,
		-996, -87, -997, -69, -998, -52, -999, -34, -999, -17,
		-1000, 0, -999, 17, -999, 34, -998, 52, -997, 69,
		-996, 87, -994, 104, -992, 121, -990, 139, -987, 156,
		-984, 173, -981, 190, -978, 207, -974, 224, -970, 241,
		-965, 258, -961, 275, -956, 292, -951, 309, -945, 325,
		-939, 342, -933, 358, -927, 374, -920, 390, -913, 406,
		-906, 422, -898, 438, -891, 453, -882, 469, -874, 484,
		-866, 500, -857, 515, -848, 529, -838, 544, -829, 559,
		-819, 573, -809, 587, -798, 601, -788, 615, -777, 629,
		-766, 642, -754, 656, -743, 669, -731, 681, -719, 694,
		-707, 707, -694, 719, -681, 731, -669, 743, -656, 754,
		-642, 766, -629, 777, -615, 788, -601, 798, -587, 809,
		-573, 819, -559, 829, -544, 838, -529, 848, -515, 857,
		-500, 866, -484, 874, -469, 882, -453, 891, -438, 898,
		-422, 906, -406, 913, -390, 920, -374, 927, -358, 933,
		-342, 939, -325, 945, -309, 951, -292, 956, -275, 961,
		-258, 965, -241, 970, -224, 974, -207, 978, -190, 981,
		-173, 984, -156, 987, -139, 990, -121, 992, -104, 994,
		-87, 996, -69, 997, -52, 998, -34, 999, -17, 999
	};

	public static int[,] csg_points = new int[8, 2]
	{
		{ 2, 0 },
		{ -2, 0 },
		{ 0, 2 },
		{ 0, -2 },
		{ 2, 2 },
		{ -2, -2 },
		{ -2, 2 },
		{ 2, -2 }
	};

	public static int[] jx_block = new int[120]
	{
		9, 4, 2, 10, 4, 116, 11, 7, 116, 19,
		2, 105, 26, 2, 201, 27, 2, 201, 28, 2,
		201, 29, 2, 201, 30, 2, 201, 27, 3, 201,
		28, 3, 201, 29, 3, 201, 28, 4, 207, 36,
		4, 3, 37, 2, 3, 37, 3, 3, 37, 4,
		3, 37, 5, 3, 37, 0, 1, 37, 1, 1,
		38, 0, 1, 38, 1, 1, 39, 0, 1, 39,
		1, 1, 40, 0, 1, 40, 1, 1, 41, 0,
		1, 41, 1, 1, 42, 0, 1, 42, 1, 1,
		43, 0, 1, 43, 1, 1, 44, 0, 1, 44,
		1, 1, 45, 0, 1, 45, 1, 1, 46, 0,
		1, 46, 1, 1, 47, 0, 1, 47, 1, 1
	};

	public static int[] yuepu = new int[48]
	{
		13, 0, 13, 0, 13, 0, 11, 0, 13, 0,
		15, 0, 5, 0, 11, 0, 5, 0, 3, 0,
		6, 0, 7, 0, 7, -1, 6, 0, 5, 0,
		13, 0, 15, 0, 16, 0, 14, 0, 15, 0,
		13, 0, 11, 0, 12, 0, 7, 0
	};

	public static int[] yf = new int[8] { 0, 1, 3, 5, 6, 8, 10, 12 };

	public static int g_start_y
	{
		get
		{
			if (game_data._instance.m_map_data.no_music == 0)
			{
				return 10;
			}
			return 30;
		}
	}

	public static int g_del_y
	{
		get
		{
			if (game_data._instance.m_map_data.no_music == 0)
			{
				return 12;
			}
			return 32;
		}
	}

	public static int get_map_exp(int tr, int rs)
	{
		if (rs < 100)
		{
			return 2;
		}
		float num = (float)tr / (float)rs;
		if (num < 0.001f)
		{
			return 11;
		}
		if (num < 0.01f)
		{
			return 9;
		}
		if (num < 0.05f)
		{
			return 7;
		}
		if (num < 0.1f)
		{
			return 6;
		}
		if (num < 0.2f)
		{
			return 5;
		}
		if (num < 0.4f)
		{
			return 4;
		}
		if (num < 0.5f)
		{
			return 3;
		}
		return 2;
	}

	public static int get_map_nd(int tr, int rs)
	{
		float num = (float)tr / (float)rs;
		if (rs > 10000)
		{
			if (num < 0.0005f)
			{
				return 4;
			}
			if (num < 0.005f)
			{
				return 3;
			}
			if (num < 0.05f)
			{
				return 2;
			}
		}
		else if (rs > 1000)
		{
			if (num < 0.005f)
			{
				return 3;
			}
			if (num < 0.05f)
			{
				return 2;
			}
		}
		else
		{
			if (rs <= 100)
			{
				return 0;
			}
			if (num < 0.05f)
			{
				return 2;
			}
		}
		return 1;
	}

	public static TweenPosition add_pos_anim(GameObject obj, float speed, Vector3 pos, float delay)
	{
		Vector3 localPosition = obj.transform.localPosition;
		Vector3 to = obj.transform.localPosition + pos;
		TweenPosition tweenPosition = TweenPosition.Begin(obj, speed, obj.transform.localPosition);
		tweenPosition.method = UITweener.Method.EaseInOut;
		tweenPosition.from = localPosition;
		tweenPosition.to = to;
		tweenPosition.delay = delay;
		return tweenPosition;
	}

	public static TweenScale add_scale_anim(GameObject obj, float speed, Vector3 scale, float delay)
	{
		Vector3 localScale = obj.transform.localScale;
		TweenScale tweenScale = TweenScale.Begin(obj, speed, obj.transform.localScale);
		tweenScale.method = UITweener.Method.EaseInOut;
		tweenScale.from = localScale;
		tweenScale.to = scale;
		tweenScale.delay = delay;
		return tweenScale;
	}

	public static byte[] Compress(byte[] inbuf)
	{
		CoderPropID[] propIDs = new CoderPropID[8]
		{
			CoderPropID.DictionarySize,
			CoderPropID.PosStateBits,
			CoderPropID.LitContextBits,
			CoderPropID.LitPosBits,
			CoderPropID.Algorithm,
			CoderPropID.NumFastBytes,
			CoderPropID.MatchFinder,
			CoderPropID.EndMarker
		};
		object[] properties = new object[8] { 23, 2, 3, 2, 1, 128, "bt4", true };
		Encoder encoder = new Encoder();
		encoder.SetCoderProperties(propIDs, properties);
		MemoryStream inStream = new MemoryStream(inbuf);
		MemoryStream memoryStream = new MemoryStream();
		encoder.WriteCoderProperties(memoryStream);
		encoder.Code(inStream, memoryStream, -1L, -1L, null);
		return memoryStream.ToArray();
	}

	public static byte[] Decompress(byte[] inbuf)
	{
		Decoder decoder = new Decoder();
		byte[] array = new byte[5];
		Array.Copy(inbuf, array, 5);
		decoder.SetDecoderProperties(array);
		MemoryStream memoryStream = new MemoryStream(inbuf);
		memoryStream.Seek(5L, SeekOrigin.Current);
		MemoryStream memoryStream2 = new MemoryStream();
		decoder.Code(memoryStream, memoryStream2, -1L, -1L, null);
		return memoryStream2.ToArray();
	}

	public static mario_point tan(int r)
	{
		switch (r)
		{
		case 0:
			return new mario_point(100, 0);
		case 90:
			return new mario_point(0, 100);
		case 180:
			return new mario_point(-100, 0);
		case 270:
			return new mario_point(0, -100);
		default:
		{
			int num = m_tan_arr[r % 180] / 10;
			int num2 = 1000;
			if (num >= 1000000 || num <= -1000000)
			{
				num /= 10000;
				num2 = 0;
			}
			else if (num >= 100000 || num <= -100000)
			{
				num /= 1000;
				num2 = 1;
			}
			else if (num >= 10000 || num <= -10000)
			{
				num /= 100;
				num2 = 10;
			}
			else if (num >= 1000 || num <= -1000)
			{
				num /= 10;
				num2 = 100;
			}
			if (r >= 90 && r < 270)
			{
				num = -num;
				num2 = -num2;
			}
			return new mario_point(num2, num);
		}
		}
	}

	public static int atan(int x, int y)
	{
		if (x == 0 && y == 0)
		{
			return 0;
		}
		if (x == 0 && y >= 0)
		{
			return 90;
		}
		if (x == 0 && y < 0)
		{
			return 270;
		}
		if (y == 0 && x > 0)
		{
			return 0;
		}
		if (y == 0 && x < 0)
		{
			return 180;
		}
		int num = y * 10000 / x;
		int num2 = 999999999;
		int num3 = 0;
		for (int i = 0; i < m_tan_arr.Length; i++)
		{
			int num4 = m_tan_arr[i] - num;
			if (num4 < 0)
			{
				num4 = -num4;
			}
			if (num4 < num2)
			{
				num2 = num4;
				num3 = i;
			}
		}
		if (y < 0)
		{
			num3 += 180;
		}
		return num3;
	}

	public static mario_point get_rxy(int r)
	{
		r %= 360;
		if (r < 0)
		{
			r = 360 + r;
		}
		return new mario_point(m_rxy_arr[r * 2], m_rxy_arr[r * 2 + 1]);
	}

	public static void do_yfu(int y, int t)
	{
		int num = 0;
		if (y >= -7 && y <= -1)
		{
			num = yf[-y] + t;
		}
		if (y >= 1 && y <= 7)
		{
			num = yf[y] + t + 12;
		}
		if (y >= 11 && y <= 17)
		{
			num = yf[y - 10] + t + 24;
		}
		num -= 5;
		if (num != 0)
		{
			mario._instance.play_sound("sound/yf/0-" + num);
		}
	}
}
