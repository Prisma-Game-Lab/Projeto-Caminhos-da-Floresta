using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectCollider : MonoBehaviour
{
    private GameObject _Other;

    public List<string> objectList = new List<string>();

    static private int count = 0;

    private bool _isTrigger = false;

    InputManager inputManager;
    PlayerLocomotion playerLocomotion;

    FMOD.Studio.EventInstance pickItem;
    FMOD.Studio.EventInstance lightOrb;
    public GameObject vfx;

    void Awake()
    {
        vfx.SetActive(false);
        /* Recuperando os itens quando se troca de cena */
        int quantItens = PlayerPrefs.GetInt("Quant_Itens");
        for (int i = 0; i < quantItens; i++)
        {
            //objectList.Add(PlayerPrefs.GetString("item_" + i));
            objectList.Remove(PlayerPrefs.GetString("item_" + i));
        }

        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    void Start()
    {
        pickItem = FMODUnity.RuntimeManager.CreateInstance("event:/pickItem");
        lightOrb = FMODUnity.RuntimeManager.CreateInstance("event:/lightOrb");
    }



    void OnDestroy()
    {
        PlayerPrefs.SetInt("Quant_Itens", objectList.Count);
        for (int i = 0; i < objectList.Count; i++)
        {
            PlayerPrefs.SetString("item_" + i, objectList[i]);
        }
    }

    void LateUpdate()
    {
        if (_isTrigger)
        {
            Interact();
        }
        vfx.SetActive(objectList.Count > 0);
    }

    void OnTriggerEnter(Collider other)
    {
        _Other = other.gameObject;
        if (_Other.CompareTag("Object") || _Other.CompareTag("Offering"))
        {
            _isTrigger = true;
        }
        if (other.CompareTag("Pedestal") && objectList.Contains("Offering"))
        {
            _isTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Object") || _Other.CompareTag("Offering"))
        {
            _isTrigger = false;
        }
    }

    void Interact()
    {
        playerLocomotion.isInteracting = true;

        if (playerLocomotion.isInteracting && !_Other.CompareTag("Pedestal"))
        {
            inputManager.HandleInteractInput();
            pickItem.start();

            objectList.Add(_Other.name);
            _Other.SetActive(false);
            count++;
            _isTrigger = false;
            StartCoroutine(DisableText());
        }
        else
        {
            if (playerLocomotion.isInteracting)
            {
                Debug.Log("INTERAGINDO!!");
                inputManager.HandleDeliverInput();
                //orbLight.start();
                _Other.GetComponent<OrbController>().orb.SetActive(true);
                objectList.Remove("Offering");
                _Other.GetComponent<FXController>().Move();

                _isTrigger = false;
                //triggerText.gameObject.SetActive(true);
                StartCoroutine(DisableText());
            }
        }
    }

    private IEnumerator DisableText()
    {
        yield return new WaitForSeconds(3f);
    }
}
