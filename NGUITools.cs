using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEngine;

public static class NGUITools
{
	private static AudioListener mListener;

	private static bool mLoaded = false;

	private static float mGlobalVolume = 1f;

	private static float mLastTimestamp = 0f;

	private static AudioClip mLastClip;

	private static Vector3[] mSides = new Vector3[4];

	public static float soundVolume
	{
		get
		{
			if (!mLoaded)
			{
				mLoaded = true;
				mGlobalVolume = PlayerPrefs.GetFloat("Sound", 1f);
			}
			return mGlobalVolume;
		}
		set
		{
			if (mGlobalVolume != value)
			{
				mLoaded = true;
				mGlobalVolume = value;
				PlayerPrefs.SetFloat("Sound", value);
			}
		}
	}

	public static bool fileAccess => Application.platform != RuntimePlatform.WindowsWebPlayer && Application.platform != RuntimePlatform.OSXWebPlayer;

	public static string clipboard
	{
		get
		{
			TextEditor textEditor = new TextEditor();
			textEditor.Paste();
			return textEditor.content.text;
		}
		set
		{
			TextEditor textEditor = new TextEditor();
			textEditor.content = new GUIContent(value);
			textEditor.OnFocus();
			textEditor.Copy();
		}
	}

	public static Vector2 screenSize => new Vector2(Screen.width, Screen.height);

	public static AudioSource PlaySound(AudioClip clip)
	{
		return PlaySound(clip, 1f, 1f);
	}

	public static AudioSource PlaySound(AudioClip clip, float volume)
	{
		return PlaySound(clip, volume, 1f);
	}

