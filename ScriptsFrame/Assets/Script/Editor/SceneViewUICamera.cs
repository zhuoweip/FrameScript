#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// 编辑器UIMode快速切换到UI界面
/// </summary>

[InitializeOnLoad]
public static class SceneViewUICamera
{
	#region Initialization

	private static bool IsInitialized = false;

	private static bool InitializeIfRequired()
	{
		var activeSceneView = SceneView.lastActiveSceneView;

		if (IsInitialized)
		{
			if (activeSceneView == null)
			{
				IsInitialized = false;
				return false;
			}
			return true;
		}
		else
		{
			if (activeSceneView == null)
				return false;

			LastViewPositionBeforeCanvasSelection = activeSceneView.pivot;
			LastViewRotationBeforeCanvasSelection = activeSceneView.rotation;

			LoadPrefs();
			IsInitialized = true;
			return true;
		}
	}

	static SceneViewUICamera()
	{
		EditorApplication.update += Update;
		SceneView.onSceneGUIDelegate += GUISceneView;
	}
	#endregion

	#region Save-Load Prefs

	static void SavePrefs()
	{
		PlayerPrefs.SetInt("SceneViewUICamera_IsActive", Convert.ToInt32(IsActive));
		PlayerPrefs.SetInt("SceneViewUICamera_IsOrthographic", Convert.ToInt32(IsOrthographic));

		PlayerPrefs.SetInt("SceneViewUICamera_IsViewSaved", Convert.ToInt32(IsViewSaved));

		PlayerPrefs.SetInt("LastViewOrthoBeforeCanvasSelection", Convert.ToInt32(LastViewOrthoBeforeCanvasSelection));
		PlayerPrefs.SetFloat("LastViewSizeBeforeCanvasSelection", LastViewSizeBeforeCanvasSelection);

		PlayerPrefs.SetFloat("LastViewPositionBeforeCanvasSelectionX", LastViewPositionBeforeCanvasSelection.x);
		PlayerPrefs.SetFloat("LastViewPositionBeforeCanvasSelectionY", LastViewPositionBeforeCanvasSelection.y);
		PlayerPrefs.SetFloat("LastViewPositionBeforeCanvasSelectionZ", LastViewPositionBeforeCanvasSelection.z);

		PlayerPrefs.SetFloat("LastViewRotationBeforeCanvasSelectionX", LastViewRotationBeforeCanvasSelection.x);
		PlayerPrefs.SetFloat("LastViewRotationBeforeCanvasSelectionY", LastViewRotationBeforeCanvasSelection.y);
		PlayerPrefs.SetFloat("LastViewRotationBeforeCanvasSelectionZ", LastViewRotationBeforeCanvasSelection.z);
		PlayerPrefs.SetFloat("LastViewRotationBeforeCanvasSelectionW", LastViewRotationBeforeCanvasSelection.w);

		PlayerPrefs.SetInt("SceneViewUICamera_IndexOf_SavedSceneView", SceneView.sceneViews.IndexOf(SavedSceneView));
	}

	static void LoadPrefs()
	{
		IsActive = Convert.ToBoolean(PlayerPrefs.GetInt("SceneViewUICamera_IsActive"));
		IsOrthographic = Convert.ToBoolean(PlayerPrefs.GetInt("SceneViewUICamera_IsOrthographic"));
		bool savedIsViewSaved = Convert.ToBoolean(PlayerPrefs.GetInt("SceneViewUICamera_IsViewSaved"));

		if (savedIsViewSaved)
		{
			bool orthog = Convert.ToBoolean(PlayerPrefs.GetInt("LastViewOrthoBeforeCanvasSelection"));
			float size = PlayerPrefs.GetFloat("LastViewSizeBeforeCanvasSelection");

			Vector3 pos = new Vector3(PlayerPrefs.GetFloat("LastViewPositionBeforeCanvasSelectionX"),
									  PlayerPrefs.GetFloat("LastViewPositionBeforeCanvasSelectionY"),
									  PlayerPrefs.GetFloat("LastViewPositionBeforeCanvasSelectionZ"));

			Quaternion rot = new Quaternion(PlayerPrefs.GetFloat("LastViewRotationBeforeCanvasSelectionX"),
											PlayerPrefs.GetFloat("LastViewRotationBeforeCanvasSelectionY"),
											PlayerPrefs.GetFloat("LastViewRotationBeforeCanvasSelectionZ"),
											PlayerPrefs.GetFloat("LastViewRotationBeforeCanvasSelectionW")) ;

			IsViewSaved = savedIsViewSaved;
			LastViewOrthoBeforeCanvasSelection = orthog;
			LastViewSizeBeforeCanvasSelection = size;
			LastViewPositionBeforeCanvasSelection = pos;
			LastViewRotationBeforeCanvasSelection = rot;

			SavedSceneView = (SceneView)SceneView.sceneViews[PlayerPrefs.GetInt("SceneViewUICamera_IndexOf_SavedSceneView")];
		}
	}

	#endregion

	#region GUI

	private static bool IsActive = true;
	private static bool IsOrthographic = true;

