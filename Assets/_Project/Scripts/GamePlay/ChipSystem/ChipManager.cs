using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Core.Event;
using _Project.Scripts.Core.Helper;
using _Project.Scripts.Core.Pooling;
using UnityEngine;

namespace _Project.Scripts.GamePlay.ChipSystem
{
    public sealed class ChipManager : Singleton<ChipManager>
    {
        [SerializeField] private ChipEnum selectedChip;
        [SerializeField] private Transform chipHighlightTransform;
        [SerializeField] private List<ChipHighlightPoints> chipHighlightPoints;
        [SerializeField] private List<ChipPoolHolder> chipPools;

        private Dictionary<ChipEnum, PoolBase> _chipPoolMap;
        private Dictionary<ChipEnum, Vector3> _highlightPointsMap;
        private Stack<UsedChipHolder> _usedChips;

        protected override void Awake()
        {
            Configure(config =>
            {
                config.Lazy = true;
                config.DestroyOthers = true;
            });
            base.Awake();
            _chipPoolMap = new Dictionary<ChipEnum, PoolBase>();
            _chipPoolMap = chipPools.ToDictionary(c => c.chipType, c => c.pool);
            _highlightPointsMap = new Dictionary<ChipEnum, Vector3>();
            _highlightPointsMap = chipHighlightPoints.ToDictionary(p => p.chipType, p =>
            {
                var point = p.point.position;
                point.y = .6f;
                return point;
            });
            _usedChips = new Stack<UsedChipHolder>();
            chipHighlightPoints.Clear();
            chipPools.Clear();
            ChangeSelectedChip(selectedChip);
        }

        private void OnEnable()
        {
            GameEventManager.Instance.RouletteEvents.OnSpinComplete += OnSpinComplete;
        }

        private void OnDisable()
        {
            GameEventManager.Instance.RouletteEvents.OnSpinComplete -= OnSpinComplete;
        }

        private void OnSpinComplete(int targetNumber)
        {
            while (_usedChips.Count > 0)
            {
                var usedChipHolder = _usedChips.Pop();
                var pool = _chipPoolMap[usedChipHolder.ChipType];
                pool.Pool.Release(usedChipHolder.ChipObject);
            }
        }

        public void ChangeSelectedChip(ChipEnum chip)
        {
            selectedChip = chip;
            chipHighlightTransform.position = _highlightPointsMap[selectedChip];
        }

        public void InstantiateChip(ChipEnum chipType, Vector3 position)
        {
            var pool = _chipPoolMap[chipType];
            var chip = pool.Pool.Get();
            chip.transform.position = position;
            _usedChips.Push(new UsedChipHolder() { ChipType = chipType, ChipObject = chip });
        }

        public ChipEnum GetSelectedChip() => selectedChip;
    }

    [Serializable]
    public sealed class ChipPoolHolder
    {
        public ChipEnum chipType;
        public PoolBase pool;
    }

    [Serializable]
    public sealed class ChipHighlightPoints
    {
        public ChipEnum chipType;
        public Transform point;
    }

    public sealed class UsedChipHolder
    {
        public ChipEnum ChipType;
        public GameObject ChipObject;
    }
}