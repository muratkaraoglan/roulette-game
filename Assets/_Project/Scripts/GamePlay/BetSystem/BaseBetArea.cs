
using System;
using _Project.Scripts.Core.Event;
using UnityEngine;

namespace _Project.Scripts.GamePlay.BetSystem
{
    public abstract class BaseBetArea : MonoBehaviour
    {
        [SerializeField] protected BetRuleSO betRule;
    }
}