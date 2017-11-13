using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancePoint : MonoBehaviour {

	private float balancePoint_x = 4.0f;
	private float balancePoint_y = 4.0f;

	/// <summary>
	/// Calculates the balance point.
	/// Formula: for each line: (sum(weight(figures))) * lineindex (starting at 1) / (weight of all figures on the board)
	/// </summary>
	/// <param name="Chesspieces">chessboard and all the chesspieces on it</param>
	/// <param name="TILE_OFFSET">offset to place the balance point between chess tiles</param>
	public void CalculateBalancePoint(Chesspiece[,] Chesspieces, float TILE_OFFSET)
	{
		balancePoint_x = 4;
		balancePoint_y = 0;

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
		balancePoint_y = (totalFieldWeight / totalFigureWeight) - 1;
		balancePoint_y += TILE_OFFSET;

		MoveBalancePoint (balancePoint_x, balancePoint_y);
	}
		
	public Vector3 MoveBalancePoint(float x, float y)
	{
		transform.position = new Vector3 (x, 0, y);
		return transform.position;
	}

	public void InitBalancePointPosition()
	{
		MoveBalancePoint (balancePoint_x, balancePoint_y);
	}

	public bool WithinBoarders(){
		return balancePoint_x >= 3 &&
		balancePoint_x <= 5 &&
		balancePoint_y >= 3 &&
		balancePoint_y <= 5;
	}

}
