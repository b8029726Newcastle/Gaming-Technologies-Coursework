using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //to access Scene

public class PlayerMovement : MonoBehaviour
{
    /* Removed player maximumSpeed because I'm now using the animation speed on Blend Tree
     * Doing so not only allows the character to move around the map faster as in previous implementation
     * BUT MORE IMPORTANTLY makes the character "appear" to run faster in terms of animation
     */
    [SerializeField]
    float rotationSpeed, jumpSpeed, jumpButtonGracePeriod; 
    
    [SerializeField]
    Transform cameraTransform;

    public int currentHealth, maxHealth = 100; //make it public instead of SerializeField because I'm accessing it in another script

    public HealthBar healthBar;

    public float speedMultiplier;

    private CharacterController characterController;
    public Animator animator;

    private float ySpeed, originalStepOffset;
    private float? lastGroundedTime, jumpButtonPressedTime; //nullible field

    [HideInInspector]
    public Scene currentScene;


    private void Awake()
    {
        //hide and lock cursor at the centre when in-game
        //pressing "Esc" will unlock the cursor again
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Start is called before the first frame update
    void Start()
    {
        speedMultiplier = 2;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;

        //set health status at start of the game
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth);

        //get current scene in case it needs to be reloaded at some point if the player fails to beat a level
        currentScene = SceneManager.GetActiveScene();
        Debug.Log("Current active scene/level is: " + currentScene.name);

    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
        {
            //ADVANCED GAMEPLAY PROGRESSION TECHNIQUES: Reload current scene if player health goes down to 0
            SceneManager.LoadScene(currentScene.name);
            Debug.Log("Game Over: You've been incapacitated! Reloading current scene/level: " + currentScene.name);
        }


        //get player inputs based on WASD Unity Default mapping
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);

        //use magnitude to reflect movement speed behaviour -- can be useful IF player decides to use thumbsticks for movement controls
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        if(Input.GetKey(KeyCode.LeftShift)  || Input.GetKey(KeyCode.RightShift))
        {
            inputMagnitude /= 2; //reduce character speed by half -- from running speed to walking speed
        }
        animator.SetFloat("Input Magnitude", inputMagnitude, 0.05f, Time.deltaTime); //adjust animations according to input magnitude, also add 0.05f damp time so changing animations dont snap too quickly

        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        ySpeed += Physics.gravity.y * Time.deltaTime; //adjusting for gravity over time, -9.81m/s

        if(characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
        }
        if(Input.GetButtonDown("Jump")) //mapped to the space button by default
        {
            jumpButtonPressedTime = Time.time;
        }
        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.3f; //prevent character from dropping off suddenly when going down a platform
            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = jumpSpeed;

                //set to null so character can't jump repeatedly while in grace period
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
        }
        else
        {
            characterController.stepOffset = 0; //to prevent jerkiness when jumping into a rigidbody WHILST the character is on top of a platform
        }

        if (movementDirection != Vector3.zero) //check if the character is moving
        {
            animator.SetBool("IsMoving", true);

            //rotate character in direction it's moving towards
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    private void OnAnimatorMove()
    {
        /* The position change according to animation, no need to calculate speed and even adjust
         * for delta time (previously movementDirection * speed).
         * I multiplied by speedMultiplier of 2 so it moves further along the map
         * compared to relying purely on distance covered by the running animation itself.
         */
        Vector3 velocity = animator.deltaPosition * speedMultiplier;

        //adjust velocity to prevent character from bouncing it's way down a slope
        velocity = AdjustVelocityToSlope(velocity); 
        velocity.y += ySpeed * Time.deltaTime;

        //character controller handles collisions
        //also ensures consistent player movement regardless of frame rate similar to Time.deltaTime
        characterController.Move(velocity);
    }

    private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        //method to smoothly move a player across a downward slope
        var ray = new Ray(transform.position, Vector3.down);

        if(Physics.Raycast(ray, out RaycastHit hitInfo, 0.2f)) //max raycast distance of 0.2f so that it only picks up collisions close to the character i.e. THE GROUND/PLANE
        {
            //if there is a collision, rotate towards direction the surface is facing
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustedVelocity = slopeRotation * velocity;

            if(adjustedVelocity.y < 0)
            {
                //only return adjusted velocity when the character is moving down a slope --- to stop it from bouncing downwards instead of just gliding downwards
                return adjustedVelocity;
            }
        }
        return velocity;
    }

    public void TakeDamage(int damageAmount) //public so it can be accessible by other classes like the Titan Enemy AI/Spike Obstacles
    {
        //deduct the player's current health depending on the damageAmount assigned within the other functions/scripts whenever TakeDamage() function is called
        currentHealth -= damageAmount;
        healthBar.SetHealth(currentHealth);
    }

    public void TakeDamageOverTime(int damageAmount, int duration)
    {
        StartCoroutine(DamageOverTimeCoroutine(damageAmount, duration));
    }

    IEnumerator DamageOverTimeCoroutine(int damageAmount, int duration)
    {
        int amountDamaged = 0;
        int damagePerLoop = damageAmount / duration;

        while(amountDamaged < damageAmount)
        {
            TakeDamage(damagePerLoop);
            Debug.Log("Current Health is:" + currentHealth);
            amountDamaged += damagePerLoop;
            yield return new WaitForSeconds(1f); //the players gets damaged every second depending on how long their collision stays with the spikeballs

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy Titan"))
        {
            /* Player receives 20 damage on initial collision with the Enemy Titan AI; i.e. The Titan charging at them
             * Player also gets damaged when Titan attacks the player, given that player is within attack range AND collision range.
             */
            TakeDamage(20);
            Debug.Log($"You've been hit by the {collision.gameObject.tag} , you are taking 20 damage!");

            //AUDIO: Play accompanying audio when player takes damage
            FindObjectOfType<AudioManager>().Play("Blunt Damage");
        }
    }
}
