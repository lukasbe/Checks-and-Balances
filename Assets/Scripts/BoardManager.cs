using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

	private BalancePoint balancePoint;

	private const float TILE_SIZE = 1.0f;
	private const float TILE_OFFSET = 0.5f;

	public static BoardManager Instance{ set; get; }

	public Chesspiece[,] Chesspieces{ set; get; }

	private Chesspiece selectedChesspiece;

	private bool[,] allowedMoves{ set; get; }

	public float currentRotAngle{ get; set;}

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

		InstantiateChessPieces ();
		isWhiteTurn = true;
		balancePoint.initBalancePointPosition ();
	}

	private void Update(){
		UpdateSelection ();
		if (Input.GetMouseButtonDown (0)) {
			if (selectionX >= 0 && selectionY >= 0) {
				if (selectedChesspiece == null)
					SelectChesspiece (selectionX, selectionY);
				else
					MoveChesspiece (selectionX, selectionY);
			}
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
		}
		MoveHighlights.Instance.HideHighlights ();
		selectedChesspiece.UnhighlightPiece ();
		selectedChesspiece = null;
        numberOfMoves += 1;
	}

	private void ShowBalanceFields ()
	{
		//BalanceHighlights.Instance.HighlightBalanceFields ();
	}

	private void ShowActionCam()
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

	private void ShowGameCam()
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
            moveCam.GetComponent<MoveCamera>().moveToWhiteTarget();
        }
        else
        {
            moveCam.GetComponent<MoveCamera>().moveToBlackTarget();
        }
    }

	private IEnumerator MoveWatchHand(){

		float balance = rb.worldCenterOfMass.z;
		setSpringPos (balance);

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

        //if(balance < 3.0f || balance > 5.0f) {
        //    endGame();
        //}

        if (numberOfMoves > 4)
        {
            endGame();
        }

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
		float balance = rb.worldCenterOfMass.z;
		//setSpringPos (balance);
	}

	private void setSpringPos(float angle){
		float springPos = Map (3.0f, 5.0f, -80.0f, 80.0f, angle);
		HingeJoint joint = GetComponent<HingeJoint> ();
		JointSpring spring = joint.spring;
		spring.targetPosition = springPos;
        //joint.spring = spring;
	}

    private void endGame()
    {
        Debug.Log("Ending game");
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        Chesspiece piece;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                piece = Chesspieces[i, j];
                if(piece != null)
                {
                    Vector3 up = MoveHighlights.moveHighlights[i, j].transform.up;
                    piece.GetComponent<BoxCollider>().enabled = true;
                    foreach (Transform child in piece.transform)
                    {
                        child.GetComponent<BoxCollider>().enabled = true;
                        child.GetComponent<Rigidbody>().isKinematic = false;
                    }
                    piece.GetComponent<Rigidbody>().isKinematic = false;
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
