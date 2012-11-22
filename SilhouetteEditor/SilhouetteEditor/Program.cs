using System;
using SilhouetteEditor.Forms;

namespace SilhouetteEditor
{
#if WINDOWS
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            MainForm form = new MainForm();
            form.Show();
            EditorLoop editorLoop = new EditorLoop(form.getDrawSurface());
            editorLoop.Run();
        }
    }
#endif
}

