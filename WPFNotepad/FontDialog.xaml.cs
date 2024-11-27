﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.SqlServer.Server;
using WPFNotepad.Models;
namespace WPFNotepad
{
    /// <summary>
    /// Interaction logic for FontDialog.xaml
    /// </summary>
    public partial class FontDialog 
    {
        public FormatModel Format { get; set; }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
           this.Close();
        }
        public FontDialog()
        {
            InitializeComponent();
        }
    }
}
