using System;
using _Project.Scripts.Core.Interact;
using UnityEngine;

namespace _Project.Scripts.Core.Managers
{
    public class SelectionManager : MonoBehaviour
    {
        [SerializeField] private float dragThresholdSqr = 10f;
        [SerializeField] private LayerMask selectionLayerMask;
        [SerializeField] private Camera raycastCamera;
        private Vector2 _mouseDownPosition;
        private Plane _plane = new Plane(Vector3.up, Vector3.zero);
        private IInteractable _interactable;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mouseDownPosition = Input.mousePosition;
                _interactable = HandleBetArea();
            }

            if (Input.GetMouseButton(0))
            {
                Vector2 mousePosition = Input.mousePosition;
                var delta = (mousePosition - _mouseDownPosition).sqrMagnitude;

                if (delta >= dragThresholdSqr)
                {
                    _mouseDownPosition = mousePosition;
                    _interactable?.OnMouseUp();
                    _interactable = HandleBetArea();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _interactable?.OnMouseUp();
                _interactable = null;
            }
        }

        private IInteractable HandleBetArea()
        {
            var ray = raycastCamera.ScreenPointToRay(_mouseDownPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, selectionLayerMask))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    _interactable = interactable;
                    _interactable.OnMouseDown();
                    Debug.DrawLine(ray.origin, hit.point, Color.red,1f);
                    return interactable;
                }
            }

            return null;
        }
    }
}