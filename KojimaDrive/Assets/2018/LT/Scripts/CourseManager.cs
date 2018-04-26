using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; 

//===================== Kojima Party - Team Lunatics 2018 ====================//
//
// Author:		Charlie Saunders
// Purpose:		Golf Manager for the placing of holes and starting positions
// Namespace:	LT
//
//===============================================================================//

namespace LT
{
	[System.Serializable]
	public class Course
	{
		[SerializeField]
		public GameObject Start;
		[SerializeField]
		public GameObject Finish;
		[SerializeField]
		public GameObject Containment;
		[SerializeField]
		public CinemachineSmoothPath DollyTrack;
	}
    public class CourseManager : MonoBehaviour
    {
        [SerializeField]
        GameObject Pole;
        [SerializeField]
        GameObject Hole;

        public static int holeNum = 2;
		public GameObject dollyCam; 

		[SerializeField]
		public List<Course> Courses; 

//        [SerializeField]
//        List<Transform> Flags;

        [HideInInspector]
        public GameObject currentPole = null;
        [HideInInspector]
        public GameObject currentHole = null;
        [HideInInspector]
        public GameObject currentStart = null;

		float pathPos = 0.0f; 
		float speed = 0.002f;


        // Use this for initialization
        public void StartGame()
        {
            if (currentPole == null)
            {
                currentPole = Instantiate(Pole, Courses[holeNum].Finish.transform.position, Quaternion.identity);
            }
            if (currentHole == null)
            {
				currentHole = Instantiate(Hole,Courses[holeNum].Finish.transform.position, Quaternion.identity);
            }

            for (int i = 0; i < Courses.Count; i++)
            {
                Courses[i].Containment.active = i == holeNum;
            }

			currentPole.transform.parent = Courses[holeNum].Finish.transform;
			currentHole.transform.parent = Courses[holeNum].Finish.transform;

			currentStart = Courses[holeNum].Start;

			//Cinemachine camera 
			dollyCam.GetComponent<CinemachineVirtualCamera>().LookAt = currentPole.transform;
			dollyCam.transform.position = currentStart.transform.position; 
			dollyCam.GetComponent<CinemachineVirtualCamera> ().GetCinemachineComponent<CinemachineTrackedDolly> ().m_Path = Courses [holeNum].DollyTrack; 
			var heading = currentPole.transform.position - currentStart.transform.position; 
			Courses[holeNum].DollyTrack.GetComponent<CinemachineSmoothPath>().m_Waypoints[0].position = ((heading) * 0.5f) + currentStart.transform.position - heading;
			Courses[holeNum].DollyTrack.GetComponent<CinemachineSmoothPath>().m_Waypoints[Courses[holeNum].DollyTrack.GetComponent<CinemachineSmoothPath>().m_Waypoints.Length - 1].position = currentPole.transform.position;
			speed *= Courses [holeNum].DollyTrack.GetComponent<CinemachineSmoothPath> ().m_Waypoints.Length; 
        }
			
		void FixedUpdate()
		{
			pathPos += speed;  
			if (pathPos > Courses[holeNum].DollyTrack.GetComponent<CinemachineSmoothPath> ().m_Waypoints.Length - 2)
				pathPos += speed * 2;
			else
				dollyCam.SetActive (true); 
			
			if (pathPos < Courses[holeNum].DollyTrack.GetComponent<CinemachineSmoothPath> ().m_Waypoints.Length - 1.0f)
				dollyCam.GetComponent<CinemachineVirtualCamera> ().GetCinemachineComponent<CinemachineTrackedDolly> ().m_PathPosition = pathPos - 0.1f;
			else
				dollyCam.SetActive (false); 
		}
        //To be called when the playes finish the game
        public void EndGame()
        {
            holeNum++;
            //clear the holes

            //Load next hole OR go back to board scene
        }
    }
}
