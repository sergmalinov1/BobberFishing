using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Es.WaveformProvider.Sample
{
    public class BoldScript : MonoBehaviour
    {
        public Texture2D waveform;
        //public WaveConductor waveConduktor;

        [SerializeField, Range(0f, 1f)]
        private float inputScaleFitter = 0.01f;

        [SerializeField, Range(0f, 1f)]
        private float strength = 1f;

        private Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            //if (transform.hasChanged )
            //{
                //waveConduktor.Input(waveform, transform.position, inputScaleFitter, strength);
            //    transform.hasChanged = false;
           // }
        }
    }

}