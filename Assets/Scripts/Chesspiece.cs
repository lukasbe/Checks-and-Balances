using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chesspiece : MonoBehaviour
{

	public int CurrentX{ set; get; }
	public int CurrentY{ set; get; }

	public bool isWhite;
	protected int weight;
	protected float height;

	public Material[] materials;
	public Renderer rend;
    public GameObject weightPrefab;

    private void Start ()
	{
		rend = GetComponent<Renderer> ();
		rend.enabled = true;
		materials = rend.materials;
		rend.sharedMaterial = materials [0];
		CalculateHeight();
		RenderWeights();
	}

	private void CalculateHeight()
	{
//		this.height = this.normalizedWeight * weightPrefab.transform.localScale.y;
        // Limit weight rendering to 10
        // Mathf because Math does not exist in unity

        Vector3 tileCenter = MoveHighlights.Instance.GetTileCenter(CurrentX, CurrentY);
        tileCenter.y += this.weight * (weightPrefab.transform.localScale.z * 2);
        transform.position = tileCenter;
	}

	private void RenderWeights()
	{
		
        // Remove all current Weight
		foreach (Transform child in transform)
        {
			GameObject.Destroy (child.gameObject);
        }

        // Limit weight rendering to 10
        // Mathf because Math does not exist in unity


		for (int i = 0; i < this.weight; i++) {
			Vector3 tileCenter = MoveHighlights.Instance.GetTileCenter (CurrentX, CurrentY);
			tileCenter.y += weightPrefab.transform.localScale.z;
			tileCenter.y += i * (weightPrefab.transform.localScale.z * 2);
			GameObject go = Instantiate (weightPrefab) as GameObject;
			go.transform.position = tileCenter;
			go.transform.up = MoveHighlights.moveHighlights [CurrentX, CurrentY].transform.up;
			go.transform.Rotate(new Vector3(90.0f, 0.0f, 0.0f));
			go.transform.SetParent (transform);
		}
	}

	public void SetPosition (int x, int y)
	{
		CurrentX = x;
		CurrentY = y;
        CalculateHeight();
        RenderWeights();
	}

	public virtual bool[,] PossibleMove ()
	{
		return new bool[8, 8];
	}

	public int GetWeight ()
	{
		return this.weight;
	}

	public void SetWeight (int weight)
	{
		this.weight = weight;
	}

	public void AddWeight (int weight)
	{	
		this.weight += weight;
		CalculateHeight();
        RenderWeights();
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
		Vector3 tc = MoveHighlights.Instance.GetTileCenter (x, y);
		return tc;
	}

}
