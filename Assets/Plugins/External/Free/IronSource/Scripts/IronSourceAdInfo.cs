using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class IronSourceAdInfo
{

    public readonly string auctionId;
    public readonly string adUnit;
    public readonly string country;
    public readonly string ab;
    public readonly string segmentName;
    public readonly string adNetwork;
    public readonly string instanceName;
    public readonly string instanceId;
    public readonly double? revenue;
    public readonly string precision;
    public readonly double? lifetimeRevenue;
    public readonly string encryptedCPM;




    public IronSourceAdInfo(string json)
    {
        if (json != null && json != IronSourceConstants.EMPTY_STRING)
        {
            try
            {
                object obj;
                double parsedDouble;


                // Retrieve a CultureInfo object.
                CultureInfo invCulture = CultureInfo.InvariantCulture;
                Dictionary<string, object> jsonDic = IronSourceJSON.Json.Deserialize(json) as Dictionary<string, object>;
                if (jsonDic.TryGetValue(IronSourceConstants.IMPRESSION_DATA_KEY_AUCTION_ID, out obj) && obj != null)
                {
                    auctionId = obj.ToString();
                }
                if (jsonDic.TryGetValue(IronSourceConstants.IMPRESSION_DATA_KEY_AD_UNIT, out obj) && obj != null)
                {
                    adUnit = obj.ToString();
                }
                if (jsonDic.TryGetValue(IronSourceConstants.IMPRESSION_DATA_KEY_COUNTRY, out obj) && obj != null)
                {
                    country = obj.ToString();
                }
                if (jsonDic.TryGetValue(IronSourceConstants.IMPRESSION_DATA_KEY_ABTEST, out obj) && obj != null)
                {
                    ab = obj.ToString();
                }
                if (jsonDic.TryGetValue(IronSourceConstants.IMPRESSION_DATA_KEY_SEGMENT_NAME, out obj) && obj != null)
                {
                    segmentName = obj.ToString();
                }
                if (jsonDic.TryGetValue(IronSourceConstants.IMPRESSION_DATA_KEY_AD_NETWORK, out obj) && obj != null)
                {
                    adNetwork = obj.ToString();
                }
                if (jsonDic.TryGetValue(IronSourceConstants.IMPRESSION_DATA_KEY_INSTANCE_NAME, out obj) && obj != null)
                {
                    instanceName = obj.ToString();
                }
                if (jsonDic.TryGetValue(IronSourceConstants.INSTANCE_ID_KEY, out obj) && obj != null)
                {
                    instanceId = obj.ToString();
                }
                if (jsonDic.TryGetValue(IronSourceConstants.IMPRESSION_DATA_KEY_PRECISION, out obj) && obj != null)
                {
                    precision = obj.ToString();
                }
                if (jsonDic.TryGetValue(IronSourceConstants.IMPRESSION_DATA_KEY_ENCRYPTED_CPM, out obj) && obj != null)
                {
                    encryptedCPM = obj.ToString();
                }

                if (jsonDic.TryGetValue(IronSourceConstants.IMPRESSION_DATA_KEY_REVENUE, out obj) && obj != null && double.TryParse(string.Format(invCulture, "{0}", obj), NumberStyles.Any, invCulture, out parsedDouble))
                {
                    revenue = parsedDouble;
                }

                if (jsonDic.TryGetValue(IronSourceConstants.IMPRESSION_DATA_KEY_LIFETIME_REVENUE, out obj) && obj != null && double.TryParse(string.Format(invCulture, "{0}", obj), NumberStyles.Any, invCulture, out parsedDouble))
                {
                    lifetimeRevenue = parsedDouble;
                }

               

            }
            catch (Exception ex)
            {
                Debug.Log("error parsing ad info " + ex.ToString());
            }

        }
    }

    public override string ToString()
    {
        return "IronSourceAdInfo {" +
                "auctionId='" + auctionId + '\'' +
                ", adUnit='" + adUnit + '\'' +
                ", country='" + country + '\'' +
                ", ab='" + ab + '\'' +
                ", segmentName='" + segmentName + '\'' +
                ", adNetwork='" + adNetwork + '\'' +
                ", instanceName='" + instanceName + '\'' +
                ", instanceId='" + instanceId + '\'' +
                ", revenue=" + revenue +
                ", precision='" + precision + '\'' +
                ", lifetimeRevenue=" + lifetimeRevenue +
                ", encryptedCPM='" + encryptedCPM + '\'' +
                '}';
    }
}