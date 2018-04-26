using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HDev
{
    public class HDev_CarTerrainProperties : HDev_TerrainPropertyReader
    {
        //[SerializeField]private string m_debugCurSurfaceName;
        private FH.LandVehicle m_carScript;
        //private List<TerrainProperties.Properties_s> m_propertiesCache;

        private HDev_TerrainProperties.Properties_s m_currentTerrainProperties;

        FH.CarInfo m_stats;

        /// <summary>
        /// Get info about the terrain the car is driving over
        /// </summary>
        /// <returns></returns>
        public HDev_TerrainProperties.Properties_s GetCurrentTerrainProperties()
        {
            return m_currentTerrainProperties;
        }

        // Use this for initialization
        void Awake()
        {
            m_currentTerrainProperties = new HDev_TerrainProperties.Properties_s();
            m_stats = new FH.CarInfo();
            //m_propertiesCache = new List<TerrainProperties.Properties_s>();
            m_carScript = GetComponent<FH.LandVehicle>();
            Debug.Assert(m_carScript, "[Yams] CarTerrainProperties requires component CarScript");
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            RaycastHit hit;
            /*Any wheels on the ground?*/
            bool grounded = m_carScript.WheelGrounded(0) || m_carScript.WheelGrounded(1) || m_carScript.WheelGrounded(2) || m_carScript.WheelGrounded(3);
            /*This rather large if statement efficiently stops processing after a step fails*/
            if ((grounded
                && Physics.Raycast(m_carScript.transform.position + (m_carScript.transform.up * 0.8f), -m_carScript.transform.up, out hit, 10.0f, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore)
                && GetPropertyFromRay(hit, ref m_currentTerrainProperties)) == false)
            {
                m_currentTerrainProperties.m_friendlyName = string.Empty;
                m_currentTerrainProperties.m_modifiers.m_acceleration = 0;
                m_currentTerrainProperties.m_modifiers.m_extraGrip = 0;
                m_currentTerrainProperties.m_modifiers.m_maxSpeed = 0;
                m_currentTerrainProperties.m_modifiers.m_turnMaxSpeed = 0;
            }
            //m_debugCurSurfaceName = m_currentTerrainProperties.m_friendlyName;

            m_currentTerrainProperties.m_modifiers.ApplyPropertiesToCarInfo(ref m_stats);
            m_carScript.ApplyNewSurfaceStats(ref m_stats);
        }
    }
}