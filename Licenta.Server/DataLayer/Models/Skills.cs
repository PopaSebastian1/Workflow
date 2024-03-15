using Licenta.Server.DataLayer.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Licenta.Server.DataLayer.Models
{
    public class Skill
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [EnumDataType(typeof(SkillLevel))]
        public SkillLevel Level { get; set; }
        
        public Skill(string name, SkillLevel level)
        {
            Name = name;
            Level = level;
        }
    }
}
