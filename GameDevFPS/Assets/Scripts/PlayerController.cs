using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerController : MonoBehaviour, IDamage, IPickup
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] List<medStats> medList = new List<medStats>();

    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;

    [SerializeField] Transform medHoldPos;

    int jumpCount;
    int HPOrig;

    float shootTimer;

    // --- Medkit inventory ---
    int medListPos;

    medStats heldMed;
    GameObject heldMedModel;

    Vector3 moveDir;
    Vector3 playerVel;

    void Start()
    {
        HPOrig = HP;
        updatePlayerUI();
        spawnPlayer();
    }

    void Update()
    {
        movement();
        sprint();

        // Scroll through held medkits (if you have more than one)
        selectMedkit();

        // Use currently selected medkit
        if (Input.GetButtonDown("Interact"))
        {
            Debug.Log("Interact pressed");
            UseMedkit();
        }
    }

    public void spawnPlayer()
    {
        controller.transform.position = gamemanager.instance.playerSpawnPos.transform.position;
        Physics.SyncTransforms();
        HP = HPOrig;
        updatePlayerUI();
    }

    void movement()
    {
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
        // Add the medkit to our inventory
        medList.Add(med);

        // Auto-select the newest medkit picked up
        medListPos = medList.Count - 1;

        changeMedkit();
    }

    void changeMedkit()
    {
        // If we have no medkits, clear held medkit + model
        if (medList.Count == 0)
        {
            heldMed = null;

            if (heldMedModel != null)
                Destroy(heldMedModel);

            heldMedModel = null;
            return;
        }

        heldMed = medList[medListPos];

        // Remove old held model
        if (heldMedModel != null)
            Destroy(heldMedModel);

        // Spawn the new held model
        if (heldMed != null && heldMed.heldModelPrefab != null && medHoldPos != null)
        {
            heldMedModel = Instantiate(heldMed.heldModelPrefab, medHoldPos.position, medHoldPos.rotation);
            heldMedModel.transform.SetParent(medHoldPos);
            heldMedModel.transform.localPosition = Vector3.zero;
            heldMedModel.transform.localRotation = Quaternion.identity;
        }
    }

    void selectMedkit()
    {
        // Need at least 2 to scroll
        if (medList.Count <= 1)
            return;

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && medListPos < medList.Count - 1)
        {
            medListPos++;
            changeMedkit();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && medListPos > 0)
        {
            medListPos--;
            changeMedkit();
        }
    }

    void UseMedkit()
    {
        if (medList.Count == 0 || heldMed == null)
            return;

        // Don't waste a medkit if already full HP
        if (HP >= HPOrig)
            return;

        // Heal
        HP += heldMed.healAmount;
        if (HP > HPOrig)
            HP = HPOrig;

        updatePlayerUI();

        // Remove used medkit from inventory
        medList.RemoveAt(medListPos);

        // Clamp index
        if (medListPos >= medList.Count)
            medListPos = medList.Count - 1;

        // Refresh held model (or clear if none left)
        changeMedkit();
    }
}