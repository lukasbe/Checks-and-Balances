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

	private float initScale;
	public float magnifier{ set; get;}

	public Material[] materials;
	public Renderer rend;
    public GameObject weightPrefab;

    private void Awake()
    {
        weightPrefab = Resources.Load("Weight", typeof(GameObject)) as GameObject;
        Debug.Log(weightPrefab);
    }

    private void Start ()
	{
		rend = GetComponent<Renderer> ();
		rend.enabled = true;
		materials = rend.materials;
		rend.sharedMaterial = materials [0];
		hitBonus = 1;
		initScale = 0.005f;
		magnifier = 0;
		calculateHeight();
		renderWeights();
	}

	private void calculateHeight()
	{
		this.height = this.normalizedWeight * 0.1f;
		transform.position = new Vector3(transform.position.x, height, transform.position.z);
	}

	private void renderWeights()
	{
		// TODO
		// Debug.Log(this.weightPrefab);
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

	public void Magnify(float weight, float magnifier){
		this.magnifier = magnifier + 1;
		this.transform.localScale = new Vector3(initScale + this.magnifier * 0.001f, initScale + this.magnifier * 0.001f, initScale + this.magnifier * 0.001f);
	}

}
