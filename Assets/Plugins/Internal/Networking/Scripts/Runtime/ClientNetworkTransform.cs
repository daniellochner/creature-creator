using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Netcode.Components
{
    public class ClientNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}