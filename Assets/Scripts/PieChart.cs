using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//class to handle creating a pie chart to represent data fed in
public class PieChart : MonoBehaviour {

    public float[] Data;
    public Color[] SegmentColours;
    public Image Segment;

	// Use this for initialization
	void Start ()
    {
        CreatePieChart();
	}
	
    public void CreatePieChart()
    {
        float total = 0f;
        float zRotation = 0f;

        for(int i = 0; i < Data.Length; i++)
        {
            total += Data[i];
        }


        for(int i = 0; i < Data.Length; i++)
        {
            //works by modifying the fill of the sprite and rotating it around a 
            //centre point to creat a pie chart
            //intended to work with circle sprite but for some reason
            //circle sprite not displaying properly, comes out as rectangle
            Image newSegment = Instantiate(Segment) as Image;
            newSegment.transform.SetParent(transform, false);
            newSegment.color = SegmentColours[i];
            newSegment.fillAmount = Data[i];
            newSegment.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));
            zRotation -= newSegment.fillAmount * 360f;
        }
    }
}
