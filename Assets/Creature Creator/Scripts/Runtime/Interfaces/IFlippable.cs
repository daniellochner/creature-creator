// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator
{
    public interface IFlippable<T>
    {
        bool IsFlipped { get; set; }

        T Flipped { get; set; }
        void SetFlipped(T main);
    }
}