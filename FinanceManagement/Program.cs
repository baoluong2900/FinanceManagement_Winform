﻿using FinanceManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


namespace AISQBTool
{
    static class Program
    {
        ///
        /// The main entry point for the application.
        ///
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLogin());
        }
    }
}