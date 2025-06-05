using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class KeyStoreTool
{
    static KeyStoreTool()
    {
        PlayerSettings.keystorePass = "hohama1234";
        PlayerSettings.keyaliasPass = "hohama1234";
    }
}
