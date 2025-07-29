using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Core.Helper;
using UnityEngine;

namespace _Project.Scripts.GamePlay.ChipSystem
{
    public sealed class ChipManager : Singleton<ChipManager>
    {
        [SerializeField] private ChipEnum selectedChip;
        [SerializeField] private Transform chipHighlightTransform;
        [SerializeField] private List<ChipHighlightPoints> chipHighlightPoints;

        [SerializeField] private List<ChipPrefabHolder> chips;

        private Dictionary<ChipEnum, GameObject> _chipPrefabMap;
        private Dictionary<ChipEnum, Vector3> _highlightPointsMap;

        protected override void Awake()
        {
            Configure(config =>
            {
                config.Lazy = true;
                config.DestroyOthers = true;
            });
            base.Awake();
            _chipPrefabMap = new Dictionary<ChipEnum, GameObject>();
            _chipPrefabMap = chips.ToDictionary(c => c.chipType, c => c.chipPrefab);
            _highlightPointsMap = new Dictionary<ChipEnum, Vector3>();
            _highlightPointsMap = chipHighlightPoints.ToDictionary(p => p.chipType, p =>
            {
                var point = p.point.position;
                point.y = .6f;
                return point;
            });
            ChangeSelectedChip(selectedChip);
        }

        public void ChangeSelectedChip(ChipEnum chip)
        {
            selectedChip = chip;
            chipHighlightTransform.position = _highlightPointsMap[selectedChip];
        }

        public void InstantiateChip(ChipEnum chip, Vector3 position)
        {
            var chipPrefab = _chipPrefabMap[chip];
            Instantiate(_chipPrefabMap[chip], position, chipPrefab.transform.rotation);
        }

        public ChipEnum GetSelectedChip() => selectedChip;
    }

    [Serializable]
    public sealed class ChipPrefabHolder
    {
        public ChipEnum chipType;
        public GameObject chipPrefab;
    }

    [Serializable]
    public sealed class ChipHighlightPoints
    {
        public ChipEnum chipType;
        public Transform point;
    }
}