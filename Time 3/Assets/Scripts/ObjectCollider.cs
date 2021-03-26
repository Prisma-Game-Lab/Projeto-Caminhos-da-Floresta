using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectCollider : MonoBehaviour
{
    [Tooltip("Referencia texto que conta objetos")]
    public GameObject Object;

    public TextMeshProUGUI triggerText;

    public List<string> objectList = new List<string>();

    static private int count = 0;

    private GameObject Other;

    private bool isTrigger = false;
    public bool energy = true;



    void Start()
    {
        Object.SetActive(false);
        triggerText.gameObject.SetActive(false);
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
        if (isTrigger)
        {
            PressZ();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Other = other.gameObject;
        if (Other.CompareTag("Object") || Other.CompareTag("Offering"))
        {
            isTrigger = true;
            triggerText.gameObject.SetActive(true);
            triggerText.text = "Aperte E para interagir";
        }
        if (other.CompareTag("Pedestal") && objectList.Contains("Offering"))
        {
            isTrigger = true;
            triggerText.gameObject.SetActive(true);
            triggerText.text = "Aperte E para realizar a oferenda";
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Object") || Other.CompareTag("Offering"))
        {
            triggerText.gameObject.SetActive(false);
            isTrigger = false;
        }
    }

    void PressZ()
    {
        if (Input.GetKeyDown("e") && !Other.CompareTag("Pedestal"))
        {
            objectList.Add(Other.name);
            Other.SetActive(false);
            count++;
            if(Other.CompareTag("Object"))
                triggerText.text = "Voce pegou o objeto!";
            else if (Other.CompareTag("Offering"))
                triggerText.text = "Voce pegou a oferenda!";
            isTrigger = false;
            triggerText.gameObject.SetActive(true);
            StartCoroutine(DisableText());
        }
        else
        {
            if (Input.GetKeyDown("e"))
            {
                Other.GetComponent<OrbController>().orb.SetActive(true);
                objectList.Remove("Offering");
                triggerText.text = "Voce entregou a oferenda, o orbe se ascende!";
                isTrigger = false;
                triggerText.gameObject.SetActive(true);
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
