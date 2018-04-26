using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[ExecuteInEditMode]
public class DefaultMatCreator : MonoBehaviour {

    public bool create = false;

    public Material BaseMat;

    void Update()
    {
        if (create)
        {
            create = false;
            int counter = 0;
            for (int r = 0; r <= 255; r += 50)
            {
                for (int g = 0; g <= 255; g += 50)
                {
                    for (int b = 0; b <= 255; b += 50)
                    {
                        Material mat = new Material(BaseMat);
                        mat.color = new Color(r / 255.0f, g / 255.0f, b / 255.0f);

                        //AssetDatabase.CreateAsset(mat, "Assets/2018/LT/Materials/Defaults/" + ColorUtility.ToHtmlStringRGB(mat.color) + ".mat");
                    }
                }
            }

            Debug.Log(counter);
        }
    }

}
