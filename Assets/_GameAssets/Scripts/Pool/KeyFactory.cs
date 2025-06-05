using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyFactory : MonoBehaviour
{
    public void InitKeys()
    {
        Key[] Keys = GetComponentsInChildren<Key>();
        foreach (Key key in Keys)
        {
            key.ReturnToPool();
        }
        int totalKey = transform.childCount;
        for (int i = 0; i < totalKey; i++)
        {
            Transform keyGD = transform.GetChild(i);
            keyGD.gameObject.SetActive(false);

            Key key = Pool.Instance.Key as Key;
            key.transform.SetParent(transform);
            key.transform.localPosition = keyGD.localPosition;
        }
    }

    public void OnResetLevel()
    {
        InitKeys();
    }
}
