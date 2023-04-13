using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    public float finalDecayTime;                //time it takes for polygon to turn complete red
    public float finalRespawnTime;              //time it takes for polygon to respawn
    public float transitionSpeed;               //speed which it takes for polygon to slowly go down (and come up)
    
    public bool startDecayTime = false;         //should the decayTime start countdown
    public bool startRespawnTime = false;       //should the respawnTime start countdown
    public bool startTransitionTime = false;    //should the transitionTime start countdown

    private float decayTime = 0f;                //stores the decay time as it increments
    private float respawnTime = 0f;              //stores the decay time as it increments

    private Renderer _renderer;
    private Vector3 originalPos;
    private static GameManager gameManager;
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        originalPos = transform.position;
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

    }

    void Update()
    {
        if(gameManager.gameOver) return;

        if (startDecayTime)
        {
           
            decayTime += Time.deltaTime;
            float greenAndBlue = 1 - Mathf.Min(decayTime / finalDecayTime, 1);
            _renderer.material.color = new Color(1, greenAndBlue, greenAndBlue);
            if (decayTime > finalDecayTime)
            {
                decayTime = 0f;
                startDecayTime = false;
                startTransitionTime = true;
            }
            
        }
        if (startTransitionTime)
        {
            Vector3 newPos = new Vector3(transform.position.x, originalPos.y - 1, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, newPos, transitionSpeed * Time.deltaTime);
            if (Mathf.Approximately(transform.position.y, newPos.y))
            {
                GetComponent<Renderer>().enabled = false;
                GetComponent<Collider>().enabled = false;
                startTransitionTime = false;
                startRespawnTime = true;
            }

        }
        if (startRespawnTime)
        {
            respawnTime += Time.deltaTime;
            if (respawnTime > finalRespawnTime)
            {
                _renderer.material.color = gameManager.hexagonRespawnColor;
                GetComponent<Renderer>().enabled = true;
                GetComponent<Collider>().enabled = true;
                transform.position = Vector3.MoveTowards(transform.position, originalPos, transitionSpeed * Time.deltaTime);
                if (Mathf.Approximately(transform.position.y, originalPos.y))
                {
                    respawnTime = 0;
                    startRespawnTime = false;
                }

            }

        }

    }

    public void ResetHexagon()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        _renderer.material.color = Color.white;
        GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
        transform.position = originalPos;


        decayTime = 0f;                
        respawnTime = 0f;              

        startDecayTime = false;         
        startRespawnTime = false;       
        startTransitionTime = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (gameManager.gameOver)
            return;

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Wheel"))
        {
            if(!startDecayTime)startDecayTime = true;
        }

    }
    private void OnTriggerEnter(Collider collision)
    {
        if (gameManager.gameOver)
            return;

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Wheel"))
        {
            if (!startDecayTime)
                startDecayTime = true;
        }
    }

}
