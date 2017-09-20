using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
	Vector3 thisObjectsPosition;
	Quaternion thisObjectsRotation;
	float sway;
	float timer = 0;
	public static bool camStuff=false;

	void Start()
	{
		thisObjectsPosition = this.gameObject.transform.position;
		thisObjectsRotation = this.gameObject.transform.rotation;
	}
	void Update()
	{

		if(camStuff){
			timer += Time.deltaTime;

			float x = Mathf.Sin (timer);
			float y = Mathf.Cos (timer);

			if (timer>1000){timer = 0f;}
			thisObjectsPosition.x = x +16; 
			thisObjectsPosition.y = y +16;
//			thisObjectsRotation.x = Quaternion.Euler(x);
//			thisObjectsRotation.y = y;
//
//			Mathf.Clamp (thisObjectsRotation.x, -5, 5);
//			Mathf.Clamp (thisObjectsRotation.y, -5, 5);
//
//			this.gameObject.transform.rotation = thisObjectsRotation;
			this.gameObject.transform.position = thisObjectsPosition;
		}
	}
}