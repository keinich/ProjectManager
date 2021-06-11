namespace ProjectManager.Api.Models {

  public class Card {
    public int Id { get; set; }
    public string Name { get; set; }
    public string UserId { get; set; }
    public int PositionX { get; set; }
    public int PositionY { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string Content { get; set; }
    public CardType Type { get; set; }
  }

}
