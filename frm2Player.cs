using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
namespace BTLL
{
    public partial class frm2Player : Form
    {
        public frm2Player()
        {
            InitializeComponent();
        }

        private void frm2Player_Load(object sender, EventArgs e)
        {
            SoundPlayer sp = new SoundPlayer();
            sp.SoundLocation = @".\AnyConv.com__game-music-loop-2-144037.wav";
            sp.PlayLooping();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            SoundPlayer sp = new SoundPlayer();
            sp.SoundLocation = @".\AnyConv.com__game-music-loop-2-144037.wav";
            sp.Stop();
            btnMute.Visible = true;
            btnPlay.Visible = false;
        }

        private void btnMute_Click(object sender, EventArgs e)
        {
            SoundPlayer sp = new SoundPlayer();
            sp.SoundLocation = @".\AnyConv.com__game-music-loop-2-144037.wav";
            sp.PlayLooping();
            btnMute.Visible = false;
            btnPlay.Visible = true;
        }

        private void btnChoi_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName1.Text.Trim() != string.Empty && txtName2.Text.Trim() != string.Empty)
                {
                    frmPlay frm = new frmPlay(this);
                    frm.HoTen1 = txtName1.Text;
                    frm.HoTen2 = txtName2.Text;
                    frm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập tên đầy đủ người chơi!!!");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Vui lòng nhập đúng dữ liệu!!");
            }
        }
    }
}
