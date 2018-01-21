using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraToggleManager : MonoBehaviour {

    private void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(call: ToggleCam);
    }

    // Update is called once per frame
    private void ToggleCam()
    {
        BoardManager.Instance.ChangeCameraOnUserRequest();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            ToggleCam();
    }

}
