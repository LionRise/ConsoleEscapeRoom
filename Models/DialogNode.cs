namespace EscapeRoomGame
{
    public class DialogNode
    {
public int Id { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public List<int> Options { get; set; }
    public string Action { get; set; } 
    public string Condition { get; set; }
    }
}
