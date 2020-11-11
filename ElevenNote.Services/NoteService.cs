using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class NoteService
    {
        private readonly Guid _userId;

        public NoteService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateNote(NoteCreate model)
        {
            var entity =
                new Note()
                {
                    OwnerId = _userId,
                    Title = model.Title,
                    Content = model.Content,
                    CreatedUtc = DateTimeOffset.Now
                };

            using (var context = new ApplicationDbContext())
            {
                context.Notes.Add(entity);
                return context.SaveChanges() == 1;
            }
        }

        public IEnumerable<NoteListItem> GetNotes()
        {
            using (var context = new ApplicationDbContext())
            {
                var query =
                    context
                    .Notes
                    .Where(e => e.OwnerId == _userId)
                    .Select(
                        e =>
                        new NoteListItem
                        {
                            NoteId = e.NoteId,
                            Title = e.Title,
                            CreatedUtc = e.CreatedUtc
                        }
                        );
                return query.ToArray();
            }
        }

        public NoteDetail GetNoteById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var entity =
                    context
                    .Notes
                    .Single(e => e.NoteId == id && e.OwnerId == _userId);

                return new NoteDetail
                {
                    NoteId = entity.NoteId,
                    Title = entity.Title,
                    Content = entity.Content,
                    CreatedUtc = entity.CreatedUtc,
                    ModifiedUtc = entity.ModifiedUtc
                };
            }
        }

        //Update note
        public bool UpdateNote(NoteEdit model)
        {
            using (var context = new ApplicationDbContext())
            {
                var entity =
                    context
                    .Notes
                    .Single(e => e.NoteId == model.NoteId && e.OwnerId == _userId);

                entity.Title = model.Title;
                entity.Content = model.Content;
                entity.ModifiedUtc = DateTimeOffset.Now;

                return context.SaveChanges() == 1;
            }
        }
    }
}
