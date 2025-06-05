using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace Hapiga.Core.Editor.Utils
{
    public static class BB_EditorUtils
    {
        private static BuildTargetGroup[] sWorkingBuildTargetGroups = null;

        /// <summary>
        /// Displays an editor dialog with the given title and message.
        /// </summary>
        /// <param name="title">the title.</param>
        /// <param name="message">the message.</param>
        public static void Alert(string title, string message)
        {
            EditorUtility.DisplayDialog(title, message, "OK");
        }

        /// <summary>
        /// Displays a modal dialog. ok and cancel are labels to be displayed on the dialog buttons. 
        /// If cancel is empty (the default), then only one button is displayed.
        /// </summary>
        /// <returns><c>true</c> if ok button is pressed., <c>false</c> otherwise.</returns>
        /// <param name="title">Title.</param>
        /// <param name="message">Message.</param>
        /// <param name="ok">Ok button label.</param>
        /// <param name="cancel">Cancel button label.</param>
        public static bool DisplayDialog(string title, string message, string ok, string cancel = "")
        {
            return EditorUtility.DisplayDialog(title, message, ok, cancel);
        }
        
        
        /// <summary>
        /// Returns the first class found with the specified class name and (optional) namespace and assembly name.
        /// Returns null if no class found.
        /// </summary>
        /// <returns>The class.</returns>
        /// <param name="className">Class name.</param>
        /// <param name="nameSpace">Optional namespace of the class to find.</param>
        /// <param name="assemblyName">Optional simple name of the assembly.</param>
        public static System.Type FindClass(string className, string nameSpace = null, string assemblyName = null)
        {
            string typeName = string.IsNullOrEmpty(nameSpace) ? className : nameSpace + "." + className;
            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly asm in assemblies)
            {
                // The assembly must match the given one if any.
                if (!string.IsNullOrEmpty(assemblyName) && !asm.GetName().Name.Equals(assemblyName))
                {
                    continue;
                }

                try
                {
                    System.Type t = asm.GetType(typeName);

                    if (t != null && t.IsClass)
                        return t;
                }
                catch (ReflectionTypeLoadException e)
                {
                    foreach (var le in e.LoaderExceptions)
                        Debug.LogException(le);
                }
            }

            return null;
        }

        /// <summary>
        /// Check if the given namespace exists within the current domain's assemblies.
        /// </summary>
        /// <returns><c>true</c>, if exists was namespaced, <c>false</c> otherwise.</returns>
        /// <param name="nameSpace">Name space.</param>
        /// <param name="assemblyName">Assembly name.</param>
        public static bool NamespaceExists(string nameSpace, string assemblyName = null)
        {
            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly asm in assemblies)
            {
                // The assembly must match the given one if any.
                if (!string.IsNullOrEmpty(assemblyName) && !asm.GetName().Name.Equals(assemblyName))
                {
                    continue;
                }

                try
                {
                    System.Type[] types = asm.GetTypes();
                    foreach (System.Type t in types)
                    {
                        // The namespace must match the given one if any. Note that the type may not have a namespace at all.
                        // Must be a class and of course class name must match the given one.
                        if (!string.IsNullOrEmpty(t.Namespace) && t.Namespace.Equals(nameSpace))
                        {
                            return true;
                        }
                    }
                }
                catch (ReflectionTypeLoadException e)
                {
                    foreach (var le in e.LoaderExceptions)
                        Debug.LogException(le);
                }
            }

            return false;
        }

        /// <summary>
        /// Makes legal identifier from string.
        /// Returns a legal C# identifier from the given string.  The transformations are:
        ///   - spaces => underscore _
        ///   - punctuation => underscore _
        ///   - leading numbers are prefixed with underscore.
        ///   - characters other than letters or digits are left out.
        /// </summary>
        /// <returns>the id</returns>
        /// <param name="key">Key to convert to an identifier.</param>
        public static string MakeIdentifier(string key)
        {
            string s;
            string retId = string.Empty;

            if (string.IsNullOrEmpty(key))
            {
                return "_";
            }

            s = key.Trim().Replace(' ', '_');   // Spaces => Underscore _
            s = s.Replace('.', '_');    // Punctuations => Underscore _

            // Construct the identifier, ignoring all special characters like , + - * : /
            foreach (char c in s)
            {
                if (char.IsLetterOrDigit(c) || c == '_')
                {
                    retId += c;
                }
            }

            // Prefix leading numbers with underscore.
            if (char.IsDigit(retId[0]))
            {
                retId = '_' + retId;
            }

            return retId;
        }

        
        /// <summary>
        /// Finds the duplicate fields between two elements in a serialized property that is an array.
        /// </summary>
        /// <returns>The duplicate field.</returns>
        /// <param name="property">Property.</param>
        /// <param name="fieldName">Name of the field to check.</param>
        public static string FindDuplicateFieldInArrayProperty(SerializedProperty property, string fieldName)
        {
            if (property.isArray)
            {
                HashSet<string> addedNames = new HashSet<string>();
                for (int i = 0; i < property.arraySize; i++)
                {
                    SerializedProperty el = property.GetArrayElementAtIndex(i);
                    string name = el.FindPropertyRelative(fieldName).stringValue;

                    if (!string.IsNullOrEmpty(name))
                    {
                        if (addedNames.Contains(name))
                        {
                            return name;
                        }
                        else
                        {
                            addedNames.Add(name);
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the number of scenes in Build Settings.
        /// </summary>
        /// <returns>The scene count in build settings.</returns>
        /// <param name="enabledScenesOnly">If set to <c>true</c> count enabled scenes only.</param>
        public static int GetSceneCountInBuildSettings(bool enabledScenesOnly = true)
        {
            int totalScenesInBuildSetting = EditorBuildSettings.scenes.Length;

            if (!enabledScenesOnly)
            {
                return totalScenesInBuildSetting;
            }
            else
            {
                int count = 0;
                for (int i = 0; i < totalScenesInBuildSetting; i++)
                {
                    count += EditorBuildSettings.scenes[i].enabled ? 1 : 0;
                }
                return count;
            }
        }

        /// <summary>
        /// Gets the paths of the scenes in Build Settings.
        /// </summary>
        /// <returns>The scene path in build settings.</returns>
        /// <param name="enabledScenesOnly">If set to <c>true</c> get enabled scenes only.</param>
        public static string[] GetScenePathInBuildSettings(bool enabledScenesOnly = true)
        {
            List<string> paths = new List<string>();
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                if (EditorBuildSettings.scenes[i].enabled || !enabledScenesOnly)
                {
                    paths.Add(EditorBuildSettings.scenes[i].path);
                }
            }
            return paths.ToArray();
        }

        /// <summary>
        /// Finds the game object with the specified name in scene. Note that this method
        /// only looks for root level objects.
        /// </summary>
        /// <returns>The first game object found in scene, or null if none was found.</returns>
        /// <param name="objectName">Object name.</param>
        /// <param name="scene">Scene.</param>
        public static GameObject FindGameObjectInScene(string objectName, Scene scene)
        {
            if (!scene.IsValid())
            {
                Debug.LogWarning("The given scene is invalid.");
                return null;
            }

            List<GameObject> roots = new List<GameObject>(scene.rootCount);
            scene.GetRootGameObjects(roots);
            GameObject go = roots.Find((GameObject g) =>
                {
                    return g.name.Equals(objectName);
                });

            return go;
        }

        /// <summary>
        /// Detemines if a root-level game object with the given name exists in the scenes specified by the scene paths.
        /// </summary>
        /// <returns><c>true</c> if a root-level game object is found; otherwise, <c>false</c>.</returns>
        /// <param name="objectName">Object name.</param>
        /// <param name="scenePaths">Scene paths.</param>
        public static bool IsGameObjectFoundInScenes(string objectName, string[] scenePaths)
        {
            bool isFound = false;
            Scene activeScene = EditorSceneManager.GetActiveScene();

            for (int i = 0; i < scenePaths.Length; i++)
            {
                if (activeScene.path.Equals(scenePaths[i]))
                {
                    if (FindGameObjectInScene(objectName, activeScene))
                        isFound = true;
                }
                else
                {
                    Scene scene = EditorSceneManager.OpenScene(scenePaths[i], OpenSceneMode.Additive);

                    if (FindGameObjectInScene(objectName, scene))
                        isFound = true;

                    EditorSceneManager.CloseScene(scene, true);
                }

                if (isFound)
                    break;
            }

            return isFound;
        }

        /// <summary>
        /// Finds the instance of the specified prefab in scene. Note that this methods only looks for
        /// root level instances.
        /// </summary>
        /// <returns>The first prefab instance found in scene, or null if none was found.</returns>
        /// <param name="prefab">Prefab.</param>
        /// <param name="scene">Scene.</param>
        public static GameObject FindPrefabInstanceInScene(GameObject prefab, Scene scene)
        {
            if (!scene.IsValid())
            {
                Debug.LogWarning("The given scene is invalid.");
                return null;
            }

#if UNITY_2018_3_OR_NEWER
            bool isNotPrefab = PrefabUtility.GetPrefabAssetType(prefab) == PrefabAssetType.NotAPrefab;
#else
            var prefabType = PrefabUtility.GetPrefabType(prefab);
            bool isNotPrefab = prefabType != PrefabType.Prefab && prefabType != PrefabType.ModelPrefab;
#endif
            if (isNotPrefab)
            {
                Debug.LogWarning("The provided object is not a valid prefab.");
                return null;
            }

            GameObject[] roots = scene.GetRootGameObjects();

            foreach (var obj in roots)
            {
#if UNITY_2018_2_OR_NEWER
                var parentPrefab = PrefabUtility.GetCorrespondingObjectFromSource(obj);
#else
                var parentPrefab = PrefabUtility.GetPrefabParent(obj);
#endif
                if (parentPrefab == prefab)
                {
#if UNITY_2018_3_OR_NEWER
                    return obj;
#else
                    var pType = PrefabUtility.GetPrefabType(obj);
                    if (pType == PrefabType.PrefabInstance || pType == PrefabType.ModelPrefabInstance)
                        return obj;
#endif
                }
            }

            return null;
        }

        /// <summary>
        /// Determines if a root-level instance of the given prefab found in the scenes specified by the scene paths.
        /// </summary>
        /// <returns><c>true</c> if a root-level prefab instance is found in the scenes; otherwise, <c>false</c>.</returns>
        /// <param name="prefab">Prefab.</param>
        /// <param name="scenePaths">Scene paths.</param>
        public static bool IsPrefabInstanceFoundInScenes(GameObject prefab, string[] scenePaths)
        {
            bool isFound = false;

#if UNITY_2018_3_OR_NEWER
            bool isNotPrefab = PrefabUtility.GetPrefabAssetType(prefab) == PrefabAssetType.NotAPrefab;
#else
            var prefabType = PrefabUtility.GetPrefabType(prefab);
            bool isNotPrefab = prefabType != PrefabType.Prefab && prefabType != PrefabType.ModelPrefab;
#endif
            if (isNotPrefab)
            {
                Debug.LogWarning("The provided object is not a valid prefab.");
                return isFound;
            }

            Scene activeScene = EditorSceneManager.GetActiveScene();

            for (int i = 0; i < scenePaths.Length; i++)
            {
                if (activeScene.path.Equals(scenePaths[i]))
                {
                    if (FindPrefabInstanceInScene(prefab, activeScene) != null)
                        isFound = true;
                }
                else
                {
                    Scene scene = EditorSceneManager.OpenScene(scenePaths[i], OpenSceneMode.Additive);

                    if (FindPrefabInstanceInScene(prefab, scene) != null)
                        isFound = true;

                    EditorSceneManager.CloseScene(scene, true);
                }

                if (isFound)
                    break;
            }

            return isFound;
        }

        /// <summary>
        /// Gets the field tooltip of the specified type. The field must not be static
        /// or private (because why would we want a tooltip otherwise?)
        /// </summary>
        /// <returns>The field tooltip.</returns>
        /// <param name="type">Type.</param>
        /// <param name="fieldName">Field name.</param>
        /// <param name="inherit">If set to <c>true</c> inherit.</param>
        public static string GetFieldTooltip(System.Type type, string fieldName, bool inherit = true)
        {
            string tooltip = "";
            var field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);

            if (field != null)
            {
                TooltipAttribute[] attributes = field.GetCustomAttributes(typeof(TooltipAttribute), inherit) as TooltipAttribute[];

                if (attributes.Length > 0)
                    tooltip = attributes[0].tooltip;
            }

            return tooltip;
        }

        /// <summary>
        /// Gets the key associated with the specified value in the given dictionary.
        /// </summary>
        /// <returns>The key for value.</returns>
        /// <param name="dict">Dict.</param>
        /// <param name="val">Value.</param>
        /// <typeparam name="TKey">The 1st type parameter.</typeparam>
        /// <typeparam name="TVal">The 2nd type parameter.</typeparam>
        public static TKey GetKeyForValue<TKey, TVal>(IDictionary<TKey, TVal> dict, TVal val)
        {
            foreach (KeyValuePair<TKey, TVal> entry in dict)
            {
                if (entry.Value.Equals(val))
                {
                    return entry.Key;
                }
            }

            return default(TKey);
        }

        /// <summary>
        /// Escapes the input string so it becomes URL-friendly,
        /// any instances of "+" will be replaced by "%20" (URL-friendly space).
        /// </summary>
        /// <returns>The escaped URL.</returns>
        /// <param name="s">Input string.</param>
        public static string EscapeURL(string s)
        {
#if UNITY_2018_3_OR_NEWER
            return UnityEngine.Networking.UnityWebRequest.EscapeURL(s).Replace("+", "%20");
#else
            return WWW.EscapeURL(s).Replace("+", "%20");
#endif
        }

        /// <summary>
        /// Gets all supported build target groups, excluding the <see cref="BuildTargetGroup.Unknown"/>
        /// and the obsolete ones.
        /// </summary>
        /// <returns>The working build target groups.</returns>
        public static BuildTargetGroup[] GetWorkingBuildTargetGroups()
        {
            if (sWorkingBuildTargetGroups != null)
                return sWorkingBuildTargetGroups;

            var groups = new List<BuildTargetGroup>();
            Type btgType = typeof(BuildTargetGroup);

            foreach (string name in System.Enum.GetNames(btgType))
            {
                // First check obsolete.
                var memberInfo = btgType.GetMember(name)[0];
                if (System.Attribute.IsDefined(memberInfo, typeof(System.ObsoleteAttribute)))
                    continue;

                // Name -> enum value and exclude the 'Unknown'.
                BuildTargetGroup g = (BuildTargetGroup)Enum.Parse(btgType, name);
                if (g != BuildTargetGroup.Unknown)
                    groups.Add(g);
            }

            sWorkingBuildTargetGroups = groups.ToArray();
            return sWorkingBuildTargetGroups;
        }

        /// <summary>
        /// Determines if a build target group is obsolete (has Obsolete attribute).
        /// </summary>
        /// <returns><c>true</c> if obsolete; otherwise, <c>false</c>.</returns>
        /// <param name="target">Target.</param>
        public static bool IsBuildTargetGroupObsolete(BuildTargetGroup target)
        {
            var type = target.GetType();
            var memberInfo = type.GetMember(target.ToString())[0];
            return System.Attribute.IsDefined(memberInfo, typeof(System.ObsoleteAttribute));
        }

        /// <summary>
        /// Returns the JDK path stored in Unity EditorPrefs or falling back to the JAVA_HOME
        /// environment variable if such path doesn't exist. Otherwise returns null.
        /// </summary>
        /// <returns>The jdk path.</returns>
        public static string GetJdkPath(bool verboseLog = false)
        {
            var jdkPath = UnityEditor.EditorPrefs.GetString("JdkPath");

            if (string.IsNullOrEmpty(jdkPath))
            {
                if (verboseLog)
                    Debug.Log(
                        "Unity 'Preferences > External Tools > Android JDK' path is not set. " +
                        "Falling back to JAVA_HOME environment variable.");
                jdkPath = System.Environment.GetEnvironmentVariable("JAVA_HOME");
            }

            return jdkPath;
        }

        /// <summary>
        /// Gets the type of the inspector window.
        /// </summary>
        /// <returns>The inspector window type.</returns>
        public static Type GetInspectorWindowType()
        {
            return Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll");
        }
    }
}
