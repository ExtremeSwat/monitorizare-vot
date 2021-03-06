﻿using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using Microsoft.Extensions.Caching.Distributed;
using VoteMonitor.Api.Core.Services;
using VoteMonitor.Entities;
using VoteMonitor.Api.Core;
using VoteMonitor.Api.Statistics.Models;
using VoteMonitor.Api.Statistics.Queries;

namespace VoteMonitor.Api.Statistics.Handlers {
    public class StatisticsQueryHandler :
        IRequestHandler<StatisticsObserversNumberQuery, ApiListResponse<SimpleStatisticsModel>>,
        IRequestHandler<StatisticiTopSesizariQuery, ApiListResponse<SimpleStatisticsModel>>,
        IRequestHandler<StatisticsOptionsQuery, StatisticsOptionsModel>
    {
        private readonly VoteMonitorContext _context;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;

        public StatisticsQueryHandler(VoteMonitorContext context, IMapper mapper, ICacheService cacheService)
        {
            _context = context;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<StatisticsOptionsModel> Handle(StatisticsOptionsQuery message, CancellationToken token)
        {
            var queryBuilder = new StatisticsQueryBuilder
            {
                Query = $@"SELECT O.Text AS Label, O.Id AS Code, OQ.Flagged AS Flagged, COUNT(*) as Value
                  FROM Answers AS A
                  INNER JOIN OptionsToQuestions AS OQ ON OQ.Id = A.IdOptionToQuestion
                  INNER JOIN Options AS O ON O.Id = OQ.IdOption
                  INNER JOIN Observers Obs ON Obs.Id = A.IdObserver
                  WHERE OQ.Id = {message.QuestionId}",
                CacheKey = $"StatisticiOptiuni-{message.QuestionId}"
            };

            queryBuilder.AndOngFilter(message.Organizator, message.IdONG);
            queryBuilder.Append("GROUP BY O.Text, O.Id, OQ.Flagged");

            return await _cacheService.GetOrSaveDataInCacheAsync(queryBuilder.CacheKey,
                async () =>
                {
                    var records = await _context.OptionsStatistics
                        .FromSql(queryBuilder.Query)
                        .ToListAsync(cancellationToken: token);

                    return new StatisticsOptionsModel
                    {
                        QuestionId = message.QuestionId,
                        Options = records.Select(s => new OptiuniStatisticsModel
                        {
                            OptionId = s.Code,
                            Label = s.Label,
                            Value = s.Value.ToString(),
                            Flagged = s.Flagged
                        })
                        .ToList(),
                        Total = records.Sum(s => s.Value)
                    };
                },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = new TimeSpan(message.CacheHours, message.CacheMinutes, message.CacheMinutes)
                }
            );
        }

        public async Task<ApiListResponse<SimpleStatisticsModel>> Handle(StatisticsObserversNumberQuery message, CancellationToken token)
        {
            var queryBuilder = new StatisticsQueryBuilder
            {
                Query = @"select count(distinct a.IdObserver) as [Value], CountyCode as Label
                          from Answers a (nolock) inner join Observers o on a.IdObserver = o.Id ",
                CacheKey = "StatisticiObservatori"
            };
            
            queryBuilder.WhereOngFilter(message.Organizator, message.IdONG);
            //queryBuilder.Append("GROUP BY J.Name ORDER BY Value DESC");            
            queryBuilder.Append("group by CountyCode order by [Value] desc");

            // get or save all records in cache
            var records = await _cacheService.GetOrSaveDataInCacheAsync(queryBuilder.CacheKey,
                async () =>
                {
                    return await _context.SimpleStatistics
                    .FromSql(queryBuilder.Query)
                    .ToListAsync();
                },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = new TimeSpan(message.CacheHours, message.CacheMinutes, message.CacheMinutes)
                }
            );
            
            // perform count and pagination on the records retrieved from the cache 
            var pagedList = records.Paginate(message.Page, message.PageSize);
           
            return new ApiListResponse<SimpleStatisticsModel>
            {
                Data = pagedList.Select(x => _mapper.Map<SimpleStatisticsModel>(x)).ToList(),
                Page = message.Page,
                PageSize = message.PageSize,
                TotalItems = records.Count()
            };
        }

