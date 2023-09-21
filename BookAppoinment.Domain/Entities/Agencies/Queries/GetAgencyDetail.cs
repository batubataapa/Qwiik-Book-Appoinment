using System;
using AutoMapper;
using BookAppoinment.Adapters.Errors;
using BookAppoinment.Adapters.Model;
using BookAppoinment.Adapters.Repositories.Interfaces;
using LanguageExt;
using MediatR;
using static LanguageExt.Prelude;

namespace BookAppoinment.Domain.Entities.Agencies.Queries;

public class GetAgencyDetail :
    IRequestHandler<GetAgencyDetailQuery, Either<QwiikError, AgencyDetailResponse>>
{
    private readonly ILogger<GetAgencyDetail> _log;
    private readonly IMapper _mapper;
    private readonly IAgencyRepository _repository;

    public GetAgencyDetail(ILogger<GetAgencyDetail> log, IMapper mapper, IAgencyRepository repository)
    {
        _log = log;
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Either<QwiikError, AgencyDetailResponse>> Handle(GetAgencyDetailQuery request,
        CancellationToken cancellationToken)
    {
        return (await _repository.GetAgencyByAgencyIdAsync(request.AgencyId))
            .ErrorIfNone(new QwiikInternalServerError())
            .Map(_mapper.Map<AgencyDetailResponse>);
    }
}

public record GetAgencyDetailQuery(int AgencyId) : IRequest<Either<QwiikError, AgencyDetailResponse>>;

public class AgencyDetailResponse
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}