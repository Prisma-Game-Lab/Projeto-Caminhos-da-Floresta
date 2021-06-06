using System.Collections;
using UnityEngine;
using TMPro;

public class RecieveGift : MonoBehaviour
{
    public Transform player;
    public string present;

    public GameObject Offering;
    private bool _isTrigger = false;

    FMOD.Studio.EventInstance giveItem;

    private void Start()
    {
        Offering.SetActive(false);
        player = GameObject.FindWithTag("Player").transform;
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
            _isTrigger = true;
        }

    }

    void OnTriggerExit(Collider other)
    {

    }

    void PressZ()
    {
        if (Input.GetKeyDown("e"))
        {
            giveItem.start();

            Offering.SetActive(true);
            Offering.transform.position = this.transform.position;


            player.gameObject.GetComponent<ObjectCollider>().objectList.Remove(present);

            _isTrigger = false;
            _isTrigger = false;
            this.gameObject.transform.position = new Vector3(0, -30);
        }
    }

}
