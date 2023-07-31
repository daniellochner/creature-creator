using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class EducationManager : MonoBehaviourSingleton<EducationManager>
    {
        #region Properties
        public string InstitutionId
        {
            get => PlayerPrefs.GetString("INSTITUTION_ID");
            set => PlayerPrefs.GetString("INSTITUTION_ID", value);
        }

        public bool IsEducational => Application.version.EndsWith("-edu");

        public bool IsLinked => !string.IsNullOrEmpty(InstitutionId);
        #endregion

        #region Methods
        public IEnumerator Link(string institutionId)
        {
            // check if exceeded allocated devices

            string countDevicesQuery = $"";

            

            // insert entry

            string insertQuery = $"INSERT INTO devices(device_id, institution_id) VALUES('{Clean(SystemInfo.deviceUniqueIdentifier)}', '{Clean(InstitutionId)}');";



            yield return null;


            // if success.. set id
            InstitutionId = institutionId;
        }

        public void Verify(Action<bool> onVerified)
        {
            if (string.IsNullOrEmpty(InstitutionId))
            {
                onVerified(false);
            }


        }

        private string Clean(string text)
        {
            return text;
        }


        #endregion
    }
}