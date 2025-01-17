//
//  DiscordRestAuditLogAPI.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Core;
using Remora.Results;

namespace Remora.Discord.Rest.API
{
    /// <inheritdoc cref="Remora.Discord.API.Abstractions.Rest.IDiscordRestAuditLogAPI" />
    [PublicAPI]
    public class DiscordRestAuditLogAPI : AbstractDiscordRestAPI, IDiscordRestAuditLogAPI
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiscordRestAuditLogAPI"/> class.
        /// </summary>
        /// <param name="discordHttpClient">The Discord HTTP client.</param>
        /// <param name="jsonOptions">The JSON options.</param>
        public DiscordRestAuditLogAPI(DiscordHttpClient discordHttpClient, IOptions<JsonSerializerOptions> jsonOptions)
            : base(discordHttpClient, jsonOptions)
        {
        }

        /// <inheritdoc />
        public virtual async Task<Result<IAuditLog>> GetAuditLogAsync
        (
            Snowflake guildID,
            Optional<Snowflake> userID = default,
            Optional<AuditLogEvent> actionType = default,
            Optional<Snowflake> before = default,
            Optional<byte> limit = default,
            CancellationToken ct = default
        )
        {
            if (limit.HasValue && limit.Value is > 100 or 0)
            {
                return new ArgumentOutOfRangeError
                (
                    nameof(limit),
                    $"The limit must be between 1 and 100."
                );
            }

            return await this.DiscordHttpClient.GetAsync<IAuditLog>
            (
                $"guilds/{guildID}/audit-logs",
                b =>
                {
                    if (userID.HasValue)
                    {
                        b.AddQueryParameter("user_id", userID.Value.ToString());
                    }

                    if (actionType.HasValue)
                    {
                        b.AddQueryParameter("action_type", ((int)actionType.Value).ToString());
                    }

                    if (before.HasValue)
                    {
                        b.AddQueryParameter("before", before.Value.ToString());
                    }

                    if (limit.HasValue)
                    {
                        b.AddQueryParameter("limit", limit.Value.ToString());
                    }
                },
                ct: ct
            );
        }
    }
}
