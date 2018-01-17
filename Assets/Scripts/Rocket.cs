﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
	public class Rocket : MonoBehaviour
	{
		public float ThrusterPower = 1;
		public float RotationSpeed = 1;
		public int Level = 0;
		public int NumberOfLevels = 0;
		public AudioClip MainEngine;
		public AudioClip Death;
		public AudioClip Success;

		private Rigidbody rigidBody;
		private AudioSource audioSource;

		enum State { Alive, Dying, Transending}
		private State state = State.Alive;

		// Use this for initialization
		void Start ()
		{
			rigidBody = GetComponent<Rigidbody>();
			audioSource = GetComponent<AudioSource>();
			NumberOfLevels = SceneManager.sceneCountInBuildSettings;
		}
	
		// Update is called once per frame
		void Update ()
		{
			ProcessInput();
		}

		void OnCollisionEnter(Collision collision)
		{
			if (state != State.Alive) return;

			switch (collision.gameObject.tag)
			{
				case "Friendly":
					// do nothing
					break;
                case "Finish":
                    StartSuccessSequence();
                    break;
                case "PowerUp":
					print("COLLECT");
					break;
                default:
                    StartDeathSequence();
                    break;
            }
		}

        private void StartSuccessSequence()
        {
            state = State.Transending;
            audioSource.Stop();
            audioSource.PlayOneShot(Success);
            Invoke("LoadNextScene", 3f);
        }

        private void StartDeathSequence()
        {
            state = State.Dying;
            audioSource.Stop();
            audioSource.PlayOneShot(Death);
            Invoke("GameOver", 3f);
        }

        private void ProcessInput()
		{
            if (state == State.Dying || state == State.Transending)
            {
                return;
            }
            RespondToThrustInput();
			RespondToRotateInput();
		}

		private void RespondToThrustInput()
		{
			if (Input.GetKey(KeyCode.Space))
			{
				ApplyThrust();
			}
			else
			{
				if (audioSource.isPlaying)
				{
					audioSource.Stop();
				}
			}
		}

		private void ApplyThrust()
		{
			rigidBody.AddRelativeForce(Vector3.up * ThrusterPower * Time.deltaTime);
			if (!audioSource.isPlaying)
			{
				audioSource.PlayOneShot(MainEngine);
			}
		}

		private void RespondToRotateInput()
		{
			rigidBody.freezeRotation = true;  // take manual control of rotation

			if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
			{
				// Do nothing
			}
			else if (Input.GetKey(KeyCode.A))
			{
				transform.Rotate(Vector3.forward * RotationSpeed * Time.deltaTime);
			}
			else if (Input.GetKey(KeyCode.D))
			{
				transform.Rotate(-Vector3.forward * RotationSpeed * Time.deltaTime);
			}

			rigidBody.freezeRotation = false;  // resume physics control of rotation 
		}

		private void LoadNextScene()
		{
			Level++;
			if (Level == NumberOfLevels)
			{
				Win();
			}
			else
			{
				LoadLevel(Level);
			}
		}

		private void GameOver()
		{
			Level = 0;
			LoadLevel(Level);
		}

		private void Win()
		{
			print("You win the game!");
		}

		private void LoadLevel(int number)
		{
			SceneManager.LoadScene(number);
		}
	}
}
