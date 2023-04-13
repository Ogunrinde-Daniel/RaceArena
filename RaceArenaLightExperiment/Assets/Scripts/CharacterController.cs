using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float turnSpeed;
    public float jumpForce;
    public float movementSpeed;

    public float dirX;
    public bool jump;
    public bool canJump = false;
    public bool hasSound = false;
    public bool engineDead = false;
    public string characterName = " ";

    public float floorLevel = 1.25f + 0.01f;//the real floor level plus an offset

    public WheelRotation frontRight;
    public WheelRotation frontLeft;

    public ParticleSystem smoke1;
    public ParticleSystem smoke2;

    public TrailRenderer trailRenderer1;
    public TrailRenderer trailRenderer2;

    public ParticleSystem jumpPs1;
    public ParticleSystem jumpPs2;
    public ParticleSystem jumpPs3;
    public ParticleSystem jumpPs4;

    private GameManager gameManager;
    private SoundManager soundManager;


    private Rigidbody rb;
    public Transform forwardPos;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    void Update()
    {
        if(gameManager.gameOver) return;
        if (engineDead) return;
        if (transform.position.y < floorLevel - 0.7f)
        {
            gameManager.registerPosition(characterName);
            engineDead = true;
            Destroy(gameObject, 0.7f);
        }
        if (dirX > 0)
        {
            smoke1.Play();
            smoke2.Play();
            var no = Random.Range(0, 200);
            if (no == 0)
            {
             trailRenderer1.emitting = true;
             trailRenderer2.emitting = true;
                if (hasSound)
                {
                    soundManager.playScreech();
                }
            }
            frontRight.turnRight = true;
            frontLeft.turnRight = true;

        }
        else if (dirX < 0)
        {
            smoke1.Play();
            smoke2.Play();
            var no = Random.Range(0, 2000);
            if (no == 0)
            {
                trailRenderer1.emitting = true;
                trailRenderer2.emitting = true;
                if (hasSound)
                {
                    soundManager.playScreech();
                }
            }
            frontRight.turnLeft = true;
            frontLeft.turnLeft = true;


        }
        else
        {
            smoke1.Stop();
            smoke2.Stop();

            trailRenderer1.emitting = false;
            trailRenderer2.emitting = false;

            frontRight.turnRight = false;
            frontLeft.turnRight = false;
            frontRight.turnLeft = false;
            frontLeft.turnLeft = false;
            if (hasSound)
            {
                soundManager.stopScreech();
            }
        }

    }
    private void FixedUpdate()
    {
        if(gameManager.gameOver) return;
        if (engineDead) return;

        if (jump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpPs1.Play();
            jumpPs2.Play();
            jumpPs3.Play();
            jumpPs4.Play();
            if (hasSound)
            {
                soundManager.playjump();
            }
            jump = false;
        }
       
        //accelerate
        transform.position = Vector3.MoveTowards(transform.position, forwardPos.position, movementSpeed * Time.deltaTime);

        float yAxis = transform.eulerAngles.y + (dirX * turnSpeed * Time.deltaTime);
        if (yAxis < -360) yAxis = 360;
        if (yAxis > 360) yAxis = 0;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, yAxis, transform.eulerAngles.z);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        canJump = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        canJump = false;
    }
    

}
