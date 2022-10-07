#nullable disable
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TaskTrackerAPI.Data.Models
{
    public class Task
    {
        public int TaskId { get; set; }
        public string Text { get; set; }
        public string Day { get; set; }
        public bool Reminder { get; set; } = false;
    }
}
