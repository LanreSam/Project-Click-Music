using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using WMPLib;
using System.Media;


namespace customSplashScreen
{
    public partial class Form2 : Form
    {
        //for opening disk tray
        [DllImport("winmm.dll", EntryPoint = "mciSendString", CharSet = CharSet.Ansi)]
        protected static extern int mciSendString(string IpstrCommand, StringBuilder IpstrReturnString, int uReturnLength, IntPtr hwndCallBack);


        public Form2()
        {
            InitializeComponent();
        }

        //global variables
        int StartIndex = 0;
        string[] FileName, FilePath; //Saves the name of songs and path to each songs.



        //cd tray code.
        #region cdTray
        public bool cdTray(bool open)
        {
            try
            {
                int ret = 0;
                switch (open)// open cd tray.
                {
                    case true:
                        ret = mciSendString("set cdaudio door open", null, 0, IntPtr.Zero);
                        return true;
                        break;
                    case false:
                        ret = mciSendString("set cdaudio door closed", null, 0, IntPtr.Zero);
                        return true;
                        break;
                    default:
                        ret = mciSendString("set cdaudio door open", null, 0, IntPtr.Zero);
                        return true;
                        break;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return true;
        }
        #endregion

        //side menu code.
        #region SIDE MENU
        public void hideSubMenu() //method to hide the subMenu panel
        {
            if (MediaSubMenu.Visible == true)
                MediaSubMenu.Visible = false;
            if (Playlist_SubMenu.Visible == true)
                Playlist_SubMenu.Visible = false;
            if (Tools_SubMenu.Visible == true)
                Tools_SubMenu.Visible = false;
        }

        public void showSubMenu(Panel SubMenu)
        {
            if (SubMenu.Visible == false)
            {
                hideSubMenu();
                SubMenu.Visible = true;
            }
            else
            {
                SubMenu.Visible = false;
            }
        }
        #endregion

        //form 2 load
        #region form2Load
        private void Form2_Load(object sender, EventArgs e)
        {
            StartIndex = 0;
            StopPlayer();
        }
        public void StopPlayer()
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }
        #endregion

        //media.
        #region media
        private void media_btn_Click(object sender, EventArgs e)
        {
            if(SideMenu.Width == 274)
            {
                showSubMenu(MediaSubMenu);
            }
            else
            {
                SideMenu.Width = 274;
                showSubMenu(MediaSubMenu);
            }

        }

        private void OpenFiles_Click(object sender, EventArgs e)
        {
            hideSubMenu();
            try
            {
                MediaItems.Visible = false;
                playlistPanel.Visible = false;
                StartIndex = 0;

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = true;
                ofd.Filter = "(mp3, wav, mp4,mov,wmv,mpg, avi,3gp,flv)|*.mp3;*.wav;*.mp4;*.mov;*.wmv;*.mpg;*.avi;*.3gp;*.flv|all files|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (MediaItems.Items.Count != 0)
                    {
                        MediaItems.Items.Clear();
                    }

                    FileName = ofd.SafeFileNames;
                    FilePath = ofd.FileNames;
                    for (int i = 0; i <= FileName.Length - 1; i++)
                    {
                        NameOfFile.Text = ofd.FileName;
                        MediaItems.Items.Add(FileName[i]);
                    }
                    MediaItems.SelectedIndex = 0;
                    StartIndex = 0;
                    playFile(StartIndex);

                    play_btn.Visible = false;
                    pause_btn.Visible = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            } 
        }


        


        //plays file in list view
        public void playFile(int playListIndex)
        {
            if (MediaItems.Items.Count <= 0)
            {
                return;
            }
            if (playListIndex < 0)
            {
                return;
            }
            axWindowsMediaPlayer1.settings.autoStart = true;
            axWindowsMediaPlayer1.Visible = true;
            axWindowsMediaPlayer1.URL = FilePath[playListIndex];
            axWindowsMediaPlayer1.Ctlcontrols.next();
            axWindowsMediaPlayer1.Ctlcontrols.play();
            
        }

        //open folder.
        private void openFolder_Click(object sender, EventArgs e)
        {
            hideSubMenu();
            try
            {
                MediaItems.Visible = false;
                playlistPanel.Visible = false;

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "(mp3, wav, mp4,mov,wmv,mpg, avi,3gp,flv)|*.mp3;*.wav;*.mp4;*.mov;*.wmv;*.mpg;*.avi;*.3gp;*.flv|all files|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (MediaItems.Items.Count != 0)
                    {
                        MediaItems.Items.Clear();
                    }

                    FileName = ofd.SafeFileNames;
                    FilePath = ofd.FileNames;
                    for (int i = 0; i <= FileName.Length - 1; i++)
                    {
                        NameOfFile.Text = ofd.FileName;
                        MediaItems.Items.Add(FileName[i]);
                    }
                    MediaItems.SelectedIndex = 0;
                    StartIndex = 0;
                    playFile(StartIndex);

                    play_btn.Visible = false;
                    pause_btn.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void OpenDisk_btn_Click(object sender, EventArgs e)
        {
            hideSubMenu();

            MediaItems.Visible = false;
            playlistPanel.Visible = false;

            OpenDisplay(new DiskTray());
            cdTray(true);
        }
        #endregion

        
        //changes the forms in display panel
        private Form activeForm = null;
        private void OpenDisplay(Form display)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = display;
            display.TopLevel = false;
            display.FormBorderStyle = FormBorderStyle.None;
            display.Dock = DockStyle.Fill;
            DisplayPanel.Controls.Add(display);
            DisplayPanel.Tag = display;
            display.BringToFront();
            display.Show();
        }

        //exit application
        private void button5_Click(object sender, EventArgs e)
        {
            string message = "Do you want to exit application?";
            string title = "Close application";  
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
            if(result == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
                return;
            }
        }

        //menu slider
        private void MenuSlider_Click(object sender, EventArgs e)
        {
            if (SideMenu.Width == 274)
            {
                SideMenu.Width = 0;
                hideSubMenu();
            }
            else
            {
                SideMenu.Width = 274;
            }
        }

        //playlist manager
        private void Playlist_btn_Click_1(object sender, EventArgs e)
        {
            if (SideMenu.Width == 274)
            {
                showSubMenu(Playlist_SubMenu);
            }
            else
            {
                SideMenu.Width = 274;
                showSubMenu(Playlist_SubMenu);
            }

        }

        //tools
        private void Tools_btn_Click_1(object sender, EventArgs e)
        {
            if (SideMenu.Width == 274)
            {
                showSubMenu(Tools_SubMenu);
            }
            else
            {
                SideMenu.Width = 274;
                showSubMenu(Tools_SubMenu);
            }
        }

        //developer
        private void Help_btn_Click_1(object sender, EventArgs e)
        {
            hideSubMenu();

            //hides playlist panel.
            playlistPanel.Visible = false;
            MediaItems.Visible = false;

            OpenDisplay(new helpForm());
        }

        //playlist
        private void button9_Click_1(object sender, EventArgs e)
        {
            hideSubMenu();
            playlistPanel.Visible = true;
            MediaItems.Visible = true;
        }

        //now playing
        private void button8_Click_1(object sender, EventArgs e)
        {
            hideSubMenu();
            playlistPanel.Visible = false;
            MediaItems.Visible = false;
            axWindowsMediaPlayer1.Visible = true;
        }

        //new playlist
        private void NewPlaylist_Click_1(object sender, EventArgs e)
        {
            hideSubMenu();
            //hides playlist panel.
            playlistPanel.Visible = false;
            MediaItems.Visible = false;


            StartIndex = 0;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (MediaItems.Items.Count != 0)
                {
                    MediaItems.Items.Clear();
                }

                FileName = ofd.SafeFileNames;
                FilePath = ofd.FileNames;
                for (int i = 0; i <= FileName.Length - 1; i++)
                {
                    NameOfFile.Text = ofd.FileName;
                    MediaItems.Items.Add(FileName[i]);
                }
                MediaItems.SelectedIndex = 0;
                StartIndex = 0;
                playFile(0);

                play_btn.Visible = false;
                pause_btn.Visible = true;
            }
        }

        //media converter......
        private void button14_Click_1(object sender, EventArgs e)
        {
            hideSubMenu();

            //hides playlist panel.
            playlistPanel.Visible = false;
            MediaItems.Visible = false;
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                   

                    OpenDisplay(new Converting());
                    try
                    {
                        var convert = new NReco.VideoConverter.FFMpegConverter();
                        convert.ConvertMedia(ofd.FileName, @"C:\Users\Olanrewaju\Desktop\Converted.mp4", NReco.VideoConverter.Format.mp4);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex);
                    }

                    MessageBox.Show("Convert complete");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        //exit application/
        private void pictureBox10_Click(object sender, EventArgs e)
        {
            string message = "Do you want to exit application?";
            string title = "Close application";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
                return;
            }
        }

