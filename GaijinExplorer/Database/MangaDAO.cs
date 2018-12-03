using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaijinExplorer.Database
{
    class MangaDAO
    {
        public static async Task<bool> DoesExist(string id)
        {
            return false;
        }

        public static async Task<bool> CreateMangaAsyncLite(Manga.Manga manga)
        {
            bool flag = true;
            if (manga.Id != null && manga.Title != null)
            {
                using (SqliteConnection db = new SqliteConnection(App.APP_DB_File))
                {
                    //Debug.WriteLine("mark 1");
                    db.Open();
                    SqliteDataReader reader = null;
                    SqliteCommand testCommand = new SqliteCommand();
                    testCommand.Connection = db;
                    testCommand.CommandText = "SELECT manga_id FROM " + App.APP_MANGA_TABLE + " WHERE manga_id = (@ID);";
                    testCommand.Parameters.AddWithValue("@ID", manga.Id);
                    try
                    {
                        reader = await testCommand.ExecuteReaderAsync();
                    }
                    catch(SqliteException e)
                    {
                        flag = false;
                        Debug.WriteLine(e.TargetSite);
                        Debug.WriteLine(e.StackTrace);
                        Debug.WriteLine("Bad Does Exist");
                    }
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            // Id already Exists!
                            flag = false;
                        }
                        if (flag)
                        {
                            //Debug.WriteLine("mark 2");
                            // Insert Command
                            SqliteCommand insertCommand = new SqliteCommand();
                            insertCommand.Connection = db;
                            insertCommand.CommandText = "INSERT INTO " + App.APP_MANGA_TABLE + " (manga_id, manga_title, status) VALUES (@ID, @TITLE, @STATUS);";
                            insertCommand.Parameters.AddWithValue("@ID", manga.Id);
                            insertCommand.Parameters.AddWithValue("@TITLE", manga.Title);
                            insertCommand.Parameters.AddWithValue("@STATUS", manga.Status.ToString());
                            //Debug.WriteLine("ID: " + manga.Id + '\n' + "Title: " + manga.Title + '\n' + "Status: " + manga.Status.ToString());
                            try
                            {
                                await insertCommand.ExecuteReaderAsync();
                            }
                            catch (SqliteException e)
                            {
                                flag = false;
                                Debug.WriteLine(e.TargetSite);
                                Debug.WriteLine(e.StackTrace);
                                //Debug.WriteLine("Bad Insert");
                            }
                        }
                    }
                    db.Close();
                    //Debug.WriteLine("mark 3");
                }
                return flag;
            }
            else
            {
                return false;
            }
            
        }
    }
}
