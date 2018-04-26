using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace LT
{
    public class KojimaPartyTab
    {
        [MenuItem("Kojima Party/Info")]
        static void DebugInfo()
        {
            Debug.Log("Hello, the editor files go in '/2018/Editor' with your namespace, click this Log to see an example. " +
                "The Menu items should be divided in Kojima Party and Kojima Drive. Thanks");
        }
    }
}
