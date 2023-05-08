using System;
public interface IUnityImpressionData
{
    event Action<IronSourceImpressionData> OnImpressionDataReady;

    event Action<IronSourceImpressionData> OnImpressionSuccess;
}
