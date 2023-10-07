using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Queries.GetNoteDetails
{
    public class GetNoteDetailsQueryHandler : IRequestHandler<GetNoteDetailsQuery, NoteDetailsVm>
    {
        private readonly INotesDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetNoteDetailsQueryHandler(INotesDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<NoteDetailsVm> Handle(GetNoteDetailsQuery requst, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Notes.
                FirstOrDefaultAsync(note => note.Id == requst.Id, cancellationToken);

            if (entity == null || entity.UserId != requst.UserId)
            {
                throw new NotFoundException(nameof(Note), requst.Id);
            }

            return _mapper.Map<NoteDetailsVm>(entity);
        }
    }
}
