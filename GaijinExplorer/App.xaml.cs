using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GaijinExplorer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool FIRST_MANGA_GRAB = true;
        public static CancellationTokenSource CancellationToken { get; set; }

        //============================================================================================

        public const string APP_DB_File = "Filename=GaijinExplorer.db.sqlite3";

        //============================================================================================

        public const string APP_MANGA_TABLE = "manga_table";
        public const string APP_MANGA_CHAPTER_TABLE = "manga_chapter_table";
        public const string APP_MANGA_CHAPTER_IMAGE_TABLE = "manga_chapter_image_table";
        public const string APP_FAVORITE_TABLE = "favorite_table";
        public const string APP_MANGA_CATEGORY_TABLE = "manga_catagory_table";
        const string INIT_MANGA_TABLE = "CREATE TABLE IF NOT EXISTS " + APP_MANGA_TABLE + " (" +
            "manga_id TEXT PRIMARY KEY," +
            "manga_title TEXT NOT NULL," +
            "image BLOB," +
            "image_url TEXT," +
            "author TEXT," +
            "artist TEXT," +
            "hits INTEGER DEFAULT 0," +
            "description TEXT," +
            "created_date INTEGER DEFAULT 0," +
            "last_date INTEGER DEFAULT 0," +
            "status TEXT NOT NULL CHECK (status IN ('Suspended', 'Ongoing', 'Completed'))" +
            ") WITHOUT ROWID;";
        const string INIT_MANGA_CHAPTER_TABLE = "CREATE TABLE IF NOT EXISTS " + APP_MANGA_CHAPTER_TABLE + " (" +
            "chapter_id TEXT PRIMARY KEY," +
            "chapter_number REAL NOT NULL," +
            "manga_id TEXT NOT NULL," +
            "date INTEGER DEFAULT 0," +
            "chapter_title TEXT," +
            "FOREIGN KEY (manga_id) REFERENCES " + APP_MANGA_TABLE + "(manga_id)" +
            ") WITHOUT ROWID;";
        const string INIT_MANGA_CHAPTER_IMAGE_TABLE = "CREATE TABLE IF NOT EXISTS " + APP_MANGA_CHAPTER_IMAGE_TABLE + " (" +
            "chapter_id TEXT NOT NULL," +
            "image_number INTEGER NOT NULL," +
            "image BLOB," +
            "PRIMARY KEY (chapter_id, image_number)," +
            "FOREIGN KEY (chapter_id) REFERENCES " + APP_MANGA_CHAPTER_TABLE + "(chapter_id)" +
            ") WITHOUT ROWID;";
        const string INIT_FAVORITE_TABLE = "CREATE TABLE IF NOT EXISTS " + APP_FAVORITE_TABLE + " (" +
            "manga_id TEXT PRIMARY KEY," +
            "FOREIGN KEY (manga_id) REFERENCES " + APP_MANGA_TABLE + "(manga_id)" +
            ") WITHOUT ROWID;";
        const string INIT_MANGA_CATEGORY_TABLE = "CREATE TABLE IF NOT EXISTS " + APP_MANGA_CATEGORY_TABLE + " (" +
            "manga_id TEXT NOT NULL," +
            "category TEXT NOT NULL," +
            "PRIMARY KEY(manga_id, category)," +
            "FOREIGN KEY (manga_id) REFERENCES " + APP_MANGA_TABLE + "(manga_id)" +
            ") WITHOUT ROWID;";

        //============================================================================================

        public App()
        {
            this.InitializeComponent();
            Debug.WriteLine("start of program");


        }
    }
}
