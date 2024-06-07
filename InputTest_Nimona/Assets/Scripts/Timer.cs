using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timertext;
    private float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        int miliseconds = (int) (time * 100) % 100;
        int seconds = (int) (time % 60) % 60;
        int minutes = (int) time / 60;

        timertext.text = string.Format("{0:00}:{1:00}:{2:00}",minutes,seconds,miliseconds);
    }
}