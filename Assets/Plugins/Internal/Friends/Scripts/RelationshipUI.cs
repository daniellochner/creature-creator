using TMPro;
using Unity.Services.Friends.Models;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class RelationshipUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI usernameText;

        protected FriendsMenu menu;
        protected Relationship relationship;
        #endregion

        #region Methods
        public virtual void Setup(FriendsMenu menu, Relationship relationship)
        {
            this.menu = menu;
            this.relationship = relationship;

            usernameText.text = relationship.Member.Profile.Name.NoParse();
        }
        #endregion
    }
}