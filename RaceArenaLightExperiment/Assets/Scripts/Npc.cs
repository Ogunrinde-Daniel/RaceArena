using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    public bool holeInFront = false;
    private int count  = 0;
    
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Hexagon"))
        {
            count++;
            holeInFront = false;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hexagon"))
        {
            count--;
            if (count <= 0) holeInFront = true;
        }
    }

}

public class Npc : MonoBehaviour
{
    public GameObject scout;    //checks ahead for holes
    public GameObject leftScout;    //checks ahead for holes
    public GameObject rightScout;    //checks ahead for holes

    private CharacterController controller;

    private float responseDelay = 0.5f; //time it takes for the Npc to change direction
    private float delayTimer = 0f;

    public enum TurnState{ RIGHT, LEFT, STRAIGHT};
    public float[] weight = new float[] { 0.2f, 0.2f, 0.6f};//sum must be equal to 1, size must be AttackStates size - 1(none)

    void Start()
    {
        controller = GetComponent<CharacterController>();
        scout.AddComponent<TriggerHandler>();
        leftScout.AddComponent<TriggerHandler>();
        rightScout.AddComponent<TriggerHandler>();
    }

    void Update()
    {
        delayTimer += Time.deltaTime;

        if (delayTimer >= responseDelay)
        {
            delayTimer = 0f;
            var randomNo = generateRandomNumber(weight);
            switch (randomNo)
            {
                case 0:
                    controller.dirX = 1;
                    break;
                case 1:
                    controller.dirX = -1;
                    break;
                case 2:
                    controller.dirX = 0;
                    break;
            }
        }

        if (scout.GetComponent<TriggerHandler>().holeInFront == true)
        {
            if (leftScout.GetComponent<TriggerHandler>().holeInFront == true)
            {
                controller.dirX = 2;
            }
            else if (rightScout.GetComponent<TriggerHandler>().holeInFront == true)
            {
                controller.dirX = -2;
            }
            else if((controller.canJump || transform.position.y <= controller.floorLevel))
            {
                var randomNo2 = Random.Range(0, 40);
                if (randomNo2 < 39)
                {
                    controller.dirX = (Random.Range(0, 1) == 0) ? 2 : -2;
                }
                else
                {
                    controller.jump = true;
                }
            }

        }

        

    }


    public int generateRandomNumber(float[] weights)
    {
        float totalWeight = 0f;
        foreach (float weight in weights)
        {
            totalWeight += weight;
        }

        float randomValue = UnityEngine.Random.value * totalWeight;
        for (int i = 0; i < weights.Length; i++)
        {
            if (randomValue < weights[i])
            {
                return i;
            }
            randomValue -= weights[i];
        }

        return weights.Length - 1;
    }

}
