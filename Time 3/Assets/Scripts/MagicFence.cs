using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MagicFence : MonoBehaviour
{
    public CreaturePatrol creatureScript;
    private void Awake() {
        Assert.IsNotNull(creatureScript);
        StartCoroutine(ActivateSelf());
    }

    private IEnumerator ActivateSelf()
    {
        while(true)
        {
            yield return new WaitUntil(() => creatureScript.alertness == CreaturePatrol.AlertnessLevel.running);
            this.gameObject.SetActive(true);
            yield return new WaitUntil(() => creatureScript.alertness != CreaturePatrol.AlertnessLevel.running);
            this.gameObject.SetActive(false);
        }
    }

}
