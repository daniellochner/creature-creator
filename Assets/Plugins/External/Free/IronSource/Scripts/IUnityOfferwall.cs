using System;
using System.Collections.Generic;

public interface IUnityOfferwall
    {

    event Action<IronSourceError> OnOfferwallShowFailed;

    event Action OnOfferwallOpened;

    event Action OnOfferwallClosed;

    event Action<IronSourceError> OnGetOfferwallCreditsFailed;

    event Action<Dictionary<string, object>> OnOfferwallAdCredited;

    event Action<bool> OnOfferwallAvailable;

}
