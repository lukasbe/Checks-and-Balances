using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

	//private float speedRot = 0; //a speed modifier
	//private float speedTrans = 3.0f;
	private Vector3 point; //the coord to the point where the camera looks at
	public bool isWhiteCam;

	void Start ()
	{
		//Set up things on start 
		point = new Vector3 (4.0f, 0.0f, 4.0f); //get target's coords
		AlignCamera();
	}

	/*
	void Update ()
	{
		//makes the camera rotate around "point" coords, rotating around its Y axis, 20 degrees per second times the speed modifier
		if (Input.GetKey (KeyCode.D)) {
			speedRot = -3.0f;
		} else if (Input.GetKey (KeyCode.A)) {
			speedRot = 3.0f;
		} else {
			speedRot = 0;
		}
		transform.RotateAround (point, new Vector3 (0.0f, 1.0f, 0.0f), 20 * Time.deltaTime * speedRot);

		if (Input.GetKey (KeyCode.S)) {
			if (transform.position.y >= 3) {
				Vector3 temp = transform.position;
				temp.y -= speedTrans * Time.deltaTime;
				transform.position = temp;
			}
		}
		if (Input.GetKey (KeyCode.W)) {
			if (transform.position.y <= 13.0f) {
				Vector3 temp = transform.position;
				temp.y += speedTrans * Time.deltaTime;
				transform.position = temp;
			}
		}
		transform.LookAt (point);
	}
	*/

	void Update() {
		if (!BoardManager.Instance.actionCamEnabled && BoardManager.Instance.gameOverCalled)
			AlignCamera ();
	}

	private void AlignCamera(){
		transform.position = point;
		Vector3 chessUp = BoardManager.Instance.transform.up;
		transform.position = point + (chessUp * 10);
		if(isWhiteCam)
			transform.RotateAround (point, new Vector3 (1.0f, 0.0f, 0.0f), 40.0f);
		else
			transform.RotateAround (point, new Vector3 (1.0f, 0.0f, 0.0f), -40.0f);
		transform.Translate (0.0f, (BoardManager.Instance.transform.localPosition.y), 0.0f);
		transform.LookAt (point); //makes the camera look to it
	}
}
