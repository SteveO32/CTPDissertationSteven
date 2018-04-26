//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Combinations : MonoBehaviour {
    
//    public static long Cfh(int f, int h)
//    {
//        return Pfh(f, h) / Factorial(h);
//    }

//    public static long Pfh(int f, int h)
//    {
//        return FactorialDivision(f, f - h);
//    }

//    private static long FactorialDivision(int topFactorial, int bottomFactorial)
//    {
//        long result = 1;
//        for (int i = topFactorial; i > bottomFactorial; i--)
//            result *= i;
//        return result;
//    }

//    private static long Factorial(int i)
//    {
//        if (i <= 1)
//            return 1;
//        return i * Factorial(i - 1);
//    }

//    public void testhelp()
//    {
//        Debug.Log(Combinations.Cfh(7, 5));
//    }

//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.P))
//        {
//            testhelp();
//        }
//    }
//}