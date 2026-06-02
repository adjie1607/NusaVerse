using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource footstepAudio;

    [Header("Movement Stats")]
    public float moveSpeed = 7f;
    public float groundDrag = 5f;

    [Header("Jump Stats")]
    public float jumpForce = 12f;
    public float jumpCooldown = 0.25f;
    public float airMultiplier = 0.4f;
    bool readyToJump = true;

    [Header("Jatuh (Gravity)")] // <--- INI SETTINGAN BARUNYA
    public float fallMultiplier = 2.5f; // Semakin besar angka ini, semakin cepat jatuh
    public float lowJumpMultiplier = 2f; // Kalau spasi dilepas cepat, loncatnya pendek

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        if (jumpForce == 0) jumpForce = 12f;
    }

    void Update()
    {
        // Menggunakan SphereCast (Bola) biar lebih gampang deteksi tangga
        // 0.4f adalah radius bolanya (sesuaikan dengan lebar player kamu)
        grounded = Physics.SphereCast(transform.position, 0.4f, Vector3.down, out RaycastHit hitInfo, playerHeight * 0.5f + 0.2f, whatIsGround);

        // Inventory Check
        if (InventoryUI.isInventoryOpen)
        {
            if (footstepAudio != null && footstepAudio.isPlaying) footstepAudio.Stop();
            return;
        }

        MyInput();
        SpeedControl();
        HandleFootsteps();
        ApplyBetterJumpPhysics(); // <--- Panggil fungsi baru disini

        // Drag Control
        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
    }

    private void FixedUpdate()
    {
        if (!InventoryUI.isInventoryOpen)
        {
            MovePlayer();
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    // === LOGIKA PEMBERAT JATUH (BIAR CEPET MENDARAT) ===
    void ApplyBetterJumpPhysics()
    {
        // Kalau sedang jatuh (velocity Y minus), kita tambah beban ke bawah
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        // Kalau sedang loncat naik tapi tombol spasi dilepas (Low Jump), kita beratin juga biar loncat pendek
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(jumpKey))
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatvel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatvel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatvel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void HandleFootsteps()
    {
        if (footstepAudio == null) return;

        bool isMoving = (horizontalInput != 0 || verticalInput != 0);

        if (grounded && isMoving)
        {
            if (!footstepAudio.isPlaying) footstepAudio.Play();
        }
        else
        {
            if (footstepAudio.isPlaying) footstepAudio.Stop();
        }
    }
}