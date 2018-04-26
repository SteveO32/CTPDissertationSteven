using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace JB
{
    /*===================== Kojima Party - Team Juice Box 2018 ====================
     Author:	    Tom Turner
     Purpose:	    Controlable screen space gun reticule that calculates the world
                    space target for a gun turret.
     Namespace:	    JB
    ===============================================================================*/
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public class GunReticule : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 400;
        [SerializeField] float fastModifier = 2;
        [SerializeField] LayerMask rayLayerTargets;
        [Range(0, 1), SerializeField] float yScreenPercentLimit = 0.35f;// Percentage from bottom of screen not accessible
        [SerializeField] List<ReticuleAnimation> reticuleAnimations = new List<ReticuleAnimation>();

        private Image reticule = null;
        private Vector3 currentTarget = Vector3.zero;
        private float lastHitDist = 1000;


        [Serializable]
        public class ReticuleAnimation
        {
            public Image targetReticule = null;
            public Vector3 scaleTarget = Vector3.one;
            public float speed = 1;
        }


        public void InitPlayerReticule()
        {
            InitPlayerReticule(Color.white);
        }


        public void InitPlayerReticule(Color _playerColor)
        {
            if (reticule == null)
                reticule = GetComponent<Image>();

            SetColour(_playerColor);
        }
        

        public Vector2 AnchoredPosition
        {
            get { return reticule.rectTransform.anchoredPosition; }
            set { reticule.rectTransform.anchoredPosition = value; }
        }


        public void MoveReticule(Vector2 _moveDir, float _fastAxis = 0)
        {
            _moveDir *= moveSpeed * (1 + (_fastAxis * fastModifier)) * Time.deltaTime;
            reticule.rectTransform.anchoredPosition += _moveDir;

            ClampReticuleToScreen();
        }


        void ClampReticuleToScreen()
        {
            Rect screen = JHelper.main_camera.pixelRect;// Actual screen dimensions

            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, reticule.rectTransform.position);// Convert to screen space

            
            screenPos = new Vector2(// Limit in screen space
                Mathf.Clamp(screenPos.x, 0, screen.width),
                Mathf.Clamp(screenPos.y, 0 + (screen.height * yScreenPercentLimit), screen.height));// Add limit from bottom of screen
            
            Vector3 clampedWorldPos = Vector3.zero;// Convert back to canvas worldspace
            RectTransformUtility.ScreenPointToWorldPointInRectangle(reticule.rectTransform, screenPos, null, out clampedWorldPos);
            reticule.rectTransform.position = clampedWorldPos;
        }


        private void Start()
        {
            reticule = GetComponent<Image>();
        }


        private void Update()
        {
            UpdateReticuleAnimations();
        }


        private void UpdateReticuleAnimations()
        {
            foreach (ReticuleAnimation reticuleAnimation in reticuleAnimations)
            {
                reticuleAnimation.targetReticule.rectTransform.localScale = Vector3.Lerp(
                    reticuleAnimation.targetReticule.rectTransform.localScale, Vector3.one,
                    reticuleAnimation.speed * Time.deltaTime);// Lerp back to original scale
            }
        }


        public void SetColour(Color _reticuleColor)
        {
            if (reticule != null)
                reticule.color = _reticuleColor;

            foreach (ReticuleAnimation reticuleAnim in reticuleAnimations)
            {
                reticuleAnim.targetReticule.color = _reticuleColor;// Set colour for all animated reticules
            }
        }


        public void GrowReticule(float _scatterModifier = 1)
        {
            foreach (ReticuleAnimation reticuleAnimation in reticuleAnimations)
            {
                reticuleAnimation.targetReticule.rectTransform.localScale = reticuleAnimation.scaleTarget * _scatterModifier;// Scale up the reticule to target
            }
        }


        public Vector3 GetGunLookTarget()
        {
            Ray ray = CalculateRayFromUIElement(reticule.rectTransform);
            currentTarget = CalculateRayEndPosition(ray, lastHitDist);// Use last hit distance, prevents snapping on ray miss

            RaycastHit hit;
            if (!Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, rayLayerTargets))// If miss, use ray end
                return currentTarget;

            currentTarget = hit.point;// Override target if the retcule hits something
            lastHitDist = hit.distance;// Record distance of hit for future use

            return currentTarget;
        }


        Ray CalculateRayFromUIElement(RectTransform _uiElement)
        {
            Camera currentCamera = JHelper.main_camera;
            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, _uiElement.position);
            return currentCamera.ScreenPointToRay(screenPos);
        }


        Vector3 CalculateRayEndPosition(Ray _ray, float _distance)
        {
            return _ray.origin + (_ray.direction * _distance);
        }


        private void OnDrawGizmos()
        {
            if (reticule == null)// Should never be null really
                return;

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(currentTarget, 0.5f);
        }

    }
}// Namespace JB
