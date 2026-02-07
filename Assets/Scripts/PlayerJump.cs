using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // 新しいInput Systemを利用するため追加

public class PlayerJump : MonoBehaviour
{

    public float moveSpeed = 7f;
    public float minJumpForce = 5f; // 最小ジャンプ力
    public float maxJumpForce = 15f; // 最大ジャンプ力
    public float chargeSpeed = 10f; // 1秒間に溜まる力

    private Rigidbody rb;
    private bool isGrounded;
    private float currentJumpForce;
    private bool isCharging = false;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; // 回転を固定
    }

    [System.Obsolete]
    void Update()
    {
        if (Keyboard.current == null) return;

        float rayLength = 0.6f;
        Vector3 rayStart = transform.position;

        bool hit = Physics.Raycast(rayStart, Vector3.down, out RaycastHit hitInfo, rayLength);

        if (hit && hitInfo.collider.CompareTag("Platform"))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        Debug.DrawRay(rayStart, Vector3.down * rayLength, isGrounded? Color.green : Color.red);

        // 自由移動
        float x = 0f;
        float z = 0f;

        if (Keyboard.current.aKey.isPressed) z = 1f;
        if (Keyboard.current.dKey.isPressed) z = -1f;
        if (Keyboard.current.wKey.isPressed) x = 1f;
        if (Keyboard.current.sKey.isPressed) x = -1f;
        moveInput = new Vector2(x, z).normalized;

        // ジャンプ
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            isCharging = true;
            currentJumpForce = minJumpForce;
        }

        // ジャンプチャージ
        if (isCharging && Keyboard.current.spaceKey.isPressed)
        {
            currentJumpForce = Mathf.Min(currentJumpForce + chargeSpeed * Time.deltaTime, maxJumpForce);
        }

        if (isCharging && Keyboard.current.spaceKey.wasReleasedThisFrame)
        {
            Jump();
        }
    }


    void FixedUpdate()
    {
        Vector3 currentVel = rb.linearVelocity;
        rb.linearVelocity = new Vector3(moveInput.x * moveSpeed, currentVel.y, moveInput.y * moveSpeed);
    }

    void Jump()
    {
        // 溜めた力でジャンプ
        rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);

        // 状態をリセット
        isGrounded = false;
        isCharging = false;
        currentJumpForce = 0f;
    }
}