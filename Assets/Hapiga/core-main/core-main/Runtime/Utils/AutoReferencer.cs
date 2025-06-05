using System.Linq;
using UnityEngine;

namespace Hapiga.Core.Runtime.Utils
{
    public class AutoReferencer<T> : MonoBehaviour where T : AutoReferencer<T>
    {
#if UNITY_EDITOR
        // This method is called once when we add component do game object
        protected virtual void Reset()
        {
            // Magic of reflection
            // For each field in your class/component we are looking only for those that are empty/null
            foreach (var field in typeof(T).GetFields().Where(field => field.GetValue(this) == null))
            {
                // Now we are looking for object (self or child) that have same name as a field
                Transform obj;
                if (transform.name == field.Name)
                {
                    obj = transform;
                }
                else
                {
                    //obj = transform.FindChild(field.Name);
                    obj = FindInChildRecursive(transform, field.Name);
                    // Or you need to implement recursion to looking into deeper childs
                }

                // If we find object that have same name as field we are trying to get component that will be in type of a field and assign it
                if (obj != null)
                {
                    field.SetValue(this, obj.GetComponent(field.FieldType));
                }
            }
        }

        private Transform FindInChildRecursive(Transform parent, string name)
        {
            Transform temp;

            temp = parent.Find(name);
            if (temp != null)
            {
                return temp;
            }

            if (parent.childCount == 0)
            {
                return null;
            }

            for (int i = 0; i < parent.childCount; i++)
            {
                temp = FindInChildRecursive(parent.GetChild(i), name);
                if (temp != null)
                {
                    return temp;
                }
            }

            return null;
        }
#endif
    }
}