using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Models
{
    public class NoteCreate
    {
        [Required]
        [MinLength(2, ErrorMessage = "Please Enter at least 2 characters.")]
        [MaxLength(100, ErrorMessage = "Too many characters.")]
        public string Title { get; set; }

        [MaxLength(8000)]
        public string Content { get; set; }
    }
}
