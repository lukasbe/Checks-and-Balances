using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancePoint : MonoBehaviour {

	private float balancePoint_x = 4.0f;
	private float balancePoint_y = 4.0f;

	public void CalculateBalancePoint(Chesspiece[,] Chesspieces, float TILE_OFFSET)
	{
		float figureCounter = 0;
		balancePoint_x = 0;
		balancePoint_y = 0;
		for (int i = 0; i < 8; i++) 
		{
			for (int j = 0; j < 8; j++) 
			{
				Chesspiece c = Chesspieces [i, j];
				if (c != null) 
				{
					figureCounter += c.getWeight();
					balancePoint_x += i * c.getWeight();
					balancePoint_y += j * c.getWeight();
				}
			}
		}
		balancePoint_x /= figureCounter;
		balancePoint_y /= figureCounter;

		balancePoint_x += TILE_OFFSET;
		balancePoint_y += TILE_OFFSET;

		MoveBalancePoint (balancePoint_x, balancePoint_y);
	}

	public void CalculateBalancePoint(Chesspiece[,] Chesspieces, int selectionX, int selectionY, float weight, float TILE_OFFSET)
	{
		float figureCounter = 0;
		balancePoint_x = 0;
		balancePoint_y = 0;
		for (int i = 0; i < 8; i++) 
		{
			for (int j = 0; j < 8; j++) 
			{
				Chesspiece c = Chesspieces [i, j];
				if (c != null) 
				{
					if (i == selectionX && j == selectionY) {
						figureCounter += weight;
						balancePoint_x += i * weight;
						balancePoint_y += j * weight;
					} else {
						figureCounter += c.getWeight ();
						balancePoint_x += i * c.getWeight ();
						balancePoint_y += j * c.getWeight ();
					}
				}
			}
		}
		balancePoint_x /= figureCounter;
		balancePoint_y /= figureCounter;

		balancePoint_x += TILE_OFFSET;
		balancePoint_y += TILE_OFFSET;

		MoveBalancePoint (balancePoint_x, balancePoint_y);
	}

	public Vector3 MoveBalancePoint(float x, float y)
	{
		transform.position = new Vector3 (x, 0, y);
		return transform.position;
	}

	public void initBalancePointPosition()
	{
		MoveBalancePoint (balancePoint_x, balancePoint_y);
	}


}
