using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public interface IConstructible
    {
        CreatureConstructor Constructor { get; set; }
    }
}