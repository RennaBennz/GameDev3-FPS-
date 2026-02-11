using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    [SerializeField] int maxHP;
    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpHeight;
    [SerializeField] int maxJumps;
    [SerializeField] int gravity;



    [SerializeField] int atkDamage;
    [SerializeField] int atkDistance;
    [SerializeField] float atkSpeed;


    int jumpCounter;
    float atkTimer;

    Vector3 moveDir;
    Vector3 playerVel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void movement()
    {
        atkTimer += Time.deltaTime;

        if (controller.isGrounded)
        {
            jumpCounter = 0;
            playerVel = Vector3.zero;
        }

        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDir * speed * Time.deltaTime);

    }

    void sprint()
    {
        if(Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }else if(Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCounter < maxJumps)
        {
            playerVel.y = jumpHeight;
            jumpCounter++;
        }
    }



}
