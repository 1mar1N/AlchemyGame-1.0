using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using AlchemyGame.UI;

namespace AlchemyGame;

static class Program
{
    [STAThread]
    static void Main()
    {
        // Включаем современные визуальные стили Windows
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        ApplicationConfiguration.Initialize();

        // Запускаем главное окно
        Application.Run(new MainForm());
    }
}
