using System;
using UnityEngine;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    public class Oscillator : MonoBehaviour
    {

        public Vector3 MovementVector = new Vector3(0, 9, 0);
        public float period = 2f;

        private float movementFactor;
        private Vector3 startingPosition;

        // Use this for initialization
        void Start ()
        {
            startingPosition = transform.position;
            
        }
	
        // Update is called once per frame
        void Update ()
        {
            if (period > Mathf.Epsilon)
            {
                var cycles = Time.time / period; // grows continally from 0

                const float tau = Mathf.PI * 2; // about 6.28...
                float rawSinWave = Mathf.Sin(cycles * tau);

                movementFactor = rawSinWave / 2f + 0.5f;
                var offset = MovementVector * movementFactor;
                transform.position = startingPosition + offset;
            }
        }
    }
}
