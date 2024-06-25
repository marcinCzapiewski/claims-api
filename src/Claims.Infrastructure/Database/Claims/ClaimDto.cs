﻿using MongoDB.Bson.Serialization.Attributes;

namespace Claims.Infrastructure.Database.Claims;
public class ClaimDto
{
    [BsonId]
    public required string Id { get; set; }

    [BsonElement("coverId")]
    public required string CoverId { get; set; }

    [BsonElement("created")]
    [BsonDateTimeOptions(DateOnly = true)]
    public DateTime Created { get; set; }

    [BsonElement("name")]
    public required string Name { get; set; }

    [BsonElement("claimType")]
    public ClaimType Type { get; set; }

    [BsonElement("damageCost")]
    public decimal DamageCost { get; set; }
}

public enum ClaimType
{
    Collision = 0,
    Grounding = 1,
    BadWeather = 2,
    Fire = 3
}
