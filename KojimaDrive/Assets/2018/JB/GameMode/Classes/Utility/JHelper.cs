using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JB
{

public class JHelper
{
    public static int CalculateIndex(int _x, int _y, int _size_x)
    {
        return (_y * _size_x) + _x;
    }


    public static bool ValidIndex(int _index, int _array_size)
    {
        return _index >= 0 && _index < _array_size;
    }


    public static Vector3 PointOnCircumreference(Vector3 _origin, float _radius, int _pointIndex, int _pointCount)
    {
        float theta = (2 * Mathf.PI / _pointCount * _pointIndex);
        float x = Mathf.Cos(theta) * _radius;
        float z = Mathf.Sin(theta) * _radius;

        return new Vector3(x, _origin.y, z);
    }


    public static List<Vector3> PointsOnCircumference(Vector3 _origin, float _radius, int _pointCount)
    {
        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < _pointCount; ++i)
        {
            Vector3 point = PointOnCircumreference(_origin, _radius, i, _pointCount);
            positions.Add(point);
        }

        return positions;
    }


    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return ((value - from1) / (to1 - from1) * (to2 - from2)) + from2;
    }


    public static Texture2D LoadPNG(string _file_path)
    {
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(_file_path))
        {
            fileData = File.ReadAllBytes(_file_path);
            tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);

            tex.LoadImage(fileData);
        }

        return tex;
    }


    public static void SetLayerRecursive(GameObject _obj, int _layer)
    {
        _obj.layer = _layer;

        foreach (Transform child in _obj.transform)
            SetLayerRecursive(child.gameObject, _layer);
    }


    public static Camera main_camera
    {
        get
        {
            if (main_camera_ == null || Camera.current != main_camera_)
                main_camera_ = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

            return main_camera_;
        }
    }


    private static Camera main_camera_;

}

} // namespace JB
