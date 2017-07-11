namespace Common.Entity
{
    [System.Serializable]
    public class WindowEntity
    {
        public int id;
        public string name;

        public WindowEntity(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
