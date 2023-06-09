using System;
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
        score.Timestamp = DateTime.Now; // Aquí es donde añades el timestamp
        _connection.Insert(score);
    }

    public IEnumerable<Score> GetTopScores()
    {
        const string query = "SELECT * FROM Score ORDER BY Round DESC, Gold DESC LIMIT 5";
        return _connection.Query<Score>(query);
    }
}

public class Score
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int Round { get; set; }

    public int Gold { get; set; }
    
    public DateTime Timestamp { get; set; } // Aquí es donde añades la nueva propiedad
}
