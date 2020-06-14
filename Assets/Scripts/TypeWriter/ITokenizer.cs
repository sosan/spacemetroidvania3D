namespace ModuloEscritura.EscrituraTextMeshPro
{
    public interface ITokenizer
    {
        IToken Tokenize(string rawToken);
    }
}