using System.Text.RegularExpressions;

namespace ModuloEscritura.EscrituraTextMeshPro
{
    public interface ITagToken : IToken
    {
    }

    public static class ITagTokenExtensions
    {
        static readonly Regex ParamterPattern = new Regex("=(.+)>");

        public static string GetParameter(this ITagToken tagToken)
        {
            var match = ParamterPattern.Match(tagToken.Text);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return "";
            }
        }

        public static bool IsOpenTag(this ITagToken tagToken)
        {
            return tagToken.Text[1] != '/';
        }
    }
}