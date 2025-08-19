// Home.cs
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SeaWars
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
            this.Text = "Головне меню";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Paint += new PaintEventHandler(Home_Paint);

            Label titleLabel = new Label();
            titleLabel.Text = "Морський бій";
            titleLabel.Font = new Font("Arial", 24, FontStyle.Bold);
            titleLabel.ForeColor = Color.DarkBlue;
            titleLabel.AutoSize = true;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.BackColor = Color.Transparent;
            this.Controls.Add(titleLabel);  // Додаємо на форму, щоб отримати актуальні розміри

            // Центруємо після додавання
            titleLabel.Location = new Point((this.ClientSize.Width - titleLabel.Width) / 2, 50);

            int shiftX = 0;
            int shiftY = 10;
            titleLabel.Location = new Point((this.ClientSize.Width - titleLabel.Width) / 2 + shiftX, 50 + shiftY);
            titleLabel.TextAlign = ContentAlignment.BottomCenter;
            this.Controls.Add(titleLabel);

            int buttonWidth = 200;
            int buttonHeight = 50;
            int centerX = (this.ClientSize.Width - buttonWidth) / 2;
            int startY = 150;
            int gap = 60;

            Button btnPlay = CreateButton("Грати", centerX, startY);
            btnPlay.Click += BtnPlay_Click;
            this.Controls.Add(btnPlay);

            Button btnRules = CreateButton("Правила", centerX, startY + gap);
            btnRules.Click += BtnRules_Click;
            this.Controls.Add(btnRules);

            Button btnExit = CreateButton("Вийти", centerX, startY + 2 * gap);
            btnExit.Click += BtnExit_Click;
            this.Controls.Add(btnExit);
        }

        private void Home_Paint(object sender, PaintEventArgs e)
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
            btn.Size = new Size(200, 50);
            btn.Location = new Point(x, y);
            btn.Font = new Font("Arial", 12, FontStyle.Bold);
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = Color.White;
            btn.ForeColor = Color.DarkBlue;
            btn.FlatAppearance.BorderColor = Color.DarkBlue;
            btn.FlatAppearance.BorderSize = 2;
            btn.MouseEnter += (s, e) => btn.BackColor = Color.LightBlue;
            btn.MouseLeave += (s, e) => btn.BackColor = Color.White;
            btn.MouseDown += (s, e) => btn.Size = new Size(190, 45);
            btn.MouseUp += (s, e) => btn.Size = new Size(200, 50);
            return btn;
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            if (Form1.currentForm == null || Form1.currentForm.IsDisposed)
            {
                Form1.currentForm = new Form1();
            }
            Form1.currentForm.Show();
            this.Hide();

        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnRules_Click(object sender, EventArgs e)
        {
            Form rulesForm = new Form();
            rulesForm.Size = new Size(450, 255);
            rulesForm.Text = "Правила гри";
            rulesForm.StartPosition = FormStartPosition.CenterScreen;
            rulesForm.BackColor = Color.LightBlue;

            RichTextBox richTextBox = new RichTextBox();
            richTextBox.Dock = DockStyle.Fill;
            richTextBox.ReadOnly = true;
            richTextBox.BackColor = Color.WhiteSmoke;
            richTextBox.ForeColor = Color.DarkBlue;
            richTextBox.Font = new Font("Arial", 12, FontStyle.Italic);
            richTextBox.Text = "Правила гри в Морський бій:\n\n" +
                               "1. Гравці по черзі стріляють по клітинках противника.\n" +
                               "2. Гравець, який потопить всі кораблі супротивника, виграє.\n" +
                               "3. Кожен корабель займає кілька клітинок на полі.\n" +
                               "4. Кораблі не можуть перекривати одна одну.\n\n" +
                               "Для перемоги необхідно точно потопити всі кораблі супротивника.";

            richTextBox.SelectionAlignment = HorizontalAlignment.Center;
            rulesForm.Controls.Add(richTextBox);

            Button btnBack = new Button();
            btnBack.Text = "Назад";
            btnBack.Size = new Size(100, 40);
            btnBack.Location = new Point((rulesForm.ClientSize.Width - btnBack.Width) / 2, rulesForm.ClientSize.Height - btnBack.Height - 20);
            btnBack.Font = new Font("Arial", 10, FontStyle.Bold);
            btnBack.BackColor = Color.White;
            btnBack.ForeColor = Color.DarkBlue;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.FlatAppearance.BorderColor = Color.DarkBlue;
            btnBack.FlatAppearance.BorderSize = 2;
            btnBack.Click += (s, args) =>
            {
                // Перевірка на наявність форми Home
                var homeForm = Application.OpenForms["Home"];
                if (homeForm == null)  // Якщо Home не відкрита, створюємо нову форму
                {
                    Home home = new Home();
                    home.Show();  // Відкриваємо Home
                }
                else
                {
                    homeForm.Show();  // Якщо форма вже відкрита, просто показуємо її
                }

                rulesForm.Close();  
            };

            rulesForm.Controls.Add(btnBack);
            rulesForm.ShowDialog();
        }
    }
}
