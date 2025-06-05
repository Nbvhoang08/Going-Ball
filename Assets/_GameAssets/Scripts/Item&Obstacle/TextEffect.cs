using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
public class TextEffect : GenericPoolableObject
{
    public TextMeshProUGUI Content;
    Vector3 endPos;
    public override void PrepareToUse()
    {
        base.PrepareToUse();
        Content = GetComponent<TextMeshProUGUI>();
        if (Content == null)
        {
            Debug.LogError("TextMeshPro component not found on the object.");
            return;
        }
        Content.text = string.Empty;
    }
    public void OnShow(Vector3 startPos, string content)
    {
        Content.text = content;

        transform.localPosition = startPos;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        endPos = startPos;
        endPos.y += 300f;
        transform.DOLocalMove(endPos, 0.5f).OnComplete(() => {
            ReturnToPool();
        });
    }






}
