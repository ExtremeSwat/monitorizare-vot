﻿using MediatR;
using Microsoft.AspNetCore.Http;
using VoteMonitor.Api.Core;

namespace VoteMonitor.Api.Core.Commands
{
    public class UploadFileCommand : IRequest<string>
    {
        public IFormFile File { get; set; }
        public UploadType UploadType { get; set; }
    }
}