using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ElevenNote.WebAPI.Controllers
{
    [Authorize]
    public class NoteController : ApiController
    {
        private NoteService CreateNoteService()
        {
            Guid userId = Guid.Parse(User.Identity.GetUserId());
            NoteService noteService = new NoteService(userId);
            return noteService;
        }

        public IHttpActionResult Get()
        {
            NoteService noteService = CreateNoteService();
            IEnumerable<NoteListItem> notes = noteService.GetNotes();
            return Ok(notes);
        }

        public IHttpActionResult Get(int id)
        {
            NoteService noteService = CreateNoteService();
            //get the note by id from note service
            NoteDetail note = noteService.GetNoteById(id);
            return Ok(note);
        }

        public IHttpActionResult Post(NoteCreate note)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            NoteService service = CreateNoteService();

            if(!service.CreateNote(note))
            {
                return InternalServerError(); //500
            }

            return Ok();
        }

        //update
        public IHttpActionResult Put(NoteEdit note)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState); //400
            }

            NoteService service = CreateNoteService();

            if(!service.UpdateNote(note))
            {
                //Update note returns a bool, this runs if false
                return InternalServerError();
            }

            return Ok();
        }
    }
}
