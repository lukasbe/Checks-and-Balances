using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	private BalancePoint balancePoint;

	private const float TILE_SIZE = 1.0f;
	private const float TILE_OFFSET = 0.5f;

	public static BoardManager Instance{ set; get;}

	public Chesspiece[,] Chesspieces{ set; get;}

	public GameObject ChessFieldPrefab;
	public List<GameObject> ChessPiecesPrefabs;
	private List<GameObject> ActiveChessPieces = new List<GameObject>();

	private void Awake(){
		balancePoint = GameObject.Find ("BalancePoint").GetComponent<BalancePoint> (); 
	}

	private void Start()
	{
		BoardManager.Instance = this;
		InstantiateChessFields ();
		InstantiateChessPieces ();
		//ShowBalanceFields ();
		balancePoint.initBalancePointPosition ();
	}


	public void InstantiateChessFields(){
		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				Vector3 position = Vector3.zero;
				position.x += (TILE_SIZE * i) + TILE_OFFSET;
				position.z += (TILE_SIZE * j) + TILE_OFFSET;
				position.y = -0.09f;
				GameObject go = Instantiate (ChessFieldPrefab, position, Quaternion.Euler (0.0f, 0.0f, 0.0f)) as GameObject;
				go.transform.SetParent (transform);
			}
		}

	}

	private void InstantiateChessPieces(){

		Chesspieces = new Chesspiece[8, 8];

		//white rooks
		SpawnChessPieces (0, 0, 7);
		SpawnChessPieces (0, 7, 7);

		//white knights
		SpawnChessPieces (1, 1, 7);
		SpawnChessPieces (1, 6, 7);

		//white bishops
		SpawnChessPieces (2, 2, 7);
		SpawnChessPieces (2, 5, 7);

		//white queen
		SpawnChessPieces (3, 4, 7);

		//white king
		SpawnChessPieces (4, 3, 7);

		//white pawns
		for (int i = 0; i < 8; i++)
			SpawnChessPieces (5, i, 6);

		//black rooks
		SpawnChessPieces (6, 0, 0);
		SpawnChessPieces (6, 7, 0);

		//black knights
		SpawnChessPieces (7, 1, 0);
		SpawnChessPieces (7, 6, 0);

		//black bishops
		SpawnChessPieces (8, 2, 0);
		SpawnChessPieces (8, 5, 0);

		//black queen
		SpawnChessPieces (9, 4, 0);

		//black king
		SpawnChessPieces (10, 3, 0);

		//black pawns
		for (int i = 0; i < 8; i++)
			SpawnChessPieces (11, i, 1);
	}

	private void SpawnChessPieces(int index, int x, int y)
	{
		GameObject go = Instantiate (ChessPiecesPrefabs [index], GetTileCenter(x,y), Quaternion.Euler(-90.0f, 0.0f, 0.0f)) as GameObject;
		go.transform.SetParent(transform);
		Chesspieces [x, y] = go.GetComponent<Chesspiece> ();
		Chesspieces [x, y].setPosition (x, y);
		ActiveChessPieces.Add (go);
	}

	private Vector3 GetTileCenter(int x, int y)
	{
		Vector3 origin = Vector3.zero;
		origin.x += (TILE_SIZE * x) + TILE_OFFSET;
		origin.z += (TILE_SIZE * y) + TILE_OFFSET;
		return origin;
	}


}
