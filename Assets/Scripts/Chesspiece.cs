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
		CalculateHeight ();
		RenderWeights ();
	}

	private void CalculateHeight ()
	{
		Vector3 up = MoveHighlights.moveHighlights [CurrentX, CurrentY].transform.up;
		Vector3 tileCenter = GetTileCenter (CurrentX, CurrentY);

		float height = (weightPrefab.transform.localScale.z * 2) * this.weight;
		Vector3 target = tileCenter + (up * height);
		Vector3 endTarget = target + (up * weightPrefab.transform.localScale.z);
		//Vector3 end = Vector3.MoveTowards (target, endTarget, 1.0f);

		transform.position = target;
		Quaternion localRot = transform.localRotation;
		localRot.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
		transform.localRotation = localRot;

	}

	private void RenderWeights ()
	{

		// Remove all current Weight
		foreach (Transform child in transform)
		{
			GameObject.Destroy (child.gameObject);
		}

		//up vector
		Vector3 up = MoveHighlights.moveHighlights [CurrentX, CurrentY].transform.up;

		//start vectors
		Vector3 tileCenter = GetTileCenter (CurrentX, CurrentY);
		Vector3 startTarget = tileCenter + (up * weightPrefab.transform.localScale.z);
		Vector3 start = Vector3.MoveTowards (tileCenter, startTarget, 1.0f);

		float height = (weightPrefab.transform.localScale.z * 2) * this.weight;
		Vector3 target = tileCenter + (up * height);
		Vector3 endTarget = target + (up * weightPrefab.transform.localScale.z);
		Vector3 end = Vector3.MoveTowards (target, endTarget, 1.0f);

		float step = height / this.weight;

		for (int i = 0; i < this.weight; i++) {
			GameObject go = Instantiate (weightPrefab) as GameObject;
			go.transform.SetParent (transform);
			go.transform.position = Vector3.MoveTowards (start, end, i * step);
			Quaternion localRot = go.transform.localRotation;
			localRot.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
			go.transform.localRotation = localRot;
		}
	}

	public void SetPosition (int x, int y)
	{
		CurrentX = x;
		CurrentY = y;
		CalculateHeight ();
		RenderWeights ();
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
		CalculateHeight ();
		RenderWeights ();
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
	public Vector3 GetTileCenter (int x, int y)
	{
		Vector3 tc = MoveHighlights.Instance.GetTileCenter (x, y);
		return tc;
	}

}
