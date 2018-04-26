using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS
{
    public enum Avatar
    {
        CAR,
        PERSON,
        TANK,
        PLANE,
        FIREENGINE
    }

    public enum ThemeStyle
    {
        RED,
        GREEN,
        PINK,
        YELLOW
    }

    [System.Serializable]
    public class Theme
    {
        public ThemeStyle styleID;
        public GameObject car;
        public GameObject person;
        public GameObject tank;
        public GameObject plane;
        public GameObject fireEngine;
    }

    public class ThemeManager : MonoBehaviour
    {
        public List<Theme> themes;

        public Theme GetTheme(ThemeStyle style)
        {
            Debug.Log((int)style);
            return themes[(int)style];
        }
    }
}