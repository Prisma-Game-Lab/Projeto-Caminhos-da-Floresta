using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SoundRadius : MonoBehaviour
{
    public float maxRadius;
    private Animator animator;
    private List<CreaturePatrol> creatureScripts;
    private PlayerLocomotion locScript;

    private void Start() {
        creatureScripts = new List<CreaturePatrol>();
        animator = GetComponentInChildren<Animator>();
        var allCreatures = GameObject.FindGameObjectsWithTag("Creature");
        foreach (var creature in allCreatures)
        {
            var creatureScript = creature.GetComponent<CreaturePatrol>();
            Assert.IsNotNull(creatureScript, "Criatura esta sem script 'Creature Patrol'");
            creatureScripts.Add(creatureScript);
        }
        locScript = GetComponent<PlayerLocomotion>();
        Assert.IsNotNull(locScript, "Player sem script 'Player Locomotion'");
    }
    public void Step(){
        float radius = maxRadius;
        float value = 5.0f;
        if(locScript.isSteath){
            radius = 0f;
            value = 0f;
        }

        foreach (var creature in creatureScripts)
        {
            var creatureTransform = creature.transform;
            var distance = Vector3.Distance(creatureTransform.position,transform.position);
            if(distance <= radius){
                creature.Alert(value);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxRadius);
    }
}
