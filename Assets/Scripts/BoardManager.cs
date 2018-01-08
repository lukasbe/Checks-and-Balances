using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

	public BalancePoint balancePoint;

	private const float TILE_SIZE = 1.0f;
	private const float TILE_OFFSET = 0.5f;

	public static BoardManager Instance{ set; get; }

	public Chesspiece[,] Chesspieces{ set; get; }

	private Chesspiece selectedChesspiece;

	private bool[,] allowedMoves{ set; get; }

	public float currentRotAngle{ get; set;}

	public bool? whiteWon{ get; set;}

	private int selectionX = -1;
	private int selectionY = -1;

    private int numberOfMoves = 0;
    private int fastCamSwitchThreshold = 2;

	public GameObject ChessFieldPrefab;
	public List<GameObject> ChessPiecesPrefabs;

	public GameObject chessboardWatchHand;

	public GameObject chessboard;
	private Rigidbody rb;

    public Camera whiteGameCam;
    public Camera whiteActionCam;

    public Camera blackGameCam;
    public Camera blackActionCam;

    public Camera moveCam;

	public bool isWhiteTurn;
	private bool gameOverCalled;

	private void Awake ()
	{
		balancePoint = GameObject.Find ("BalancePoint").GetComponent<BalancePoint> ();
		BoardManager.Instance = this;
		chessboard = transform.GetChild (1).gameObject;
		rb = GetComponent<Rigidbody> ();
	}

	private void Start ()
	{
        whiteGameCam.enabled = true;
        whiteActionCam.enabled = false;
        blackGameCam.enabled = false;
        blackActionCam.enabled = false;
        moveCam.enabled = false;

		gameOverCalled = false;

		InstantiateChessPieces ();
		isWhiteTurn = true;
		whiteWon = null;
		balancePoint.initBalancePointPosition ();
	}

	private void Update(){
		UpdateSelection ();
		PiecesImageManager.Instance.SetTexture (selectionX, selectionY);
		if (Input.GetMouseButtonDown (0)) {
			if (selectionX >= 0 && selectionY >= 0) {
				if (selectedChesspiece == null)
					SelectChesspiece (selectionX, selectionY);
				else
					MoveChesspiece (selectionX, selectionY);
			}
		}
		if (moveCam.transform.localPosition.x > 13.0f || moveCam.transform.localPosition.x < -6.0f) {
			rb.isKinematic = false;
		}
		whiteWon = WhiteWon ();
		if (whiteWon != null) {
			GameOver ();
			WonTextManager.Instance.setWonText ();
		}
	}

	public void InstantiateChessPlanes ()
	{
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

	private void InstantiateChessPieces ()
	{

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
		SpawnChessPieces (4, 4, 7);

		//white king
		SpawnChessPieces (3, 3, 7);

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
		SpawnChessPieces (10, 4, 0);

		//black king
		SpawnChessPieces (9, 3, 0);

		//black pawns
		for (int i = 0; i < 8; i++)
			SpawnChessPieces (11, i, 1);
	}

	private void SpawnChessPieces (int index, int x, int y)
	{
		GameObject go = Instantiate (ChessPiecesPrefabs [index], GetTileCenter(x,y), Quaternion.Euler (-90.0f, 0.0f, 0.0f)) as GameObject;
		go.transform.SetParent (chessboard.transform);
		Chesspieces [x, y] = go.GetComponent<Chesspiece> ();
		Chesspieces [x, y].SetPosition (x, y);
	}

	public static Vector3 GetTileCenter (int x, int y)
	{
		Vector3 origin = Vector3.zero;
		origin.x += (TILE_SIZE * x) + TILE_OFFSET;
		origin.z += (TILE_SIZE * y) + TILE_OFFSET;
		return origin;
	}

	private void SelectChesspiece (int x, int y)
	{
		if (Chesspieces [x, y] == null)
			return;

		if (Chesspieces [x, y].isWhite != isWhiteTurn)
			return;

		bool hasAtLeastOneMove = false;
		allowedMoves = Chesspieces [x, y].PossibleMove ();
		for (int i = 0; i < 8; i++)
			for (int j = 0; j < 8; j++)
				if (allowedMoves [i, j])
					hasAtLeastOneMove = true;

		if (!hasAtLeastOneMove)
			return;

		selectedChesspiece = Chesspieces [x, y];
		selectedChesspiece.HighlightPiece ();
		MoveHighlights.Instance.HighlightAllowedMoves (allowedMoves);
	}

    private Camera activeCamera ()
    {
        if (isWhiteTurn == true)
            return whiteGameCam;
        else
            return blackGameCam;
    }

	private void UpdateSelection ()
	{
        //if (!Camera.main)
        //return;

        Camera activeCam = activeCamera();

		RaycastHit hit;
        if (Physics.Raycast (activeCam.ScreenPointToRay (Input.mousePosition), out hit, 25.0f, LayerMask.GetMask ("MoveHighlight"))) {
			//Debug.Log ("Mouse: (" + hit.point.x + ", " + hit.point.y + ", " + hit.point.z + ")");
			GameObject selectedmoveHighlight = hit.collider.gameObject;
			MoveHighlights.Instance.GetSelectionIndex (selectedmoveHighlight, out selectionX, out selectionY);
			//selectionX = (int)hit.point.x;
			//selectionY = (int)hit.point.z;
		//} else {
			//selectionX = -1;
			//selectionY = -1;
		}
	}

	private void MoveChesspiece (int x, int y)
	{
		if (allowedMoves [x, y]) {
			Chesspiece c = Chesspieces [x, y];
			if (c != null && c.isWhite != isWhiteTurn) {
				selectedChesspiece.AddWeight (c.GetWeight ());
				c.weights.Clear ();
				Destroy (c.gameObject);
			}

			Chesspieces [selectedChesspiece.CurrentX, selectedChesspiece.CurrentY] = null;
			selectedChesspiece.transform.position = selectedChesspiece.GetTileCenter (x, y);
			selectedChesspiece.SetPosition (x, y);
			Chesspieces [x, y] = selectedChesspiece;
			isWhiteTurn = !isWhiteTurn;
			//if (selectedChesspiece.GetType() == typeof(Pawn)) {
			//if (selectedChesspiece.isWhite && y == 0)
			//	ChangePawnToQueen (selectedChesspiece, x, y, true);
			//if (!selectedChesspiece.isWhite && y == 7)
			//	ChangePawnToQueen (selectedChesspiece, x, y, false);
			//}
			balancePoint.CalculateBalancePoint(Chesspieces, TILE_OFFSET);
			
            if (numberOfMoves < fastCamSwitchThreshold)
            {
                MoveGameCam();
            }
            else
            {
                ShowActionCam();
            }


			StartCoroutine ("MoveWatchHand");
			Debug.Log ("BalancePoint: " + (4 - balancePoint.y));
			Debug.Log ("Balance: " + (4 - rb.centerOfMass.z));
		}
		MoveHighlights.Instance.HideHighlights ();
		selectedChesspiece.UnhighlightPiece ();
		selectedChesspiece = null;
	}

	private void ShowBalanceFields ()
	{
		//BalanceHighlights.Instance.HighlightBalanceFields ();
	}

	public void ShowActionCam()
    {
        if(isWhiteTurn == true)
        {
            whiteGameCam.enabled = false;
            whiteActionCam.enabled = true;
            blackGameCam.enabled = false;
            blackActionCam.enabled = false;
            moveCam.enabled = false;

        }
        else
        {
            whiteGameCam.enabled = false;
            whiteActionCam.enabled = false;
            blackGameCam.enabled = false;
            blackActionCam.enabled = true;
            moveCam.enabled = false;
        }
	}

	public void ShowGameCam()
    {
        if (isWhiteTurn == true)
        {
            whiteGameCam.enabled = true;
            whiteActionCam.enabled = false;
            blackGameCam.enabled = false;
            blackActionCam.enabled = false;
            moveCam.enabled = false;
        }
        else
        {
            whiteGameCam.enabled = false;
            whiteActionCam.enabled = false;
            blackGameCam.enabled = true;
            blackActionCam.enabled = false;
            moveCam.enabled = false;
        }

        moveCam.GetComponent<MoveCamera>().resetPosition();
	}

    private void MoveGameCam()
    {
        whiteGameCam.enabled = false;
        whiteActionCam.enabled = false;
        blackGameCam.enabled = false;
        blackActionCam.enabled = false;
        moveCam.enabled = true;

        if(isWhiteTurn == true)
        {
            moveCam.GetComponent<MoveCamera>().moveToBlackTarget();
        }
        else
        {
            moveCam.GetComponent<MoveCamera>().moveToWhiteTarget();
        }
    }

	private IEnumerator MoveWatchHand(){

		float balance = rb.centerOfMass.z;
		setSpringPos (balance);

		rb.isKinematic = true;

		yield return new WaitForSeconds (2);

        if (numberOfMoves < fastCamSwitchThreshold)
        {
            yield return new WaitForSeconds(4);
        }
        else
        {
            yield return new WaitForSeconds(2);
        }

        ShowGameCam();
	}

	private float Map(float oldMin, float oldMax, float newMin, float newMax, float value){
		float oldRange = (oldMax - oldMin);
		float newRange = (newMax - newMin);
		float newValue = (((value - oldMin) * newRange) / oldRange) + newMin;

		return newValue ;
	}

	public void resetRotationToStart(){
		chessboardWatchHand.transform.rotation = Quaternion.Euler (-90.0f, 0.0f, 0.0f);
	}

	public void redoRotation(){
		float balance = rb.centerOfMass.z;
		//setSpringPos (balance);
	}

	private void setSpringPos(float angle){
		float springPos = Map (3.0f, 5.0f, -80.0f, 80.0f, angle);
		HingeJoint joint = GetComponent<HingeJoint> ();
		JointSpring spring = joint.spring;
		spring.targetPosition = springPos;
		//joint.spring = spring;
	}

	private bool? WhiteWon(){
		if (gameOverCalled)
			return null;
		if (rb.centerOfMass.z <= 2.9)
			return true;
		else if (rb.centerOfMass.z >= 5.1)
			return false;
		else
			return null;
	}

	private void GameOver(){
		gameOverCalled = true;
		rb.isKinematic = true;
		foreach (Chesspiece c in Chesspieces) {
			if (c != null) {
				c.transform.SetParent (null);
				Rigidbody crb = c.gameObject.AddComponent (typeof(Rigidbody)) as Rigidbody;
				BoxCollider bc = c.gameObject.AddComponent (typeof(BoxCollider)) as BoxCollider;
				foreach (GameObject g in c.weights) {
					if (g != null) {
						g.transform.SetParent (null);
						Rigidbody wrb = g.AddComponent (typeof(Rigidbody)) as Rigidbody;
						BoxCollider wbc = g.AddComponent (typeof(BoxCollider)) as BoxCollider;
					}
				}
			}
		}
	}

	/*
	private void TempMoveBalancePoint ()
	{
		if (selectionX >= 0 && selectionX < 8 && selectionY >= 0 && selectionY < 8) {
			//uncomment this if condition for debug purposes.
			if (allowedMoves [selectionX, selectionY]) {
				Chesspiece[,] tempCP = (Chesspiece[,])Chesspieces.Clone ();
				Chesspiece cp = tempCP [selectedChesspiece.CurrentX, selectedChesspiece.CurrentY];
				Chesspiece other = tempCP [selectionX, selectionY];
				tempCP [selectedChesspiece.CurrentX, selectedChesspiece.CurrentY] = null;
				tempCP [selectionX, selectionY] = cp;
				if (other != null && other.isWhite != isWhiteTurn) {
					balancePoint.CalculateBalancePoint (tempCP, selectionX, selectionY, cp.tempAddWeight (other.getWeight ()), TILE_OFFSET);
				} else {	
					balancePoint.CalculateBalancePoint (tempCP, TILE_OFFSET);
				}
			} else {
				balancePoint.CalculateBalancePoint (Chesspieces, TILE_OFFSET);
			}
		}
	}
	*/
}
