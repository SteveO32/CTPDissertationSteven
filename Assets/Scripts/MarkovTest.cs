using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkovTest : MonoBehaviour {

    public Dictionary<string, int> SegmentDataStorage;

    private string TrackSegmentData = "ABAABBABAA";
    private char currentChar;
    private char nextChar;
    private string currentCharConvertedToString;
    private string nextCharConvertedToString;
    private string pairedString;
    private string addingToDictionary;
    

	// Use this for initialization
	void Start () {
        SegmentDataStorage = new Dictionary<string, int>();
    }
	
    void doStuff()
    {
        for (int i = 0; i < TrackSegmentData.Length - 1; i++)
        {
            currentChar = TrackSegmentData[i];
            nextChar = TrackSegmentData[i + 1];
            currentCharConvertedToString = currentChar.ToString();
            nextCharConvertedToString = nextChar.ToString();
            pairedString = currentCharConvertedToString + nextCharConvertedToString;

            if (SegmentDataStorage.ContainsKey(pairedString))
            {
                SegmentDataStorage[pairedString] += 1;
            }

            if (!SegmentDataStorage.ContainsKey(pairedString))
            {
                SegmentDataStorage.Add(pairedString, 1);
            }
        }

        foreach (KeyValuePair<string, int> kvp in SegmentDataStorage)
        {
            Debug.Log(kvp.Key);
            Debug.Log(kvp.Value);
        }

    }

	// Update is called once per frame
	void Update () {
		
        if(Input.GetKeyDown(KeyCode.Space))
        {
            doStuff();
        }
        
	}
}