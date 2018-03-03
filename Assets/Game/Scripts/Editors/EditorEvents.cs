using System;
using UnityEngine;

namespace CCore.Senary.Editors
{
    public enum EditorMouseState
    {
        Down,
        Drag,
        Up,
    }

    public class EditorMouseEventArgs : EventArgs
    {
        public Vector2 position { get; private set; }

        public EditorMouseState editorMouseState { get; private set; }

        public EditorMouseEventArgs(Vector2 position, EditorMouseState editorMouseState)
        {
            this.position = position;

            this.editorMouseState = editorMouseState;
        }
    }
    
    // TODO: Make this a Base class for editor windows instead
    public class EditorEvents
    {
        public event EventHandler<EditorMouseEventArgs> EditorMouseEvent;

        private void DispatchEditorMouseEvent(Vector2 position, EditorMouseState editorMouseState)
        {
            EditorMouseEventArgs eventArgs = new EditorMouseEventArgs(position, editorMouseState);

            if (EditorMouseEvent != null)
            {
                EditorMouseEvent(this, eventArgs);
            }
        }

        /// <summary>
        /// Call this method from any Unity OnGUI method for events to work
        /// </summary>
        public void OnGUI()
        {
            Event e = Event.current;

            if (e.type == EventType.MouseDown)
            {
                DispatchEditorMouseEvent(e.mousePosition, EditorMouseState.Down);
            }
            
            if (e.type == EventType.MouseDrag)
            {
                DispatchEditorMouseEvent(e.mousePosition, EditorMouseState.Drag);
            }

            if (e.type == EventType.MouseUp)
            {
                DispatchEditorMouseEvent(e.mousePosition, EditorMouseState.Up);
            }
        }
    }
}