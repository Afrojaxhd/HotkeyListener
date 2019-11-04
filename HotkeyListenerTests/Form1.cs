﻿using System;
using System.Drawing;
using System.Windows.Forms;

using WK.Libraries.HotkeyListenerNS;
using WK.Libraries.HotkeyListenerNS.Helpers;

namespace HotkeyListenerTests
{
    public partial class Form1 : Form
    {
        public HotkeyListener hkl = new HotkeyListener();
        public HotkeySelector hks = new HotkeySelector();

        private string hotkey1 = "Control+Shift+E";
        private string hotkey2 = "Control+Y";

        private Form2 form2 = new Form2();

        public Form1()
        {
            InitializeComponent();
            
            hkl.Add(new[] { hotkey1, hotkey2 });
            
            hkl.HotkeyPressed += Hkl_HotkeyPressed;

            hks.Enable(textBox1, "Control+Shift+E");

            hkl.SuspendOn(form2);
        }

        private void Hkl_HotkeyPressed(object sender, HotkeyEventArgs e)
        {
            if (e.Hotkey == hotkey1)
            {
                // textBox3.Text = hkl.GetSelection();
                
                // Activate();
                
                MessageBox.Show(
                    $"You pressed: {e.Hotkey}\n" +
                    $"Name: {e.SourceApplication.Name}\n" +
                    $"Title: {e.SourceApplication.Title}\n" +
                    $"ID: {e.SourceApplication.ID}\n" +
                    $"Handle: {e.SourceApplication.Handle}\n" +
                    $"Path: {e.SourceApplication.Path}"
                );
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(hks.Convert(Keys.F, Keys.Alt | Keys.Shift));
            
            // hkl.SuspendHotkeys();
            // hks.Enable(textBox1, Keys.F1, Keys.Shift | Keys.Alt);
            // hks.Set(textBox1, Keys.F2, Keys.Shift | Keys.Control);
            hkl.Suspend();
            hkl.Update(ref hotkey1, hotkey2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            hkl.Resume();

            // hkl.ResumeHotkeys();
            // hks.Reset(textBox2);
            // MessageBox.Show(hks.IsEnabled(textBox2).ToString());
        }
        
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            form2.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            hkl.ResumeOn(form2);
        }
    }
}