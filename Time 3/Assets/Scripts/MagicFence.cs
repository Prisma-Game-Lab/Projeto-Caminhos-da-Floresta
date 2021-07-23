using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MagicFence : MonoBehaviour
{
    public CreaturePatrol creatureScript;
    public GameObject fence;
    private void Awake() {
        Assert.IsNotNull(creatureScript);
        StartCoroutine(ActivateSelf());
    }

    private IEnumerator ActivateSelf()
    {
        while(true)
        {
            yield return new WaitUntil(() => creatureScript.alertness == CreaturePatrol.AlertnessLevel.running);
            fence.SetActive(true);
            yield return new WaitUntil(() => creatureScript.alertness != CreaturePatrol.AlertnessLevel.running);
            fence.SetActive(false);
        }
    }

}
