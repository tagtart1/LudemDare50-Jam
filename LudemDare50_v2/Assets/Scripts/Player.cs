using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class Player : MonoBehaviour
{
    [SerializeField] public Inventory inventory;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Animator anim;
    [SerializeField] private TextMeshProUGUI aliveTimerUI;
    [SerializeField] private Transform characterPlane;
    [SerializeField] private Transform hand1;
    [SerializeField] private Transform hand2;
    [SerializeField] private float movementSpeed;

    private CharacterMotor cm;
    private StatBarHandler statBar;
    private CharacterInputs characterInputs;

    private Transform[] childrenObjects;
    private GameObject oldEquippedHand1Item;
    private GameObject oldEquippedHand2Item;

    private Vector3 moveDirection;
    private Vector3 inHandScale;
    private Vector2 mousePosition;
    private Vector2 moveInput;

    private float timeAlive;
    private bool pressedPause;
    private bool canMove = true;
    private bool isDead;
    private bool isPaused;
    private bool isFlipping;
    private bool isFacingRight;
    private bool isSleeping = false;
    private bool pressedInteract;
    private bool pressedDropItem;
    private bool pressedLeftClick;
  
    public bool CanMove { set => canMove = value; }
    public bool IsSleeping { get => isSleeping; set => isSleeping = value; }
    public bool PressedInteract { get => pressedInteract; }
    public bool PressedDropItem { get => pressedDropItem; }
    public bool PressedLeftClick { get => pressedLeftClick; }
    public bool PressedPause { get => pressedPause; }


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
        timeAlive += Time.deltaTime;
        
        if (characterInputs.Player.Interact.triggered && !isPaused) pressedInteract = true;
        else pressedInteract = false;

       

        if (characterInputs.Player.Inventory.triggered && !isPaused) inventory.ToggleInventoryMenu();

        if (characterInputs.Player.MouseClick.triggered && !isPaused) pressedLeftClick = true;
        else pressedLeftClick = false;

        if (characterInputs.Player.Pause.triggered) pressedPause = true;
        else pressedPause = false;

        mousePosition = characterInputs.Player.Mouse.ReadValue<Vector2>();
       
        if (isSleeping)
        {
            statBar.IncrementStatBar(.1f, Stat.sanity);
        }
        if (characterInputs.Player.DropItem.triggered)
        {
            pressedDropItem = true;
        }
        else pressedDropItem = false;


        if (moveInput != Vector2.zero && !isPaused)
        {
            
            anim.SetFloat("Speed", 1);
            
        }
        else anim.SetFloat("Speed", 0);


        if (moveInput.x < 0 && isFacingRight && !isFlipping)
        {
            
            StartCoroutine(Flip());
        }
        else if (moveInput.x > 0 && !isFacingRight && !isFlipping)
        {
            
            StartCoroutine(Flip());
        }


        aliveTimerUI.text = GetTimeAlive(); // updatas onscreen timer
    }

    private void FixedUpdate()
    {
        cm.SetMoveVelocity(SetMovementFromInput());
    }

   


    public Vector3 SetMovementFromInput()
    {
        if (!canMove || isPaused) return Vector3.zero;
        moveDirection = mainCamera.transform.forward * moveInput.y;
        moveDirection = moveDirection + mainCamera.transform.right * moveInput.x;
        moveDirection.Normalize();
        moveDirection.y = 0;

        return moveDirection * movementSpeed * 10f;
    }





    private IEnumerator Flip() 
    {
        float timeElapsed = 0;
        Vector3 handScale = Vector3.one;
     
        Vector3 targetScale = transform.localScale;
        targetScale.x *= -1;
        while (timeElapsed < .4f)
        {
            isFlipping = true;
            timeElapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, timeElapsed / 1.6f);
            if (oldEquippedHand1Item != null)
            oldEquippedHand1Item.transform.localScale = handScale;
            if (oldEquippedHand2Item != null)
            oldEquippedHand2Item.transform.localScale = handScale;
            yield return null;
        }
        isFlipping = false;
        transform.localScale = targetScale;
        isFacingRight = !isFacingRight;
       
    }

    public void EquipItemToHand(InventoryItem inventoryItem, bool lhs)
    {
        
        GameObject inHand = Instantiate(inventoryItem.itemData.pickupPrefab);
        inHandScale = inHand.transform.localScale;
        if (isFacingRight) // makes the sprite face the correct direction on equipping
        {
            inHandScale.x *= -1f;
            inHand.transform.localScale = inHandScale;
        }

        if (inHand.GetComponent<Tool>() != null) //items are only a tool, temperature control, or consumable so we check
        {
            inHand.GetComponent<Tool>().enabled = true;
            inHand.GetComponent<Tool>().SetToolStats(inventoryItem.damage);
            inHand.GetComponent<ResourcePickup>().id = inventoryItem.id;
        }
        else if (inHand.GetComponent<TemperatureControl>() != null) 
        {
            inHand.GetComponent<TemperatureControl>().enabled = true;
            inHand.GetComponent<ResourcePickup>().id = inventoryItem.id;
        }
        else if (inHand.GetComponent<Consumable>() != null)
        {
            inHand.GetComponent<Consumable>().enabled = true;
        }
        inHand.GetComponent<Rigidbody>().useGravity = false;
        inHand.GetComponent<Rigidbody>().isKinematic = true;
        inHand.GetComponent<BoxCollider>().enabled = false;
        inHand.transform.SetParent(lhs ? hand1 : hand2);
        inHand.transform.localPosition = Vector3.zero;
    
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
    

    public void Attack() //animation trigger
    {
        anim.SetTrigger("Attack");
    }

    public bool IsDead()
    {
        return isDead;
    }
  

    public void SetPlayerDead(bool value)
    {
        isDead = value;
    }

    public void DisableInputs()
    {
        isPaused = true;
    }

    public void EnableInputs()
    {
        isPaused = false;
    }

    public string GetTimeAlive()
    {
        string time = Mathf.Floor(timeAlive / 60).ToString("00") + ":" + (timeAlive % 60).ToString("00");
        return time;
    }

    public Transform GetCharacterPlane()
    {
        return characterPlane;
    }

}
