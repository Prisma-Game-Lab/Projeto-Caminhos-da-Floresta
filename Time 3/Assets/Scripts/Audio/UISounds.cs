using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISounds : MonoBehaviour
{
    FMOD.Studio.EventInstance selectUI;
    FMOD.Studio.EventInstance backUI;

    // Start is called before the first frame update
    void Start()
    {
        selectUI = FMODUnity.RuntimeManager.CreateInstance("event:/UI/select");
        backUI = FMODUnity.RuntimeManager.CreateInstance("event:/UI/back");
    }

   public void PlaySelect(){
       selectUI.start();
   }

   public void PlayBack(){
       backUI.start();
   }
}
