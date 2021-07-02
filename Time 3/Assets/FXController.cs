using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXController : MonoBehaviour
{
    public Transform goTo;
    public Transform VFX;
    public float speed = 1f;

    public void Move()
    {
        float step = speed * Time.deltaTime;
        VFX.position = Vector3.MoveTowards(VFX.position, goTo.position, step);
    }
}
