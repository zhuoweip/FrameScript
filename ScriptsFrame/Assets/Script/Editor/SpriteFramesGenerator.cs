using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.Linq;

public class SpriteFramesGenerator : MonoBehaviour {
	[MenuItem("Tools/Generate Sprite Frames")]
	public static void CheckText ()
	{
		List<UGUISpriteAnimation> Frames = new List<UGUISpriteAnimation>();
		MonoBehaviour[] monoScripts = FindObjectsOfType(typeof(MonoBehaviour)) as MonoBehaviour[];

		foreach(MonoBehaviour monoScript in monoScripts)
		{
			//				if(typeof(KinectGestures.GestureListenerInterface).IsAssignableFrom(monoScript.GetType()) &&
			//				   monoScript.enabled)
			if((monoScript is UGUISpriteAnimation) && monoScript.enabled)
			{
				//KinectGestures.GestureListenerInterface gl = (KinectGestures.GestureListenerInterface)monoScript;
				Frames.Add((UGUISpriteAnimation)monoScript);
			}
		}

		foreach (UGUISpriteAnimation Frame in Frames) {
			Debug.Log (Frame.SpritePath+" Start");
			//查找指定路径下指定类型的所有资源，返回的是资源GUID
			if (Frame.SpritePath.Length == 0)
				return;
			string[] guids = AssetDatabase.FindAssets ("t:Sprite", new string[] { Frame.SpritePath });
			//从GUID获得资源所在路径
			List<string> paths = new List<string> ();
			guids.ToList ().ForEach (m => paths.Add (AssetDatabase.GUIDToAssetPath (m)));
			//从路径获得该资源
			Frame.SpriteFrames = new List<Sprite> ();
			paths.ForEach (p => Frame.SpriteFrames.Add (AssetDatabase.LoadAssetAtPath (p, typeof(Sprite)) as Sprite));

			Frame.gameObject.GetComponent<Image> ().sprite = Frame.SpriteFrames [0];
			Debug.Log (Frame.SpritePath+" End");
		}
	}

}
