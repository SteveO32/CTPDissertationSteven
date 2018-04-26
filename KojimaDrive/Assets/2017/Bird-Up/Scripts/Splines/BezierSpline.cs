using UnityEngine;
using System.Collections;
using System;

namespace Bird {

    public class BezierSpline : MonoBehaviour
    {
        [SerializeField] public Vector3[] points;
        [SerializeField] private BezierControlPointMode[] modes;
        [SerializeField] private bool loop;
        [SerializeField] public bool reverse;
        [SerializeField][Range(0, 1)] public float splineStart;


        public bool Loop {

            get {
                return loop;
            }
            set {
                loop = value;
                if (value == true) {
                    modes[modes.Length - 1] = modes[0];
                    SetControlPoint(0, points[0]);
                }
            }
        }

        public int ControlPointCount {
            get {
                return points.Length;
            }
        }

        public Vector3 GetControlPoint(int index) {
            return points[index];
        }

        public void SetControlPoint(int index, Vector3 point) {
            if (index % 3 == 0) {
                Vector3 delta = point - points[index];
                if (loop) {
                    if (index == 0) {
                        points[1] += delta;
                        points[points.Length - 2] += delta;
                        points[points.Length - 1] = point;
                    } else if (index == points.Length - 1) {
                        points[0] = point;
                        points[1] += delta;
                        points[index - 1] += delta;
                    } else {
                        points[index - 1] += delta;
                        points[index + 1] += delta;
                    }
                } else {
                    if (index > 0) {
                        points[index - 1] += delta;
                    }
                    if (index + 1 < points.Length) {
                        points[index + 1] += delta;
                    }
                }
            }
            points[index] = point;
            EnforceMode(index);
        }

        public BezierControlPointMode GetControlPointMode(int index) {
            return modes[(index + 1) / 3];
        }

        public void SetControlPointMode(int index, BezierControlPointMode mode) {
            int modeIndex = (index + 1) / 3;
            modes[modeIndex] = mode;
            if (loop) {
                if (modeIndex == 0) {
                    modes[modes.Length - 1] = mode;
                } else if (modeIndex == modes.Length - 1) {
                    modes[0] = mode;
                }
            }
            EnforceMode(index);
        }

        private void EnforceMode(int index) {
            int modeIndex = (index + 1) / 3;
            BezierControlPointMode mode = modes[modeIndex];
            if (mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Length - 1)) {
                return;
            }

            int middleIndex = modeIndex * 3;
            int fixedIndex, enforcedIndex;
            if (index <= middleIndex) {
                fixedIndex = middleIndex - 1;
                if (fixedIndex < 0) {
                    fixedIndex = points.Length - 2;
                }
                enforcedIndex = middleIndex + 1;
                if (enforcedIndex >= points.Length) {
                    enforcedIndex = 1;
                }
            } else {
                fixedIndex = middleIndex + 1;
                if (fixedIndex >= points.Length) {
                    fixedIndex = 1;
                }
                enforcedIndex = middleIndex - 1;
                if (enforcedIndex < 0) {
                    enforcedIndex = points.Length - 2;
                }
            }

            Vector3 middle = points[middleIndex];
            Vector3 enforcedTangent = middle - points[fixedIndex];
            if (mode == BezierControlPointMode.Aligned) {
                enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
            }
            points[enforcedIndex] = middle + enforcedTangent;
        }

        public int CurveCount {
            get {
                return (points.Length - 1) / 3;
            }
        }

        public Vector3 GetPoint(float _progress)
        {
            float actual = ProgressToActual(_progress);
            int i;
            if (actual >= 1f) {
                actual = 1f;
                i = points.Length - 4;
            } else {
                actual = Mathf.Clamp01(actual) * CurveCount;
                i = (int)actual;
                actual -= i;
                i *= 3;
            }
            return transform.TransformPoint(Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], actual));
        }

        public Vector3 GetVelocity(float _progress)
        {
            float actual = ProgressToActual(_progress);
            int i;
            if (actual >= 1f) {
                actual = 1f;
                i = points.Length - 4;
            } else {
                actual = Mathf.Clamp01(actual) * CurveCount;
                i = (int)actual;
                actual -= i;
                i *= 3;
            }

            Vector3 vel = transform.TransformPoint(Bezier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], actual)) - transform.position;
            if (reverse)
                return -vel;

            return vel;
        }

        public Vector3 GetDirection(float _progress)
        {
            float actual = ProgressToActual(_progress);
            return GetVelocity(actual).normalized;
        }

        public void AddCurve() {
            Vector3 point = points[points.Length - 1];
            Array.Resize(ref points, points.Length + 3);
            point.x += 1f;
            points[points.Length - 3] = point;
            point.x += 1f;
            points[points.Length - 2] = point;
            point.x += 1f;
            points[points.Length - 1] = point;

            Array.Resize(ref modes, modes.Length + 1);
            modes[modes.Length - 1] = modes[modes.Length - 2];
            EnforceMode(points.Length - 4);

            if (loop) {
                points[points.Length - 1] = points[0];
                modes[modes.Length - 1] = modes[0];
                EnforceMode(0);
            }
        }

        public void Reset() {
            points = new Vector3[] {
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
            new Vector3(4f, 0f, 0f)
        };
            modes = new BezierControlPointMode[] {
            BezierControlPointMode.Free,
            BezierControlPointMode.Free
        };
        }

        float ProgressToActual(float _progress)
        {
            float actual_progress = splineStart;

            if (reverse)
            {
                actual_progress = 1 - splineStart - _progress;

                if (actual_progress < 0)
                    actual_progress += 1;
            }
            else
            {
                actual_progress += _progress;

                if (actual_progress > 1)
                    actual_progress -= 1;
            }

            return actual_progress;
        }
    }

}