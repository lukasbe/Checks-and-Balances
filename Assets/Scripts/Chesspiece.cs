using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chesspiece : MonoBehaviour
{

	public int CurrentX{ set; get; }

	public int CurrentY{ set; get; }

	public bool isWhite;
	public int weight;

	public Material[] materials;
	public Renderer rend;

	private void Start ()
	{
		rend = GetComponent<Renderer> ();
		rend.enabled = true;
		materials = rend.materials;
		rend.sharedMaterial = materials [0];
	}

	public void setPosition (int x, int y)
	{
		CurrentX = x;
		CurrentY = y;
	}

	public virtual bool[,] PossibleMove ()
	{
		return new bool[8, 8];
	}

	public virtual int getWeight ()
	{
		return -1;
	}

	public void addWeight (int weight)
	{
		this.weight += weight;
	}

	public void HighlightPiece ()
	{
		rend.sharedMaterial = materials [1];
	}

	public void UnhighlightPiece ()
	{
		rend.sharedMaterial = materials [0];
	}

}
