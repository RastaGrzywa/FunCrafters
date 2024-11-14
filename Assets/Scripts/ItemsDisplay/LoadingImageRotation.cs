using System;
using UnityEngine;

namespace ItemsDisplay
{
    public class LoadingImageRotation : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed;

        private void Update()
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }
}