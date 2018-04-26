using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfBallIDS : MonoBehaviour {

    private int ball_id;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetID(int id)
    {
        ball_id = id;
    }

    public int GetID()
    {
        return ball_id;
    }
}
