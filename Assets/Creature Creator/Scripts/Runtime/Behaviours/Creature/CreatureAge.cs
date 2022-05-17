// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureAge : MonoBehaviour
    {
        #region Fields
        [SerializeField, ReadOnly] private int age = 0;
        #endregion

        #region Properties
        public Action<int> OnAgeChanged { get; set; }

        public int Age
        {
            get => age;
            set
            {
                age = Mathf.Max(0, value);
                OnAgeChanged?.Invoke(age);
            }
        }
        #endregion
    }
}