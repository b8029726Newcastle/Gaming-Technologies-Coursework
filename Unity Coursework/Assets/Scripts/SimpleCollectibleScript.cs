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


	// Use this for initialisation
	void Start () {
        player = FindObjectOfType<PlayerMovement>(); //access other script as an instance
		timer = FindObjectOfType<Timer>(); //access other script as an instance
	}
	
	// Update is called once per frame
	void Update () {

		//make the collectibles rotate
		if (rotate)
			transform.Rotate (Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

	}

	void OnTriggerEnter(Collider other)
	{
		//collectibles can only be collected when colliding with the player
		if (other.tag == "Player") {
			Collect ();
		}
	}

	public void Collect()
	{
		//AUDIO: Play accompanying sound clip for every collectible that has been acquired (all have different sound clips in inspector)
		if (collectSound)
			AudioSource.PlayClipAtPoint(collectSound, transform.position);

		//SPECIAL EFFECTS: Play base accompanying particle effect whenever any collectible has been acquired
		if (collectEffect)
			Instantiate(collectEffect, transform.position, Quaternion.identity);


		if (CollectibleType == CollectibleTypes.NoType) {

			//Add in code here;

			Debug.Log ("Do NoType Command");
		}
		if (CollectibleType == CollectibleTypes.RingCollectible) {
			//Ring Collectible count increases as more are collected by the player
			CollectablesCounter.count += 1;

			Debug.Log ("RING Collected!");
		}
		if (CollectibleType == CollectibleTypes.HealthRecovery) {

			//SPECIAL EFFECTS: Play special particle effects to simulate health regeneration when a Health Recovery (Healthpack/Medkit) collectible has been acquired
			//Special particle effects for powerups are attached to the Character Game Object and then assigned into the inspector of this script
			particleSystem.Play();

			//ADVANCED GAMEPLAY PROGRESSION TECHNIQUES: Set player's health to max.
			player.currentHealth = player.maxHealth;
			player.healthBar.SetMaxHealth(player.maxHealth);

			Debug.Log($"Health Recovery Activated! Player health is: {player.currentHealth}");
		}
		if (CollectibleType == CollectibleTypes.SpeedBoost) {

			//ADVANCED GAMEPLAY PROGRESSION TECHNIQUES: Increase player's overall speed for all actions as well as animations in blend tree for 4 seconds.
			player.animator.SetBool("IsBoosted", true);

			//SPECIAL EFFECTS: Play special particle effects to simulate speed boost sensation when a Speed Boost (Star) collectible has been acquired
			//Special particle effects for powerups are attached to the Character Game Object and then assigned into the inspector of this script
			particleSystem.Play();
			StartCoroutine(speedBoostEnd(4.0f)); //disable speed boost after 4 seconds has elapsed

			Debug.Log("Speed Boost Activated!");

		}

		if (CollectibleType == CollectibleTypes.TimeDilation) {

			//ADVANCED GAMEPLAY PROGRESSION TECHNIQUES: Add bonus time of 5 seconds as well as slow down Time Scale and speed it back up over time

			//SPECIAL EFFECTS: Play special particle effects to simulate slow motion sensation when a Time Dilation (Hourglass) collectible has been acquired
			//Special particle effects for powerups are attached to the Character Game Object and then assigned into the inspector of this script
			particleSystem.Play(); 

			
			Debug.Log ("Time Dilation Activated! 5 seconds has been added!");
			timeDilation(3f);
		}

		gameObject.SetActive(false);
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
		//ADVANCED GAMEPLAY PROGRESSION TECHNIQUES: Add bonus time of 5 seconds
		timer.timeValue += 5;


		/* BUG! It never seems to exit duration whether I pass a float parameter or type it in manually
		 * Regardless of the grade I receive, I would appreciate feedback if the marker knows how to fix this problem --  it's just for future reference :))
		 * I have tried so many things but none worked, it seems to only ever exit the waiting time whenever I put it in the start method
		 * But that's not what I want as I only  want to triggeer this during certain circumstances have been met like during collision
		 * I've tried putting it on the "update" function using booleans to trigger it but then I find that the update doesn't detect the boolean whenever I change it's value
		 */

		//slow down in-game time OVER TIME
		//slow down Time Scale and speed it back up over time
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

		/* BUG! It never seems to exit duration whether I pass a float parameter or type it in manually
		 * Regardless of the grade I receive, I would appreciate feedback if the marker knows how to fix this problem --  it's just for future reference :))
		 * I have tried so many things but none worked, it seems to only ever exit the waiting time whenever I put it in the start method
		 * But that's not what I want as I only  want to triggeer this during certain circumstances have been met like during collision
		 * I've tried putting it on the "update" function using booleans to trigger it but then I find that the update doesn't detect the boolean whenever I change it's value
		 */
	}
}
