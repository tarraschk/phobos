using UnityEngine;
using UnityEditor;

using System.Collections;

public class FixBuildSettings : MonoBehaviour
{

    [MenuItem("PUN Guide/Reset build settings")]
    static void FixBSet()
    {
        //
        //  SET SCENES
        //

        if (!EditorUtility.DisplayDialog("Resetting build settings", "Can the current build settings be overwritten with the scenes for the PUN guide?", "OK", "No, cancel"))
            return;

        EditorBuildSettingsScene[] sceneAr = new EditorBuildSettingsScene[18];
        int i = 0;
        sceneAr[i++] = new EditorBuildSettingsScene("Assets/PUNGuide_M2H/_MenuScene.unity", true);
        sceneAr[i++] = new EditorBuildSettingsScene("Assets/PUNGuide_M2H/_Tutorial 1/Tutorial_1A.unity", true);
        sceneAr[i++] = new EditorBuildSettingsScene("Assets/PUNGuide_M2H/_Tutorial 1/Tutorial_1B.unity", true);
        sceneAr[i++] = new EditorBuildSettingsScene("Assets/PUNGuide_M2H/_Tutorial 1/Tutorial_1C.unity", true);
        sceneAr[i++] = new EditorBuildSettingsScene("Assets/PUNGuide_M2H/_Tutorial 2/Tutorial_2A1.unity", true);
        sceneAr[i++] = new EditorBuildSettingsScene("Assets/PUNGuide_M2H/_Tutorial 2/Tutorial_2A2.unity", true);
        sceneAr[i++] = new EditorBuildSettingsScene("Assets/PUNGuide_M2H/_Tutorial 2/Tutorial_2A3.unity", true);
        sceneAr[i++] = new EditorBuildSettingsScene("Assets/PUNGuide_M2H/_Tutorial 2/Tutorial_2B.unity", true);
        sceneAr[i++] = new EditorBuildSettingsScene("Assets/PUNGuide_M2H/_Tutorial 3/Tutorial_3.unity", true);
        sceneAr[i++] = new EditorBuildSettingsScene("Assets/PUNGuide_M2H/_Tutorial 4/Tutorial_4.unity", true);
        sceneAr[i++] = new EditorBuildSettingsScene("Assets/PUNGuide_M2H/Example1/Example1_Chat.unity", true);
        sceneAr[i++] = new EditorBuildSettingsScene("Assets/PUNGuide_M2H/Example2/Example2_menu.unity", true);
        sceneAr[i++] = new EditorBuildSettingsScene("Assets/PUNGuide_M2H/Example2/Example2_game.unity", true);
        sceneAr[i++] = new EditorBuildSettingsScene("Assets/PUNGuide_M2H/Example3/Example3_lobbymenu.unity", true);
        sceneAr[i++] = new EditorBuildSettingsScene("Assets/PUNGuide_M2H/Example3/Example3_game.unity", true);
        sceneAr[i++] = new EditorBuildSettingsScene("Assets/PUNGuide_M2H/Example4/Example4_Menu.unity", true);
        sceneAr[i++] = new EditorBuildSettingsScene("Assets/PUNGuide_M2H/Example4/Example4_Game.unity", true);
        sceneAr[i++] = new EditorBuildSettingsScene("Assets/PUNGuide_M2H/Example5/Example5_Game.unity", true);

        EditorBuildSettings.scenes = sceneAr;
        Debug.Log("PUN Guide: reset project build settings.");


        /*
        
        //Output current build settings
        string bl = "";
        int i = 0;
        foreach (EditorBuildSettingsScene sc in EditorBuildSettings.scenes)
        {
            bl += "sceneAr[i++] = new EditorBuildSettingsScene(\"" + sc.path + "\", true);\n";

            i++;
        }
        Debug.Log(bl);
          
        */

    }
}