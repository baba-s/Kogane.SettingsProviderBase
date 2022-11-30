using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kogane
{
    public abstract class SettingsProviderBase<T> : SettingsProvider where T : ScriptableObject
    {
        private Editor m_editor;

        protected abstract ScriptableSingleton<T> Instance { get; }

        protected SettingsProviderBase
        (
            string              path,
            SettingsScope       scopes,
            IEnumerable<string> keywords = null
        ) : base( path, scopes, keywords )
        {
        }

        public override void OnActivate( string searchContext, VisualElement rootElement )
        {
            Instance.hideFlags = HideFlags.HideAndDontSave & ~HideFlags.NotEditable;

            Editor.CreateCachedEditor( Instance, null, ref m_editor );
        }

        public override void OnGUI( string searchContext )
        {
            using var changeCheckScope = new EditorGUI.ChangeCheckScope();

            m_editor.OnInspectorGUI();

            if ( !changeCheckScope.changed ) return;

            Instance.Save( true );
        }
    }
}