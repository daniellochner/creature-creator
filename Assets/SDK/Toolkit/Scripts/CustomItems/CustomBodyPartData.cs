using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Creature Creator SDK/Custom/Body Part")]
public class CustomBodyPartData : ScriptableObject
{
    public AbilityType[] abilities;
    [Range(1, 5)] public int appeal = 1;
    public bool isLightSource;
    public BodyPartDefaultColours defaultColours;



    [System.Flags]
    public enum Transformation
    {
        Pivot = 1 << 0,
        PivotXY = 1 << 1,
        Rotate = 1 << 2,
        Scale = 1 << 3,
        StretchX = 1 << 4,
        StretchY = 1 << 5,
        StretchZ = 1 << 6
    }

    [Serializable]
    public class BodyPartDefaultColours
    {
        public Color primary;
        public Color secondary;
    }

    public enum AbilityType
    {
        Bite,
        Command,
        Dance,
        Drop,
        Eat,
        Emit,
        Flap,
        Growl,
        Hear,
        Hold,
        Jump,
        Launch,
        Luminesce,
        NightVision,
        Ping,
        See,
        Smell,
        Spin,
        Sprint,
        Strike,
        Swim,
        Walk
    }
}
