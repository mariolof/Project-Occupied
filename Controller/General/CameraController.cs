using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {

    Vector3 lastPos;
    Vector3 currentPos;


    void Start ()
    {
        Camera.main.transform.position = new Vector3(WorldController.Instance.currentWorld.width/2, WorldController.Instance.currentWorld.length/2, -10);
    }
	
	void Update ()
    {
        currentPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);

        Zoom();
        MoveKeyBoard();
        MoveMouse();

        lastPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
    }


    void Zoom()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            MouseController.Instance.SelectRec.SetActive(false);
            return;
        }

        Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * Camera.main.orthographicSize;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 3f, 20f);
    }


    void MoveKeyBoard()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Camera.main.transform.Translate(Vector3.up * Camera.main.orthographicSize * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            Camera.main.transform.Translate(Vector3.down * Camera.main.orthographicSize * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Camera.main.transform.Translate(Vector3.left * Camera.main.orthographicSize * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Camera.main.transform.Translate(Vector3.right * Camera.main.orthographicSize * Time.deltaTime);
        }
    }


    void MoveMouse()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            MouseController.Instance.SelectRec.SetActive(false);
            return;
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 diff = lastPos - currentPos;
            Camera.main.transform.Translate(diff);
        }
    }

}
