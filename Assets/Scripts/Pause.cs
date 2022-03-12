using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    bool inPaused = false;
    public void pauseGame(){
        if (inPaused){
            Time.timeScale = 1;
            inPaused = false;
        }else{
            Time.timeScale = 0;
            inPaused = true;
        }
    }
}
