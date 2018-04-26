//========================= Kojima Drive - Bird-Up 2017 =========================//
//
// Author: Sam Morris (SpAMCAN)
// Purpose: Menus
// Namespace: Bird
//
//===============================================================================//

using UnityEngine;
using System.Collections;

namespace Bird 
{
	public class UI_FlashStartButton : MonoBehaviour 
{
		public GameObject m_target;
		public float m_fFlashTime = 1.0f;

		private Material m_MatInst;
        private float m_fStandardAlpha;
		private float m_fNextChangeTime;
        private int nColID;


		private void Start() 
        {
			m_fNextChangeTime = Time.realtimeSinceStartup + (m_fFlashTime);

			m_MatInst = m_target.GetComponent<Renderer>().material;
            if(!m_MatInst)
            {
                Debug.LogWarning("Warning: Material setup error");
                return;
            }

			nColID = Shader.PropertyToID("_GlobalMultiplierColor");
			m_fStandardAlpha = m_MatInst.GetColor(nColID).a;
		} 


		private void Update() 
        {
			if (m_fNextChangeTime <= Time.realtimeSinceStartup)
            {
				var col = m_MatInst.GetColor(nColID);
				col.a = (col.a == m_fStandardAlpha ? 0.0f : m_fStandardAlpha);

				m_MatInst.SetColor(nColID, col);
				m_fNextChangeTime = Time.realtimeSinceStartup + m_fFlashTime;
				m_MatInst.EnableKeyword("GLOBAL_MULTIPLIER_ON");
			}
		} 
	}
}