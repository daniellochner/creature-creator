// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class CreatureData : INetworkSerializable
    {
        #region Fields
        [SerializeField] private string name;
        [SerializeField] private List<Bone> bones = new List<Bone>();
        [SerializeReference] private List<AttachedBodyPart> attachedBodyParts = new List<AttachedBodyPart>();
        [Space]
        [SerializeField] private string patternID = "";
        [SerializeField] private Vector2 scale = Vector2.one;
        [SerializeField] private Vector2 offset = new Vector2(0.25f, 0f);
        [SerializeField] private Color primaryColour = Color.white;
        [SerializeField] private Color secondaryColour = Color.black;
        [SerializeField] private float shine = 0f;
        [SerializeField] private float metallic = 0f;
        #endregion

        #region Properties
        public string Name
        {
            get => name;
            set => name = value;
        }
        public string PatternID
        {
            get => patternID;
            set => patternID = value;
        }
        public Vector2 Tiling
        {
            get => scale;
            set => scale = value;
        }
        public Vector2 Offset
        {
            get => offset;
            set => offset = value;
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
        public float Shine
        {
            get => shine;
            set => shine = value;
        }
        public float Metallic
        {
            get => metallic;
            set => metallic = value;
        }
        public List<Bone> Bones
        {
            get => bones;
        }
        public List<AttachedBodyPart> AttachedBodyParts
        {
            get => attachedBodyParts;
        }
        #endregion

        #region Methods
        public void Reset()
        {
            name = "";
            bones.Clear();
            attachedBodyParts.Clear();

            patternID = "";
            scale = Vector2.one;
            offset = new Vector2(0.25f, 0f);
            primaryColour = Color.white;
            secondaryColour = Color.black;
            shine = 0f;
            metallic = 0f;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref name);
            serializer.SerializeValue(ref patternID);
            serializer.SerializeValue(ref scale);
            serializer.SerializeValue(ref offset);
            serializer.SerializeValue(ref primaryColour);
            serializer.SerializeValue(ref secondaryColour);
            serializer.SerializeValue(ref shine);
            serializer.SerializeValue(ref metallic);

            Bone[] b = null;
            AttachedBodyPart[] a = null;

            if (serializer.IsWriter)
            {
                b = bones.ToArray();
                a = attachedBodyParts.ToArray();
            }
            //
            serializer.SerializeValue(ref b);
            serializer.SerializeValue(ref a);

            if (serializer.IsReader)
            {
                bones = new List<Bone>(b);
                attachedBodyParts = new List<AttachedBodyPart>(a);
            }
        }
        #endregion
    }
}