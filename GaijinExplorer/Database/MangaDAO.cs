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
                    await db.OpenAsync();
                    //Debug.WriteLine("mark 2");
                    // Insert Command
                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;
                    insertCommand.CommandText = "INSERT OR IGNORE INTO " + App.APP_MANGA_TABLE + " (manga_id, manga_title, status) VALUES (@ID, @TITLE, @STATUS);";
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

        public static async Task CreateMangaAsync(Manga.Manga manga)
        {
            if (manga.Id != null && manga.Title != null)
            {

            }
        }

        public static async Task UpdateMangaAsync(Manga.Manga manga)
        {
            if (manga.Id != null && manga.Title != null)
            {
                using (SqliteConnection db = new SqliteConnection(App.APP_DB_File))
                {
                    await db.OpenAsync();
                    SqliteCommand updateCommand = new SqliteCommand();
                    updateCommand.Connection = db;
                    updateCommand.CommandText = "UPDATE " + App.APP_MANGA_TABLE + " SET WHERE manga_id = @ID;";
                    // TODO
                    // update Manga

                    // TODO 
                    // create categories
                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;
                    insertCommand.CommandText = "INSERT OR INGNORE INTO " + App.APP_MANGA_CATEGORY_TABLE + " ;";

                    db.Close();
                }
            }
        }

        /// <summary>
        /// Gets List of Mangas that are favorited.
        /// Only gets the id and title.
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Manga.Manga>> GetFavoriteMangasAsyncLite()
        {
            List<Manga.Manga> mangas = new List<Manga.Manga>();
            using (SqliteConnection db = new SqliteConnection(App.APP_DB_File))
            {
                await db.OpenAsync();
                SqliteCommand readCommand = new SqliteCommand();
                readCommand.Connection = db;
                readCommand.CommandText = "SELECT " + App.APP_MANGA_TABLE + ".manga_id, manga_title FROM " + App.APP_MANGA_TABLE + 
                    " INNER JOIN " + App.APP_MANGA_FAVORITE_TABLE + " ON " + App.APP_MANGA_TABLE + ".manga_id = " + App.APP_MANGA_FAVORITE_TABLE + ".manga_id;";
                SqliteDataReader reader = null;
                try
                {
                    reader = await readCommand.ExecuteReaderAsync();
                }
                catch (SqliteException e)
                {
                    Debug.WriteLine(e.TargetSite);
                    Debug.WriteLine(e.StackTrace);
                }
                if (reader != null)
                {
                    while(await reader.ReadAsync())
                    {
                        Manga.Manga manga = new Manga.Manga();
                        manga.Id = reader.GetString(0);
                        manga.Title = reader.GetString(1);
                        mangas.Add(manga);
                    }
                }
                db.Close();
            }
            return mangas;
        }

        public static async Task AddMangaToFavoritesAsync(string id)
        {
            using (SqliteConnection db = new SqliteConnection(App.APP_DB_File))
            {
                await db.OpenAsync();
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;
                insertCommand.CommandText = "INSERT OR IGNORE INTO " + App.APP_MANGA_FAVORITE_TABLE + " (manga_id) VALUES (@ID);";
                insertCommand.Parameters.AddWithValue("@ID", id);
                try
                {
                    await insertCommand.ExecuteReaderAsync();
                }
                catch (SqliteException e)
                {
                    Debug.WriteLine(e.TargetSite);
                    Debug.WriteLine(e.StackTrace);
                }
                db.Close();
            }
        }

        public static async Task RemoveMangaFromFavoritesAsync(string id)
        {
            using (SqliteConnection db = new SqliteConnection(App.APP_DB_File))
            {
                await db.OpenAsync();
                SqliteCommand deleteCommand = new SqliteCommand();
                deleteCommand.Connection = db;
                deleteCommand.CommandText = "DELETE FROM " + App.APP_MANGA_FAVORITE_TABLE + " WHERE manga_id = @ID;";
                deleteCommand.Parameters.AddWithValue("@ID", id);
                try
                {
                    await deleteCommand.ExecuteReaderAsync();
                }
                catch (SqliteException e)
                {
                    Debug.WriteLine(e.TargetSite);
                    Debug.WriteLine(e.StackTrace);
                }
                db.Close();
            }
        }
    }
}
