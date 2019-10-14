using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using VoteMonitor.Entities;
using VoteMonitor.Api.Note.Models;
using VoteMonitor.Api.Note.Queries;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System;

namespace VoteMonitor.Api.Note.Handlers
{
    public class NoteQueryHandler : IRequestHandler<NoteQuery, List<NoteModel>>
    {

        private readonly VoteMonitorContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        public NoteQueryHandler(VoteMonitorContext context, IMapper mapper, IHttpContextAccessor httpContext)
        {
            _context = context;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        public async Task<List<NoteModel>> Handle(NoteQuery message, CancellationToken token)
        {
            var parsed = int.TryParse(_httpContext.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId);
            if (!parsed)
                throw new Exception($"No authentication context !");

            var query = _context.Notes.AsQueryable();

            if (message.IdObserver.HasValue)
                query = query.Where(x => x.IdObserver == message.IdObserver);
            else
                query = query.Where(x => x.IdObserver == userId);

            if (message.IdPollingStation.HasValue)
                query = query.Where(x => x.IdPollingStation == message.IdPollingStation);

            return await query
                .OrderBy(n => n.LastModified)
                .AsNoTracking()
                .Select(n => new NoteModel
                {
                    NoteAttachments = n.NoteAttachments.Select(x => x.NotePath).ToList(),
                    Text = n.Text,
                    FormCode = n.Question.FormSection.Form.Code,
                    QuestionId = n.Question.Id
                })
                .ToListAsync(cancellationToken: token);
        }
    }
}
