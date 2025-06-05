using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hapiga.Core.Editor.Utils
{
    public static class DefineUtilsMenu
    {
        private const string df_APPLOVIN_MAX = "APPLOVIN_MAX";
        private const string df_FIREBASE_ANALYTIC = "FIREBASE_ANALYTIC";
        private const string df_FIREBASE_REMOTE = "FIREBASE_REMOTE";

        #region add define
        [MenuItem("Hapiga Package/Add Define/APPLOVIN_MAX", false, 0)]
        private static void Add_APPLOVIN_MAX()
        {
            GlobalDefineUtils.AddDefine(df_APPLOVIN_MAX);
        }

        [MenuItem("Hapiga Package/Add Define/FIREBASE_ANALYTIC", false, 0)]
        private static void Add_FIREBASE_ANALYTIC()
        {
            GlobalDefineUtils.AddDefine(df_FIREBASE_ANALYTIC);
        }
        [MenuItem("Hapiga Package/Add Define/FIREBASE_REMOTE", false, 0)]
        private static void Add_FIREBASE_REMOTE()
        {
            GlobalDefineUtils.AddDefine(df_FIREBASE_REMOTE);
        }
        #endregion






        #region remove define
        [MenuItem("Hapiga Package/Remove Define/APPLOVIN_MAX", false, 0)]
        private static void Remove_APPLOVIN_MAX()
        {
            GlobalDefineUtils.RemoveDefine(df_APPLOVIN_MAX);
        }
        [MenuItem("Hapiga Package/Remove Define/FIREBASE_ANALYTIC", false, 0)]
        private static void Remove_FIREBASE_ANALYTIC()
        {
            GlobalDefineUtils.RemoveDefine(df_FIREBASE_ANALYTIC);


        }
        [MenuItem("Hapiga Package/Remove Define/FIREBASE_REMOTE", false, 0)]
        private static void Remove_FIREBASE_REMOTE()
        {
            GlobalDefineUtils.RemoveDefine(df_FIREBASE_REMOTE);
        }
        #endregion

    }
}