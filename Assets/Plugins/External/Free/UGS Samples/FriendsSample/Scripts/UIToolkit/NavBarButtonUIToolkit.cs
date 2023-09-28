using System;
using UnityEngine.UIElements;

namespace Unity.Services.Samples.Friends.UIToolkit
{
    public class NavBarButtonUIToolkit
    {
        public Action onSelected;

        Button m_Button;

        public NavBarButtonUIToolkit(VisualElement navBarView, string buttonName)
        {
            m_Button = navBarView.Q<Button>(buttonName);
            m_Button.RegisterCallback<ClickEvent>((_) => Select());
            Deselect();
        }

        public void Select()
        {
            m_Button.style.backgroundColor = ColorUtils.SelectedNavBarTabColor;
            m_Button.style.unityBackgroundImageTintColor = ColorUtils.YellowColor;
            onSelected?.Invoke();
        }

        public void Deselect()
        {
            m_Button.style.backgroundColor = ColorUtils.DefaultNavBarTabColor;
            m_Button.style.unityBackgroundImageTintColor = ColorUtils.DefaultNavBarIconColor;
        }
    }
}