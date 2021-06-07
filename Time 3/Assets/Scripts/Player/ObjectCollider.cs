using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectCollider : MonoBehaviour
{
    
    [Tooltip("Texto que � usado para mostrar mensagens na tela")]
    public TextMeshProUGUI triggerText;
    private GameObject _Other;

    [Tooltip("Tela de Fim de Jogo")]
    public GameObject gameOverUI;

    public List<string> objectList = new List<string>();

    static private int count = 0;
    

    private GameObject _Other;

    private bool _isTrigger = false;
    
    InputManeger inputmaneger;
    PlayerLocomotion playerLocomotion;

    FMOD.Studio.EventInstance pickItem;
    FMOD.Studio.EventInstance lightOrb;

    void Start()
    {
        triggerText.gameObject.SetActive(false);
        pickItem = FMODUnity.RuntimeManager.CreateInstance("event:/pickItem");
        lightOrb = FMODUnity.RuntimeManager.CreateInstance("event:/lightOrb");
    }

    void Awake()
    {
        /* Recuperando os itens quando se troca de cena */
        int quantItens = PlayerPrefs.GetInt("Quant_Itens");
        for (int i = 0; i < quantItens; i++)
        {
            //objectList.Add(PlayerPrefs.GetString("item_" + i));
            objectList.Remove(PlayerPrefs.GetString("item_" + i));
        }

        inputmaneger = GetComponent<InputManeger>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    void Start()
    {
        triggerText.gameObject.SetActive(false);
        gameOverUI.SetActive(false);
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
    }

    void OnTriggerEnter(Collider other)
    {
        _Other = other.gameObject;
        if (_Other.CompareTag("Object") || _Other.CompareTag("Offering"))
        {
            _isTrigger = true;
            triggerText.gameObject.SetActive(true);
            triggerText.text = "Aperte E para interagir";
        }
        if (other.CompareTag("Pedestal") && objectList.Contains("Offering"))
        {
            _isTrigger = true;
            triggerText.gameObject.SetActive(true);
            triggerText.text = "Aperte E para realizar a oferenda";
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Object") || _Other.CompareTag("Offering"))
        {
            triggerText.gameObject.SetActive(false);
            _isTrigger = false;
        }
    }

    void Interact()
    {
        inputmaneger.HandleInteractInput();
        if (playerLocomotion.isInteracting && !_Other.CompareTag("Pedestal"))
        {
            pickItem.start();

            objectList.Add(_Other.name);
            _Other.SetActive(false);
            count++;
            if(_Other.CompareTag("Object"))
                triggerText.text = "Voce pegou o objeto!";
            else if (_Other.CompareTag("Offering"))
                triggerText.text = "Voce pegou a oferenda!";
            _isTrigger = false;
            triggerText.gameObject.SetActive(true);
            StartCoroutine(DisableText());
        }
        else
        {
            if (playerLocomotion.isInteracting)
            {
                //orbLight.start();
                _Other.GetComponent<OrbController>().orb.SetActive(true);
                objectList.Remove("Offering");

                triggerText.text = "Voce entregou a oferenda, o orbe se acende!";
                _isTrigger = false;
                //triggerText.gameObject.SetActive(true);
                Time.timeScale = 0;
                StartCoroutine(DisableText());
            }
        }
    }

    private IEnumerator DisableText()
    {
        yield return new WaitForSeconds(3f);
        triggerText.gameObject.SetActive(false);
    }
}
