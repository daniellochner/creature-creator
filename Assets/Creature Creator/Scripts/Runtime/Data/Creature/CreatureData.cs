// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class CreatureData
    {
        #region Fields
        [SerializeField] private string name;
        [SerializeField] private List<Bone> bones = new List<Bone>();
        [SerializeReference] private List<AttachedBodyPart> attachedBodyParts = new List<AttachedBodyPart>();
        [Space]
        [SerializeField] private string patternID = "";
        [SerializeField] private Color primaryColour = Color.white;
        [SerializeField] private Color secondaryColour = Color.black;
        #endregion

        #region Properties
        public string Name
        {
            get => name;
            set => name = value;
        }
        public List<Bone> Bones
        {
            get => bones;
        }
        public List<AttachedBodyPart> AttachedBodyParts
        {
            get => attachedBodyParts;
        }
        public string PatternID
        {
            get => patternID;
            set => patternID = value;
        }
        public Color PrimaryColour
        {
            get => primaryColour;
            set => primaryColour = value;
        }
        public Color SecondaryColour
        {
            get => secondaryColour;
            set => secondaryColour = value;
        }
        #endregion
    }
}