using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    public GameObject targetObject;
    public GameObject mainCanvas;
    public GameObject mainPanel;
    public Camera cam;

	// Use this for initialization
	void Start () {
        //Do Main Menu Stuff    
    }
	
	// Update is called once per frame
	void Update () {
		//Do Main Menu Stuff
        if(targetObject != null)
        {
            mainCanvas.transform.LookAt(targetObject.transform);

            mainCanvas.transform.RotateAround(targetObject.transform.position, Vector3.up, Time.deltaTime * 10);

           // mainCanvas.transform.Rotate(new Vector3(0.0f, 0.2f, 0.0f));
           // mainCanvas.GetComponent<RectTransform>().anchoredPosition = thing.transform.position;
           // Debug.Log(mainCanvas.transform.position.ToString());
            // Vector3 screenPos = cam.ScreenToWorldPoint(new Vector3(mainCanvas.GetComponent<RectTransform>().anchoredPosition.x, mainCanvas.GetComponent<RectTransform>().anchoredPosition.y));

            // mainCanvas.GetComponent.transform.position = screenPos;
            //mainCanvas.GetComponent<RectTransform>().anchoredPosition = screenPos;
        }
	}

    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }

    public void Credits()
    {
        SceneManager.LoadScene(4);
    }

    public void Exit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
