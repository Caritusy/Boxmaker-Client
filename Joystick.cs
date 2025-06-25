using UnityEngine;

[ExecuteInEditMode]
public class Joystick : MonoBehaviour
{
	public delegate void JoystickEventHandler(Joystick joystick);

	[HideInInspector]
	private bool isLimitInCircle = true;

	[SerializeField]
	private int radius = 100;

	[SerializeField]
	private float minAlpha = 0.3f;

	private Vector2 joystickAxis = Vector2.zero;

	private Vector2 lastJoystickAxis = Vector2.zero;

	private bool isForBid;

	private bool isHolding;

	private UIWidget root;

	[SerializeField]
	private UISprite bg;

	[SerializeField]
	private UISprite thumb;

	public bool IsLimitInCircle => isLimitInCircle;

	public int Radius => radius;

	public float MinAlpha => minAlpha;

	public Vector2 JoystickAxis => joystickAxis;

	public Vector2 LastJoystickAxis => lastJoystickAxis;

	public bool IsForBid => isForBid;

	public bool IsHolding => isHolding;

	public static event JoystickEventHandler On_JoystickMoveStart;

	public static event JoystickEventHandler On_JoystickHolding;

	public static event JoystickEventHandler On_JoystickMoveEnd;

	private void Awake()
	{
		root = GetComponent<UIWidget>();
		Init();
	}

	private void Update()
	{
		if (Application.isEditor && !Application.isPlaying)
		{
			SetJoystickSize(radius);
		}
		if (!isForBid && isHolding && Joystick.On_JoystickHolding != null)
		{
			Joystick.On_JoystickHolding(this);
		}
	}

	private void Init()
	{
		bg.transform.localPosition = Vector3.zero;
		thumb.transform.localPosition = Vector3.zero;
		SetJoystickSize(radius);
		Lighting(minAlpha);
	}

	private void OnPress(bool isPressed)
	{
		if (isForBid)
		{
			Debug.Log("joystick is forbid!");
			return;
		}
		if (isPressed)
		{
			Lighting(1f);
			CalculateJoystickAxis();
			if (Joystick.On_JoystickMoveStart != null)
			{
				Joystick.On_JoystickMoveStart(this);
			}
			isHolding = true;
			return;
		}
		CalculateJoystickAxis();
		if (Joystick.On_JoystickMoveEnd != null)
		{
			Joystick.On_JoystickMoveEnd(this);
		}
		thumb.transform.localPosition = Vector3.zero;
		Lighting(minAlpha);
		isHolding = false;
	}

	private void OnDrag(Vector2 delta)
	{
		if (!isForBid)
		{
			CalculateJoystickAxis();
			if (Joystick.On_JoystickMoveStart != null)
			{
				Joystick.On_JoystickMoveStart(this);
			}
		}
	}

	private void CalculateJoystickAxis()
	{
		Vector3 localPosition = ScreenPos_to_NGUIPos(UICamera.currentTouch.pos);
		Transform parent = base.transform;
		do
		{
			localPosition -= parent.localPosition;
			parent = parent.parent;
		}
		while (parent.gameObject != UICamera.currentCamera.gameObject);
		if (localPosition.magnitude > (float)radius)
		{
			localPosition = localPosition.normalized * radius;
		}
		thumb.transform.localPosition = localPosition;
		lastJoystickAxis = joystickAxis;
		joystickAxis = new Vector2(localPosition.x / (float)radius, localPosition.y / (float)radius);
	}

	private Vector3 ScreenPos_to_NGUIPos(Vector3 screenPos)
	{
		Vector3 position = UICamera.currentCamera.ScreenToWorldPoint(screenPos);
		return UICamera.currentCamera.transform.InverseTransformPoint(position);
	}

	private Vector3 ScreenPos_to_NGUIPos(Vector2 screenPos)
	{
		return ScreenPos_to_NGUIPos(new Vector3(screenPos.x, screenPos.y, 0f));
	}

	private void SetJoystickSize(int radius)
	{
		root.width = 2 * radius;
		root.height = 2 * radius;
		thumb.width = (int)(0.5f * (float)root.width);
		thumb.height = (int)(0.5f * (float)root.height);
	}

	private void Lighting(float alpha)
	{
		root.alpha = alpha;
	}

	public float Axis2Angle(bool inDegree = true)
	{
		float num = Mathf.Atan2(joystickAxis.x, joystickAxis.y);
		if (inDegree)
		{
			return num * 57.29578f;
		}
		return num;
	}
}
