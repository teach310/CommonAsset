public struct IdText
{
    private int id;
    public int Id { 
        get { return id; }
    }

    private string text;
    public string Text
    {
        get{ return text; }
    }

    public IdText(int id, string text){
        this.id = id;
        this.text = text;
    }
}