	public static AudioSource PlaySound(AudioClip clip, float volume, float pitch)
	{
		float time = Time.time;
		if (mLastClip == clip && mLastTimestamp + 0.1f > time)
		{
			return null;
		}
		mLastClip = clip;
		mLastTimestamp = time;
		volume *= soundVolume;
		if (clip != null && volume > 0.01f)
		{
			if (mListener == null || !GetActive(mListener))
			{
				if (UnityEngine.Object.FindObjectsOfType(typeof(AudioListener)) is AudioListener[] array)
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (GetActive(array[i]))
						{
							mListener = array[i];
							break;
						}
					}
				}
				if (mListener == null)
				{
					Camera camera = Camera.main;
					if (camera == null)
					{
						camera = UnityEngine.Object.FindObjectOfType(typeof(Camera)) as Camera;
					}
					if (camera != null)
					{
						mListener = camera.gameObject.AddComponent<AudioListener>();
					}
				}
			}
			if (mListener != null && mListener.enabled && GetActive(mListener.gameObject))
			{
				AudioSource audioSource = mListener.audio;
				if (audioSource == null)
				{
					audioSource = mListener.gameObject.AddComponent<AudioSource>();
				}
				audioSource.priority = 50;
				audioSource.pitch = pitch;
				audioSource.PlayOneShot(clip, volume);
				return audioSource;
			}
		}
		return null;
	}

	public static int RandomRange(int min, int max)
	{
		if (min == max)
		{
			return min;
		}
		return UnityEngine.Random.Range(min, max + 1);
	}

	public static string GetHierarchy(GameObject obj)
	{
		if (obj == null)
		{
			return string.Empty;
		}
		string text = obj.name;
		while (obj.transform.parent != null)
		{
			obj = obj.transform.parent.gameObject;
			text = obj.name + "\\" + text;
		}
		return text;
	}

	public static T[] FindActive<T>() where T : Component
	{
		return UnityEngine.Object.FindObjectsOfType(typeof(T)) as T[];
	}

	public static Camera FindCameraForLayer(int layer)
	{
		int num = 1 << layer;
		Camera cachedCamera;
		for (int i = 0; i < UICamera.list.size; i++)
		{
			cachedCamera = UICamera.list.buffer[i].cachedCamera;
			if ((bool)cachedCamera && (cachedCamera.cullingMask & num) != 0)
			{
				return cachedCamera;
			}
		}
		cachedCamera = Camera.main;
		if ((bool)cachedCamera && (cachedCamera.cullingMask & num) != 0)
		{
			return cachedCamera;
		}
		Camera[] array = new Camera[Camera.allCamerasCount];
		int allCameras = Camera.GetAllCameras(array);
		for (int j = 0; j < allCameras; j++)
		{
			cachedCamera = array[j];
			if ((bool)cachedCamera && cachedCamera.enabled && (cachedCamera.cullingMask & num) != 0)
			{
				return cachedCamera;
			}
		}
		return null;
	}

	public static void AddWidgetCollider(GameObject go)
	{
		AddWidgetCollider(go, considerInactive: false);
	}

	public static void AddWidgetCollider(GameObject go, bool considerInactive)
	{
		if (!(go != null))
		{
			return;
		}
		Collider component = go.GetComponent<Collider>();
		BoxCollider boxCollider = component as BoxCollider;
		if (boxCollider != null)
		{
			UpdateWidgetCollider(boxCollider, considerInactive);
		}
		else
		{
			if (component != null)
			{
				return;
			}
			BoxCollider2D component2 = go.GetComponent<BoxCollider2D>();
			if (component2 != null)
			{
				UpdateWidgetCollider(component2, considerInactive);
				return;
			}
			UICamera uICamera = UICamera.FindCameraForLayer(go.layer);
			if (uICamera != null && (uICamera.eventType == UICamera.EventType.World_2D || uICamera.eventType == UICamera.EventType.UI_2D))
			{
				component2 = go.AddComponent<BoxCollider2D>();
				component2.isTrigger = true;
				UIWidget component3 = go.GetComponent<UIWidget>();
				if (component3 != null)
				{
					component3.autoResizeBoxCollider = true;
				}
				UpdateWidgetCollider(component2, considerInactive);
			}
			else
			{
				boxCollider = go.AddComponent<BoxCollider>();
				boxCollider.isTrigger = true;
				UIWidget component4 = go.GetComponent<UIWidget>();
				if (component4 != null)
				{
					component4.autoResizeBoxCollider = true;
				}
				UpdateWidgetCollider(boxCollider, considerInactive);
			}
		}
	}

	public static void UpdateWidgetCollider(GameObject go)
	{
		UpdateWidgetCollider(go, considerInactive: false);
	}

	public static void UpdateWidgetCollider(GameObject go, bool considerInactive)
	{
		if (!(go != null))
		{
			return;
		}
		BoxCollider component = go.GetComponent<BoxCollider>();
		if (component != null)
		{
			UpdateWidgetCollider(component, considerInactive);
			return;
		}
		BoxCollider2D component2 = go.GetComponent<BoxCollider2D>();
		if (component2 != null)
		{
			UpdateWidgetCollider(component2, considerInactive);
		}
	}

	public static void UpdateWidgetCollider(BoxCollider box, bool considerInactive)
	{
		if (!(box != null))
		{
			return;
		}
		GameObject gameObject = box.gameObject;
		UIWidget component = gameObject.GetComponent<UIWidget>();
		if (component != null)
		{
			Vector4 drawRegion = component.drawRegion;
			if (drawRegion.x != 0f || drawRegion.y != 0f || drawRegion.z != 1f || drawRegion.w != 1f)
			{
				Vector4 drawingDimensions = component.drawingDimensions;
				box.center = new Vector3((drawingDimensions.x + drawingDimensions.z) * 0.5f, (drawingDimensions.y + drawingDimensions.w) * 0.5f);
				box.size = new Vector3(drawingDimensions.z - drawingDimensions.x, drawingDimensions.w - drawingDimensions.y);
			}
			else
			{
				Vector3[] localCorners = component.localCorners;
				box.center = Vector3.Lerp(localCorners[0], localCorners[2], 0.5f);
				box.size = localCorners[2] - localCorners[0];
			}
		}
		else
		{
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(gameObject.transform, considerInactive);
			box.center = bounds.center;
			box.size = new Vector3(bounds.size.x, bounds.size.y, 0f);
		}
	}

	public static void UpdateWidgetCollider(BoxCollider2D box, bool considerInactive)
	{
		if (box != null)
		{
			GameObject gameObject = box.gameObject;
			UIWidget component = gameObject.GetComponent<UIWidget>();
			if (component != null)
			{
				Vector3[] localCorners = component.localCorners;
				box.center = Vector3.Lerp(localCorners[0], localCorners[2], 0.5f);
				box.size = localCorners[2] - localCorners[0];
			}
			else
			{
				Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(gameObject.transform, considerInactive);
				box.center = bounds.center;
				box.size = new Vector2(bounds.size.x, bounds.size.y);
			}
		}
	}

	public static string GetTypeName<T>()
	{
		string text = typeof(T).ToString();
		if (text.StartsWith("UI"))
		{
			text = text.Substring(2);
		}
		else if (text.StartsWith("UnityEngine."))
		{
			text = text.Substring(12);
		}
		return text;
	}

	public static string GetTypeName(UnityEngine.Object obj)
	{
		if (obj == null)
		{
			return "Null";
		}
		string text = obj.GetType().ToString();
		if (text.StartsWith("UI"))
		{
			text = text.Substring(2);
		}
		else if (text.StartsWith("UnityEngine."))
		{
			text = text.Substring(12);
		}
		return text;
	}

	public static void RegisterUndo(UnityEngine.Object obj, string name)
	{
	}

	public static void SetDirty(UnityEngine.Object obj)
	{
	}

	public static GameObject AddChild(GameObject parent)
	{
		return AddChild(parent, undo: true);
	}

	public static GameObject AddChild(GameObject parent, bool undo)
	{
		GameObject gameObject = new GameObject();
		if (parent != null)
		{
			Transform transform = gameObject.transform;
			transform.parent = parent.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			gameObject.layer = parent.layer;
		}
		return gameObject;
	}

	public static GameObject AddChild(GameObject parent, GameObject prefab)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(prefab) as GameObject;
		if (gameObject != null && parent != null)
		{
			Transform transform = gameObject.transform;
			transform.parent = parent.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			gameObject.layer = parent.layer;
		}
		return gameObject;
	}

	public static int CalculateRaycastDepth(GameObject go)
	{
		UIWidget component = go.GetComponent<UIWidget>();
		if (component != null)
		{
			return component.raycastDepth;
		}
		UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
		if (componentsInChildren.Length == 0)
		{
			return 0;
		}
		int num = int.MaxValue;
		int i = 0;
		for (int num2 = componentsInChildren.Length; i < num2; i++)
		{
			if (componentsInChildren[i].enabled)
			{
				num = Mathf.Min(num, componentsInChildren[i].raycastDepth);
			}
		}
		return num;
	}

	public static int CalculateNextDepth(GameObject go)
	{
		int num = -1;
		UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
		int i = 0;
		for (int num2 = componentsInChildren.Length; i < num2; i++)
		{
			num = Mathf.Max(num, componentsInChildren[i].depth);
		}
		return num + 1;
	}

	public static int CalculateNextDepth(GameObject go, bool ignoreChildrenWithColliders)
	{
		if (ignoreChildrenWithColliders)
		{
			int num = -1;
			UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
			int i = 0;
			for (int num2 = componentsInChildren.Length; i < num2; i++)
			{
				UIWidget uIWidget = componentsInChildren[i];
				if (!(uIWidget.cachedGameObject != go) || (!(uIWidget.collider != null) && !(uIWidget.GetComponent<Collider2D>() != null)))
				{
					num = Mathf.Max(num, uIWidget.depth);
				}
			}
			return num + 1;
		}
		return CalculateNextDepth(go);
	}

	public static int AdjustDepth(GameObject go, int adjustment)
	{
		if (go != null)
		{
			UIPanel component = go.GetComponent<UIPanel>();
			if (component != null)
			{
				UIPanel[] componentsInChildren = go.GetComponentsInChildren<UIPanel>(includeInactive: true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].depth += adjustment;
				}
				return 1;
			}
			component = FindInParents<UIPanel>(go);
			if (component == null)
			{
				return 0;
			}
			UIWidget[] componentsInChildren2 = go.GetComponentsInChildren<UIWidget>(includeInactive: true);
			int j = 0;
			for (int num = componentsInChildren2.Length; j < num; j++)
			{
				UIWidget uIWidget = componentsInChildren2[j];
				if (!(uIWidget.panel != component))
				{
					uIWidget.depth += adjustment;
				}
			}
			return 2;
		}
		return 0;
	}

	public static void BringForward(GameObject go)
	{
		switch (AdjustDepth(go, 1000))
		{
		case 1:
			NormalizePanelDepths();
			break;
		case 2:
			NormalizeWidgetDepths();
			break;
		}
	}

	public static void PushBack(GameObject go)
	{
		switch (AdjustDepth(go, -1000))
		{
		case 1:
			NormalizePanelDepths();
			break;
		case 2:
			NormalizeWidgetDepths();
			break;
		}
	}

	public static void NormalizeDepths()
	{
		NormalizeWidgetDepths();
		NormalizePanelDepths();
	}

	public static void NormalizeWidgetDepths()
	{
		NormalizeWidgetDepths(FindActive<UIWidget>());
	}

	public static void NormalizeWidgetDepths(GameObject go)
	{
		NormalizeWidgetDepths(go.GetComponentsInChildren<UIWidget>());
	}

	public static void NormalizeWidgetDepths(UIWidget[] list)
	{
		int num = list.Length;
		if (num <= 0)
		{
			return;
		}
		Array.Sort(list, UIWidget.FullCompareFunc);
		int num2 = 0;
		int depth = list[0].depth;
		for (int i = 0; i < num; i++)
		{
			UIWidget uIWidget = list[i];
			if (uIWidget.depth == depth)
			{
				uIWidget.depth = num2;
				continue;
			}
			depth = uIWidget.depth;
			num2 = (uIWidget.depth = num2 + 1);
		}
	}

	public static void NormalizePanelDepths()
	{
		UIPanel[] array = FindActive<UIPanel>();
		int num = array.Length;
		if (num <= 0)
		{
			return;
		}
		Array.Sort(array, UIPanel.CompareFunc);
		int num2 = 0;
		int depth = array[0].depth;
		for (int i = 0; i < num; i++)
		{
			UIPanel uIPanel = array[i];
			if (uIPanel.depth == depth)
			{
				uIPanel.depth = num2;
				continue;
			}
			depth = uIPanel.depth;
			num2 = (uIPanel.depth = num2 + 1);
		}
	}

	public static UIPanel CreateUI(bool advanced3D)
	{
		return CreateUI(null, advanced3D, -1);
	}

	public static UIPanel CreateUI(bool advanced3D, int layer)
	{
		return CreateUI(null, advanced3D, layer);
	}

	public static UIPanel CreateUI(Transform trans, bool advanced3D, int layer)
	{
		UIRoot uIRoot = ((!(trans != null)) ? null : FindInParents<UIRoot>(trans.gameObject));
		if (uIRoot == null && UIRoot.list.Count > 0)
		{
			foreach (UIRoot item in UIRoot.list)
			{
				if (item.gameObject.layer == layer)
				{
					uIRoot = item;
					break;
				}
			}
		}
		if (uIRoot == null)
		{
			int i = 0;
			for (int count = UIPanel.list.Count; i < count; i++)
			{
				UIPanel uIPanel = UIPanel.list[i];
				GameObject gameObject = uIPanel.gameObject;
				if (gameObject.hideFlags == HideFlags.None && gameObject.layer == layer)
				{
					trans.parent = uIPanel.transform;
					trans.localScale = Vector3.one;
					return uIPanel;
				}
			}
		}
		if (uIRoot != null)
		{
			UICamera componentInChildren = uIRoot.GetComponentInChildren<UICamera>();
			if (componentInChildren != null && componentInChildren.camera.isOrthoGraphic == advanced3D)
			{
				trans = null;
				uIRoot = null;
			}
		}
		if (uIRoot == null)
		{
			GameObject gameObject2 = AddChild(null, undo: false);
			uIRoot = gameObject2.AddComponent<UIRoot>();
			if (layer == -1)
			{
				layer = LayerMask.NameToLayer("UI");
			}
			if (layer == -1)
			{
				layer = LayerMask.NameToLayer("2D UI");
			}
			gameObject2.layer = layer;
			if (advanced3D)
			{
				gameObject2.name = "UI Root (3D)";
				uIRoot.scalingStyle = UIRoot.Scaling.Constrained;
			}
			else
			{
				gameObject2.name = "UI Root";
				uIRoot.scalingStyle = UIRoot.Scaling.Flexible;
			}
		}
		UIPanel uIPanel2 = uIRoot.GetComponentInChildren<UIPanel>();
		if (uIPanel2 == null)
		{
			Camera[] array = FindActive<Camera>();
			float num = -1f;
			bool flag = false;
			int num2 = 1 << uIRoot.gameObject.layer;
			foreach (Camera camera in array)
			{
				if (camera.clearFlags == CameraClearFlags.Color || camera.clearFlags == CameraClearFlags.Skybox)
				{
					flag = true;
				}
				num = Mathf.Max(num, camera.depth);
				camera.cullingMask &= ~num2;
			}
			Camera camera2 = AddChild<Camera>(uIRoot.gameObject, undo: false);
			camera2.gameObject.AddComponent<UICamera>();
			camera2.clearFlags = ((!flag) ? CameraClearFlags.Color : CameraClearFlags.Depth);
			camera2.backgroundColor = Color.grey;
			camera2.cullingMask = num2;
			camera2.depth = num + 1f;
			if (advanced3D)
			{
				camera2.nearClipPlane = 0.1f;
				camera2.farClipPlane = 4f;
				camera2.transform.localPosition = new Vector3(0f, 0f, -700f);
			}
			else
			{
				camera2.orthographic = true;
				camera2.orthographicSize = 1f;
				camera2.nearClipPlane = -10f;
				camera2.farClipPlane = 10f;
			}
			AudioListener[] array2 = FindActive<AudioListener>();
			if (array2 == null || array2.Length == 0)
			{
				camera2.gameObject.AddComponent<AudioListener>();
			}
			uIPanel2 = uIRoot.gameObject.AddComponent<UIPanel>();
		}
		if (trans != null)
		{
			while (trans.parent != null)
			{
				trans = trans.parent;
			}
			if (IsChild(trans, uIPanel2.transform))
			{
				uIPanel2 = trans.gameObject.AddComponent<UIPanel>();
			}
			else
			{
				trans.parent = uIPanel2.transform;
				trans.localScale = Vector3.one;
				trans.localPosition = Vector3.zero;
				SetChildLayer(uIPanel2.cachedTransform, uIPanel2.cachedGameObject.layer);
			}
		}
		return uIPanel2;
	}

	public static void SetChildLayer(Transform t, int layer)
	{
		for (int i = 0; i < t.childCount; i++)
		{
			Transform child = t.GetChild(i);
			child.gameObject.layer = layer;
			SetChildLayer(child, layer);
		}
	}

	public static T AddChild<T>(GameObject parent) where T : Component
	{
		GameObject gameObject = AddChild(parent);
		gameObject.name = GetTypeName<T>();
		return gameObject.AddComponent<T>();
	}

	public static T AddChild<T>(GameObject parent, bool undo) where T : Component
	{
		GameObject gameObject = AddChild(parent, undo);
		gameObject.name = GetTypeName<T>();
		return gameObject.AddComponent<T>();
	}

	public static T AddWidget<T>(GameObject go) where T : UIWidget
	{
		int depth = CalculateNextDepth(go);
		T result = AddChild<T>(go);
		result.width = 100;
		result.height = 100;
		result.depth = depth;
		return result;
	}

	public static T AddWidget<T>(GameObject go, int depth) where T : UIWidget
	{
		T result = AddChild<T>(go);
		result.width = 100;
		result.height = 100;
		result.depth = depth;
		return result;
	}

	public static UISprite AddSprite(GameObject go, UIAtlas atlas, string spriteName)
	{
		UISpriteData uISpriteData = ((!(atlas != null)) ? null : atlas.GetSprite(spriteName));
		UISprite uISprite = AddWidget<UISprite>(go);
		uISprite.type = ((uISpriteData != null && uISpriteData.hasBorder) ? UIBasicSprite.Type.Sliced : UIBasicSprite.Type.Simple);
		uISprite.atlas = atlas;
		uISprite.spriteName = spriteName;
		return uISprite;
	}

	public static GameObject GetRoot(GameObject go)
	{
		Transform transform = go.transform;
		while (true)
		{
			Transform parent = transform.parent;
			if (parent == null)
			{
				break;
			}
			transform = parent;
		}
		return transform.gameObject;
	}

	public static T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null)
		{
			return (T)null;
		}
		T component = go.GetComponent<T>();
		if ((UnityEngine.Object)component == (UnityEngine.Object)null)
		{
			Transform parent = go.transform.parent;
			while (parent != null && (UnityEngine.Object)component == (UnityEngine.Object)null)
			{
				component = parent.gameObject.GetComponent<T>();
				parent = parent.parent;
			}
		}
		return component;
	}

	public static T FindInParents<T>(Transform trans) where T : Component
	{
		if (trans == null)
		{
			return (T)null;
		}
		return trans.GetComponentInParent<T>();
	}

	public static void Destroy(UnityEngine.Object obj)
	{
		if (!(obj != null))
		{
			return;
		}
		if (obj is Transform)
		{
			obj = (obj as Transform).gameObject;
		}
		if (Application.isPlaying)
		{
			if (obj is GameObject)
			{
				GameObject gameObject = obj as GameObject;
				gameObject.transform.parent = null;
			}
			UnityEngine.Object.Destroy(obj);
		}
		else
		{
			UnityEngine.Object.DestroyImmediate(obj);
		}
	}

	public static void DestroyImmediate(UnityEngine.Object obj)
	{
		if (obj != null)
		{
			if (Application.isEditor)
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
			else
			{
				UnityEngine.Object.Destroy(obj);
			}
		}
	}

	public static void Broadcast(string funcName)
	{
		GameObject[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			array[i].SendMessage(funcName, SendMessageOptions.DontRequireReceiver);
		}
	}

	public static void Broadcast(string funcName, object param)
	{
		GameObject[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			array[i].SendMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
		}
	}

	public static bool IsChild(Transform parent, Transform child)
	{
		if (parent == null || child == null)
		{
			return false;
		}
		while (child != null)
		{
			if (child == parent)
			{
				return true;
			}
			child = child.parent;
		}
		return false;
	}

	private static void Activate(Transform t)
	{
		Activate(t, compatibilityMode: false);
	}

	private static void Activate(Transform t, bool compatibilityMode)
	{
		SetActiveSelf(t.gameObject, state: true);
		if (!compatibilityMode)
		{
			return;
		}
		int i = 0;
		for (int childCount = t.childCount; i < childCount; i++)
		{
			Transform child = t.GetChild(i);
			if (child.gameObject.activeSelf)
			{
				return;
			}
		}
		int j = 0;
		for (int childCount2 = t.childCount; j < childCount2; j++)
		{
			Transform child2 = t.GetChild(j);
			Activate(child2, compatibilityMode: true);
		}
	}

	private static void Deactivate(Transform t)
	{
		SetActiveSelf(t.gameObject, state: false);
	}

	public static void SetActive(GameObject go, bool state)
	{
		SetActive(go, state, compatibilityMode: true);
	}

	public static void SetActive(GameObject go, bool state, bool compatibilityMode)
	{
		if ((bool)go)
		{
			if (state)
			{
				Activate(go.transform, compatibilityMode);
				CallCreatePanel(go.transform);
			}
			else
			{
				Deactivate(go.transform);
			}
		}
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	private static void CallCreatePanel(Transform t)
	{
		UIWidget component = t.GetComponent<UIWidget>();
		if (component != null)
		{
			component.CreatePanel();
		}
		int i = 0;
		for (int childCount = t.childCount; i < childCount; i++)
		{
			CallCreatePanel(t.GetChild(i));
		}
	}

	public static void SetActiveChildren(GameObject go, bool state)
	{
		Transform transform = go.transform;
		if (state)
		{
			int i = 0;
			for (int childCount = transform.childCount; i < childCount; i++)
			{
				Transform child = transform.GetChild(i);
				Activate(child);
			}
		}
		else
		{
			int j = 0;
			for (int childCount2 = transform.childCount; j < childCount2; j++)
			{
				Transform child2 = transform.GetChild(j);
				Deactivate(child2);
			}
		}
	}

	[Obsolete("Use NGUITools.GetActive instead")]
	public static bool IsActive(Behaviour mb)
	{
		return mb != null && mb.enabled && mb.gameObject.activeInHierarchy;
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static bool GetActive(Behaviour mb)
	{
		return (bool)mb && mb.enabled && mb.gameObject.activeInHierarchy;
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static bool GetActive(GameObject go)
	{
		return (bool)go && go.activeInHierarchy;
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static void SetActiveSelf(GameObject go, bool state)
	{
		go.SetActive(state);
	}

	public static void SetLayer(GameObject go, int layer)
	{
		go.layer = layer;
		Transform transform = go.transform;
		int i = 0;
		for (int childCount = transform.childCount; i < childCount; i++)
		{
			Transform child = transform.GetChild(i);
			SetLayer(child.gameObject, layer);
		}
	}

	public static Vector3 Round(Vector3 v)
	{
		v.x = Mathf.Round(v.x);
		v.y = Mathf.Round(v.y);
		v.z = Mathf.Round(v.z);
		return v;
	}

	public static void MakePixelPerfect(Transform t)
	{
		UIWidget component = t.GetComponent<UIWidget>();
		if (component != null)
		{
			component.MakePixelPerfect();
		}
		if (t.GetComponent<UIAnchor>() == null && t.GetComponent<UIRoot>() == null)
		{
			t.localPosition = Round(t.localPosition);
			t.localScale = Round(t.localScale);
		}
		int i = 0;
		for (int childCount = t.childCount; i < childCount; i++)
		{
			MakePixelPerfect(t.GetChild(i));
		}
	}

	public static bool Save(string fileName, byte[] bytes)
	{
		//Discarded unreachable code: IL_0057
		if (!fileAccess)
		{
			return false;
		}
		string path = Application.persistentDataPath + "/" + fileName;
		if (bytes == null)
		{
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			return true;
		}
		FileStream fileStream = null;
		try
		{
			fileStream = File.Create(path);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError(ex.Message);
			return false;
		}
		fileStream.Write(bytes, 0, bytes.Length);
		fileStream.Close();
		return true;
	}

	public static byte[] Load(string fileName)
	{
		if (!fileAccess)
		{
			return null;
		}
		string path = Application.persistentDataPath + "/" + fileName;
		if (File.Exists(path))
		{
			return File.ReadAllBytes(path);
		}
		return null;
	}

	public static Color ApplyPMA(Color c)
	{
		if (c.a != 1f)
		{
			c.r *= c.a;
			c.g *= c.a;
			c.b *= c.a;
		}
		return c;
	}

	public static void MarkParentAsChanged(GameObject go)
	{
		UIRect[] componentsInChildren = go.GetComponentsInChildren<UIRect>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			componentsInChildren[i].ParentHasChanged();
		}
	}

	[Obsolete("Use NGUIText.EncodeColor instead")]
	public static string EncodeColor(Color c)
	{
		return NGUIText.EncodeColor24(c);
	}

	[Obsolete("Use NGUIText.ParseColor instead")]
	public static Color ParseColor(string text, int offset)
	{
		return NGUIText.ParseColor24(text, offset);
	}

	[Obsolete("Use NGUIText.StripSymbols instead")]
	public static string StripSymbols(string text)
	{
		return NGUIText.StripSymbols(text);
	}

	public static T AddMissingComponent<T>(this GameObject go) where T : Component
	{
		T val = go.GetComponent<T>();
		if ((UnityEngine.Object)val == (UnityEngine.Object)null)
		{
			val = go.AddComponent<T>();
		}
		return val;
	}

	public static Vector3[] GetSides(this Camera cam)
	{
		return cam.GetSides(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), null);
	}

	public static Vector3[] GetSides(this Camera cam, float depth)
	{
		return cam.GetSides(depth, null);
	}

	public static Vector3[] GetSides(this Camera cam, Transform relativeTo)
	{
		return cam.GetSides(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), relativeTo);
	}

	public static Vector3[] GetSides(this Camera cam, float depth, Transform relativeTo)
	{
		if (cam.isOrthoGraphic)
		{
			float orthographicSize = cam.orthographicSize;
			float num = 0f - orthographicSize;
			float num2 = orthographicSize;
			float y = 0f - orthographicSize;
			float y2 = orthographicSize;
			Rect rect = cam.rect;
			Vector2 vector = screenSize;
			float num3 = vector.x / vector.y;
			num3 *= rect.width / rect.height;
			num *= num3;
			num2 *= num3;
			Transform transform = cam.transform;
			Quaternion rotation = transform.rotation;
			Vector3 position = transform.position;
			int num4 = Mathf.RoundToInt(vector.x);
			int num5 = Mathf.RoundToInt(vector.y);
			if ((num4 & 1) == 1)
			{
				position.x -= 1f / vector.x;
			}
			if ((num5 & 1) == 1)
			{
				position.y += 1f / vector.y;
			}
			ref Vector3 reference = ref mSides[0];
			reference = rotation * new Vector3(num, 0f, depth) + position;
			ref Vector3 reference2 = ref mSides[1];
			reference2 = rotation * new Vector3(0f, y2, depth) + position;
			ref Vector3 reference3 = ref mSides[2];
			reference3 = rotation * new Vector3(num2, 0f, depth) + position;
			ref Vector3 reference4 = ref mSides[3];
			reference4 = rotation * new Vector3(0f, y, depth) + position;
		}
		else
		{
			ref Vector3 reference5 = ref mSides[0];
			reference5 = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, depth));
			ref Vector3 reference6 = ref mSides[1];
			reference6 = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, depth));
			ref Vector3 reference7 = ref mSides[2];
			reference7 = cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, depth));
			ref Vector3 reference8 = ref mSides[3];
			reference8 = cam.ViewportToWorldPoint(new Vector3(0.5f, 0f, depth));
		}
		if (relativeTo != null)
		{
			for (int i = 0; i < 4; i++)
			{
				ref Vector3 reference9 = ref mSides[i];
				reference9 = relativeTo.InverseTransformPoint(mSides[i]);
			}
		}
		return mSides;
	}

	public static Vector3[] GetWorldCorners(this Camera cam)
	{
		float depth = Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f);
		return cam.GetWorldCorners(depth, null);
	}

	public static Vector3[] GetWorldCorners(this Camera cam, float depth)
	{
		return cam.GetWorldCorners(depth, null);
	}

	public static Vector3[] GetWorldCorners(this Camera cam, Transform relativeTo)
	{
		return cam.GetWorldCorners(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), relativeTo);
	}

	public static Vector3[] GetWorldCorners(this Camera cam, float depth, Transform relativeTo)
	{
		if (cam.isOrthoGraphic)
		{
			float orthographicSize = cam.orthographicSize;
			float num = 0f - orthographicSize;
			float num2 = orthographicSize;
			float y = 0f - orthographicSize;
			float y2 = orthographicSize;
			Rect rect = cam.rect;
			Vector2 vector = screenSize;
			float num3 = vector.x / vector.y;
			num3 *= rect.width / rect.height;
			num *= num3;
			num2 *= num3;
			Transform transform = cam.transform;
			Quaternion rotation = transform.rotation;
			Vector3 position = transform.position;
			ref Vector3 reference = ref mSides[0];
			reference = rotation * new Vector3(num, y, depth) + position;
			ref Vector3 reference2 = ref mSides[1];
			reference2 = rotation * new Vector3(num, y2, depth) + position;
			ref Vector3 reference3 = ref mSides[2];
			reference3 = rotation * new Vector3(num2, y2, depth) + position;
			ref Vector3 reference4 = ref mSides[3];
			reference4 = rotation * new Vector3(num2, y, depth) + position;
		}
		else
		{
			ref Vector3 reference5 = ref mSides[0];
			reference5 = cam.ViewportToWorldPoint(new Vector3(0f, 0f, depth));
			ref Vector3 reference6 = ref mSides[1];
			reference6 = cam.ViewportToWorldPoint(new Vector3(0f, 1f, depth));
			ref Vector3 reference7 = ref mSides[2];
			reference7 = cam.ViewportToWorldPoint(new Vector3(1f, 1f, depth));
			ref Vector3 reference8 = ref mSides[3];
			reference8 = cam.ViewportToWorldPoint(new Vector3(1f, 0f, depth));
		}
		if (relativeTo != null)
		{
			for (int i = 0; i < 4; i++)
			{
				ref Vector3 reference9 = ref mSides[i];
				reference9 = relativeTo.InverseTransformPoint(mSides[i]);
			}
		}
		return mSides;
	}

	public static string GetFuncName(object obj, string method)
	{
		if (obj == null)
		{
			return "<null>";
		}
		string text = obj.GetType().ToString();
		int num = text.LastIndexOf('/');
		if (num > 0)
		{
			text = text.Substring(num + 1);
		}
		return (!string.IsNullOrEmpty(method)) ? (text + "/" + method) : text;
	}

	public static void Execute<T>(GameObject go, string funcName) where T : Component
	{
		T[] components = go.GetComponents<T>();
		T[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			T obj = array[i];
			obj.GetType().GetMethod(funcName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.Invoke(obj, null);
		}
	}

	public static void ExecuteAll<T>(GameObject root, string funcName) where T : Component
	{
		Execute<T>(root, funcName);
		Transform transform = root.transform;
		int i = 0;
		for (int childCount = transform.childCount; i < childCount; i++)
		{
			ExecuteAll<T>(transform.GetChild(i).gameObject, funcName);
		}
	}

	public static void ImmediatelyCreateDrawCalls(GameObject root)
	{
		ExecuteAll<UIWidget>(root, "Start");
		ExecuteAll<UIPanel>(root, "Start");
		ExecuteAll<UIWidget>(root, "Update");
		ExecuteAll<UIPanel>(root, "Update");
		ExecuteAll<UIPanel>(root, "LateUpdate");
	}
}
