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
using System.Diagnostics;

namespace Notepad
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string filename = "";
        public Form1(string filename)
        {
            InitializeComponent();
            if (filename != null)
            {
                this.filename = filename;
                OpenFile();
            }
        }
        private void 新建NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtBox.Modified == true)
            {
                DialogResult dr = MessageBox.Show("文件发生变化，是否更改保存？", "注意", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    保存SToolStripMenuItem_Click(sender, e);
                    return;
                }
                else if (dr == DialogResult.Cancel)
                {
                    return;
                }
                txtBox.Clear();
                this.Text = "NewNotepad";
            }
            else
            {
                txtBox.Clear();
                this.Text = "NewNotepad";
            }
        }
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog.FileName;
                OpenFile();
            }
        }
        protected void OpenFile()
        {
            try
            {
                txtBox.Clear();
                txtBox.Text = File.ReadAllText(filename);
            }
            catch
            { MessageBox.Show("Error!"); }
        }
        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                StreamWriter sw = File.AppendText(Application.ExecutablePath);
                sw.Write(txtBox.Text);
                sw.Dispose();
            }
            catch
            {
                SaveFileDialog sf = new SaveFileDialog();
                sf.DefaultExt = "*.txt";
                sf.Filter = "文本文档(.txt)|*.txt";
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter sw = File.AppendText(sf.FileName);
                    sw.Write(txtBox.Text);
                    sw.Dispose();
                }
            }
        }
        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name;
            //SaveFileDialog类
            SaveFileDialog save = new SaveFileDialog();
            //过滤器
            save.Filter = "*.txt|*.TXT|(*.*)|*.*";
            //显示
            if (save.ShowDialog() == DialogResult.OK)
            {
                name = save.FileName;
                FileInfo info = new FileInfo(name);
                //info.Delete();
                StreamWriter writer = info.CreateText();
                writer.Write(txtBox.Text);
                writer.Close();
            }
        }
        private void 页面设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //弹出页面设置界面
            pageSetupDialog.Document = printDocument;
            pageSetupDialog.ShowDialog();
        }
        private void 打印PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //显示允许用户选择打印机的选项及其它打印选项的对话框
            this.printDialog.Document = this.printDocument;
            this.printDialog.PrinterSettings = this.pageSetupDialog.PrinterSettings;
            //向打印机发送打印指令
            if (this.printDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.printDocument.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误信息！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            剪切ToolStripMenuItem.Enabled = txtBox.Modified;
            if (txtBox.SelectedText == "")
            {
                剪切ToolStripMenuItem.Enabled = false;
                复制ToolStripMenuItem.Enabled = false;
                删除ToolStripMenuItem.Enabled = false;
            }
            else
            {
                剪切ToolStripMenuItem.Enabled = true;
                复制ToolStripMenuItem.Enabled = true;
                删除ToolStripMenuItem.Enabled = true;
            }
            if (txtBox.Text == "")
            {
                查找ToolStripMenuItem.Enabled = false;
                查找下一个ToolStripMenuItem.Enabled = false;
                查找上一个ToolStripMenuItem.Enabled = false;
                替换ToolStripMenuItem.Enabled = false;
            }
            else
            {
                查找ToolStripMenuItem.Enabled = true;
                查找下一个ToolStripMenuItem.Enabled = true;
                查找上一个ToolStripMenuItem.Enabled = true;
                替换ToolStripMenuItem.Enabled = true;
            }
            if (Clipboard.GetText() == "")
                粘贴ToolStripMenuItem.Enabled = false;
            else
                粘贴ToolStripMenuItem.Enabled = true;
        }
        private void 撤销ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtBox.CanUndo)
            {
                txtBox.Undo();
                txtBox.ClearUndo();
            }
        }
        private void 剪切ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtBox.Cut();
        }
        private void 复制CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtBox.Copy();
        }
        private void 粘贴PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtBox.Paste();
        }
        private void 删除lToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtBox.SelectedText = string.Empty;
        }
        TextBox txtInput = new TextBox()
        {
            Font = new Font("宋体", 10)
        };
        TextBox txtInputReplace = new TextBox()
        {
            Font = new Font("宋体", 10)
        };
        Label lblSearch = new Label
        {
            Text = "查找内容：",
            Size = new Size(65, 25),
            Location = new Point(5, 22)
        };
        Label lblDirection = new Label
        {
            Text = "查找方向：",
            Size = new Size(65, 25),
            Location = new Point(5, 58)
        };
        Button FindNext = new Button
        {
            Name = "btnFindNext",
            Text = "查找下一项",
            Size = new Size(80, 25),
            Location = new Point(265, 15)
        };
        Button Cancel = new Button
        {
            Name = "btnCancel",
            Text = "取消",
            Size = new Size(80, 25),
            Location = new Point(265, 50)
        };
        RadioButton down = new RadioButton
        {
            Name = "radDown",
            Text = "向下",
            Size = new Size(55, 25),
            Location = new Point(70, 53),
            Checked = true
        };
        RadioButton upward = new RadioButton
        {
            Name = "radUpward",
            Text = "向上",
            Size = new Size(55, 25),
            Location = new Point(140, 53),
            Checked = false
        };
        new Form FindForm = new Form
        {
            Text = "查找文本",
            FormBorderStyle = FormBorderStyle.FixedSingle,
            MaximizeBox = false,
            MinimizeBox = false
        };
        private void 查找ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //显示查找对话框
            txtInput.Size = new Size(190, 33);
            txtInput.Location = new Point(70, 15);
            txtInput.Multiline = true;

            FindNext.Click += new EventHandler(Direction_Click);
            //FindNext.Click += new EventHandler(Visible_Click);

            Cancel.Click += new EventHandler(Cancel_Click);

            FindForm.Controls.Add(lblSearch);
            FindForm.Controls.Add(lblDirection);
            FindForm.Controls.Add(txtInput);
            FindForm.Controls.Add(down);
            FindForm.Controls.Add(upward);
            FindForm.Controls.Add(FindNext);
            FindForm.Controls.Add(Cancel);
            FindForm.Top = this.Top + 50;
            FindForm.Left = this.Left + 50;
            FindForm.Height = 120;
            FindForm.Width = 380;
            FindForm.StartPosition = FormStartPosition.CenterParent;
            FindForm.ShowDialog();
        }
        private void Visible_Click(object sender, EventArgs e)
        {
            FindForm.Visible = false;
        }
        private void Cancel_Click(object sender, EventArgs e)
        {
            //关闭对话框
            FindForm.Close();
            ReplaceForm.Close();
        }
        private void Direction_Click(object sender, EventArgs e)
        {
            //选择字符查找方向
            if (down.Checked == true)
            {
                Find_Click(sender, e);
            }
            else if (upward.Checked == true)
            {
                FindLast_Click(sender, e);
            }
        }
        int nextPosition, firstPosition;
        string word;
        Boolean IF = false;
        private void Find_Click(object sender, EventArgs e)
        {
            txtBox.Focus();
            FindWords(txtInput.Text);
        }
        private void FindWords(string words)
        {
            //向下查找字符
            if (nextPosition >= txtBox.Text.Length)
                nextPosition = 0;
            firstPosition = txtBox.Text.IndexOf(words, nextPosition);
            if (firstPosition == -1)
                nextPosition = 0;
            else
            {
                txtBox.Select(firstPosition, words.Length);
                nextPosition = firstPosition + 1;
            }
            word = words;
            IF = true;
        }
        private void 查找下一个ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //查找下一项，如果未查找过，则显示查找对话框
            down.Checked = true;
            upward.Checked = false;
            try
            {
                FindWords(word);
            }
            catch
            {
                查找ToolStripMenuItem_Click(sender, e);
            }
        }
        private void FindLast_Click(object sender, EventArgs e)
        {
            txtBox.Focus();
            FindWordsLast(txtInput.Text);
        }
        private void FindWordsLast(string words)
        {
            //向上查找字符
            if (IF == false)
                nextPosition = txtBox.Text.Length;
            if (nextPosition < 0)
                nextPosition = txtBox.Text.Length;

            firstPosition = txtBox.Text.LastIndexOf(words, nextPosition);

            if (firstPosition == -1)
                nextPosition = txtBox.Text.Length;
            else
            {
                txtBox.Select(firstPosition, words.Length);
                nextPosition = firstPosition - 1;
            }
            word = words;
            IF = true;
        }
        private void 查找上一个ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //查找上一项，如果未查找过，则显示查找对话框
            upward.Checked = true;
            down.Checked = false;
            try
            {
                FindWordsLast(word);
            }
            catch
            {
                查找ToolStripMenuItem_Click(sender, e);
            }
        }
        Label LblReplace = new Label
        {
            Name = "lblReplace",
            Text = "替换：",
            Size = new Size(55, 25),
            Location = new Point(15, 50)
        };
        Form ReplaceForm = new Form
        {
            Text = "替换文本",
            FormBorderStyle = FormBorderStyle.FixedSingle,
            MaximizeBox = false,
            MinimizeBox = false
        };
        private void 替换ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtInput.Size = new Size(190, 30);
            txtInput.Location = new Point(70, 12);
            txtInput.Multiline = true;

            txtInputReplace.Size = new Size(190, 30);
            txtInputReplace.Location = new Point(70, 47);
            txtInputReplace.Multiline = true;

            Button Replace = new Button
            {
                Name = "btnReplace",
                Text = "替换",
                Size = new Size(80, 25),
                Location = new Point(265, 15)
            };
            Replace.Click += new EventHandler(Replace_Click);
            Cancel.Click += new EventHandler(Cancel_Click);

            ReplaceForm.Controls.Add(lblSearch);
            ReplaceForm.Controls.Add(LblReplace);
            ReplaceForm.Controls.Add(txtInput);
            ReplaceForm.Controls.Add(txtInputReplace);
            ReplaceForm.Controls.Add(Replace);
            ReplaceForm.Controls.Add(Cancel);
            ReplaceForm.Top = this.Top + 50;
            ReplaceForm.Left = this.Left + 50;
            ReplaceForm.Height = 140;
            ReplaceForm.Width = 380;
            ReplaceForm.StartPosition = FormStartPosition.CenterParent;
            ReplaceForm.ShowDialog();
        }
        private void Replace_Click(object sender, EventArgs e)
        {
            txtBox.Text = txtBox.Text.Replace(txtInput.Text, txtInputReplace.Text);
        }
        private void 全选AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtBox.SelectAll();
        }
        private void 自动换行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //默认自动换行，点击按钮打开或关闭自动换行
            if (自动换行ToolStripMenuItem.Checked == true)
            {
                txtBox.WordWrap = false;
                自动换行ToolStripMenuItem.Checked = false;
            }
            else
            {
                txtBox.WordWrap = true;
                自动换行ToolStripMenuItem.Checked = true;
            }
        }
        private void 字体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //提示用户从本地计算机安装的字体中选择字体字号
            FontDialog fontDialog = new FontDialog();
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                txtBox.Font = fontDialog.Font;
            }
        }
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            //窗体的txtBox控件随窗体改变而改变的大小
            if (状态栏ToolStripMenuItem.Checked == true && 工具栏TToolStripMenuItem.Checked == true)
                txtBox.Height = this.Height - menuStrip.Height - toolStrip.Height - statusStrip.Height - 39;
            else if (状态栏ToolStripMenuItem.Checked == false && 工具栏TToolStripMenuItem.Checked == true)
                txtBox.Height = this.Height - menuStrip.Height - toolStrip.Height - 39;
            else if (状态栏ToolStripMenuItem.Checked == true && 工具栏TToolStripMenuItem.Checked == false)
                txtBox.Height = this.Height - menuStrip.Height - statusStrip.Height - 39;
            else
                txtBox.Height = this.Height - menuStrip.Height - 39;
            txtBox.Width = this.Width - 16;
        }
        private void 工具栏TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //默认打开工具栏，点击按钮打开或关闭工具栏
            if (工具栏TToolStripMenuItem.Checked == true)
            {
                toolStrip.Visible = false;
                工具栏TToolStripMenuItem.Checked = false;
                txtBox.Top = 25;
            }
            else if (工具栏TToolStripMenuItem.Checked == false)
            {
                toolStrip.Visible = true;
                工具栏TToolStripMenuItem.Checked = true;
                txtBox.Top = 50;
            }
            Form1_SizeChanged(sender, e);
        }
        private void 放大ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //放大字体大小
            var fontsize = txtBox.Font.Size;
            var fontFamily = txtBox.Font.FontFamily;
            txtBox.Font = new Font(fontFamily, fontsize + 1);
        }
        private void 缩小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //缩小字体大小
            var fontsize = txtBox.Font.Size;
            var fontFamily = txtBox.Font.FontFamily;
            txtBox.Font = new Font(fontFamily, fontsize - 1);
        }
        private void 恢复默认缩放ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //恢复默认字体大小
            txtBox.Font = new Font(txtBox.Font.FontFamily, 11);
        }
        private void 状态栏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //默认显示状态栏，点击按钮显示或关闭状态栏
            if (状态栏ToolStripMenuItem.Checked == true)
            {
                statusStrip.Visible = false;
                状态栏ToolStripMenuItem.Checked = false;
            }
            else if (状态栏ToolStripMenuItem.Checked == false)
            {
                statusStrip.Visible = true;
                状态栏ToolStripMenuItem.Checked = true;
            }
            Form1_SizeChanged(sender, e);
        }
        //private int GetStringLen(string s)
        //{
        //    if (!string.IsNullOrEmpty(s))
        //    {
        //        int len = s.Length;
        //        for (int i = 0; i < s.Length; i++)
        //        {
        //            if (s[i] > 255)
        //                len++;
        //        }
        //        return len;
        //    }
        //    return 0;
        //}
        private void 查看帮助HToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //调用系统自带的浏览器打开网页查看帮助
            Process.Start("https://jingyan.baidu.com/article/a24b33cdd86a0f19fe002be9.html");
        }
        private void 关于记事本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //关于记事本说明
            Label lblTitle = new Label()
            {
                Text = "多功能记事本",
                Size = new Size(150, 25),
                Location = new Point(100, 50)
            };
            Label lblEdition = new Label()
            {
                Text = "版本号：个性测试版",
                Size = new Size(150, 25),
                Location = new Point(85, 100)
            };
            Label lblMail = new Label()
            {
                Text = "E-Mail：",
                Size = new Size(55, 25),
                Location = new Point(30, 180)
            };
            LinkLabel llblMail = new LinkLabel()
            {
                Text = "2417525822@qq.com",
                Size = new Size(110, 25),
                Location = new Point(85, 180)
            };
            Label lblCNDS = new Label()
            {
                Text = "CNDS博客：",
                Size = new Size(65, 25),
                Location = new Point(20, 220)
            };
            LinkLabel llblCNDS = new LinkLabel()
            {
                Text = "https://blog.csdn.net/UFO_Harold",
                Size = new Size(200, 25),
                Location = new Point(85, 220)
            };
            Form about = new Form
            {
                Text = "关于记事本",
                FormBorderStyle = FormBorderStyle.FixedSingle,
                MaximizeBox = false
            };

            llblCNDS.Click += new EventHandler(LlblCNDS_Click);
            about.Controls.Add(lblTitle);
            about.Controls.Add(lblEdition);
            about.Controls.Add(lblMail);
            about.Controls.Add(llblMail);
            about.Controls.Add(lblCNDS);
            about.Controls.Add(llblCNDS);
            about.Top = this.Top + this.Height / 2 - about.Height / 2;
            about.Left = this.Left + this.Width / 2 - about.Width / 2;
            about.StartPosition = FormStartPosition.CenterParent;
            about.ShowDialog();
        }
        private void LlblCNDS_Click(object sender, EventArgs e)
        {
            Process.Start("https://blog.csdn.net/UFO_Harold");
        }
        private void 新建toolStripButton_Click(object sender, EventArgs e)
        {
            新建NToolStripMenuItem_Click(this, e);
        }
        private void 另存为toolStripButton_Click(object sender, EventArgs e)
        {
            另存为ToolStripMenuItem_Click(this, e);
        }
        private void 保存StoolStripButton_Click(object sender, EventArgs e)
        {
            保存SToolStripMenuItem_Click(this, e);
        }
        private void 打印PtoolStripButton_Click(object sender, EventArgs e)
        {
            打印PToolStripMenuItem_Click(this, e);
        }
        private void 剪切toolStripButton_Click(object sender, EventArgs e)
        {
            剪切ToolStripMenuItem_Click(this, e);
        }
        private void 复制CtoolStripButton_Click(object sender, EventArgs e)
        {
            复制CToolStripMenuItem_Click(this, e);
        }
        private void 粘贴PtoolStripButton_Click(object sender, EventArgs e)
        {
            粘贴PToolStripMenuItem_Click(this, e);
        }
        private void 帮助HtoolStripButton_Click(object sender, EventArgs e)
        {
            查看帮助HToolStripMenuItem_Click(this, e);
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            //显示编辑光标所在几行几列
            int row = txtBox.GetLineFromCharIndex(txtBox.SelectionStart) + 1;
            int col = (txtBox.SelectionStart - txtBox.GetFirstCharIndexFromLine(txtBox.GetLineFromCharIndex(txtBox.SelectionStart))) + 1;
            toolStripStatusLblLocation.Text = "第 " + row + " 行, 第 " + col + " 列";
            toolStripStatusLblNow.Text = "" + DateTime.Now.ToLocalTime();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //关闭窗体时如果已修改内容，则弹出是否保存对话框，否则直接关闭窗体
            if (txtBox.Modified == true)
            {
                DialogResult dr = MessageBox.Show("文件发生变化，是否更改保存？", "注意", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    保存SToolStripMenuItem_Click(sender, e);
                    return;
                }
                else if (dr == DialogResult.No)
                {
                    return;
                }
                else if (dr == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}