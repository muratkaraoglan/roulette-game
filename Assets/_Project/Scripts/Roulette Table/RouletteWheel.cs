using System;
using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Roulette_Table
{
    public class RouletteWheel : MonoBehaviour
    {
        [SerializeField] private Transform wheelTransform;

        [Header("Spinning")] [SerializeField] private float idleSpinSpeed = 30f;
        [SerializeField] private float gameSpinSpeed = 100f;
        [SerializeField] private float speedTransitionTime = 2f;

        [Header("Animation Settings")] [SerializeField]
        private float minSpinDuration = 4f;
        [SerializeField] private float maxSpinDuration = 6f;
        [SerializeField] private float ballSpinRadius = 2f;
        
        private bool _isGameSpinning = false;
        private float _currentSpinSpeed;
        private Coroutine _speedTransitionCoroutine;
        
        private void Start()
        {
            _currentSpinSpeed = idleSpinSpeed;
        }

        private void Update()
        {
            if (!wheelTransform) return;
            var rotationSpeed = _currentSpinSpeed * 6f;
            wheelTransform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SetWheelSpeed(gameSpinSpeed);
            }
        }

        private void SetWheelSpeed(float targetSpeed)
        {
            if(_speedTransitionCoroutine != null)
                StopCoroutine(_speedTransitionCoroutine);
            
            _speedTransitionCoroutine = StartCoroutine(SpeedTransition(targetSpeed));
        }

        IEnumerator SpeedTransition(float targetSpeed)
        {
            var startSpeed = _currentSpinSpeed;
            var elapsedTime = 0f;
            
            while (elapsedTime < speedTransitionTime)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / speedTransitionTime;
                _currentSpinSpeed = Mathf.Lerp(startSpeed, targetSpeed, progress);
                yield return null;
            }
            _currentSpinSpeed = targetSpeed;
        }
    }
}