using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PiecesImageManager : MonoBehaviour {

	public static PiecesImageManager Instance { get; set;}

	public Texture[] textures;
	public RawImage rawImage;


	void Start(){
		Instance = this;
		rawImage = GetComponent<RawImage> ();
	}

	public void SetTexture(int x, int y){
		int texIndex = 0;
		int colorOffset = 6;
		if (x >= 0 && x < 8 && y >= 0 && y < 8) {
			Chesspiece c = BoardManager.Instance.Chesspieces [x, y];
			if (c != null) {
				if (c.GetType () == typeof(Pawn)) {
					texIndex = 0 + (c.isWhite ? colorOffset : 0);
				} else if (c.GetType () == typeof(Rook)) {
					texIndex = 1 + (c.isWhite ? colorOffset : 0);
				} else if (c.GetType () == typeof(Knight)) {
					texIndex = 2 + (c.isWhite ? colorOffset : 0);
				} else if (c.GetType () == typeof(Bishop)) {
					texIndex = 3 + (c.isWhite ? colorOffset : 0);
				} else if (c.GetType () == typeof(Queen)) {
					texIndex = 4 + (c.isWhite ? colorOffset : 0);
				} else {
					texIndex = 5 + (c.isWhite ? colorOffset : 0);
				}
			} else {
				texIndex = 12;
			}
			rawImage.texture = textures [texIndex];
		}
	}

}
