﻿using Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.DbModels;

[Table("Players")]
public class DbPlayer
{
    [Key]
    public int Id { get; set; }

    public string Alias { get; set; }

    public int TotalScore { get; set; }

    public ICollection<DbScoreEntry> ScoreEntries { get; set; }

    public DbPlayer()
    {
        ScoreEntries = [];
    }

    public DbPlayer(Player player) : this()
    {
        Alias = player.Alias;
        TotalScore = player.TotalScore;

        ScoreEntries = [.. player.ScoreEntries.Select(e => new DbScoreEntry(e))];
    }

    public Player ToModel() => new(Alias)
    {
        TotalScore = TotalScore,
        ScoreEntries = [.. ScoreEntries.Select(e => e.ToModel())]
    };
}
