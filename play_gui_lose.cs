using UnityEngine;

public class play_gui_lose : MonoBehaviour
{
	private void OnEnable()
	{
		mario_tool._instance.cyad();
		mario_tool._instance.hfad();
	}

	private void OnDisable()
	{
		mario_tool._instance.close_hfad();
	}
}