	static void GUISceneView(SceneView sceneview)
	{
		Handles.BeginGUI();

		if (IsActive && IsViewSaved)
		{
			GUI.color = Color.cyan;
		}
		else
		{
			GUI.color = Color.white;
		}

		GUI.Box(new Rect(1, 1, 70, 34), "");

		if(GUI.Toggle(new Rect(1, 1, 70, 17), IsActive, "UI mode") != IsActive)
		{
			IsActive = !IsActive;
			SavePrefs();
		}

		if (GUI.Toggle(new Rect(1, 18, 70, 17), IsOrthographic, "Iso") != IsOrthographic)
		{
			IsOrthographic = !IsOrthographic;
			SavePrefs();
			CheckSelection(true);
		}

		Handles.EndGUI();
	}

	#endregion

	#region Update

	private static void Update()
	{

		if (!InitializeIfRequired())
		{
			return;
		}

		if (IsActive)
		{
			CheckSelection(false);
		}
		if (!IsActive && IsViewSaved)
		{
			LoadView();
			CurrentSelection = null;
			CurrentSelectedCanvas = null;
		}
	}

	#endregion

	#region Selection

	private static Transform CurrentSelection;
	private static Canvas CurrentSelectedCanvas;

	private static void CheckSelection(bool ignoreCurrentSelection)
	{
		if (!ignoreCurrentSelection)
		{
			if (CurrentSelection == Selection.activeTransform)
				return;
		}

		CurrentSelection = Selection.activeTransform;
		if (CurrentSelection == null)
		{
			CurrentSelectedCanvas = null;
			LoadView();
			return;
		}

		var canvas = CurrentSelection.GetComponentInParent<Canvas>();
		if (canvas == null)
		{
			LoadView();
			CurrentSelectedCanvas = null;
		}
		else
		{
			if (CurrentSelectedCanvas != null)
			{
				if (canvas != CurrentSelectedCanvas)
				{
					LookAtCanvas(canvas.GetComponent<RectTransform>());
					CurrentSelectedCanvas = canvas;
				}
				else
				{
					var canvasOwn = CurrentSelection.GetComponent<Canvas>();
					if (canvasOwn != null)
					{
						LookAtCanvas(canvas.GetComponent<RectTransform>());
						CurrentSelectedCanvas = canvasOwn;
					}
					else if (IsOrthographic != SavedSceneView.orthographic) 
					{
						LookAtCanvas(canvas.GetComponent<RectTransform>());
					}
				}
			}
			else
			{
				SaveView();
				LookAtCanvas(canvas.GetComponent<RectTransform>());
				CurrentSelectedCanvas = canvas;
			}
		}
	}

	#endregion

	#region Save/Load View

	private static bool IsViewSaved;
	private static Vector3 LastViewPositionBeforeCanvasSelection;
	private static Quaternion LastViewRotationBeforeCanvasSelection;
	private static float LastViewSizeBeforeCanvasSelection;
	private static bool LastViewOrthoBeforeCanvasSelection;
	private static SceneView SavedSceneView;

	private static void SaveView()
	{
		var activeSceneView = SceneView.lastActiveSceneView;

		if (IsViewSaved)
		{
			if (activeSceneView == SavedSceneView)
				return;

			LoadView();
		}

		IsViewSaved = true;
		SavedSceneView = activeSceneView;

		LastViewPositionBeforeCanvasSelection = activeSceneView.pivot;
		LastViewRotationBeforeCanvasSelection = activeSceneView.rotation;
		LastViewSizeBeforeCanvasSelection = activeSceneView.size;
		LastViewOrthoBeforeCanvasSelection = activeSceneView.orthographic;
		
		SavePrefs();
	}

	private static void LoadView()
	{
		if (!IsViewSaved)
			return;
		IsViewSaved = false;

		if (SavedSceneView == null)
			return;

		SavedSceneView.LookAt(LastViewPositionBeforeCanvasSelection, LastViewRotationBeforeCanvasSelection, LastViewSizeBeforeCanvasSelection, LastViewOrthoBeforeCanvasSelection);
		SavedSceneView = null;

		SavePrefs();
	}

	#endregion

	#region Look At Canvas

	private static void LookAtCanvas(RectTransform rectTransform)
	{
		var activeSceneView = SceneView.lastActiveSceneView;

		var selectedDimension = rectTransform.rect.width > rectTransform.rect.height
			? rectTransform.rect.width
			: rectTransform.rect.height;

		if (!IsOrthographic)
		{
			var position = rectTransform.position - selectedDimension * (rectTransform.rotation * Vector3.forward) * rectTransform.lossyScale.x;
			var rotation = rectTransform.rotation;
			if (rectTransform.lossyScale.x > 0.1f) activeSceneView.LookAt(position, rotation, 2.0f, false);
			else activeSceneView.LookAt(position, rotation, 0.0f, false);
		}
		else
		{
			var position = rectTransform.position;
			var rotation = rectTransform.rotation;

			activeSceneView.LookAt(position, rotation, selectedDimension * rectTransform.lossyScale.x, true);
		}
	}

	#endregion
}

#endif
