using System.Text.RegularExpressions;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class NetworkCreatureData : INetworkSerializable
    {
        public CreatureData Data { get; set; }

        public NetworkCreatureData()
        {
        }
        public NetworkCreatureData(CreatureData data)
        {
            Data = Data;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            string data = "";
            
            if (serializer.IsWriter)
            {
                data = JsonUtility.ToJson(Data);

                // Replace all exponential notation with 0
                data = Regex.Replace(data, "[+-]?[0-9]+\\.[0-9]+e[+-][0-9]+", "0");

                // Round all floats to X decimal points
                MatchEvaluator round = new MatchEvaluator(delegate (Match t)
                {
                    string num = t.Value;
                    int i = num.IndexOf('.');

                    int length = num.Substring(i + 1).Length;

                    if (length > 3)
                    {
                        return num.Substring(0, (i + 1) + 3);
                    }
                    else
                    {
                        return num;
                    }
                });
                data = Regex.Replace(data, "[+-]?[0-9]+\\.[0-9]+", round);
            }

            serializer.SerializeValue(ref data);

            if (serializer.IsReader)
            {
                Data = JsonUtility.FromJson<CreatureData>(data);
            }
        }
    }
}