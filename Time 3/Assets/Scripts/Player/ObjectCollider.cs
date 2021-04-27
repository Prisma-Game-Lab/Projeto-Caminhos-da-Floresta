using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectCollider : MonoBehaviour
{
    public TextMeshProUGUI triggerText;

    public List<string> objectList = new List<string>();

    static private int count = 0;

    private GameObject _Other;
    public GameObject gameOverUI;

    private bool _isTrigger = false;
    public bool energy = true;
    public Animator anim;

    FMOD.Studio.EventInstance pickItem;
    FMOD.Studio.EventInstance lightOrb;

    void Start()
    {
        triggerText.gameObject.SetActive(false);
        gameOverUI.SetActive(false);
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
    }

    void OnDestroy()
    {
        PlayerPrefs.SetInt("Quant_Itens", objectList.Count);
        for (int i = 0; i < objectList.Count; i++)
        {
            PlayerPrefs.SetString("item_" + i, objectList[i]);
        }
    }

    void Update()
    {
        if (_isTrigger)
        {
            PressZ();
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

    void PressZ()
    {
        if (Input.GetKeyDown("e") && !_Other.CompareTag("Pedestal"))
        {
            pickItem.start();
            anim.SetTrigger("Pegar_item");

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
            if (Input.GetKeyDown("e"))
            {
                //orbLight.start();
                _Other.GetComponent<OrbController>().orb.SetActive(true);
                objectList.Remove("Offering");
                //triggerText.text = "Voce entregou a oferenda, o orbe se acende!";
                _isTrigger = false;
                //triggerText.gameObject.SetActive(true);
                gameOverUI.SetActive(true);
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
