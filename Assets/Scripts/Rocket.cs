using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
	public class Rocket : MonoBehaviour
	{
		public float ThrusterPower = 1;
		public float RotationSpeed = 1;
		public int Level = 0;
		public int NumberOfLevels = 0;

		private Rigidbody rigidBody;
		private AudioSource thrusterSound;

		enum State { Alive, Dying, Transending}
		private State state = State.Alive;

		// Use this for initialization
		void Start ()
		{
			rigidBody = GetComponent<Rigidbody>();
			thrusterSound = GetComponent<AudioSource>();
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
					state = State.Transending;
					Invoke("LoadNextScene", 3f);
					break;
				case "PowerUp":
					print("COLLECT");
					break;
				default:
					state = State.Dying;
					Invoke("GameOver", 3f);
					break;
			}
		}

		private void ProcessInput()
		{
			if (state == State.Dying || state == State.Transending)
			{
				thrusterSound.Stop();
				return;
			}
			Thrust();
			Rotate();
		}

		private void Thrust()
		{
			if (Input.GetKey(KeyCode.Space))
			{
				rigidBody.AddRelativeForce(Vector3.up * ThrusterPower * Time.deltaTime);
				if (!thrusterSound.isPlaying)
				{
					thrusterSound.Play();
				}
			}
			else
			{
				if (thrusterSound.isPlaying)
				{
					thrusterSound.Stop();
				}
			}
		}

		private void Rotate()
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
