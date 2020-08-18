using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 MoveVector;
    private Rigidbody2D RB;
    private Animator Anim;
    public bool IsDashing = false;
    public bool FacingRight = true;
    public float Speed;
    public float DashPwr;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        RB.velocity = new Vector2(MoveVector.normalized.x, MoveVector.normalized.y) * Speed * Time.deltaTime;

        if (IsDashing && MoveVector.x != 0 || MoveVector.y != 0)
        {
            RB.AddForce(new Vector2(MoveVector.normalized.x, MoveVector.normalized.y) * DashPwr * Time.deltaTime);
            RB.velocity = Vector2.zero;
            IsDashing = false;
        }
        else if(IsDashing && MoveVector.x == 0 && MoveVector.y == 0 && FacingRight)
        {
            RB.AddForce(Vector2.right * DashPwr * Time.deltaTime);
            RB.velocity = Vector2.zero;
            IsDashing = false;
        }
        else if (IsDashing && MoveVector.x == 0 && MoveVector.y == 0 && !FacingRight)
        {
            RB.AddForce(Vector2.left * DashPwr * Time.deltaTime);
            RB.velocity = Vector2.zero;
            IsDashing = false;
        }

        #region FlipFunction
        Vector3 Scale;
        Scale = transform.localScale;
        if(MoveVector.x > 0)
        {
            FacingRight = true;
             Scale.x = 1;
        }
        else if(MoveVector.x < 0)
        {
            FacingRight = false;
            Scale.x = -1;
        }
        transform.localScale = Scale;
        #endregion


        if (MoveVector.x > 0 || MoveVector.y > 0 || MoveVector.x < 0 || MoveVector.y < 0)
        {
            Anim.SetBool("IsFlying", true);
        }
        else
        {
            Anim.SetBool("IsFlying", false);
        }
    }


    #region InputActions
    public void OnMove(InputAction.CallbackContext ctx)
    {
        MoveVector = ctx.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if(ctx.phase == InputActionPhase.Performed)
        {
            IsDashing = true;
        }
    }
    #endregion
}
