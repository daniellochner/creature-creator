// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BoxCreatureSpawner : MonoBehaviour
    {
        #region Fields
        [SerializeField] private BoxCreature boxCreaturePrefab;
        [SerializeField] private bool checkYouTube;
        [Space]
        [SerializeField] private TextAsset[] creatureTextAssets;
        [SerializeField] private float spawnCooldown;
        [SerializeField] private float rotationOffset;

        private List<string> creatures = new List<string>();
        #endregion

        #region Methods
        private IEnumerator Start()
        {
            if (creatureTextAssets.Length > 0)
            {
                foreach (TextAsset creature in creatureTextAssets)
                {
                    creatures.Add(creature.text);
                }
            }
            else if (checkYouTube)
            {
                string key = DemoManager.Instance.Keys.GoogleAPI.Value;
                string parentId = DemoManager.Instance.Keys.PinnedCommentId.Value;

                // Parent
                UnityWebRequest getParent = UnityWebRequest.Get($"https://youtube.googleapis.com/youtube/v3/comments?part=snippet&id={parentId}&textFormat=plainText&key={key}");
                yield return getParent.SendWebRequest();

                JObject pData = JObject.Parse(getParent.downloadHandler.text);
                JObject pItems = pData.GetValue("items").First as JObject;
                JObject pSnippet = (pItems as JObject).GetValue("snippet") as JObject;

                string pTextDisplay = pSnippet.GetValue("textDisplay").ToString();
                string acKey = "acceptedCreatures:\n";
                string acVal = pTextDisplay.Substring(pTextDisplay.IndexOf(acKey) + acKey.Length);
                List<string> acIDs = new List<string>(acVal.Split('\n'));

                // Replies
                UnityWebRequest getReplies = UnityWebRequest.Get($"https://youtube.googleapis.com/youtube/v3/comments?part=snippet&parentId={parentId}&textFormat=plainText&maxResults=100&key={key}");
                yield return getReplies.SendWebRequest();

                JObject rData = JObject.Parse(getReplies.downloadHandler.text);
                JToken rItems = rData.GetValue("items");
                foreach (JToken rItem in rItems)
                {
                    string id = (rItem as JObject).GetValue("id").ToString();
                    id = id.Substring(id.IndexOf('.') + 1);
                    if (acIDs.Contains(id))
                    {
                        JObject rSnippet = (rItem as JObject).GetValue("snippet") as JObject;

                        string rPublishedDate = rSnippet.GetValue("publishedAt").ToString();
                        string rUpdatedDate = rSnippet.GetValue("updatedAt").ToString();
                        if (rPublishedDate == rUpdatedDate)
                        {
                            creatures.Add(rSnippet.GetValue("textDisplay").ToString());
                        }
                    }
                }
            }

            yield return new WaitForSeconds(1); // Wait at least 1 second before spawning creatures

            if (creatures.Count > 0)
            {
                creatures.Shuffle();

                int index = 0;
                while (true)
                {
                    Vector3 position = transform.position;
                    Quaternion rotation = transform.rotation * Quaternion.Euler(Random.Range(-rotationOffset, rotationOffset), Random.Range(-rotationOffset, rotationOffset), Random.Range(-rotationOffset, rotationOffset));

                    BoxCreature boxCreature = Instantiate(boxCreaturePrefab, position, rotation, transform);
                    boxCreature.Setup(JsonUtility.FromJson<CreatureData>(creatures[index % creatures.Count]));

                    yield return new WaitForSeconds(spawnCooldown);
                }
            }
        }
        #endregion
    }
}