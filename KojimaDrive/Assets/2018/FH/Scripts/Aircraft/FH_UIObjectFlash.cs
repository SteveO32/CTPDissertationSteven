using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




//========================= Kojima Party =========================//
//
// Author: ddaqes
// Purpose: Get the Graphic from a UI element in target objects 
//              and toggle its enabled button.
// Namespace: FH
//
//===============================================================================//


namespace FH
{
    public class FH_UIObjectFlash : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> m_targetObjects = new List<GameObject>();
        // TODO: change to onTime and offTime
        [SerializeField]
        private float m_pauseTime;

        private float m_currTime = 0f;


        private void Update()
        {
            if(m_currTime < m_pauseTime)
                m_currTime += Time.deltaTime;
            // Flash
            else
            {
                foreach(var obj in m_targetObjects)
                {
                    var col = obj.GetComponent<Graphic>().color;
                    col.a = (col.a == 1f ? 0f : 1f);
                    obj.GetComponent<Graphic>().color = col;
                }
                m_currTime = 0f;
            }
        }
    }
}
