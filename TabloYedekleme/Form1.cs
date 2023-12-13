using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace TabloYedekleme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string connectionString = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
        private void Form1_Load(object sender, EventArgs e)
        {
            FillTableList();
        }
        private void FillTableList()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SHOW TABLES";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        checkedListBox1.Items.Add(reader.GetString(0));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluştu: " + ex.Message);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

            List<string> liste = new List<string>();
            foreach (var item in checkedListBox1.CheckedItems)
            {
                liste.Add(item.ToString());
            }
            FolderBrowserDialog folder = new FolderBrowserDialog();
            if(folder.ShowDialog() == DialogResult.OK)
            {
                RestoreAndBackup.BackupTable(connectionString, liste, folder.SelectedPath);
                MessageBox.Show("Seçilen Tablolarda Yedek Alındı!");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "SQL FİLE |*.sql";
            if(openFile.ShowDialog() == DialogResult.OK)
            {
                string SafeFileName = Path.GetFileNameWithoutExtension(openFile.SafeFileName);
                RestoreAndBackup.RestoreTable(connectionString, SafeFileName, openFile.FileName);
                MessageBox.Show(SafeFileName + " Yüklendi");
            }
        }
    }
}
