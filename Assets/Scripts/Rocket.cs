using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
	public class Rocket : MonoBehaviour
	{
		public float ThrusterPower = 1f;
		public float RotationSpeed = 1f;
		public int Level = 0;
		public int NumberOfLevels = 0;
	    public float LevelLoadDelay = 3f;

        public AudioClip MainEngine;
		public AudioClip Death;
		public AudioClip Success;

	    public ParticleSystem MainEngineParticles;
	    public ParticleSystem DeathParticles;
	    public ParticleSystem SuccessParticles;

        private Rigidbody rigidBody;
		private AudioSource audioSource;

		enum State { Alive, Dying, Transending}
		private State state = State.Alive;

	    public bool collisionsAreDisabled = false;

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
		    if (collisionsAreDisabled) return;

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
            SuccessParticles.Play();
            Invoke("LoadNextScene", LevelLoadDelay);
        }

        private void StartDeathSequence()
        {
            state = State.Dying;
            audioSource.Stop();
            audioSource.PlayOneShot(Death);
            DeathParticles.Play();
            Invoke("GameOver", LevelLoadDelay);
        }

        private void ProcessInput()
		{
            if (state == State.Dying || state == State.Transending)
            {
                return;
            }
            RespondToThrustInput();
			RespondToRotateInput();

		    if (Debug.isDebugBuild)
		    {
		        RespondToDebugInput();
		    }
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
				    MainEngineParticles.Stop();
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
            MainEngineParticles.Play();
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
		    var levelIndex = SceneManager.GetActiveScene().buildIndex;
		    var nextLevelIndex = levelIndex + 1;

            print("Current level: " + levelIndex);
            print("Next level: " + nextLevelIndex);

            if (nextLevelIndex == NumberOfLevels)
			{
				//Win();
                LoadLevel(0);
			}
			else
			{
				LoadLevel(nextLevelIndex);
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

	    private void RespondToDebugInput()
	    {
	        if (Input.GetKey(KeyCode.L))
	        {
	            LoadNextScene();
	        }

	        if (Input.GetKey(KeyCode.C))
	        {
	            collisionsAreDisabled = !collisionsAreDisabled;
	        }
	    }
	}
}
