using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SoundRadius : MonoBehaviour
{
    private Animator animator;
    private List<CreaturePatrol> creatureScripts;
    private PlayerLocomotion locScript;
    private TerrainDetector terrainDetector;

    private void Start()
    {
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

        terrainDetector = new TerrainDetector();
    }
    public void Step()
    {
        float radius = 0.0f;
        float value = 0.0f;
        int terrainTextureIndex = terrainDetector.GetActiveTerrainTextureIdx(transform.position);
        bool isStealth = locScript.isSteath;
        switch (terrainTextureIndex)
        {
            // grama
            case 0:
                radius = isStealth ? 5f : 10f;
                value = isStealth ? 5f : 10f;
                break;

            // pedra
            case 1:

            // agua
            case 2:

            // cascalho
            case 3:

            // tronco
            case 4:

            // terra
            case 5:

            // folhas
            case 6:

            default:
                radius = isStealth ? 5f : 10f;
                value = isStealth ? 5f : 10f;
                break;
        }
        foreach (var creature in creatureScripts)
        {
            if (creature == null) continue;
            var creatureTransform = creature.transform;
            var distance = Vector3.Distance(creatureTransform.position, transform.position);
            if (distance <= radius)
            {
                creature.Alert(value);
            }
        }
    }
}
