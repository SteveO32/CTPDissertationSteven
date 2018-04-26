using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace JB
{

    public class GameModeJB : KojimaParty.GameMode
    {
        [SerializeField] GameObject boatPrefab;
        [SerializeField] GameObject playerPrefab;
        [SerializeField] List<BoatCircuit> circuits;
        [SerializeField] GameObject weapon_prefab;
        [SerializeField] float weapon_forward_offset = 2;

        [Space]
        [SerializeField] ObjectSpawner objectSpawner;
        [SerializeField] RectTransform reticule_canvas;

        private BoatCircuit selectedCircuit;
        private DuckHuntBoat boat;


        public override void StartGameMode()
        {
            if (circuits == null || circuits.Count == 0)
            {
                Debug.LogWarning("GameModeJB: Error in initialisation. No circuits to select.");
                return;
            }

            SelectRandomCircuit();
            InitBoat();
            InitPlayers();
        }


        void Update()
        {

        }


        void SelectRandomCircuit()
        {
            selectedCircuit = circuits[Random.Range(0, circuits.Count)];
        }


        void InitBoat()
        {
            var clone = Instantiate(boatPrefab);
            boat = clone.GetComponent<DuckHuntBoat>();

            objectSpawner.SetTargetObject(clone);
            boat.SetStart(selectedCircuit.GetStartPoint());
        }



        void InitPlayers()
        {
            var spawns = boat.GetSpawnPoints();

            for (int i = 0; i < ReInput.controllers.joystickCount; ++i)
            {
                Transform spawn = spawns[i];

                var clone = Instantiate(playerPrefab, spawn.position, spawn.rotation);
                clone.transform.position = spawn.position;
                clone.transform.SetParent(spawn);

                var dhPlayer = clone.GetComponent<DuckHuntPlayer>();
                dhPlayer.Init(i, IDtoColor(i));

                Vector3 pos = dhPlayer.transform.position + (dhPlayer.transform.forward * weapon_forward_offset);
                clone = Instantiate(weapon_prefab, pos, dhPlayer.transform.rotation);

                clone.transform.SetParent(boat.transform);
                dhPlayer.Weapon = clone.GetComponent<TurretMount>();
                dhPlayer.Weapon.InitMount(reticule_canvas, IDtoColor(i));
                dhPlayer.Weapon.PlayerID = i;
            }
        }



        Color IDtoColor(int _id)
        {
            switch (_id)
            {
                case 0: return Color.red;
                case 1: return Color.blue;
                case 2: return Color.green;
                case 3: return Color.yellow;

                default: return Color.white;
            }
        }

    }

} // namespace JB