using System;

namespace AdaloExtensionPack.Core.Tables.Data;

public interface IAdaloEntity
{
    int Id { get; set; }
    DateTimeOffset CreatedAt { get; init; }
    DateTimeOffset UpdatedAt { get; init; }
}