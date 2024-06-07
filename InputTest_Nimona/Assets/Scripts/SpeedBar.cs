using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBar : MonoBehaviour
{
    [SerializeField] private Image speedBar;
    private Rigidbody2D playerRB;
    private float speed;
    private float maxSpeed = 700f;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        playerRB = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = Mathf.Abs(playerRB.velocity.x);

        float speedFlr = Mathf.Floor(speed);

        // Debug.Log($"X SPEED: {speedFlr} MAX X SPEED: {maxSpeed}");

        float barFillAmount = Mathf.InverseLerp(0f,maxSpeed,speedFlr);
        speedBar.fillAmount = barFillAmount;
    }
}