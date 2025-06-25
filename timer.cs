using System;

public class timer
{
	private static long dtime_;

	private static long dtime1_;

	public static ulong start_time_;

	public static void set_server_time(ulong server_time)
	{
		dtime_ = DateTime.Now.Ticks / 10000 - (long)server_time;
		dtime1_ = (DateTime.Now.Ticks - DateTime.Parse("1/1/1970").Ticks) / 10000 - 28800000 - (long)server_time;
	}

	public static DateTime dtnow()
	{
		return DateTime.Now.AddTicks(-dtime1_ * 10000);
	}

	public static ulong now()
	{
		return (ulong)(DateTime.Now.Ticks / 10000 - dtime_);
	}

	public static DateTime time2dt(ulong time)
	{
		long value = (long)((time + 28800000) * 10000);
		return DateTime.Parse("1/1/1970").AddTicks(value);
	}

	public static DateTime GetTime(string timeStamp)
	{
		DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
		long ticks = long.Parse(timeStamp + "0000000");
		TimeSpan value = new TimeSpan(ticks);
		return dateTime.Add(value);
	}

	public static int last_time_today()
	{
		long num = (DateTime.Parse(dtnow().ToShortDateString() + " 23:59:59").Ticks - dtnow().Ticks) / 10000;
		num %= 86400000;
		return (int)num;
	}

	public static bool trigger_time(ulong old_time, int hour, int minute)
	{
		DateTime dateTime = time2dt(old_time);
		ulong num = now();
		DateTime dateTime2 = time2dt(num);
		if (num <= old_time)
		{
			return false;
		}
		if (num - old_time >= 86400000)
		{
			return true;
		}
		bool flag = is_small(dateTime, hour, minute);
		bool flag2 = is_small(dateTime2, hour, minute);
		if (is_same_day(dateTime, dateTime2))
		{
			if (flag && !flag2)
			{
				return true;
			}
			return false;
		}
		if (!flag && !flag2)
		{
			return true;
		}
		if (flag && flag2)
		{
			return true;
		}
		return false;
	}

	public static bool trigger_week_time(ulong old_time)
	{
		DateTime dateTime = time2dt(old_time);
		ulong num = now();
		DateTime dateTime2 = time2dt(num);
		if (num <= old_time)
		{
			return false;
		}
		if (num - old_time >= 604800000)
		{
			return true;
		}
		int num2 = (int)dateTime2.DayOfWeek;
		if (num2 == 0)
		{
			num2 = 7;
		}
		int num3 = (int)dateTime.DayOfWeek;
		if (num3 == 0)
		{
			num3 = 7;
		}
		if (num2 < num3)
		{
			return true;
		}
		if (num2 == num3 && num - old_time >= 518400000)
		{
			return true;
		}
		return false;
	}

	public static bool trigger_month_time(ulong old_time)
	{
		DateTime dateTime = time2dt(old_time);
		ulong num = now();
		DateTime dateTime2 = time2dt(num);
		if (num <= old_time)
		{
			return false;
		}
		if (num - old_time >= 2678400000u)
		{
			return true;
		}
		int month = dateTime2.Month;
		int month2 = dateTime.Month;
		if (month != month2)
		{
			return true;
		}
		return false;
	}

	private static bool is_same_day(DateTime old_dt, DateTime new_dt)
	{
		if (old_dt.Year != new_dt.Year)
		{
			return false;
		}
		if (old_dt.Month != new_dt.Month)
		{
			return false;
		}
		if (old_dt.Day != new_dt.Day)
		{
			return false;
		}
		return true;
	}

	private static bool is_small(DateTime dt, int hour, int minute)
	{
		if (dt.Hour < hour)
		{
			return true;
		}
		if (dt.Hour == hour)
		{
			if (dt.Minute < minute)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public static int run_day(ulong old_time)
	{
		ulong num = now();
		if (old_time >= num)
		{
			return 0;
		}
		ulong num2 = num - old_time;
		ulong num3 = num2 / 86400000uL;
		ulong old_time2 = old_time + num3 * 86400000;
		if (trigger_time(old_time2, 0, 0))
		{
			num3++;
		}
		return (int)num3;
	}

	public static string get_time_show(long t)
	{
		if (t < 0)
		{
			t = 0L;
		}
		int num = (int)(t / 1000);
		int num2 = num / 3600;
		int num3 = num % 3600 / 60;
		int num4 = num % 60;
		string text = num2.ToString();
		string text2 = num3.ToString();
		string text3 = num4.ToString();
		if (text.Length < 2)
		{
			text = "0" + text;
		}
		if (text2.Length < 2)
		{
			text2 = "0" + text2;
		}
		if (text3.Length < 2)
		{
			text3 = "0" + text3;
		}
		string result = text + ":" + text2 + ":" + text3;
		if (num2 == 0)
		{
			result = text2 + ":" + text3;
		}
		return result;
	}

	public static string get_game_time(int time)
	{
		int num = time / 3000;
		int num2 = time / 50 % 60;
		int num3 = time % 50 * 2;
		string text = num.ToString();
		string text2 = num2.ToString();
		string text3 = num3.ToString();
		if (num < 10)
		{
			text = "0" + text;
		}
		if (num2 < 10)
		{
			text2 = "0" + text2;
		}
		if (num3 < 10)
		{
			text3 = "0" + text3;
		}
		return text + ":" + text2 + ":" + text3 + "\"";
	}
}
