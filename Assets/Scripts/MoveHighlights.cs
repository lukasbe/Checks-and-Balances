using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHighlights : MonoBehaviour {

	public static MoveHighlights Instance{ set; get;}

	public GameObject highlightPrefab;
	private List<GameObject> highlights;
	private const float TILE_OFFSET = 0.5f;

	private void Start()
	{
		Instance = this;
		highlights = new List<GameObject> ();
	}

	private GameObject GetHighlightObject()
	{
		GameObject go = highlights.Find (g => !g.activeSelf);
		if (go == null) 
		{
			go = Instantiate (highlightPrefab);
			highlights.Add (go);
		}

		return go;
	}

	public void HighlightAllowedMoves(bool[,] moves)
	{
		for (int i = 0; i < 8; i++) 
		{
			for (int j = 0; j < 8; j++) 
			{
				if (moves [i, j]) 
				{
					GameObject go = GetHighlightObject ();
					go.SetActive (true);
					go.transform.position = new Vector3 (i + TILE_OFFSET, -0.0808f, j + TILE_OFFSET);
				}
			}
		}
	}

	public void HideHighlights()
	{
		foreach(GameObject go in highlights)
		{
			go.SetActive (false);
		}
	}
}
