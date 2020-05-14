using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ch3_7.Models
{
    public class GuestbookModels
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Display(Name ="留言內容")]
        public string Content { get; set; }

        [Display(Name ="建立時間")]
        public DateTime? Created { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}