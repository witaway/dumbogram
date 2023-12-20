using Dumbogram.Api.Persistence.Context.Application.Configurations.Messages;
using Dumbogram.Api.Persistence.Context.Application.Enumerations;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Api.Persistence.Context.Application.Entities.Messages;

[EntityTypeConfiguration(typeof(SystemMessageConfiguration))]
public class SystemMessage : Message
{
    public SystemMessageType SystemMessageType { get; set; }
    public SystemMessageDetails? SystemMessageDetails { get; set; }
}