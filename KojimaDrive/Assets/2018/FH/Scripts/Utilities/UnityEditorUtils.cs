using System.IO;
using System.Collections;
using System.Collections.Generic;


//===================== Kojima Party - FluffyHedgehog 2018 ====================//
//
// Author:		Dudley 
// Purpose:		Some general Utilities. Mostly to add features to the Unity Editor but other handy scripts as well. 
//                  Feel free to add some more :D
// Namespace:	FH
//
//===============================================================================//


namespace FH
{
    public class UnityEditorUtils : UnityEditor.ScriptableWizard
    {

        /// <summary>
        /// Return a the information regarding all files at the path passed in.
        /// </summary>
        /// <param name="_path"></param>
        /// <returns></returns>
        public static FileInfo[] FilesInDirectory(string _path)
        {
            var path = new DirectoryInfo(_path);
            var fileInfo = path.GetFiles("*.*", SearchOption.AllDirectories);

            return fileInfo;
        }

        /// <summary>
        /// Add the clear console commad to the Tools bar.
        /// </summary>
        [UnityEditor.MenuItem("Tools/Clear Console %c")]
        static void ClearConsoleWizard()
        {
            var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries, UnityEditor.dll");
            var clearMethod = logEntries.GetMethod("Clear",
                System.Reflection.BindingFlags.Static
                | System.Reflection.BindingFlags.Public);
            clearMethod.Invoke(null, null);

        }
    }
}