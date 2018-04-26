using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JB
{
    /*===================== Kojima Party - Team Juice Box 2018 ====================
     Author:	    Tom Turner
     Purpose:	    A turret mounting system that manages a number of installed 
                    turrets the player can interface with.
     Namespace:	    JB
    ===============================================================================*/
    public class TurretMount : MonoBehaviour
    {
        [SerializeField] List<Turret> startingTurrets = new List<Turret>();
        [SerializeField] Transform mountPoint = null;

        private List<Turret> installedTurrets = new List<Turret>();
        private Turret mountedTurret = null;
        private Color mountColour = Color.gray;
        private int? playerID = null;


        public int? PlayerID
        {
            set
            {
                playerID = value;
                installedTurrets.ForEach(turret => turret.PlayerID = playerID);
            }
            get { return playerID; }
        }


        public Turret MountedTurret
        {
            get { return mountedTurret; }
            set { mountedTurret = value; }
        }


        public void InitMount(RectTransform _rect)
        {    
            InstallAllTurrets(_rect);
            MountStartingTurret();
        }


        public void InitMount(RectTransform _rect, Color _mountColor)// Allow passing null to use turret default
        {
            mountColour = _mountColor;
            InitMount(_rect);
        }


        public void InstallAllTurrets(RectTransform _rect)
        {
            foreach (Turret turret in startingTurrets)
            {
               InstallTurret(turret, _rect);
            }
        }


        public void SetColour(Color _mountColour)
        {
            installedTurrets.ForEach(turret => turret.SetColour(_mountColour));
        }


        void InstallTurret(Turret _turretToInstall, RectTransform _rect)
        {
            Transform turretParent = mountPoint == null ? transform : mountPoint;// If a mount point isn't set use mounts transform
            Turret turretInstance = Instantiate(_turretToInstall, turretParent);// Create turret instance

            turretInstance.transform.position = turretParent.position;
            turretInstance.Install(_rect);// Let turret install self
            turretInstance.Mount(Vector2.zero);
            turretInstance.SetColour(mountColour);// Assign it mount colour
            turretInstance.UnMount();// No turret starts mounted

            installedTurrets.Add(turretInstance);// Store as an installed turret
        }


        public void MountStartingTurret()
        {
            if (installedTurrets.Count > 0)// If at least one turret in loadoat
            {
                mountedTurret = installedTurrets[0];

                if (mountedTurret != null)
                {
                    mountedTurret.Mount(Vector2.zero);// Mount and set reticule to centre screen
                }
            }
        }


        public void MountNextTurret()
        {
            if (installedTurrets.Count <= 0)
                return;

            int currentIndex = installedTurrets.IndexOf(mountedTurret);        
            ++currentIndex;
            MountTurret(currentIndex);
        }


        public void MountPreviousTurret()
        {
            if (installedTurrets.Count <= 0)
                return;

            int currentIndex = installedTurrets.IndexOf(mountedTurret);
            --currentIndex;
            MountTurret(currentIndex);
        }


        private void MountTurret(int _installedTurretIndex)
        {
            Vector3 lastReticulePos = mountedTurret.Reticule.AnchoredPosition;// Store reticule pos for next turret to use
            mountedTurret.UnMount();
            mountedTurret = installedTurrets[WrapIndex(_installedTurretIndex, startingTurrets.Count)];// Will wrap index if out of bounds
            mountedTurret.Mount(lastReticulePos);
        }


        public int WrapIndex(int _index, int _size)
        {
            return ((_index % _size) + _size) % _size;// Wraps index back around
        }

    }
}// JB namespace
