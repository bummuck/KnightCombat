using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private CharacterController2D controller;
    [SerializeField] private Animator animator;
    [SerializeField] public Rigidbody2D body;
    [SerializeField] private float runSpeed = 40f;

    [Header("Coliding")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private BoxCollider2D boxCollider;

    public enum State
    {
        Normal,
        Rolling
    }
    private float horizontalMove;
    private bool jump = false;
    public State state = State.Normal;
    private float rollSpeed;
    private Vector2 rollDir = new Vector2(1f, 0f);
    private bool face_right = true;

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 10);
    }

    private void Update()
    {
        switch (state)
        {
            case State.Normal:

                if (IsGrounded())
                {
                    animator.SetBool("Grounded", true);
                }
                else
                {
                    animator.SetBool("Grounded", false);
                }

                if (Input.GetKeyDown(KeyCode.A))
                {
                    face_right = false;
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    face_right = true;
                }

                horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

                animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
                animator.SetFloat("AirSpeedY", body.velocity.y);

                if (Input.GetKeyDown(KeyCode.W))
                {
                    jump = true;
                    animator.SetTrigger("Jump");
                }

                if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
                {
                    state = State.Rolling;

                    if (face_right)
                        rollDir = new Vector2(1f, 0f);
                    else
                        rollDir = new Vector2(-1f, 0f);

                    Physics2D.IgnoreLayerCollision(8, 7);
                    rollSpeed = 10f;
                    animator.SetTrigger("Roll");
                }
                break;

            case State.Rolling:
                float rollSpeedDropMultiplier = 2f;
                rollSpeed -= rollSpeed * rollSpeedDropMultiplier * Time.deltaTime;

                float rollSpeedMinimum = 4f;
                if (rollSpeed < rollSpeedMinimum)
                {
                    state = State.Normal;
                    Physics2D.IgnoreLayerCollision(8, 7, false);
                }
                break;
        }

    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Normal:
                controller.Move(horizontalMove * Time.deltaTime, false, jump);
                jump = false;
                break;

            case State.Rolling:
                body.velocity = rollDir * rollSpeed;
                break;
        }

    }

    private bool IsGrounded()
    {
        float extraHeight = .02f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, extraHeight, groundMask);
        return raycastHit;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
            transform.SetParent(collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
            transform.SetParent(null);
    }

}
