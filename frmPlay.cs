using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BTLL
{
    public partial class frmPlay : Form
    {
        int Crong = 30;
        int Ccao = 30;
        int KthuocR = 15;
        int KthuocC = 15;
        string path = @"Caro\";
        private List<Player> player;
        private int currentplayer;
        private List<List<Button>> maxtrix;
        private Image anh;
        private string s , C_Anh;
        MemoryStream ms = new MemoryStream();
        frm2Player frmparent;
        string hoten1, hoten2 ;
        int minute = 0, second = -1, dem = 0;

        public frmPlay()
        {
            InitializeComponent();
        }

        public frmPlay(frm2Player f)
        {
            InitializeComponent();
            this.frmparent = f;
        }

        public string HoTen1
        {
            get { return hoten1; }
            set { hoten1 = value; }
        }

        public string HoTen2
        {
            get { return hoten1; }
            set { hoten2 = value; }
        }

        private void frmPlay_Load(object sender, EventArgs e)
        {
            this.IsMdiContainer = true;
            TaoBanCo();
            this.player = new List<Player>()
            {
                new Player(hoten1,Image.FromFile(path + 0 + ".PNG")),
                new Player(hoten2,Image.FromFile(path + 1 + ".PNG")),

            };
            currentplayer = 0;
            DoiNguoiChoi();
        }

        void TaoBanCo()
        {
            maxtrix = new List<List<Button>>();
            Button btCu = new Button()
            {
                Width = 0,
                Location = new Point(0, 0)
            };
            for (int i = 0; i < KthuocC; i++)
            {
                maxtrix.Add(new List<Button>());
                for (int j = 0; j < KthuocR; j++)
                {
                    Button btn = new Button()
                    {
                        Width = Crong,
                        Height = Ccao,
                        Location = new Point(btCu.Location.X + btCu.Width, btCu.Location.Y),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Tag = i.ToString(), 
                    };
                    maxtrix[i].Add(btn);
                    btn.Click += Btn_Click;
                    pBanCo.Controls.Add(btn);
                    btCu = btn;
                }
                btCu.Location = new Point(0, btCu.Location.Y + Ccao);
                btCu.Width = 0;
                btCu.Height = 0;
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            TG.Enabled = true;
            Button btn = sender as Button;
            if (btn.BackgroundImage != null) return;
            btn.BackgroundImage = player[currentplayer].Mark;
            HinhDaiDien(btn);
            DoiNguoiChoi();
            if (isEndGame(btn))
            {
                EndGame();
            }
        }

        private void EndGame()
        {
            dem++;
            TG.Stop();
            Add(dem, TenNguoiThang(), lbTG.Text);
            string tBao = "CHÚC MỪNG CHIẾN THẮNG PLAYER " + TenNguoiThang().ToUpper();
            MessageBox.Show(tBao, "CHÚC MỪNG", MessageBoxButtons.OK);
            var d = MessageBox.Show("CÁC BẠN CÓ MUỐN CHƠI LẠI HONG!!!", "THÔNG BÁO", MessageBoxButtons.YesNo);
            if (d == DialogResult.Yes)
            {                 
                TG.Enabled = false;
                pBanCo.Controls.Clear();
                TaoBanCo();
                lbTG.Text = null;
                second = -1;
                minute = 0;
            }
            else this.Close();
        }

        string TenNguoiThang()
        {
            if (ChuyenAnh(anh) == ChuyenAnh(Image.FromFile(path + 0 + ".PNG")))
            {
                s = player[0].Name;
            }
            else if (ChuyenAnh(anh) == ChuyenAnh(Image.FromFile(path + 1 + ".PNG")))
            {
                s = player[1].Name;
            }
            return s;
        }

        private bool isEndGame(Button btn)
        {
            return kiemtrachieungang(btn) || kiemtrachieudoc(btn) || Duongcheochinh(btn) || Duongcheophu(btn);
        }

        private Point Vitributton(Button btn)
        {
            int chieuthang = Convert.ToInt32(btn.Tag); // chiều y
            int chieungang = maxtrix[chieuthang].IndexOf(btn); // chiều x (IndexOf dùng để kiểm tra button có trong list thì trả về vị trí xuất hiện của btn)
            Point point = new Point(chieungang, chieuthang); // tạo vị trí cho button
            return point;
        }

        private bool kiemtrachieungang(Button btn)
        {
            Point point = Vitributton(btn);
            int demtrai = 0;
            for (int i = point.X; i >= 0; i--)
            {
                if (maxtrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                {
                    demtrai++;
                }
                else
                    break;
            }
            int demphai = 0;
            for (int i = point.X + 1; i < KthuocR; i++)
            {
                if (maxtrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                {
                    demphai++;
                }
                else
                    break;
            }
            if( demtrai + demphai >= 5)
            {
                if (ChuyenAnh(btn.BackgroundImage) == ChuyenAnh(Image.FromFile(path + 0 + ".PNG")))
                {
                    anh = Image.FromFile(path + 0 + ".PNG");
                }
                else if (ChuyenAnh(btn.BackgroundImage) == ChuyenAnh(Image.FromFile(path + 1 + ".PNG"))) anh = Image.FromFile(path + 1 + ".PNG");
            }
            return demtrai + demphai >= 5;
        }

        private bool kiemtrachieudoc(Button btn)
        {
            Point point = Vitributton(btn);
            int demtren = 0;
            for (int i = point.Y; i >= 0; i--)
            {
                if (maxtrix[i][point.X].BackgroundImage == btn.BackgroundImage)
                {
                    demtren++;
                }
                else
                    break;
            }
            int demduoi = 0;
            for (int i = point.Y + 1; i < KthuocC; i++)
            {
                if (maxtrix[i][point.X].BackgroundImage == btn.BackgroundImage)
                {
                    demduoi++;
                }
                else
                    break;
            }
            if (demtren + demduoi >= 5)
            {
                if (ChuyenAnh(btn.BackgroundImage) == ChuyenAnh(Image.FromFile(path + 0 + ".PNG")))
                {
                    anh = Image.FromFile(path + 0 + ".PNG");
                }
                else if (ChuyenAnh(btn.BackgroundImage) == ChuyenAnh(Image.FromFile(path + 1 + ".PNG"))) anh = Image.FromFile(path + 1 + ".PNG");
            }
            return demtren + demduoi >= 5;
        }

        private bool Duongcheochinh(Button btn)
        {
            Point point = Vitributton(btn);
            int demtren = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X - i < 0 || point.Y - i < 0) break; // x, y không ra khỏi bàn cờ
                if (maxtrix[point.Y - i][point.X - i].BackgroundImage == btn.BackgroundImage)
                {
                    demtren++;
                }
                else
                    break;
            }
            int demduoi = 0;
            for (int i = 1; i <= KthuocR - point.X; i++)
            {
                if (point.Y + i >= KthuocC|| point.X + i >= KthuocR) break;
                if (maxtrix[point.Y + i][point.X + i].BackgroundImage == btn.BackgroundImage)
                {
                    demduoi++;
                }
                else
                    break;
            }
            if (demtren + demduoi >= 5)
            {
                if (ChuyenAnh(btn.BackgroundImage) == ChuyenAnh(Image.FromFile(path + 0 + ".PNG")))
                {
                    anh = Image.FromFile(path + 0 + ".PNG");
                }
                else if (ChuyenAnh(btn.BackgroundImage) == ChuyenAnh(Image.FromFile(path + 1 + ".PNG"))) anh = Image.FromFile(path + 1 + ".PNG");
            }
            return demtren + demduoi >= 5;
        }

        private bool Duongcheophu(Button btn)
        {
            Point point = Vitributton(btn);
            int demtren = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X + i > KthuocR || point.Y - i < 0)
                    break;
                if (maxtrix[point.Y - i][point.X + i].BackgroundImage == btn.BackgroundImage)
                {
                    demtren++;
                }
                else
                    break;
            }
            int demduoi = 0;
            for (int i = 1 ; i <= (KthuocR - point.X) ; i++)
            {
                if (point.Y + i >= KthuocC || point.X - i < 0)
                    break;
                if (maxtrix[point.Y + i][point.X - i].BackgroundImage == btn.BackgroundImage)
                {
                    demduoi++;
                }
                else
                    break;
            }
            if (demtren + demduoi >= 5)
            {
                if (ChuyenAnh(btn.BackgroundImage) == ChuyenAnh(Image.FromFile(path + 0 + ".PNG")))
                {
                    anh = Image.FromFile(path + 0 + ".PNG");
                }
                else if (ChuyenAnh(btn.BackgroundImage) == ChuyenAnh(Image.FromFile(path + 1 + ".PNG"))) anh = Image.FromFile(path + 1 + ".PNG");
            }
            return demtren + demduoi >= 5;
        }

        void DoiNguoiChoi()
        {
            txtName.Text = player[currentplayer].Name;
            picMark.Image = player[currentplayer].Mark;
        }

        void HinhDaiDien(Button btn)
        {
            currentplayer = currentplayer == 1 ? 0 : 1;
            picMark.Image = player[currentplayer].Mark;
        }

        string ChuyenAnh(Image anh)
        {
            anh.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            C_Anh = Convert.ToBase64String(ms.ToArray()); ms.Position = 0;
            return C_Anh;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dem = 0;
            pBanCo.Controls.Clear();
            listLichSuChoi.Items.Clear();
            TaoBanCo();
            TG.Enabled = false;
            lbTG.Text = null;
            second = -1;
            minute = 0;
        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dem = 0;
            pBanCo.Controls.Clear();
            listLichSuChoi.Items.Clear();
            TaoBanCo();
            TG.Enabled = false;
            lbTG.Text = null;
            second = -1;
            minute = 0;
        }

        private void closeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var d = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông Báo", MessageBoxButtons.YesNo);
            if (d == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Help frmH = new Help();
            frmH.Show();
        }

        private void frmPlay_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.frmparent.Close();
        }

        private void TG_Tick(object sender, EventArgs e)
        {
            second++;
            lbTG.Text = minute.ToString() + ":" + second.ToString();
            if (second == 59)
            {
                second = -1;
                minute++;
            }
            if(minute == 3 && second == 0)
            { 
                dem = 0;
                pBanCo.Controls.Clear();
                listLichSuChoi.Items.Clear();
                TaoBanCo();
                TG.Enabled = false;
                lbTG.Text = null;
                second = -1;
                minute = 0;
                MessageBox.Show("Đã quá thời gian cho 1 ván các bạn vui lòng chơi lại ván mới nha!!!");
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help frmH = new Help();
            frmH.Show();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var d = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông Báo", MessageBoxButtons.YesNo);
            if (d == DialogResult.Yes)
            {
                this.Close();
            }
        }

        public void Add(int dem, string Name, string TG )
        {
            ListViewItem item = new ListViewItem();
            item.Text = dem.ToString();
            item.ImageIndex = new Random().Next(0, 1);
            item.SubItems.Add(Name);
            item.SubItems.Add(TG);
            listLichSuChoi.Items.Add(item);
        }

    }

}
