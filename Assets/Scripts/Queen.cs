using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Chesspiece {

	public Queen(){
		this.setWeight(5);
	}

	public override bool[,] PossibleMove ()
	{
		bool[,] r = new bool[8, 8];

		Chesspiece c;
		int i,j;

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

		//top left
		i = CurrentX;
		j = CurrentY;
		while (true) 
		{
			i--;
			j++;
			if (i < 0 || j >= 8)
				break;

			c = BoardManager.Instance.Chesspieces [i, j];
			if (c == null)
				r [i, j] = true;
			else 
			{
				if (isWhite != c.isWhite)
					r [i, j] = true;

				break;
			}

		}

		//top right
		i = CurrentX;
		j = CurrentY;
		while (true) 
		{
			i++;
			j++;
			if (i >= 8 || j >= 8)
				break;

			c = BoardManager.Instance.Chesspieces [i, j];
			if (c == null)
				r [i, j] = true;
			else 
			{
				if (isWhite != c.isWhite)
					r [i, j] = true;

				break;
			}

		}

		//down left
		i = CurrentX;
		j = CurrentY;
		while (true) 
		{
			i--;
			j--;
			if (i < 0 || j < 0)
				break;

			c = BoardManager.Instance.Chesspieces [i, j];
			if (c == null)
				r [i, j] = true;
			else 
			{
				if (isWhite != c.isWhite)
					r [i, j] = true;

				break;
			}

		}

		//down right
		i = CurrentX;
		j = CurrentY;
		while (true) 
		{
			i++;
			j--;
			if (i >= 8 || j < 0)
				break;

			c = BoardManager.Instance.Chesspieces [i, j];
			if (c == null)
				r [i, j] = true;
			else 
			{
				if (isWhite != c.isWhite)
					r [i, j] = true;

				break;
			}

		}

		return r;
	}
}
