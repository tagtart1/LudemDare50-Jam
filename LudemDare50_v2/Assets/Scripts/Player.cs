using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    private CharacterMotor cm;
    [SerializeField] private float movementSpeed;
    [SerializeField] Camera mainCamera;
   
    [SerializeField] Transform equippedItem;
    public Vector2 moveInput;
    public bool pressedInteract;
    
    private CharacterInputs characterInputs;
    private bool isFacingRight;

    private void Awake()
    {
        cm = GetComponent<CharacterMotor>();
    }


    private void OnEnable()
    {
        if (characterInputs == null)
        {
            characterInputs = new CharacterInputs();
            characterInputs.Player.Movement.performed += i => moveInput = i.ReadValue<Vector2>();
            characterInputs.Enable();
        }
    }

    private void OnDisable()
    {
        characterInputs.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (characterInputs.Player.Interact.triggered) pressedInteract = true;
        else pressedInteract = false;

        if (moveInput.x < 0 && isFacingRight) Flip();
        else if (moveInput.x > 0 && !isFacingRight) Flip();
    }

    private void FixedUpdate()
    {
        cm.SetMoveVelocity(SetMovementFromInput());
    }

    public Vector3 SetMovementFromInput()
    {
        Vector3 moveDirection =mainCamera.transform.forward * moveInput.y;
        moveDirection = moveDirection + mainCamera.transform.right * moveInput.x;
        moveDirection.Normalize();
        moveDirection.y = 0;

        return moveDirection * movementSpeed * 10f;
    }

    private void Flip()
    {
        Vector3 targetScale = transform.localScale;
        targetScale.x *= -1;
        transform.localScale = targetScale;
        isFacingRight = !isFacingRight;
    }

    //private IEnumerator Flip()
    //{
    //    Vector3 targetScale = transform.localScale;
       
    //    targetScale.x *= -1;
    //    while (transform.localScale != targetScale)
    //    {
    //        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, .5f);
            
    //        yield return null;
    //    }

    //    transform.localScale = targetScale;
    //    isFacingRight = !isFacingRight;
    //}
}
