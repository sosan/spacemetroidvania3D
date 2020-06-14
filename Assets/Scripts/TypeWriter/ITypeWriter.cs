using System;

namespace ModuloEscritura.EscrituraTextMeshPro
{
    public interface ITypeWriter : IWriter, IDisposable
    {
        event Action<char> OnPrintedCharacter;
        event Action OnWriteCompleted;

        IDisposable Write(string text);
        void Skip();
    }
}