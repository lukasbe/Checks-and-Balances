using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchHandManager : MonoBehaviour {

	GameObject chessBoard;

	// Use this for initialization
	void Start () {
		chessBoard = GameObject.Find ("chessboard");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.up = chessBoard.transform.up;
		transform.Rotate (90.0f, 0.0f, 0.0f);
	}
}
