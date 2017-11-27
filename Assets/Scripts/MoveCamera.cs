using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    private Camera cam;
    public Transform origin;
    public Transform target;
    public Canvas canvas;

    private bool move = false;
    private float moveSpeed = 2.0f;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.aspect = 1.8f;
        resetPosition();
        setAspectRatio();
    }

    void Update()
    {
        if (move)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, moveSpeed * Time.deltaTime);
        }

        else
        {
            resetPosition();
        }
    }

    public void setAspectRatio()
    {
        var variance = 2.3f / cam.aspect;

        if (variance < 1.0f)
        {
            cam.rect = new Rect((1.0f - variance) / 2.0f, 0.0f, variance, 1.0f);
        }
        else
        {
            variance = 1.0f / variance;
            cam.rect = new Rect(0.0f, (1.0f - variance) / 2.0f, 1.0f, variance);
        }
    }

    public void resetAspectRatio()
    {

    }

    public void moveToTarget()
    {
        move = true;
        canvas.transform.gameObject.SetActive(true);
    }

    public void resetPosition()
    {
        canvas.transform.gameObject.SetActive(false);
        move = false;
        transform.position = origin.position;
        transform.rotation = origin.rotation;
    }
}