using protocol.game;

public class player
{
	public int m_review;

	public int m_pck_id;

	public string openid;

	public string openkey;

	public int userid;

	public string m_sig;

	public string nationality;

	public int visitor;

	public string name;

	public int head;

	public int level;

	public int exp;

	public int jewel;

	public int job_level;

	public int job_exp;

	public int upload;

	public int testify;

	public ulong exp_time;

	public int guide;

	public int mapid;

	public int support;

	public string notify_uri;

	public int init_life;

	public int life_per_time;

	public int download_num;

	public int br_life;

	public int br_index;

	public int br_start;

	public int br_hard;

	public int br_max;

	public int br_user_head;

	public string br_user_name;

	public string br_user_country;

	public string br_map_name;

	public int per_user_head;

	public string per_user_name;

	public string per_user_country;

	public string per_map_name;

	public smsg_mission_finish m_finish;

	public int m_finish_type;

	private ulong m_daily_time;

	public player()
	{
	}

	public player(smsg_login msg)
	{
		openid = msg.openid;
		openkey = msg.openkey;
		userid = msg.userid;
		m_sig = msg.sig;
		nationality = msg.nationality;
		visitor = msg.visitor;
		name = msg.name;
		head = msg.head;
		level = msg.level;
		exp = msg.exp;
		jewel = msg.jewel;
		upload = msg.upload;
		testify = msg.testify;
		exp_time = msg.exp_time;
		guide = msg.guide;
		mapid = msg.mapid;
		support = msg.support;
		notify_uri = msg.notify_uri;
		init_life = msg.init_life;
		life_per_time = msg.life_per_time;
		br_start = msg.challenge_start;
		download_num = msg.download_num;
		timer.set_server_time(msg.server_time);
		m_daily_time = timer.now();
	}

	public string get_name()
	{
		return get_name(userid, name, visitor);
	}

	public static string get_name(int u, string n, int v)
	{
		if (v == 1)
		{
			return game_data._instance.get_language_string("play_yk") + u;
		}
		return n;
	}

	public void add_exp(int e)
	{
		exp += e;
		s_t_exp s_t_exp2 = game_data._instance.get_t_exp(level + 1);
		while (s_t_exp2 != null && exp >= s_t_exp2.exp)
		{
			exp -= s_t_exp2.exp;
			level++;
			s_t_exp2 = game_data._instance.get_t_exp(level + 1);
		}
	}

	public void add_job_exp(int e)
	{
		job_exp += e;
		s_t_job_exp s_t_job_exp2 = game_data._instance.get_t_job_exp(job_level + 1);
		while (s_t_job_exp2 != null && job_exp >= s_t_job_exp2.exp)
		{
			job_exp -= s_t_job_exp2.exp;
			job_level++;
			s_t_job_exp2 = game_data._instance.get_t_job_exp(job_level + 1);
		}
	}

	public void set_reset_time(ulong stime, ulong ltime)
	{
		timer.set_server_time(stime);
		m_daily_time = timer.now();
	}

	public void set_per(int icon, string gq, string name, string map_name)
	{
		per_user_head = icon;
		per_user_name = name;
		per_user_country = gq;
		per_map_name = map_name;
	}

	public void set_br(int icon, string gq, string name, string map_name)
	{
		per_user_head = br_user_head;
		per_user_name = br_user_name;
		per_user_country = br_user_country;
		per_map_name = br_map_name;
		br_user_head = icon;
		br_user_name = name;
		br_user_country = gq;
		br_map_name = map_name;
	}

	public void update()
	{
		if (timer.trigger_time(m_daily_time, 0, 0))
		{
			upload = 0;
			download_num = 0;
		}
		m_daily_time = timer.now();
	}
}
