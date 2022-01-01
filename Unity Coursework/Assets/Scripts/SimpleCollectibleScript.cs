using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //to access component "Text"
using System.Threading; //to access Threading
using System.Threading.Tasks; //to access Task.Delay

[RequireComponent(typeof(AudioSource))]
public class SimpleCollectibleScript : MonoBehaviour {

	PlayerMovement player;
	Timer timer;

	public enum CollectibleTypes {NoType, RingCollectible, HealthRecovery, SpeedBoost, TimeDilation}; 

	public CollectibleTypes CollectibleType; // this gameObject's type

	public bool rotate;

	public float rotationSpeed;

	public AudioClip collectSound;

	public GameObject collectEffect;

	public float minimumTimeScale = 0.4f; 

	public bool durationTime = false;

	public ParticleSystem particleSystem;

	//public AudioSource collectSound; //for future implementation



	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerMovement>(); //access other script as an instance
		timer = FindObjectOfType<Timer>(); //access other script as an instance
	}
	
	// Update is called once per frame
	void Update () {

		if (rotate)
			transform.Rotate (Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			Collect ();
		}
	}

	public void Collect()
	{
		if(collectSound)
			AudioSource.PlayClipAtPoint(collectSound, transform.position);
		if(collectEffect)
			//special effects when a collectible has been acquired
			Instantiate(collectEffect, transform.position, Quaternion.identity);


		if (CollectibleType == CollectibleTypes.NoType) {

			//Add in code here;

			Debug.Log ("Do NoType Command");
		}
		if (CollectibleType == CollectibleTypes.RingCollectible) {

			//Add in code here;

			//collectSoundIAN.Play(); //cannot be null or it stops the game
			CollectablesCounter.count += 1;

			Debug.Log ("RING Collision!!");
		}
		if (CollectibleType == CollectibleTypes.HealthRecovery) {

			particleSystem.Play();

			//set player's health to max
			player.currentHealth = player.maxHealth;
			player.healthBar.SetMaxHealth(player.maxHealth);
		}
		if (CollectibleType == CollectibleTypes.SpeedBoost) {
			//increase speed for 3 seconds
			player.animator.SetBool("IsBoosted", true);
			particleSystem.Play();
			StartCoroutine(speedBoostEnd(3.0f)); //disable speed boost after 3 seconds has elapsed
			
		}

		if (CollectibleType == CollectibleTypes.TimeDilation) {

			//prob keep the same effect with speedBoost SO MAYBE just delete the TimeDilationEFFECT and REUSE WarpSpeedEFFECT
			//hopefully particles will slowdown during slow-mo
			particleSystem.Play(); 

			//Time.timeScale = 0.4f;
			Debug.Log ("TIME DILATION Collision!!");
			timeDilation(3f);
		}

		gameObject.SetActive(false);
		//Destroy (gameObject);
	}

	IEnumerator speedBoostEnd(float duration)
    {
		Debug.Log("Entered Waiting time");
		yield return new WaitForSeconds(duration);
		Debug.Log("Finished Waiting time!");
		player.animator.SetBool("IsBoosted", false);
	}

	void timeDilation(float duration)
    {
		//add bonus time
		timer.timeValue += 5;


		/*BUG! It never seems to exit duration whether I pass a float parameter or type it in manually
		 * Regardless of the grade I receive, I would appreciate feedback if the marker knows how to fix this problem --  it's just for future reference :))
		 * I have tried so many things but none worked, it seems to only ever exit the waiting time whenever I put it in the start method
		 * But that's not what I want as I only  want to triggeer this during certain circumstances have been met like during collision
		 * I've tried putting it on the "update" function using booleans to trigger it but then I find that the update doesn't detect the boolean whenever I change it's value
		*/
		//slow down in-game time OVER TIME
		while (Time.timeScale > minimumTimeScale)
        {
			Time.timeScale -= 0.05f;
		}
		Time.timeScale = 1f;

		StartCoroutine(timeDilationEnd(duration)); //end time dilation after 3 seconds
    }
	IEnumerator timeDilationEnd(float duration)
    {
		//reset Time Scale after duration has expired -- 3 seconds
		Debug.Log("Entered Time Dilation Waiting time");
		yield return new WaitForSeconds(duration);
		Debug.Log("Finished Waiting time!");
		Time.timeScale = 1f;

		/*BUG! It never seems to exit duration whether I pass a float parameter or type it in manually
		 * Regardless of the grade I receive, I would appreciate feedback if the marker knows how to fix this problem --  it's just for future reference :))
		 * I have tried so many things but none worked, it seems to only ever exit the waiting time whenever I put it in the start method
		 * But that's not what I want as I only  want to triggeer this during certain circumstances have been met like during collision
		 * I've tried putting it on the "update" function using booleans to trigger it but then I find that the update doesn't detect the boolean whenever I change it's value
		*/
	}
}
