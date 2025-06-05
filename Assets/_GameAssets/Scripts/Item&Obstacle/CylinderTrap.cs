using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderTrap : MonoBehaviour
{

    [SerializeField] bool move;
    [SerializeField] Vector3 direction;
    [SerializeField, Range(-20, 20f)] float min, max;
    [SerializeField, Range(0f, 20f)] float speed;
    [SerializeField, Range(0, 5f)] float timeSleep;
    [SerializeField, Range(0f, 10f)] float timeDelay;
    [Space]
    [SerializeField] Transform model;
    [SerializeField] bool delaying;
    [SerializeField] bool sleeping;
    float timeSleeping;

    private void Update()
    {
        if (!move)
            return;
        if (delaying)
            return;
        if (sleeping)
        {
            timeSleeping += Time.deltaTime;
            if (timeSleeping >= timeSleep)
            {
                sleeping = false;
                timeSleeping -= timeSleeping;
            }
        }
        else
        {
            model.localPosition += direction * speed * Time.deltaTime;
            float face = Vector3.Dot(model.localPosition.normalized, direction);
            if (face >= 0 && model.localPosition.magnitude >= (direction * max).magnitude)
            {
                model.localPosition = Vector3.ClampMagnitude(model.localPosition, (direction * max).magnitude);
                direction *= -1f;
                sleeping = true;
            }
            else if (face >= 0 && model.localPosition.magnitude >= (direction * max).magnitude)
            {
                model.localPosition = Vector3.ClampMagnitude(model.localPosition, (direction * max).magnitude);
                direction *= -1f;
                sleeping = true;
            }
        }
    }

    IEnumerator StopDelay()
    {
        yield return new WaitForSeconds(timeDelay);
        delaying = false;
    }
    private void OnEnable()
    {
        if (timeDelay > 0)
        {
            delaying = true;
            StartCoroutine(StopDelay());
        }
    }
}
