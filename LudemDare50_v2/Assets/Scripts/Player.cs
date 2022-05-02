using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    Vector3 moveDirection;

    private CharacterMotor cm;
    [SerializeField] private float movementSpeed;
    [SerializeField] Camera mainCamera;
    [SerializeField] public GameObject playerPlane;
    [SerializeField] Transform hand1;
    [SerializeField] Transform hand2;
    [SerializeField] public Inventory inventory;

    private StatBarHandler statBar;
    public Vector2 moveInput;
    public bool pressedInteract;
    public bool pressedDropItem;
    public bool pressedLeftClick;
    public bool isSleeping = false;
    public bool canMove = true;
    private CharacterInputs characterInputs;
    private Transform[] childrenObjects;
    private bool isFacingRight;
    private bool isFlipping;
    private GameObject oldEquippedHand1Item;
    private GameObject oldEquippedHand2Item;
    private Vector2 mousePosition;

    private void Awake()
    {
        cm = GetComponent<CharacterMotor>();
        childrenObjects = GetComponentsInChildren<Transform>();
        statBar = GetComponent<StatBarHandler>();
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

        if (characterInputs.Player.Inventory.triggered) inventory.ToggleInventoryMenu();

        if (characterInputs.Player.MouseClick.triggered) pressedLeftClick = true;
        else pressedLeftClick = false;

        mousePosition = characterInputs.Player.Mouse.ReadValue<Vector2>();
       
        if (isSleeping)
        {
            statBar.IncrementStatBar(.015f, Stat.sanity);
        }
        if (characterInputs.Player.DropItem.triggered)
        {
            pressedDropItem = true;
        }
        else pressedDropItem = false;

       



        if (moveInput.x < 0 && isFacingRight && !isFlipping)
        {
            StopAllCoroutines();
            StartCoroutine(Flip());
        }
        else if (moveInput.x > 0 && !isFacingRight && !isFlipping)
        {
            StopAllCoroutines();
            StartCoroutine(Flip());
        }
    }

    private void FixedUpdate()
    {

        cm.SetMoveVelocity(SetMovementFromInput());

    }



    public Vector3 SetMovementFromInput()
    {
        if (!canMove) return Vector3.zero;
        moveDirection = mainCamera.transform.forward * moveInput.y;
        moveDirection = moveDirection + mainCamera.transform.right * moveInput.x;
        moveDirection.Normalize();
        moveDirection.y = 0;

        return moveDirection * movementSpeed * 10f;
    }





    private IEnumerator Flip()
    {
        float timeElapsed = 0;
        Vector3 targetScale = transform.localScale;
        targetScale.x *= -1;
        while (timeElapsed < .4f)
        {
            isFlipping = true;
            timeElapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, timeElapsed / 1.6f);
            yield return null;
        }
        isFlipping = false;
        transform.localScale = targetScale;
        isFacingRight = !isFacingRight;
         
    }

    public void EquipItemToHand(InventoryItem inventoryItem, bool lhs)
    {
        
        GameObject inHand = Instantiate(inventoryItem.itemData.pickupPrefab);
        if (inHand.GetComponent<Tool>() != null)
        {
            inHand.GetComponent<Tool>().enabled = true;
            inHand.GetComponent<Tool>().SetToolStats(inventoryItem.damage);
            inHand.GetComponent<ResourcePickup>().id = inventoryItem.id;
        }
        else if (inHand.GetComponent<Food>() != null)
        {
            inHand.GetComponent<Food>().enabled = true;
        }
        else if (inHand.GetComponent<TemperatureControl>() != null)
        {
            inHand.GetComponent<Food>().enabled = true;
        }
        inHand.GetComponent<Rigidbody>().useGravity = false;
        inHand.GetComponent<Rigidbody>().isKinematic = true;
        inHand.GetComponent<BoxCollider>().enabled = false;
        inHand.transform.SetParent(lhs ? hand1 : hand2);
        inHand.transform.localPosition = Vector3.zero;
       // inHand.transform.localScale = Vector3.one / 10;
        if (lhs)
        {
            
            Destroy(oldEquippedHand1Item);
            oldEquippedHand1Item = inHand;
        }
        else
        {
            Destroy(oldEquippedHand2Item);
            oldEquippedHand2Item = inHand;
        }
        
    } 

    public void UnequipItemInHand(InventoryItem itemToDestroy)
    {
        if (oldEquippedHand1Item != null && oldEquippedHand1Item.GetComponent<ResourcePickup>().resourceData == itemToDestroy.itemData)
            Destroy(oldEquippedHand1Item);
        else if (oldEquippedHand2Item != null && oldEquippedHand2Item.GetComponent<ResourcePickup>().resourceData == itemToDestroy.itemData)
            Destroy(oldEquippedHand2Item);
    }



public void CreateDroppedPickup(InventoryItem inventoryItem)
    {
        Vector3 pickupPosition = base.transform.position;
        pickupPosition.x += (!isFacingRight ? 2f : -2f);
        GameObject pickup = Instantiate(inventoryItem.itemData.pickupPrefab, pickupPosition, inventoryItem.itemData.pickupPrefab.transform.rotation);       
        pickup.GetComponent<ResourcePickup>().itemCount = inventoryItem.stackSize;

        pickup.GetComponent<ResourcePickup>().damage = inventoryItem.damage;
        pickup.GetComponent<ResourcePickup>().durability = inventoryItem.durability;
        pickup.GetComponent<ResourcePickup>().id = inventoryItem.id;

        pickup.GetComponent<Rigidbody>().AddForce(7f * (!isFacingRight ? base.transform.right : -base.transform.right), ForceMode.Impulse);
        Destroy(pickup, 20f);
    }

    public void EnableChildrenObjects(bool active)
    {
        
        foreach(Transform childTransform in childrenObjects)
        {
            if (childTransform != this.transform)
            {
                childTransform.gameObject.SetActive(active);
            }
            
        }
    }

    public Vector2 GetMousePosition()
    {
        return mousePosition;
    }
    
}
