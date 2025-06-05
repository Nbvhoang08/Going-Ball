using DG.Tweening;
using Hapiga.Core.Runtime.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiReward : MonoBehaviour
{
    [SerializeField] RectTransform arrow;
    [SerializeField] float maxX = 400f;
    Coroutine animationArrow;
    //Tween arrowLeft, ArrowRight;


    Tween arrowTween;
    public void OnStart()
    {
        
        arrow.anchoredPosition = new Vector2(-405f, arrow.anchoredPosition.y);
        arrowTween?.Kill(false); 
        arrowTween = arrow.DOAnchorPosX(maxX, 0.6f)
                          .SetEase(Ease.InOutQuad)
                          .SetLoops(-1, LoopType.Yoyo); 
    }
    
    private void StopAnimation()
    {
        arrowTween?.Kill(false); // Kill tween nếu tồn tại
       
     
    }
    public float caculationMulti(bool Finish, bool ads = true)
    {
        if (Finish)
            StopAnimation();

        if (ads)
        {
            float x = arrow.anchoredPosition.x;
            float distance = (maxX * 2f) / 5f;
           
            if (x <= -maxX + distance * 1f || x >= -maxX + distance * 4f)
            {
                return 2f;
            }
            else if (x > -maxX + distance * 2f && x < -maxX + distance * 3f)
            {
                return 5f;
            }
            else
            {
                return 3f;
            }
        }
        else
            return 1f;
    }   
}
