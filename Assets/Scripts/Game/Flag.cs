namespace Game
{
    public class Flag
    {
        public string Id;

        public float AddTime;

        public bool Alive = true;
        
        public Flag(string id, float time)
        {
            Id = id;
            AddTime = time;
            Alive = true;
        }
    }
}