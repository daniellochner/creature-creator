using System;
using System.Collections;
using System.Collections.Generic;

public class IronSourceUtils
    {
    private const string ERROR_CODE = "error_code";
    private const string ERROR_DESCRIPTION = "error_description";
    private const string INSTANCE_ID_KEY = "instanceId";
    private const string PLACEMENT_KEY = "placement";

    public static IronSourceError getErrorFromErrorObject(object descriptionObject)
    {
        Dictionary<string, object> error = null;
        if (descriptionObject is IDictionary)
        {
            error = descriptionObject as Dictionary<string, object>;
        }
        else if (descriptionObject is String && !String.IsNullOrEmpty(descriptionObject.ToString()))
        {
            error = IronSourceJSON.Json.Deserialize(descriptionObject.ToString()) as Dictionary<string, object>;
        }

        IronSourceError sse = new IronSourceError(-1, "");
        if (error != null && error.Count > 0)
        {
            int eCode = Convert.ToInt32(error[ERROR_CODE].ToString());
            string eDescription = error[ERROR_DESCRIPTION].ToString();
            sse = new IronSourceError(eCode, eDescription);
        }

        return sse;
    }

    public static IronSourcePlacement getPlacementFromObject(object placementObject)
    {
        Dictionary<string, object> placementJSON = null;
        if (placementObject is IDictionary)
        {
            placementJSON = placementObject as Dictionary<string, object>;
        }
        else if (placementObject is String)
        {
            placementJSON = IronSourceJSON.Json.Deserialize(placementObject.ToString()) as Dictionary<string, object>;
        }

        IronSourcePlacement ssp = null;
        if (placementJSON != null && placementJSON.Count > 0)
        {
            int rewardAmount = Convert.ToInt32(placementJSON["placement_reward_amount"].ToString());
            string rewardName = placementJSON["placement_reward_name"].ToString();
            string placementName = placementJSON["placement_name"].ToString();

            ssp = new IronSourcePlacement(placementName, rewardName, rewardAmount);
        }

        return ssp;
    }
}
