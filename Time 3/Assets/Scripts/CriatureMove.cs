using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CriatureMove : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI triggerText;
    public string present;

    public GameObject Offering;
    private bool isTrigger = false;

    private void Start()
    {
        Offering.SetActive(false);
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
        if (other.CompareTag("Player") && player.gameObject.GetComponent<ObjectCollider>().objectList.Contains(present))
        {
            triggerText.gameObject.SetActive(true);
            triggerText.text = "Aperte E para dar o presente";
            isTrigger = true;
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
            Offering.SetActive(true);
            Offering.transform.position = this.transform.position;
            

            player.gameObject.GetComponent<ObjectCollider>().objectList.Remove(present);

            triggerText.text = "Voce entregou o presente e recebeu uma oferenda!";
            isTrigger = false;
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
