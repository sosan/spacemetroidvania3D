using UnityEngine;

namespace ModuloEscritura.EscrituraTextMeshPro
{
    public interface IWriter
    {
        bool IsSkipRequested { get; }
        bool IsPaused { get; }
        TypeWriterSettings Settings { get; }
        WaitForSeconds Delay { get; set; }

        void SetText();
        void AddText(char addCharacter);
        void AddText(string addString);
    }
}