namespace DanielLochner.Assets
{
    public interface ISetupable
    {
        bool IsSetup { get; set; }
        void Setup();
    }
}