        public async Task<ApiListResponse<SimpleStatisticsModel>> Handle(StatisticiTopSesizariQuery message, CancellationToken token)
        {
            return message.Grupare == StatisticsGroupingTypes.Judet
                ? await GetSesizariJudete(message, token)
                : await GetSesizariSectii(message, token);
        }

        private async Task<ApiListResponse<SimpleStatisticsModel>> GetSesizariJudete(StatisticiTopSesizariQuery message, CancellationToken token)
        {
            var queryBuilder = new StatisticsQueryBuilder
            {
                Query = @"SELECT R.CountyCode AS Label, COUNT(*) as Value
                  FROM Answers AS R 
                  INNER JOIN OptionsToQuestions AS RD ON RD.Id = R.IdOptionToQuestion
                  INNER JOIN Observers O ON O.Id = R.IdObserver
                  INNER JOIN Questions I ON I.Id = RD.IdQuestion
                    inner join FormSections fs on i.IdSection = fs.Id
                    inner join Forms f on fs.IdForm = f.Id
                  WHERE RD.Flagged = 1",
                CacheKey = "StatisticiJudete"
            };

            queryBuilder.AndOngFilter(message.Organizator, message.IdONG);
            queryBuilder.AndFormularFilter(message.Formular);
            queryBuilder.Append("GROUP BY R.CountyCode ORDER BY Value DESC");

            // get or save all records in cache
            var records = await _cacheService.GetOrSaveDataInCacheAsync(queryBuilder.CacheKey,
                async () => await _context.SimpleStatistics
                    .FromSql(queryBuilder.Query)
                    .ToListAsync(),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = new TimeSpan(message.CacheHours, message.CacheMinutes, message.CacheMinutes)
                }
            );

            // perform count and pagination on the records retrieved from the cache 
            var pagedList = records.Paginate(message.Page, message.PageSize);

            return new ApiListResponse<SimpleStatisticsModel>
            {
                Data = pagedList.Select(x => _mapper.Map<SimpleStatisticsModel>(x)).ToList(),
                Page = message.Page,
                PageSize = message.PageSize,
                TotalItems = records.Count()
            };
        }

        private async Task<ApiListResponse<SimpleStatisticsModel>> GetSesizariSectii(StatisticiTopSesizariQuery message, CancellationToken token)
        {
            var queryBuilder = new StatisticsQueryBuilder
            {
                Query = @"SELECT R.CountyCode AS Label, R.PollingStationNumber AS Code, COUNT(*) as Value
                  FROM Answers AS R 
                  INNER JOIN OptionsToQuestions AS RD ON RD.Id = R.IdOptionToQuestion
                  INNER JOIN Observers O ON O.Id = R.IdObserver
                  INNER JOIN Questions I ON I.Id = RD.IdQuestion
                    inner join FormSections fs on i.IdSection = fs.Id
                    inner join Forms f on fs.IdForm = f.Id
                  WHERE RD.Flagged = 1",
                CacheKey = "StatisticiSectii"
            };

            queryBuilder.AndOngFilter(message.Organizator, message.IdONG);
            queryBuilder.AndFormularFilter(message.Formular);
            queryBuilder.Append("GROUP BY R.CountyCode, R.PollingStationNumber");

            // get or save paginated response in cache
            
            return await _cacheService.GetOrSaveDataInCacheAsync($"{queryBuilder.CacheKey}-{message.Page}",
                async () =>
                {
                    var records = await _context.ComposedStatistics
                        .FromSql(queryBuilder.GetPaginatedQuery(message.Page, message.PageSize))
                        .ToListAsync();

                    return new ApiListResponse<SimpleStatisticsModel>
                    {
                        Data = records.Select(x => _mapper.Map<SimpleStatisticsModel>(x)).ToList(),
                        Page = message.Page,
                        PageSize = message.PageSize,
                        TotalItems = await _context.ComposedStatistics.FromSql(queryBuilder.Query).CountAsync()
                    };
                },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = new TimeSpan(message.CacheHours, message.CacheMinutes, message.CacheMinutes)
                }
            );
        }
    }
}
