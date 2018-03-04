using System;
using UnityEditor;
using UnityEngine;

namespace CCore.Senary.Editors
{    
    public abstract class BaseEditorWindow : EditorWindow
    {
        public static void ShowWindow<T>()
        {
            EditorWindow.GetWindow(typeof(T));
        }

        /// <summary>
        /// Call this method from any Unity OnGUI method for events to work
        /// </summary>
        protected virtual void OnGUI()
        {
            Event e = Event.current;

            if (e.type == EventType.MouseDown)
            {
                OnMouseDown(e.mousePosition);
            }
            
            if (e.type == EventType.MouseDrag)
            {
                OnMouseDrag(e.mousePosition);
            }

            if (e.type == EventType.MouseUp)
            {
                OnMouseUp(e.mousePosition);
            }
        }

        protected abstract void OnMouseDown(Vector2 position);

        protected abstract void OnMouseDrag(Vector2 position);

        protected abstract void OnMouseUp(Vector2 position);
    }
}
