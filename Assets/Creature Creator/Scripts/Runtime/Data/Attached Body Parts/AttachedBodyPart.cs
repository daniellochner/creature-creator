// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class AttachedBodyPart
    {
        public string GUID;
        public string bodyPartID;
        public int boneIndex = -1;
        public SerializableTransform serializableTransform;
        public Vector3 stretch;
        public Color primaryColour = Color.white;
        public Color secondaryColour = Color.black;

        public AttachedBodyPart(string bodyPartID)
        {
            this.bodyPartID = bodyPartID;

            GUID = Guid.NewGuid().ToString();
        }
    }
}