using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessplaneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = BoardManager.Instance.chessboard.transform.position;
		transform.rotation = BoardManager.Instance.chessboard.transform.rotation;
	}
}
