using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public static class RemoteConfigUtility
    {
        #region Methods
        public static IEnumerator FetchConfigRoutine()
        {
            Task fetchConfig = FetchConfigAsync();
            yield return new WaitUntil(() => fetchConfig.IsCompleted);
        }

        public static async Task FetchConfigAsync()
        {
            await UnityServices.InitializeAsync();
            await RemoteConfigService.Instance.FetchConfigsAsync(new UserAttributes(), new AppAttributes());
        }
        #endregion

        #region Nested
        private struct UserAttributes { }
        private struct AppAttributes { }
        #endregion
    }
}