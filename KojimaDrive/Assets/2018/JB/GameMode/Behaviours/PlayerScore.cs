using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JB
{

public class PlayerScore : MonoBehaviour
{
    //player gameobjects

    [Header("Parameters")]
    int[] scores = new int[4];
    [SerializeField] private List<GameObject> scoreBoards;

    [Header("References")]
    [SerializeField] Text p1Display;
    [SerializeField] Text p2Display;
    [SerializeField] Text p3Display;
    [SerializeField] Text p4Display;

    private int playerID;


    public void ModifyPlayerScore(int _playerID, int _score)
    {
        scores[_playerID] += _score;

        UpdateBoards();
        UpdateScoreText();
    }


    void Update()
    {

    }

    void UpdateScoreText()
    {
        p1Display.text = scores[0].ToString();
        p2Display.text = scores[1].ToString();
        p3Display.text = scores[2].ToString();
        p4Display.text = scores[3].ToString();
    }


    void Start()
    {
        p1Display.color = Color.red;
        p2Display.color = Color.blue;
        p3Display.color = Color.green;
        p4Display.color = Color.magenta;

        UpdateBoards();
        UpdateScoreText();
    }

    void Awake()
    {
        scoreBoards = new List<GameObject>();
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject o in allObjects)
        {
            if (o.GetComponent<ElectronicBoard>() != null) scoreBoards.Add(o);
        }
    }

    private void UpdateBoards()
    {
        if (scoreBoards != null)
        {
            foreach (GameObject o in scoreBoards)
            {
                o.GetComponent<ElectronicBoard>().UpdateBoard(scores);
            }
        }
    }

}

} // namespace JB
