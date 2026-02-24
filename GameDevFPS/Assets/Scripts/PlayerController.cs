using System;
using System.Diagnostics;
using System.Collections;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerController : MonoBehaviour, IDamage, IPickup
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] Transform medHoldPos;

    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;

    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;

    int jumpCount;
    int HPOrig;

    float shootTimer;
    medStats heldMed;
    GameObject heldMedModel;

    Vector3 moveDir;
    Vector3 playerVel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
        updatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
        if (Input.GetButtonDown("Interact"))
        {
            Debug.Log("E pressed");
            UseMedkit();
        }
    }

    void movement()
    {
        shootTimer += Time.deltaTime;

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);

        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDir * speed * Time.deltaTime);

        jump();
        controller.Move(playerVel * speed * Time.deltaTime);

        playerVel.y -= gravity * Time.deltaTime;

        if (Input.GetButton("Fire1") && shootTimer >= shootRate)
        shoot();
    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            playerVel.y = jumpSpeed;
            jumpCount++;

        }
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }

    void shoot()
    {
        shootTimer = 0f;

        Vector3 origin = Camera.main.transform.position;
        Vector3 dir = Camera.main.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(origin, dir, out hit, shootDist))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
        }
    }

    public void takeDamage(int Amount)
    {
        HP -= Amount;
        updatePlayerUI();
        StartCoroutine(flashScreen());

        if (HP <= 0)
        {
            gamemanager.instance.youLose();
        }
    }

    IEnumerator flashScreen()
    {
        gamemanager.instance.playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gamemanager.instance.playerDamageFlash.SetActive(false);
    }

    private void updatePlayerUI()
    {
        gamemanager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
    }

    public void getMedStats(medStats med)
    {
        heldMed = med;

        if (heldMedModel != null)
            Destroy(heldMedModel);

        if (heldMed != null && heldMed.heldModelPrefab != null && medHoldPos != null)
        {
            heldMedModel = Instantiate(heldMed.heldModelPrefab, medHoldPos.position, medHoldPos.rotation);
            heldMedModel.transform.SetParent(medHoldPos);
            heldMedModel.transform.localPosition = Vector3.zero;
            heldMedModel.transform.localRotation = Quaternion.identity;
        }
    }

    void UseMedkit()
    {
        if (heldMed == null)
            return;

        // Heal
        HP += heldMed.healAmount;
        if (HP > HPOrig)
            HP = HPOrig;

        updatePlayerUI();

        // Remove from hand after use
        if (heldMedModel != null)
            Destroy(heldMedModel);

        heldMedModel = null;
        heldMed = null;
    }
}
