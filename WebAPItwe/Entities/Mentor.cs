﻿using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPItwe.Entities
{
    public partial class Mentor
    {
        public Mentor()
        {
            MentorMajors = new HashSet<MentorMajor>();
            MentorSessions = new HashSet<MentorSession>();
            MentorSkills = new HashSet<MentorSkill>();
            Sessions = new HashSet<Session>();
        }
        public string Id { get; set; }
        public string Fullname { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public string Sex { get; set; }
        public double Price { get; set; }
        public string Birthday { get; set; }
        public string Status { get; set; }

        public virtual ICollection<MentorMajor> MentorMajors { get; set; }
        public virtual ICollection<MentorSession> MentorSessions { get; set; }
        public virtual ICollection<MentorSkill> MentorSkills { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
