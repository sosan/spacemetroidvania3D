using UnityEngine;
using System.Collections.Generic;

namespace ModuloEscritura.EscrituraTextMeshPro
{
    //jl. comentado para que no haya tanta xxxx por los menus
    //[CreateAssetMenu(fileName = "EscrituraCharlaSettings", menuName = "Escritura/EscrituraCharlaSettings")]
    public class TypeWriterSettings : ScriptableObject
    {
        [SerializeField] float defaultDelaySeconds;
        [SerializeField] float punctuationDelaySeconds;
        [SerializeField] List<char> punctuationMarks;

        WaitForSeconds defaultDelay;
        WaitForSeconds punctuationDelay;

        public WaitForSeconds DefaultDelay => defaultDelay != null ? defaultDelay : (defaultDelay = new WaitForSeconds(defaultDelaySeconds));
        public WaitForSeconds PunctuationDelay => punctuationDelay != null ? punctuationDelay : (punctuationDelay = new WaitForSeconds(punctuationDelaySeconds));
        public IReadOnlyList<char> PunctuationMarks => punctuationMarks;
    }
}