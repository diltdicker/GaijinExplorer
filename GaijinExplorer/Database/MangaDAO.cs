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

        /// <summary>
        /// To be called by MangaPage which gets a full copy of a manga
        /// </summary>
        /// <param name="manga"></param>
        /// <returns></returns>
        public static async Task CreateMangaAsync(Manga.Manga manga)
        {
            if (manga.Id != null && manga.Title != null)
            {
                using (SqliteConnection db = new SqliteConnection(App.APP_DB_File))
                {

                }
            }
        }

        /// <summary>
        /// To be called after updating DB with all titles
        /// </summary>
        /// <param name="manga"></param>
        /// <returns></returns>
        public static async Task UpdateMangaAsync(Manga.Manga manga)
        {
            if (manga.Id != null && manga.Title != null)
            {
                //Debug.WriteLine("Update Manga: " + manga.Id + " - " + manga.Title);
                using (SqliteConnection db = new SqliteConnection(App.APP_DB_File))
                {
                    bool flag = true;
                    await db.OpenAsync();
                    SqliteCommand updateCommand = new SqliteCommand();
                    updateCommand.Connection = db;
                    updateCommand.CommandText = "UPDATE " + App.APP_MANGA_TABLE + " SET manga_title = @TITLE, image_url = @URL, " +
                        "author = @AUTHOR, artist = @ARTIST, hits = @HITS, description = @DESCRIPTION, last_date = @DATE, " +
                        "status = @STATUS WHERE manga_id = @ID;";
                    //updateCommand.CommandText = "UPDATE " + App.APP_MANGA_TABLE + " SET manga_title = @TITLE, last_date = @DATE, status = @STATUS, hits = @HITS, image_url = @URL, author = @AUTHOR, artist = @ARTIST WHERE manga_id = @ID;";
                    //Debug.WriteLine("Params start");
                    updateCommand.Parameters.AddWithValue("@ID", manga.Id);
                    updateCommand.Parameters.AddWithValue("@TITLE", manga.Title);
                    updateCommand.Parameters.AddWithValue("@DATE", manga.LastDate);
                    updateCommand.Parameters.AddWithValue("@STATUS", manga.Status.ToString());
                    updateCommand.Parameters.AddWithValue("@HITS", manga.Hits);
                    //Debug.WriteLine("Params mid");
                    if (manga.ImageString != null)
                    {
                        updateCommand.Parameters.AddWithValue("@URL", manga.ImageString);
                    }
                    else
                    {
                        updateCommand.Parameters.AddWithValue("@URL", DBNull.Value);
                    }
                    if (manga.Author != null)
                    {
                        updateCommand.Parameters.AddWithValue("@AUTHOR", manga.Author);
                    }
                    else
                    {
                        updateCommand.Parameters.AddWithValue("@AUTHOR", DBNull.Value);
                    }
                    if (manga.Artist != null)
                    {
                        updateCommand.Parameters.AddWithValue("@ARTIST", manga.Artist);
                    }
                    else
                    {
                        updateCommand.Parameters.AddWithValue("@ARTIST", DBNull.Value);
                    }
                    if (manga.Description != null)
                    {
                        updateCommand.Parameters.AddWithValue("@DESCRIPTION", manga.Description);
                    }
                    else
                    {
                        updateCommand.Parameters.AddWithValue("@DESCRIPTION", DBNull.Value);
                    }
                    //Debug.WriteLine("Params end");
                    try
                    {
                        await updateCommand.ExecuteReaderAsync();
                    }
                    catch(SqliteException e)
                    {
                        Debug.WriteLine("Manga: " + manga.Id + " - " + manga.Title);
                        flag = false;
                        Debug.WriteLine(e.TargetSite);
                        Debug.WriteLine(e.StackTrace);
                    }
                    updateCommand.Dispose();
                    if (manga.Categories != null && flag)
                    {
                        foreach (string category in manga.Categories)
                        {
                            //Debug.WriteLine("category: " + category);
                            SqliteCommand insertCommand = new SqliteCommand();
                            insertCommand.Connection = db;
                            insertCommand.CommandText = "INSERT OR IGNORE INTO " + App.APP_MANGA_CATEGORY_TABLE + " (manga_id, category) VALUES (@ID, @CATEGORY);";
                            insertCommand.Parameters.AddWithValue("@ID", manga.Id);
                            insertCommand.Parameters.AddWithValue("@CATEGORY", category);
                            //insertCommand.CommandText = "INSERT OR INGNORE INTO " + App.APP_MANGA_CATEGORY_TABLE + " (manga_id, category) VALUES (" + manga.Id + ", " + category + ");";
                            try
                            {
                                await insertCommand.ExecuteReaderAsync();
                            }
                            catch (SqliteException e)
                            {
                                Debug.WriteLine("Manga: " + manga.Id + " - " + manga.Title);
                                Debug.WriteLine(e.TargetSite);
                                Debug.WriteLine(e.StackTrace);
                            }
                            insertCommand.Dispose();
                        }
                    }
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
