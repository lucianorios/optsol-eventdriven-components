using EventDriven.Arch.Domain;
using EventDriven.Arch.Domain.Beneficiarios;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Repositories;

namespace EventDriven.Arch.Driven.Infra.Data.MongoDb;

public class BeneficiarioWriteRepository : WriteRepository<Beneficiario>, IBeneficiarioWriteRepository
{
    public BeneficiarioWriteRepository(MongoContext context, IMessageBus messageBus) : base(context, messageBus, nameof(Beneficiario))
    {
    }
}

