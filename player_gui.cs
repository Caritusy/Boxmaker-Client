using protocol.game;
using UnityEngine;

public class player_gui : MonoBehaviour
{
	public GameObject m_view;

	public GameObject m_touxiang;

	public GameObject m_name;

	public GameObject m_guojia;

	public GameObject m_level;

	public GameObject m_level_text;

	public GameObject m_exp_bar;

	public GameObject m_exp_text;

	public GameObject m_job_level;

	public GameObject m_job_exp_bar;

	public GameObject m_job_exp_text;

	public GameObject m_time;

	public GameObject m_ztz;

	public GameObject m_ztg;

	public GameObject m_zfs;

	public GameObject m_zpl;

	public GameObject m_gk;

	public GameObject m_bgk;

	public GameObject m_jqwg_s;

	public GameObject m_jqwg_t;

	public GameObject m_tscd_s;

	public GameObject m_tscd_t;

	public GameObject m_wgzd_s;

	public GameObject m_wgzd_t;

	private smsg_view_player m_msg;

	public GameObject m_player_sub;

	public void reset(smsg_view_player msg)
	{
		m_msg = msg;
		m_name.GetComponent<UILabel>().text = player.get_name(m_msg.data.userid, m_msg.data.name, m_msg.data.visitor);
		m_touxiang.GetComponent<UISprite>().spriteName = game_data._instance.get_t_touxiang(m_msg.data.head);
		m_guojia.GetComponent<UISprite>().spriteName = game_data._instance.get_t_guojia(m_msg.data.country);
		s_t_exp s_t_exp2 = game_data._instance.get_t_exp(m_msg.data.level);
		m_level.GetComponent<UISprite>().spriteName = s_t_exp2.icon;
		m_level_text.GetComponent<UILabel>().text = m_msg.data.level.ToString();
		s_t_exp2 = game_data._instance.get_t_exp(m_msg.data.level + 1);
		if (s_t_exp2 != null)
		{
			float value = (float)m_msg.data.exp / (float)s_t_exp2.exp;
			m_exp_bar.GetComponent<UIProgressBar>().value = value;
			m_exp_text.GetComponent<UILabel>().text = m_msg.data.exp + "/" + s_t_exp2.exp;
		}
		else
		{
			m_exp_bar.GetComponent<UIProgressBar>().value = 1f;
			m_exp_text.GetComponent<UILabel>().text = m_msg.data.exp + "/--";
		}
		m_job_level.GetComponent<UILabel>().text = "Lv" + m_msg.data.mlevel;
		s_t_job_exp s_t_job_exp2 = game_data._instance.get_t_job_exp(m_msg.data.mlevel + 1);
		if (s_t_job_exp2 != null)
		{
			float value2 = (float)m_msg.data.mexp / (float)s_t_job_exp2.exp;
			m_job_exp_bar.GetComponent<UIProgressBar>().value = value2;
			m_job_exp_text.GetComponent<UILabel>().text = m_msg.data.mexp + "/" + s_t_job_exp2.exp;
		}
		else
		{
			m_job_exp_bar.GetComponent<UIProgressBar>().value = 1f;
			m_job_exp_text.GetComponent<UILabel>().text = m_msg.data.mexp + "/--";
		}
		m_time.GetComponent<UILabel>().text = m_msg.data.register;
		m_ztz.GetComponent<UILabel>().text = m_msg.data.amount.ToString("N0");
		m_ztg.GetComponent<UILabel>().text = m_msg.data.pas.ToString("N0");
		m_zfs.GetComponent<UILabel>().text = m_msg.data.point.ToString("N0");
		m_zpl.GetComponent<UILabel>().text = m_msg.data.comment.ToString("N0");
		m_gk.GetComponent<UILabel>().text = m_msg.data.video.ToString("N0");
		m_bgk.GetComponent<UILabel>().text = m_msg.data.watched.ToString("N0");
		mario._instance.remove_child(m_jqwg_s);
		if (msg.recent.Count == 0)
		{
			m_jqwg_t.SetActive(value: true);
			m_jqwg_s.GetComponent<UISprite>().height = 140;
		}
		else
		{
			m_jqwg_t.SetActive(value: false);
			for (int i = 0; i < msg.recent.Count; i++)
			{
				GameObject gameObject = (GameObject)Object.Instantiate(m_player_sub);
				gameObject.transform.parent = m_jqwg_s.transform;
				gameObject.transform.localPosition = new Vector3(0f, -53 - 95 * i, 0f);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject.GetComponent<player_sub>().reset(1, msg.recent[i].id, msg.recent[i].name, msg.recent[i].url, msg.recent[i].rank, msg.recent[i].time, base.gameObject);
				gameObject.SetActive(value: true);
			}
			m_jqwg_s.GetComponent<UISprite>().height = 95 * msg.recent.Count + 10;
		}
		mario._instance.remove_child(m_tscd_s);
		if (msg.upload.Count == 0)
		{
			m_tscd_t.SetActive(value: true);
			m_tscd_s.GetComponent<UISprite>().height = 140;
		}
		else
		{
			m_tscd_t.SetActive(value: false);
			for (int j = 0; j < msg.upload.Count; j++)
			{
				GameObject gameObject2 = (GameObject)Object.Instantiate(m_player_sub);
				gameObject2.transform.parent = m_tscd_s.transform;
				gameObject2.transform.localPosition = new Vector3(0f, -53 - 95 * j, 0f);
				gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject2.GetComponent<player_sub>().reset(2, msg.upload[j].id, msg.upload[j].name, msg.upload[j].url, 0, msg.upload[j].time, base.gameObject);
				gameObject2.SetActive(value: true);
			}
			m_tscd_s.GetComponent<UISprite>().height = 95 * msg.upload.Count + 10;
		}
		mario._instance.remove_child(m_wgzd_s);
		if (msg.play.Count == 0)
		{
			m_wgzd_t.SetActive(value: true);
			m_wgzd_s.GetComponent<UISprite>().height = 140;
		}
		else
		{
			m_wgzd_t.SetActive(value: false);
			for (int k = 0; k < msg.play.Count; k++)
			{
				GameObject gameObject3 = (GameObject)Object.Instantiate(m_player_sub);
				gameObject3.transform.parent = m_wgzd_s.transform;
				gameObject3.transform.localPosition = new Vector3(0f, -53 - 95 * k, 0f);
				gameObject3.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject3.GetComponent<player_sub>().reset(3, msg.play[k].id, msg.play[k].name, msg.play[k].url, msg.play[k].play, string.Empty, base.gameObject);
				gameObject3.SetActive(value: true);
			}
			m_wgzd_s.GetComponent<UISprite>().height = 95 * msg.play.Count + 10;
		}
		m_view.GetComponent<UIScrollView>().ResetPosition();
	}

	private void click(GameObject obj)
	{
		if (obj.name == "close")
		{
			GetComponent<ui_show_anim>().hide_ui();
		}
	}
}
