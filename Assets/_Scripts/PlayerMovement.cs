using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speedValue = 5f;
    [SerializeField] private float jumpValue = 4f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float mouseSensitivity = 5f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject cameraHolder;

    private Vector3 spawnPosition;
    private Vector3 movementDirection;
    private float horizontalMovement;
    private float verticalMovement;
    private KeyCode jumpKey = KeyCode.Space;
    private bool isGrounded = true;
    private bool canMove;

    private float mouseX;
    private float mouseY;
    private float xRotation;
    private float yRotation;
    private float maxLook = 60f;

    private void Awake()
    {
        GameManager.OnGameStarted += Init;

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        spawnPosition = transform.position;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStarted -= Init;
    }

    private void Init(bool isGameStarted)
    {
        canMove = isGameStarted;

        if (isGameStarted)
        {
            transform.position = spawnPosition;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
            Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        MovePlayer();
        MouseMovement();
        Jump();
        CheckFallPosition();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isGrounded) return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void MovePlayer()
    {
        if (!canMove) return;

        if (!isGrounded) return;

        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        if (Mathf.Approximately(horizontalMovement, 0f) &&
            Mathf.Approximately(verticalMovement, 0f) &&
            isGrounded == true)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else if (Mathf.Abs(rb.velocity.x) < maxSpeed &&
                 Mathf.Abs(rb.velocity.y) < maxSpeed &&
                 Mathf.Abs(rb.velocity.z) < maxSpeed)
        {
            movementDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;
            rb.AddForce(movementDirection.normalized * speedValue, ForceMode.Acceleration);
        }
    }

    private void MouseMovement()
    {
        if (!canMove) return;

        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        xRotation -= mouseY * mouseSensitivity;
        yRotation += mouseX * mouseSensitivity;

        xRotation = Mathf.Clamp(xRotation, -maxLook, maxLook);
        cameraHolder.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void Jump()
    {
        if (!Input.GetKeyDown(jumpKey)) return;

        if (!canMove) return;

        if (!isGrounded) return;

        rb.AddForce(Vector3.up * jumpValue, ForceMode.VelocityChange);
        isGrounded = false;
    }

    private void CheckFallPosition()
    {
        if (transform.position.y < -5)
            transform.position = spawnPosition;
    }
}