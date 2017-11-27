using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHighlights : MonoBehaviour {

	public static MoveHighlights Instance{ set; get;}

	public GameObject highlightPrefab;
	public static GameObject[,] moveHighlights{ get; set;}
	private const float TILE_OFFSET = 0.5f;

	private void Start()
	{
		Instance = this;
		moveHighlights = new GameObject[8, 8];
		InitMoveHighlights ();
	}

	private void InitMoveHighlights(){
		for(int i = 0; i < 8; i++){
			for(int j = 0; j < 8; j++){
				GameObject go = GameObject.Instantiate (highlightPrefab) as GameObject;
				go.transform.position = new Vector3 (i + TILE_OFFSET, -0.08f, j + TILE_OFFSET);
				go.transform.SetParent (BoardManager.Instance.chessboard.transform);

				go.GetComponent<Renderer> ().enabled = false;
				moveHighlights [i, j] = go;
			}
		}
	}

	public void HighlightAllowedMoves(bool[,] moves)
	{
		for (int i = 0; i < 8; i++) 
		{
			for (int j = 0; j < 8; j++) 
			{
				if (moves [i, j]) 
				{
					GameObject go = moveHighlights[i,j];
					go.GetComponent<Renderer> ().enabled = true;
				}
			}
		}
	}

	public void HideHighlights()
	{
		foreach(GameObject go in moveHighlights)
		{
			go.GetComponent<Renderer> ().enabled = false;
		}
	}

	public void GetSelectionIndex(GameObject go, out int xPos, out int yPos){
		xPos = -1;
		yPos = -1;
		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				if (moveHighlights [i, j] == go) {
					xPos = i;
					yPos = j;
				}
			}
		}
	}

	public Vector3 GetTileCenter(int x, int y){
		return moveHighlights [x, y].GetComponent<Renderer> ().bounds.center;
	}
		
}
