using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXController : MonoBehaviour
{
    public Transform goTo;
    public Transform VFX;
    public float speed = 1f;
    public bool _moving = false;

    public void Move()
    {
        _moving = true;
    }
    private void Update() {
        if(_moving){
            float step = speed * Time.deltaTime;
            VFX.position = Vector3.MoveTowards(VFX.position, goTo.position, step);
        }

    }
}
