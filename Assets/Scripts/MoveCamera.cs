using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    private Camera cam;
    public Canvas canvas;

    public Transform whiteOrigin;
    public Transform whiteTarget;

    public Transform blackOrigin;
    public Transform blackTarget;
 
    private bool move = false;
    private bool white = true;
    private float moveSpeed = 2.0f;
    private Vector3 velocity = Vector3.zero;

    private float rotSpeed = 0.7f;

    void Start()
    {
        cam = GetComponent<Camera>();
        //cam.aspect = 1.8f;
        resetPosition();
        setAspectRatio();
    }

    public void setAspectRatio()
    {
        //var variance = 2.3f / cam.aspect;

        //if (variance < 1.0f)
        //{
        //    cam.rect = new Rect((1.0f - variance) / 2.0f, 0.0f, variance, 1.0f);
        //}
        //else
        //{
        //    variance = 1.0f / variance;
        //    cam.rect = new Rect(0.0f, (1.0f - variance) / 2.0f, 1.0f, variance);
        //}
    }


    public void moveToWhiteTarget()
    {
        move = true;
        white = true;
        transform.position = whiteOrigin.position;
        transform.rotation = whiteOrigin.rotation;

		CameraMovement (whiteTarget);

        //canvas.transform.gameObject.SetActive(true);
    }

    public void moveToBlackTarget()
    {
        move = true;
        white = false;
        transform.position = blackOrigin.position;
        transform.rotation = blackOrigin.rotation;

		CameraMovement (blackTarget);

        //canvas.transform.gameObject.SetActive(true);
    }

	private void CameraMovement(Transform actionCamPos){
		LeanTween.move (cam.gameObject, actionCamPos.position, 1.5f).setEaseInOutQuint();
		LeanTween.rotate (cam.gameObject, actionCamPos.rotation.eulerAngles, 1.5f).setEaseInOutQuint ();
	}

    public void resetPosition()
    {
        //canvas.transform.gameObject.SetActive(false);
        move = false;
    }
}
