using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace JB
{

public class ElectronicBoard : MonoBehaviour
{

    [Header("References")]
    [SerializeField]
    Text p1Display;
    [SerializeField]
    Text p2Display;
    [SerializeField]
    Text p3Display;
    [SerializeField]
    Text p4Display;

    // Use this for initialization
    void Awake ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void Start()
    {
        p1Display.color = Color.red;
        p2Display.color = Color.blue;
        p3Display.color = Color.green;
        p4Display.color = Color.magenta;
    }

    public void UpdateBoard(int[] _scores)
    {
        p1Display.text = _scores[0].ToString();
        p2Display.text = _scores[1].ToString();
        p3Display.text = _scores[2].ToString();
        p4Display.text = _scores[3].ToString();
    }
}

} // namespace JB
