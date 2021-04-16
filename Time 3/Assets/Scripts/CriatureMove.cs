using System.Collections;
using UnityEngine;
using TMPro;

public class CriatureMove : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI triggerText;
    public string present;

    public GameObject Offering;
    private bool _isTrigger = false;

    FMOD.Studio.EventInstance giveItem;

    private void Start()
    {
        Offering.SetActive(false);
        giveItem = FMODUnity.RuntimeManager.CreateInstance("event:/giveItem");
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
        if (other.CompareTag("Player") && player.gameObject.GetComponent<ObjectCollider>().objectList.Contains(present))
        {
            triggerText.gameObject.SetActive(true);
            triggerText.text = "Aperte E para dar o presente";
            _isTrigger = true;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            triggerText.gameObject.SetActive(false);
        }
    }

    void PressZ()
    {
        if (Input.GetKeyDown("e"))
        {
            giveItem.start();

            Offering.SetActive(true);
            Offering.transform.position = this.transform.position;
            

            player.gameObject.GetComponent<ObjectCollider>().objectList.Remove(present);

            triggerText.text = "Voce entregou o presente e recebeu uma oferenda!";
            _isTrigger = false;
            _isTrigger = false;
            triggerText.gameObject.SetActive(true);
            StartCoroutine(DisableText());
            this.gameObject.transform.position = new Vector3(0, -30);
        }
    }

    private IEnumerator DisableText()
    {
        yield return new WaitForSeconds(1.5f);
        triggerText.gameObject.SetActive(false);
    }
}
