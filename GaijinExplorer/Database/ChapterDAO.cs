using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaijinExplorer.Database
{
    class ChapterDAO
    {
        /// <summary>
        /// If the Chapter doesn't exist it creates it.
        /// If the Chapter does exist it checks the HasViewed Property
        /// </summary>
        /// <param name="chapter">An already pre-filled Chapter object</param>
        /// <param name="mangaId">Id to Manga</param>
        /// <returns>Returns a modified Chapter that has an updated 'HasViewed' property</returns>
        public static Manga.Chapter CreateAndGetChapterLite(Manga.Chapter chapter, string mangaId)
        {
            if (chapter.Id != null && mangaId != null)
            {
                using (SqliteConnection db = new SqliteConnection(App.APP_DB_File))
                {
                    db.Open();
                    SqliteCommand readCommand = new SqliteCommand();
                    readCommand.Connection = db;
                    readCommand.CommandText = "SELECT has_viewed FROM " + App.APP_MANGA_CHAPTER_TABLE + " WHERE chapter_id = @ID;";
                    readCommand.Parameters.AddWithValue("@ID", chapter.Id);
                    SqliteDataReader reader = null;
                    try
                    {
                        reader = readCommand.ExecuteReader();
                    }
                    catch(SqliteException e)
                    {
                        Debug.WriteLine(e.TargetSite);
                        Debug.WriteLine(e.StackTrace);
                    }
                    if (reader != null)
                    {
                        bool exists = false;
                        while (reader.Read())
                        {
                            exists = true;
                            chapter.setHasViewed(reader.GetString(0));
                        }
                        Debug.WriteLine("chapter exists: " + exists);
                        if (!exists)
                        {
                            SqliteCommand insertCommand = new SqliteCommand();
                            insertCommand.Connection = db;
                            insertCommand.CommandText = "INSERT INTO " + App.APP_MANGA_CHAPTER_TABLE + " (chapter_id, chapter_number, manga_id, date, chapter_title, has_viewed) VALUES (@ID, @NUM, @MANGA_ID, @DATE, @TITLE, @VIEWED);";
                            insertCommand.Parameters.AddWithValue("@ID", chapter.Id);
                            insertCommand.Parameters.AddWithValue("@MANGA_ID", mangaId);
                            insertCommand.Parameters.AddWithValue("@NUM", chapter.Number);
                            insertCommand.Parameters.AddWithValue("@DATE", chapter.Date);
                            insertCommand.Parameters.AddWithValue("@VIEWED", chapter.ViewedStatus.ToString());
                            if (chapter.Title != null)
                            {
                                insertCommand.Parameters.AddWithValue("@TITLE", chapter.Title);
                            }
                            else
                            {
                                insertCommand.Parameters.AddWithValue("@TITLE", DBNull.Value);
                            }
                            try
                            {
                                insertCommand.ExecuteReader();
                            } 
                            catch(SqliteException e)
                            {
                                Debug.WriteLine(e.TargetSite);
                                Debug.WriteLine(e.StackTrace);
                            }
                        }
                    }
                    db.Close();
                }
            }
            return chapter;
        }

        public static async Task<Manga.Chapter> CreateAndGetChapterAsync(Manga.Chapter chapter)
        {
            Debug.WriteLine("start: " + chapter.Number);
            await Task.Delay(5000);
            Debug.WriteLine("done: " + chapter.Number);
            return chapter;
        }

        public static async void UpdateHasViewedAsync(string chapterId)
        {
            if (chapterId != null)
            {
                using (SqliteConnection db = new SqliteConnection(App.APP_DB_File))
                {
                    Debug.WriteLine("Updated Chapter");
                    db.Open();
                    SqliteCommand updateCommand = new SqliteCommand();
                    updateCommand.Connection = db;
                    updateCommand.CommandText = "UPDATE " + App.APP_MANGA_CHAPTER_TABLE + " SET has_viewed = @VIEWED WHERE chapter_id = @ID;";
                    updateCommand.Parameters.AddWithValue("@ID", chapterId);
                    updateCommand.Parameters.AddWithValue("@VIEWED", Manga.Chapter.HasViewed.Viewed.ToString());
                    try
                    {
                        await updateCommand.ExecuteReaderAsync();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.TargetSite);
                        Debug.WriteLine(e.StackTrace);
                    }
                    db.Close();
                }
            }
        }
    }
}
