
using UnityEngine;

public class MapInfo : BoundedBehaviour
{
	public override Color BoundsColor => Color.clear;
	public override Color BoundsOutlineColor => Color.green;
	public Texture2D MiniMapTexture;

	[HideInInspector]
	public PlatformProxy[] PlatformProxies;
}