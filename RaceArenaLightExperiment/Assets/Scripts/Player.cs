using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        controller.hasSound = true;
        controller.characterName = PlayerInfo.playerName;

    }
    void Update()
    {
        controller.dirX = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && (controller.canJump || transform.position.y <= controller.floorLevel))
        {
            controller.jump = true;
        }
    }
}
