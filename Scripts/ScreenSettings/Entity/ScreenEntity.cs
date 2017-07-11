namespace Common.Entity
{
    [System.Serializable]
    public class ScreenEntity
    {
        public int id;
        public int windowId;
        public string name;

        public ScreenEntity(int id, int windowId, string name){
            this.id = id;
            this.windowId = windowId;
            this.name = name;
        }

    }
}
