using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancePoint : MonoBehaviour {

	public float x { get; set;}
	public float y { get; set;}

	public void Start(){
		GetComponent<Renderer> ().enabled = false;
		x = 4.0f;
		y = 4.0f;
	}

	/// <summary>
	/// Calculates the balance point.
	/// Formula: for each line: (sum(weight(figures))) * lineindex (starting at 1) / (weight of all figures on the board)
	/// </summary>
	/// <param name="Chesspieces">chessboard and all the chesspieces on it</param>
	/// <param name="TILE_OFFSET">offset to place the balance point between chess tiles</param>
	public void CalculateBalancePoint(Chesspiece[,] Chesspieces, float TILE_OFFSET)
	{
		x = 4;
		y = 0;

		float totalFigureWeight = 0, totalFieldWeight = 0;

		for (int i = 0; i < 8; i++) {
			float lineWeight = 0;
			for (int j = 0; j < 8; j++) {
				//change j and i here to get a line scan instead of a column scan
				Chesspiece c = Chesspieces [j, i];
				if (c != null) {
					lineWeight += c.GetWeight ();
				}
			}
			totalFigureWeight += lineWeight;
			//index + 1 to map to [1,8]
			totalFieldWeight += lineWeight * (i + 1);
		}
		//-1 to map back to [0,7]
		y = (totalFieldWeight / totalFigureWeight) - 1;
		y += TILE_OFFSET;

		MoveBalancePoint (x, y);
	}
		
	public Vector3 MoveBalancePoint(float x, float y)
	{
		transform.position = new Vector3 (x, 0, y);
		return transform.position;
	}

	public void initBalancePointPosition()
	{
		MoveBalancePoint (x, y);
	}


}
