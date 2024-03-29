using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Security.AccessControl;
using ID3;
using ID3.ID3v1Frame;
using ID3.ID3v2Frames;

using System.Xml;
using System.Xml.Serialization;


namespace Mp3Rename
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (checkBox1.Checked)
            {
                comboBoxInput.Enabled = true;
                comboBoxOut.Enabled = true;

                buttonInput.Enabled = true;
                buttonOut.Enabled = true;
            }
            else
            {
                comboBoxInput.Enabled = false;
                comboBoxOut.Enabled = false;

                buttonInput.Enabled = false;
                buttonOut.Enabled = false;
            }
            CenterToScreen();
            InitializeCodePageLists();
            loadSetting();
        }

        struct CodePage
        {
            public int iCodePage;
            public string strName;
            public string strDisplayName;
            public CodePage(int i, string n, string d)
            {
                iCodePage = i;
                strName = n;
                strDisplayName = d;
            }

            public int CodePageNum
            {
                get { return iCodePage; }
            }

            public string DisplayName
            {
                get { return strName + " - " + strDisplayName; }
            }
        }

        private CodePage[] ConstructCodePageArray()
        {
            return new CodePage[] { 
                new CodePage(37 	   ,"IBM037"                     ,"IBM EBCDIC (US-Canada)"),
                new CodePage(437       ,"IBM437"                     ,"OEM United States"),
                new CodePage(500       ,"IBM500"                     ,"IBM EBCDIC (International)"),
                new CodePage(708       ,"ASMO-708"                   ,"Arabic (ASMO 708)"),
                new CodePage(720       ,"DOS-720"                    ,"Arabic (DOS)"),
                new CodePage(737       ,"ibm737"                     ,"Greek (DOS)"),
                new CodePage(775       ,"ibm775"                     ,"Baltic (DOS)"),
                new CodePage(850       ,"ibm850"                     ,"Western European (DOS)"),
                new CodePage(852       ,"ibm852"                     ,"Central European (DOS)"),
                new CodePage(855       ,"IBM855"                     ,"OEM Cyrillic"),
                new CodePage(857       ,"ibm857"                     ,"Turkish (DOS)"),
                new CodePage(858       ,"IBM00858"                   ,"OEM Multilingual Latin I"),
                new CodePage(860       ,"IBM860"                     ,"Portuguese (DOS)"),
                new CodePage(861       ,"ibm861"                     ,"Icelandic (DOS)"),
                new CodePage(862       ,"DOS-862"                    ,"Hebrew (DOS)"),
                new CodePage(863       ,"IBM863"                     ,"French Canadian (DOS)"),
                new CodePage(864       ,"IBM864"                     ,"Arabic (864)"),
                new CodePage(865       ,"IBM865"                     ,"Nordic (DOS)"),
                new CodePage(866       ,"cp866"                      ,"Cyrillic (DOS)"),
                new CodePage(869       ,"ibm869"                     ,"Greek, Modern (DOS)"),
                new CodePage(870       ,"IBM870"                     ,"IBM EBCDIC (Multilingual Latin-2)"),
                new CodePage(874       ,"windows-874"                ,"Thai (Windows)"),
                new CodePage(875       ,"cp875"                      ,"IBM EBCDIC (Greek Modern)"),
                new CodePage(932       ,"shift_jis"                  ,"Japanese (Shift-JIS)"),
                new CodePage(936       ,"gb2312"                     ,"Chinese Simplified (GB2312)"),
                new CodePage(949       ,"ks_c_5601-1987"             ,"Korean"),
                new CodePage(950       ,"big5"                       ,"Chinese Traditional (Big5)"),
                new CodePage(1026      ,"IBM1026"                    ,"IBM EBCDIC (Turkish Latin-5)"),
                new CodePage(1047      ,"IBM01047"                   ,"IBM Latin-1"),
                new CodePage(1140      ,"IBM01140"                   ,"IBM EBCDIC (US-Canada-Euro)"),
                new CodePage(1141      ,"IBM01141"                   ,"IBM EBCDIC (Germany-Euro)"),
                new CodePage(1142      ,"IBM01142"                   ,"IBM EBCDIC (Denmark-Norway-Euro)"),
                new CodePage(1143      ,"IBM01143"                   ,"IBM EBCDIC (Finland-Sweden-Euro)"),
                new CodePage(1144      ,"IBM01144"                   ,"IBM EBCDIC (Italy-Euro)"),
                new CodePage(1145      ,"IBM01145"                   ,"IBM EBCDIC (Spain-Euro)"),
                new CodePage(1146      ,"IBM01146"                   ,"IBM EBCDIC (UK-Euro)"),
                new CodePage(1147      ,"IBM01147"                   ,"IBM EBCDIC (France-Euro)"),
                new CodePage(1148      ,"IBM01148"                   ,"IBM EBCDIC (International-Euro)"),
                new CodePage(1149      ,"IBM01149"                   ,"IBM EBCDIC (Icelandic-Euro)"),
                new CodePage(1200      ,"utf-16"                     ,"Unicode"),
                new CodePage(1201      ,"unicodeFFFE"                ,"Unicode (Big-Endian)"),
                new CodePage(1250      ,"windows-1250"               ,"Central European (Windows)"),
                new CodePage(1251      ,"windows-1251"               ,"Cyrillic (Windows)"),
                new CodePage(1252      ,"Windows-1252"               ,"Western European (Windows)"),
                new CodePage(1253      ,"windows-1253"               ,"Greek (Windows)"),
                new CodePage(1254      ,"windows-1254"               ,"Turkish (Windows)"),
                new CodePage(1255      ,"windows-1255"               ,"Hebrew (Windows)"),
                new CodePage(1256      ,"windows-1256"               ,"Arabic (Windows)"),
                new CodePage(1257      ,"windows-1257"               ,"Baltic (Windows)"),
                new CodePage(1258      ,"windows-1258"               ,"Vietnamese (Windows)"),
                new CodePage(1361      ,"Johab"                      ,"Korean (Johab)"),
                new CodePage(10000     ,"macintosh"                  ,"Western European (Mac)"),
                new CodePage(10001     ,"x-mac-japanese"             ,"Japanese (Mac)"),
                new CodePage(10002     ,"x-mac-chinesetrad"          ,"Chinese Traditional (Mac)"),
                new CodePage(10003     ,"x-mac-korean"               ,"Korean (Mac)"),
                new CodePage(10004     ,"x-mac-arabic"               ,"Arabic (Mac)"),
                new CodePage(10005     ,"x-mac-hebrew"               ,"Hebrew (Mac)"),
                new CodePage(10006     ,"x-mac-greek"                ,"Greek (Mac)"),
                new CodePage(10007     ,"x-mac-cyrillic"             ,"Cyrillic (Mac)"),
                new CodePage(10008     ,"x-mac-chinesesimp"          ,"Chinese Simplified (Mac)"),
                new CodePage(10010     ,"x-mac-romanian"             ,"Romanian (Mac)"),
                new CodePage(10017     ,"x-mac-ukrainian"            ,"Ukrainian (Mac)"),
                new CodePage(10021     ,"x-mac-thai"                 ,"Thai (Mac)"),
                new CodePage(10029     ,"x-mac-ce"                   ,"Central European (Mac)"),
                new CodePage(10079     ,"x-mac-icelandic"            ,"Icelandic (Mac)"),
                new CodePage(10081     ,"x-mac-turkish"              ,"Turkish (Mac)"),
                new CodePage(10082     ,"x-mac-croatian"             ,"Croatian (Mac)"),
                new CodePage(20000     ,"x-Chinese-CNS"              ,"Chinese Traditional (CNS)"),
                new CodePage(20001     ,"x-cp20001"                  ,"TCA Taiwan"),
                new CodePage(20002     ,"x-Chinese-Eten"             ,"Chinese Traditional (Eten)"),
                new CodePage(20003     ,"x-cp20003"                  ,"IBM5550 Taiwan"),
                new CodePage(20004     ,"x-cp20004"                  ,"TeleText Taiwan"),
                new CodePage(20005     ,"x-cp20005"                  ,"Wang Taiwan"),
                new CodePage(20105     ,"x-IA5"                      ,"Western European (IA5)"),
                new CodePage(20106     ,"x-IA5-German"               ,"German (IA5)"),
                new CodePage(20107     ,"x-IA5-Swedish"              ,"Swedish (IA5)"),
                new CodePage(20108     ,"x-IA5-Norwegian"            ,"Norwegian (IA5)"),
                new CodePage(20127     ,"us-ascii"                   ,"US-ASCII"),
                new CodePage(20261     ,"x-cp20261"                  ,"T.61"),
                new CodePage(20269     ,"x-cp20269"                  ,"ISO-6937"),
                new CodePage(20273     ,"IBM273"                     ,"IBM EBCDIC (Germany)"),
                new CodePage(20277     ,"IBM277"                     ,"IBM EBCDIC (Denmark-Norway)"),
                new CodePage(20278     ,"IBM278"                     ,"IBM EBCDIC (Finland-Sweden)"),
                new CodePage(20280     ,"IBM280"                     ,"IBM EBCDIC (Italy)"),
                new CodePage(20284     ,"IBM284"                     ,"IBM EBCDIC (Spain)"),
                new CodePage(20285     ,"IBM285"                     ,"IBM EBCDIC (UK)"),
                new CodePage(20290     ,"IBM290"                     ,"IBM EBCDIC (Japanese katakana)"),
                new CodePage(20297     ,"IBM297"                     ,"IBM EBCDIC (France)"),
                new CodePage(20420     ,"IBM420"                     ,"IBM EBCDIC (Arabic)"),
                new CodePage(20423     ,"IBM423"                     ,"IBM EBCDIC (Greek)"),
                new CodePage(20424     ,"IBM424"                     ,"IBM EBCDIC (Hebrew)"),
                new CodePage(20833     ,"x-EBCDIC-KoreanExtended"    ,"IBM EBCDIC (Korean Extended)"),
                new CodePage(20838     ,"IBM-Thai"                   ,"IBM EBCDIC (Thai)"),
                new CodePage(20866     ,"koi8-r"                     ,"Cyrillic (KOI8-R)"),
                new CodePage(20871     ,"IBM871"                     ,"IBM EBCDIC (Icelandic)"),
                new CodePage(20880     ,"IBM880"                     ,"IBM EBCDIC (Cyrillic Russian)"),
                new CodePage(20905     ,"IBM905"                     ,"IBM EBCDIC (Turkish)"),
                new CodePage(20924     ,"IBM00924"                   ,"IBM Latin-1"),
                new CodePage(20932     ,"EUC-JP"                     ,"Japanese (JIS 0208-1990 and 0212-1990)"),
                new CodePage(20936     ,"x-cp20936"                  ,"Chinese Simplified (GB2312-80)"),
                new CodePage(20949     ,"x-cp20949"                  ,"Korean Wansung"),
                new CodePage(21025     ,"cp1025"                     ,"IBM EBCDIC (Cyrillic Serbian-Bulgarian)"),
                new CodePage(21866     ,"koi8-u"                     ,"Cyrillic (KOI8-U)"),
                new CodePage(28591     ,"iso-8859-1"                 ,"Western European (ISO)"),
                new CodePage(28592     ,"iso-8859-2"                 ,"Central European (ISO)"),
                new CodePage(28593     ,"iso-8859-3"                 ,"Latin 3 (ISO)"),
                new CodePage(28594     ,"iso-8859-4"                 ,"Baltic (ISO)"),
                new CodePage(28595     ,"iso-8859-5"                 ,"Cyrillic (ISO)"),
                new CodePage(28596     ,"iso-8859-6"                 ,"Arabic (ISO)"),
                new CodePage(28597     ,"iso-8859-7"                 ,"Greek (ISO)"),
                new CodePage(28598     ,"iso-8859-8"                 ,"Hebrew (ISO-Visual)"),
                new CodePage(28599     ,"iso-8859-9"                 ,"Turkish (ISO)"),
                new CodePage(28603     ,"iso-8859-13"                ,"Estonian (ISO)"),
                new CodePage(28605     ,"iso-8859-15"                ,"Latin 9 (ISO)"),
                new CodePage(29001     ,"x-Europa"                   ,"Europa"),
                new CodePage(38598     ,"iso-8859-8-i"               ,"Hebrew (ISO-Logical)"),
                new CodePage(50220     ,"iso-2022-jp"                ,"Japanese (JIS)"),
                new CodePage(50221     ,"csISO2022JP"                ,"Japanese (JIS-Allow 1 byte Kana)"),
                new CodePage(50222     ,"iso-2022-jp"                ,"Japanese (JIS-Allow 1 byte Kana - SO/SI) 	"),
                new CodePage(50225     ,"iso-2022-kr"                ,"Korean (ISO)"),
                new CodePage(50227     ,"x-cp50227"                  ,"Chinese Simplified (ISO-2022)"),
                new CodePage(51932     ,"euc-jp"                     ,"Japanese (EUC)"),
                new CodePage(51936     ,"EUC-CN"                     ,"Chinese Simplified (EUC)"),
                new CodePage(51949     ,"euc-kr"                     ,"Korean (EUC)"),
                new CodePage(52936     ,"hz-gb-2312"                 ,"Chinese Simplified (HZ)"),
                new CodePage(54936     ,"GB18030"                    ,"Chinese Simplified (GB18030)"),
                new CodePage(57002     ,"x-iscii-de"                 ,"ISCII Devanagari"),
                new CodePage(57003     ,"x-iscii-be"                 ,"ISCII Bengali"),
                new CodePage(57004     ,"x-iscii-ta"                 ,"ISCII Tamil"),
                new CodePage(57005     ,"x-iscii-te"                 ,"ISCII Telugu"),
                new CodePage(57006     ,"x-iscii-as"                 ,"ISCII Assamese"),
                new CodePage(57007     ,"x-iscii-or"                 ,"ISCII Oriya"),
                new CodePage(57008     ,"x-iscii-ka"                 ,"ISCII Kannada"),
                new CodePage(57009     ,"x-iscii-ma"                 ,"ISCII Malayalam"),
                new CodePage(57010     ,"x-iscii-gu"                 ,"ISCII Gujarati"),
                new CodePage(57011     ,"x-iscii-pa"                 ,"ISCII Punjabi"),
                new CodePage(65000     ,"utf-7"                      ,"Unicode (UTF-7)"),
                new CodePage(65001     ,"utf-8"                      ,"Unicode (UTF-8)"),
                new CodePage(65005     ,"utf-32"                     ,"Unicode (UTF-32)"),
                new CodePage(65006     ,"utf-32BE"                   ,"Unicode (UTF-32 Big-Endian)")
            };
        }

        private CodePage[] ConstructShortCodePageArray()
        {
            return new CodePage[] { 
                new CodePage(1200      ,"utf-16"                     ,"Unicode"),
                new CodePage(1201      ,"unicodeFFFE"                ,"Unicode (Big-Endian)"),
                new CodePage(1250      ,"windows-1250"               ,"Central European (Windows)"),
                new CodePage(1251      ,"windows-1251"               ,"Cyrillic (Windows)"),
                new CodePage(1252      ,"Windows-1252"               ,"Western European (Windows)"),
                new CodePage(1253      ,"windows-1253"               ,"Greek (Windows)"),
                new CodePage(1254      ,"windows-1254"               ,"Turkish (Windows)"),
                new CodePage(1255      ,"windows-1255"               ,"Hebrew (Windows)"),
                new CodePage(1256      ,"windows-1256"               ,"Arabic (Windows)"),
                new CodePage(1257      ,"windows-1257"               ,"Baltic (Windows)"),
                new CodePage(1258      ,"windows-1258"               ,"Vietnamese (Windows)"),
                new CodePage(65001     ,"utf-8"                      ,"Unicode (UTF-8)"),
            };
        }


        private void InitializeCodePageLists()
        {

            this.comboBoxInput.DataSource = ConstructShortCodePageArray();
            this.comboBoxInput.DisplayMember = "DisplayName";
            this.comboBoxInput.ValueMember = "CodePageNum";

            this.comboBoxOut.DataSource = ConstructShortCodePageArray();
            this.comboBoxOut.DisplayMember = "DisplayName";
            this.comboBoxOut.ValueMember = "CodePageNum";            
        }

        #region ====== Notify icon =====
        
        FormWindowState previouseState;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            previouseState = WindowState;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (previouseState != WindowState)
            {
                previouseState = WindowState;

                if (WindowState.ToString() == "Minimized")
                {
                    this.ShowInTaskbar = false;
                    this.Visible = false;
                    this.notifyIcon1.Visible = true;
                }
                else if (WindowState.ToString() == "Normal")
                {
                    this.ShowInTaskbar = true;
                    this.Visible = true;
                    this.notifyIcon1.Visible = false;
                }

            }
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_Click_1(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.ShowInTaskbar = true;
                //                notifyIcon.Visible = true;
                this.Visible = true;
                this.StartPosition = FormStartPosition.CenterScreen;
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }
            else
            {
                this.ShowInTaskbar = true;
                //                notifyIcon.Visible = false;
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;


            }
        }
        
        #endregion =====================
        
        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            SaveSetting();
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                string[] direkts = Directory.GetDirectories(textBox1.Text, "*", SearchOption.AllDirectories);
                /*
                string res = "";
                foreach (string Dir in direkts)
                {
                    res += Dir + "\n";
                }
                MessageBox.Show(res);
                */
                this.Cursor = Cursors.WaitCursor;
                progressBar1.Show();

                

                if (direkts.Length != 0)
                {
                    WorkWithFolder(ConvName(textBox1.Text));

                    foreach (string dir in direkts)
                    {
                        if (Regex.Match(dir, "[а-яА-Я]").Value.Length > 0)
                        {
                            string newName = ConvName(dir);
                            DirectoryInfo myInfo = new DirectoryInfo(dir);
                            myInfo.Attributes = FileAttributes.Archive;
                            Directory.Move(dir, newName);
                            WorkWithFolder(newName);
                        }
                        else
                        {
                            WorkWithFolder(dir);
                        }
                    }
                }
                else
                {
                    WorkWithFolder(textBox1.Text);
                }

                progressBar1.Hide();
                
                this.Cursor = Cursors.Arrow;

                if (this.ShowInTaskbar)
                {
                    MessageBox.Show("Ok !", "Work done !");
                }
                else
                {
                    notifyIcon1.ShowBalloonTip(30, "Work done !", "Ok !", ToolTipIcon.Info);
                }

            }
            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }
        
        private void WorkWithFolder(string Folder)
        {
            try
            {
                string[] files = Directory.GetFiles(Folder, "*.*");
                Application.DoEvents();
                if (files.Length != 0)
                {
                    foreach (string FileName in files)
                    {
                        progressBar1.Minimum = 0;
                        progressBar1.Maximum = files.Length;
                        System.IO.FileInfo fi = new System.IO.FileInfo(FileName);

                        fi.Attributes = FileAttributes.Archive;

                        if (!Folder.EndsWith("\\"))
                        {
                            Folder = Folder + "\\";
                        }

                        if (Regex.Match(FileName, "[а-яА-Я№]").Value.Length > 0)
                        {
                            try
                            {
                                File.Move(FileName, Folder + ConvName(fi.Name));

                                if (checkBox1.Checked)
                                {
                                    ConvertID3_2(Folder + ConvName(fi.Name));
                                }

                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            if (checkBox1.Checked)
                            {
                                ConvertID3_2(FileName);
                            }
                        }

                        progressBar1.Value += 1;

                    }
                }

                progressBar1.Maximum = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ConvertID3(string mp3FilePath)
        {
            ID3Info info = new ID3Info(mp3FilePath, true);

            if (info.ID3v1Info.HaveTag)
            {
                if (Regex.Match(StupiddEncoding(info.ID3v1Info.Artist), "[а-яА-Я№]").Value.Length > 0 || Regex.Match(StupiddEncoding(info.ID3v1Info.Album), "[а-яА-Я№]").Value.Length > 0
                      || Regex.Match(StupiddEncoding(info.ID3v1Info.Title), "[а-яА-Я№]").Value.Length > 0)
                {
                    info.ID3v1Info.Artist = ConvName(StupiddEncoding(info.ID3v1Info.Artist));
                    info.ID3v1Info.Album = ConvName(StupiddEncoding(info.ID3v1Info.Album));
                    info.ID3v1Info.Title = ConvName(StupiddEncoding(info.ID3v1Info.Title));
                    info.ID3v1Info.Comment = ConvName(StupiddEncoding(info.ID3v1Info.Comment));

                    if (info.ID3v2Info.HaveTag)
                    {
                        info.ID3v2Info.ClearAll();
                        info.ID3v2Info.SetMinorVersion(3);
                        //Title            
                        info.ID3v2Info.SetTextFrame("TIT2", ConvName(StupiddEncoding(info.ID3v1Info.Title)));

                        //Track
                        //info.ID3v2Info.SetTextFrame("TRCK", ConvName(StupiddEncoding(info.ID3v1Info.Title)));

                        //Disk
                        //info.ID3v2Info.SetTextFrame("TPOS","Text 3");

                        //Artist
                        //info.ID3v2Info.SetTextFrame("TPE1", ConvName(StupiddEncoding(info.ID3v1Info.Artist)));

                        //Album
                        //info.ID3v2Info.SetTextFrame("TALB", ConvName(StupiddEncoding(info.ID3v1Info.Album)));

                        //Genre
                        //info.ID3v2Info.SetTextFrame("TCON", cmb2Genre.Genre);
                        //Languege
                        //info.ID3v2Info.SetTextFrame("TLAN", cmb2Language.Language);                     
                    }
                    info.Save();
                }
            }

        }

        private void ConvertID3_2(string mp3FilePath)
        {
            ID3Info info = new ID3Info(mp3FilePath, true);

            if (info.ID3v1Info.HaveTag)
            {
                if (Regex.Match(Converts(info.ID3v1Info.Artist, Encoding.GetEncoding((int)comboBoxInput.SelectedValue), Encoding.GetEncoding((int)comboBoxOut.SelectedValue)), "[а-яА-Я№]").Value.Length > 0 ||
                    Regex.Match(Converts(info.ID3v1Info.Album, Encoding.GetEncoding((int)comboBoxInput.SelectedValue), Encoding.GetEncoding((int)comboBoxOut.SelectedValue)), "[а-яА-Я№]").Value.Length > 0 ||
                    Regex.Match(Converts(info.ID3v1Info.Title, Encoding.GetEncoding((int)comboBoxInput.SelectedValue), Encoding.GetEncoding((int)comboBoxOut.SelectedValue)), "[а-яА-Я№]").Value.Length > 0 ||
                    Regex.Match(Converts(info.ID3v1Info.Comment, Encoding.GetEncoding((int)comboBoxInput.SelectedValue), Encoding.GetEncoding((int)comboBoxOut.SelectedValue)), "[а-яА-Я№]").Value.Length > 0)
                {

                    info.ID3v1Info.Artist = Converts(info.ID3v1Info.Artist, Encoding.GetEncoding((int)comboBoxInput.SelectedValue), Encoding.GetEncoding((int)comboBoxOut.SelectedValue));
                    info.ID3v1Info.Album = Converts(info.ID3v1Info.Album, Encoding.GetEncoding((int)comboBoxInput.SelectedValue), Encoding.GetEncoding((int)comboBoxOut.SelectedValue));
                    info.ID3v1Info.Title = Converts(info.ID3v1Info.Title, Encoding.GetEncoding((int)comboBoxInput.SelectedValue), Encoding.GetEncoding((int)comboBoxOut.SelectedValue));
                    info.ID3v1Info.Comment = Converts(info.ID3v1Info.Comment, Encoding.GetEncoding((int)comboBoxInput.SelectedValue), Encoding.GetEncoding((int)comboBoxOut.SelectedValue));




                    info.ID3v2Info.ClearAll();
                    info.ID3v2Info.SetMinorVersion(3);
                    //Title            
                    info.ID3v2Info.SetTextFrame("TIT2", info.ID3v1Info.Title);
                    //Artist
                    info.ID3v2Info.SetTextFrame("TPE1", info.ID3v1Info.Artist);
                    //Album
                    info.ID3v2Info.SetTextFrame("TALB", info.ID3v1Info.Album);

                    info.ID3v1Info.Album = "";
                    info.ID3v1Info.Artist = "";
                    info.ID3v1Info.Comment = "";
                    info.ID3v1Info.Title = "";


                    info.Save();
                }
            } 

            /*
            if (info.ID3v1Info.HaveTag)
            {
                if (Regex.Match(Converts(info.ID3v1Info.Artist, Encoding.GetEncoding(1251), Encoding.GetEncoding(1250)), "[а-яА-Я№]").Value.Length > 0 ||
                    Regex.Match(Converts(info.ID3v1Info.Album, Encoding.GetEncoding(1251), Encoding.GetEncoding(1250)), "[а-яА-Я№]").Value.Length > 0 ||
                    Regex.Match(Converts(info.ID3v1Info.Title, Encoding.GetEncoding(1251), Encoding.GetEncoding(1250)), "[а-яА-Я№]").Value.Length > 0 ||
                    Regex.Match(Converts(info.ID3v1Info.Comment, Encoding.GetEncoding(1251), Encoding.GetEncoding(1250)), "[а-яА-Я№]").Value.Length > 0)
                {

                    info.ID3v1Info.Artist = Converts(info.ID3v1Info.Artist, Encoding.GetEncoding(1251), Encoding.GetEncoding(1250));
                    info.ID3v1Info.Album = Converts(info.ID3v1Info.Album, Encoding.GetEncoding(1251), Encoding.GetEncoding(1250));
                    info.ID3v1Info.Title = Converts(info.ID3v1Info.Title, Encoding.GetEncoding(1251), Encoding.GetEncoding(1250));
                    info.ID3v1Info.Comment = Converts(info.ID3v1Info.Comment, Encoding.GetEncoding(1251), Encoding.GetEncoding(1250));




                    info.ID3v2Info.ClearAll();
                    info.ID3v2Info.SetMinorVersion(3);
                    //Title            
                    info.ID3v2Info.SetTextFrame("TIT2", info.ID3v1Info.Title);
                    //Artist
                    info.ID3v2Info.SetTextFrame("TPE1", info.ID3v1Info.Artist);
                    //Album
                    info.ID3v2Info.SetTextFrame("TALB", info.ID3v1Info.Album);

                    info.ID3v1Info.Album = "";
                    info.ID3v1Info.Artist = "";
                    info.ID3v1Info.Comment = "";
                    info.ID3v1Info.Title = "";


                    info.Save();
                }
            } 
             */            
        }
        
        private string Encoding2UTF(string source, Encoding ascii, Encoding unicode)
        {

            // Convert the string into a byte[].
            byte[] unicodeBytes = unicode.GetBytes(source);

            // Perform the conversion from one encoding to the other.
            byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);

            // Convert the new byte[] into a char[] and then into a string.
            // This is a slightly different approach to converting to illustrate
            // the use of GetCharCount/GetChars.
            char[] asciiChars = new char[ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
            ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
            string asciiString = new string(asciiChars);

            return asciiString;
        }

        private string Encodings(string source)
        {
            
            Encoding cp1251 = Encoding.GetEncoding(1250);
            Encoding utf8 = Encoding.UTF8;
            byte[] cp1251Bytes = cp1251.GetBytes(source);
            byte[] utf8Bytes = Encoding.Convert(cp1251, utf8, cp1251Bytes);
            string utf8String = utf8.GetString(utf8Bytes);

            return utf8String;
        }

        public static string Converts(string value, Encoding src, Encoding trg)
        {
            Decoder dec = src.GetDecoder();
            byte[] ba = trg.GetBytes(value);
            int len = dec.GetCharCount(ba, 0, ba.Length);
            char[] ca = new char[len];
            dec.GetChars(ba, 0, ba.Length, ca, 0);
            return new string(ca);
        }

        private string ConvName(string fileMp3Name)
        {

            fileMp3Name = fileMp3Name.Replace("А", "A");
            fileMp3Name = fileMp3Name.Replace("а", "a");

            fileMp3Name = fileMp3Name.Replace("Б", "B");
            fileMp3Name = fileMp3Name.Replace("б", "b");

            fileMp3Name = fileMp3Name.Replace("В", "V");
            fileMp3Name = fileMp3Name.Replace("в", "v");

            fileMp3Name = fileMp3Name.Replace("Г", "G");
            fileMp3Name = fileMp3Name.Replace("г", "g");

            fileMp3Name = fileMp3Name.Replace("Д", "D");
            fileMp3Name = fileMp3Name.Replace("д", "d");

            fileMp3Name = fileMp3Name.Replace("Е", "E");
            fileMp3Name = fileMp3Name.Replace("е", "e");

            fileMp3Name = fileMp3Name.Replace("Ё", "JO");
            fileMp3Name = fileMp3Name.Replace("ё", "jo");

            fileMp3Name = fileMp3Name.Replace("Ж", "G");
            fileMp3Name = fileMp3Name.Replace("ж", "g");

            fileMp3Name = fileMp3Name.Replace("З", "Z");
            fileMp3Name = fileMp3Name.Replace("з", "z");

            fileMp3Name = fileMp3Name.Replace("И", "I");
            fileMp3Name = fileMp3Name.Replace("и", "i");

            fileMp3Name = fileMp3Name.Replace("Й", "I");
            fileMp3Name = fileMp3Name.Replace("й", "i");

            fileMp3Name = fileMp3Name.Replace("К", "K");
            fileMp3Name = fileMp3Name.Replace("к", "k");

            fileMp3Name = fileMp3Name.Replace("Л", "L");
            fileMp3Name = fileMp3Name.Replace("л", "l");

            fileMp3Name = fileMp3Name.Replace("М", "M");
            fileMp3Name = fileMp3Name.Replace("м", "m");

            fileMp3Name = fileMp3Name.Replace("Н", "N");
            fileMp3Name = fileMp3Name.Replace("н", "n");

            fileMp3Name = fileMp3Name.Replace("О", "O");
            fileMp3Name = fileMp3Name.Replace("о", "o");

            fileMp3Name = fileMp3Name.Replace("П", "P");
            fileMp3Name = fileMp3Name.Replace("п", "p");

            fileMp3Name = fileMp3Name.Replace("Р", "R");
            fileMp3Name = fileMp3Name.Replace("р", "r");

            fileMp3Name = fileMp3Name.Replace("С", "S");
            fileMp3Name = fileMp3Name.Replace("с", "s");

            fileMp3Name = fileMp3Name.Replace("Т", "T");
            fileMp3Name = fileMp3Name.Replace("т", "t");

            fileMp3Name = fileMp3Name.Replace("У", "U");
            fileMp3Name = fileMp3Name.Replace("у", "u");

            fileMp3Name = fileMp3Name.Replace("Ф", "F");
            fileMp3Name = fileMp3Name.Replace("ф", "f");

            fileMp3Name = fileMp3Name.Replace("Х", "H");
            fileMp3Name = fileMp3Name.Replace("х", "h");

            fileMp3Name = fileMp3Name.Replace("Ц", "C");
            fileMp3Name = fileMp3Name.Replace("ц", "c");

            fileMp3Name = fileMp3Name.Replace("Ч", "Ch");
            fileMp3Name = fileMp3Name.Replace("ч", "ch");

            fileMp3Name = fileMp3Name.Replace("Ш", "Sh");
            fileMp3Name = fileMp3Name.Replace("ш", "sh");

            fileMp3Name = fileMp3Name.Replace("Щ", "Sh");
            fileMp3Name = fileMp3Name.Replace("щ", "sh");

            fileMp3Name = fileMp3Name.Replace("Ъ", "");
            fileMp3Name = fileMp3Name.Replace("ъ", "");

            fileMp3Name = fileMp3Name.Replace("Ы", "y");
            fileMp3Name = fileMp3Name.Replace("ы", "y");

            fileMp3Name = fileMp3Name.Replace("Ь", "");
            fileMp3Name = fileMp3Name.Replace("ь", "");

            fileMp3Name = fileMp3Name.Replace("Э", "E");
            fileMp3Name = fileMp3Name.Replace("э", "e");

            fileMp3Name = fileMp3Name.Replace("Ю", "Ju");
            fileMp3Name = fileMp3Name.Replace("ю", "ju");

            fileMp3Name = fileMp3Name.Replace("Я", "Ja");
            fileMp3Name = fileMp3Name.Replace("я", "ja");

            fileMp3Name = fileMp3Name.Replace("№", "N");

            return fileMp3Name;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    folderBrowserDialog1.SelectedPath = textBox1.Text;
                }
   

                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = folderBrowserDialog1.SelectedPath;
                }                
                else
                {
                    if (textBox1.Text == "")
                    {

                        MessageBox.Show("Please Select Folder !", "Error");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
            SaveSetting();
        }

        private string StupiddEncoding(string str)
        {
            str = str.Replace("Ŕ", "А");
            str = str.Replace("ŕ", "а");

            str = str.Replace("Á", "Б");
            str = str.Replace("á", "б");

            str = str.Replace("Â", "В");
            str = str.Replace("â", "в");

            str = str.Replace("Ă", "Г");
            str = str.Replace("ă", "г");

            str = str.Replace("Ä", "Д");
            str = str.Replace("ä", "д");

            str = str.Replace("Ĺ", "Е");
            str = str.Replace("ĺ", "е");

            str = str.Replace("¨", "Ё");
            str = str.Replace("¸", "ё");

            str = str.Replace("Ć", "Ж");
            str = str.Replace("ć", "ж");

            str = str.Replace("Ç", "З");
            str = str.Replace("ç", "з");

            str = str.Replace("Č", "И");
            str = str.Replace("č", "и");

            str = str.Replace("É", "Й");
            str = str.Replace("é", "й");

            str = str.Replace("Ę", "К");
            str = str.Replace("ę", "к");

            str = str.Replace("Ë", "Л");
            str = str.Replace("ë", "л");

            str = str.Replace("Ě", "М");
            str = str.Replace("ě", "м");

            str = str.Replace("Í", "Н");
            str = str.Replace("í", "н");

            str = str.Replace("Î", "O");
            str = str.Replace("î", "о");

            str = str.Replace("Ď", "П");
            str = str.Replace("ď", "п");

            str = str.Replace("Đ", "Р");
            str = str.Replace("đ", "р");

            str = str.Replace("Ń", "С");
            str = str.Replace("ń", "с");

            str = str.Replace("Ň", "Т");
            str = str.Replace("ň", "т");

            str = str.Replace("Ó", "У");
            str = str.Replace("ó", "у");

            str = str.Replace("Ô", "Ф");
            str = str.Replace("ô", "ф");

            str = str.Replace("Ő", "Х");
            str = str.Replace("ő", "х");

            str = str.Replace("Ö", "Ц");
            str = str.Replace("ö", "ц");

            str = str.Replace("×", "Ч");
            str = str.Replace("÷", "ч");

            str = str.Replace("Ř", "Ш");
            str = str.Replace("ř", "ш");

            str = str.Replace("Ů", "Щ");
            str = str.Replace("ů", "щ");

            str = str.Replace("Ú", "Ъ");
            str = str.Replace("ú", "ъ");

            str = str.Replace("Ű", "Ы");
            str = str.Replace("ű", "ы");

            str = str.Replace("Ü", "Ь");
            str = str.Replace("ü", "ь");

            str = str.Replace("Ý", "Э");
            str = str.Replace("ý", "э");

            str = str.Replace("Ţ", "Ю");
            str = str.Replace("ţ", "ю");

            str = str.Replace("ß", "Я");
            str = str.Replace("˙", "я");
            
            return str;


        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                comboBoxInput.Enabled = true;
                comboBoxOut.Enabled = true;

                buttonInput.Enabled = true;
                buttonOut.Enabled = true;
            }
            else
            {
                comboBoxInput.Enabled = false;
                comboBoxOut.Enabled = false;
                
                buttonInput.Enabled = false;
                buttonOut.Enabled = false;
            }
        }

        private void buttonInput_Click(object sender, EventArgs e)
        {
            this.comboBoxInput.DataSource = ConstructCodePageArray();
            this.comboBoxInput.DisplayMember = "DisplayName";
            this.comboBoxInput.ValueMember = "CodePageNum";

        }

        private void buttonOut_Click(object sender, EventArgs e)
        {
            this.comboBoxOut.DataSource = ConstructCodePageArray();
            this.comboBoxOut.DisplayMember = "DisplayName";
            this.comboBoxOut.ValueMember = "CodePageNum";      
        }

        private void loadSetting()
        {
            try
            {

                XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
                string Path = "seting.ini";
                AppSettings mySetting;

                if (File.Exists(Path))
                {

                    FileStream fs = File.OpenRead(Path);
                    
                    try
                    {
                        mySetting = (AppSettings)serializer.Deserialize(fs);
                    }
                    finally
                    {
                        fs.Close();
                    }
                }
                else
                {
                    mySetting = new AppSettings();
                }

                comboBoxInput.SelectedIndex = mySetting.Input;
                comboBoxOut.SelectedIndex = mySetting.Out;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

        }
        
        private void SaveSetting()
        {
            try
            {
                AppSettings mySetting = new AppSettings();

                mySetting.Input = comboBoxInput.SelectedIndex;
                mySetting.Out = comboBoxOut.SelectedIndex;

                XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
                
                
                string Path = "seting.ini";
                if (File.Exists(Path))
                {
                    File.Delete(Path);
                } 

                FileStream fs = File.OpenWrite(Path);
                XmlTextWriter writer = new XmlTextWriter(fs, System.Text.Encoding.UTF8);                
                writer.Formatting = Formatting.Indented;
                try
                {                    
                    serializer.Serialize(writer, mySetting);
                }
                finally
                {                    
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        public void RemoveFileSecurity(string fileName, string account, FileSystemRights rights, AccessControlType controlType)
        {

            // Get a FileSecurity object that represents the
            // current security settings.
            FileSecurity fSecurity = File.GetAccessControl(fileName);

            // Remove the FileSystemAccessRule from the security settings.
            fSecurity.RemoveAccessRule(new FileSystemAccessRule(account,
                rights, controlType));

            // Set the new access settings.
            File.SetAccessControl(fileName, fSecurity);

        }

        public void AddFileSecurity(string fileName, string account, FileSystemRights rights, AccessControlType controlType)
        {
            // Get a FileSecurity object that represents the
            // current security settings.
            FileSecurity fSecurity = File.GetAccessControl(fileName);

            // Add the FileSystemAccessRule to the security settings.
            fSecurity.AddAccessRule(new FileSystemAccessRule(account,
                rights, controlType));

            // Set the new access settings.
            File.SetAccessControl(fileName, fSecurity);

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 my = new AboutBox1();

            if (my.ShowDialog() == DialogResult.OK)
            {

            }
        }

    }
}