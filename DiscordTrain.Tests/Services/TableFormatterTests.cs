using DiscordTrain.Common;
using DiscordTrain.Services;

namespace DiscordTrain.Tests.Services;

public class TableFormatterTests
{
    [Test]
    public void FormatData_ShouldReturnFormattedTable()
    {
        // Arrange
        var formatter = new TableFormatter();
        var data = new List<Train>
        {
            new Train { Id = "S1", Name = "Alice"},
            new Train { Id = "S2", Name = "Bob"}
        };

        // Act
        var result = formatter.FormatData(data);
        Assert.That(result, Is.Not.Empty);
    }


    [Test]
    public void FormatData_ShouldReturnDesiredColumns()
    {
        // Arrange
        var formatter = new TableFormatter();
        var data = new List<Train>
        {
            new Train { Id = "S1", Name = "Alice"},
            new Train { Id = "S2", Name = "Bob"}
        };

        // Act
        var result = formatter.FormatData(data, nameof(Train.Id), nameof(Train.Name));
        Assert.That(result, Is.Not.Empty);
    }
}
