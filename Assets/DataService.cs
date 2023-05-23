using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.VisualScripting.Dependencies.Sqlite;

public class DataService
{
    private SQLiteConnection _connection;

    public DataService(string databaseName)
    {
        var dbPath = string.Format("{0}/{1}", Application.persistentDataPath, databaseName);
        _connection = new SQLiteConnection(dbPath);
        _connection.CreateTable<Score>();
    }

    public void CreateScore(Score score)
    {
        _connection.Insert(score);
    }

    public List<Score> GetTopScores(int limit)
    {
        return _connection.Table<Score>().OrderByDescending(x => x.Round).Take(limit).ToList();
    }
}

public class Score
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int Round { get; set; }

    public int Gold { get; set; }
}
