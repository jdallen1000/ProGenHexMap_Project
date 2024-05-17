using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovment : MonoBehaviour
{
    float xDirection;
    float yDirection;
    float zoom = 10;
    public float speed = 1;
    public GameObject Camera;
    Camera thisCamera;

    // Start is called before the first frame update
    void Start()
    {
        thisCamera = Camera.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        xDirection = Input.GetAxisRaw("Horizontal");
        yDirection = Input.GetAxisRaw("Vertical");
        
        transform.position += new Vector3(xDirection, yDirection)* speed * Time.deltaTime;
        zoom += Input.mouseScrollDelta.y * -1;
        zoom = Mathf.Clamp(zoom, 0.1f, zoom + 1);
        thisCamera.orthographicSize = zoom;
        
        
    }
}
