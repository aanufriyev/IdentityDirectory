namespace Klaims.Framework.IdentityMangement.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;


    public class UserPhone
    {
        public virtual int Id { get; protected internal set; }

        public virtual Guid UserId { get; protected internal set; }

        [StringLength(20)]
        public virtual string Value { get; protected internal set; }

        public virtual string Type { get; protected internal set; }

        public virtual DateTime? Changed { get; protected internal set; }
    }
}