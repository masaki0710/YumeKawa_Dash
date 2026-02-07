using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private Rigidbody rb;
    private Renderer[] playRenderers;

    // ジャンプ時の色変化
    public Color normalColor = Color.white;
    public Color chargedColor = new Color(1f, 0.5f, 1.0f);

    private bool isGrounded;
    private float currentJumpForce;
    private bool isCharging = false;
    private Vector2 moveInput;

    // 音響関連
    public AudioSource chargeAudioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playRenderers = GetComponentsInChildren<Renderer>();

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        SetPlayerColor(normalColor);
    }

    void Update()
    {
        HandleGroundCheck();
        HandleInput();
        HandleJumpCharge();
    }

    private void HandleGroundCheck()
    {
        float rayLength = 0.7f;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, rayLength);

        Debug.DrawRay(transform.position, Vector3.down * rayLength, isGrounded ? Color.green : Color.red);
    }

    private void HandleInput()
    {
        if (Keyboard.current == null) return;

        float x = 0f, z = 0f;
        if (Keyboard.current.aKey.isPressed) z = 1f;
        if (Keyboard.current.dKey.isPressed) z = -1f;
        if (Keyboard.current.wKey.isPressed) x = 1f;
        if (Keyboard.current.sKey.isPressed) x = -1f;

        moveInput = new Vector2(x, z).normalized;
    }

    private void HandleJumpCharge()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            isCharging = true;
            currentJumpForce = GameConfig.MinJumpForce;

            if (chargeAudioSource != null)
            {
                chargeAudioSource.Play();
            }
        }

        if (isCharging)
        {
            if (Keyboard.current.spaceKey.isPressed)
            {
                currentJumpForce = Mathf.Min(currentJumpForce + GameConfig.JumpChargeSpeed * Time.deltaTime, GameConfig.MaxJumpForce);

                float chargeRatio = (currentJumpForce - GameConfig.MinJumpForce) / (GameConfig.MaxJumpForce - GameConfig.MinJumpForce);

                Color tColor = Color.Lerp(normalColor, chargedColor, chargeRatio);
                SetPlayerColor(tColor);
            }

            if (Keyboard.current.spaceKey.wasReleasedThisFrame)
            {
                Jump();

                if (chargeAudioSource != null)
                {
                    chargeAudioSource.Stop();
                }
            }
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);

        isCharging = false;
        currentJumpForce = 0f;
        SetPlayerColor(normalColor);
    }

    private void SetPlayerColor(Color color)
    {
        if (playRenderers != null)
        {
            foreach (Renderer r in playRenderers)
            {
                r.material.color = color;
            }
        }

    }


    void FixedUpdate()
    {
        Vector3 moveVel = new Vector3(moveInput.x * GameConfig.DefaultPlayerMoveSpeed, 0, moveInput.y * GameConfig.DefaultPlayerMoveSpeed);
        Vector3 platformVel = Vector3.left * GameConfig.PlatformMoveSpeed;
        float yVel = rb.linearVelocity.y;

        rb.linearVelocity = new Vector3(moveVel.x + platformVel.x, yVel, moveVel.z);

        Vector3 clampedPosition = rb.position;
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, -GameConfig.ObstacleFloorWidth/2.0f, GameConfig.ObstacleFloorWidth/2.0f);

        rb.position = clampedPosition;
    }

    private void ApplyMovement()
    {
        Vector3 moveVel = new Vector3(moveInput.x * GameConfig.DefaultPlayerMoveSpeed, 0, moveInput.y * GameConfig.DefaultPlayerMoveSpeed);
        Vector3 platformVel = Vector3.left * GameConfig.PlatformMoveSpeed;
        rb.linearVelocity = new Vector3(moveVel.x + platformVel.x, rb.linearVelocity.y, moveVel.z);
    }

    private void ApplyPositionClamp()
    {
        Vector3 clampedPosition = rb.position;
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, -GameConfig.ObstacleFloorWidth, GameConfig.ObstacleFloorWidth);
        rb.position = clampedPosition;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = false;
            transform.SetParent(null);
        }
    }
}