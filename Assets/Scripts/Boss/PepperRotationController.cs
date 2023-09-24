using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PepperRotationController : MonoBehaviour
{
    private void SetRotation()
    {
        float playerSpace = GameManager.instance.GetPlayer().transform.position.x - transform.position.x;
        if (playerSpace <= 0)
        {
            transform.localScale= new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale= new Vector3(1, 1, 1);
        }
        transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        SetRotation();
    }
}
