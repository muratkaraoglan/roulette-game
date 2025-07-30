using _Project.Scripts.Core.Interact;
using UnityEngine;

namespace _Project.Scripts.GamePlay.ChipSystem
{
    public class Chip : MonoBehaviour, IInteractable
    {
        [SerializeField] private ChipEnum chip;

        public void OnSelect()
        {
            print("selected chip:" + chip.ToString());
            ChipManager.Instance.ChangeSelectedChip(chip);
        }
        //TODO: wrong usage 
        public void OnDeselect()
        {
        }
    }
}