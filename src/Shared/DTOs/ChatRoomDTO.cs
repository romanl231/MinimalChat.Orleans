using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    [GenerateSerializer, Immutable]
    public record ChatDTO(
        [property: Id(0)] string Title,
        [property: Id(1)] string Description,
        [property: Id(2)] string CreatorId,
        [property: Id(3)] List<string> MemberIds);
}
