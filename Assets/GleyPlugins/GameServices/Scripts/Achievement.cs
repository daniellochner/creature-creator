namespace GleyGameServices
{
    //class used to store achievements properties
    [System.Serializable]
    public class Achievement
    {
        public string name;
        public string idGoogle;
        public string idIos;
        public bool achivementComplete;

        public Achievement(string name, string idGoogle, string idIos)
        {
            this.name = name;
            this.idGoogle = idGoogle;
            this.idIos = idIos;
            achivementComplete = false;
        }

        public Achievement()
        {
            name = "";
            idGoogle = "";
            idIos = "";
        }
    }
}