using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;

    [Header("----- Player Stats -----")]
    [Range(1, 10)]    [SerializeField] int HP;
    [Range(1, 5)]     [SerializeField] float speed;
    [Range(1, 3)]     [SerializeField] int jumps;
    [Range(5, 25)]    [SerializeField] int jumpSpeed;
    [Range(-15, -35)] [SerializeField] int gravity;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] Transform shootPos;

    int jumpCount;
    Vector3 moveDir;
    Vector3 playerVel;
    bool isShooting;

    public bool isSprinting;
    float walkSpeed = 6;
    float sprintSpeed = 10;

    private Vector3 crouchHeight = new Vector3(1, 0.5f, 1);
    private Vector3 playerHeight = new Vector3(1, 2, 1);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if (Input.GetButton("Fire1") && !isShooting)
        {
            StartCoroutine(Shoot());
        }
    }

    void Movement()
    {
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        moveDir = Input.GetAxis("Horizontal") * transform.right
                + Input.GetAxis("Vertical") * transform.forward;

        controller.Move(moveDir * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && jumpCount < jumps)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
            speed = sprintSpeed;
        }
        else
        {
            isSprinting = false;
            speed = walkSpeed;
        }

        if (Input.GetKey(KeyCode.C))
        {
            transform.localScale = crouchHeight;
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        }
        else
        {
            transform.localScale = playerHeight;
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }

        playerVel.y += gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);


    }

    IEnumerator Shoot()
    {
        isShooting = true;

        Instantiate(bullet, shootPos.position, shootPos.transform.rotation);

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
    }
}
