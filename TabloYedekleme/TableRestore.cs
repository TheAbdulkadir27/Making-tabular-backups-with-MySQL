using System.IO;
using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using static System.Windows.Forms.CheckedListBox;

namespace TabloYedekleme
{
    public static class RestoreAndBackup
    {
        public static void BackupTable(string connectionString, List<string> tableName, string backupPath)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    string yol = $"{backupPath}\\{DateTime.Today.ToShortDateString()} backup.sql";
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = connection;
                        mb.ExportInfo.TablesToBeExportedList = tableName;
                        mb.ExportToFile(yol);
                    }
                }
            }
        }
        public static void RestoreTable(string connectionString, string tableName, string backupPath)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = connection;
                            mb.ExportInfo.TablesToBeExportedList = new List<string>()
                            {
                                tableName
                            };
                            mb.ImportFromFile(backupPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
    }
}