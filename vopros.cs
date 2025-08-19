using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SeaWars
{
    public partial class Vopros : Form
    {
        private Button btnRetry;
        private Button btnMainMenu;

        public Vopros()
        {
            this.Text = "Запитання";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Paint += Vopros_Paint;

            Label titleLabel = new Label();
            titleLabel.Text = "Морський бій";
            titleLabel.Font = new Font("Arial", 24, FontStyle.Bold);
            titleLabel.ForeColor = Color.DarkBlue;
            titleLabel.AutoSize = true;
            titleLabel.BackColor = Color.Transparent;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(titleLabel);

            titleLabel.Location = new Point((this.ClientSize.Width - titleLabel.Width) / 2, 50);

            Label questionLabel = new Label();
            questionLabel.Text = "Чи хочете ви зіграти ще раз?";
            questionLabel.Font = new Font("Arial", 16, FontStyle.Bold);
            questionLabel.ForeColor = Color.DarkBlue;
            questionLabel.AutoSize = true;
            questionLabel.BackColor = Color.Transparent;
            this.Controls.Add(questionLabel);
            questionLabel.Location = new Point((this.ClientSize.Width - questionLabel.Width) / 2, 130);

            int buttonWidth = 250;
            int buttonHeight = 50;
            int gap = 30;
            int totalWidth = buttonWidth * 2 + gap;
            int startX = (this.ClientSize.Width - totalWidth) / 2;
            int startY = 230;

            btnRetry = CreateButton("Зіграти ще раз", startX, startY);
            btnRetry.Click += BtnRetry_Click;
            this.Controls.Add(btnRetry);

            btnMainMenu = CreateButton("Повернутися до головного меню", startX + buttonWidth + gap, startY);
            btnMainMenu.Click += BtnMainMenu_Click;
            this.Controls.Add(btnMainMenu);
        }

        private void Vopros_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                new Point(0, 0), new Point(this.Width, 0),
                Color.White, Color.LightBlue))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private Button CreateButton(string text, int x, int y)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Size = new Size(250, 50);
            btn.Location = new Point(x, y);
            btn.Font = new Font("Arial", 12, FontStyle.Bold);
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = Color.White;
            btn.ForeColor = Color.DarkBlue;
            btn.FlatAppearance.BorderColor = Color.DarkBlue;
            btn.FlatAppearance.BorderSize = 2;
            btn.MouseEnter += (s, e) => btn.BackColor = Color.LightBlue;
            btn.MouseLeave += (s, e) => btn.BackColor = Color.White;
            btn.MouseDown += (s, e) => btn.Size = new Size(240, 45);
            btn.MouseUp += (s, e) => btn.Size = new Size(250, 50);
            return btn;
        }

        private void BtnRetry_Click(object sender, EventArgs e)
        {
            if (Form1.currentForm == null || Form1.currentForm.IsDisposed)
            {
                Form1.currentForm = new Form1();
            }
            else
            {
                Form1.currentForm.Init();
            }

            Form1.currentForm.Show();
            this.Close();
        }

        private void BtnMainMenu_Click(object sender, EventArgs e)
        {
            Home homeForm = new Home();
            homeForm.Show();
            this.Close();
        }
        private void btnPlayAgain_Click(object sender, EventArgs e)
        {
            Form1 newGame = new Form1();
            newGame.Show();
            this.Close();
        }
    }
}
