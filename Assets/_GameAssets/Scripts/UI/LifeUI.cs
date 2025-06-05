using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUI : MonoBehaviour
{
    [SerializeField] private GameObject LifeEmpty;
    [SerializeField] private GameObject LifeFull;

    public void ChangeLife(bool hasBall) 
    {
        if (hasBall)
        {
            LifeEmpty.SetActive(false);
            LifeFull.SetActive(true);
        }
        else
        {
            LifeEmpty.SetActive(true);
            LifeFull.SetActive(false);
        }
    }


}
