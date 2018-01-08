using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class WonTextManager : MonoBehaviour {

	public static WonTextManager Instance{ get; set;}

	public Text wonText;

	public void Start(){
		Instance = this;
	}

	public void setWonText(){
		if (BoardManager.Instance.whiteWon == true)
			wonText.text = "White won!";
		else
			wonText.text = "Black won!";
	}

}
