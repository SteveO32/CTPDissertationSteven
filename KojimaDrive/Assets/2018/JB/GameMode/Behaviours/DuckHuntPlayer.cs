using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace JB
{

    public class DuckHuntPlayer : MonoBehaviour
    {
        public int playerID { get; private set; }
        private TurretMount weapon;
        public GameObject targetIndicator = null;

        private Rewired.Player input;
        private Color playerColor;


        public TurretMount Weapon
        {
            get { return weapon; }
            set { weapon = value; }
        }


        public void Init(int _playerID, Color _playerColor)
        {
            playerID = _playerID;
            playerColor = _playerColor;
            input = ReInput.players.GetPlayer(_playerID);
        }


        void Update()
        {
            if (weapon == null)
                return;

            if (input.GetButtonDown("TurretNext"))
            {
                weapon.MountNextTurret();
            }
            else if (input.GetButtonDown("TurretPrevious"))
            {
                weapon.MountPreviousTurret();
            }

            float fastAxis = input.GetAxis("FastAim");
            bool shooting = input.GetAxis("Fire") > 0;

            Vector2 inputDir = new Vector2(input.GetAxis("Horizontal"), input.GetAxis("Vertical"));
            weapon.MountedTurret.Aim(inputDir, fastAxis);

            if (shooting)
            {
                weapon.MountedTurret.Fire();
            }
            else
            {
                weapon.MountedTurret.ReleaseFire();
            }
        }

    }

} // namespace