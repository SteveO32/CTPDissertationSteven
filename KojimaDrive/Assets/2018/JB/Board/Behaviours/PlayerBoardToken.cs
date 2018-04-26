using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JB
{

public class PlayerBoardToken : MonoBehaviour
{
    [HideInInspector] CustomEvents.IntEvent onTokenStopped = new CustomEvents.IntEvent();
    [HideInInspector] CustomEvents.IntEvent onTokenCrossedFinish = new CustomEvents.IntEvent();

    public SplinePoint targetPoint { get; private set; }

    [Header("Parameters")]
    [SerializeField] float moveSpeed = 0.1f;
    [SerializeField] float offsetRadius = 10.0f;

    [Header("References")]
    [SerializeField] Animator anim;
    [SerializeField] GameObject modelObj;

    private int playerID;
    private SplineFollower splineFollower;


    public void SetID(int _playerID, int _maxPlayers)
    {
        playerID = _playerID;

        GetComponent<LT.PlayerMaterialsApplier>().playerIndex = _playerID;
        GetComponent<LT.PlayerMaterialsApplier>().ApplyMaterial();

        Vector3 offset = JHelper.PointOnCircumreference(Vector3.zero, offsetRadius, _playerID, _maxPlayers);
        modelObj.transform.localPosition = offset;

        anim.SetFloat("Speed", 1);
        anim.speed = 1.5f;
    }


    public void AddSplineFollower(Bird.BezierSpline _spline)
    {
        if (splineFollower != null)
            return;

        splineFollower = this.gameObject.AddComponent<SplineFollower>();
        splineFollower.SetSpline(_spline);
    }


    public void SetStartPoint(SplinePoint _start)
    {
        splineFollower.SetProgress(_start.splineProgress);
        targetPoint = _start;

        UpdateFacingDir();
    }


    public void SetTargetPoint(SplinePoint _target)
    {
        targetPoint = _target;
    }


    public bool IsMoving()
    {
        if (targetPoint == null)
            return false;

        return splineFollower.progress != targetPoint.splineProgress;
    }


    public float GetProgress()
    {
        if (splineFollower == null)
            return 0;

        return splineFollower.progress;
    }


    void Start()
    {
    }


    void Update()
    {
        HandleMovement();
        UpdateFacingDir();
    }


    void HandleMovement()
    {
        if (!IsMoving())
            return;

        float prev_progress = splineFollower.progress;
        float adjustment = moveSpeed * Time.deltaTime;

        if (splineFollower.progress < targetPoint.splineProgress)
        {
            if (splineFollower.progress + adjustment >= targetPoint.splineProgress)
            {
                // Prevent overshooting.
                splineFollower.SetProgress(targetPoint.splineProgress);
            }
            else
            {
                splineFollower.AdjustProgress(adjustment);
            }
        }
        else // if (spline_follower.progress > target_point.spline_progress)
        {
            if (splineFollower.progress + adjustment > 1)
            {
                // Prevent overshooting.
                splineFollower.SetProgress(targetPoint.splineProgress);
            }
            else
            {
                splineFollower.AdjustProgress(adjustment);
            }
        }

        if (prev_progress > splineFollower.progress)
        {
            // We must have crossed the finish line as number wrapped around ..
            splineFollower.SetProgress(0);

            onTokenCrossedFinish.Invoke(playerID);
            Debug.Log("PlayerBoardToken crossed finish.");
        }
        else if (splineFollower.progress == targetPoint.splineProgress)
        {
            onTokenStopped.Invoke(playerID);
            Debug.Log("PlayerBoardToken finished moving.");
        }
    }


    void UpdateFacingDir()
    {
        transform.LookAt(transform.position + splineFollower.GetSplineVelocity());
    }

}

} // namespace JB
