using UnityEngine;
using System.Collections;

namespace JB
{

[ExecuteInEditMode]
public class SplineFollower : MonoBehaviour
{
    public float progress { get { return progress_; } private set { progress_ = value; } }

    [SerializeField] Bird.BezierSpline spline;
    [Range(0, 1)][SerializeField] float progress_;
    [SerializeField] bool velocity_rotation = false;
    [SerializeField] float rotate_speed = 5;

    private float prev_progress;


    public void SetSpline(Bird.BezierSpline _spline)
    {
        spline = _spline;
    }


    public void SetProgress(float _progress)
    {
        progress = _progress;
        prev_progress = Mathf.Infinity;

        WrapProgress();
    }


    public void AdjustProgress(float _amount)
    {
        progress += _amount;
        WrapProgress();
    }


    public Vector3 GetSplineVelocity()
    {
        return spline.GetVelocity(progress);
    }


    void Start()
    {

    }


    void Update()
    {
        if (spline == null)
            return;

        FollowSpline();

        if (velocity_rotation)
        {
            Vector3 slerp = Vector3.Slerp(transform.forward, spline.GetDirection(progress), rotate_speed * Time.deltaTime);
            transform.forward = slerp;
        }
    }


    void FollowSpline()
    {
        if (progress_ == prev_progress)
            return;

        Vector3 position = spline.GetPoint(progress_);
        transform.position = position;

        prev_progress = progress_;
    }


    void WrapProgress()
    {
        progress = 0 + (progress - 0) % (1 - 0);
    }

}

} // namespace JB
