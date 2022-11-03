using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 1f;
    public ContactFilter2D movementFilter;
    public float collisionOffset = 0.05f;
    Vector2 movementInput;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    bool CanMove = true;

    public SwordAttack swordAttack;



    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        if(CanMove){
            //If movement input is not 0 try to move
            if(movementInput != Vector2.zero) {
                bool success = TryMove(movementInput);
                
                if(!success && movementInput.x > 0){
                    success = TryMove(new Vector2(movementInput.x, 0));

                    if(!success && movementInput.y > 0){
                        success = TryMove(new Vector2(0, movementInput.y));
                    }
                }

            //Set player to idle/run aniimation if moving or not
                animator.SetBool("Is_Moving", success);
            } else {
                animator.SetBool("Is_Moving", false);
            }

            //Set Direction of sprite to movement direction
            if(movementInput.x < 0){ //move left
                spriteRenderer.flipX = true;
            } else if (movementInput.x > 0){ //move right
                spriteRenderer.flipX = false;
            }
        }
    }

    private bool TryMove(Vector2 direction){
        //check for potential collisions
        int count = rb.Cast(
            direction, //X & Y values between -1 and 1 that represent the direction from the body to look for collisions
            movementFilter, //The settings that determine where a collision can occur on such laters to collide with
            castCollisions, //List of collisions to store the found collisions into after the Cast is finished
            moveSpeed * Time.fixedDeltaTime + collisionOffset); //The amount to cast equal to the movement plus an offset
        
        if(count == 0) {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        } else {
            return false;
        }
    }

    void OnMove(InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire(){
        animator.SetTrigger("swordAttack");
    }

    public void SwordAttack(){
        LockMovement();
        if(spriteRenderer.flipX == true){
            swordAttack.AttackLeft();
        } else { 
            swordAttack.AttackRight();
        }
    }

    public void endSwordAttack(){
        UnlockMovement();
        swordAttack.StopAttack();
    }

    public void LockMovement(){
        CanMove = false;
    }

    public void UnlockMovement() {
        CanMove = true;
    }
}
