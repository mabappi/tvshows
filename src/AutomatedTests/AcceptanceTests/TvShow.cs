namespace AcceptanceTests;

internal class TvShow
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<Cast> Cast { get; set; }
}

internal class Cast
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Birthday { get; set; }
}
