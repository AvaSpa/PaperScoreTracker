using Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.DbModels;

[Table("ScoreEntries")]
public class DbScoreEntry
{
    [Key]
    public int Id { get; set; }

    public DbPlayer? Player { get; set; }

    public int ScoreValue { get; set; }

    public DbScoreEntry()
    {
    }

    public DbScoreEntry(ScoreEntry scoreEntry)
    {
        ScoreValue = scoreEntry.Value;
    }

    public ScoreEntry ToModel() => new ScoreEntry(ScoreValue);
}
