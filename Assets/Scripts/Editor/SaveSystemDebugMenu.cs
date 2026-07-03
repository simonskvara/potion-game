using System.IO;
using UnityEditor;
using UnityEngine;

public static class SaveSystemDebugMenu
{
    [MenuItem("Tools/Save System/Reveal Save Folder")]
    private static void RevealSaveFolder()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }

    [MenuItem("Tools/Save System/Delete Save")]
    private static void DeleteSave()
    {
        SaveManager.DeleteSave();
    }
}
