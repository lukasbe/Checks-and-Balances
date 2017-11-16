using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceHighlights : MonoBehaviour {

	public static BalanceHighlights Instance{ set; get;}

	public GameObject balanceHighlightPrefab;
	private List<GameObject> balanceHighlights;

	private const float TILE_OFFSET = 0.5f;

	private void Awake()
	{
		Instance = this;
		balanceHighlights = new List<GameObject> ();
	}


	private GameObject GetHighlightObject()
	{
		GameObject go = balanceHighlights.Find (g => !g.activeSelf);
		if (go == null) 
		{
			go = Instantiate (balanceHighlightPrefab);
			go.transform.SetParent (BoardManager.Instance.chessboard.transform);
			balanceHighlights.Add (go);
		}
		return go;
	}


	public void HighlightBalanceFields(){
		for (int i = 0; i < 8; i++) 
		{
			for (int j = 3; j < 5; j++) {
				GameObject go = GetHighlightObject ();
				go.SetActive (true);
				go.transform.position = new Vector3 (i + TILE_OFFSET,-0.0809f, j + TILE_OFFSET);
			}
		} 
	}


}
