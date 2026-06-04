using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OffshoreInsights.Domain.Enums;

namespace OffshoreInsights.Persistence.Converters;

/// <summary>
/// Converts between the VesselType enum and its string representation in the database.
/// Handles known naming mismatches between the database values and C# enum member names,
/// and falls back to VesselType.Unknown for any value that cannot be mapped rather than throwing.
/// </summary>
public class VesselTypeConverter() : ValueConverter<VesselType, string>(
    v => v.ToString(),
    v => ToVesselType(v))
{
    /// <summary>
    /// Explicit mappings for database values that do not match the C# enum member name exactly.
    /// Any value not present here and not parseable directly falls back to VesselType.Unknown.
    /// Note: "Survey" exists in the database but has no corresponding enum member — it maps to Unknown.
    /// </summary>
    private static readonly Dictionary<string, VesselType> KnownMappings = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Dredging"]    = VesselType.DredgingOrUnderwaterOps,
        ["PilotVessel"] = VesselType.Pilot,
    };

    private static VesselType ToVesselType(string value)
    {
        if (KnownMappings.TryGetValue(value, out var mapped))
            return mapped;

        if (Enum.TryParse<VesselType>(value, ignoreCase: true, out var parsed))
            return parsed;

        return VesselType.Unknown;
    }
}
