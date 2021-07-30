using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

[RequireComponent(typeof(Collider))]
public class SceneChange : MonoBehaviour
{
    public static SceneChange instance;
    public string sceneName;
    private OrbController[] allOrbs;
    private void Start() {
        Assert.IsNull(instance);
        Assert.IsTrue(GetComponent<Collider>().isTrigger);
        Assert.IsFalse(sceneName == "");
        instance = this;
        allOrbs = FindObjectsOfType<OrbController>();
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player"))
        {
            foreach (var orb in allOrbs)
            {
                if(orb.orb.activeSelf)
                {
                    continue;
                }
                return;
            }
            SceneManager.LoadScene(sceneName);
        }
    }
}
