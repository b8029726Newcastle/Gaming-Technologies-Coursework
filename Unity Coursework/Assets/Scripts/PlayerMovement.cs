using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed, jumpSpeed, jumpButtonGracePeriod; //removed  maximumSpeed because I'm now using the animation speed on Blend Tree

    [SerializeField]
    Transform cameraTransform;

    public int currentHealth, maxHealth = 100; //make it public instead of SerializeField because I'm accessing it in another script

    public HealthBar healthBar;

    public float speedMultiplier;

    private CharacterController characterController;
    public Animator animator;

    private float ySpeed, originalStepOffset;
    private float? lastGroundedTime, jumpButtonPressedTime; //nullible field

 

    // Start is called before the first frame update
    void Start()
    {
        speedMultiplier = 2;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(5);
        }


        //get player inputs based on WASD Unity Default mapping
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude); //use magnitude to reflect movement speed behaviour IF player decides to use thumbsticks

        if(Input.GetKey(KeyCode.LeftShift)  || Input.GetKey(KeyCode.RightShift))
        {
            inputMagnitude /= 2; //reduce character speed by half
        }
        animator.SetFloat("Input Magnitude", inputMagnitude, 0.05f, Time.deltaTime); //adjust animations according to input magnitude, also add 0.05f damp time so changing animations dont snap too quickly

        //float speed = inputMagnitude * maximumSpeed; //make sure speed stays between 0 and 1
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        ySpeed += Physics.gravity.y * Time.deltaTime; //adjusting for gravity over time, -9.81/s

        if(characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
        }
        if(Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }
        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.3f; //prevent character from dropping off suddenly when going down a platform
            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod) //mapped to the space button by default
            {
                ySpeed = jumpSpeed;

                //set to null so character can't jump repeatedly whiile in grace period
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
        }
        else
        {
            characterController.stepOffset = 0; //to prevent jerkiness when pressing jumping into a rigidbody WHILST the character is on top of a platform
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
        //move character relative to the world
        //transform.Translate(movementDirection * magnitude * speed * Time.deltaTime, Space.World); 

        //character controller handles collisions
        //also ensures consistent player movement regardless of frame rate similar to Time.deltaTime
        Vector3 velocity = animator.deltaPosition * speedMultiplier; //position change according to animation, no need to calculate speed and even adjust for delta time           //previously movementDirection * speed
        //I multiplied by speedMultiplier of 2 so it moves further along the map compared to relying purely on distance covered by the running animation itself


        velocity = AdjustVelocityToSlope(velocity); //adjust velocity to prevent character from bouncing it's way down a slope
        velocity.y += ySpeed * Time.deltaTime;
        characterController.Move(velocity); //multiply by deltaTime to ensure character/player movement is consistent regardless of frame rate
    }

    private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        //might not need this

        //method to smoothly move a player across a downward slope
        var ray = new Ray(transform.position, Vector3.down);

        if(Physics.Raycast(ray, out RaycastHit hitInfo, 0.2f)) //max raycast distance of 0.2f to so that it only picks up collisions close to the character i.e. THE GROUND/PLANE
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

    private void OnApplicationFocus(bool focus)
    {
        //hide and lock cursor at the centre when in-game
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

    }
    public void TakeDamage(int damageAmount) //public so it can be accessible by other classes like the AI/Obstacles
    {
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
            yield return new WaitForSeconds(1f);

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy Titan")) ;
        {
            Debug.Log($"Colliding with {gameObject.tag} , you are taking initial 20collision damage!");
            /*players receives 3 damage on initial collision
             * players take more damage if collision lingers
             * (i.e. Some obstacles are designed to be movable and a player accidentally pushing an obstacle may cause them damage over time!)*/
            TakeDamage(20);
        }
    }
}
