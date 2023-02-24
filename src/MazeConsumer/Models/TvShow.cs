namespace MazeConsumer.Services;

public class TvShow
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<Cast> Casts { get; set; }
}