        //maximize button
        private void pictureBox11_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            restore.Visible = true;
            maximize.Visible = false;
        }

        //restore button
        private void restore_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            restore.Visible = false;
            maximize.Visible = true;
        }

        //minimize button
        private void minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //Volume button code.
        private void VolumeSlider_ValueChanged(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = VolumeSlider.Value;
            volumePercent.Text = VolumeSlider.Value.ToString();
        }

        //select media from playlist panel.
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            MediaItems.Visible = false;
            StartIndex = 0;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (MediaItems.Items.Count != 0)
                {
                    MediaItems.Items.Clear();
                }

                FileName = ofd.SafeFileNames;
                FilePath = ofd.FileNames;
                for (int i = 0; i <= FileName.Length - 1; i++)
                {
                    NameOfFile.Text = ofd.FileName;
                    MediaItems.Items.Add(FileName[i]);
                }
                MediaItems.SelectedIndex = 0;
                StartIndex = 0;
                playFile(0);

                play_btn.Visible = false;
                pause_btn.Visible = true;
            }
        }

        //play button.
        private void play_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (MediaItems.Items.Count.Equals(0))
                {
                    MessageBox.Show("Please select a media file");
                    MediaItems.Visible = false;
                    playlistPanel.Visible = false;

                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "(mp3, wav, mp4,mov,wmv,mpg, avi,3gp,flv)|*.mp3;*.wav;*.mp4;*.mov;*.wmv;*.mpg;*.avi;*.3gp;*.flv|all files|*.*";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        if (MediaItems.Items.Count != 0)
                        {
                            MediaItems.Items.Clear();
                        }

                        FileName = ofd.SafeFileNames;
                        FilePath = ofd.FileNames;
                        for (int i = 0; i <= FileName.Length - 1; i++)
                        {
                            NameOfFile.Text = ofd.FileName;
                            MediaItems.Items.Add(FileName[i]);
                        }
                        MediaItems.SelectedIndex = 0;
                        StartIndex = 0;
                        playFile(StartIndex);

                        //changes visibility of pause and play buttons
                        play_btn.Visible = false;
                        pause_btn.Visible = true;
                        axWindowsMediaPlayer1.Visible = true;
                        axWindowsMediaPlayer1.Ctlcontrols.play();
                    }
                }
                else //if (MediaItems.Items.Count.Equals(MediaItems.Items))
                {
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                    axWindowsMediaPlayer1.Visible = true;
                    play_btn.Visible = false;
                    pause_btn.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        //pause button
        private void pause_btn_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.pause();

            //changes visibility of pause and play buttons
            play_btn.Visible = true;
            pause_btn.Visible = false;
        }

        //previous button
        private void Previous_btn_Click(object sender, EventArgs e)
        {
            if(StartIndex > 0)
            {
                BeginInvoke(new Action(() => {
                    MediaItems.SelectedIndex = MediaItems.SelectedIndex - 1;
                }));
            }
        }

        //next button.
        private void Next_btn_Click(object sender, EventArgs e)
        {
            try
            {
                MediaItems.SelectedIndex = MediaItems.SelectedIndex + 1;
            }
            catch(Exception ex)
            {
                MessageBox.Show("End of Playlist", null);
                axWindowsMediaPlayer1.Ctlcontrols.stop();
                axWindowsMediaPlayer1.Visible = false;
                playlistPanel.Visible = false;
                MediaItems.Visible = false;

                play_btn.Visible = true;
                pause_btn.Visible = false;
            }
        }

        //stop button.
        private void Stop_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            BeginInvoke(new Action(() => {
                MediaItems.SelectedIndex = MediaItems.SelectedIndex - 1;
                axWindowsMediaPlayer1.Ctlcontrols.stop();
                axWindowsMediaPlayer1.Visible = false;
                
            }));

            

            //changes visibility of pause and play buttons
            play_btn.Visible = true;
            pause_btn.Visible = false;
            playlistPanel.Visible = false;
            MediaItems.Visible = false;  
        }

        //track time code.
        private void trackTime_Tick(object sender, EventArgs e)
        {
            bunifuCustomLabel5.Text = axWindowsMediaPlayer1.Ctlcontrols.currentPositionString;
            bunifuCustomLabel6.Text = axWindowsMediaPlayer1.Ctlcontrols.currentItem.durationString.ToString();

            if(axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                MediatimeLine.Value = (int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
            }
        }

        //media playstate changed.
        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            //this continues to play the next song if there is one availabe.
            if (e.newState == 1)
            {
                if (MediaItems.SelectedIndex != MediaItems.Items.Count - 1)
                {
                    BeginInvoke(new Action(() => {
                        MediaItems.SelectedIndex = MediaItems.SelectedIndex + 1;
                    }));
                }

                else
                {
                    //all these checks if there are no more songs to play, then stop the player.
                    axWindowsMediaPlayer1.Ctlcontrols.stop();
                    axWindowsMediaPlayer1.Visible = false;
                    playlistPanel.Visible = false;
                    MediaItems.Visible = false;
                    play_btn.Visible = true;
                    pause_btn.Visible = false;
                }
            }

            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                MediatimeLine.MaximumValue = (int)axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration; //check
                trackTime.Start();
            }
            else if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                trackTime.Stop();
            }
            else if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                axWindowsMediaPlayer1.Ctlcontrols.stop();
                trackTime.Stop();
                MediatimeLine.Value = 0;
            }
        }


        //selected index for listbox.
        private void MediaItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            StartIndex = MediaItems.SelectedIndex;
            playFile(StartIndex);
            NameOfFile.Text = MediaItems.Text;
        }


        //click event for media player.
        private void axWindowsMediaPlayer1_ClickEvent(object sender, AxWMPLib._WMPOCXEvents_ClickEvent e)
        {
            if (playlistPanel.Visible == true)
            {
                playlistPanel.Visible = false;

            }
            else
            {
                MediaItems.Visible = true;
                playlistPanel.Visible = true;
            }
        }


        #region tooltips.
        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.SetToolTip(pictureBox3, "Select media");
        }

        private void Previous_btn_MouseHover(object sender, EventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.SetToolTip(Previous_btn, "Previous");
        }

        private void Next_btn_MouseHover(object sender, EventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.SetToolTip(Next_btn, "next");
        }

        private void play_btn_MouseHover(object sender, EventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.SetToolTip(Playlist_btn, "Play");
        }

        private void pause_btn_MouseHover(object sender, EventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.SetToolTip(pause_btn, "Pause");
        }

        private void Stop_MouseHover(object sender, EventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.SetToolTip(Stop, "Stop");
        }

        private void repeat_btn_MouseHover(object sender, EventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.SetToolTip(repeat_btn, "Repeat");
        }

        private void minimize_MouseHover(object sender, EventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.SetToolTip(minimize, "Miniimize");
        }

        private void close_MouseHover(object sender, EventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.SetToolTip(close, "Close");
        }

        private void restore_MouseHover(object sender, EventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.SetToolTip(restore, "Restore");
        }

        private void maximize_MouseHover(object sender, EventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.SetToolTip(maximize, "Maximize");
        }

        private void MenuSlider_MouseHover(object sender, EventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.SetToolTip(MenuSlider, "Menu");
        }
        #endregion

        //top menu panel code.
        private void TopMenuPanel_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
                restore.Visible = true;
                maximize.Visible = false;
            }
            else if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                restore.Visible = false;
                maximize.Visible = true;
            }

        }


        private void playListPanelClose_Click(object sender, EventArgs e)
        {
            playlistPanel.Hide();
        }

        //media track bar value changed.
        private void MediatimeLine_ValueChanged(object sender, EventArgs e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition = MediatimeLine.Value;
            }
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition = MediatimeLine.Value;
            }
            else
            {
                MediatimeLine.Value = 0;
            }
        }


        //shuffle button code
        //not yet completed.
        private void shuffleBtn_Click(object sender, EventArgs e)
        {
         
        }

        //repeat button
        #region repeat_btn
        private void repeat_btn_Click(object sender, EventArgs e)
        {
            if (repeat_btn.Visible == true)
            {
                repeat_btn.Visible = false;
                repeat_btn2.Visible = true;
                axWindowsMediaPlayer1.settings.setMode("loop", true);
            }
            
        }

        private void repeat_btn2_Click(object sender, EventArgs e)
        {
            if (repeat_btn2.Visible == true)
            {
                repeat_btn2.Visible = false;
                repeat_btn.Visible = true;
                axWindowsMediaPlayer1.settings.setMode("loop", false);
            }
        }

        private void repeat_btn2_MouseHover(object sender, EventArgs e)
        {
            ToolTip tp = new ToolTip();
            tp.SetToolTip(repeat_btn2, "Repeat active");
        }
        #endregion


        //keyboard arrow action performed codes.
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //code to increase volume when up arrow button is clicked
            if (keyData == Keys.Up)
            {
                int newValue = 5;
                if (VolumeSlider.Value == 100)
                {
                    newValue = 0;
                }

                VolumeSlider.Value += newValue;
                axWindowsMediaPlayer1.settings.volume = VolumeSlider.Value;
                volumePercent.Text = VolumeSlider.Value.ToString();
                
                return true; 
            }
      
            //code to reduce volume when down arrow button is clicked
            if (keyData == Keys.Down)
            {
                int newValue = 5;
                if (VolumeSlider.Value == 0)
                {
                    newValue = 0;
                }
                VolumeSlider.Value -= newValue;
                axWindowsMediaPlayer1.settings.volume = VolumeSlider.Value;
                volumePercent.Text = VolumeSlider.Value.ToString();
                
                return true;
            }
            if (keyData == Keys.Right)
            {
                int newValue = 10;
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition += newValue;
                return true;
            }
            if (keyData == Keys.Left)
            {
                int newValue = 5;
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition -= newValue;
                return true;
            }
            if (keyData == Keys.Space)
            {
                if(play_btn.Visible == false)
                {
                    axWindowsMediaPlayer1.Ctlcontrols.pause();
                    play_btn.Visible = true;
                    pause_btn.Visible = false;
                }
                else if (play_btn.Visible == true)
                {
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                    play_btn.Visible = false;
                    pause_btn.Visible = true;
                }

                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        //Code amendment
    }
}