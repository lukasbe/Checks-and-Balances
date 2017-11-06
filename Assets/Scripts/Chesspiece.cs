using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chesspiece : MonoBehaviour
{

	public int CurrentX{ set; get; }
	public int CurrentY{ set; get; }

	public bool isWhite;
	protected int weight;
	protected int normalizedWeight;
	protected float height;
	private int hitBonus;

	public Material[] materials;
	public Renderer rend;
    public GameObject weightPrefab;

    private void Start ()
	{
		rend = GetComponent<Renderer> ();
		rend.enabled = true;
		materials = rend.materials;
		rend.sharedMaterial = materials [0];
		hitBonus = 1;
		calculateHeight();
		renderWeights();
	}

	private void calculateHeight()
	{
		this.height = this.normalizedWeight * weightPrefab.transform.localScale.y;
		transform.position = new Vector3(transform.position.x, height, transform.position.z);
	}

	private void renderWeights()
	{
		for (int i = 0; i < this.normalizedWeight; i++) {
			Vector3 tileCenter = BoardManager.GetTileCenter (CurrentX, CurrentY);
			tileCenter.y = i * weightPrefab.transform.localScale.y;
			GameObject go = Instantiate (weightPrefab, tileCenter, Quaternion.Euler (0.0f, 0.0f, 0.0f)) as GameObject;
			go.transform.SetParent (transform);
		}
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

	public int getWeight ()
	{
		return this.weight;
	}

	public void setWeight (int weight)
	{
		this.weight = weight;
		// Store weight without hitbonus for calculation of height
		this.normalizedWeight = weight;
	}

	public float tempAddWeight(int weight){
		return this.weight + (weight * hitBonus);
	}

	public void addWeight (int weight)
	{	
		this.normalizedWeight += weight;
		this.weight += weight * hitBonus;
		hitBonus *= 2;
		calculateHeight();
	}

	public void HighlightPiece ()
	{
		rend.sharedMaterial = materials [1];
	}

	public void UnhighlightPiece ()
	{
		rend.sharedMaterial = materials [0];
	}

	/// <summary>
	/// Gets the tile center. this method differs from BoardManager.GetTileCenter() because it takes the weights' height into account.
	/// </summary>
	public Vector3 GetTileCenter(int x, int y){
		Vector3 tc = BoardManager.GetTileCenter (x, y);
		tc.y = height;
		return tc;
	}

}
