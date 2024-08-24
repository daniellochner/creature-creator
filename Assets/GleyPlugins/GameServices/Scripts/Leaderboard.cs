namespace GleyGameServices
{
    //class used to store leaderboards properties
    [System.Serializable]
    public class Leaderboard
    {
        public string name;
        public string idGoogle;
        public string idIos;

        public Leaderboard(string name, string idGoogle, string idIos)
        {
            this.name = name;
            this.idGoogle = idGoogle;
            this.idIos = idIos;
        }

        public Leaderboard()
        {
            name = "";
            idGoogle = "";
            idIos = "";
        }
    }
}
