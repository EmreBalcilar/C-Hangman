using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace Adam_Asmaca
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string[] words = { "a", "b", "c", "ç", "d", "e", "f", "g", "ğ", "h", "ı", "i", "j", "k", "l", "m", "n", "o", "ö", "p", "r", "s", "ş", "t", "u", "ü", "v", "y", "z", "x", "w" };
        hangman_operations hangman;
        string word;
        string[] choosen_word;
        int health = 7;
        int remaining_life;
        int width, height;
        private void Form1_Load(object sender, EventArgs e)
        {
            location();
            width = pcb_adam.Width;
            height = pcb_adam.Height;
            remaining_life = health;
            choosen_word = new string[0];
            hangman = new hangman_operations();
        }
        private void Choose_Word() //Chose Word
        {
            hangman_operations.New_Word selected_word = hangman.take_word();
            lbl_kategori.Text = "This is " + selected_word.category;
            word = selected_word.word;
            foreach (char letter in word.ToCharArray())
            {
                lbl_kelime.Text = lbl_kelime.Text + "_ ";
            }
            button1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pcb_adam.Visible = true;
            textBox1.Visible = true;
            lbl_soylenenler.Visible = true;
            lbl_kelime.Visible = true;
            lbl_kategori.Visible = true;
            button2.Visible = true;
            Choose_Word();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string data = textBox1.Text;
            bool find = false;
            bool said = false;
            foreach (var item in choosen_word)
            {
                if (data.ToLower() == item.ToLower())
                {
                    said = true;
                    MessageBox.Show("You said this letter before");
                }
            }
            if (!said)
            {
                for (int i = 0; i < words.Length; i++)
                {
                    if (data.ToLower() == words[i])
                    {
                        find = true;
                        find_inword(data);
                        break;
                    }
                }
                if (!find)
                {
                    MessageBox.Show("Write one of the 26 letters");
                }
                update();
            }
            int index = lbl_kelime.Text.IndexOf("_");
            if (index == -1)
            {
                next_section();
            }

        }
        private void find_inword(string letter)
        {
            bool isit_correct = false;
            char[] dizi = word.ToCharArray();
            for (int a = 0; a < dizi.Length; a++)
            {
                if (dizi[a].ToString() != "")
                {
                    if (letter == dizi[a].ToString().ToLower())
                    {
                        isit_correct = true;
                        lbl_kelime.Text = lbl_kelime.Text.Remove(a * 2, 1);
                        lbl_kelime.Text = lbl_kelime.Text.Insert(a * 2, letter).ToUpper();

                    }
                }
            }
            if (!isit_correct)
            {
                remaining_life--;
                pcb_adam.Invalidate();
            }
            add_word(letter);

        }
        private void update()
        {
            lbl_soylenenler.Text = "";
            foreach (string item in choosen_word)
            {
                lbl_soylenenler.Text = lbl_soylenenler.Text + item + " ";
            }
        }
        private void add_word(string letter)
        {
            string[] a = new string[choosen_word.Length + 1];
            for (int i = 0; i < choosen_word.Length; i++)
            {
                a[i] = choosen_word[i];
            }
            a[a.Length - 1] = letter;
            choosen_word = a;
        }

        private void pcb_adam_Paint(object sender, PaintEventArgs e)
        {
            Pen kalem = new Pen(Color.Black, 12);
            if (remaining_life < health)
            {
                e.Graphics.DrawLine(kalem, width / 10, height / 15, width / 10, height / 15 * 14);
                e.Graphics.DrawLine(kalem, width / 10, height / 15, width / 2, height / 15);
            }
            if (remaining_life < health - 1)//head and rop
            {
                e.Graphics.DrawLine(kalem, width / 2, height / 15, width / 2, height / 15 * 3);
                e.Graphics.DrawEllipse(kalem, width / 2 - width / 10, height / 5, width / 5, height / 10);
            }
            if (remaining_life < health - 2)//body
            {
                e.Graphics.DrawLine(kalem, width / 2, height / 10 * 3, width / 2, height / 10 * 6);
            }
            if (remaining_life < health - 3)//right arm 
            {
                e.Graphics.DrawLine(kalem, width / 2, height / 10 * 3, width / 2 + width / 10, height / 10 * 3 + height / 5);
            }
            if (remaining_life < health - 4)//left arm
            {
                e.Graphics.DrawLine(kalem, width / 2, height / 10 * 3, width / 2 - width / 10, height / 10 * 3 + height / 5);
            }
            if (remaining_life < health - 5)//right foot
            {
                e.Graphics.DrawLine(kalem, width / 2, height / 10 * 6, width / 2 + width / 10, height / 10 * 6 + height / 10);
            }
            if (remaining_life < health - 6)//left foot
            {
                e.Graphics.DrawLine(kalem, width / 2, height / 10 * 6, width / 2 - width / 10, height / 10 * 6 + height / 10);
                gameover();
            }
        }
        private void next_section()
        {
            clear();
            Choose_Word();
        }
        private void gameover()
        {
            MessageBox.Show("Game Over Correct word: " + word);
            button1.Visible = true;
            textBox1.Visible = false;
            button2.Visible = false;
            clear();
        }
        private void clear()
        {
            choosen_word = new string[0];
            remaining_life = health;
            pcb_adam.Invalidate();
            lbl_kategori.Text = "";
            lbl_kelime.Text = "";
            lbl_soylenenler.Text = "";
            textBox1.Text = "";
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            location();
        }
        public void location()
        {
            int width, height;
            width = this.Width;
            height = this.Height;
            panel1.Width = width;
            panel1.Height = height;
            panel1.Location = new Point(0, 0);
            pcb_adam.Width = width / 9 * 4;
            pcb_adam.Height = height / 4 * 3;
            pcb_adam.Location = new Point(width - pcb_adam.Width - 10, 10);
            lbl_kategori.Location = new Point(10, 10);
            lbl_kelime.Location = new Point(10, 50);
            lbl_soylenenler.Location = new Point(10, 120);
            button1.Size = new System.Drawing.Size(width / 3, height / 10);
            textBox1.Location = new Point(10, 90);
            button1.Location = new Point(width / 2 - button1.Width / 2, height / 2 - button1.Height / 2);
            button2.Location = new Point(50, 90);
            lbl_soylenenler.Location = new Point(10, height / 2);
        }

    }
    class hangman_operations
    {
        private List<New_Word> new_wordlist;
        private List<New_Word> wordl_list;
        public hangman_operations()
        {
            take_data_file();
        }
        public struct New_Word
        {
            public string word;
            public string category;
        }
        public New_Word take_word()
        {
            if (wordl_list.Count == 0)
            {
                wordl_list = new_wordlist;
            }
            Random rastg = new Random();
            int a = rastg.Next(0, wordl_list.Count);
            New_Word kelimemiz = wordl_list[a];
            return kelimemiz;
        }
        private New_Word Create_new_word(string word, string category)
        {
            New_Word kelimemiz = new New_Word();
            kelimemiz.word = word;
            kelimemiz.category = category;
            return kelimemiz;
        }
        private void take_data_file()
        {
            new_wordlist = new List<New_Word>();
            StreamReader read = new StreamReader(@"data.txt", Encoding.Default);
            string data;
            while ((data = read.ReadLine()) != null)
            {
                if (data != "")
                {
                    new_wordlist.Add(Create_new_word(data.Split(',')[0], data.Split(',')[1]));
                }
            }
            wordl_list = new_wordlist;
        }
    }
}
