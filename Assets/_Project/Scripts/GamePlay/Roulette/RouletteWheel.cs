using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Event;
using _Project.Scripts.Core.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.GamePlay.Roulette
{
    public class RouletteWheel : MonoBehaviour
    {
        [SerializeField] private Transform wheelTransform;
        [SerializeField] private Transform ballTransform;

        [Header("Spinning")] [SerializeField] private float idleSpinSpeed = 30f;
        [SerializeField] private float gameSpinSpeed = 100f;
        [SerializeField] private float speedTransitionTime = 2f;
        [SerializeField] private float rotationSpeedRate = 6f;
        [SerializeField] private float ballStartSpeed = 380f;
        [SerializeField] private float ballEndSpeed = 100f;
        [SerializeField, Range(0.6f, 0.75f)] private float reducedSpinRadiusRatio = .7f;

        [Header("Animation Settings")] [SerializeField]
        private float minSpinDuration = 4f;
        [SerializeField] private float maxSpinDuration = 6f;
        [SerializeField] private float ballSpinRadius = 2f;
        [SerializeField, Range(5, 20)] private float alignmentToleranceAngle = 10f;

        [Header("Settle Settings")] [SerializeField,Min(1)]
        private int bounceCount = 6;
        [SerializeField,Min(1f)] private float settleTime = 1.2f;
        [SerializeField] private float bounceIntensityRate = .2f;
        [SerializeField,Min(.1f)] private float finalSettleBounceTime = .4f;
        [Space] [SerializeField] private List<Transform> numberPoints;

        private bool _isGameSpinning;
        private float _currentSpinSpeed;
        private Coroutine _speedTransitionCoroutine;
        private int _targetNumber;

        private void Start()
        {
            _currentSpinSpeed = idleSpinSpeed;
        }

        private void Update()
        {
            if (!wheelTransform) return;
            var rotationSpeed = _currentSpinSpeed * rotationSpeedRate;
            wheelTransform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }

        private void SetWheelSpeed(float targetSpeed)
        {
            if (_speedTransitionCoroutine != null)
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

        private void StartGameSpin()
        {
            SetWheelSpeed(gameSpinSpeed);
        }

        private void ReturnToIdleSpin()
        {
            SetWheelSpeed(idleSpinSpeed);
            // spin sound
        }

        public void SpinToNumber(int targetNumber)
        {
            //if (_isGameSpinning) return;
            _targetNumber = targetNumber;
            StartCoroutine(SpinAnimation(targetNumber));
        }

        private IEnumerator SpinAnimation(int targetNumber)
        {
            //_isGameSpinning = true;
            ballTransform.SetParent(null);
            StartGameSpin();

            var spinDuration = Random.Range(minSpinDuration, maxSpinDuration);
            AudioManager.Instance.PlaySoundWithFade(SoundType.Spin, spinDuration);
            yield return StartCoroutine(SpinBall(targetNumber, spinDuration));
            ReturnToIdleSpin();
            yield return StartCoroutine(BallSettleAnimation());

            //ballTransform.SetParent(wheelTransform);
            // yield return Extension.GetWaitForSeconds(1f);

            _isGameSpinning = false;
           GameEventManager.Instance.RouletteEvents.RaiseSpinComplete(targetNumber);
        }

        private IEnumerator SpinBall(int targetNumber, float duration)
        {
            if (ballTransform == null) yield break;

            var elapsed = 0f;
            var centerPosition = transform.position;

            float ballDirection = 1f; // Always counter-clockwise

            float ballAngle = 0f; // Track cumulative ball angle

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / duration;

                // Gradually slow down the ball with realistic physics
                float decelerationCurve = 1f - (progress * progress); // Cubic deceleration
                float currentBallSpeed = Mathf.Lerp(ballEndSpeed, ballStartSpeed, decelerationCurve);

                // Update ball angle based on current speed
                ballAngle += currentBallSpeed * ballDirection * Time.deltaTime;

                // Ball radius decreases as it slows down (spiral effect)
                float radiusProgress = Mathf.Pow(progress, 0.2f); // Slower radius change
                float currentRadius = Mathf.Lerp(ballSpinRadius, ballSpinRadius * reducedSpinRadiusRatio, radiusProgress);

                // Add realistic vertical bounce and wobble
                float bounceFrequency = Mathf.Lerp(25f, 8f, progress); // Slower bounces as ball slows
                float bounceAmplitude = Mathf.Lerp(0.025f, 0.008f, progress);
                float bounceHeight = 1.12f + Mathf.Sin(Time.time * bounceFrequency) * bounceAmplitude;

                // Add slight random wobble for realism
                float wobbleX = Mathf.PerlinNoise(Time.time * 8f, 0f) * 0.03f * (1f - progress);
                float wobbleZ = Mathf.PerlinNoise(Time.time * 8f, 100f) * 0.03f * (1f - progress);

                Vector3 ballPosition = centerPosition + new Vector3(
                    Mathf.Cos(ballAngle * Mathf.Deg2Rad) * currentRadius + wobbleX,
                    bounceHeight,
                    Mathf.Sin(ballAngle * Mathf.Deg2Rad) * currentRadius + wobbleZ
                );

                ballTransform.position = ballPosition;
                yield return null;
            }

            var currentHeight = ballTransform.position.y;

            while (!IsBallAlignedWithTargetSlot(targetNumber))
            {
                ballAngle += ballEndSpeed * ballDirection * Time.deltaTime;
                Vector3 ballPosition = centerPosition + new Vector3(
                    Mathf.Cos(ballAngle * Mathf.Deg2Rad) * ballSpinRadius * reducedSpinRadiusRatio,
                    currentHeight,
                    Mathf.Sin(ballAngle * Mathf.Deg2Rad) * ballSpinRadius * reducedSpinRadiusRatio
                );
                ballTransform.position = ballPosition;
                yield return null;
            }

            ballTransform.SetParent(numberPoints[targetNumber], true);
            print("Spinning finished");
        }

        private bool IsBallAlignedWithTargetSlot(int targetNumber)
        {
            var centerPosition = wheelTransform.position;
            var slotPos = numberPoints[targetNumber].position;
            var ballPos = ballTransform.position;

            var slotDir = new Vector3(slotPos.x - centerPosition.x, 0, slotPos.z - centerPosition.z).normalized;
            var ballDir = new Vector3(ballPos.x - centerPosition.x, 0, ballPos.z - centerPosition.z).normalized;

            var dot = Vector3.Dot(ballDir, slotDir);

            return dot >= Mathf.Cos(alignmentToleranceAngle * Mathf.Deg2Rad);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            if (!_isGameSpinning) return;
            var centerPosition = wheelTransform.position;
            centerPosition.y = 1f;
            var slotPos = numberPoints[_targetNumber].position;
            var ballPos = ballTransform.position;

            var slotDir = new Vector3(slotPos.x - centerPosition.x, 0, slotPos.z - centerPosition.z).normalized;
            var ballDir = new Vector3(ballPos.x - centerPosition.x, 0, ballPos.z - centerPosition.z).normalized;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(centerPosition, centerPosition + ballDir * ballSpinRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(centerPosition, centerPosition + slotDir * ballSpinRadius);
        }
        
        private IEnumerator BallSettleAnimation()
        {
            if (ballTransform == null) yield break;

            AudioManager.Instance.PlaySound(SoundType.BallDrop);
            var finalPosition = Vector3.zero;
            var startPosition = ballTransform.localPosition;

            for (float t = 0f; t < settleTime; t += Time.deltaTime)
            {
                float progress = t / settleTime;
                
                var bounceIntensity = (1f - progress) * bounceIntensityRate;
                var bounce = Mathf.Abs(Mathf.Sin(progress * bounceCount * Mathf.PI)) * bounceIntensity;

                // var chaosFreq = 15f;
                // var chaosIntensity = (1f - progress) * .04f;
                // var lateralChaos = Mathf.Sin(progress * chaosFreq) * chaosIntensity;

                var currentPos = Vector3.Lerp(startPosition, finalPosition, progress);
                currentPos.y += bounce;

                //var perpendicular = new Vector3(-finalPosition.z, 0, finalPosition.x).normalized;
                //currentPos += perpendicular * lateralChaos;

                ballTransform.localPosition = currentPos;
                yield return null;
            }

            yield return null;
            ballTransform.localPosition = finalPosition;
            yield return StartCoroutine(FinalSettleBounce(finalPosition));
        }

        private IEnumerator FinalSettleBounce(Vector3 finalPosition)
        {
            for (float t = 0; t < finalSettleBounceTime; t += Time.deltaTime)
            {
                var progress = t / finalSettleBounceTime;
                var bounce = Mathf.Sin(progress * Mathf.PI / 2f) * 0.015f * (1f - progress);

                var pos = finalPosition;
                pos.y += bounce;
                ballTransform.localPosition = pos;
                yield return null;
            }
        }
    }
}