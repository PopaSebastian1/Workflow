﻿namespace Licenta.Server.DataLayer.Models
{
    public class UserSkill
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid SkillId { get; set; }
    }
}
