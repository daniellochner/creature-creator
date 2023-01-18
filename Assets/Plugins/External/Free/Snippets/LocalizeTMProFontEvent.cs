using UnityEngine;
using UnityEngine.Events;
using System;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;

/// <summary>
/// UnityEvent which can pass a Texture as an argument.
/// </summary>
[Serializable]
public class UnityEventFont : UnityEvent<TMP_FontAsset> { }

/// <summary>
/// Component that can be used to Localize a Texture asset.
/// Provides an update event that can be used to automatically update the texture whenever the Locale changes.
/// </summary>
[AddComponentMenu("Localization/Asset/Localize TMPro Font Event")]
public class LocalizeTMProFontEvent : LocalizedAssetEvent<TMP_FontAsset, LocalizeTMProFont, UnityEventFont>
{}