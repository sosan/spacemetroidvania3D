using System.Collections;
using System.Threading;
using System.Linq;

namespace ModuloEscritura.EscrituraTextMeshPro
{
    public class TextToken : IToken
    {
        const string LessThan = "&lt;";
        const string GreaterThan = "&gt;";

        public string Text { get; private set; }

        public TextToken(string rawToken)
        {
            Text = rawToken.Replace(LessThan, "<").Replace(GreaterThan, ">").Replace("\n", "\n");
        }

        public void Print(IWriter writer)
        {
            writer.AddText(Text);
        }

        public IEnumerator PrintCoroutine(IWriter writer, TypeWriterCancel cancel)
        {
            for (int i = 0; i < Text.Length; ++i)
            {
                if (cancel.IsCancellationRequested)
                {
                    yield break;
                }

                if (writer.IsPaused)
                {
                    --i;
                    yield return null;
                    continue;
                }

                var addCharacter = Text[i];

                if (writer.IsSkipRequested)
                {
                    writer.AddText(addCharacter);
                    continue;
                }
                else
                {
                    writer.AddText(addCharacter);
                    writer.SetText();

                    if (writer.Settings.PunctuationMarks.Contains(addCharacter))
                    {
                        yield return writer.Settings.PunctuationDelay;
                    }
                    else
                    {
                        yield return writer.Delay;
                    }
                }
            }
        }
    }
}