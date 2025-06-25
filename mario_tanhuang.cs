public class mario_tanhuang : mario_charater
{
	public override bool be_left_hit(mario_obj obj, ref int px)
	{
		if (obj.m_type != mario_type.mt_charater)
		{
			return false;
		}
		obj.m_pvelocity.x = 120;
		obj.m_velocity.x = 0;
		obj.m_no_mc_time = 20;
		obj.change_way(mario_fx.mf_right);
		px = obj.get_left_hit_pos(this);
		play_anim("left_hit");
		mario._instance.play_sound("sound/tanhuang");
		return true;
	}

	public override bool be_right_hit(mario_obj obj, ref int px)
	{
		if (obj.m_type != mario_type.mt_charater)
		{
			return false;
		}
		obj.m_pvelocity.x = -120;
		obj.m_velocity.x = 0;
		obj.m_no_mc_time = 20;
		obj.change_way(mario_fx.mf_left);
		px = obj.get_right_hit_pos(this);
		play_anim("right_hit");
		mario._instance.play_sound("sound/tanhuang");
		return true;
	}

	public override bool be_top_hit(mario_obj obj, ref int py)
	{
		if (obj.m_type != mario_type.mt_charater)
		{
			return false;
		}
		if (obj.m_main)
		{
			if (obj.m_pos.x >= m_pos.x)
			{
				obj.m_pvelocity.x = 120;
				if (obj.m_velocity.x < 0)
				{
					obj.m_velocity.x = -obj.m_velocity.x;
				}
				obj.m_no_mc_time = 20;
				play_anim("left_hit");
				mario._instance.play_sound("sound/tanhuang");
			}
			else
			{
				obj.m_pvelocity.x = -120;
				if (obj.m_velocity.x > 0)
				{
					obj.m_velocity.x = -obj.m_velocity.x;
				}
				obj.m_no_mc_time = 20;
				play_anim("right_hit");
				mario._instance.play_sound("sound/tanhuang");
			}
		}
		else if (obj.m_velocity.y > 0)
		{
			m_pvelocity.y = obj.m_velocity.y;
			obj.m_velocity.y = 0;
		}
		py = obj.m_pos.y;
		return true;
	}

	public override bool be_bottom_hit(mario_obj obj, ref int py)
	{
		if (obj.m_type != mario_type.mt_charater)
		{
			return false;
		}
		if (obj.m_main)
		{
			play_mode._instance.caisi(80);
			obj.m_pvelocity.y = 240;
		}
		else
		{
			obj.m_pvelocity.y = 300;
		}
		if (obj.m_velocity.y < 0)
		{
			obj.m_velocity.y = 0;
		}
		py = obj.get_bottom_hit_pos(this);
		play_anim("bottom_hit");
		mario._instance.play_sound("sound/tanhuang");
		return true;
	}

	public override void be_hit(mario_obj obj)
	{
	}
}
