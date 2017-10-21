using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Chesspiece {

	public Rook(){
		this.weight = 5;
	}

	public override int getWeight ()
	{
		return weight;
	}

	public override bool[,] PossibleMove ()
	{
		bool[,] r = new bool[8, 8];

		Chesspiece c;
		int i;


		//right move
		i = CurrentX;
		while (true) 
		{
			i++;
			if (i >= 8)
				break;
			c = BoardManager.Instance.Chesspieces [i, CurrentY];
			if (c == null)
				r [i, CurrentY] = true;
			else 
			{
				if (c.isWhite != isWhite)
					r [i, CurrentY] = true;

				break;
			}
		}

		//left move
		i = CurrentX;
		while (true) 
		{
			i--;
			if (i < 0)
				break;
			c = BoardManager.Instance.Chesspieces [i, CurrentY];
			if (c == null)
				r [i, CurrentY] = true;
			else 
			{
				if (c.isWhite != isWhite)
					r [i, CurrentY] = true;

				break;
			}
		}

		//up move
		i = CurrentY;
		while (true) 
		{
			i++;
			if (i >= 8)
				break;
			c = BoardManager.Instance.Chesspieces [CurrentX, i];
			if (c == null)
				r [CurrentX, i] = true;
			else 
			{
				if (c.isWhite != isWhite)
					r [CurrentX, i] = true;

				break;
			}
		}

		//down move
		i = CurrentY;
		while (true) 
		{
			i--;
			if (i < 0)
				break;
			c = BoardManager.Instance.Chesspieces [CurrentX, i];
			if (c == null)
				r [CurrentX, i] = true;
			else 
			{
				if (c.isWhite != isWhite)
					r [CurrentX, i] = true;

				break;
			}
		}
		return r;
	}

}
