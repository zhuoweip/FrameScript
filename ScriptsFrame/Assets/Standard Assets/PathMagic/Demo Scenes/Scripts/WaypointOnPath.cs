using UnityEngine;
using System.Collections;
using Jacovone;

public class WaypointOnPath : MonoBehaviour
{
	public PathMagic wpPm;
	public int wpIndex;

	public Transform target;


	// Update is called once per frame
	void Update ()
	{
		wpPm.Waypoints [wpIndex].position = wpPm.transform.InverseTransformPoint (target.position);
	}
}
