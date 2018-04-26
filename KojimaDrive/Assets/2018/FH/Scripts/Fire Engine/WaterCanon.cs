using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace FH
{
    public class WaterCanon : MonoBehaviour
    {
        
        [SerializeField]
        private ParticleSystem m_waterHosePS;
        [SerializeField]
        private GameObject m_waterColliderPrefab;
        [SerializeField]
        private GameObject m_hose;
        [SerializeField]
        private ControllerID m_controllerID = ControllerID.Unassigned;


        private Rewired.Player m_rewiredPlayer;

        private void Start()
        {
            if(m_controllerID == ControllerID.Unassigned)
            {
                Debug.LogWarning("ERROR: Cannot initialise the rewired player with a controller ID - " + m_controllerID.ToString());
            }
            else
            {
                m_rewiredPlayer = Rewired.ReInput.players.GetPlayer((int)m_controllerID);
            }


           
        }


        private void Update()
        {
            var fire = m_rewiredPlayer.GetAxis("Turret Fire");
            if (fire > 0f)
            {
                if(!m_waterHosePS) return;
                m_waterHosePS.Play();

                var pSpeed = m_waterHosePS.main.startSpeed.constant;
                var pDir = m_hose.transform.forward;

                var waterCol = Instantiate(m_waterColliderPrefab, m_waterHosePS.transform.position, Quaternion.identity, this.transform);
                waterCol.GetComponent<Rigidbody>().velocity = (pSpeed * pDir * Time.deltaTime) * 100f; // TODO: Put this in fixed update 
            }
            else
            {
                if(!m_waterHosePS)
                    return;
                m_waterHosePS.Stop();
            }
        }
    }
}