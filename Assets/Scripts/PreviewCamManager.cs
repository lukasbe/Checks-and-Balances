using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewCamManager : MonoBehaviour {
    public bool isWhite;

	// Update is called once per frame
	void Update () {
        if (isWhite && !BoardManager.Instance.whiteCamPreview)
            GetComponent<Camera>().enabled = false;
        if (isWhite && BoardManager.Instance.whiteCamPreview)
            GetComponent<Camera>().enabled = true;
        if (!isWhite && BoardManager.Instance.whiteCamPreview)
            GetComponent<Camera>().enabled = false;
        if (!isWhite && !BoardManager.Instance.whiteCamPreview)
            GetComponent<Camera>().enabled = true;

	}
}
