using System;

public class IronSourcePlacement
{
	private string rewardName;
	private int rewardAmount;
	private string placementName;

	public IronSourcePlacement (string placementName, string rewardName, int rewardAmount)
	{
		this.placementName = placementName;
		this.rewardName = rewardName;
		this.rewardAmount = rewardAmount;
	}

	public string getRewardName ()
	{
		return rewardName;
	}

	public int getRewardAmount ()
	{
		return rewardAmount;
	}

	public string getPlacementName ()
	{
		return placementName;
	}

	public override string ToString ()
	{
		return placementName + " : " + rewardName + " : " + rewardAmount;
	}


}
