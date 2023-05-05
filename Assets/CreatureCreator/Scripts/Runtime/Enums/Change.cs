namespace DanielLochner.Assets.CreatureCreator
{
    public enum Change
    {
        Load,

        DragBody,
        DragBodyBone,
        DragBodyArrowFront,
        DragBodyArrowBack,
        ScrollBodyBoneUp,
        ScrollBodyBoneDown,

        SetTiling,
        SetOffset,
        SetShine,
        SetMetallic,
        SetPattern,
        SetBodyPrimaryColor,
        SetBodySecondaryColor,
        AttachBodyPart,
        DetachBodyPart,

        DragBodyPart,
        StretchBodyPart,

        ScaleBodyPartDown,
        ScrollBodyPartUp,
        NudgeBodyPart,

        ScrollLimbBoneUp,
        ScrollLimbBoneDown,
        DragLimbBone,

        RotateBodyPart,
        PivotBodyPart,
    }
}