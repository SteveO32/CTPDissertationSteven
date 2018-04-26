using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JB
{

public class MissileManager : MonoBehaviour
{
    private List<DuckHuntPlayer> players;
    private float lastMissileTimeStamp;
    private bool lastMissileTargetHit;
    private DuckHuntPlayer lastMissileTarget;
    public DuckHuntPlayer target;
    public List<TurretJB> readyToShooTurretJbs;
    private float timeoffset = 5;
    private float targetingTime = 3.0f;
    public bool targeting;

    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        players = FindObjectsOfType<DuckHuntPlayer>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if (!(Time.time - lastMissileTimeStamp > timeoffset)) return;

        if (targeting) return;

        StartCoroutine(FindTarget());
        targeting = true;
    }

    IEnumerator FindTarget()
    {
        if (players.Count <= 0)
            yield break;

        var amountOfTargeting = Random.Range(10, 20);
        var targetCounter = 0;
        int index = Random.Range(0, players.Count);

        while (targetCounter < amountOfTargeting)
        {
            AudioManager.PlayOneShot("Sweep");
            target = players[index];
            target.targetIndicator.SetActive(true);
            yield return new WaitForSeconds(targetingTime / amountOfTargeting * Random.Range(0.8f, 1.2f));
            targetCounter++;
            index--;
            if (index == -1)
                index = players.Count - 1;
            target.targetIndicator.SetActive(false);
        }

        target.targetIndicator.SetActive(true);
        StartCoroutine(CheckReadyTurrets());
    }

    IEnumerator CheckReadyTurrets()
    {
        var randomTurretIndex = 0;
        TurretJB turret = null;
        bool scan = true;
        while (scan)
        {
            yield return new WaitForSeconds(1.0f);
            readyToShooTurretJbs.Clear();

            List<TurretJB> turrets = FindObjectsOfType<TurretJB>().ToList();
            foreach (var turretJb in turrets)
            {
                if (turretJb == null) continue;
                if (turretJb.ready)
                {
                    readyToShooTurretJbs.Add(turretJb);
                }
            }

            randomTurretIndex = Random.Range(0, readyToShooTurretJbs.Count);
            if (randomTurretIndex < readyToShooTurretJbs.Count)
            {
                turret = readyToShooTurretJbs[randomTurretIndex];
            }

            if (turret == null) continue;

            scan = !turret.ready;
        }

        Fire(turret);
    }

    private void Fire(TurretJB turret)
    {
        if (turret == null) return;

        AudioManager.PlayOneShot("RocketLaunch");
        turret.Shoot(target.transform);
        lastMissileTimeStamp = Time.time;
        targeting = false;
        StopCoroutine(CheckReadyTurrets());
    }
}

} // namespace JB
