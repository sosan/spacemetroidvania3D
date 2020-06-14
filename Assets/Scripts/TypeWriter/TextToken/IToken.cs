using System.Collections;
using System.Threading;

namespace ModuloEscritura.EscrituraTextMeshPro
{
    public interface IToken
    {
        string Text { get; }

        void Print(IWriter writer);

        IEnumerator PrintCoroutine(IWriter writer, TypeWriterCancel cancel);
    }
}